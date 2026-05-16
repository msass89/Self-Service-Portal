using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelfServiceHub.Models.Entities;
using SelfServiceHub.Services;
using SelfServiceHub.Services.EmailSender;

namespace SelfServiceHub.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {   
        private readonly UserService _userService;
        private readonly TenantService _tenantService;
        private readonly AccountEmailService _accountEmailService;
        private readonly ApplicationDbContext _db;

        public RegisterModel(
            UserService userService,
            TenantService tenantService,
            AccountEmailService accountEmailService,
            ApplicationDbContext db)
        {
            _userService = userService;
            _tenantService = tenantService;
            _accountEmailService = accountEmailService;
            _db = db;
        }


        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [RegularExpression(@"^[a-zA-Z]+ [a-zA-Z]+$", ErrorMessage = "Please enter both first and last name, separated by a space.")]
            [Display(Name = "First and Last Name")]
            public string DisplayName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Company Name")]
            public string TenantName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Use a transaction to ensure that tenant and user creation are atomic operations
                using var transaction = await _db.Database.BeginTransactionAsync();
                
                try
                {
                    // Create tenant and user, and associate the user with the tenant
                    var tenant = CreateTenant();
                    await _tenantService.CreateTenant(tenant);

                    var user = CreateUser(tenant);
                    var result = await _userService.CreateUserAsync(user, Input.Password);

                    // If email is duplicate, add a model error to display a user-friendly message
                    var emailError = result.Errors.FirstOrDefault(e => e.Code == "DuplicateEmail");

                    if (emailError != null)
                    {
                        ModelState.AddModelError(string.Empty, "Email is already used.");
                    }
                    else if (!result.Succeeded) //ensure user creation succeeded before committing transaction
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        throw new Exception(errors);
                    }

                    await transaction.CommitAsync();

                    // Add claims and roles for the user
                    await _userService.AddClaimAsync(user, "DisplayName", user.DisplayName); 
                    await _userService.AddUserToRoleAsync(user, "Admin"); 

                    await _accountEmailService.SendConfirmationEmailAsync(user);

                    // redirect to a page that instructs the user to check their email for the confirmation link
                    return RedirectToPage("/Account/EmailConfirmationSent");
                }
                catch (Exception ex)
                {
                    // If any error occurs during tenant or user creation, roll back the transaction and display an error message
                    ModelState.AddModelError(string.Empty, $"Error creating tenant: {ex.Message}");
                    await transaction.RollbackAsync();
                    return Page();
                }                
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        // helper method to create user with tenant association
        private ApplicationUser CreateUser(Tenant tenant)
        {
            var user = new ApplicationUser
            {
                DisplayName = Input.DisplayName,
                UserName = Input.Email,
                Email = Input.Email,
                TenantId = tenant.Id
            };
            return user;
        }
        
        // helper method to create tenant from input
        private Tenant CreateTenant()
        {
            var tenant = new Tenant
            {
                Name = Input.TenantName
            };
            return tenant;
        }
    }
}

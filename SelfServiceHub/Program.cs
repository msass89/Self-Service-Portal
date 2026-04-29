using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SelfServiceHub.Models.Entities;
using SelfServiceHub.Services;
using SelfServiceHub.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

// configure the MySQL database connection with Entity Framework Core
var version = builder.Configuration["Database:ServerVersion"];

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped<AuthService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IEmailSender, DevEmailSender>();
}
else
{
    //builder.Services.AddScoped<IEmailSender, SendGridEmailSender>();
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(version)
    )
);

// add identity services with ApplicationUser and IdentityRoles,
// configure to use Entity Framework Core for storing user data, 
// and enables token providers for password reset and email confirmation
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;

    // configure account lockout settings to lock out users after 5 failed login attempts for 5 minutes
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
    
builder.Services.AddRazorPages();

var app = builder.Build();

// seed the roles into the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeed.SeedRolesAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();


using Microsoft.AspNetCore.Identity;

namespace SelfServiceHub.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } // Navigation property to link user to tenant
        public string DisplayName { get; set; }
    }
}

namespace SelfServiceHub.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property to link tenants to users
        public ICollection<ApplicationUser> Users { get; set; }
    }
}



using Microsoft.EntityFrameworkCore;
using SelfServiceHub.Models.Entities;

namespace SelfServiceHub.Services
{
    public class TenantService
    {
        public readonly ApplicationDbContext _db;
        public TenantService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Example method to find tenant by ID (this can be expanded as needed)
        public async Task<Tenant?> FindTenantById(string tenantId)
        {
            // In a real application, this would likely query a database or external service
            return await _db.Tenants.FirstOrDefaultAsync(t => t.Id.ToString() == tenantId);
        }

        public async Task<Tenant> FindTenantByName(string tenantName)
        {
            return (await _db.Tenants.FirstOrDefaultAsync(t => t.Name == tenantName)) ?? throw new InvalidOperationException("Tenant not found");
        }

        public async Task<List<ICollection<ApplicationUser>>> GetUsersForTenant(string tenantId)
        {
            var tenant = await FindTenantById(tenantId);
            return tenant != null ? new List<ICollection<ApplicationUser>> { tenant.Users } : new List<ICollection<ApplicationUser>>(); // Return empty list if tenant not found
        }

        public async Task CreateTenant(Tenant tenant)
        {
            _db.Tenants.Add(tenant);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateTenant(Tenant tenant)
        {
            _db.Tenants.Update(tenant);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteTenant(Tenant tenant)
        {
            _db.Tenants.Remove(tenant);
            await _db.SaveChangesAsync();
        }

    }
}
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

        public async Task<Tenant?> FindTenantById(string tenantId)
        {
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

        public async Task<bool> UpdateTenant(Tenant tenant)
        {
            var existing = await _db.Tenants.FirstOrDefaultAsync(t => t.Id.ToString() == tenant.Id.ToString());
            
            if (existing == null)
                return false;

            _db.Entry(existing).CurrentValues.SetValues(tenant);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTenant(string TenantId)
        {
            var tenant = await _db.Tenants.FirstOrDefaultAsync(t => t.Id.ToString() == TenantId);

            if (tenant == null)
                return false;

            _db.Tenants.Remove(tenant);
            await _db.SaveChangesAsync();

            return true;
        }

    }
}
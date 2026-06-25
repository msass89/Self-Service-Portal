using SelfServiceHub.Services;
using SelfServiceHub.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace SelfServiceHub.UnitTests.Services
{
    [TestClass]
    public sealed class TenantService_UnitTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            return new ApplicationDbContext(options);
        }

        [TestMethod]
        public void CreatesTenantService_WhenDbContextIsProvided()
        {
            var inMemoryDbContext = GetInMemoryDbContext();

            var tenantService = new TenantService(inMemoryDbContext);

            Assert.IsNotNull(tenantService);
        }

        [TestMethod]
        public void TenantService_FindTenantById_ReturnTenant_WhenTenantExist()
        {
            var inMemoryDbContext = GetInMemoryDbContext();

            var testTenant = new Tenant { Id = 44, Name = "testTenant" };
            inMemoryDbContext.Tenants.Add(testTenant);
            inMemoryDbContext.SaveChangesAsync();

            var tenantService = new TenantService(inMemoryDbContext);

            var result = tenantService.FindTenantById("44").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(testTenant.Name, result.Name);
        }

        [TestMethod]
        public void TenantService_FindTenantByName_ReturnTenant_WhenTenantExist()
        {
            var inMemoryDbContext = GetInMemoryDbContext();

            var testTenant = new Tenant { Id = 22, Name = "testTenantName" };
            inMemoryDbContext.Tenants.Add(testTenant);
            inMemoryDbContext.SaveChangesAsync();

            var tenantService = new TenantService(inMemoryDbContext);

            var result = tenantService.FindTenantByName("testTenantName").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(testTenant.Name, result.Name);
        }

        [TestMethod]
        public void TenantService_GetUsersForTenant_ReturnUsers_WhenTenantExist()
        {
            var inMemoryDbContext = GetInMemoryDbContext();

            var user1 = new ApplicationUser { Id = "34", UserName = "User1" };
            var user2 = new ApplicationUser { Id = "11", UserName = "User2" };

            var tenant = new Tenant
            {
                Id = 44,
                Name = "TestTenant",
                Users = new List<ApplicationUser> { user1, user2 }
            };
            inMemoryDbContext.Tenants.Add(tenant);
            inMemoryDbContext.SaveChangesAsync();

            var tenantService = new TenantService(inMemoryDbContext);

            var result = tenantService.GetUsersForTenant("44").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);                
            Assert.AreEqual(2, result[0].Count);              
            Assert.IsTrue(result[0].Any(u => u.UserName == "User1"));
            Assert.IsTrue(result[0].Any(u => u.UserName == "User2"));
        }

        [TestMethod]
        public async Task TenantService_CreateTenant_ReturnsTrue_WhenTenantCreated()
        {
            var inMemoryDbContext = GetInMemoryDbContext();

            var tenantService = new TenantService(inMemoryDbContext);

            var testTenant = new Tenant { Id = 28, Name = "tenant" };

            await tenantService.CreateTenant(testTenant);

            var savedTenant = await inMemoryDbContext.Tenants.FindAsync(28);

            Assert.IsNotNull(savedTenant);
            Assert.AreEqual("tenant", savedTenant.Name);
        }

        [TestMethod]
        public async Task TenantService_UpdateTenant_ReturnsTrue_WhenTenantUpdated()
        {
            var inMemoryDbContext = GetInMemoryDbContext();

            var testTenantCreate = new Tenant { Id = 99, Name = "tenant" };
            inMemoryDbContext.Tenants.Add(testTenantCreate);
            await inMemoryDbContext.SaveChangesAsync();

            var tenantService = new TenantService(inMemoryDbContext);

            var testTenantUpdate = new Tenant { Id = 99, Name = "updatedTenantName" };
            var result = await tenantService.UpdateTenant(testTenantUpdate);

            var updatedTenant = await inMemoryDbContext.Tenants.FindAsync(99);

            Assert.IsNotNull(updatedTenant);
            Assert.AreEqual("updatedTenantName", updatedTenant.Name);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task TenantService_DeleteTenant_ReturnsTrue_WhenTenantNotExists()
        {
            var inMemoryDbContext = GetInMemoryDbContext();

            var testTenantCreate = new Tenant { Id = 5, Name = "tenant" };
            inMemoryDbContext.Tenants.Add(testTenantCreate);
            await inMemoryDbContext.SaveChangesAsync();

            var tenantService = new TenantService(inMemoryDbContext);

            var result = await tenantService.DeleteTenant("5");

            var deletedTenant = await inMemoryDbContext.Tenants.FindAsync(5);

            Assert.IsNull(deletedTenant);
            Assert.IsTrue(result);
        }
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SelfServiceHub.Models.Entities;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    //
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // important for Identity!

    }

    // creates table for tenants
    public DbSet<Tenant> Tenants { get; set; }

}
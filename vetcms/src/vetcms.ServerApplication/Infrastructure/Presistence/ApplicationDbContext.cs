using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Domain.Entity.MedicalPillManagement;

//Migration: WebApi appból kell indítani.
//    PM > add - migration PrepareUserModelToComplyWithSRS -Project vetcms.ServerApplication
//    PM> Update-database -Project vetcms.ServerApplication
[assembly: InternalsVisibleTo("vetcms.ServerApplicationTests")]
namespace vetcms.ServerApplication.Infrastructure.Presistence
{
    public class ApplicationDbContext : DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<MedicalPill> MedicalPills { get; set; }
        DbSet<MedicalPillStock> MedicalPillStock { get; set; }
        DbSet<MedicalPillStockAlert> MedicalPillStockAlerts { get; set; }
        DbSet<MedicalPillUsageLog> MedicalPillUsageLogs { get; set; }
        DbSet<FirstTimeAuthenticationCode> FirstTimeAuthenticationCodes { get; set; }
        DbSet<SentEmail> SentEmails { get; set; }

        public ApplicationDbContext()
        {
            
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
            : base(contextOptions)
        {

        }

        //public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        //{
        //    foreach (var entry in ChangeTracker.Entries<AuditedEntity>())
        //    {
        //        switch (entry.State)
        //        {
        //            case EntityState.Added:
        //                entry.Entity.CreatedByUserId = 0;
        //                entry.Entity.Created = DateTime.Now;
        //                break;
        //            case EntityState.Modified:
        //                entry.Entity.LastModifiedByUserId = 0;
        //                entry.Entity.LastModified = DateTime.Now;
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    var result = await base.SaveChangesAsync(cancellationToken);
        //    return result;
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}

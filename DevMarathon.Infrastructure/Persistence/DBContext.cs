using System.Text.RegularExpressions;
using DevMarathon.Domain.Entities.Mongo;
using DevMarathon.Domain.Entities.SQL;
using DevMarathon.Infrastructure.EntityConfigurations;
using DevMarathon.Infrastructure.SQLRepositories;
using DevMarathon.Utility;
using Microsoft.EntityFrameworkCore;


namespace DevMarathon.Infrastructure.Persistence;

public class CleanContext:DbContext
{
    public CleanContext(DbContextOptions<CleanContext> options):base(options) {  }
    public DbSet<UserEntity> Users { get; set; }
    
    

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var now = DateTime.Now;
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.IsDeleted = false;
                    entry.Entity.IsEnable = true;
                    entry.Entity.CreatedAt = now;
                    entry.Entity.JalaliCreatedAt = now.ToFa("yyyy/MM/dd");
                    entry.Entity.JalaliDateKey = int.Parse(now.ToFa("yyyyMMdd"));
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifyDate = now;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
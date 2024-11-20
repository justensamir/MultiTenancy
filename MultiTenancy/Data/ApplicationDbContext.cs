using Microsoft.EntityFrameworkCore;

namespace MultiTenancy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public string TenantId { get; set; }
        private readonly ITenantService _tenantService;
        public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
            TenantId = _tenantService.GetCurrentTenant()?.TId;
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Global Filter will applied on all quires of the product entity filter the products by tenant
            modelBuilder.Entity<Product>().HasQueryFilter(e => e.TenantId == TenantId);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();
            if (!string.IsNullOrWhiteSpace(tenantConnectionString) || !string.IsNullOrEmpty(tenantConnectionString))
            {
                string dbProvider = _tenantService.GetDBProvider();
                if (dbProvider?.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(tenantConnectionString);
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Add TenantId foreach entity that implement IMustHaveTenant interface
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().Where(e => e.State == EntityState.Added))
            {
                entry.Entity.TenantId = TenantId;
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}

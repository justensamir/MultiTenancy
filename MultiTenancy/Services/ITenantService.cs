namespace MultiTenancy.Services
{
    public interface ITenantService
    {
        string? GetDBProvider();
        string? GetConnectionString();
        Tenant? GetCurrentTenant();
    }
}

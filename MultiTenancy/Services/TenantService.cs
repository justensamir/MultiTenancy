
using Microsoft.Extensions.Options;

namespace MultiTenancy.Services
{
    public class TenantService : ITenantService
    {
        private Tenant? _currentTenant;
        private TenantSettings _tenantSettings;
        private HttpContext? _httpContext;

        public TenantService(IHttpContextAccessor contextAccessor, IOptions<TenantSettings> tenantSettings)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContext = contextAccessor.HttpContext;

            if (_httpContext is not null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetCurrentTenant(tenantId!);
                }
                else
                {
                    throw new Exception("No Tenant Provided!");
                }
            }
        }

        public string? GetConnectionString()
        {
            string? currentConnectionString = _currentTenant is null ?
                _tenantSettings.Defaults.ConnectionString :
                _currentTenant.ConnectionString;

            return currentConnectionString;
        }

        public Tenant? GetCurrentTenant()
        {
            return _currentTenant;
        }

        public string? GetDBProvider()
        {
            return _tenantSettings.Defaults?.DBProvider;
        }

        private void SetCurrentTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.TId == tenantId);

            if (_currentTenant is null)
            {
                throw new Exception($"No Tenant match this id {tenantId}");
            }

            if (string.IsNullOrEmpty(_currentTenant.ConnectionString) || string.IsNullOrWhiteSpace(_currentTenant.ConnectionString))
            {
                _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
            }
        }
    }
}

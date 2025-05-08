using Domain.Interfaces;
namespace Business.Services
{ 
    public class TenantProvider : ITenantProvider
    {
        private string? _tenantId;
        private string? _tenantBase;
        public string? ConsultarTenantID() => _tenantId;
        public string? ConsultarTenantBase() => _tenantBase;
        public void SetarTenantID(string tenantId) => _tenantId = tenantId;
        public void SetarNomeBase(string tenantBase) => _tenantBase = tenantBase;
    }
}
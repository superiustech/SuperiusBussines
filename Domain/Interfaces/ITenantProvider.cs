namespace Domain.Interfaces
{
    public interface ITenantProvider
    {
        string? ConsultarTenantID();
        string? ConsultarTenantBase();
        void SetarTenantID(string tenantId);
        void SetarNomeBase(string tenantId);
    }
}
// Domain/Interfaces/IRuntimeDbContextFactory.cs
namespace Domain.Interfaces
{
    public interface IRuntimeDbContextFactory<TDbContext> where TDbContext : class
    {
        TDbContext CreateDbContext(string tenantId);
    }
}
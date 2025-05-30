using Domain.Interfaces;
using Infra;
public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;
    public DashboardRepository(ApplicationDbContext context)
    {
        _context = context;
    }
}

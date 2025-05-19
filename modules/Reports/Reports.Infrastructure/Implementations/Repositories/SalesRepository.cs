using System.Linq.Expressions;
using Infrastructure.Implementations.Persistence.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Reports.Domain.Modules.Entities;
using Reports.Domain.Modules.Repositories;
using Reports.Infrastructure.Implementations.Persistence.EFCore;

namespace Reports.Infrastructure.Implementations.Repositories;

public class SalesRepository(ReportDbContext Context)
    : Repository<SalesReport>(Context), ISalesRepository
{
    public new async Task<IEnumerable<SalesReport>> GetAllAsync(params Expression<Func<SalesReport, object>>[] includes)
    {
        return await Context.SalesReports
            .Include(x => x.SalesDetails)
            .Include(x => x.Customer).ToListAsync();

    }
}

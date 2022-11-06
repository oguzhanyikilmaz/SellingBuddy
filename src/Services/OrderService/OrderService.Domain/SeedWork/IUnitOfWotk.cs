using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Domain.SeedWork
{
    public interface IUnitOfWotk:IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken=default(CancellationToken));
        Task<int> SaveEntitiesAsync(CancellationToken cancellationToken=default(CancellationToken));
    }
}

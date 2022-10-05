using E_CommercialAPI.Application.Repositories;
using E_CommercialAPI.Domain.Entities.Common;
using E_CommercialAPI.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Persistance.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ECommercialAPIDbContext _context;

        public ReadRepository(ECommercialAPIDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            return tracking ? query : query.AsNoTracking();
        }

        public async Task<T> GetByIdAsync(string id, bool tracking = true) //=> await Table.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
        {
            var query = Table.AsQueryable();
            return tracking ? await Table.FindAsync(Guid.Parse(id)) : await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            return tracking ? await query.FirstOrDefaultAsync(method) : await query.AsNoTracking().FirstOrDefaultAsync(method);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true) 
        {
            var query = Table.Where(method);
            return tracking ? query : query.AsNoTracking();
        }
    }
}

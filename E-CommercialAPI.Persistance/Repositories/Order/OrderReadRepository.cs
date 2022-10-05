using E_CommercialAPI.Application.Repositories;
using E_CommercialAPI.Domain.Entities;
using E_CommercialAPI.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Persistance.Repositories
{
    public class OrderReadRepository : ReadRepository<Order>, IOrderReadRepository
    {
        public OrderReadRepository(ECommercialAPIDbContext context) : base(context)
        {
        }
    }
}

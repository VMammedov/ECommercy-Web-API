using E_CommercialAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Repositories
{
    public interface IOrderWriteRepository : IWriteRepository<Order>
    {
    }
}

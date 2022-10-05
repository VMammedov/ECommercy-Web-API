using E_CommercialAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public ICollection<Order> Orders { get; set; }
    }
}

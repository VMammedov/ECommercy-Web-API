using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Way = E_CommercialAPI.Domain.Entities;

namespace E_CommercialAPI.Application.Repositories
{
    public interface IFileWriteRepository : IWriteRepository<Way.File> // Way::File
    {
    }
}

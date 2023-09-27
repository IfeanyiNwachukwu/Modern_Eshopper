using Catalogue.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Core.Repositories
{
    public interface ITypesRepository
    {
        Task<IEnumerable<ProductType>> GetAllTypes();
    }
}

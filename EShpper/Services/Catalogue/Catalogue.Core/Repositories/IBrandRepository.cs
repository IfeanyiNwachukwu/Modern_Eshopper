using Catalogue.Core.Entities;

namespace Catalogue.Core.Repositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<ProductBrand>> GetAllBrands();
    }
}

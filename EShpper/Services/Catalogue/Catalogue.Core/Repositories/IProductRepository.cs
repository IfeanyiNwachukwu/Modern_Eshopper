using Catalogue.Core.Entities;
using Catalogue.Core.Specs;

namespace Catalogue.Core.Repositories
{
    public interface IProductRepository
    {

        //Task<IEnumerable<Product>> GetProducts();
        Task<Pagination<Product>> GetProducts(CatalogueSpecParams catalogueSpecParams);
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<IEnumerable<Product>> GetProductByBrand(string name);
        Task<Product> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
    }
}

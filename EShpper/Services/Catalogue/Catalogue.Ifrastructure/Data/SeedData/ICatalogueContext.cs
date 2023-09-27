using Catalogue.Core.Entities;
using MongoDB.Driver;

namespace Catalogue.Infrastructure.Data.SeedData
{
    public interface ICatalogueContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<ProductBrand> Brands { get; }
        IMongoCollection<ProductType> Types { get; }
    }
}

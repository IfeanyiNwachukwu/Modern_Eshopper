using Catalogue.Core.Entities;
using Catalogue.Core.Repositories;
using Catalogue.Core.Specs;
using Catalogue.Infrastructure.Data.SeedData;
using MongoDB.Driver;

namespace Catalogue.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
    {
        private readonly ICatalogueContext _context;
        public ProductRepository(ICatalogueContext catalogue)
        {
            _context = catalogue;
        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;

        }

        public async Task<bool> DeleteProduct(string id)
        {

            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged & deleteResult.DeletedCount > 0;

        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _context.Brands.Find(b => true).ToListAsync();
        }

        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _context.Types.Find(t => true).ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByBrand(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, name);

            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context.Products.Find(filter).ToListAsync();
        }

        //public async Task<IEnumerable<Product>> GetProducts()
        //{
        //    return await _context.Products.Find(p => true).ToListAsync();
        //}
        public async Task<Pagination<Product>> GetProducts(CatalogueSpecParams catalogueSpecParams)
        {
            var builder = Builders<Product>.Filter;

            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(catalogueSpecParams.Search))
            {
                var searchFilter = builder.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(catalogueSpecParams.Search));

                filter &= searchFilter;
            }

            if (!string.IsNullOrEmpty(catalogueSpecParams.BrandId))
            {
                var brandFilter = builder.Eq(x => x.Brands.Id, catalogueSpecParams.BrandId); 

                filter &= brandFilter;
            }

            if (!string.IsNullOrEmpty(catalogueSpecParams.TypeId))
            {
                var typeFilter = builder.Eq(x => x.Brands.Id, catalogueSpecParams.TypeId);

                filter &= typeFilter;
            }

            if (!string.IsNullOrEmpty(catalogueSpecParams.Sort))
            {
                return new Pagination<Product>
                {
                    PageSize = catalogueSpecParams.PageSize,
                    PageIndex = catalogueSpecParams.PageIndex,
                    Data = await DataFilter(catalogueSpecParams, filter),
                    Count = await _context.Products.CountDocumentsAsync(p =>
                        true) //TODO: Need to check while applying with UI
                };
            }

            return new Pagination<Product>()
            {
                PageSize = catalogueSpecParams.PageSize,
                PageIndex = catalogueSpecParams.PageIndex,
                Data = await _context
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Name"))
                    .Skip(catalogueSpecParams.PageSize * (catalogueSpecParams.PageIndex - 1))
                    .Limit(catalogueSpecParams.PageSize)
                    .ToListAsync(),
                Count = await _context.Products.CountDocumentsAsync(p => true)
            };
        }
        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        private async Task<IReadOnlyList<Product>> DataFilter(CatalogueSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            switch (catalogSpecParams.Sort)
            {
                case "priceAsc":
                    return await _context
                        .Products
                        .Find(filter)
                        .Sort(Builders<Product>.Sort.Ascending("Price"))
                        .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                        .Limit(catalogSpecParams.PageSize)
                        .ToListAsync();
                case "priceDesc":
                    return await _context
                        .Products
                        .Find(filter)
                        .Sort(Builders<Product>.Sort.Descending("Price"))
                        .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                        .Limit(catalogSpecParams.PageSize)
                        .ToListAsync();
                default:
                    return await _context
                        .Products
                        .Find(filter)
                        .Sort(Builders<Product>.Sort.Ascending("Name"))
                        .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                        .Limit(catalogSpecParams.PageSize)
                        .ToListAsync();
            }
        }
    }
}

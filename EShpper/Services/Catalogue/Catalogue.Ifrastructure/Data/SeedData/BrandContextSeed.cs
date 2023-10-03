using Catalogue.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Data.SeedData
{
    public static class BrandContextSeed
    {
        public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
        {
            //Check if collection has alread been seeded
            bool checkBrands = brandCollection.Find(b => true).Any();
            var filePath = @"C:\Users\USER\source\repos\Modern_Eshopper\EShpper\Services\Catalogue\Catalogue.Ifrastructure\Data\SeedData";
            string path = Path.Combine(filePath, "brands.json");

            if (!checkBrands)
            {
                var brandsData = File.ReadAllText(path);
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands != null)
                {
                    foreach (var item in brands)
                    {
                        brandCollection.InsertOneAsync(item);
                    }
                }
            }


        }
    }
}

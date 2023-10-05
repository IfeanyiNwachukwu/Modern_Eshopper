using Catalogue.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalogue.Infrastructure.Data.SeedData
{
    public static class BrandContextSeed
    {
        public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
        {
            //Check if collection has alread been seeded
            bool checkBrands = brandCollection.Find(b => true).Any();
           
            //This will give us the full name path of the executable file:
             string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            
            //This will strip just the working path name:
            string strWorkPath = Path.GetDirectoryName(strExeFilePath);

         


            string path = Path.Combine(strWorkPath, "Data", "SeedData", "brands.json");

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

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
    public class CatalogueContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool checkProducts = productCollection.Find(b => true).Any();
          
            //This will give us the full name path of the executable file:
            //i.e. C:\Program Files\MyApplication\MyApplication.exe
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //This will strip just the working path name:
            //C:\Program Files\MyApplication
            string strWorkPath = Path.GetDirectoryName(strExeFilePath);

            string path = Path.Combine(strWorkPath, "Data", "SeedData", "products.json");



            if (!checkProducts)
            {
                var productsData = File.ReadAllText(path);
                var brands = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (brands != null)
                {
                    foreach (var item in brands)
                    {
                        productCollection.InsertOneAsync(item);
                    }
                }
            }


        }
    }
}

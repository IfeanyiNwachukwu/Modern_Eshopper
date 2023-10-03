using Catalogue.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalogue.Infrastructure.Data.SeedData
{
    public class TypeContextSeed
    {
        public static void SeedData(IMongoCollection<ProductType> typeCollection)
        {
            bool checkTypes = typeCollection.Find(b => true).Any();
            var filePath = @"C:\Users\USER\source\repos\Modern_Eshopper\EShpper\Services\Catalogue\Catalogue.Ifrastructure\Data\SeedData";
            string path = Path.Combine(filePath, "types.json");

            if (!checkTypes)
            {
                var typesData = File.ReadAllText(path);
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                if (types != null)
                {
                    foreach (var item in types)
                    {
                        typeCollection.InsertOneAsync(item);
                    }
                }
            }


        }
    }
}

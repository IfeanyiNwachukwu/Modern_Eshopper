﻿using Catalogue.Core.Entities;
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
            string path = Path.Combine("Data", "SeedData", "products.json");

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
using ESourcing.Products.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ESourcing.Products.Data
{
    public class ProductContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool isExist = productCollection.Find(p => true).Any();
            if (!isExist)
            {
                productCollection.InsertManyAsync(GetConfigureProducts());
            }
        }

        private static IEnumerable<Product> GetConfigureProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name="Huawei Plus",
                    Summary="Summary...",
                    Description ="Description...",
                    ImageFile ="product-3.png",
                    Price=960.00M,
                    Category="White Appliances"
                },
                new Product()
                {
                    Name = "iPhone 13",
                    Summary = "Summary...",
                    Description = "Description...",
                    ImageFile = "iphone-13.png",
                    Price = 1099.99M,
                    Category = "Electronics"
                },
                new Product()
                {
                    Name = "Adidas Superstar Ayakkabı",
                    Summary = "Summary...",
                    Description = "Description...",
                    ImageFile = "superstar-shoes.png",
                    Price = 129.99M,
                    Category = "Footwear"
                },
                new Product()
                {
                    Name = "Samsung 4K Ultra HD TV",
                    Summary = "Summary...",
                    Description = "Description...",
                    ImageFile = "samsung-tv.png",
                    Price = 799.00M,
                    Category = "Electronics"
                },
                new Product()
                {
                    Name = "Levi's 501 Kot Pantolon",
                    Summary = "Summary...",
                    Description = "Description...",
                    ImageFile = "levis-501-jeans.png",
                    Price = 79.99M,
                    Category = "Apparel"
                },
                new Product()
                {
                    Name = "Bose Noise-Cancelling Kulaklık",
                    Summary = "Summary...",
                    Description = "Description...",
                    ImageFile = "bose-headphones.png",
                    Price = 299.99M,
                    Category = "Electronics"
                },
                new Product()
                {
                    Name = "Yeti Rambler Termos",
                    Summary = "Summary...",
                    Description = "Description...",
                    ImageFile = "yeti-thermos.png",
                    Price = 39.99M,
                    Category = "Outdoor Gear"
                }
            };
        }
    }
}

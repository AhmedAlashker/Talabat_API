using System.Text.Json;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;
using Talabat.Infrastructure._Data;

namespace Talabat.Infrastructure._Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext _dbContext)
        {
            #region Product Brand Seeding Data

            if (_dbContext.ProductBrands.Count() == 0)
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/_Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);


                if (brands?.Count() > 0)
                {
                    foreach (var brand in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(brand);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            #endregion


            #region Porduct Category Seeding Data


            if (_dbContext.ProductCategories.Count() == 0)
            {
                var categoriesData = File.ReadAllText("../Talabat.Repository/_Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);


                if (categories?.Count() > 0)
                {
                    foreach (var category in categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(category);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }


            #endregion


            #region Product Seeding Data

            if (_dbContext.Products.Count() == 0)
            {
                var productsData = File.ReadAllText("../Talabat.Repository/_Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);


                if (products?.Count() > 0)
                {
                    foreach (var product in products)
                    {
                        _dbContext.Set<Product>().Add(product);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            #endregion


            if (_dbContext.DeliveryMethods.Count() == 0)
            {
                var deliverMethodsData = File.ReadAllText("../Talabat.Repository/_Data/DataSeed/delivery.json");
                var deliverMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverMethodsData);


                if (deliverMethods?.Count() > 0)
                {
                    foreach (var deliverMethod in deliverMethods)
                    {
                        _dbContext.Set<DeliveryMethod>().Add(deliverMethod);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }



        }
    }
}

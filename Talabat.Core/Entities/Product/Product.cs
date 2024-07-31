using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Product
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        //[ForeignKey(nameof(Product.Brand))]
        public int BrandId { get; set; }    // Foregin Key Column => ProductBrand
        //[InverseProperty(nameof(ProductBrand.Products))]
        public ProductBrand Brand { get; set; } // Navigational Property ONE

        public int CategoryId { get; set; } // Foregin Key Column => ProductCategory
        public ProductCategory Category { get; set; } // Navigational Property ONE



    }
}

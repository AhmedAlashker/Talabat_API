using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTO
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string PictureUrl { get; set; } = null!;
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than Zero.")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be 1 Item at least.")]
        public int Quantity { get; set; }
        [Required]
        public string Category { get; set; } = null!;
        [Required]
        public string Brand { get; set; } = null!;
    }
}
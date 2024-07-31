using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.DTO
{
    public class OrderDto
    {
        //[Required]
        //public string BuyerEmail { get; set; } = null!;
        [Required]
        public string BasketId { get; set; } = null!;
        [Required]
        public int DeliveryMethodId { get; set; }
        public OrderAddressDto ShippingAddress { get; set; } = null!;


    }
}

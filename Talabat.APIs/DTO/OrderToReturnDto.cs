using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.DTO
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }

        public string BuyerEmail { get; set; } = null!;

        public DateTimeOffset OrderDate { get; set; }

        public string Status { get; set; }

        public Address ShippingAddress { get; set; } = null!;

        public string DeliveryMethod { get; set; } = null!; // Navigational Property  [ONE]
        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>(); 

        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }


        //[NotMapped]
        //public decimal Total => Subtotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; } = string.Empty;
    }
}

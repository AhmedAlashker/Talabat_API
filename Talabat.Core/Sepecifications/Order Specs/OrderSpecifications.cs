using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Sepecifications.Order_Specs
{
    public class OrderSpecifications : BaseSepecifications<Order>
    {
        public OrderSpecifications(string buyerEmail)
            :base(O => O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            AddOrderByDesc(O => O.OrderDate);
        }

        public OrderSpecifications(int orderID, string buyerEmail)
            :base(O => O.Id == orderID && O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }

    }
}

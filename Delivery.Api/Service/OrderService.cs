using Delivery.Api.Models;

namespace Delivery.Api.Service
{
    public class OrderService
    {
        private static readonly List<Order> Orders = [];

        public List<Order> GetAll()
        {
            return Orders;
        }

        public Order Create(Order order)
        {
            Orders.Add(order);
            return order;
        }
    }
}

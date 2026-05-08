namespace Delivery.Api.Models
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CustomerName { get; set; } = "";

        public string Product { get; set; } = "";

        public decimal Amount { get; set; }

        public string Status { get; set; } = "Pending";
    }
}

namespace MyFirstApp.Models
{
    public class OrderItemModel // Update the class name here
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public OrderModel Order { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
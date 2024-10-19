using System.ComponentModel.DataAnnotations;

namespace MyFirstApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ICollection<OrderItemodel> OrderItems { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal Total { get; set; }

        public virtual UserModel Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public virtual Order Order { get; set; }
        public virtual Item Item { get; set; }
    }
}
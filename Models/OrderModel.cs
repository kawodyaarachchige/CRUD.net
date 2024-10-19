namespace MyFirstApp.Models;

public class OrderModel
{

        public int Id { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItemModel> OrderItems { get; set; }

}
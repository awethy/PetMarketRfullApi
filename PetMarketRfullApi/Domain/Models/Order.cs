namespace PetMarketRfullApi.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quanity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

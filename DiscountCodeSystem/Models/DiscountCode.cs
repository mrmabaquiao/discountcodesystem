namespace DiscountCodeSystem.Models
{
    public class DiscountCode
    {
        public int Id { get; set; }
        public string Code  { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRedeemed { get; set; } = false;
      
    }
}

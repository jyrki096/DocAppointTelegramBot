namespace IRON_PROGRAMMER_BOT_Common.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
    }
}

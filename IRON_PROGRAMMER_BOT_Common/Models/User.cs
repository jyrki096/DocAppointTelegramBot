namespace IRON_PROGRAMMER_BOT_Common.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public List<Review> Reviews { get; set; } = new();
        public List<Appoint> Appoints { get; set; } = new();
    }
}

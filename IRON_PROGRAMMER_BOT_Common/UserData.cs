using IRON_PROGRAMMER_BOT_Common.Models;
using IRON_PROGRAMMER_BOT_Common.User.Pages;

namespace IRON_PROGRAMMER_BOT_Common
{
    public class UserData
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string SentMessage { get; set; }
        public Message? LastMessage { get; set; }
        public bool isAuthorized { get; set; } = false;
        public string selectedDocType { get; set; }
        public string selectedDocName { get; set; }

        public bool isAllTimeBooked = false;
        public Appoint? AppointRegistration { get; set; }

        public override string ToString()
        {
            return $"UserId={PhoneNumber}";
        }
    }
}

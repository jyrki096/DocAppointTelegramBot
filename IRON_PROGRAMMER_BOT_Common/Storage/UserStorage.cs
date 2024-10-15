namespace IRON_PROGRAMMER_BOT_Common.Storage
{
    public class UserStorage(ApplicationContext database)
    {
        public bool Exists(string phone) => database.Users.FirstOrDefault(user => user.PhoneNumber.Contains(phone.Substring(1))) != null;

        public Models.User? GetUser(string phone, string password) => database.Users.FirstOrDefault(user => user.PhoneNumber == phone && user.Password == password);

        public void SaveUser(Models.User user)
        {
            database.Users.Add(user);
            database.SaveChanges();
        }

        public void DeleteUser(string phone)
        {
            var user = database.Users.FirstOrDefault(x => x.PhoneNumber == phone);
            if (user is not null)
            {
                database.Users.Remove(user);
                database.SaveChanges();
            }          
        }
    }
}

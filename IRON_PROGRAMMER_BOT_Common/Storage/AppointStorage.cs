using IRON_PROGRAMMER_BOT_Common.Models;

namespace IRON_PROGRAMMER_BOT_Common.Storage
{
    public class AppointStorage(ApplicationContext database)
    {
        public List<Appoint> GetAppoints(int userId) => database.Appoints.Where(x => x.UserId == userId).ToList();

        public void AddAppoint(Appoint appoint)
        {
            database.Appoints.Add(appoint);
            database.SaveChanges();
        }

        public void RemoveLast()
        {
            var lastAppoint = database.Appoints.OrderBy(x => x.Id).Last();
            if (lastAppoint != null)
            {
                database.Appoints.Remove(lastAppoint);
                database.SaveChanges();
            }
        }
    }
}

using IRON_PROGRAMMER_BOT_Common.Models;
using Microsoft.EntityFrameworkCore;

namespace IRON_PROGRAMMER_BOT_Common.Storage
{
    public class ReviewStorage(ApplicationContext database)
    {
        public List<Review> GetReviews() => database.Reviews.Include(x => x.User).ToList();

        public void SaveReview(Review review)
        {
            database.Reviews.AddAsync(review);
            database.SaveChanges();
        }
    }
}

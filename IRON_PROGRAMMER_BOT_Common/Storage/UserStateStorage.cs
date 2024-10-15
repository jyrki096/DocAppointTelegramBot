using System.Collections.Concurrent;

namespace IRON_PROGRAMMER_BOT_Common.Storage
{
    public class UserStateStorage
    {
        private readonly ConcurrentDictionary<long, UserState> cache = new ConcurrentDictionary<long, UserState>();

        public void AddOrUpdate(long userId, UserState userState) => cache.AddOrUpdate(userId, userState, (x, y) => userState);

        public UserState TryGet(long userId)
        {
            cache.TryGetValue(userId, out UserState? userState);
            return userState;
        }
    }
}

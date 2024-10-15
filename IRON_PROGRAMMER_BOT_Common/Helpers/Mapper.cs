namespace IRON_PROGRAMMER_BOT_Common.Helpers
{
    public class Mapper
    {
        public static void AuthorizeUser(UserState userState, Models.User user)
        {
            userState.UserData.Id = user.Id;
            userState.UserData.isAuthorized = true;
            userState.UserData.PhoneNumber = user.PhoneNumber;
            userState.UserData.Name = user.Name;
        }
    }
}

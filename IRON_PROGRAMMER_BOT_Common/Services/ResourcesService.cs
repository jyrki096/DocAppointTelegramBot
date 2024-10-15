using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_ConsoleApp.Services
{
    public class ResourcesService
    {
        public InputFileStream GetResource(byte[] buffer, string filename = "filename")
        {
            var memoryStream = new MemoryStream(buffer);
            return InputFile.FromStream(memoryStream, filename);
        }
    }
}

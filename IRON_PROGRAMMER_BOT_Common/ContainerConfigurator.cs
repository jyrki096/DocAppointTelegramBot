using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Microsoft.Extensions.Options;
using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.Configuration;
using IRON_PROGRAMMER_BOT_Common.User.Pages;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using IRON_PROGRAMMER_BOT_ConsoleApp.Services;


namespace IRON_PROGRAMMER_BOT_Common
{
    public class ContainerConfigurator
    {
        public static void Configure(IConfiguration configuration, IServiceCollection services)
        {

            var botConfigurationSection = configuration.GetSection(BotConfiguration.SectionName);
            var connection = "Server=(localdb)\\MSSQLLocalDB;Database=tg_bot_doc_appoint;Trusted_Connection=True;";

            services.Configure<BotConfiguration>(botConfigurationSection);
            services.AddSingleton<ResourcesService>();
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            services.AddSingleton<UserStateStorage>();
            services.AddSingleton<AppointStorage>();
            services.AddSingleton<ReviewStorage>();
            services.AddSingleton<UserStorage>();
            services.AddHttpClient("tg_bot_client").AddTypedClient<ITelegramBotClient>((httpClient, services) =>
            {
                var botConfig = services.GetService<IOptions<BotConfiguration>>()!.Value;
                var options = new TelegramBotClientOptions(botConfig.BotToken);
                return new TelegramBotClient(options, httpClient);
            });

            services.AddSingleton<IUpdateHandler, UpdateHandler>();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes().Where(t => typeof(IPage).IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var type in types)
            {
                services.AddScoped(type);
            }
        }
    }
}

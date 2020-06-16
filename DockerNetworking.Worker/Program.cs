using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DockerNetworking.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var clientOptions = hostContext.Configuration
                        .GetSection(nameof(ClientOptions))
                        .Get<ClientOptions>();
                    services.AddSingleton(clientOptions);
                    services.AddHttpClient();
                    services.AddHostedService<Worker>();
                });
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DockerNetworking.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ClientOptions clientOptions;
        private readonly IHostEnvironment environment;
        private readonly ILogger<Worker> logger;

        public Worker(IHttpClientFactory clientFactory, ClientOptions clientOptions, 
            IHostEnvironment environment, ILogger<Worker> logger)
        {
            this.clientFactory = clientFactory;
            this.clientOptions = clientOptions;
            this.environment = environment;
            this.logger = logger;
            logger.LogInformation($"Host Environment: {environment.EnvironmentName}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation($"ApiHostName: {clientOptions.ApiHostName}");

                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"http://{clientOptions.ApiHostName}/weatherforecast");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "DockerNetworking.Worker");

                var client = clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var weathers = (await JsonSerializer
                        .DeserializeAsync<IEnumerable<WeatherForecast>>(responseStream, 
                            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }))
                        .ToArray();

                    var weather = weathers[new Random().Next(1, weathers.Length)];
                    logger.LogInformation("Weather at {time}: {temp} degrees C {weather}", 
                        DateTime.Now, weather.TemperatureC, weather.Summary);
                }

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}

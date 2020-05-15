using System.Threading;
using System.Threading.Tasks;
using MassTransit.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RSMassTransit
{
    internal static class Program
    {
        internal static Task Main(string[] args)
        {
            // .NET Host Builder Documentation
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1
            // 
            // The default host builder:
            // - Sets the content root to the path returned by GetCurrentDirectory
            // - Loads host configuration from:
            //     - Environment variables prefixed with DOTNET_
            //     - Command-line arguments
            // - Loads app configuration from:
            //     - appsettings.json
            //     - appsettings.{Environment}.json
            //     - Secret Manager when the app runs in the Development environment
            //     - Environment variables
            //     - Command-line arguments
            // - Adds the following logging providers:
            //     - Console
            //     - Debug
            //     - EventSource
            //     - EventLog (only when running on Windows)
            // - Enables scope validation and dependency validation when the environment is Development.

            return Host
                .CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices)
                .Build()
                .RunAsync();
        }

        private static void ConfigureServices(
            HostBuilderContext context,
            IServiceCollection services)
        {
            services.AddHostedService<SomeService>();
        }

        internal class SomeService : BackgroundService
        {
            public SomeService(ILogger<SomeService>? logger)
            {
                Logger = logger as ILogger ?? NullLogger.Instance;
            }

            private ILogger Logger { get; }

            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                Logger.LogInformation("Hi");

                return Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
    }
}

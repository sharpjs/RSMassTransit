using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddRSMassTransit(context.Configuration);
        }
    }
}

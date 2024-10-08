// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Subatomix.Logging.Console;
using Subatomix.Logging.Debugger;

namespace RSMassTransit.Service;

internal static class Program
{
    private static Task Main(string[] args)
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

        // .NET Windows Service Documentation
        // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-3.1&tabs=visual-studio
        //
        // The UseWindowsService method:
        // - Sets the host lifetime to WindowsServiceLifetime
        // - Sets the content root to AppContext.BaseDirectory
        // - Enables logging to the event log:
        //   - The default log level is Warning; override with Logging:EventLog:LogLevel:Default
        //   - The default source name is the application name
        //     | NOTE: Only administrators can create new event sources.  If the logger fails
        //     | to create the event source, the logger writes a warning to the Application
        //     | event source and disables itself.

        return Host
            .CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureHostConfiguration(ConfigureHostConfiguration)
            .ConfigureServices(ConfigureServices)
            .Build()
            .RunAsync();
    }

    private static void ConfigureHostConfiguration(IConfigurationBuilder builder)
    {
        builder.AddJsonFile("hostsettings.json");
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddLogging(ConfigureLogging);
        services.AddRSMassTransit(context.Configuration);
    }

    private static void ConfigureLogging(ILoggingBuilder builder)
    {
        builder.ClearProviders();
        builder.AddPrettyConsole();
        builder.AddDebugger();

        if (OperatingSystem.IsWindows())
        {
            builder.AddEventLog();

            // Required to convince the EventLogLoggerProvider to honor
            // configuration for more than just log level.
            // https://github.com/dotnet/extensions/issues/2519
            LoggerProviderOptions.RegisterProviderOptions
                <EventLogSettings, EventLogLoggerProvider>(builder.Services);
        }
    }
}

/*
    Copyright 2020 Jeffrey Sharp

    Permission to use, copy, modify, and distribute this software for any
    purpose with or without fee is hereby granted, provided that the above
    copyright notice and this permission notice appear in all copies.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
    WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
    MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
    ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
    WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
    ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
    OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RSMassTransit
{
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

            return Host
                .CreateDefaultBuilder(args)
                .UseContentRoot(AppContext.BaseDirectory)
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

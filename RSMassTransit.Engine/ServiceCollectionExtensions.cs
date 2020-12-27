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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSMassTransit.Bus;
using RSMassTransit.ReportingServices;
using RSMassTransit.Storage;

namespace RSMassTransit
{
    /// <summary>
    ///   Extension methods for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///   Registers a complete set of RSMassTransit services with the
        ///   specified configuration.
        /// </summary>
        /// <param name="services">
        ///   The services collection in which to register services.
        /// </param>
        /// <param name="configuration">
        ///   The configuration to use.
        /// </param>
        /// <returns>
        ///   The services collection <paramref name="services"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="services"/> and/or
        ///   <paramref name="configuration"/> are <c>null</c>.
        /// </exception>
        /// <exception cref="ConfigurationException">
        ///   One or more values in <paramref name="configuration"/> is
        ///   invalid.
        /// </exception>
        public static IServiceCollection AddRSMassTransit(
            this IServiceCollection services,
            IConfiguration          configuration)
        {
            services.AddBus               (configuration.GetSection("Bus"    ));
            services.AddBlobStorage       (configuration.GetSection("Storage"));
            services.AddReportingServices ();
            return services;
        }
    }
}

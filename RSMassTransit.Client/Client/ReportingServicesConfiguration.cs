/*
    Copyright (C) 2020 Jeffrey Sharp

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
using System.Net;

namespace RSMassTransit.Client
{
    /// <summary>
    ///   Properties specifying how to communicate with a RSMassTransit instance.
    /// </summary>
    public class ReportingServicesConfiguration
    {
        /// <summary>
        ///   The default value of the <see cref="BusQueue"/> property.
        /// </summary>
        public const string DefaultBusQueue = "reports";

        /// <summary>
        ///   The default value of the <see cref="RequestTimeout"/> property, in seconds.
        /// </summary>
        public const int DefaultRequestTimeoutSeconds = 30;

        /// <summary>
        ///   URI of the message bus.  The scheme of the URI specifies the kind
        ///   of message bus (RabbitMQ, etc.).
        /// </summary>
        public Uri BusUri { get; set; }

        /// <summary>
        ///   Name of the queue within the message bus.  The default value is
        ///   <c>"reports"</c>.
        /// </summary>
        public string BusQueue { get; set; } = DefaultBusQueue;

        /// <summary>
        ///   Credential used to authenticate with the message bus.
        ///   If omitted, behavior is client-specific.
        /// </summary>
        public NetworkCredential BusCredential { get; set; }

        /// <summary>
        ///   The duration after which the client will cease waiting for a
        ///   response, if the caller does not provide an explicit timeout.
        ///   The default value is 30 seconds.
        /// </summary>
        public TimeSpan RequestTimeout { get; set; }
            = TimeSpan.FromSeconds(DefaultRequestTimeoutSeconds);

        ///// <summary>
        /////   The duration after which an unprocessed request is deleted from
        /////   its queue.
        ///// </summary>
        //TimeSpan? RequestTimeToLive { get; }

        ///// <summary>
        /////   The duration after which the client times out when waiting for a
        /////   response to a report execution request.
        ///// </summary>
        //TimeSpan ExecuteReportTimeout { get; }

        ///// <summary>
        /////   The duration after which the client times out when waiting for a
        /////   response to a report management request.
        ///// </summary>
        //TimeSpan ManageReportTimeout { get; }
    }
}

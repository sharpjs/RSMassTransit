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

namespace RSMassTransit.Bus
{
    /// <summary>
    ///   Configuration for bus access.
    /// </summary>
    public interface IBusConfiguration
    {
        /// <summary>
        ///   Gets the URI of the bus host.
        /// </summary>
        Uri HostUri { get; }

        /// <summary>
        ///   Gets the name of the queue from which to consume requests.
        /// </summary>
        string QueueName { get; }

        /// <summary>
        ///   Gets the name of the shared secret used for authentication.
        /// </summary>
        string Secret { get; }

        /// <summary>
        ///   Gets the content of the shared secret used for authentication.
        /// </summary>
        string SecretName { get; }
    }
}

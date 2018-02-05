/*
    Copyright (C) 2018 Jeffrey Sharp

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
using System.Collections.Generic;

namespace RSMassTransit.Messages
{
    /// <summary>
    ///   The response to a request to execute a report.
    /// </summary>
    public interface IExecuteReportResponse : IMessage
    {
        /// <summary>
        ///   URI of the rendered report.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     The following URI types are valid:
        ///   </para>
        ///   <list type="bullet">
        ///     <item><c>file:</c></item>
        ///     <item><c>https:</c></item>
        ///   </list>
        /// </remarks>
        Uri Uri { get; set; }

        /// <summary>
        ///   Content type (MIME type) of the rendered report.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        ///   File name extension of the rendered report.
        /// </summary>
        string FileNameExtension { get; set; }

        /// <summary>
        ///   Length of the rendered report, in bytes.
        /// </summary>
        long Length { get; set; }

        /// <summary>
        ///   Diagnostic messages produced during report execution.
        /// </summary>
        IList<string> Messages { get; set; }
    }
}

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

using System.Collections.Generic;

namespace RSMassTransit.Messages
{
    /// <summary>
    ///   A request to execute a report.
    /// </summary>
    public interface IExecuteReportRequest : ICredential, IMessage
    {
        /// <summary>
        ///   Virtual path of the report on the report server.
        ///   Example: <c>"/My Reports/Contoso/BalanceSheet"</c>
        /// </summary>
        string Path { get; set; }

        /// <summary>
        ///   Values for the report's parameters.
        /// </summary>
        /// <remarks>
        ///   To provide multiple values for one parameter, populate this
        ///   collection with multiple key-value pairs for that parameter name.
        /// </remarks>
        IList<KeyValuePair<string, string>> ParameterValues { get; set; }

        /// <summary>
        ///   Language and locale used to interpret parameter values such as
        ///   dates and numbers.  Example: <c>"en-US"</c>
        /// </summary>
        string ParameterLanguage { get; set; }

        /// <summary>
        ///   Format in which to render the report.
        /// </summary>
        ReportFormat Format { get; set; }
    }
}

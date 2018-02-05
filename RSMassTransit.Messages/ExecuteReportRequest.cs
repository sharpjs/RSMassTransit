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
    ///   Default implementation of <see cref="IExecuteReportRequest"/>.
    /// </summary>
    public class ExecuteReportRequest : IExecuteReportRequest
    {
        /// <inheritdoc/>
        public string Path { get; set; }

        /// <inheritdoc/>
        public IList<KeyValuePair<string, string>> ParameterValues
        {
            get => _parameterValues ?? (_parameterValues = new List<KeyValuePair<string, string>>());
            set => _parameterValues = value;
        }
        private IList<KeyValuePair<string, string>> _parameterValues;

        /// <inheritdoc/>
        public string ParameterLanguage { get; set; }

        /// <inheritdoc/>
        public ReportFormat Format { get; set; }

        /// <inheritdoc/>
        public string UserName { get; set; }

        /// <inheritdoc/>
        public string Password { get; set; }
    }
}

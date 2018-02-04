// Copyright (C) 2018 (to be determined)

using System;
using System.Collections.Generic;

namespace RSMassTransit.Messages
{
    /// <summary>
    ///   Default implementation of <see cref="IExecuteReportResponse"/>.
    /// </summary>
    public class ExecuteReportResponse : IExecuteReportResponse
    {
        /// <inheritdoc/>
        public bool Succeeded { get; set; }

        /// <inheritdoc/>
        public IList<string> Messages
        {
            get => _messages ?? (_messages = new List<string>());
            set => _messages = value;
        }
        private IList<string> _messages;

        /// <inheritdoc/>
        public Uri Uri { get; set; }

        /// <inheritdoc/>
        public string ContentType { get; set; }

        /// <inheritdoc/>
        public string FileNameExtension { get; set; }

        /// <inheritdoc/>
        public long Length { get; set; }
    }
}

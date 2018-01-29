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
    }
}

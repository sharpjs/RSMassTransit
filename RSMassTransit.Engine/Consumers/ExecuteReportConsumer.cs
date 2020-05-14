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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using Microsoft.Extensions.Logging;
using RSMassTransit.Messages;
using RSMassTransit.ReportingServices;
using RSMassTransit.ReportingServices.Execution;
using Sharp.BlobStorage;

#nullable enable

namespace RSMassTransit.Consumers
{
    internal class ExecuteReportConsumer : IConsumer<IExecuteReportRequest>
    {
        private readonly IReportingServicesClientFactory _services;
        private readonly IBlobStorage                    _storage;
        private readonly ILogger                         _logger;

        public ExecuteReportConsumer(
            IReportingServicesClientFactory services,
            IBlobStorage                    storage,
            ILogger<ExecuteReportConsumer>? logger)
        {
            _services = services
                ?? throw new ArgumentNullException(nameof(services));

            _storage = storage
                ?? throw new ArgumentNullException(nameof(storage));

            _logger = logger as ILogger
                ?? NullLogger.Instance;
        }

        public async Task Consume(ConsumeContext<IExecuteReportRequest> context)
        {
            using var _ = _logger.BeginScope(nameof(IExecuteReportRequest));

            //return Log.DoAsync(nameof(IExecuteReportRequest), async () =>
            //{
                var request  = context.Message;
                var response = new ExecuteReportResponse();

                // Execute the report
                var bytes = await ExecuteReportAsync(request, response);

                // Upload to storage
                await StoreRenderedReportAsync(request, response, bytes);

                await context.RespondAsync<IExecuteReportResponse>(response);
            //});
        }

        private async Task<byte[]> ExecuteReportAsync(
            IExecuteReportRequest  request,
            IExecuteReportResponse response)
        {
            _logger.LogDebug("Creating report execution service client.");
            var credential = request.GetNetworkCredential();

            using (var client = _services.CreateExecutionClient(credential))
            {
                _logger.LogDebug("Invoking LoadReport2.");
                var loaded = await client.LoadReport2Async(
                    new LoadReport2Request { Report = request.Path }
                );

                var executionHeader = loaded.ExecutionHeader;

                _logger.LogDebug("Invoking SetExecutionParameters2.");
                await client.SetExecutionParameters2Async(
                    new SetExecutionParameters2Request
                    {
                        ExecutionHeader   = executionHeader,
                        Parameters        = GetParameterValues   (request),
                        ParameterLanguage = GetParameterLanguage (request),
                    }
                );

                _logger.LogDebug("Invoking Render2.");
                var rendered = await client.Render2Async(new Render2Request
                {
                    ExecutionHeader = executionHeader,
                    Format          = GetFormat(request),
                    PaginationMode  = PageCountMode.Actual
                });

                response.ContentType       = rendered.MimeType;
                response.FileNameExtension = rendered.Extension;
                response.Length            = rendered.Result.Length;
                response.Messages          = TranslateWarnings(rendered.Warnings);

                return rendered.Result;
            }
        }

        private async Task StoreRenderedReportAsync(
            IExecuteReportRequest  request,
            IExecuteReportResponse response,
            byte[]                 bytes)
        {
            _logger.LogDebug("Uploading rendered report to storage.");

            var extension = GetFileExtension(request);

            using (var stream = new MemoryStream(bytes, writable: false))
                response.Uri = await _storage.PutAsync(stream, extension);
        }

        private static ParameterValue[] GetParameterValues(IExecuteReportRequest request)
            => request.ParameterValues
                .Select(p => new ParameterValue { Name = p.Key, Value = p.Value })
                .ToArray();

        private static string GetParameterLanguage(IExecuteReportRequest request)
            => request.ParameterLanguage.NullIfEmpty()
                ?? CultureInfo.CurrentCulture.Name; // Perhaps make configurable?

        private static string GetFormat(IExecuteReportRequest request)
            => FormatNames[request.Format]; // TODO: Handle not-found errors

        private static readonly Dictionary<ReportFormat, string>
            FormatNames = new Dictionary<ReportFormat, string>
            {
                [ReportFormat.Word]        = "WORDOPENXML",
                [ReportFormat.WordLegacy]  = "WORD",
                [ReportFormat.Excel]       = "EXCELOPENXML",
                [ReportFormat.ExcelLegacy] = "EXCEL",
                [ReportFormat.PowerPoint]  = "PPTX",
                [ReportFormat.Pdf]         = "PDF",
                [ReportFormat.Tiff]        = "IMAGE",
                [ReportFormat.Html4]       = "HTML4.0",
                [ReportFormat.Html5]       = "HTML5",
                [ReportFormat.Mhtml]       = "MHTML",
                [ReportFormat.Csv]         = "CSV",
                [ReportFormat.Xml]         = "XML"
            };

        private static string GetFileExtension(IExecuteReportRequest request)
            => FileExtensions[request.Format]; // TODO: Handle not-found errors

        private static readonly Dictionary<ReportFormat, string>
            FileExtensions = new Dictionary<ReportFormat, string>
            {
                [ReportFormat.Word]        = ".docx",
                [ReportFormat.WordLegacy]  = ".doc",
                [ReportFormat.Excel]       = ".xlsx",
                [ReportFormat.ExcelLegacy] = ".xls",
                [ReportFormat.PowerPoint]  = ".pptx",
                [ReportFormat.Pdf]         = ".pdf",
                [ReportFormat.Tiff]        = ".tiff",
                [ReportFormat.Html4]       = ".html",
                [ReportFormat.Html5]       = ".html",
                [ReportFormat.Mhtml]       = ".mhtml",
                [ReportFormat.Csv]         = ".csv",
                [ReportFormat.Xml]         = ".xml"
            };

        private IList<string> TranslateWarnings(Warning[] warnings)
        {
            if (warnings == null || warnings.Length == 0)
                return new string[0];

            return Array.ConvertAll(
                warnings,
                w => $"{w.Severity}: [{w.ObjectType}/{w.ObjectName}]: {w.Message} [{w.Code}]"
            );
        }
    }
}

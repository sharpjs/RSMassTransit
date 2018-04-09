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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using RSMassTransit.Messages;
using RSMassTransit.ReportingServices;
using RSMassTransit.ReportingServices.Execution;
using Sharp.BlobStorage;

namespace RSMassTransit.Consumers
{
    internal class ExecuteReportConsumer : IConsumer<IExecuteReportRequest>
    {
        private readonly IReportingServicesClientFactory _services;
        private readonly IBlobStorage                    _storage;

        public ExecuteReportConsumer(
            IReportingServicesClientFactory services,
            IBlobStorage                    storage)
        {
            _services = services
                ?? throw new ArgumentNullException(nameof(services));

            _storage = storage
                ?? throw new ArgumentNullException(nameof(storage));
        }

        public Task Consume(ConsumeContext<IExecuteReportRequest> context)
        {
            return Log.DoAsync(nameof(IExecuteReportRequest), async () =>
            {
                var request  = context.Message;
                var response = new ExecuteReportResponse();

                // Execute the report
                var bytes = await ExecuteReport(request, response);

                // Upload to storage
                using (var stream = new MemoryStream(bytes, writable: false))
                    response.Uri = await _storage.PutAsync(stream);

                await context.RespondAsync<IExecuteReportResponse>(response);
            });
        }

        private async Task<byte[]> ExecuteReport(
            IExecuteReportRequest  request,
            IExecuteReportResponse response)
        {
            Log.Verbose("Creating report execution service client.");
            var credential = request.GetNetworkCredential();

            using (var client = _services.CreateExecutionClient(credential))
            {
                Log.Verbose("Invoking LoadReport2.");
                var loaded = await client.LoadReport2Async(
                    new LoadReport2Request { Report = request.Path }
                );

                var executionHeader = loaded.ExecutionHeader;

                Log.Verbose("Invoking SetExecutionParameters2.");
                await client.SetExecutionParameters2Async(
                    new SetExecutionParameters2Request
                    {
                        ExecutionHeader   = executionHeader,
                        Parameters        = GetParameterValues   (request),
                        ParameterLanguage = GetParameterLanguage (request),
                    }
                );

                Log.Verbose("Invoking Render2.");
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

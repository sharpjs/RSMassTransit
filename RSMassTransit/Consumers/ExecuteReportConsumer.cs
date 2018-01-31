﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

using RSMassTransit.Messages;
using RSMassTransit.ReportingServices;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.Consumers
{
    internal class ExecuteReportConsumer : IConsumer<IExecuteReportRequest>
    {
        private readonly IReportingServicesClientFactory _services;

        public ExecuteReportConsumer(IReportingServicesClientFactory services)
        {
            _services = services
                ?? throw new ArgumentNullException(nameof(services));
        }

        public async Task Consume(ConsumeContext<IExecuteReportRequest> context)
        {
            var response = new ExecuteReportResponse();

            try
            {
                var request = context.Message;
                var bytes   = ExecuteReport(request, response);
                // upload to azure
                response.Succeeded = true;
            }
            catch (Exception e)
            {
                response.Messages.Add(e.ToString());
            }

            await context.RespondAsync<IExecuteReportResponse>(response);
        }

        private async Task<byte[]> ExecuteReport(IExecuteReportRequest request, IExecuteReportResponse response)
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
                response.Messages          = TranslateWarnings(rendered.Warnings);
                //???                      = rendered.Encoding;

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
            => warnings
                .Select(w => $"{w.Severity}: [{w.ObjectType}/{w.ObjectName}]: {w.Message} [{w.Code}]")
                .ToList();
    }
}
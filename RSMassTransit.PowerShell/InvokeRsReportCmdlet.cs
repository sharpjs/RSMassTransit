// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using System.Collections;
using System.Globalization;
using System.Management.Automation;
using RSMassTransit.Messages;

namespace RSMassTransit.PowerShell;

[Cmdlet(VerbsLifecycle.Invoke, "RsReport")]
public class InvokeRsReportCmdlet : RSMassTransitCmdlet
{
    [Parameter(Position = 0, Mandatory = true)]
    [ValidateNotNullOrEmpty]
    public string? Path { get; set; }

    [Parameter(Position = 1, Mandatory = true)]
    public ReportFormat Format { get; set; }

    [Parameter(Position = 2)]
    [AllowNull, AllowEmptyCollection]
    public Hashtable? Parameters { get; set; }

    protected override void ProcessRecord()
    {
        WriteVerbose("Sending ExecuteReportRequest");

        var request = new ExecuteReportRequest
        {
            Path              = Path,
            Format            = Format,
            ParameterValues   = GetParameters(),
            ParameterLanguage = CultureInfo.CurrentCulture.Name
        };

        ProvideRsCredential(request);

        // NULLS: Client is set in BeginProcessing
        var response = WithFaultHandling(() => Client!.ExecuteReport(request));

        WriteObject(response);
    }

    private IList<KeyValuePair<string, string>> GetParameters()
    {
        var pairs = new List<KeyValuePair<string, string>>();

        if (Parameters == null)
            return pairs;

        foreach (DictionaryEntry e in Parameters)
        {
            if (e.Value is ICollection values)
                foreach (object value in values)
                    pairs.Add(CreateParameter(e.Key, value));
            else
                pairs.Add(CreateParameter(e.Key, e.Value));
        }

        return pairs;
    }

    private static KeyValuePair<string, string> CreateParameter(object key, object value)
    {
        return new KeyValuePair<string, string>
        (
            key  ?.ToString() ?? "",
            value?.ToString() ?? ""
        );
    }
}

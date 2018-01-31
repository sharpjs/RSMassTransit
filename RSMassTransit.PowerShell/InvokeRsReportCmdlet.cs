using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace RSMassTransit.PowerShell
{
    [Cmdlet(VerbsLifecycle.Invoke, "RsReport")]
    public class InvokeRsReportCmdlet : PSCmdlet, IDisposable
    {
        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        [ValidateSet("Word", "Excel", "PowerPoint", "Pdf")]
        public string Format { get; set; }

        [Parameter(Position = 2)]
        [AllowNull, AllowEmptyCollection]
        public Hashtable Parameters { get; set; }

        [Parameter]
        [Credential]
        public PSCredential Credential { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        public void Dispose()
        {
            // Note: Simple dispose, because only managed resources here
        }
    }
}

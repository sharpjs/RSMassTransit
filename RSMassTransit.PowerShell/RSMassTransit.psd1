<#
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
#>
@{
    # Identity
    GUID          = '86812cf8-957d-4eca-b899-6e8c580d68ef'
    RootModule    = 'RSMassTransit.PowerShell.dll'
    ModuleVersion = '0.0.1'

    # General
    Author      = 'Jeffrey Sharp'
    CompanyName = 'Jeffrey Sharp'
    Copyright   = 'Copyright (C) 2020 Jeffrey Sharp'
    Description = 'Provides the Invoke-RsReport cmdlet, which executes a SQL Server Reporting Services report via a MassTransit message bus.'

    # Requirements
    PowerShellVersion      = '5.1'
    CompatiblePSEditions   = @("Desktop")
    DotNetFrameworkVersion = '4.7.1'  # Valid for Desktop edition only
    CLRVersion             = '4.0'    # Valid for Desktop edition only

    # Exports
    # NOTE: Use empty arrays to indicate no exports.
    FunctionsToExport    = @()
    CmdletsToExport      = @("Invoke-RsReport")
    VariablesToExport    = @()
    AliasesToExport      = @()
    DscResourcesToExport = @()

    # Discoverability and URLs
    PrivateData = @{
        PSData = @{
            Tags = @("SSRS", "Reporting", "Report", "RsReport", "Invoke", "MassTransit", "message", "bus")
            LicenseUri = 'https://github.com/sharpjs/RSMassTransit/blob/master/LICENSE.txt'
            ProjectUri = 'https://github.com/sharpjs/RSMassTransit'
            # IconUri = ''
            ReleaseNotes = @"
Release notes are available at:
https://github.com/sharpjs/RSMassTransit/releases
"@
        }
    }
}

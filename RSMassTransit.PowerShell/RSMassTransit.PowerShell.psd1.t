<#
    {Copyright}

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
    ModuleVersion = '{VersionPrefix}'

    # General
    Description = 'Provides the Invoke-RsReport cmdlet, which executes a SQL Server Reporting Services report via a MassTransit message bus.'
    Author      = 'Jeffrey Sharp'
    CompanyName = 'Subatomix Research Inc.'
    Copyright   = '{Copyright}'

    # Requirements
    PowerShellVersion      = '5.1'
    CompatiblePSEditions   = "Desktop", "Core"  # Added in PowerShell 5.1
    DotNetFrameworkVersion = '4.6.1'            # Valid for Desktop edition only
    CLRVersion             = '4.0'              # Valid for Desktop edition only
    #RequiredModules       = @(...)
    #RequiredAssemblies    = @(...)

    # Initialization
    #ScriptsToProcess = @(...)
    #TypesToProcess   = @(...)
    #FormatsToProcess = @(...)
    #NestedModules    = @(...)

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
            # Additional metadata
            Prerelease   = '{VersionSuffix}'
            ProjectUri   = 'https://github.com/sharpjs/RSMassTransit'
            ReleaseNotes = "https://github.com/sharpjs/RSMassTransit/blob/main/CHANGES.md"
            LicenseUri   = 'https://github.com/sharpjs/RSMassTransit/blob/main/LICENSE.txt'
            IconUri      = 'https://github.com/sharpjs/RSMassTransit/blob/main/icon.png'
            Tags         = @(
                "SSRS", "Reporting", "Report", "RsReport", "Invoke", "MassTransit", "message", "bus",
                "PSEdition_Desktop", "PSEdition_Core", "Windows", "Linux", "MacOS"
            )
        }
    }
}

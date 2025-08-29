# {Copyright}
# SPDX-License-Identifier: MIT
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
                "SSRS", "Reporting", "MassTransit", "Bus", "RabbitMQ", "ServiceBus",
                "PSEdition_Desktop", "PSEdition_Core", "Windows", "Linux", "MacOS"
            )
        }
    }
}

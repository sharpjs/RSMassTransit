<#
.SYNOPSIS
    Configures RSMassTransit.

.DESCRIPTION
    This script modifies the specified entries in the RSMassTransit configuration file, appsettings.json.

.INPUTS
    None.
        This script does not accept pipeline input.

.OUTPUTS
    None.
        This script does not produce output.

.NOTES
    Copyright Subatomix Research Inc.
    SPDX-License-Identifier: MIT

.EXAMPLE
    Set-RSMassTransitConfiguration -BusHostUri sb://my-bus

    Sets the URI of the bus host to 'sb://my-bus'.

.LINK
    https://github.com/sharpjs/RSMassTransit
#>
[CmdletBinding()]
param (
    # URI of the bus host.  The following URI formats are supported:
    #
    # - rabbitmq://some-hostname/optional-vhost  (RabbitMQ)
    # - sb://some-namespace-name                 (Azure Service Bus)
    [Parameter(ValueFromPipelineByPropertyName)]
    [Alias("Host")]
    [uri]
    $BusHostUri,

    # Name of the queue from which to consume requests.
    [Parameter(ValueFromPipelineByPropertyName)]
    [Alias("Queue")]
    [string]
    $BusQueueName,

    # Credential used to authenticate with the bus host.
    [Parameter(ValueFromPipelineByPropertyName)]
    [Alias("Credential")]
    [pscredential]
    $BusCredential,

    # URI of the SSRS SOAP endpoint.
    [Parameter(ValueFromPipelineByPropertyName)]
    [uri]
    $ReportServiceUri,

    # Maximum size in bytes of response to allow from the SSRS SOAP endpoint.
    [Parameter(ValueFromPipelineByPropertyName)]
    [long]
    $ReportServiceMaxResponseSize,

    # Maximum duration to wait for a response from the SSRS SOAP endpoint.
    [Parameter(ValueFromPipelineByPropertyName)]
    [timespan]
    $ReportServiceTimeout,

    # Type of report storage. Valid types are File and AzureBlob.
    [Parameter(ValueFromPipelineByPropertyName)]
    [ValidateSet($null, "File", "AzureBlob")]
    [string]
    $StorageType,

    # Path to a directory in which to store reports as files.
    [Parameter(ValueFromPipelineByPropertyName)]
    [Alias("Path")]
    [string]
    $FilePath,

    # Azure Storage connection string.
    [Parameter(ValueFromPipelineByPropertyName)]
    [Alias("ConnectionString")]
    [string]
    $AzureBlobConnectionString,

    # Name of blob container in Azure Storage in which to store reports as blobs.
    [Parameter(ValueFromPipelineByPropertyName)]
    [Alias("ContainerName")]
    [string]
    $AzureBlobContainerName,

    # Maximum duration to wait for running reports to complete when RSMassTransit is stopping.
    [Parameter(ValueFromPipelineByPropertyName)]
    [timespan]
    $StopTimeout
)

begin {
    #Requires -Version 5.1
    $ErrorActionPreference = "Stop"
    Set-StrictMode -Off

    $AppConfigChanged  = $false
    $AppConfigPath     = Join-Path $PSScriptRoot appsettings.json -Resolve
    $AppConfig         = Get-Content $AppConfigPath -Raw | ConvertFrom-Json

    $HostConfigChanged = $false
    $HostConfigPath    = Join-Path $PSScriptRoot hostsettings.json -Resolve
    $HostConfig        = Get-Content $HostConfigPath -Raw | ConvertFrom-Json
}

process {
    if ($BusHostUri -and $BusHostUri.ToString()) {
        $AppConfig.Bus.HostUri = $BusHostUri
        $AppConfigChanged      = $true
    }

    if ($BusQueueName) {
        $AppConfig.Bus.QueueName = $BusQueueName
        $AppConfigChanged        = $true
    }

    if ($BusCredential -and $BusCredential.UserName -and $BusCredential.Password) {
        $AppConfig.Bus.SecretName = $BusCredential.UserName
        $AppConfig.Bus.Secret     = $BusCredential.GetNetworkCredential().Password
        $AppConfigChanged         = $true
    }

    if ($ReportServiceUri -and $ReportServiceUri.ToString()) {
        $AppConfig.Reporting.ExecutionUri = "$ReportServiceUri/ReportExecution2005.asmx"
        $AppConfigChanged                 = $true
    }

    if ($ReportServiceMaxResponseSize) {
        $AppConfig.Reporting.MaxResponseSize = $ReportServiceMaxResponseSize
        $AppConfigChanged                    = $true
    }

    if ($ReportServiceTimeout) {
        $AppConfig.Reporting.Timeout = $ReportServiceTimeout.ToString()
        $AppConfigChanged            = $true
    }

    if ($StorageType) {
        $AppConfig.Storage.Type = $StorageType
        $AppConfigChanged       = $true
    }

    if ($FilePath) {
        $AppConfig.Storage.File.Path = $FilePath
        $AppConfigChanged            = $true
    }

    if ($AzureBlobConnectionString) {
        $AppConfig.Storage.AzureBlob.ConnectionString = $AzureBlobConnectionString
        $AppConfigChanged                             = $true
    }

    if ($AzureBlobContainerName) {
        $AppConfig.Storage.AzureBlob.ContainerName = $AzureBlobContainerName
        $AppConfigChanged                          = $true
    }

    if ($StopTimeout) {
        $HostConfig.shutdownTimeoutSeconds = [long] $StopTimeout.TotalSeconds
        $HostConfigChanged                 = $true
    }
}

end {
    if ($AppConfigChanged) {
        $AppConfig | ConvertTo-Json -Depth 32 | Set-Content $AppConfigPath -Encoding utf8
    }

    if ($HostConfigChanged) {
        $HostConfig | ConvertTo-Json -Depth 32 | Set-Content $HostConfigPath -Encoding utf8
    }
}

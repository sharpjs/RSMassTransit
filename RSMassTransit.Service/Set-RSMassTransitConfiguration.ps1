<#
.SYNOPSIS
    Configures RSMassTransit.

.DESCRIPTION
    This script modifies the specified entries in the RSMassTransit configuration file, appsettings.json.

.INPUTS
    None.

.OUTPUTS
    None.

.NOTES
    Copyright 2021 Jeffrey Sharp

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

.EXAMPLE
    Set-Configuration -BusHostUri sb://my-bus

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
    $AzureBlobContainerName
)

begin {
    #Requires -Version 5.1
    $ErrorActionPreference = "Stop"
    Set-StrictMode -Off

    $ConfigPath = Join-Path $PSScriptRoot appsettings.json -Resolve
    $Config     = Get-Content $ConfigPath -Raw | ConvertFrom-Json
    $Changed    = $false
}

process {
    if ($BusHostUri -and $BusHostUri.ToString()) {
        $Config.Bus.HostUri = $BusHostUri
        $Changed            = $true
    }

    if ($BusQueueName) {
        $Config.Bus.QueueName = $BusQueueName
        $Changed              = $true
    }

    if ($BusCredential -and $BusCredential.UserName -and $BusCredential.Password) {
        $Config.Bus.SecretName = $BusCredential.UserName
        $Config.Bus.Secret     = $BusCredential.GetNetworkCredential().Password
        $Changed               = $true
    }

    if ($StorageType) {
        $Config.Storage.Type = $StorageType
        $Changed             = $true
    }

    if ($FilePath) {
        $Config.Storage.File.Path = $FilePath
        $Changed                  = $true
    }

    if ($AzureBlobConnectionString) {
        $Config.Storage.AzureBlob.ConnectionString = $AzureBlobConnectionString
        $Changed                                   = $true
    }

    if ($AzureBlobContainerName) {
        $Config.Storage.AzureBlob.ContainerName = $AzureBlobContainerName
        $Changed                                = $true
    }
}

end {
    if ($Changed) {
        $Config | ConvertTo-Json -Depth 32 | Set-Content $ConfigPath -Encoding utf8
    }
}

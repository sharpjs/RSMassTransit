#Requires -Version 5.1
#Requires -RunAsAdministrator
using namespace System.Management.Automation 
using namespace System.Runtime.InteropServices

<#
.SYNOPSIS
    Unregisters the RSMassTransit Windows service.

.DESCRIPTION
    This command stops and unregisters an existing Windows service for RSMassTransit.

    If the service does not exist, this script does nothing.

.INPUTS
    None
        This script does not accept pipeline input.

.OUTPUTS
    None
        This script does not produce output.

.NOTES
    Copyright Subatomix Research Inc.
    SPDX-License-Identifier: MIT

.LINK
    https://github.com/sharpjs/RSMassTransit
#>

[CmdletBinding()]
param ()

begin {
    $ErrorActionPreference = "Stop"
    Set-StrictMode -Off
}

process {
    $ServiceName = "RSMassTransit"

    if (Get-Service $ServiceName -ErrorAction Ignore) {
        Write-Host "Stopping service $ServiceName."
        Stop-Service $ServiceName -Force

        Write-Host "Removing service $ServiceName."
        & sc.exe delete $ServiceName > $null
        if ($LASTEXITCODE) {
            $e = [ExternalException] "sc.exe exited with code $LASTEXITCODE."
            $PSCmdlet.ThrowTerminatingError([ErrorRecord]::new($e, $null, 0, $null))
        }
    }
}

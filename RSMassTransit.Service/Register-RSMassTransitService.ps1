#Requires -Version 5.1
#Requires -RunAsAdministrator
using namespace System.Diagnostics
using namespace System.Management.Automation 
using namespace System.Runtime.InteropServices

<#
.SYNOPSIS
    Registers the RSMassTransit Windows service.

.DESCRIPTION
    This command registers a new Windows service for RSMassTransit.

    If the service already exists, this script updates the existing registration.

.INPUTS
    None
        This script does not accept pipeline input.

.OUTPUTS
    System.ServiceProcess.ServiceController
        This script returns an object that represents the new service.

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

    $DependsOn = @(
        Get-Service SQLServerReportingServices, ReportServer -ErrorAction Ignore `
            | ForEach-Object Name
    )

    if (-not (Get-Service $ServiceName -ErrorAction Ignore)) {
        Write-Host "Creating service $ServiceName."
        New-Service `
            -Name           $ServiceName `
            -DisplayName    'SQL Server Reporting Services MassTransit Interface' `
            -Description    'Executes reports in response to messages received on a MassTransit message bus.' `
            -BinaryPathName (Join-Path $PSScriptRoot RSMassTransit.Service.exe) `
            -StartupType    Automatic `
            -DependsOn      $DependsOn `
            > $null
    }

    Write-Host "Setting service $ServiceName recovery behavior."
    & sc.exe failure $ServiceName actions= restart/1000/restart/10000/restart/30000 reset= 300 > $null
    # on 1st failure, do this ────────────────╯     │      │      │      │      │           │
    #   after this delay (ms) ──────────────────────╯      │      │      │      │           │
    # on 2nd failure, do this ─────────────────────────────╯      │      │      │           │
    #   after this delay (ms) ────────────────────────────────────╯      │      │           │
    # and thereafter, do this ───────────────────────────────────────────╯      │           │
    #   after this delay (ms) ──────────────────────────────────────────────────╯           │
    # reset failure count after (s) ────────────────────────────────────────────────────────╯
    if ($LASTEXITCODE) {
        $e = [ExternalException] "sc.exe exited with code $LASTEXITCODE."
        $PSCmdlet.ThrowTerminatingError([ErrorRecord]::new($e, $null, 0, $null))
    }

    if (![EventLog]::Exists($ServiceName) -or ![EventLog]::SourceExists($ServiceName)) {
        Write-Host "Creating event log $ServiceName."
        [EventLog]::CreateEventSource($ServiceName, $ServiceName)
    }

    Get-Service $ServiceName 
}

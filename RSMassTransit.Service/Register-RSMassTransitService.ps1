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

    if (-not (Get-Service $ServiceName -ErrorAction SilentlyContinue)) {
        Write-Host "Creating service $ServiceName."
        New-Service `
            -Name           $ServiceName `
            -DisplayName    'SQL Server Reporting Services MassTransit Interface' `
            -Description    'Executes reports in response to messages received on a MassTransit message bus.' `
            -BinaryPathName (Join-Path $PSScriptRoot RSMassTransit.Service.exe) `
            -StartupType    Manual `
            -DependsOn      SQLServerReportingServices `
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

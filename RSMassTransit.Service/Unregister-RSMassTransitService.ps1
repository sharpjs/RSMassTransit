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

    if (Get-Service $ServiceName -ErrorAction SilentlyContinue) {
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

# RSMassTransit
A [MassTransit](https://github.com/MassTransit/MassTransit)
message bus interface for SQL Server Reporting Services.

Idea by [Jahn Swob (@jahn-swob)](https://github.com/jahn-swob),
implemented by [Jeff Sharp (@sharpjs)](https://github.com/sharpjs).

## Status

Experimental.  Currently, only report execution is supported.

## Development Setup

* Requirements:
  * Visual Studio 2017
  * SQL Server Reporting Services 2016 (might work with earlier versions)
  * PowerShell 5 (might work with earlier versions)
* In RSMassTransit.PowerShell project properties, Debug tab:
  * Click "Start external program" and enter:<br>
    `C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe`
  * In "Command line arguments", enter:<br>
    `-NoLogo -NoProfile -NoExit -Command "Import-Module .\RSMassTransit.psd1"`
* In solution properties, Startup Project tab, ensure that Action is set to `Start` for:
  * RSMassTransit
  * RSMassTransit.PowerShell
* On F5 run, two windows should appear:
  * RSMassTransit service console window
  * PowerShell prompt
* In the PowerShell window, enter a command similar to the following:
  ```powershell
    Invoke-RsReport `
        -Path           '/My Reports/IT Reports/TPS Report' `
        -Format         Pdf `
        -TimeoutSeconds 30 `
        -RsCredential   (Get-Credential)
  ```

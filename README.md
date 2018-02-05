# RSMassTransit
A [MassTransit](https://github.com/MassTransit/MassTransit)
message bus interface for SQL Server Reporting Services.

Idea by [Jahn Swob (@jahn-swob)](https://github.com/jahn-swob),
implemented by [Jeff Sharp (@sharpjs)](https://github.com/sharpjs).

## Status

Experimental.  Currently, only report execution is supported.

## Development Setup

* Use Visual Studio 2017 and PowerShell 5 or later.
* In RSMassTransit.PowerShell project properties, Debug tab:
  * Click "Start external program" and enter:<br>
    `C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe`
  * In "Command line arguments", enter:<br>
    `-NoLogo -NoProfile -NoExit -Command "Import-Module .\RSMassTransit.psd1"`
* In solution properties, Startup Project tab, ensure that Action is set to `Start` for:
  * RSMassTransit
  * RSMassTransit.PowerShell

# RSMassTransit

A [MassTransit](https://github.com/MassTransit/MassTransit)
message bus interface for SQL Server Reporting Services.

Idea by [Jahn Swob (@jahn-swob)](https://github.com/jahn-swob),
implemented by [Jeff Sharp (@sharpjs)](https://github.com/sharpjs).

## Status

[![Build](https://github.com/sharpjs/RSMassTransit/workflows/Build/badge.svg)](https://github.com/sharpjs/RSMassTransit/actions?query=workflow%3ABuild)

- Currently, only report execution is implemented.
- Report deployment and discovery are planned for implementation sometime
  between right now and the entropic end of the universe.
- Versions targeting .NET Framework are deployed in production and have been
  so for several years, with few defects.
- A port to .NET Core 3.1 is nearing release.
- Test coverage is inadequate.

## Installation

*TODO*

## Development Setup

* Requirements:
  * Visual Studio 2019
  * SQL Server Reporting Services 2019 (might work with earlier versions)
  * PowerShell 5.1+
  * PowerShell Core 6.x or PowerShell 7+
* In solution properties, Startup Project tab, ensure that Action is set to `Start` for:
  * RSMassTransit.Tool
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

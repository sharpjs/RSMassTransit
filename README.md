# RSMassTransit

A [MassTransit](https://github.com/MassTransit/MassTransit)
message bus interface for SQL Server Reporting Services.

Idea by [Jahn Swob (@jahn-swob)](https://github.com/jahn-swob),
implemented by [Jeff Sharp (@sharpjs)](https://github.com/sharpjs).

## Status

[![Build](https://github.com/sharpjs/RSMassTransit/workflows/Build/badge.svg)](https://github.com/sharpjs/RSMassTransit/actions)
[![NuGet](https://img.shields.io/nuget/v/RSMassTransit.Client.svg)](https://www.nuget.org/packages/RSMassTransit.Client)
[![NuGet](https://img.shields.io/nuget/dt/RSMassTransit.Client.svg)](https://www.nuget.org/packages/RSMassTransit.Client)
<!--
[![NuGet](https://img.shields.io/powershellgallery/v/RSMassTransit.PowerShell.svg)](https://www.powershellgallery.com/packages/RSMassTransit.PowerShell)
[![NuGet](https://img.shields.io/powershellgallery/dt/RSMassTransit.PowerShell.svg)](https://www.powershellgallery.com/packages/RSMassTransit.PowerShell)
-->

- RSMassTransit exexutes reports and uploads their contents to blob storage.
  Other features like report discovery or deployment might become supported at
  some indeterminate time in the future.
- Used in production for several years with few reported defects.
- Test coverage is inadequate.
- Documentation is inadequate.

## Installation

*TODO*

## Development Setup

- Requirements:
  - Visual Studio 2022 or later
  - SQL Server Reporting Services 2022 (might work with earlier versions)
  - .NET SDKs and targeting packs (see `TargetFrameworks` in each project file)
  - PowerShell 5.1+
  - PowerShell 7.2+
- In solution properties, Startup Project tab, ensure that Action is set to `Start` for:
  - RSMassTransit.Tool
  - RSMassTransit.PowerShell
- On F5 run, two windows should appear:
  - RSMassTransit service console window
  - PowerShell prompt
- In the PowerShell window, enter a command similar to the following:
  ```powershell
  Invoke-RsReport `
      -Path           '/My Reports/IT Reports/TPS Report' `
      -Format         Pdf `
      -TimeoutSeconds 30 `
      -RsCredential   (Get-Credential)
  ```

<!--
  Copyright Jeffrey Sharp
  SPDX-License-Identifier: ISC
-->

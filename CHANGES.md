# Changes in RSMassTransit
This file documents all notable changes.

Most lines should begin with one of these words:
*Add*, *Fix*, *Update*, *Change*, *Deprecate*, *Remove*.

<!--
## [Unreleased](https://github.com/sharpjs/RSMassTransit/compare/release/1.1.0...HEAD)
(none)
-->

## [Unreleased](https://github.com/sharpjs/RSMassTransit/compare/release/1.0.0...HEAD)
<!--
## [2.0.0](https://github.com/sharpjs/RSMassTransit/compare/release/1.0.0...release/2.0.0)
-->
- Add prettier console output.
- Fix unwanted `_error` and `_skipped` queues being created for each client's
  default (response) queue.
- Update bus tuning:
  - Change prefetch count from zero to the concurrency level.
  - Lower Azure Service Bus maximum delivery count from 4X concurrency to
    concurrency plus one.
- Add dependency: Subatomix.Logging [1.0.0](https://github.com/sharpjs/Subatomix.Logging).

- Change target frameworks:

  Component               | .NET Core/5+ | .NET Standard | .NET Framework
  :-----------------------|:------------:|:-------------:|:--------------:
  Client packages         | 8.0          | 2.0           | 4.6.1
  Engine assembly         | 8.0          | *none*        | *none*
  .NET tool package       | 8.0          | *none*        | *none*
  Windows service package | 8.0          | *none*        | *none*
  Test suite              | 8.0          | *none*        | 4.8.1

- Add/Update dependencies:

  Package                        | Version
  :------------------------------|:-----------------
  Microsoft.Extensions.Hosting.* | [8.0.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.0/8.0.0.md)
  Microsoft.Extensions.Logging   | [8.0.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.0/8.0.0.md)
  Sharp.Disposable               | [1.1.1](https://github.com/sharpjs/Sharp.Disposable/releases/tag/release/1.1.1)
  Subatomix.Logging              | [1.0.0-pre.4](https://github.com/sharpjs/Subatomix.Logging)
  System.ServiceModel.*          | [8.0.0](https://github.com/dotnet/wcf/releases/tag/v3.4.0-rtm)

## [1.0.0](https://github.com/sharpjs/RSMassTransit/compare/v0.1.1...release/1.0.0)
- Change project structure.
  - Extract server engine into `RSMassTransit.Engine.dll`.
  - Add Windows service package.
  - Add .NET tool package.
  - Move client packages into this repository.
  - Move PowerShell module into this repository.

- Change target frameworks:

  Component               | .NET Core/5+ | .NET Standard | .NET Framework
  :-----------------------|:------------:|:-------------:|:--------------:
  Client packages         | ➤            | 2.0           | 4.6.1
  Engine assembly         | 6.0          | 2.1           | 4.8
  Test suite              | 6.0, 3.1     | *none*        | 4.8
  .NET tool package       | 6.0          | *none*        | *none*
  Windows service package | 6.0          | *none*        | *none*

- Add/Update dependencies:

  Package                     | Version
  :---------------------------|:-----------------
  Client packages             | 1.0.0
  MassTransit                 | [7.3.1](https://masstransit-project.com/releases/)
  Microsoft.Extensions.*      | [6.0.0](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.0/6.0.0.md), [6.0.1](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.2/6.0.2.md)
  System.ServiceModel.*       | [4.9.0](https://github.com/dotnet/wcf/releases/tag/v3.3.0-rtm)

- Change automated build system to GitHub Actions.

## [0.1.1](https://github.com/sharpjs/RSMassTransit/compare/v0.1.0...v0.1.1)
- Fix message prefetch setting not being applied on Azure Service Bus.

## [0.1.0](https://github.com/sharpjs/RSMassTransit/tree/v0.1.0)
- Update MassTransit to 6.2.5
- Fix bus tuning options not being applied completely

## 0.0.1
- Update MassTransit to 5.5.1

## 0.0.0
- Initial release

<!--
  Copyright Subatomix Research Inc.
  SPDX-License-Identifier: MIT
-->

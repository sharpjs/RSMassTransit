# Changes in RSMassTransit
This file documents all notable changes.

Most lines should begin with one of these words:
*Add*, *Fix*, *Update*, *Change*, *Deprecate*, *Remove*.

<!--
## [Unreleased](https://github.com/sharpjs/RSMassTransit/compare/v1.0.0-pre.2...HEAD)
(none)
-->

## [1.0.0 (WIP)](https://github.com/sharpjs/RSMassTransit/compare/v0.1.1...release/1.0.0-pre.3)
- Change project structure.
  - Extract server engine into `RSMassTransit.Engine.dll`.
  - Add Windows service package.
  - Add .NET tool package.
  - Move client packages into this repository.
  - Move PowerShell module into this repository.

- Change target frameworks:

  Component               | .NET Standard/Core/5+ | .NET Framework
  :-----------------------|:----------------------|:--------------------
  Client packages         | .NET Standard 2.0     | .NET Framework 4.6.1
  Engine assembly         | .NET Standard 2.1     | .NET Framework 4.8
  Test suite              | .NET Core 3.1         | .NET Framework 4.8
  .NET tool package       | .NET Core 3.1         | none
  Windows service package | .NET Core 3.1         | none

- Add/Update dependencies:
  - MassTransit 7.1.3
  - Microsoft.Extensions.* 3.1.11
  - Subatomix.Testing 1.1.0 *(build)*
  - System.ServiceModel.* [4.9.0](https://github.com/dotnet/wcf/releases/tag/v3.3.0-rtm)

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
  Copyright 2022 Jeffrey Sharp

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
-->

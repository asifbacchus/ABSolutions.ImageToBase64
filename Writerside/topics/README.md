# ABSolutions.ImageToBase64: Library for .NET Core

<link-summary>General overview and information about the ABSolutions.ImageToBase64 library.</link-summary>
<card-summary>General overview and information about the ABSolutions.ImageToBase64 library.</card-summary>
<web-summary>Information about ABSolutions.ImageToBase64 .NET library and links to in-depth documentation.</web-summary>

## Project information

| Author        | A-B Solutions (Asif Bacchus)                                            |
|---------------|-------------------------------------------------------------------------|
| Contact       | asif@a-b.solutions                                                      |
| Git repo      | [](https://github.com/asifbacchus/ABSolutions.ImageToBase64)            |
| Documentation | [](https://a-b.solutions/documentation/nuget/ABSolutions.ImageToBase64) |

{style="none"}

### Description

`ABSolutions.ImageToBase64` is a .NET Core library that converts images to base64 strings. Images can be located in the
local file system or on a remote system accessible via HTTP(S). The library uses a preconfigured or default `HttpClient`
provided via `IHttpClientFactory` injection, supports logging, and includes an in-memory caching system.

## Requirements and dependencies

This library is targeted for .NET Core 8 and has the following dependencies:

- `Microsoft.Extensions.Http` (>= 8.0.0)
- `Microsoft.Extensions.Options` (>= 8.0.2)
- `Microsoft.Extensions.Options.ConfigurationExtensions` (>= 8.0.0)

## Using this library

Please refer to the following topics for more information about using this library:

- [](Quickstart.md)
- [](Build-a-Demo-API.md)

Alternatively, you can inspect the `ABSolutions.ImageToBase64.Demo.API` project in
the [git repository](https://github.com/asifbacchus/ABSolutions.ImageToBase64).

## Terms of use

This project is licensed under
the [MIT license](https://github.com/asifbacchus/ABSolutions.ImageToBase64/blob/main/LICENSE). Basically, you are free
to use, modify, and distribute this project as you see fit. However, you *must* include the original license and
copyright notice in any copy of the project or substantial portion of the project. Also, understand that you are using
this library at your own risk and the author does **not** accept liability for any damages or issues that
may arise from your use of this library.

## Contributing guidelines

If you have suggestions for additional features or code improvements and would like to contribute to this project,
please let me know and/or submit a pull-request in the
the [git repository](https://github.com/asifbacchus/ABSolutions.ImageToBase64).

## Bug reports and help

If you discover a bug in this project or need assistance/clarification, please open an issue in
the [git repository](https://github.com/asifbacchus/ABSolutions.ImageToBase64) and I'll reply as soon as I can.

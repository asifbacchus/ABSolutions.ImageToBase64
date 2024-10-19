# ABSolutions.ImageToBase64: Blazor Component

<link-summary>General overview and information about the ABSolutions.ImageToBase64 Blazor Component.</link-summary>
<card-summary>General overview and information about the ABSolutions.ImageToBase64 Blazor Component.</card-summary>
<web-summary>Information about ABSolutions.ImageToBase64 Blazor Component and links to in-depth documentation.</web-summary>

## Project information

| Author        | A-B Solutions (Asif Bacchus)                                                |
|---------------|-----------------------------------------------------------------------------|
| Contact       | asif@a-b.solutions                                                          |
| Git repo      | [](https://github.com/asifbacchus/ABSolutions.ImageToBase64)                |
| Documentation | [](https://a-b.solutions/documentation/nuget/ImageToBase64-BlazorComponent) |

{style="none"}

### Description

`ABSolutions.ImageToBase64.BlazorComponent` is a Blazor Component that helps implement the `ABSolutions.ImageToBase64`
library in a Blazor application. The component takes care of injecting the backend library and all other "code-behind"
tasks. You can simply drop this component into your Blazor page, specify the file name to retrieve as an attribute, and
an `<img>` tag will be generated for you with the image embedded as a Base64 string.

Since this component is an implementation of the underlying library, you should refer to
the [ABSolutions.ImageToBase64](https://a-b.solutions/documentation/nuget/ImageToBase64-Library) documentation for
background information. This documentation will focus specifically on using the Blazor component.

## Requirements and dependencies

This library is targeted for .NET Core 8 and has the following dependencies:

- `Microsoft.AspNetCore.Components.Web` (>= 8.0.8)
- `ABSolutions.ImageToBase64` (>= 1.0.0)

## Using this library

### Usage example

Since this is a component, you can use it in your Blazor pages like this:

```html
@page "/my-page-with-an-image"
@using ABSolutions.ImageToBase64.BlazorComponent

<h1>My Page with an Image</h1>
<p>The image below is embedded using a Base64-encoded string</p>
<Base64Image Alt="Base64-encoded image" FileName="my-image.jpg"/>
```

> This example omits backend configuration, etc. This is just a sample showing how easily the component itself
> (`Base64Image` tag) can integrate with your projects.

### Detailed usage information

Please refer to the following topics for more information about using this library:

- [](Quickstart.md)
- [](Build-a-Demo-Blazor-Site.md)

Alternatively, you can inspect the `ABSolutions.ImageToBase64.Demo.Blazor` project in
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

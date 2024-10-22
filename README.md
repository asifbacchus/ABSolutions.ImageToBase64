# A-B Solutions: ImageToBase64 library and Blazor Component

This repository contains two (2) separate primary libraries:

1. `ABSolutions.ImageToBase64`: Retrieves an image file from the local file system or a remote source (via HTTP/S) and converts it to a Base64 string with a data URI scheme.
2. `ABSolutions.ImageToBase64.BlazorComponent`: A Blazor component that uses the `ABSolutions.ImageToBase64` library to convert an image file to a Base64 string and display it in an HTML image tag.

Usage details and release notes can be found in the README.md files of each library.

This repository also includes demo projects for each library:

- `ABSolutions.ImageToBase64.Demo.Api`: A simple ASP.NET Core Web API demonstrating the operation of the `ABSolutions.ImageToBase64` library. The API has one endpoint that returns the Base64-encoded string representation of a remote image and another endpoint that returns a webpage displaying that image.
- `ABSolutions.ImageToBase64.Demo.Blazor`: A Blazor Server (SSR) application that uses the `ABSolutions.ImageToBase64.BlazorComponent` library to display a Base64-encoded image retrieved from a remote source.

Each demo project is fully explained in the documentation of the respective library.

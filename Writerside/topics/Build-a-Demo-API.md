# Build a Demo API

The best way to understand how this library works and how to use it is to review the
`ABSolutions.ImageToBase64.Demo.Api` project. The project is a minimal API that has two endpoints of interest:

1. `GET /base64`: Returns the Base64 representation of a random remote image.
2. `GET /picture`: Retrieves a random remote image, converts it to a Base64 string, and returns a webpage displaying
   that image using the embedded Base64 string.

While I think the code is pretty easy to follow, I know some people understand better by building things themselves. So,
I'll walk you through the process of building the demo project in the the pages contained in this section. By the end, I
think you'll have a pretty solid idea of how to integrate this library into your own projects.

> This demo project is purely for educational purposes. It is not intended to be used in production. There is no error
> trapping, logging, security/authentication or any other production-level features.

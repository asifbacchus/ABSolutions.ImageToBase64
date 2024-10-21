# Default image

In the event that a component call is cancelled or fails for **any** reason, the backend `ImageToBase64` library will
return the following default image:

![DefaultImage](defaultBase64ReturnImage.png)

This image is a Base64-encoded 512x512 pixel black image of an exclamation mark surrounded by a rounded triangle.

Returning a default image prevents layout-breaking errors in the frontend due to missing images. It also provides a
visual cue that something went wrong.

> If you'd prefer to handle retrieval errors in a different way, I'd suggest directly calling the backend library in a
> code-behind section and not using this Blazor component.
> {style="warning"}

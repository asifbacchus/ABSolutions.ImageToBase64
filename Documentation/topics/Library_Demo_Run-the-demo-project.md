# Run the demo project

<link-summary>API Demo: Running the project to see how this library works.</link-summary>
<card-summary>Run the demo project to see how this library works.</card-summary>
<web-summary>ABSolutions.ImageToBase64 API demo: Running the demo project.</web-summary>

That's it! You're ready to run this demo project. Somewhat counterintuitively, I strongly recommend you access this API
using your web browser instead of a tool like *Insomnia* or *Postman*. This is because the `GET /picture` endpoint
returns an HTML page with an embedded image. Plus, using a browser will allow you to quickly confirm the image is being
displayed using an embedded Base64 string and not a link of any kind.

## Main page

When you open your browser to whatever address your launch settings are configured to use, you will be greeted with our
modified "homepage". This is not exciting. Let's check out the other endpoints.

## Base64 endpoint

Navigate to `/base64` to see the Base64 representation of a random image.

If you add `?cache=true` to the URL, you'll notice that for the next 5 minutes you only ever get the same result. This
is because the image is being cached. After 5 minutes a new request will be made upstream and the Base64 string will
change.

If you add `?error=true` to the URL, you'll notice the response is always the same -- the *default image* Base64 string.
In addition, the response will be a 404 error.

## Picture endpoint

Navigate to `/picture` and you'll see a picture displayed. This picture is being displayed from the embedded Base64
string. To verify this, open your browser's *developer tools* and check the HTML for the `img` tag. Notice the `src`
attribute is not a link or URL, it's a Base64 string!

If you add `?cache=true` to the URL, you'll notice that for the next 5 minutes you only ever get the same result. This
is because the image is being cached. After 5 minutes a new request will be made upstream and the Base64 string will
change.

If you add `?error=true` to the URL, you'll notice the response is always the same -- the *default image*. In addition,
the response will be a 404 error.

# Run the project

<link-summary>Blazor Demo: Run the sample project with the ImageToBase64 component.</link-summary>
<card-summary>Run the demo project to see how the ImageToBase64 component works.</card-summary>
<web-summary>ABSolutions.ImageToBase64 Blazor demo: Run the demo project.</web-summary>

That's it! You're ready to run this demo project.

## Main page

When you open the browser to whatever address your launch settings are configured to use, you will see the standard
Blazor demo page. Notice, however, that the menu has an entry for our *Base64 Image* page.

## The Base64 Image page

This page is the entire point of the demo project. When you navigate to it, you will see the page we made earlier. There
is a 50% chance you'll see the default warning triangle images showing that the cancellation tokens were cancelled. If
that happens, just reload the page or click on the navigation menu entry again. You may have to do this several times (
since cancellation is random, remember?) but eventually, you should see the images.

When the images load, you'll see there are actually two of them. The top one is the *non-cached* image and will update
every time the component is called. The bottom one is the *cached* version and will only change when the cache expires(
after 5 minutes). Try this out by reloading the page a few times. If you get the cancellation triangles, keep reloading
the page until you get new images. You'll see that the top one is indeed new while the bottom one is still the
previously cached image.

Finally, I suppose you should verify we are actually using Base64-encoded strings, right? Open your browser's developer
tools and inspect the two `<img>` tags.

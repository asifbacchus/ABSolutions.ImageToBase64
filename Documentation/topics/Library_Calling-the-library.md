# Calling the library

<link-summary>How to call this library to convert images to Base64 strings.</link-summary>
<card-summary>Discussion of how to call this library to convert images to Base64 strings.</card-summary>
<web-summary>How to call the ABSolutions.ImageToBase64 .NET library to convert images to Base64 strings.</web-summary>
<show-structure depth="2"/>

## Injecting the library

Since the library is registered with dependency injection, you can inject it anywhere you need it.

```c#
public class MyClass
{
    private readonly IBase64Converter _base64Image;

    public MyClass(IBase64Converter base64Image)
    {
        _base64Image = base64Image;
    }
}
```

## The GetImageAsBase64Async method

The service only has one method, `GetImageAsBase64Async`, with the following signature:

<code-block lang="c#" src="Library_libInterface.cs" include-lines="23-24"/>

| Parameter               | Explanation                                                                                                                                                                                                                               | Default                  |
|-------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------|
| filename                | The file (or relative path and file name) to retrieve.                                                                                                                                                                                    | `none`                   |
| useCache                | Whether to use the in-memory cache for this particular request. If specified (not null), this overrides the global configuration setting.                                                                                                 | `null`                   |
| noExpiry                | Set the cache entry for this request to never expire. If specified (not null), this overrides the global configuration setting.                                                                                                           | `null`                   |
| loggingCorrelationValue | The value to use for log correlation. This will be the 'value' assigned to the 'key' as defined by the `LoggingCorrelationIdentifier` in `appsettings.json`. If this is an empty string, no correlation key will be included in the logs. | `empty string`           |
| cancellationToken       | Optional cancellation token to use. If none supplied, this task will not be cancellable.                                                                                                                                                  | `CancellationToken.None` |

This is an *asynchronous* method that returns a `Task<Base64Result>`. More information about
the [Base64Result](#base64result-return-type) return type is provided later on this page.

Continuing with the previous example code where we injected the library, this is an example of how to call this method:

```c#
public class MyClass
{
    private readonly IBase64Converter _base64Image;

    public MyClass(IBase64Converter base64Image)
    {
        _base64Image = base64Image;
    }
    
    public async Task PrintBase64Image(string filename)
    {
        Console.WriteLine(await _base64Image.GetImageAsBase64Async(filename));
    }
}
```

### Default return

<link-summary>Information about the default return image.</link-summary>
If there are any problems retrieving the image or if any exceptions are thrown for whatever reason, the method will
return a default image. This allows your webpages, applications, etc. to retain their layout while still being obvious
that something has gone wrong. The default image looks like this:

![Default Image](defaultBase64ReturnImage.png){height="200" width="200"}

The image is Base64-encoded 512x512 pixel image of a black exclamation mark surrounded by a black outline of a rounded
triangle. The image has a transparent background.

### Correlation logging

If you are using the `loggingCorrelationValue` parameter, you will need to set up the `LoggingCorrelationIdentifier` in
`appsettings.json`. Assuming both these fields are populated, the log context will be updated accordingly. For example,
let's take the following scenario:

**appsettings.json**

```json
{
  "Base64Converter": {
    "LoggingCorrelationIdentifier": "TransactionId"
  }
}
```

Now, assume we call the library as follows:

```c#
Console.WriteLine(
    await _base64Image
      .GetImageAsBase64Async(filename, loggingCorrelationValue: "12345"));
```

An example JSON structured logging message would be output as follows:

```
{
  "Timestamp": "[00:40:12] ",
  "EventId": 0,
  "LogLevel": "Debug",
  "Category": "ABSolutions.ImageToBase64.Services.Base64Converter",
  "Message": "Requesting 500.webp from https://picsum.photos/",
  "State": {
    "Message": "Requesting 500.webp from https://picsum.photos/",
    "Filename": "500.webp",
    "UpstreamAssetSource": "https://picsum.photos/",
    "{OriginalFormat}": "Requesting {Filename} from {UpstreamAssetSource}"
  },
  "Scopes": [
    {
      "Message": "SpanId:ce33d07dee7278cd, TraceId:6757ec2cffe9a60e525fd9c00390dd2c, ParentId:0000000000000000",
      "SpanId": "ce33d07dee7278cd",
      "TraceId": "6757ec2cffe9a60e525fd9c00390dd2c",
      "ParentId": "0000000000000000"
    },
    {
      "Message": "ConnectionId:0HN7ICHEOUV9S",
      "ConnectionId": "0HN7ICHEOUV9S"
    },
    {
      "Message": "RequestPath:/base64 RequestId:0HN7ICHEOUV9S:00000011",
      "RequestId": "0HN7ICHEOUV9S:00000011",
      "RequestPath": "/base64"
    },
    {
      "Message": "System.Linq.Enumerable\u002BConcat2Iterator\u00601[System.Collections.Generic.KeyValuePair\u00602[System.String,System.Object]]",
      "MethodName": "GetImageAsBase64Async",
      "TransactionId": "12345",
      "ClassName": "Base64Converter"
    },
    {
      "Message": "System.Linq.Enumerable\u002BConcat2Iterator\u00601[System.Collections.Generic.KeyValuePair\u00602[System.String,System.Object]]",
      "MethodName": "GetImageFromUpstreamAsync",
      "TransactionId": "12345",
      "ClassName": "Base64Converter"
    }
  ]
}
```

Notice our `TransactionId` is now included in the log context within the relevant scopes.

> This assumes you have set-up structured logging and the output has been formatted as human-readable (indented) JSON.
> Please refer to the demo project for an example of how to do this.

## Base64Result Return type

<include from="Shared_Snippets.topic" element-id="returnResultStruct"/>

If the image retrieval and conversion to a Base64-encoded string was successful, `IsSuccess` will be `true` and the
`Base64String` will contain the Base64-encoded string. If the operation failed, `IsSuccess` will be `false` and the
`Base64String` will contain the *default* image.

> The `Base64String` property will **never** be null.

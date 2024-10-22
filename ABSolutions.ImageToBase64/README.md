# ImageToBase64: Class Library

This service either reads a local image file or uses an **injected** `IHttpClient` to read an image from a remote
system and then convert that image into a `Base64` encoded `string` with a data URI prefix.

This version implements an **in-memory** cache to reduce calls to the upstream source. The cache can be disabled as
detailed in the configuration section.

**Please refer to the documentation site below for more information**

|               |                                                                 |
|---------------|-----------------------------------------------------------------|
| Git repo      | https://github.com/asifbacchus/ABSolutions.ImageToBase64        |              |                                                                 |
| Author        | A-B Solutions (Asif Bacchus)                                    |
| Contact       | asif@a-b.solutions                                              |
| Documentation | https://a-b.solutions/documentation/nuget/ImageToBase64-Library |
| Nuget         | [ABSolutions.ImageToBase64]()                                   |

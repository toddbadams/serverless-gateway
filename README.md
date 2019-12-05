**STATUS** currently being documented, pre code devlopment.

# Azure functions serverless gateway

- [Authorization](#Authorization)
- [Correlation](#Correlation)
- [Routing](#Routing)
- [Eventing](#Eventing)
- [Tracing](#Tracing)
- [WebSockets](#WebSockets)
- [Middleware](#Middleware)
- [Configuration](#Configuration)


## Authorization

Claims based authorization using [JWT](https://jwt.io/introduction/).  The incomming authorization token has it's signature verified, and expresses all claims as headers to the downstream service.

### Incoming authorization header

``` json
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhNzZjMzcxYy01MzU0LTQ0ZjUtYTljZC01ZWNiZDQ2NGQ1ODIiLCJuYW1lIjoiSm9obiBEb2UiLCJpYXQiOjE1MTYyMzkwMjJ9.zM1cC8DaLEmP1KB0QKP8u5nL9u_sa2Z-Ce8oVsZXsag
```

this has the following payload:

``` json
{
  "sub": "a76c371c-5354-44f5-a9cd-5ecbd464d582",
  "name": "John Doe",
  "iat": 1516239022
}
```
### Downstream headers
The incomming claims are expressed as headers in the downstream request as:

``` json
x-jwt-sub: a76c371c-5354-44f5-a9cd-5ecbd464d582
x-jwt-name: John Doe
x-jwt-iat: 1516239022
```


## Correlation
Reads an incomming header `X-Request-ID` and applies that to the downstream request. If the header is absent a GUID value is generated.

```
X-Request-ID: 94836156-78ac-4b9d-9beb-01786f975f12
```

## Routing
Routes incomming requests to a downstream service. 

### Example
In this example an anonymous endpoint is created that supports `GET` method.  The incoming route is given by `wines/{wineId?}`, this allows and optional routing parameter of the wine identifer.

The downstream service configuration is given by the string array `"wines-v1", "wines-v2"`.  This then retrieves the following convention based settings from Azure Configuration Management.
- `wines-v1-route`: /api/v1/wines/{wineId?}
- `wines-v1-scheme`: https
- `wines-v1-host`: localhost
- `wines-v1-port`: 80
- `wines-v1-event`: false  // disables event logging
- `wines-v2-route`: /api/v2/wines/{wineId?}
- `wines-v2-scheme`: https
- `wines-v2-host`: localhost
- `wines-v2-port`: 80
- `wines-v2-event`: true  // enables event logging

``` csharp
    public class WinesApi
    {
        private readonly IGatewayProcessor _gatewayProcessor;

        public WinesApi(IGatewayProcessor gatewayProcessor)
        {
            this._gatewayProcessor = gatewayProcessor;
        }

        [FunctionName("wines")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "wines/{wineId?}")] HttpRequest req)
        {
           return await _gatewayProcessor.ProcessAsync(req, new []{"wines-v1", "wines-v2" });
        }
    }
```


## Eventing


## Tracing




## WebSockets




## Middleware


## Configuration
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights;

namespace Gateway.Application.Pipelines
{
    public class GatewayEvent
    {
        private readonly string _eventName;
        private readonly IDictionary<string, string> _meta;
        private Stopwatch _watch;
        private readonly TelemetryClient _telemetry;

        public GatewayEvent(string eventName, string requestBody, string requestMethod, string contentType,
            string correlationId, IDictionary<string, string> meta, TelemetryClient telemetry)
        {
            _watch = Stopwatch.StartNew();
            _meta = new Dictionary<string, string>();
            _telemetry = telemetry;
            _eventName = eventName;

            foreach (var keyValue in meta)
            {
                _meta.Add(keyValue);
            }
            _meta.Add("Method", requestMethod);
            _meta.Add("CorrelationId", correlationId);
            _meta.Add("ContentType", contentType);
            _meta.Add("RequestBody", requestBody);
        }


        public void Log(string statusCode, string content)
        {
            _watch.Stop();
            var metrics = new Dictionary<string, double> { { "Latency", _watch.ElapsedMilliseconds } };
            _meta.Add("ResponseBody", content);
            _meta.Add("StatusCode", statusCode);
            _telemetry.TrackEvent(_eventName, _meta, metrics);
        }
    }
}
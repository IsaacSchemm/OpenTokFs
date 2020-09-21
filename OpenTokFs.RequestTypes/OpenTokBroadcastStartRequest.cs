using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTokFs.RequestTypes {
    [JsonConverter(typeof(OpenTokBroadcastStartRequestPropertyConverter))]
    public class OpenTokBroadcastStartRequest {
        public string SessionId { get; }

        public OpenTokVideoLayout Layout { get; set; } = OpenTokVideoLayout.BestFit;
        public TimeSpan Duration { get; set; } = TimeSpan.FromHours(2);
        public bool Hls { get; set; } = false;
        public IEnumerable<OpenTokRtmpDestination> Rtmp { get; set; } = Enumerable.Empty<OpenTokRtmpDestination>();
        public string Resolution { get; set; } = "640x480";

        public OpenTokBroadcastStartRequest(string sessionId) {
            this.SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
        }
    }

    public class OpenTokBroadcastStartRequestPropertyConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return objectType == typeof(OpenTokBroadcastStartRequest);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (value == null) {
                serializer.Serialize(writer, null);
                return;
            }

            var obj = value as OpenTokBroadcastStartRequest;

            var outputs = new Dictionary<string, object>();
            if (obj.Hls)
                outputs.Add("hls", new object());
            outputs.Add("rtmp", obj.Rtmp);

            var dict = new Dictionary<string, object> {
                ["sessionId"] = obj.SessionId,
                ["layout"] = obj.Layout,
                ["maxDuration"] = (int)obj.Duration.TotalSeconds,
                ["outputs"] = outputs,
                ["resolution"] = obj.Resolution
            };
            serializer.Serialize(writer, dict);
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenTokFs.RequestTypes {
    [JsonConverter(typeof(OpenTokArchiveStartRequestPropertyConverter))]
    public class OpenTokArchiveStartRequest {
        public string SessionId { get; }

        public OpenTokVideoLayout Layout { get; set; } = OpenTokVideoLayout.BestFit;
        public bool HasAudio { get; set; } = true;
        public bool HasVideo { get; set; } = true;
        public string? Name { get; set; } = null;
        public string OutputMode { get; set; } = "composed";
        public string Resolution { get; set; } = "640x480";

        public OpenTokArchiveStartRequest(string sessionId) {
            this.SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
        }
    }

    public class OpenTokArchiveStartRequestPropertyConverter : JsonConverter {
        public override bool CanConvert(Type? objectType) {
            return objectType == typeof(OpenTokArchiveStartRequest);
        }

        public override object? ReadJson(JsonReader? reader, Type? objectType, object? existingValue, JsonSerializer? serializer) {
            return false;
        }

        public override void WriteJson(JsonWriter? writer, object? value, JsonSerializer? serializer) {
            if (!(value is OpenTokArchiveStartRequest obj)) {
                serializer!.Serialize(writer!, value);
                return;
            }

            var dict = new Dictionary<string, object?> {
                ["sessionId"] = obj.SessionId,
                ["hasAudio"] = obj.HasAudio,
                ["hasVideo"] = obj.HasVideo,
                ["name"] = obj.Name,
                ["outputMode"] = obj.OutputMode
            };
            if (obj.OutputMode == "composed") {
                dict.Add("layout", obj.Layout);
                dict.Add("resolution", obj.Resolution);
            }
            serializer!.Serialize(writer!, dict);
        }
    }
}

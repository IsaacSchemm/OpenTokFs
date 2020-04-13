using System;
using System.Collections.Generic;

namespace OpenTokFs.Json.ResponseTypes {
    public class OpenTokRtmpStream {
        public string Id { get; set; }
        public string ServerUrl { get; set; }
        public string StreamName { get; set; }
        public string Status { get; set; }

        public override string ToString() => $"{ServerUrl}/{StreamName} ({Id ?? "no ID"}) ({Status})";
    }

    public class OpenTokBroadcastStreams {
        public string Hls { get; set; }
        public IEnumerable<OpenTokRtmpStream> Rtmp { get; set; }

        public override string ToString() {
            return $"HLS: {Hls ?? "none"}; RTMP: [{string.Join(", ", Rtmp)}]";
        }
    }

    public class OpenTokBroadcast {
        public string Id { get; set; }
        public string SessionId { get; set; }
        public int ProjectId { get; set; }
        public long CreatedAt { get; set; }
        public OpenTokBroadcastStreams BroadcastUrls { get; set; }
        public long UpdatedAt { get; set; }
        public string Status { get; set; }
        public string Resolution { get; set; }

        public DateTimeOffset GetCreationTime() => DateTimeOffset.FromUnixTimeMilliseconds(CreatedAt);
        public DateTimeOffset GetUpdatedTime() => DateTimeOffset.FromUnixTimeMilliseconds(UpdatedAt);

        public override string ToString() => $"{Id} ({Status})";
    }
}

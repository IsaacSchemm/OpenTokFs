using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTokFs.ResponseTypes {
    public class OpenTokRtmpStream {
        public string? Id { get; set; } = null;
        public string ServerUrl { get; set; } = "";
        public string StreamName { get; set; } = "";
        public string Status { get; set; } = "";

        public override string ToString() => $"{ServerUrl}/{StreamName} ({Id ?? "no ID"}) ({Status})";
    }

    public class OpenTokBroadcastStreams {
        public string? Hls { get; set; } = null;
        public IEnumerable<OpenTokRtmpStream>? Rtmp { get; set; } = null;

        public override string ToString() {
            return $"HLS: {Hls ?? "none"}; RTMP: [{string.Join(", ", Rtmp ?? Enumerable.Empty<OpenTokRtmpStream>())}]";
        }
    }

    public class OpenTokBroadcast {
        public string Id { get; set; } = "";
        public string SessionId { get; set; } = "";
        public int ProjectId { get; set; } = 0;
        public long CreatedAt { get; set; } = 0L;
        public OpenTokBroadcastStreams BroadcastUrls { get; set; } = new OpenTokBroadcastStreams();
        public long UpdatedAt { get; set; } = 0L;
        public string Status { get; set; } = "";
        public string Resolution { get; set; } = "";

        public DateTimeOffset GetCreationTime() => DateTimeOffset.FromUnixTimeMilliseconds(CreatedAt);
        public DateTimeOffset GetUpdatedTime() => DateTimeOffset.FromUnixTimeMilliseconds(UpdatedAt);

        public override string ToString() => $"{Id} ({Status})";
    }
}

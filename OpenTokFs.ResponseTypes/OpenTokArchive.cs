using System;

namespace OpenTokFs.ResponseTypes {
    public class OpenTokArchive {
        public long CreatedAt { get; set; } = 0L;
        public int Duration { get; set; } = 0;
        public bool HasAudio { get; set; } = false;
        public bool HasVideo { get; set; } = false;
        public string Id { get; set; } = "";
        public string? Name { get; set; } = null;
        public string? OutputMode { get; set; } = null;
        public int ProjectId { get; set; } = 0;
        public string Reason { get; set; } = "";
        public string? Resolution { get; set; } = null;
        public string SessionId { get; set; } = "";
        public long Size { get; set; } = 0;
        public string Status { get; set; } = "";
        public string? Url { get; set; } = null;

        public DateTimeOffset GetCreationTime() => DateTimeOffset.FromUnixTimeMilliseconds(CreatedAt);
        public TimeSpan GetDuration() => TimeSpan.FromSeconds(Duration);

        public override string ToString() => $"{Id} ({Name ?? "no name"}) ({OutputMode}, {Status})";
    }
}

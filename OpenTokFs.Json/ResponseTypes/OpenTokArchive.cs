using System;

namespace OpenTokFs.Json.ResponseTypes {
    public class OpenTokArchive {
        public long CreatedAt { get; set; }
        public int Duration { get; set; }
        public bool HasAudio { get; set; }
        public bool HasVideo { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string OutputMode { get; set; }
        public int ProjectId { get; set; }
        public string Reason { get; set; }
        public string Resolution { get; set; }
        public string SessionId { get; set; }
        public long Size { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }

        public DateTimeOffset GetCreationTime() => DateTimeOffset.FromUnixTimeMilliseconds(CreatedAt);
        public TimeSpan GetDuration() => TimeSpan.FromSeconds(Duration);

        public override string ToString() => $"{Id} ({Name ?? "no name"}) ({OutputMode}, {Status})";
    }
}

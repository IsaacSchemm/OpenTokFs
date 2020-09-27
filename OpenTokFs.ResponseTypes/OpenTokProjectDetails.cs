using System;

namespace OpenTokFs.ResponseTypes {
    public class OpenTokProjectDetails {
        public int Id { get; set; } = 0;
        public string Secret { get; set; } = "";
        public string Status { get; set; } = "";
        public string Name { get; set; } = "";
        public string Environment { get; set; } = "";
        public long CreatedAt { get; set; } = 0L;

        public DateTimeOffset GetCreationTime() => DateTimeOffset.FromUnixTimeMilliseconds(CreatedAt);

        public override string ToString() => $"{Id} ({Name})";
    }
}

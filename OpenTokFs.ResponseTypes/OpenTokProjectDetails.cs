using OpenTokFs.Credentials;
using System;

namespace OpenTokFs.ResponseTypes {
    public class OpenTokProjectDetails : IProjectCredentials {
        public int Id { get; set; } = 0;
        public string Secret { get; set; } = "";
        public string Status { get; set; } = "";
        public string Name { get; set; } = "";
        public string Environment { get; set; } = "";
        public long CreatedAt { get; set; } = 0L;

        int IProjectCredentials.ApiKey => Id;
        string IProjectCredentials.ApiSecret => Secret;

        public DateTimeOffset GetCreationTime() => DateTimeOffset.FromUnixTimeMilliseconds(CreatedAt);

        public override string ToString() => $"{Id} ({Name})";
    }
}

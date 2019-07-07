using System;

namespace ISchemm.OpenTokFs.RequestTypes
{
    public interface IRtmpDestination
    {
        string Id { get; }
        string ServerUrl { get; }
        string StreamName { get; }
    }

    public class RtmpDestination : IRtmpDestination
    {
        public string Id { get; set; }
        public string ServerUrl { get; }
        public string StreamName { get; }

        public RtmpDestination(string serverUrl, string streamName)
        {
            this.Id = null;
            this.ServerUrl = serverUrl ?? throw new ArgumentNullException(nameof(serverUrl));
            this.StreamName = streamName ?? throw new ArgumentNullException(nameof(streamName));
        }
    }
}

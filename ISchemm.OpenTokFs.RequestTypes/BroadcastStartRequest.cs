using System;
using System.Collections.Generic;
using System.Linq;

namespace ISchemm.OpenTokFs.RequestTypes
{
    public interface IBroadcastStartRequest
    {
        string SessionId { get; }
        string LayoutType { get; }
        string LayoutStylesheet { get; }
        TimeSpan Duration { get; }
        bool Hls { get; }
        IEnumerable<IRtmpDestination> Rtmp { get; }
        string Resolution { get; }
    }

    public class BroadcastStartRequest : IBroadcastStartRequest
    {
        public string SessionId { get; }
        public string LayoutType { get; set; }
        public string LayoutStylesheet { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Hls { get; set; }
        public IEnumerable<IRtmpDestination> Rtmp { get; set; }
        public string Resolution { get; set; }

        public BroadcastStartRequest(string sessionId)
        {
            this.SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
            this.LayoutType = "bestFit";
            this.LayoutStylesheet = null;
            this.Duration = TimeSpan.FromHours(2);
            this.Hls = false;
            this.Rtmp = Enumerable.Empty<IRtmpDestination>();
            this.Resolution = "640x480";
        }
    }
}

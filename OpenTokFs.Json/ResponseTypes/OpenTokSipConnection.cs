using System;

namespace OpenTokFs.Json.ResponseTypes {
    public class OpenTokSipConnection {
        public Guid Id { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid StreamId { get; set; }
    }
}

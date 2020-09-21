using System;

namespace OpenTokFs.ResponseTypes {
    public class OpenTokSipConnection {
        public Guid Id { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid StreamId { get; set; }
    }
}

namespace OpenTokFs.Credentials {
    /// <summary>
    /// A set of project-level credentials for Vonage Video API.
    /// An implementation of this interface exists in the OpenTokFs library (OpenTokProjectCredentials).
    /// </summary>
    public interface IProjectCredentials {
        /// <summary>
        /// The project ID / API key.
        /// </summary>
        int ApiKey { get; }
        /// <summary>
        /// The API secret.
        /// </summary>
        string ApiSecret { get; }
    }
}

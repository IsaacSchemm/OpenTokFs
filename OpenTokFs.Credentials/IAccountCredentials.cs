namespace OpenTokFs.Credentials {
    /// <summary>
    /// A set of account-level credentials for Vonage Video API.
    /// An implementation of this interface exists in the OpenTokFs library (OpenTokAccountCredentials).
    /// </summary>
    public interface IAccountCredentials {
        /// <summary>
        /// The account-level API key.
        /// </summary>
        int ApiKey { get; }
        /// <summary>
        /// The account-level API secret.
        /// </summary>
        string ApiSecret { get; }
    }
}

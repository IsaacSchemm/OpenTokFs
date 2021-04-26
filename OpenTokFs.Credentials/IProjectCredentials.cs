using System;

namespace OpenTokFs.Credentials {
    /// <summary>
    /// A set of project-level credentials for Vonage Video API.
    /// You can use <seealso cref="ProjectCredentials"/> or implement this interface on your own.
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

    /// <summary>
    /// A set of project-level credentials for Vonage Video API.
    /// </summary>
    public readonly struct ProjectCredentials : IProjectCredentials, IEquatable<ProjectCredentials> {
        /// <summary>
        /// The project ID / API key.
        /// </summary>
        public int ApiKey { get; }

        /// <summary>
        /// The API secret.
        /// </summary>
        public string ApiSecret { get; }

        public ProjectCredentials(int apiKey, string apiSecret) {
            ApiKey = apiKey;
            ApiSecret = apiSecret ?? throw new ArgumentNullException(nameof(apiSecret));
        }

        public override bool Equals(object obj) =>
            obj is ProjectCredentials credentials && Equals(credentials);

        public bool Equals(ProjectCredentials other) =>
            (ApiKey, ApiSecret) == (other.ApiKey, other.ApiSecret);

        public override int GetHashCode() =>
            (ApiKey, ApiSecret).GetHashCode();

        public static bool operator ==(ProjectCredentials left, ProjectCredentials right) =>
            left.Equals(right);

        public static bool operator !=(ProjectCredentials left, ProjectCredentials right) =>
            !(left == right);
    }
}

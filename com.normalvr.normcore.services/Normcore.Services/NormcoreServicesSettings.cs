namespace Normcore.Services
{
    /// <summary>
    /// A class used to configure the Normcore Services client.
    /// </summary>
    public static class NormcoreServicesSettings
    {
        /// <summary>
        /// The base url of the public Normcore Services API.
        /// </summary>
        public const string DefaultUrl = "https://alpha.services.normcore.io";

        private static string _url;

        /// <summary>
        /// The base URL used when making API requests.
        /// </summary>
        /// <seealso cref="SetCustomUrl"/>
        public static string Url => _url ?? DefaultUrl;

        /// <summary>
        /// Override the base URL used when making API requests.
        /// </summary>
        /// <remarks>
        /// The provided base URL is assumed to be valid, and will cause issues
        /// if it is incorrectly configured.
        /// </remarks>
        /// <param name="url">The base URL to use when making API requests.
        /// Set to <see langword="null"/> to revert to the default base URL.</param>
        public static void SetCustomUrl(string url)
        {
            _url = url;
        }
    }
}

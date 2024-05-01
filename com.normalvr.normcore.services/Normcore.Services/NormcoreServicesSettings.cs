namespace Normcore.Services
{
    public static class NormcoreServicesSettings
    {
        /// <summary>
        /// The default public Normcore Services host.
        /// </summary>
        public const string DefaultHost = "https://alpha.services.normcore.io";

        /// <summary>
        /// The host (complete root API URL, including protocol) used to make service requests.
        /// </summary>
        public static string Host => customHost ?? DefaultHost;

        /// <summary>
        /// The custom host, if any.
        /// </summary>
        private static string customHost;

        /// <summary>
        /// Override the default Normcore Services host.
        /// </summary>
        /// <remarks>
        /// This can cause issues if the custom host is incorrectly configured.
        /// </remarks>
        public static void SetCustomHost(string host)
        {
            customHost = host;
        }
    }
}

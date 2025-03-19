namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when an API request fails because a connection with the server cannot be established.
    /// </summary>
    public sealed class ConnectionException : RequestException
    {
        internal ConnectionException(in RequestInfo request) : base(request, "Unable to connect to Normcore Services backend.")
        {
        }
    }
}

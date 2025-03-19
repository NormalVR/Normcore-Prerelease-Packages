using System.Net;

using Newtonsoft.Json;

namespace Normcore.Services
{
    /// <summary>
    /// An exception raised when a Normcore Services API request fails because the
    /// request contained invalid parameters or is missing required parameters.
    /// </summary>
    /// <seealso cref="ErrorCode.InvalidSchema"/>
    public sealed class InvalidSchemaException : ErrorResponseException
    {
        /// <summary>
        /// The validation issues detected while processing the request.
        /// </summary>
        public ValidationIssue[] Issues { get; }

        internal InvalidSchemaException(in RequestInfo requestInfo, HttpStatusCode status, ValidationError error)
            : base(requestInfo, status, error, JsonConvert.SerializeObject(error.Data, Formatting.Indented))
        {
            Issues = error.Data.Issues;
        }
    }
}

using System;

namespace Normcore.Services
{
    public static class Validation
    {
        // Only up to three parameters are provided because passing more than three formatting
        // arguments to string.Format will use the allocating params variant. If we add an
        // endpoint with more than three parameters, we'll need to revisit this.

        public static ValidatedPath FormatPath(string path)
        {
            return new ValidatedPath { Value = path }; // without any parameters we trust that the path is safe to use
        }

        public static ValidatedPath FormatPath(string path, string a)
        {
            a = EscapePathParameter(a);

            return new ValidatedPath { Value = string.Format(path, a) };
        }

        public static ValidatedPath FormatPath(string path, string a, string b)
        {
            a = EscapePathParameter(a);
            b = EscapePathParameter(b);

            return new ValidatedPath { Value = string.Format(path, a, b) };
        }

        public static ValidatedPath FormatPath(string path, string a, string b, string c)
        {
            a = EscapePathParameter(a);
            b = EscapePathParameter(b);
            c = EscapePathParameter(c);

            return new ValidatedPath { Value = string.Format(path, a, b, c) };
        }

        private static string EscapePathParameter(string param)
        {
            try
            {
                return Uri.EscapeDataString(param); // only allocates if escaping occurs
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to escape path parameter \"{param}\"", ex);
            }
        }
    }

    /// <summary>
    /// An endpoint path which has passed through validation and is safe to request.
    /// </summary>
    public struct ValidatedPath
    {
        public string Value;
    }
}

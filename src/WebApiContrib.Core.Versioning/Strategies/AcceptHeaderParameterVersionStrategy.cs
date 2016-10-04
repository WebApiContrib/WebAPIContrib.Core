using System;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// This version strategy gets its version from the <c>Accept</c>-header, using a parameter.
    /// The parameter name can be configured using the <see cref="ParameterName"/> property.
    /// </summary>
    public class AcceptHeaderParameterVersionStrategy : AcceptHeaderVersionStrategy
    {
        /// <summary>
        /// Gets or sets the parameter name to use for determining the version.
        /// </summary>
        /// <remarks>
        /// The default value is <c>version</c>.
        /// </remarks>
        public string ParameterName { get; set; } = "version";

        /// <inheritdoc />
        protected override int? GetVersion(MediaTypeHeaderValue acceptHeader)
        {
            if (acceptHeader == null)
            {
                throw new ArgumentNullException(nameof(acceptHeader));
            }

            var subType = StripSuffix(acceptHeader.SubType);

            if (string.IsNullOrEmpty(subType))
            {
                return null;
            }

            int version;
            if (TryGetParameterVersion(acceptHeader, ParameterName, out version))
            {
                return version;
            }

            return null;
        }

        private static bool TryGetParameterVersion(MediaTypeHeaderValue acceptHeader, string parameterName, out int version)
        {
            foreach (var parameter in acceptHeader.Parameters)
            {
                if (parameter.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase))
                {
                    if (ParsingUtility.TryParseVersion(parameter.Value, out version))
                    {
                        return true;
                    }
                }
            }

            version = 0;
            return false;
        }
    }
}
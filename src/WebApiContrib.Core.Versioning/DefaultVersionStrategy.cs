using System;
using System.Linq;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// <para>
    /// The default versioning strategy.
    /// </para>
    /// <para>
    /// This strategy gets the version number from the <c>Accept</c> header in one of two ways:
    /// 1. Using a <c>version</c> parameter; <c>application/vnd.my-app; version=2</c>
    /// 2. Using a facet-based sub-type with the version as the last facet; <c>application/vnd.my-app.v2</c>
    /// </para>
    /// </summary>
    public class DefaultVersionStrategy : AcceptHeaderVersionStrategy
    {
        private static readonly char[] DotSeparator = {'.'};

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

            int? version;

            if (TryGetParameterVersion(acceptHeader, ParameterName, out version))
            {
                return version;
            }

            if (TryGetFacetVersion(subType, out version))
            {
                return version;
            }

            return null;
        }

        private static string StripSuffix(string subType)
        {
            var suffixSeparatorIndex = subType.IndexOf('+');

            if (suffixSeparatorIndex >= 0)
            {
                return subType.Substring(0, suffixSeparatorIndex);
            }

            return subType;
        }

        private static bool TryGetParameterVersion(MediaTypeHeaderValue acceptHeader, string parameterName, out int? version)
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

            version = null;
            return false;
        }

        private static bool TryGetFacetVersion(string subType, out int? version)
        {
            var facetSeparatorIndex = subType.IndexOf('.');

            if (facetSeparatorIndex >= 0)
            {
                var facets = subType.Split(DotSeparator).Reverse();

                foreach (var facet in facets)
                {
                    if (ParsingUtility.TryParseVersion(facet, out version))
                    {
                        return true;
                    }
                }
            }

            version = null;
            return false;
        }
    }
}
using System;
using System.Linq;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// This version strategy gets its version from the <c>Accept</c>-header, using a faceted sub-type.
    /// </summary>
    public class AcceptHeaderFacetVersionStrategy : AcceptHeaderVersionStrategy
    {
        private static readonly char[] DotSeparator = {'.'};

        /// <inheritdoc />
        protected override int? GetVersion(MediaTypeHeaderValue acceptHeader)
        {
            if (acceptHeader == null)
            {
                throw new ArgumentNullException(nameof(acceptHeader));
            }

            var subType = StripSuffix(acceptHeader.SubType);

            if (!subType.HasValue)
            {
                return null;
            }

            int version;
            if (TryGetFacetVersion(subType, out version))
            {
                return version;
            }

            return null;
        }

        private static bool TryGetFacetVersion(StringSegment subType, out int version)
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

            version = 0;
            return false;
        }
    }
}

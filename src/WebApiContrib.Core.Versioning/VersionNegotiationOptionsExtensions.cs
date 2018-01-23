using System;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// Various extension methods for adding and configuring version negotiation strategies.
    /// </summary>
    public static class VersionNegotiationOptionsExtensions
    {
        /// <summary>
        /// Adds version negotiation based on the default <c>Accept</c>-header parameter.
        /// </summary>
        public static VersionNegotiationOptions UseAcceptHeaderParameterStrategy(this VersionNegotiationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.UseStrategy<AcceptHeaderParameterVersionStrategy>();
        }

        /// <summary>
        /// Adds version negotiation based on the specified <c>Accept</c>-header <paramref name="parameterName"/>.
        /// </summary>
        public static VersionNegotiationOptions UseAcceptHeaderParameterStrategy(this VersionNegotiationOptions options, string parameterName)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (parameterName == null)
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            if (parameterName.Length == 0)
            {
                throw new ArgumentException($"{nameof(parameterName)} must not be empty.", nameof(parameterName));
            }

            return options.UseStrategy<AcceptHeaderParameterVersionStrategy>(x => x.ParameterName = parameterName);
        }

        /// <summary>
        /// Adds version negotiation based on an <c>Accept</c>-header facet, i.e. vnd.github.v3.
        /// </summary>
        public static VersionNegotiationOptions UseAcceptHeaderFacetStrategy(this VersionNegotiationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.UseStrategy<AcceptHeaderFacetVersionStrategy>();
        }

        /// <summary>
        /// Adds version negotiation based on the default route value.
        /// </summary>
        public static VersionNegotiationOptions UseRouteValueStrategy(this VersionNegotiationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.UseStrategy<RouteValueVersionStrategy>();
        }

        /// <summary>
        /// Adds version negotiation based on the specified <paramref name="routeValueKey"/>.
        /// </summary>
        public static VersionNegotiationOptions UseRouteValueStrategy(this VersionNegotiationOptions options, string routeValueKey)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (routeValueKey == null)
            {
                throw new ArgumentNullException(nameof(routeValueKey));
            }

            if (routeValueKey.Length == 0)
            {
                throw new ArgumentException($"{nameof(routeValueKey)} must not be empty.", nameof(routeValueKey));
            }

            return options.UseStrategy<RouteValueVersionStrategy>(x => x.RouteValueKey = routeValueKey);
        }

        /// <summary>
        /// Adds version negotiation based on the default custom header.
        /// </summary>
        public static VersionNegotiationOptions UseCustomHeaderStrategy(this VersionNegotiationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.UseStrategy<CustomHeaderVersionStrategy>();
        }

        /// <summary>
        /// Adds version negotiation based on the specified custom <paramref name="headerName"/>.
        /// </summary>
        public static VersionNegotiationOptions UseCustomHeaderStrategy(this VersionNegotiationOptions options, string headerName)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (headerName == null)
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (headerName.Length == 0)
            {
                throw new ArgumentException($"{nameof(headerName)} must not be empty.", nameof(headerName));
            }

            return options.UseStrategy<CustomHeaderVersionStrategy>(x => x.HeaderName = headerName);
        }

        /// <summary>
        /// Adds version negotiation based on the default query string parameter.
        /// </summary>
        public static VersionNegotiationOptions UseQueryStringParameter(this VersionNegotiationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.UseStrategy<QueryStringVersionStrategy>();
        }

        /// <summary>
        /// Adds version negotiation based on the specified query string <paramref name="parameterName"/>.
        /// </summary>
        public static VersionNegotiationOptions UseQueryStringParameter(this VersionNegotiationOptions options, string parameterName)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (parameterName == null)
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            if (parameterName.Length == 0)
            {
                throw new ArgumentException($"{nameof(parameterName)} must not be empty.", nameof(parameterName));
            }

            return options.UseStrategy<QueryStringVersionStrategy>(x => x.ParameterName = parameterName);
        }
    }
}

using System;

namespace WebApiContrib.Core.Versioning
{
    public static class VersionNegotiationOptionsExtensions
    {
        public static VersionNegotiationOptions UseAcceptHeaderParameterStrategy(this VersionNegotiationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.UseStrategy<AcceptHeaderParameterVersionStrategy>();
        }

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

        public static VersionNegotiationOptions UseAcceptHeaderFacetStrategy(this VersionNegotiationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.UseStrategy<AcceptHeaderFacetVersionStrategy>();
        }

        public static VersionNegotiationOptions UseRouteValueStrategy(this VersionNegotiationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.UseStrategy<RouteValueVersionStrategy>();
        }

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

        public static VersionNegotiationOptions UseCustomHeaderStrategy(this VersionNegotiationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.UseStrategy<CustomHeaderVersionStrategy>();
        }

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

        public static VersionNegotiationOptions UseQueryStringParameter(this VersionNegotiationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return options.UseStrategy<QueryStringVersionStrategy>();
        }

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
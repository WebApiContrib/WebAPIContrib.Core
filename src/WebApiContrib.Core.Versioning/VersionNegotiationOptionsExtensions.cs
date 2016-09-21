namespace WebApiContrib.Core.Versioning
{
    public static class VersionNegotiationOptionsExtensions
    {
        public static VersionNegotiationOptions UseAcceptHeaderParameterStrategy(this VersionNegotiationOptions options)
        {
            return options.UseStrategy<AcceptHeaderParameterVersionStrategy>();
        }

        public static VersionNegotiationOptions UseAcceptHeaderParameterStrategy(this VersionNegotiationOptions options, string parameterName)
        {
            return options.UseStrategy<AcceptHeaderParameterVersionStrategy>(x => x.ParameterName = parameterName);
        }

        public static VersionNegotiationOptions UseAcceptHeaderFacetStrategy(this VersionNegotiationOptions options)
        {
            return options.UseStrategy<AcceptHeaderFacetVersionStrategy>();
        }

        public static VersionNegotiationOptions UseRouteValueStrategy(this VersionNegotiationOptions options)
        {
            return options.UseStrategy<RouteValueVersionStrategy>();
        }

        public static VersionNegotiationOptions UseRouteValueStrategy(this VersionNegotiationOptions options, string routeValueKey)
        {
            return options.UseStrategy<RouteValueVersionStrategy>(x => x.RouteValueKey = routeValueKey);
        }

        public static VersionNegotiationOptions UseCustomHeaderStrategy(this VersionNegotiationOptions options)
        {
            return options.UseStrategy<CustomHeaderVersionStrategy>();
        }

        public static VersionNegotiationOptions UseCustomHeaderStrategy(this VersionNegotiationOptions options, string headerName)
        {
            return options.UseStrategy<CustomHeaderVersionStrategy>(x => x.HeaderName = headerName);
        }
    }
}
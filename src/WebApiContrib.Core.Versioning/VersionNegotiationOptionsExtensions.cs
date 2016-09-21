namespace WebApiContrib.Core.Versioning
{
    public static class VersionNegotiationOptionsExtensions
    {
        public static VersionNegotiationOptions UseDefaultStrategy(this VersionNegotiationOptions options)
        {
            return options.UseStrategy<DefaultVersionStrategy>();
        }

        public static VersionNegotiationOptions UseDefaultStrategy(this VersionNegotiationOptions options, string parameterName)
        {
            return options.UseStrategy<DefaultVersionStrategy>(x => x.ParameterName = parameterName);
        }

        public static VersionNegotiationOptions UseRouteValueVersionStrategy(this VersionNegotiationOptions options)
        {
            return options.UseStrategy<RouteValueVersionStrategy>();
        }

        public static VersionNegotiationOptions UseRouteValueVersionStrategy(this VersionNegotiationOptions options, string routeValueKey)
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
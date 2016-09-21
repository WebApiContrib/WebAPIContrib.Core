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

        public static VersionNegotiationOptions UseRouteDataStrategy(this VersionNegotiationOptions options)
        {
            return options.UseStrategy<RouteDataVersionStrategy>();
        }

        public static VersionNegotiationOptions UseRouteDataStrategy(this VersionNegotiationOptions options, string routeValueKey)
        {
            return options.UseStrategy<RouteDataVersionStrategy>(x => x.RouteValueKey = routeValueKey);
        }
    }
}
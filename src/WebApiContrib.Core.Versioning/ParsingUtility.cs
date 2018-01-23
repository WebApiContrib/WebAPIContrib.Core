using Microsoft.Extensions.Primitives;

namespace WebApiContrib.Core.Versioning
{
    internal static class ParsingUtility
    {
        public static bool TryParseVersion(StringSegment value, out int version)
        {
            if (!value.HasValue)
            {
                version = 0;
                return false;
            }

            if (value[0] == 'v' || value[0] == 'V')
            {
                value = value.Subsegment(1);
            }

            int intVersion;
            if (int.TryParse(value.Value, out intVersion))
            {
                version = intVersion;
                return true;
            }

            version = 0;
            return false;
        }
    }
}
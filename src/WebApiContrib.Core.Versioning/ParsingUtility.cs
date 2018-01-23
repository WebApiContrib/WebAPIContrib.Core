using Microsoft.Extensions.Primitives;

namespace WebApiContrib.Core.Versioning
{
    internal static class ParsingUtility
    {
        public static bool TryParseVersion(StringSegment value, out int version)
        {
            if (value.HasValue)
            {
                if (value[0] == 'v' || value[0] == 'V')
                {
                    value = value.Subsegment(1);
                }

                if (int.TryParse(value.Value, out version))
                {
                    return true;
                }
            }

            version = 0;
            return false;
        }
    }
}

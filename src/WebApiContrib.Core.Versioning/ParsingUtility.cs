namespace WebApiContrib.Core.Versioning
{
    internal static class ParsingUtility
    {
        public static bool TryParseVersion(string value, out int version)
        {
            if (string.IsNullOrEmpty(value))
            {
                version = 0;
                return false;
            }

            if (value[0] == 'v' || value[0] == 'V')
            {
                value = value.Substring(1);
            }

            int intVersion;
            if (int.TryParse(value, out intVersion))
            {
                version = intVersion;
                return true;
            }

            version = 0;
            return false;
        }
    }
}
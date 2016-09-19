namespace WebApiContrib.Core.Versioning
{
    internal static class ParsingUtility
    {
        public static bool TryParseVersion(string value, out int? version)
        {
            if (string.IsNullOrEmpty(value))
            {
                version = null;
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

            version = null;
            return false;
        }
    }
}
namespace WebApiContrib.Core.Versioning
{
    public struct VersionResult
    {
        public int Version { get; }

        public string VaryOn { get; }

        public VersionResult(int version) : this(version, null)
        {
        }

        public VersionResult(int version, string varyOn)
        {
            Version = version;
            VaryOn = varyOn ?? string.Empty;
        }
    }
}
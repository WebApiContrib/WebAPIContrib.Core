namespace WebApiContrib.Core.Versioning
{
    public class VersionContext
    {
        public int? Version { get; }
        public string VaryOn { get; }

        public VersionContext(int? version) : this(version, null)
        {
        }

        public VersionContext(int? version, string varyOn)
        {
            Version = version;
            VaryOn = varyOn;
        }
    }
}
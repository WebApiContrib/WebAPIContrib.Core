namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// Represents a matched version number and potential <c>Vary</c>-header values.
    /// </summary>
    public struct VersionResult
    {
        /// <summary>
        /// The matched version number.
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Optional value for the <c>Vary</c>-header.
        /// </summary>
        public string VaryOn { get; }

        /// <summary>
        /// Creates an instance of <see cref="VersionResult"/> using the specified version number.
        /// </summary>
        /// <param name="version">The matched version number.</param>
        public VersionResult(int version) : this(version, null)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="VersionResult"/> using the
        /// specified version number and <c>Vary</c>-header value.
        /// </summary>
        /// <param name="version">The matched version number.</param>
        /// <param name="varyOn">The <c>Vary</c>-header value.</param>
        public VersionResult(int version, string varyOn)
        {
            Version = version;
            VaryOn = varyOn ?? string.Empty;
        }
    }
}

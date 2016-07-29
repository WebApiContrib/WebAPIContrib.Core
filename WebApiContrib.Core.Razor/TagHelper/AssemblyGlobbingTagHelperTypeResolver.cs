using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;

namespace WebApiContrib.Core.Razor.TagHelper
{
    public class AssemblyNameGlobbingTagHelperTypeResolver : ITagHelperTypeResolver
    {
        protected TagHelperFeature Feature { get; }

        public AssemblyNameGlobbingTagHelperTypeResolver(ApplicationPartManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            Feature = new TagHelperFeature();
            manager.PopulateFeature(Feature);
        }

        public IEnumerable<Type> Resolve(
            string name,
            SourceLocation documentLocation,
            ErrorSink errorSink)
        {
            if (errorSink == null)
            {
                throw new ArgumentNullException(nameof(errorSink));
            }

            if (string.IsNullOrEmpty(name))
            {
                var errorLength = name == null ? 1 : Math.Max(name.Length, 1);
                errorSink.OnError(
                    documentLocation,
                    "Tag Helper Assembly Name Cannot Be Empty Or Null",
                    errorLength);

                return Type.EmptyTypes;
            }


            IEnumerable<TypeInfo> libraryTypes;
            try
            {
                libraryTypes = GetExportedTypes(name);
            }
            catch (Exception ex)
            {
                errorSink.OnError(
                    documentLocation,
                    $"Cannot Resolve Tag Helper Assembly: {name}, {ex.Message}",
                    name.Length);

                return Type.EmptyTypes;
            }

            return libraryTypes.Select(a => a.AsType());

        }

        protected IEnumerable<System.Reflection.TypeInfo> GetExportedTypes(string assemblyNamePattern)
        {
            if (assemblyNamePattern == null)
            {
                throw new ArgumentNullException(nameof(assemblyNamePattern));
            }

            var results = new List<System.Reflection.TypeInfo>();
            bool isGlobbingPattern = assemblyNamePattern.Contains("*");

            for (var i = 0; i < Feature.TagHelpers.Count; i++)
            {
                var tagHelperAssemblyName = Feature.TagHelpers[i].Assembly.GetName();
                if (isGlobbingPattern)
                {
                    if (MatchesGlob(tagHelperAssemblyName.Name, assemblyNamePattern))
                    {
                        results.Add(Feature.TagHelpers[i]);
                        continue;
                    }
                }

                // not a pattern so treat as normal assembly name.
                var assyName = new AssemblyName(assemblyNamePattern);
                if (AssemblyNameComparer.OrdinalIgnoreCase.Equals(tagHelperAssemblyName, assyName))
                {
                    results.Add(Feature.TagHelpers[i]);
                    continue;
                }
            }

            return results;
        }

        public static bool MatchesGlob(string evaluateString, string pattern)
        {
            return new Regex("^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline).IsMatch(evaluateString);
        }

        private class AssemblyNameComparer : IEqualityComparer<AssemblyName>
        {
            public static readonly IEqualityComparer<AssemblyName> OrdinalIgnoreCase = new AssemblyNameComparer();

            private AssemblyNameComparer()
            {
            }

            public bool Equals(AssemblyName x, AssemblyName y)
            {
                // Ignore case because that's what Assembly.Load does.
                return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase) &&
                       string.Equals(x.CultureName ?? string.Empty, y.CultureName ?? string.Empty, StringComparison.Ordinal);
            }

            public int GetHashCode(AssemblyName obj)
            {
                var hashCode = 0;
                if (obj.Name != null)
                {
                    hashCode ^= obj.Name.GetHashCode();
                }

                hashCode ^= (obj.CultureName ?? string.Empty).GetHashCode();
                return hashCode;
            }
        }

    }
}

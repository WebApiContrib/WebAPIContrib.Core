using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiContrib.Core.Filters
{
    public class OverrideFilter : Attribute, IFilterMetadata
    {
        public OverrideFilter()
        {
        }

        public OverrideFilter(Type type)
        {
            Type = type;
        }

        public Type Type { get; set; }
    }
}
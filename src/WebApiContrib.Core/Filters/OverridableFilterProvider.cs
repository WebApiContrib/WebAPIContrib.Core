using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiContrib.Core.Filters
{
    public class OverridableFilterProvider : IFilterProvider
    {
        public void OnProvidersExecuting(FilterProviderContext context)
        {
            if (context.ActionContext.ActionDescriptor.FilterDescriptors != null)
            {
                var overrideFilters = context.Results.Where(filterItem => filterItem.Filter is OverrideFilter).ToArray();

                if (overrideFilters.Length > 0)
                {
                    for (var i = context.Results.Count - 1; i >= 0; i--)
                    {
                        foreach (var overrideFilter in overrideFilters)
                        {
                            if (context.Results[i].Descriptor.Filter.GetType() ==
                                ((OverrideFilter)overrideFilter.Filter).Type &&
                                context.Results[i].Descriptor.Scope <= overrideFilter.Descriptor.Scope)
                            {
                                context.Results.Remove(context.Results[i]);
                            }
                        }
                    }
                }
            }
        }

        public void OnProvidersExecuted(FilterProviderContext context)
        {
        }

        //all framework providers have negative orders at -1000, so ours will come later, 
        //but before user providers which will likely by 0 or higher
        public int Order => -900;
    }
}

using Splg.Core.Attributes;
using System.Web;
using System.Web.Mvc;

namespace Splg
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // filters.Add(new HandleErrorAttribute());
            filters.Add(new ErrorLoggerAttribute());
            filters.Add(new AppActionLogFilterAttribute());
        }
    }
}

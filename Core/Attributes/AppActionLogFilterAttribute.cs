using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Core.Providers;
using Splg.Core.Constant;

namespace Splg.Core.Attributes
{
    public class AppActionLogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //親ビュー判定
            if (filterContext.ParentActionViewContext == null)
            {
                var controllerLoggingProvider = new ControllerLoggingProvider();

                controllerLoggingProvider.WriteLog(filterContext, ControllerLoggingConst.LoggingType.AppActionLog);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
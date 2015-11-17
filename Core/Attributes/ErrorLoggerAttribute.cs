#region (c) 2015 Prime Labo - All rights reserved
/*                                      COPYRIGHT NOTICE
 * -------------------------------------------------------------------------------------
 * All materials (including but not limited to source code, compiled assemblies, images,
 * resources, etc.) are copyrighted to Prime Labo. No usage is allowed unless permitted 
 * by written consent. You may not use, reverse-engineer these materials under any
 * circumstances.
 * 
 *                                    PROJECT DESCRIPTION
 * -------------------------------------------------------------------------------------
 * Namespace	: Splg.Utilities
 * Class		: ErrorLoggerAttribute
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Splg.Core.Providers;
using Splg.Core.Constant;

namespace Splg.Core.Attributes
{
    public class ErrorLoggerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var controllerLoggingProvider = new ControllerLoggingProvider();

            controllerLoggingProvider.WriteLog(filterContext, ControllerLoggingConst.LoggingType.ExceptionLog);

            base.OnException(filterContext);
        }
    }
}
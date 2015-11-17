using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Core.Constant;
using Splg.Core.Models.Logging.Dto;
using Splg.Core.Extend;

namespace Splg.Core.Services
{
    public class ExceptionLogService : ILoggingService
    {
        /// <summary>
        /// ログ書き込み
        /// </summary>
        public void WriteLog(ControllerContext filterContext)
        {
            var writeLogFileService = new WriteLogFileService(filterContext, WriteLogFileConst.ExceptionLoggerName);

            var exceptionContext = (ExceptionContext)filterContext;

            //例外書き込み用
            var exceptionLogModel = new ExceptionLogModel() 
            {
                MemberId = filterContext.HttpContext.Session["CurrentUser"].GetNullableLong(),
                ControllerName = filterContext.RouteData.Values["controller"].GetString(),
                ActionName = filterContext.RouteData.Values["action"].GetString(),
                Url = filterContext.HttpContext.Request.Url.UriString(),
                UserAgent = filterContext.HttpContext.Request.UserAgent,
                UrlReferrer = filterContext.HttpContext.Request.UrlReferrer.UriString(),
                SessionId = filterContext.HttpContext.Session.SessionID,
            };

            writeLogFileService.Fatal(exceptionContext.Exception, exceptionLogModel);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using System.Web.Mvc;
using Splg.Core.Models.Logging.Dto;
using Splg.Core.Models.Logging.Resources;

namespace Splg.Core.Services
{

    public class WriteLogFileService
    {
        private ControllerContext ControllerContext { get; set; }

        private Logger Logger{get;set;}

        #region constructor
        public WriteLogFileService(ControllerContext controllerContext, string loggerName)
        {
            ControllerContext = controllerContext;

            Logger = NLog.LogManager.GetLogger(loggerName);
        }

        public WriteLogFileService(string loggerName)
        {
            Logger = NLog.LogManager.GetLogger(loggerName);
        }
        #endregion

        /// <summary>
        /// 例外書込
        /// </summary>
        public void Fatal(Exception exception, ExceptionLogModel exceptionLogModel)
        {
            var exeptionLogParameter = new List<object>();

            exeptionLogParameter.Add(exceptionLogModel.MemberId);
            
            exeptionLogParameter.Add(exceptionLogModel.ControllerName);
            
            exeptionLogParameter.Add(exceptionLogModel.ActionName);
            
            exeptionLogParameter.Add(exceptionLogModel.Url);
            
            exeptionLogParameter.Add(exceptionLogModel.UserAgent);
            
            exeptionLogParameter.Add(exceptionLogModel.UrlReferrer);
            
            exeptionLogParameter.Add(exceptionLogModel.SessionId);

            exeptionLogParameter.Add(exception);

            exeptionLogParameter.Add(exception.InnerException);

            Logger.Fatal(exception, LoggingResource.ExceptionLogTemplete, exeptionLogParameter.ToArray());   
        }

        /// <summary>
        /// 例外書込
        /// </summary>
        public void Fatal(Exception exception, ExceptionLogModel exceptionLogModel, int httpStatusCode)
        {
            var applicationErrorLogParameter = new List<object>();

            applicationErrorLogParameter.Add(exceptionLogModel.MemberId);

            applicationErrorLogParameter.Add(httpStatusCode);

            applicationErrorLogParameter.Add(exceptionLogModel.Url);

            applicationErrorLogParameter.Add(exceptionLogModel.UserAgent);

            applicationErrorLogParameter.Add(exceptionLogModel.UrlReferrer);

            applicationErrorLogParameter.Add(exceptionLogModel.SessionId);

            applicationErrorLogParameter.Add(exception);

            applicationErrorLogParameter.Add(exception.InnerException);

            Logger.Fatal(exception, LoggingResource.ApplicationErrorLogTemplete, applicationErrorLogParameter.ToArray());   
        }

        /// <summary>
        /// 例外書込
        /// </summary>
        public void Fatal(string Message)
        {
            Logger.Fatal(Message);
        }
    }
}
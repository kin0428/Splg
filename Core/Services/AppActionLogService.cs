using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Models;
using Splg.Core.Models.Logging;
using Splg.Core.Extend;

namespace Splg.Core.Services
{
    public class AppActionLogService : ILoggingService
    {
        /// <summary>
        /// ログ書き込み
        /// </summary>
        public void WriteLog(ControllerContext filterContext)
        {
            //Todo:Importしたい
            var comEntities = new ComEntities();

            using (var dbContextTransaction = comEntities.Database.BeginTransaction())
            {
                try
                {
                    var appActionLog = new AppActionLog()
                    {
                        MemberId = filterContext.HttpContext.Session["CurrentUser"].GetNullableLong(),
                        ControllerName =  filterContext.RouteData.Values["controller"].GetString(),
                        ActionName = filterContext.RouteData.Values["action"].GetString(),
                        Url = filterContext.HttpContext.Request.Url.UriString(),
                        UserAgent = filterContext.HttpContext.Request.UserAgent,
                        UrlReferrer = filterContext.HttpContext.Request.UrlReferrer.UriString(),   
                        SessionId = filterContext.HttpContext.Session.SessionID,
                        Description = null,
                        CreateDate = DateTime.Now
                    };

                    var loggingDao = new LoggingDao(comEntities);

                    loggingDao.CreateAppActionLog(appActionLog);
          
                    comEntities.SaveChanges();

                    dbContextTransaction.Commit();
                }
                finally
                {
                    if (dbContextTransaction.UnderlyingTransaction.Connection != null && 
                        dbContextTransaction.UnderlyingTransaction.Connection.State == System.Data.ConnectionState.Open)
                    {
                        dbContextTransaction.Rollback();                    
                    }
                }
            }
        }
    }
}
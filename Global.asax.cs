using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using System.Text.RegularExpressions;
using Splg.Controllers;
using System.Web.Security;
using Splg.Models;
using Splg.Core.Services;
using Splg.Core.Constant;
using Splg.Core.Models.Logging.Dto;
using Splg.Core.Extend;
using System.Net;

namespace Splg
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private string memberId = string.Empty;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.AddGenericMobile<RazorViewEngine>();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["CurrentUser"] == null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[Constants.AUTH_COOKIE];
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(cookie.Value))
                    {
                        var memberId = MyCookie.Base64Decode(cookie.Value);
                        HttpContext.Current.Session.Add("CurrentUser", memberId);
                        cookie.Expires = DateTime.Now.AddDays(90);
                        Response.Cookies.Set(cookie);
                    }
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var CookieName = "splgub";
            
            if(HttpContext.Current.Request.Cookies[CookieName] == null)
            {
                HttpCookie myCookie = new HttpCookie(CookieName);
                string newGuid = Guid.NewGuid().ToString();
                myCookie.Value = newGuid;
                myCookie.Expires = DateTime.MaxValue;
                HttpContext.Current.Response.Cookies.Add(myCookie);

                using (var comEntities = new ComEntities())
                {
                    var unitBrowser = new UnitBrowser();
                    unitBrowser.BrowserID = HttpContext.Current.Request.Browser.Id;
                    unitBrowser.BrowserType = HttpContext.Current.Request.Browser.Type;
                    unitBrowser.CookieGuid = newGuid;
                    unitBrowser.UserAgent = HttpContext.Current.Request.UserAgent.ToString();
                    unitBrowser.CreatedDate = DateTime.Now;
                    comEntities.UnitBrowser.Add(unitBrowser);
                    comEntities.SaveChanges();
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            Response.Clear();

            var writeLogFileService = new WriteLogFileService(WriteLogFileConst.ExceptionLoggerName);

            int httpStatusCode = 0;

            if (HttpContext.Current.Session == null)
            {
                httpStatusCode = 500;

                writeLogFileService.Fatal("HttpContext.Current.Session is Null.");
            }
            else if (exception == null)
            {
                httpStatusCode = 500;

                writeLogFileService.Fatal("ExceptionObject is Null.");
            }
            else
            {
                //例外書き込み用
                var exceptionLogModel = new ExceptionLogModel()
                {
                    MemberId = HttpContext.Current.Session["CurrentUser"].GetNullableLong(),
                    Url = HttpContext.Current.Request.Url.UriString(),
                    UserAgent = HttpContext.Current.Request.UserAgent,
                    UrlReferrer = HttpContext.Current.Request.UrlReferrer.UriString(),
                    SessionId = HttpContext.Current.Session.SessionID,
                };

                httpStatusCode = exception.GetHttpCode();

                writeLogFileService.Fatal(exception, exceptionLogModel, httpStatusCode);
            }

            var action = string.Empty;

            switch (httpStatusCode)
            {
                case 404:
                    action = "Error404";
                    break;
                default:
                    action = "Error500";
                    break;
            }

            Server.ClearError();

            //Todo:エラー用の設定をWeb.configでやるべき
            Response.Redirect(String.Format("~/errorpage/{0}/", action));
        }
    }
}

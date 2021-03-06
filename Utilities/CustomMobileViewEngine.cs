﻿#region (c) 2015 Prime Labo - All rights reserved
/*                                      COPYRIGHT NOTICE
 * -------------------------------------------------------------------------------------
 * All materials (including but not limited to source code, compiled assemblies, images,
 * resources, etc.) are copyrighted to Prime Labo. No usage is allowed unless permitted 
 * by written consent. You may not use, reverse-engineer these materials under any
 * circumstances.
 * 
 *                                    PROJECT DESCRIPTION
 * -------------------------------------------------------------------------------------
 * Namespace	: Splg
 * Class		: CustomMobileViewEngine
 * Developer	: Nguyen Minh Kiet
 * 
 */
#endregion

using Splg.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Splg
{
    public class CustomMobileViewEngine : IViewEngine
    {        
        public IViewEngine BaseViewEngine { get; private set; }
        public Func<ControllerContext, bool> IsTheRightDevice { get; private set; }
        public string PathToSearch { get; private set; }

        public CustomMobileViewEngine(Func<ControllerContext, bool> isTheRightDevice, string pathToSearch, IViewEngine baseViewEngine)
        {
            BaseViewEngine = baseViewEngine;
            IsTheRightDevice = isTheRightDevice;
            PathToSearch = pathToSearch;
        }

        public ViewEngineResult FindPartialView(ControllerContext context, string viewName, bool useCache)
        {
            if (IsTheRightDevice(context))
            {
                return BaseViewEngine.FindPartialView(context, PathToSearch + "/" + viewName, useCache);
            }
            return new ViewEngineResult(new string[] { }); //we found nothing and we pretend we looked nowhere
        }

        public ViewEngineResult FindView(ControllerContext context, string viewName, string masterName, bool useCache)
          {
                    
            if (IsTheRightDevice(context))
            {
                return BaseViewEngine.FindView(context, PathToSearch + "/" + viewName, masterName, useCache);
            }
            return new ViewEngineResult(new string[] { }); //we found nothing and we pretend we looked nowhere
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            throw new NotImplementedException();
        }
    }

    public static class MobileHelpers
    {
        public static bool UserAgentContains(this ControllerContext c, string agentToFind)
        {
            return (c.HttpContext.Request.UserAgent.IndexOf(agentToFind, StringComparison.OrdinalIgnoreCase) > 0);
        }

        public static bool IsMobileDevice(this ControllerContext c)
        {
            return c.HttpContext.Request.Browser.IsMobileDevice;
        }

        public static void AddMobile<T>(this ViewEngineCollection ves, Func<ControllerContext, bool> isTheRightDevice, string pathToSearch)
            where T : IViewEngine, new()
        {
            ves.Add(new CustomMobileViewEngine(isTheRightDevice, pathToSearch, new T()));
        }

        public static void AddMobile<T>(this ViewEngineCollection ves, string userAgentSubstring, string pathToSearch)
            where T : IViewEngine, new()
        {
            ves.Add(new CustomMobileViewEngine(c => c.UserAgentContains(userAgentSubstring), pathToSearch, new T()));
        }

        public static void AddIPhone<T>(this ViewEngineCollection ves) //specific example helper
            where T : IViewEngine, new()
        {
            ves.Add(new CustomMobileViewEngine(c => c.UserAgentContains("iPhone"), "iPhone", new T()));
        }

        public static void AddGenericMobile<T>(this ViewEngineCollection ves)
            where T : IViewEngine, new()
        {
            ves.Add(new CustomMobileViewEngine(c => c.IsMobileDevice(), "Mobile", new T()));
        }
    }
}
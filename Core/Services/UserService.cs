using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Services
{
    public class UserService
    {
        public static bool IsLogined(HttpSessionStateBase session)
        {
            return (session["CurrentUser"] != null);
        }

        public static long GetMemberIdAtLong(HttpSessionStateBase session)
        {
            return Convert.ToInt64(session["CurrentUser"].ToString());
        }
    }
}
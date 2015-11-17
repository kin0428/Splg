using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Extend
{
    public static class ExceptionExtend
    {
        public static int GetHttpCode(this Exception target)
        {
            if (target is HttpException)
            {
                return ((HttpException)target).GetHttpCode();
            }
            else
            {
                return 500;
            }
        }
    }
}
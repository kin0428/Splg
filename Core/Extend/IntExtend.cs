using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Constant;

namespace Splg.Core.Extend
{
    public static class IntExtend
    {
        public static DateTime ParseDatetime(this int target)
        {
            return DateTime.Parse(target.ToString("####/##/##"));
        }
    }
}
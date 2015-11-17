using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Extend
{
    public static class UriExtend
    {
        /// <summary>
        /// Uri文字列取得（Nullの場合はNull取得）
        /// </summary>
        public static string UriString(this Uri target)
        {
            if (target == null)
            {
                return null;
            }
            else
            {
                return target.ToString();
            }
        }
    }
}
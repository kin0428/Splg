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
 * Namespace	: Splg.Controllers
 * Class		: Splg
 * Developer	: Nguyen Ho Long Hai
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg
{
    public class MyCookie
    {
        /// <summary>
        /// encode to base 64
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Decode base 64
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void SetCookie(string memberId)
        {
            HttpCookie myCookie = HttpContext.Current.Response.Cookies[Constants.AUTH_COOKIE];
            myCookie.Value = Base64Encode(memberId);
            myCookie.Expires = DateTime.Now.AddDays(90);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }
   }
}
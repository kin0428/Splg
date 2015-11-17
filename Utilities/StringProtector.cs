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
 * Namespace	: Splg
 * Class		: StringProtector
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
#endregion

namespace Splg
{
    /// <summary>
    /// Use for encrypt and decrypt text when need.
    /// </summary>
    public static class StringProtector
    {
        #region Protect
        /// <summary>
        /// Encryption string with key.
        /// </summary>
        /// <param name="unprotectedText">Text need encryption.</param>
        /// <returns>Text encrypted.</returns>
        public static string Protect(string unprotectedText)
        {
            var unprotectedBytes = Encoding.UTF8.GetBytes(unprotectedText);
            var protectedBytes = MachineKey.Protect(unprotectedBytes, Constants.ENCRYTION);
            var protectedText = Convert.ToBase64String(protectedBytes);
            return protectedText;
        }
        #endregion

        #region Unprotect
        /// <summary>
        /// Decryption string with key.
        /// </summary>
        /// <param name="protectedText">Text need decryption.</param>
        /// <returns>Text decrypted.</returns>
        public static string Unprotect(string protectedText)
        {
            var protectedBytes = Convert.FromBase64String(protectedText);
            var unprotectedBytes = MachineKey.Unprotect(protectedBytes, Constants.ENCRYTION);
            var unprotectedText = Encoding.UTF8.GetString(unprotectedBytes);
            return unprotectedText;
        }
        #endregion
       
    }
}
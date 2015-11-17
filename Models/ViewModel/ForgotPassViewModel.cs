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
 * Namespace	: Splg.Models.ViewModel
 * Class		: ForgotPassViewModel
 * Developer	: Nguyen Ho Long Hai
 * 
 */
#endregion

using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Splg.Models.ViewModel
{
    public class ForgotPassViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errorRequiredEmail", ErrorMessageResourceType = typeof(Japanese))]
        [EmailAddress(ErrorMessageResourceType = typeof(Japanese), ErrorMessageResourceName = "errorInvalidEmail", ErrorMessage = null)]
        [System.Web.Mvc.Remote("CheckEmailForgotPass", "Member")]
        public string Email { get; set; }
    }
}
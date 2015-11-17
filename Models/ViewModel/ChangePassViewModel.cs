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
 * Class		: ChangePassViewModel
 * Developer	: Nguyen Ho Long Hai
 * 
 */
#endregion

#region Using directives
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Models.ViewModel
{
    public class ChangePassViewModel
    {
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errorRequiredPass", ErrorMessageResourceType = typeof(Japanese))]
        [StringLength(20, ErrorMessageResourceName = "errorLengthPassword", ErrorMessageResourceType = typeof(Japanese), MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errorRequiredConfirmPass", ErrorMessageResourceType = typeof(Japanese))]
        [Compare("Password", ErrorMessageResourceName = "errorComparePassword", ErrorMessageResourceType = typeof(Japanese))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
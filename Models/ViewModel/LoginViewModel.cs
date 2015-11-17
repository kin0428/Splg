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
 * Class		: MemberController
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
    public class LoginViewModel : IValidatableObject
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errorRequiredEmail", ErrorMessageResourceType = typeof(Japanese))]
        [EmailAddress(ErrorMessageResourceType = typeof(Japanese), ErrorMessageResourceName = "errorInvalidEmail", ErrorMessage = null)]
        public string Email { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errorRequiredPass", ErrorMessageResourceType = typeof(Japanese))]
        [StringLength(20, ErrorMessageResourceName = "errorLengthPassword", ErrorMessageResourceType = typeof(Japanese), MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ErrorLogin { get; set; }

      
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var member = ComCommon.GetMemberLogin(Email, Password);
            ErrorLogin = member != null ? member.Mail : string.Empty;
            if (string.IsNullOrEmpty(ErrorLogin))
            {
                yield return new ValidationResult(Japanese.errorLogin, new [] { "ErrorLogin" });
            }
        }
    }
}
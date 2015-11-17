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
 * Class		: MemberRegistViewModel
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
    public class MemberRegistViewModel
    {
        public MemberRegistViewModel()
        {
            ReceiveEmail = true;
            IsSNS = false;
        }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errorRequiredEmail", ErrorMessageResourceType = typeof(Japanese))]
        [EmailAddress(ErrorMessageResourceType = typeof(Japanese), ErrorMessageResourceName = "errorInvalidEmail", ErrorMessage = null)]
        [System.Web.Mvc.Remote("CheckEmailAvailability", "Member")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errorRequiredPass", ErrorMessageResourceType = typeof(Japanese))]
        [StringLength(20, ErrorMessageResourceName = "errorLengthPassword", ErrorMessageResourceType = typeof(Japanese), MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errorRequiredConfirmPass", ErrorMessageResourceType = typeof(Japanese))]
        [Compare("Password", ErrorMessageResourceName = "errorComparePassword", ErrorMessageResourceType = typeof(Japanese))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string NickName { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Gender { get; set; }
        public string FullName { get; set; }
        public string ImageLink { get; set; }
        public string Place { get; set; }
        public bool ReceiveEmail { get; set; }
        public bool RememberMe { get; set; }
        public short TypeID { get; set; }
        public string SNSName { get; set; }
        public bool IsSNS { get; set; }
        public string SNSID { get; set; }
        public static DateTime GetStartOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }
        public static DateTime GetEndOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 000);
        }
    }
}
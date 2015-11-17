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
 * Class		: ContactViewModel
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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion


namespace Splg.Models.ViewModel
{
    public class ContactViewModel
    {
        public ContactViewModel()
        {
            Title = Resources.Japanese.defaultValueTitleEmailContact;
        }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errorRequiredEmail", ErrorMessageResourceType = typeof(Japanese))]
        [EmailAddress(ErrorMessageResourceType = typeof(Japanese), ErrorMessageResourceName = "errorInvalidEmail", ErrorMessage = null)]
        public string EmailTo { get; set; }

        //[DefaultValue(typeof(String), "【スポログ】お問い合わせ")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "errorRequiredContentEmail", ErrorMessageResourceType = typeof(Japanese))]
        public string Content { get; set; }
        public string DisplayMemberId { get; set; }
        public string Nickname { get; set; }
        public string UserLoad { get; set; }
    }
}
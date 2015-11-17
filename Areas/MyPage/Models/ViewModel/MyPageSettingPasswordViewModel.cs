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
 * Namespace	: Splg.Areas.MyPage.Models
 * Class		: MyPageTopViewModel
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models.News.ViewModel;
using Splg.Models.Game.ViewModel;

using Splg.Models.ViewModel;
#endregion

namespace Splg.Areas.MyPage.Models.ViewModel
{
    public class MyPageSettingPasswordViewModel
    {
        public SettingPasswordModel SettingAddress { get; set; }

        public class SettingPasswordModel
        {
            public MemberRegistViewModel MemberRegisterInfo { get; set; }
            public string ErrorMessage { get; set; }
        }

    }
}
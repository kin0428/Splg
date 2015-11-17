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
 * Namespace	: Splg.Areas.Jleague.Models.ViewModel
 * Class		: JlgTeamInfoPlayerDetailViewModel
 * Developer	: e-concier
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models.News.ViewModel;
#endregion

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgTeamInfoPlayerDetailViewModel
    {
        public int TeamID { get; set; }
        public int PlayerID { get; set; }

        public JlgPlayerInfoYear PlayerInfoYear { get; set; }
        public JlgPlayerInfoSum PlayerSum { get; set; }
        public IEnumerable<JlgPlayerInfoOccasion> PlayerInfoOccasion { get; set; }
        public IEnumerable<PostedInfoViewModel> TeamPostedInfoList { get; set; }
    }
}
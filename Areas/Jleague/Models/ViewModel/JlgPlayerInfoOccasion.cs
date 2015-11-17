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
 * Class		: PlayerInfoOccasion
 * Developer	: e-concier
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgPlayerInfoOccasion
    {
        public int? OccasionNo { get; set; }
        public int? Time { get; set; }
        public int? StartF { get; set; }
        public int? Score { get; set; }
        public int? PKScore { get; set; }
        public int? Shoot { get; set; }
        public int? Yellow { get; set; }
        public int? Red { get; set; }
    }
}
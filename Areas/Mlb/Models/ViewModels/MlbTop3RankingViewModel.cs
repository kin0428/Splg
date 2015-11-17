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
 * Namespace	: Splg.Areas.Npb.Models.ViewModel
 * Class		: Top3RankingViewModel
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Mlb.Models.ViewModels
{
    /// <summary>
    /// Layer class to store list ranking from db.
    /// </summary>
    public class MlbTop3RankingViewModel
    {
        public IEnumerable<OfficialStatsMlb> AmericanEast { get; set; }
        public IEnumerable<OfficialStatsMlb> AmericanCenteral { get; set; }
        public IEnumerable<OfficialStatsMlb> AmericanWest { get; set; }
        public IEnumerable<OfficialStatsMlb> NationalEast { get; set; }
        public IEnumerable<OfficialStatsMlb> NationalCenteral { get; set; }
        public IEnumerable<OfficialStatsMlb> NationalWest { get; set; }
    }
}
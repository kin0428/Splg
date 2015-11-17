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
 * Namespace	: Splg.Areas.Prize.Models
 * Class		: PrizeTopViewModel
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
#endregion

namespace Splg.Areas.Prize.Models.ViewModel
{
    public class PrizeGoodViewModel
    {
        public RallyGoodViewModel RallyGoods { get; set; }

        public List<RallyGoodRemarksViewModel> RallyGoodsRemarks { get; set; }

        public List<RallyGoodRemarksTextViewModel> RallyGoodsRemarksText { get; set; }

        public List<RallyGoodRemarksLinkViewModel> RallyGoodsRemarksLink { get; set; }

        public int AvailablePoint { get; set; }

        public int EntryCount { get; set; }

        public bool IsLogined { get; set; }

        public string DispEntryCount 
        {
            get
            {
                string result = string.Empty;

                if (EntryCount > 0)
                {
                    result = "応募口数：" + EntryCount + "口";
                }

                return result;
            }
        }

    }
}
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Prize.Models;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;

namespace Splg.Areas.Prize.Models.ViewModel
{
    public class PrizeTopViewModel
    {
        /// <summary>
        /// 応募可能ポイント
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]  
        public int AvailablePoint { get; set; }

        /// <summary>
        /// 大会
        /// </summary>
        public RallyViewModel RallyViewModel { get; set; }

        /// <summary>
        /// 大会景品
        /// </summary>
        public List<RallyGoodViewModel> RallyGoodsModel { get; set; }

        /// <summary>
        /// 大会履歴
        /// </summary>
        public List<RallyViewModel> RallyHistories { get; set; }
    }
}
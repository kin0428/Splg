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
 * Namespace	: Splg.Models.Game.ViewModel
 * Class		: PointInfoViewModel
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models.PointInfo.InfoModel;
using Splg.Models.Game.InfoModel;

namespace Splg.Models.Game.ViewModel
{
    /// <summary>
    /// Layer class use for Point in Right Content of _NpbLayout.cshtml.
    /// </summary>
    public class PointInfoViewModel
    {

        public PointInfoModel PointInfoModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ExpectationInfoModel> ExpectationInfoModels { get; set; }
    }
}
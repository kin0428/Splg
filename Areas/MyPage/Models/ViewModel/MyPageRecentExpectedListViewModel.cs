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

using System;
using System.Collections.Generic;
using Splg.Models.Game.InfoModel;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;

namespace Splg.Areas.MyPage.Models.ViewModel
{
    public class MyPageRecentExpectedListViewModel
    {

        #region 年月用変数

        /// <summary>
        /// 試合日
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsDateAndDayOfWeekWithSlashSeparator)]
        public DateTime Today { get; set; } // 今日
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsDateAndDayOfWeekWithSlashSeparator)]
        public DateTime Yesterday { get; set; } // 昨日
        #endregion

        public IEnumerable<GameInfoModel> GameInfoTdy { get; set; }
        public IEnumerable<GameInfoModel> GameInfoYdy { get; set; }

        public class ExpectChangeModel
        {
            public int SportsID { get; set; }
            public int GameID { get; set; }
            public int ExpectPoint { get; set; }
            public int OldExpectPoint { get; set; }

        }

    }

}
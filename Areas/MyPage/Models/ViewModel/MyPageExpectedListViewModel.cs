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
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Models.Game.InfoModel;
#endregion

namespace Splg.Areas.MyPage.Models.ViewModel
{
    public class MyPageExpectedListViewModel
    {

        #region 年月用変数
        public string LastYear; // 昨年
        public string ThisYear; // 今年
        public string NextYear; // 来年
        public string ThisMonth; // 今月
        public string[] YearListClass = new string[3]; //　アクティブ月のとき'class="active"'、そうでないとき''
        public string[] MonthListClass = new string[12]; //　アクティブ月のとき'class="active"'、そうでないとき''
        #endregion

        public IEnumerable<GameInfoModel> GameInfo { get; set; }

    }

}
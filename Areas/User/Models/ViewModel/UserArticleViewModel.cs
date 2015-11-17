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
using Splg.Areas.User.Models.ViewModel;
using Splg.Areas.User.Models.InfoModel;
#endregion

namespace Splg.Areas.User.Models.ViewModel
{
    public class UserArticleViewModel
    {
        /// <summary>
        /// 初期表示時の表示件数
        /// </summary>
        public const int INITIAL_SIZE = 3;
        /// <summary>
        /// １ページあたりの件数初期値
        /// </summary>
        public const int INITIAL_PAGE_SIZE = 10;

        /// <summary>
        /// 他ユーザーの会員ID
        /// </summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 他ユーザのニックネーム
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 投稿
        /// </summary>
        public IEnumerable<ContributionForUser> Contributions { get; set; }

        /// <summary>
        /// 総件数（表示ページ以外含む）
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 現在のページ番号
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 現在の１ページ当たりの件数
        /// </summary>
        public int PageSize { get; set; }
    }
}
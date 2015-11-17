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
using Splg.Models;
using Splg.Areas.MyPage.Models.InfoModel;
#endregion

namespace Splg.Areas.MyPage.Models.ViewModel
{
    public class MyPageNoticeViewModel
    {
        /// <summary>
        /// 運営からのお知らせの初期表示時の表示件数
        /// </summary>
        public  const int MANAGEMENT_NOTICE_INITIAL_SIZE = 3;

        /// <summary>
        /// ○○さんへのお知らせの初期表示時の表示件数
        /// </summary>
        public  const int USER_NOTICE_INITIAL_SIZE = 10;


        /// <summary>
        /// １ページあたりの件数初期値
        /// </summary>
        public const int INITIAL_PAGE_SIZE = 10;

        /// <summary>
        /// 会員ID
        /// </summary>
        public long MemberId { get; set; }

        /// <summary>
        /// ニックネーム
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 運営からのお知らせ
        /// </summary>
        public IEnumerable<NoticeInfoForMyPage> ManagementNotices { get; set; }

        /// <summary>
        /// ○○さんへのお知らせ
        /// </summary>
        public IEnumerable<NoticeInfoForMyPage> UserNotices { get; set; }

        /// <summary>
        /// 運営からのお知らせの総件数（表示ページ以外含む）
        /// </summary>
        public int ManagementNoticeTotalCount { get; set; }

        /// <summary>
        /// 運営からのお知らせの現在のページ番号
        /// </summary>
        public int ManagementPageNo { get; set; }

        /// <summary>
        /// 運営からのお知らせの現在の１ページ当たりの件数
        /// </summary>
        public int ManagementPageSize { get; set; }

        /// <summary>
        /// ○○さんへのお知らせの総件数（表示ページ以外含む）
        /// </summary>
        public int UserNoticeTotalCount { get; set; }

        /// <summary>
        /// ○○さんへのお知らせの現在のページ番号
        /// </summary>
        public int UserNoticePageNo { get; set; }

        /// <summary>
        /// ○○さんへのお知らせの現在の１ページ当たりの件数
        /// </summary>
        public int UserNoticePageSize { get; set; }

    }
}
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
using System.Collections;
using System.Linq;
using System.Web;
using Splg.Models.News.ViewModel;
using Splg.Models.Game.ViewModel;
#endregion

namespace Splg.Areas.MyPage.Models.ViewModel
{
    public class MyPageGroupListViewModel
    {
        public Int64 MemberId;

        public IEnumerable<GroupListInfo> GroupLists;

        public class GroupListInfo
        {
            public Int64 GroupID;
            public string GroupName;
            private int numberOfMember;

            public int NumberOfMember
            {
                get { return numberOfMember; }
                set { numberOfMember = value;
                    NumberOfOther = value;
                }
            }

            public int NumberOfOther;
            public List<GroupMemberProfile> ProfileMember;
        }

        public class GroupMemberProfile
        {
            public Int64 MemberId;

            public string Nickname;
            private string profileImg;
            public string ProfileImage
            {
                get
                {
                    string result = "/Content/img/upload/member/DefaultProfilePicture.png";
                    if (profileImg != null && !String.IsNullOrEmpty(profileImg.Trim()))
                    {
                        result = profileImg;
                    }

                    return result;
                }
                set { profileImg = value; }
            }

        }
        /// <summary>
        /// 初期表示時の表示件数
        /// </summary>
        public const int INITIAL_SIZE = 0;

        /// <summary>
        /// １ページあたりの件数初期値
        /// </summary>
        public const int INITIAL_PAGE_SIZE = 10;

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
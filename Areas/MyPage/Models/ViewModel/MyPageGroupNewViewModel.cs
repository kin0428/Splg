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
 * Namespace	: Splg.Areas.MyPage.Models.ViewModel
 * Class		: MyPageGroupNewViewModel
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using System.Collections.Generic;
using Splg.Models.Members.InfoModel;
#endregion


namespace Splg.Areas.MyPage.Models.ViewModel
{
    public class MyPageGroupNewViewModel
    {
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

        public NewGroupModel NewGroup;

        public class NewGroupModel
        {
            public long MemberId { get; set; }
            public long GroupId { get; set; }
            public string GroupName { get; set; }
            public string FollowerSearchString { get; set; }
            public int CurrentCountFollowing { get; set; }
            public List<MemberModel> GroupMembers { get; set; }
            public List<MemberModel> FollowMembers { get; set; }
            public List<long> GroupMemberIdList { get; set; }
            public List<long> FollowMemberIdList { get; set; }
        }
        //public class MemberModel : Splg.Models.Members.InfoModel.MemberModel
        //{

        //}

        public class AddGroupModel
        {
            public int GroupID { get; set; }
            public string GroupName { get; set; }
            public string SearchString { get; set; }
            public List<long> MemberIDs { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }

    }
}
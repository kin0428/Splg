using System;
using System.Collections.Generic;
using System.Linq;

namespace Splg.Areas.MyPage.Models.InfoModel
{
    public class MyPageGroupModel
    {
        /// <summary>
        /// グループID
        /// </summary>
        public Int64 GroupId { get; set; }

        /// <summary>
        /// グループ名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 登録会員ID
        /// </summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int NumberOfMember
        {
            get
            {
                return GroupMembers == null ? 0 : GroupMembers.Count();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NumberOfOther { get; set; }  //編集で使用する

        /// <summary>
        /// 
        /// </summary>
        public List<MyPageGroupMemberModel> GroupMembers { get; set; }
    }
}
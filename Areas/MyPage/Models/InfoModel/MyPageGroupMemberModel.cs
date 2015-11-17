
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.MyPage.Models.InfoModel
{
    public class MyPageGroupMemberModel : Splg.Models.Members.InfoModel.MemberModel
    {
        public string userURL
        {
            get
            {
                if (this.IsLoginUser)
                    return "/mypage/";

                return "/user/" + MemberId.ToString();
            }
        }

        /// <summary>
        /// 予想数
        /// </summary>
        public int ExpectNumber { get; set; }

        /// <summary>
        /// 的中率
        /// </summary>
        public int CorrectPercent
        {
            get
            {
                if (ExpectNumber == 0)
                    return 0;

                return CorrectPoint / ExpectNumber;
            }
        }

        /// <summary>
        /// 的中ポイント数
        /// </summary>
        public int CorrectPoint { get; set; }

    }
}
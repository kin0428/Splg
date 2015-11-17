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
#endregion

namespace Splg.Areas.MyPage.Models.ViewModel
{
    public class MyPageSettingViewModel
    {
        public IEnumerable<MemberSettingInfoModel> MemberSettingInfo { get; set; }  // 会員情報
        public IEnumerable<PrefecturesInfoModel> PrefecturesInfo { get; set; }      // 出身地情報
        public IEnumerable<SportsInfoModel> SportsInfo { get; set; }                // スポーツ情報
        public IEnumerable<SendMailConditionInfoModel> SendMailInfo { get; set; }   // メール情報
        public string MailAddress { get; set; }   // メール

        #region 会員情報
        public class MemberSettingInfoModel
        {
            public Int64        MemberID{ get; set; }           // メンバーID
            public string       Nickname{ get; set; }           // メンバー名
            private string profileImg;        // 画像

            public string ProfileImg
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

            public int          ExpectNumber{ get; set; }       // 予想数
            public int          FollowingNumber{ get; set; }    // フォロー数
            public int          FollowerNumber{ get; set; }     // フォロワー数
            public short        Gender{ get; set; }             // 性別：1男性、2女性、3その他
            public string GenderCheckedMale { get; set; }  //
            public string GenderCheckedFeMale { get; set; }//
            public string Mail { get; set; }//
            public short        BirthdayYear{ get; set; }       // 誕生日
            public short        BirthdayMonth { get; set; }      // 誕生日
            public short        BirthdayDay { get; set; }        // 誕生日
            public short        PrefecturesID{ get; set; }      // 出身県
            public List<int>    LikeSportsID{ get; set; }       // 好きなスポーツ
            public List<TeamInfoModel> LikeTeam { get; set; }    // 好きなチーム
        }
        #endregion    

        #region 出身地情報
        public class PrefecturesInfoModel
        {
            public int PrefecturesID;
            public string PrefecturesName;
        }
        #endregion

        #region スポーツ情報
        public class SportsInfoModel
        {
            public int SportsID;
            public string SportsName;
            public List<TeamInfoModel> Teams;
        }
        #endregion

        #region チーム情報
        public class TeamInfoModel
        {
            public int SportsID{ get; set; }
            public int TeamID{ get; set; }
            public string TeamName{ get; set; }
        }
        #endregion

        #region メール情報
        public class SendMailConditionInfoModel
        {
            public int MailSendAtStart { get; set; }
            public int MailSendAtEnd { get; set; }
        }
        #endregion
    }
}
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
 * Namespace	: Splg.Areas.User.Models
 * Class		: UserTopViewModel
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
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;
#endregion

namespace Splg.Areas.User.Models.ViewModel
{
    public class UserTopViewModel
    {
        /// <summary>
        ///他ユーザーの会員ID 
        /// </summary>
        public long MemberId { get; set; }
        /// <summary>
        /// 他ユーザーのニックネーム
        /// </summary>
        public string Nickname { get; set; }

        public IEnumerable<NewsInfoViewModel> TopMostViewNews { get; set; }
        public IEnumerable<NewsInfoViewModel> TopRecentNews { get; set; }
        public IEnumerable<PostedInfoViewModel> TopRecentPost { get; set; }

        public IEnumerable<MemberInfoModel> MemberInfo { get; set; }    // 会員情報
        public IEnumerable<ReportInfoModel> ReportInfo { get; set; }    // 月間レポート
        public PointInfoModel PointInfo { get; set; }      // ポイント情報

        public int TotalPoints;   // 月別合計
        public int MonthlyRank;   // 月別順位
        public int TotalRank;   // 月別順位

        /// <summary>
        /// 応募可能ポイント
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]
        public int AvailablePoint
        {
            get
            {
                var availablePoint = TotalPoints - Constants.ExcludedPoint;
                return (availablePoint < 0) ? 0 : availablePoint;
            }
        }

        public string MonnthlyPointStr
        {
            get
            {
                string result = TotalPoints.ToString("##,##0");

                return result;
            }
        }
        public string MonthlyRankStr
        {
            get
            {
                string result = MonthlyRank.ToString("##,##0");

                return result;
            }
        }
        public string TotalRankStr
        {
            get
            {
                string result = TotalRank.ToString("##,##0");

                return result;
            }
        }

        public struct TeamInfo
        {
            public int SportsID;       // 好きなチーム
            public int TeamID;         // 好きなチーム
            public string TeamName;    // 好きなチーム
            public string Url;         // url
            public TeamInfo(int sid, int id, string name, string url)
            {
                SportsID = sid;
                TeamID = id;
                TeamName = name;
                Url = url;

            }

        }
        #region 会員情報
        public class MemberInfoModel
        {
            public Int64 MemberId;           // メンバーID
            public string Nickname;           // メンバー名

            private string profileImg { get; set; }
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
            public int ExpectNumber;       // 予想数
            public int FollowingNumber;    // フォロー数
            public int FollowerNumber;     // フォロワー数
            public string Gender;             // 性別：男性、女性、その他
            public int BirthdayYear;       // 誕生日
            public int BirthdayMonth;      // 誕生日
            public int BirthdayDay;        // 誕生日
            public string PrefecturesName;    // 出身県
            public string LikeSports;         // 好きなスポーツ
            public List<TeamInfo> Team;
        }
        #endregion

        #region 年月用変数
        public int RegistratedYear; // 先月
        public string ThisYear; // 今月
        public string NextYear; // 来月
        public int[] YearStatuses = null; //　アクティブ月のとき'class="active"'、データがあるとき''、そうでないとき'class="gray"'
        public int[] MonthStatuses = new int[12]; //　アクティブ月のとき'class="active"'、データがあるとき''、そうでないとき'class="gray"'

        public int[] YearClass = new int[3];
        #endregion

        public class ReportInfoModel
        {
            public int Year;
            public int Month;
            public long MemberID;
            public int SportsID;        // スポーツID
            public string SportsName;   // スポーツ名
            public int ExpectNumber;    // 予想数
            public int CorrectPercent;  // 的中率
            public int CorrectPoint;    // 的中ポイント数
            public DateTime CalculationDate;

        }

        #region ポイント情報
        public struct TargetWeekPointInfo
        {
            public int TargetYear;      // 年
            public int TargetMonth;     // 月
            public int TargetWeek;      // 週次
            public int PossesionPoint;  // 所持ポイント
            public int FundsPoint;      // 原資ポイント
            public int WinPoint;        // 獲得ポイント
            public int PayOffPoints;     // 精算済ポイント
            public string Ranking;         // 順位
        }
        #endregion


        public class PointInfoModel:Splg.Models.PointInfo.InfoModel.PointInfoModel
        {
        }
    }
}
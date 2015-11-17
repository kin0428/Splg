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
 * Namespace	: Splg.Areas.MyPage.Controllers
 * Class		: MyPageTopController
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using Splg.Areas.Jleague.Models;
using Splg.Areas.Mlb.Models;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.Npb.Models;
using Splg.Models;
using Splg.Services.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

#endregion


namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageTopController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();

        NpbEntities npb = new NpbEntities();
        JlgEntities jlg = new JlgEntities();
        MlbEntities mlb = new MlbEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //MyPageEntities mypage = new MyPageEntities();
        #endregion

        /// <summary>
        /// GET: /mypage/ 
        /// </summary>
        public ActionResult Index()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            MyPageTopViewModel viewModel = new MyPageTopViewModel();

            //予想情報を取得する
            var today = DateTime.Today;
            int thisYear = today.Year;// 今年
            int thisMonth = today.Month;

            GetReportInfo(viewModel, thisYear, thisMonth);

            return View(viewModel);
        }

        private void GetReportInfo(MyPageTopViewModel viewModel, int targetYear, int targetmonth)
        {

            Int64 memberId = GetMemberID();

            viewModel.MemberInfo = GetMemberInfo(memberId);

            DateTime today = DateTime.Today;
            int thisYear = today.Year;// 今年
            int thisMonth = today.Month;

            int totalPoints = 0;
            long monthlyRank = 0;
            long totalRank = 0;
            viewModel.PointInfo = GetPointInfo(memberId, thisYear, thisMonth, out totalRank, out totalPoints, out monthlyRank);
            viewModel.TotalRank = (int)totalRank;
            viewModel.TotalPoints = (int)totalPoints;

            #region 年月用変数の設定
            //ユーザー登録年から現在の年まで
            //データの存在チェックを行う

            DateTime? registTime = (from m in com.Member where m.MemberId == memberId select m.RegistTime).FirstOrDefault();
            int registYear = registTime.Value.Year;// 登録年

            viewModel.RegistratedYear = registYear;
            viewModel.ThisYear = thisYear.ToString();

            List<MyPageTopViewModel.ReportInfoModel> allYearMonth = GetAllYearMonthHasResults(memberId);

            int duration = thisYear - registYear;
            viewModel.YearStatuses = new int[duration + 1];

            for (int y = 0; y <= duration; y++)
            {
                int year = y + registYear;
                viewModel.YearStatuses[y] = 0; //データなし(初期値）

                if (year == targetYear) // target year
                {
                    viewModel.YearStatuses[y] = 1; //アクティブ

                    for (int m = 1; m <= 12; m++)
                    {
                        int month = m;
                        viewModel.MonthStatuses[m - 1] = 0; //データなし(初期値）

                        if (month == targetmonth)
                        {
                            viewModel.MonthStatuses[m - 1] = 1; //　アクティブ月のとき'class="active"'、そうでないとき''
                        }
                        else
                        {
                            //データが存在するか確認
                            var d = (from a in allYearMonth where a.Year == (year) && a.Month == month select a);
                            if (d.Count() > 0)
                                viewModel.MonthStatuses[m - 1] = 2; //データ有
                        }
                    }
                }
                else
                {
                    //データが存在するか確認
                    var d = (from a in allYearMonth where a.Year == (year) select a);
                    if (d.Count() > 0)
                        viewModel.YearStatuses[y] = 2; //データ有

                }
            }
            #endregion

            //対象年月のデータを取得
            viewModel.ReportInfo = GetMonthlyResults(memberId, targetYear, targetmonth);

        }

        #region Change YearMonth
        /// <summary>
        /// Change YearMonth
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult ChangeYearMonth(string yearmonth)
        {
            MyPageTopViewModel myPageTop = new MyPageTopViewModel();
            Int64 memberID = GetMemberID();
            int iyear = Convert.ToInt16(yearmonth.Split('-')[0]);
            int imonth = Convert.ToInt16(yearmonth.Split('-')[1]);
            myPageTop.ReportInfo = GetMonthlyResults(memberID, iyear, imonth);

            return Json(myPageTop.ReportInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region member情報の取得
        private IEnumerable<MyPageTopViewModel.MemberInfoModel> GetMemberInfo(Int64 memberId)
        {
            List<MyPageTopViewModel.MemberInfoModel> result = new List<MyPageTopViewModel.MemberInfoModel> { };

            if (memberId > 0)
            {

                MyPageTopViewModel.MemberInfoModel mbi = new MyPageTopViewModel.MemberInfoModel();
                MyPageTopViewModel.PointInfoModel pim = new MyPageTopViewModel.PointInfoModel();

                #region メンバー情報

                var member = (from m in com.Member
                              where m.MemberId == memberId
                              select m).FirstOrDefault();
                if (member != null)
                {
                    mbi.MemberId = Convert.ToInt64(member.MemberId);
                    mbi.Nickname = member.Nickname;
                    mbi.ProfileImg = member.ProfileImg;
                    switch (Convert.ToInt16(member.Gender)) // 1：男性、2：女性、3：その他
                    {
                        case 1: mbi.Gender = "男性"; break;
                        case 2: mbi.Gender = "女性"; break;
                        default: break;
                    }
                    mbi.BirthdayYear = Convert.ToInt32(member.BirthdayYear);
                    mbi.BirthdayMonth = Convert.ToInt32(member.BirthdayMonth);
                    mbi.BirthdayDay = Convert.ToInt32(member.BirthdayDay);

                    // 出身県
                    var prefecture = (from p in com.PrefectureMaster
                                      where p.PrefecturesID == member.PrefecturesId
                                      select p).FirstOrDefault();
                    if (prefecture != null)
                    {
                        mbi.PrefecturesName = prefecture.PrefecturesName;
                    }

                    // 好きなスポーツ
                    var sports = (from s in com.LikeSports
                                  join sm in com.SportsMaster
                                  on s.SportsID equals sm.SportsID
                                  where s.MemberID == member.MemberId
                                  orderby s.CreatedDate
                                  select sm);
                    foreach (var sp in sports)
                    {
                        mbi.LikeSports += sp.SportsName + " ";
                    }

                    // 好きなチーム
                    List<MyPageTopViewModel.TeamInfo> team_info = new List<MyPageTopViewModel.TeamInfo> { };
                    MyPageTopViewModel.TeamInfo ti;
                    var teams = (from t in com.LikeTeam
                                 where t.MemberID == member.MemberId
                                 orderby t.CreatedDate
                                 select t);
                    foreach (var tid in teams)
                    {
                        ti.TeamID = tid.TeamID;
                        ti.SportsID = tid.SportsID;
                        ti.TeamName = "";
                        ti.Url = "";
                        ti.TeamName = GetTeamName(ref ti);
                        team_info.Add(ti);
                    }
                    mbi.Team = team_info;

                }
                #endregion

                #region 予想数
                var seasontotal = (from s in com.SeasonTotalResults
                                   where
                                      s.MemberID == memberId
                                   // &&
                                   // s.StartYear == year   // 年度は見ない
                                   group s by s.MemberID into g
                                   select new
                                   {
                                       ExpectNumber = g.Sum(s => s.ExpectNumber)
                                   }
                                   ).FirstOrDefault();
                if (seasontotal != null)
                {
                    mbi.ExpectNumber = Convert.ToInt32(seasontotal.ExpectNumber);
                }
                else
                {
                    mbi.ExpectNumber = 0;
                }
                #endregion

                //フォロー数
                int followingCount = (from f in com.FollowList
                                join m in com.Member on f.MemberID equals m.MemberId
                                where f.FollowerMemberID == memberId
                                && m.Status == 1 select f).Count();
                mbi.FollowingNumber = followingCount;

                //フォロワー数
                int followerCount = (from f in com.FollowList
                                join m in com.Member on f.FollowerMemberID equals m.MemberId
                                where f.MemberID == memberId
                                && m.Status == 1 select f).Count();

                mbi.FollowerNumber = followerCount;

                result.Add(mbi);
            }

            return result;
        }
        #endregion

        /// <summary>
        /// ポイント情報の取得
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="target_year"></param>
        /// <param name="target_month"></param>
        /// <param name="targetRanking">総合ランキング</param>
        /// <param name="targetPossesionPoint">懸賞応募可能ポイント</param>
        /// <param name="monthlyRank"></param>
        /// <returns>PointInfo</returns>
        private MyPageTopViewModel.PointInfoModel GetPointInfo(Int64 memberId, int target_year, int target_month, out long targetRanking, out int targetPossesionPoint, out long monthlyRank)
        {
            MyPageTopViewModel.PointInfoModel pim = new MyPageTopViewModel.PointInfoModel();

            PointInfoService pointInfoService = new PointInfoService(com);

            targetRanking = 0;
            targetPossesionPoint = 0;
            monthlyRank = 0;

            if (memberId > 0)
            {
                var m = pointInfoService.GetMemberWithOnlinePoints(memberId, target_year, target_month);

                if (m == null)
                    return new MyPageTopViewModel.PointInfoModel();

                pim = new MyPageTopViewModel.PointInfoModel
                      {
                          PossesionPoint = m.PossesionPoint,
                          FundsPoint = m.FundsPoint,
                          PayOffPoints = m.PayOffPoints
                      };

                //所持ポイントランキングから総合ランキングと懸賞応募可能ポイントを取得する
                var targetDate = DateTime.Now.Date.AddDays(-1);
                var possesionPointRanking = (from ppr in com.PossesionPointRanking
                               where ppr.MemberID == memberId && ppr.TargetDate == targetDate
                               select ppr).FirstOrDefault();

                if (possesionPointRanking != null)
                {
                    targetRanking = possesionPointRanking.TargetRanking;
                    targetPossesionPoint = possesionPointRanking.TargetPossesionPoint;
                }
            }


            return pim;
        }


        #region memberIDの取得
        private Int64 GetMemberID()
        {
            Int64 memberId = -1;
            object currentUser = Session["CurrentUser"];

            if (currentUser != null)
                memberId = Convert.ToInt64(currentUser.ToString());

            //debug
            //memberId = 2;

            return memberId;
        }
        #endregion

        #region チーム名の取得
        private string GetTeamName(ref MyPageTopViewModel.TeamInfo orgti)
        {
            MyPageTopViewModel.TeamInfo ti = orgti;
            string result = null;
            switch (ti.SportsID)
            {
                case Constants.NPB_SPORT_ID:
                    // NPB
                    var nteam = (from tn in npb.TeamInfoMST
                                 where tn.TeamCD == ti.TeamID
                                 select tn).FirstOrDefault();
                    if (nteam != null)
                    {
                        ti.Url = "/npb/teams/" + ti.TeamID + "";
                        result = nteam.Team;
                    }
                    break;
                case Constants.JLG_SPORT_ID:
                    // Jleague
                    var jteam = (from tj in jlg.TeamInfoTE
                                 where tj.TeamID == ti.TeamID
                                 select tj).FirstOrDefault();
                    if (jteam != null)
                    {
                        ti.Url = "/jleague/j1/teams/" + ti.TeamID + "";
                        result = jteam.TeamName;
                    }
                    break;
                case Constants.MLB_SPORT_ID:
                    // MLB
                    var mteam = (from tm in mlb.TeamInfo
                                 where tm.TeamID == ti.TeamID
                                 select tm).FirstOrDefault();
                    if (mteam != null)
                    {
                        ti.Url = "/mlb/teams/" + ti.TeamID + "";
                        result = mteam.TeamName;
                    }
                    break;
                default:
                    break;
            }
            orgti = ti;
            return result;
        }
        #endregion

        #region レポート情報すべて取得
        private List<MyPageTopViewModel.ReportInfoModel> GetAllYearMonthHasResults(Int64 memberId)
        {
            var report = (from s in com.SportsMaster
                          join r in com.MonthlyResults on s.SportsID equals r.SportsID //海外サッカーを表示する場合はleft join
                          where r.MemberID == memberId
                          orderby r.SportsID, r.ReleVantYear, r.ReleVantYear
                          select new MyPageTopViewModel.ReportInfoModel
                          {
                              Year = r.ReleVantYear,
                              Month = r.ReleVantMonth
                          }
            ).ToList();

            return report;
        }
        #endregion

        #region レポート情報の取得
        private IEnumerable<MyPageTopViewModel.ReportInfoModel> GetMonthlyResults(Int64 memberId, int year, int month)
        {
            var report = (from s in com.SportsMaster
                          join r in com.MonthlyResults on s.SportsID equals r.SportsID //海外サッカーを表示する場合はleft join
                          where r.MemberID == memberId && r.ReleVantYear == year && r.ReleVantMonth == month
                          orderby r.SportsID
                          select new MyPageTopViewModel.ReportInfoModel
                          {
                              CorrectPercent = r.CorrectPercent,
                              CorrectPoint = r.CorrectPoint,
                              ExpectNumber = r.ExpectNumber,
                              MemberID = r.MemberID,
                              SportsID = r.SportsID,
                              SportsName = s.SportsName

                          }
            ).ToList();

            return report;
        }
        #endregion


    }
}
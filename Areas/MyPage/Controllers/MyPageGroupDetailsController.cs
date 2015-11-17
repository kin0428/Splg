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
using Splg.Areas.Jleague.Controllers;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.Mlb.Controllers;
using Splg.Areas.Mlb.Models;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.MyPage.Service;
using Splg.Areas.Npb.Controllers;
using Splg.Areas.Npb.Models;
using Splg.Controllers;
using Splg.Models;
using Splg.Models.Game.ViewModel;
using Splg.Services.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Splg.Areas.Jleague;
using Splg.Areas.Mlb;

#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageGroupDetailsController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        NpbEntities npb = new NpbEntities();

        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //MyPageEntities mypage = new MyPageEntities();

        // todo: interfaceへ変更
        private MyPageGroupDetailsService workerService;

        private SystemDatetimeService systemDatetimeService;
        
        #endregion

        public MyPageGroupDetailsController()
        {
            // todo インスタンス管理
            this.workerService = new MyPageGroupDetailsService(this.com);
            this.systemDatetimeService = new SystemDatetimeService();
        }

        /// <summary>
        /// GET: /mypage/group/{id}/detail
        /// </summary>
        public ActionResult Index(int group_id, int? page)
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            long memberId = this.GetLoginMemberId();

            int year = this.systemDatetimeService.TargetYear;
            int month = this.systemDatetimeService.TargetMonth;

            MyPageGroupDetailsViewModel viewModel = workerService.GetViewModel(group_id,
                                                                                memberId,
                                                                                0,
                                                                                MyPageGroupDetailsViewModel.INITIAL_SIZE,
                                                                                year,
                                                                                month);

            //メニュー年月用変数の設定
            viewModel.ThisYear = year.ToString(); // 今年
            viewModel.ThisMonth = month.ToString(); // 今月

            for (int i = 1; i <= 12; i++)
            {
                if (i == month)
                {
                    viewModel.MonthListClassRanking[i - 1] = "class=active"; //　アクティブ月のとき'class="active"'、そうでないとき''
                    viewModel.MonthListClassCorrect[i - 1] = "class=active"; //　アクティブ月のとき'class="active"'、そうでないとき''
                }
                else
                {
                    viewModel.MonthListClassRanking[i - 1] = ""; //　アクティブ月のとき'class="active"'、そうでないとき''
                    viewModel.MonthListClassCorrect[i - 1] = ""; //　アクティブ月のとき'class="active"'、そうでないとき''
                }
            }

            //TODO ロジックを確認する必要がある
            viewModel.NpbPostedList = PostedController.GetRecentPosts(Constants.NPB_POST_TYPE, Constants.NPB_SPORT_ID, null, 1);

            return View(viewModel);
        }

        private const int CLASSCLASS_TEAM = 2;
        private const int CLASSCLASS_GAME = 4;

        /// <summary>
        /// 当日のゲーム情報取得
        /// </summary>
        /// <param name="sports_id"></param>
        /// <param name="today"></param>
        /// <returns></returns>
        public List<MyPageGroupDetailsViewModel.MyPageGameInfoViewModel> GetTodaysGames(int? sports_id, int today)
        {
            Int64 memberId = GetLoginMemberId();
            List<MyPageGroupDetailsViewModel.MyPageGameInfoViewModel> ListGames = new List<MyPageGroupDetailsViewModel.MyPageGameInfoViewModel>();

            List<GameInfoViewModel> npbGameInfos = new List<GameInfoViewModel>();
            List<GameInfoViewModel> mlbGameInfos = new List<GameInfoViewModel>();
            List<JlgGameInfos> jlgGameInfos = new List<JlgGameInfos>();

            // グループメンバ０が予想している試合のみを選択
            var query = (from et in com.ExpectTarget
                         join ep in com.ExpectPoint on et.ExpectTargetID equals ep.ExpectTargetID
                         join pt in com.Point on ep.PointID equals pt.PointID
                         join gm in com.GroupMember on pt.MemberID equals gm.MemberID
                         where et.ClassClass == CLASSCLASS_GAME
                         && pt.MemberID == memberId
                         orderby et.SportsID, et.UniqueID
                         select et);
            if (query != null)
            {
                List<int> npbGameIDlist = (from q in query where q.SportsID == Constants.NPB_SPORT_ID select q.UniqueID).Distinct().ToList();
                List<int> mlbGameIDlist = (from q in query where q.SportsID == Constants.MLB_SPORT_ID select q.UniqueID).Distinct().ToList();
                List<int> jlgGameIDlist = (from q in query where q.SportsID == Constants.JLG_SPORT_ID select q.UniqueID).Distinct().ToList();

                NpbTopController NpbCtl = new NpbTopController();
                MlbTopController MlbCtl = new MlbTopController();
                JlgTopController jlgCtl = new JlgTopController();

                switch (sports_id)
                {

                    case Constants.NPB_SPORT_ID:
                        foreach (var game_id in npbGameIDlist)
                        {
                            IEnumerable<GameInfoViewModel> npbs = NpbCtl.GetGameInfo(null, null, null, null, game_id, null);
                            if (npbs != null)
                            {
                                GameInfoViewModel npbGameInfo = npbs.FirstOrDefault();
                                if (npbGameInfo != null)
                                {
                                    if (npbGameInfo.GameDate == today)
                                    {
                                        npbGameInfos.Add(npbGameInfo);
                                    }
                                }
                            }

                        }


                        if (npbGameInfos != null)
                        {
                            foreach (var s in npbGameInfos)
                            {
                                MyPageGroupDetailsViewModel.MyPageGameInfoViewModel g = new MyPageGroupDetailsViewModel.MyPageGameInfoViewModel();
                                g.SportsID = Constants.NPB_SPORT_ID;
                                g.GameTypeName = s.GameTypeName;
                                g.statusMatch = NpbCommon.GetStatusMatch(s.GameID, memberId.ToString());
                                g.SortKey = g.SportsID.ToString() + s.GameTypeName + s.GameID.ToString();
                                g.npbGameInfo = s;
                                ListGames.Add(g);
                            }
                        }
                        break;
                    case Constants.MLB_SPORT_ID:
                        foreach (var game_id in mlbGameIDlist)
                        {
                            IEnumerable<GameInfoViewModel> mlbs = MlbCtl.GetGameInfo(null, null, null, null, game_id, null);
                            if (mlbs != null)
                            {
                                GameInfoViewModel mlbGameInfo = mlbs.FirstOrDefault();
                                if (mlbGameInfo != null)
                                {
                                    if (mlbGameInfo.GameDate == today)
                                    {
                                        mlbGameInfos.Add(mlbGameInfo);
                                    }
                                }
                            }

                        }

                        if (mlbGameInfos != null)
                        {
                            foreach (var s in mlbGameInfos)
                            {
                                MyPageGroupDetailsViewModel.MyPageGameInfoViewModel g = new MyPageGroupDetailsViewModel.MyPageGameInfoViewModel();
                                g.SportsID = Constants.MLB_SPORT_ID;
                                g.GameTypeName = s.GameTypeName;
                                g.statusMatch = MlbCommon.GetStatusMatch(s.GameID, memberId.ToString());
                                g.SortKey = g.SportsID.ToString() + s.GameTypeName + s.GameID.ToString();
                                g.mlbGameInfo = s;
                                ListGames.Add(g);
                            }
                        }
                        break;

                    case Constants.JLG_SPORT_ID:
                        foreach (var game_id in jlgGameIDlist)
                        {
                            IEnumerable<JlgGameInfos> jlgs = jlgCtl.GetGameInfo(null, null, null, game_id, null, null, null);
                            if (jlgs != null)
                            {
                                JlgGameInfos jlgGameInfo = jlgs.FirstOrDefault();
                                if (jlgGameInfo != null)
                                {
                                    if (jlgGameInfo.GameDate == today)
                                    {
                                        jlgGameInfos.Add(jlgGameInfo);
                                    }
                                }
                            }

                        }

                        if (jlgGameInfos != null)
                        {
                            foreach (var s in jlgGameInfos)
                            {
                                MyPageGroupDetailsViewModel.MyPageGameInfoViewModel g = new MyPageGroupDetailsViewModel.MyPageGameInfoViewModel();
                                g.SportsID = Constants.JLG_SPORT_ID;
                                g.GameTypeName = s.GameKindName;
                                g.statusMatch = JlgCommon.GetStatusMatch(s.GameID, memberId.ToString());
                                g.SortKey = g.SportsID.ToString() + s.GameKindName + s.GameID.ToString();
                                g.jlgGameInfo = s;
                                ListGames.Add(g);
                            }
                        }

                        break;
                }



            }


            var result = ListGames.OrderBy(s => s.SortKey).ToList();
            ScoreGameInfo lstScoreHome;
            ScoreGameInfo lstScoreVisitor;
            foreach (var r in result)
            {
                switch (r.SportsID)
                {
                    case Constants.NPB_SPORT_ID:
                        lstScoreHome = NpbCommon.GetTeamInfoGTSByGameIDTeamID(r.npbGameInfo.GameID, r.npbGameInfo.HomeTeamID);
                        lstScoreVisitor = NpbCommon.GetTeamInfoGTSByGameIDTeamID(r.npbGameInfo.GameID, r.npbGameInfo.VisitorTeamID);
                        r.npbGameInfo.BottomTop = Utils.GetRoundName(lstScoreHome.TB);
                        r.npbGameInfo.Round = Convert.ToInt32(lstScoreHome.Inning);
                        r.npbGameInfo.HomeTeamR = lstScoreHome.R;
                        r.npbGameInfo.VisitorTeamR = lstScoreVisitor.R;
                        r.gOdds = NpbCommon.GetOddsInfoByGameID(r.npbGameInfo.GameID);
                        break;
                    case Constants.MLB_SPORT_ID:
                        lstScoreHome = MlbCommon.GetTeamInfoGTSByGameIDTeamIDHome(r.mlbGameInfo.GameID, r.mlbGameInfo.HomeTeamID);
                        lstScoreVisitor = MlbCommon.GetTeamInfoGTSByGameIDTeamIDVisitor(r.mlbGameInfo.GameID, r.mlbGameInfo.VisitorTeamID);
                        r.mlbGameInfo.BottomTop = Utils.GetRoundName(lstScoreHome.TB);
                        r.mlbGameInfo.Round = Convert.ToInt32(lstScoreHome.Inning);
                        r.mlbGameInfo.HomeTeamR = lstScoreHome.R;
                        r.mlbGameInfo.VisitorTeamR = lstScoreVisitor.R;
                        r.gOdds = MlbCommon.GetOddsInfoByGameID(r.mlbGameInfo.GameID);
                        break;
                    case Constants.JLG_SPORT_ID:
                        lstScoreHome = JlgCommon.GetTeamInfoGTSByGameIDTeamID(r.jlgGameInfo.GameID, Convert.ToInt32(r.jlgGameInfo.HomeTeamID));
                        lstScoreVisitor = JlgCommon.GetTeamInfoGTSByGameIDTeamID(r.jlgGameInfo.GameID, r.jlgGameInfo.AwayTeamID);
                        r.jlgGameInfo.Round = Convert.ToInt32(lstScoreHome.Inning);
                        r.jlgGameInfo.HomeTeamR = Convert.ToInt32(lstScoreHome.R);
                        r.jlgGameInfo.AwayTeamR = Convert.ToInt32(lstScoreVisitor.R);
                        r.gOdds = JlgCommon.GetOddsInfoByGameID(r.jlgGameInfo.GameID);
                        break;
                    default:
                        break;
                }
            }

            return result;

        }


        /// <summary>
        /// ログインユーザのMemberIDを取得
        /// </summary>
        /// <returns></returns>
        private long GetLoginMemberId()
        {
            // HACK: 共通化されるまでの仮メソッドです

            long memberId = 0;

            object currentUser = Session["CurrentUser"];
            if (currentUser != null)
            {
                memberId = Convert.ToInt64(currentUser.ToString());
            }

            return memberId;
        }

        #region Json Result

        /// <summary>
        /// メンバーから抜ける Leave Group
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult LeaveGroup(string groupid)
        {
            Int64 memberID = GetLoginMemberId();
            bool isResult = false;
            int groupID = Convert.ToInt16(groupid);

            using (var dbContextTransaction = com.Database.BeginTransaction())
            {
                try
                {
                    if (groupID > 0 && memberID > 0)
                    {
                        var del_gm = (from gm in com.GroupMember
                                      where gm.GroupID == groupID && gm.MemberID == memberID
                                      select gm).FirstOrDefault();
                        com.GroupMember.Remove(del_gm);
                        com.SaveChanges();
                        dbContextTransaction.Commit();
                        isResult = true;
                    }
                    else
                    {
                        //Rollback transaction.
                        dbContextTransaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ポイントランキングの取得 GetRanking
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult JsonGetRanking(int group_id, int year, int month)
        {
            Int64 memberID = GetLoginMemberId();
            var members = this.workerService.GetRanking(group_id, year, month, null, memberID);
            return Json(members, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 的中率の取得 GetCorrect
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult JsonGetCorrect(int group_id, int year, int month)
        {
            //GetRankingと同じになっている
            Int64 memberID = GetLoginMemberId();
            var members = this.workerService.GetRanking(group_id, year, month, null, memberID);
            return Json(members, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Partial View

        /// <summary>
        /// 試合情報 Show game info in partial view.
        /// </summary>
        /// <returns>PartialView : _NpbGameInfo</returns>        
        [NoCache]
        public ActionResult ShowGameInfo(int? type, int? gameDate, int? groupID, int? sports_id)
        {
            MyPageGroupDetailsViewModel viewModel = new MyPageGroupDetailsViewModel();

            Int64 memberId = GetLoginMemberId();
            int gdate = Convert.ToInt32(gameDate); // yyyymmdd

            ViewBag.Type = type;
            //            ViewBag.Link = link;

            viewModel.ListGames = GetTodaysGames(sports_id, gdate);

            //フォローユーザーの予想
            int GameID = 0;

            if (memberId > 0)
            {
                foreach (var g in viewModel.ListGames)
                {
                    switch (g.SportsID)
                    {
                        case Constants.NPB_SPORT_ID:
                            if (g.npbGameInfo != null)
                            {
                                GameID = g.npbGameInfo.GameID;
                            }
                            break;
                        case Constants.MLB_SPORT_ID:
                            if (g.mlbGameInfo != null)
                            {
                                GameID = g.mlbGameInfo.GameID;
                            }
                            break;
                        case Constants.JLG_SPORT_ID:
                            if (g.jlgGameInfo != null)
                            {
                                GameID = g.jlgGameInfo.GameID;
                            }
                            break;
                        default:
                            GameID = 0;
                            break;
                    }
                    if (GameID > 0)
                    {
                        FollowMemberList FollowMember = new FollowMemberList();
                        //ホームの勝ち
                        List<Member> ToWin = Utils.GetExpectingMembers(com, (int)GameID, memberId, CLASSCLASS_GAME, g.SportsID, 1);
                        List<Member> ToDraw = Utils.GetExpectingMembers(com, (int)GameID, memberId, CLASSCLASS_GAME, g.SportsID, 3);
                        List<Member> ToLose = Utils.GetExpectingMembers(com, (int)GameID, memberId, CLASSCLASS_GAME, g.SportsID, 2);
                        FollowMember.FollowMembersBetToWin = ToWin;
                        FollowMember.FollowMembersBetToDraw = ToDraw;
                        FollowMember.FollowMembersBetToLose = ToLose;

                        g.FollowMembers = FollowMember;

                    }
                }
            }
            return PartialView("_NpbGameInfo", viewModel);

        }

        #endregion

        #region Format GameDate
        /// <summary>
        /// Get game date from server side in session.
        /// </summary>
        /// <returns>String date formatted.</returns>
        [HttpPost]
        public JsonResult FormatGameDate(string date)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(date))
            {
                DateTime tmpDate = DateTime.ParseExact(date, "yyyyMMdd", null);
                //var tmpDate = Convert.ToDateTime(date);
                result = Utils.GetMonthAndDayOfWeek(tmpDate) + "の試合";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
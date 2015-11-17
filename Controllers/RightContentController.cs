#region Author Information
/*
 * Developer	: Tran Sy Huynh
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Areas.Jleague;
using Splg.Models;
using Splg.Models.ViewModel;
using Splg.Services.Point;
using Splg.Models.Game.InfoModel;
using Splg.Models.Game.ViewModel;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Mlb;
using Splg.Areas.Npb.Models;
using Splg.Areas.Mlb.Models;
using Splg.Core.Constant;

namespace Splg.Controllers
{
    public class RightContentController : Controller
    {
        #region Global variables

        // Entity for member, point and prize
        ComEntities com = new ComEntities();

        #endregion

        #region Action
        // GET: RightContent
        public ActionResult ShowPointRankings()
        {
            return PartialView("PointRankings", GetHomeRightContentViewModel());
        }

        /// <summary>
        /// 予想情報パネル表示
        /// Show point in right content : game that user predicted.
        /// </summary>
        [NoCache]
        public ActionResult ShowExpectedPoints(bool showCurrentPointInfoOnly = false)
        {
            ActionResult result = PartialView("_ExpectedPointInfo");

            PointInfoService service = new PointInfoService(com);

            int memberID = 0;
            object currentUser = Session["CurrentUser"];

            if (currentUser == null)
                return result;

            int.TryParse(currentUser.ToString(), out memberID);

            if (memberID == 0)
                return result;

            var pointInfoViewModel = service.GetExpectedPointInfo(memberID, showCurrentPointInfoOnly);
            result = PartialView("_ExpectedPointInfo", pointInfoViewModel);

            return result;
        }

        /// <summary>
        /// 予想済試合予定表示
        /// </summary>
        public ActionResult ShowExpectedGameSchedules(List<ExpectationInfoModel> model, DateTime targetDate, string memberId)
        {
            var divIdSeq = 0;

            var expectedGameSchedulesViewModel = new ExpectedGameSchedulesViewModel()
               {
                   ScheduledDate = targetDate,
                   ExpectedGameSchedules = new List<ExpectedGameSchedule>()
               };

            foreach (var item in model)
            {
                if (item.GameID == 0)
                {
                    continue;
                }

                divIdSeq++;

                var row = new ExpectedGameSchedule()
                            {
                                Status = 0,
                                InputPoint = item.ExpectPoint / 100,
                                SportsId = item.SportID,
                                Odds = item.Odds,
                                ExpectPoint = item.ExpectPoint,
                                ExpectPointID = item.ExpectPointID,
                                PointId = item.PointID,
                            };

                LoadExpectedGameSchedule(row, item, item.SportID, memberId);

                row.GameDate = GetGameDate(item, row);

                LoadGameCardTitle(row, item.SportID);

                row.BetTargetAlert = GetBetTargetAlert(row,item);

                row.DivId = targetDate.ToString("yyyyMMdd") + divIdSeq.ToString();  

                expectedGameSchedulesViewModel.ExpectedGameSchedules.Add(row);
            }

            return PartialView("ExpectedGameSchedules", expectedGameSchedulesViewModel);

        }

        /// <summary>
        /// 試合日時取得
        /// </summary>
        private  DateTime GetGameDate(ExpectationInfoModel item, ExpectedGameSchedule row)
        {
            var gameTime = DateTime.ParseExact(row.GameInfoViewModel.Time, "HHmm", null);

            return new DateTime(item.GameDate.Year, item.GameDate.Month, item.GameDate.Day, gameTime.Hour, gameTime.Minute, 0);
        }
        #endregion

        #region method
        // get model for view
        private HomeRightContentViewModel GetHomeRightContentViewModel()
        {
            var homeRightContentViewModel = new HomeRightContentViewModel();

            homeRightContentViewModel.NewPrizes = GetNewPrizes();

            var pointRankingService = new PointRankingService(com);

            homeRightContentViewModel.PointRankings = pointRankingService.GetPointRankingsByPullCount(5);

            return homeRightContentViewModel;
        }

        public IEnumerable<Prize> GetNewPrizes()
        {
            DateTime today = DateTime.Now;
            var query = from p in com.Prize
                        where p.EntryStartTime <= today && p.EntryEndTime >= today
                        orderby p.PrizePoints descending
                        select p;
            return query;
        }

        /// <summary>
        /// 予想済試合スケジュール読込
        /// </summary>
        private void LoadExpectedGameSchedule(ExpectedGameSchedule row, ExpectationInfoModel item, int sportsId, string memberId)
        {
            //Todo:インターフェースがほしい
            switch (sportsId)
            {
                case Constants.NPB_SPORT_ID:
                    row.GameInfoViewModel = NpbCommon.GetGameInfoByGameID(Constants.NPB_SPORT_ID, item.GameID.Value);
                    row.Status = NpbCommon.GetStatusMatch(item.GameID.Value, memberId);
                    row.SportsName = "プロ野球";
                    row.RouteName = RouteNameConst.NpbGameDetail;
                    break;
                case Constants.MLB_SPORT_ID:
                    row.GameInfoViewModel = NpbCommon.GetGameInfoByGameID(Constants.MLB_SPORT_ID, item.GameID.Value);
                    row.Status = MlbCommon.GetStatusMatch(item.GameID.Value, memberId);
                    row.SportsName = "MLB";
                    row.RouteName = RouteNameConst.MlbGameDetail;
                    break;
                case Constants.JLG_SPORT_ID:
                    row.GameInfoViewModel = NpbCommon.GetGameInfoByGameID(Constants.JLG_SPORT_ID, item.GameID.Value);
                    row.Status = JlgCommon.GetStatusMatch(item.GameID.Value, memberId);
                    row.SportsName = "Jリーグ";
                    row.RouteName = RouteNameConst.JlgGameDetail;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 試合カードタイトル読込
        /// </summary>
        private static void LoadGameCardTitle(ExpectedGameSchedule row, int sportsId)
        {
            switch (sportsId)
            {
                case Constants.NPB_SPORT_ID:
                    if (row.Status == 0)
                    {
                        row.GameCardTitle = row.GameInfoViewModel.HomeTeamName + " vs " + row.GameInfoViewModel.VisitorTeamName;
                    }
                    else
                    {
                        var lstScoreHome = NpbCommon.GetTeamInfoGTSByGameIDTeamID(row.GameInfoViewModel.GameID, row.GameInfoViewModel.HomeTeamID);
                        var lstScoreVisitor = NpbCommon.GetTeamInfoGTSByGameIDTeamID(row.GameInfoViewModel.GameID, row.GameInfoViewModel.VisitorTeamID);
                        row.GameCardTitle = row.GameInfoViewModel.HomeTeamName + " " + lstScoreHome.R + " - " + lstScoreVisitor.R + " " + row.GameInfoViewModel.VisitorTeamName;
                    }
                    break;
                case Constants.MLB_SPORT_ID:
                    if (row.Status == 2 || row.Status == 3)
                    {
                        //2=5分前以前、ベットなし  3=5分前以前、ベットあり
                        row.GameCardTitle = row.GameInfoViewModel.HomeTeamName + " vs " + row.GameInfoViewModel.VisitorTeamName;
                    }
                    else if (row.Status >= 6 && row.Status <= 9)
                    {
                        //6=6=試合中、ベットなし 7=試合中、ベットあり
                        //8=試合終了、ベットなし 9=試合終了、ベットあり
                        ScoreGameInfo lstScoreHome = MlbCommon.GetTeamInfoGTSByGameIDTeamIDHome(row.GameInfoViewModel.GameID, row.GameInfoViewModel.HomeTeamID);
                        ScoreGameInfo lstScoreVisitor = MlbCommon.GetTeamInfoGTSByGameIDTeamIDVisitor(row.GameInfoViewModel.GameID, row.GameInfoViewModel.VisitorTeamID);
                        row.GameCardTitle = row.GameInfoViewModel.HomeTeamName + " " + lstScoreHome.R + " - " + lstScoreVisitor.R + " " + row.GameInfoViewModel.VisitorTeamName;
                    }
                    else
                    {
                        //1ベット不可
                        //4=5分前以降、ベットなし  5=5分前以降、ベットあり
                        //10:試合中止
                        row.GameCardTitle = row.GameInfoViewModel.HomeTeamName + " vs " + row.GameInfoViewModel.VisitorTeamName;
                    }
                    break;
                case Constants.JLG_SPORT_ID:
                    if (row.Status == 2 || row.Status == 3)
                    {
                        //2=5分前以前、ベットなし  3=5分前以前、ベットあり
                        row.GameCardTitle = row.GameInfoViewModel.HomeTeamName + " vs " + row.GameInfoViewModel.VisitorTeamName;
                    }
                    else if (row.Status >= 6 && row.Status <= 9)
                    {
                        //6=6=試合中、ベットなし 7=試合中、ベットあり
                        //8=試合終了、ベットなし 9=試合終了、ベットあり
                        ScoreGameInfo lstScoreHome = JlgCommon.GetTeamInfoGTSByGameIDTeamID(row.GameInfoViewModel.GameID, row.GameInfoViewModel.HomeTeamID);
                        ScoreGameInfo lstScoreVisitor = JlgCommon.GetTeamInfoGTSByGameIDTeamID(row.GameInfoViewModel.GameID, row.GameInfoViewModel.VisitorTeamID);
                        row.GameCardTitle = row.GameInfoViewModel.HomeTeamName + " " + lstScoreHome.R + " - " + lstScoreVisitor.R + " " + row.GameInfoViewModel.VisitorTeamName;
                    }
                    else
                    {
                        //1ベット不可
                        //4=5分前以降、ベットなし  5=5分前以降、ベットあり
                        //10:試合中止
                        row.GameCardTitle = row.GameInfoViewModel.HomeTeamName + " vs " + row.GameInfoViewModel.VisitorTeamName;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 予想表示アラート取得
        /// </summary>
        private string GetBetTargetAlert(ExpectedGameSchedule row, ExpectationInfoModel item)
        {
            switch (item.BetSelectID)
            {
                case 1:
                    return row.GameInfoViewModel.HomeTeamName + "の勝ちに予想中";

                case 2:
                    return row.GameInfoViewModel.VisitorTeamName + "の勝ちに予想中";

                default:
                    return "引き分けに予想中";
            }
        }
        #endregion
    }
}
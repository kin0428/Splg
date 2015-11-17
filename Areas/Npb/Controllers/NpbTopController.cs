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
 * Namespace	: Splg.Areas.Npb.Controllers
 * Class		: NpbTopController
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Splg.Areas.Jleague;
using Splg.Areas.Mlb;
using Splg.Areas.MyPage;
using Splg.Areas.Npb.Models;
using Splg.Areas.Npb.Models.ViewModel;
using Splg.Controllers;
using Splg.Models;
using Splg.Models.Game.InfoModel;
using Splg.Models.Game.ViewModel;
using Splg.Services.Game;
using Splg.Services.Point;
using Splg.Services.System;
using WebGrease.Css.Extensions;

#endregion

namespace Splg.Areas.Npb.Controllers
{
    public class NpbTopController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Npb to get db.
        /// </summary>
        NpbEntities npb = new NpbEntities();
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();

        private SystemDatetimeService systemDatetimeService;
        #endregion

        public NpbTopController()
        {
            this.systemDatetimeService = new SystemDatetimeService();
        }

        #region Index
        // GET: Npb/NpbTop
        public ActionResult Index()
        {
          //// todo for Debug
          //  Session["CurrentUser"] = 133;
            
            NpbTopViewModel npbTop = new NpbTopViewModel();

            npbTop.ListTeamExpectationsDeviation = GetTop3DeviationByYear(1, DateTime.Now);
            npbTop.ListTeamBetrayalDeviation = GetTop3DeviationByYear(2, DateTime.Now);
            npbTop.NpbPostedList = PostedController.GetRecentPosts(Constants.NPB_POST_TYPE, Constants.NPB_SPORT_ID, null, 1);           

            return View(npbTop);
        }
        #endregion     
  
        #region Partial View

        #region Show game info

        /// <summary>
        /// Show game info in partial view.
        /// </summary>
        /// <param name = "type">
        /// 1:NpbTop, NpbScheduleResult
        /// 2:NpbScheduleResult, NpbTeamInfoDailyResult
        /// 3:NpbTeamInfoTeamTop
        /// 4:_NpbGameInfoPlayerInfo
        /// 5:NpbGameInformation
        /// </param>
        /// <param name="link"></param>
        /// <param name="gameDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="teamID"></param>
        /// <param name="gameID"></param>
        /// <param name="lstGameID"></param>
        /// <returns>PartialView : _NpbGameInfo</returns>        
        [NoCache]
        public ActionResult ShowGameInfo(int? type, int? link, int? gameDate, string startDate, string endDate, int? teamID, int? gameID, string lstGameID)
        {
            var lstGame = default(IEnumerable<GameInfoViewModelForNBP>);
            ViewBag.Type = type;
            ViewBag.Link = link;


            int? sDate = null;
            int? eDate = null;
            var lstgId = new List<int>();

            long memberId = this.GetMemberId();
        	
            if(type == 5)
            {
                int? gDate = npb.GameInfoSS.Where(m => m.GameDate > gameDate.Value).Min(m => m.GameDate);
                lstGame = GetGameInfo(gDate, sDate, eDate, teamID, null, null, memberId);      
            }
            else
            {
                if(!string.IsNullOrEmpty(startDate))
                    sDate = Convert.ToInt32(startDate);
              
                if (!string.IsNullOrEmpty(endDate))
                    eDate = Convert.ToInt32(endDate);
            	
                //Convert string to list
                if(type == 6 && !string.IsNullOrEmpty(lstGameID))
                {
                    List<string> listofIDs = lstGameID.Split(',').ToList();
                    lstgId = listofIDs.Select(int.Parse).ToList();
                    lstGame = GetGameInfo(gameDate, sDate, eDate, teamID, gameID, lstgId, memberId);
                }
                else
                {
                    lstGame = GetGameInfo(gameDate, sDate, eDate, teamID, gameID, null, memberId);
                }
            }

            lstGame.ForEach(x => x.ParameterInfo = new GameInfoViewModelForNBP.ParameterInfoModel
            {
                ParameterType = type,
                Link = link,
                GameDate = gameDate,
                StartDate = startDate,
                EndDate = endDate,
                TeamId = teamID,
                GameId = gameID,
                LstGameId = lstGameID
            });

            return PartialView("_NpbGameInfo", lstGame);
        }
        #endregion

        #endregion        

        #region Utilities

        private long GetMemberId()
        {
            long memberId = -1;
            object currentUser = Session["CurrentUser"];

            if (currentUser != null)
                memberId = Convert.ToInt64(currentUser.ToString());
            return memberId;
        }

        #region Get Top 3 Deviations By Year
        /// <summary>
        /// Get top 3 deviation by this year.
        /// </summary>
        /// <param name="dateNow">This year.</param>
        /// <returns>Top 3 deviation.</returns>
        public IEnumerable<TeamRankingDeviation> GetTop3DeviationByYear(int type, DateTime dateNow)
        {
            var classDeviation = (short)2;
            var queryFirst = default(IEnumerable<Deviation>);
            if (type == 1)
            {
                queryFirst = com.Deviation.Where(m => m.StartYear == dateNow.Year && m.SportsID == Constants.NPB_SPORT_ID && m.ClassClass == classDeviation).OrderByDescending(m => m.ExpectationsDeviation).Take(3);
            }
            else
            {
                queryFirst = com.Deviation.Where(m => m.StartYear == dateNow.Year && m.SportsID == Constants.NPB_SPORT_ID && m.ClassClass == classDeviation).OrderByDescending(m => m.BetrayalDeviation).Take(3);
            }
            var result = default(IEnumerable<TeamRankingDeviation>);
            if (queryFirst != null)
            {
                var lstQuery = queryFirst.ToList();
                var querySecond = from q1 in lstQuery
                                  join q2 in npb.TeamInfoMST on q1.UniqueID equals q2.TeamCD
                                  join q3 in npb.TeamIconNpb on q2.TeamCD equals q3.TeamCD
                                  join q4 in npb.OfficialStatsNpb on q1.UniqueID equals q4.TeamCD
                                  join q5 in npb.OfficialStatsHeaderNpb on q4.OfficialStatsHeaderNpbId equals q5.OfficialStatsHeaderNpbId
                                  where (q5.GameAssortment == 1 || q5.GameAssortment == 2)
                                  select new TeamRankingDeviation
                                  {
                                      TeamID = q1.UniqueID,
                                      TeamName = q2.Team,
                                      TeamIcon = q3.TeamIcon,
                                      ExpectationsDeviation = q1.ExpectationsDeviation,
                                      BetrayalDeviation = q1.BetrayalDeviation,
                                      Ranking = q4.Ranking,
                                      LeagueName = q2.ShortNameLeague
                                  };
                if (querySecond != null)
                {
                    result = querySecond;
                }
            }
            return result;
        }
        #endregion

        #region Get Game Info

        /// <summary>
        /// Get game info use for many page : 8-1, 8-2, 8-5-1, 8-6 Top and Bottom, Home,...
        /// </summary>
        /// <param name="gameDate">GameDate.</param>
        /// <param name="startDate">StartDate.</param>
        /// <param name="endDate">EndDate.</param>
        /// <param name="teamID">TeamID.</param>
        /// <param name="gameID">GameID.</param>
        /// <param name="lstGameID">List GameID.</param>
        /// <param name="memberId">MemberID</param>
        /// <returns>List game for each condition.</returns>
        public IEnumerable<GameInfoViewModelForNBP> GetGameInfo(int? gameDate, int? startDate, int? endDate, int? teamID, int? gameID, List<int> lstGameID, long memberId = -1)
        {
            var query = default(IEnumerable<GameInfoViewModelForNBP>);
            var queryFirst = default(IQueryable<GameInfoSSByCondidtion>);
            bool isValue = true;
        	
            if (lstGameID == null)
            {
                isValue = false;
                lstGameID = new List<int>();
            }
        	
            queryFirst = from giss in npb.GameInfoSS
                         join mss in npb.MonthGroupSS on giss.MonthGroupSSId equals mss.MonthGroupSSId
                         join ss in npb.SeasonScheduleSS on mss.SeasonScheduleSSId equals ss.SeasonScheduleSSId
                         where (gameDate == null || giss.GameDate == gameDate) && (startDate == null || giss.GameDate >= startDate)
                         && (endDate == null || giss.GameDate <= endDate)
                         && (teamID == null || giss.HomeTeamID == teamID || giss.VisitorTeamID == teamID)
                         && (giss.ID == gameID || gameID == null)
                         && (!isValue || lstGameID.Contains(giss.ID))
                         select new GameInfoSSByCondidtion
                         {
                             GameInfoSSNpb = giss,
                             GameTypeID = ss.GameTypeID,
                             GameTypeName = ss.GameTypeName
                         };

            //Continue get extra info for game.
            if (queryFirst == null)
            {
                query = null;
            }
            else
            {
                query = (from q in queryFirst
                         join ti in npb.TeamIconNpb on q.GameInfoSSNpb.HomeTeamID equals ti.TeamCD
                         join ti1 in npb.TeamIconNpb on q.GameInfoSSNpb.VisitorTeamID equals ti1.TeamCD
                         join osh1 in npb.OfficialStatsHeaderNpb on q.GameTypeID equals osh1.GameAssortment
                         join os1 in npb.OfficialStatsNpb on new { p1 = q.GameInfoSSNpb.HomeTeamID.Value, p2 = osh1.OfficialStatsHeaderNpbId } equals new { p1 = os1.TeamCD, p2 = os1.OfficialStatsHeaderNpbId }
                         join osh2 in npb.OfficialStatsHeaderNpb on q.GameTypeID equals osh2.GameAssortment
                         join os2 in npb.OfficialStatsNpb on new { p1 = q.GameInfoSSNpb.VisitorTeamID.Value, p2 = osh2.OfficialStatsHeaderNpbId } equals new { p1 = os2.TeamCD, p2 = os2.OfficialStatsHeaderNpbId }
                         join tschd in npb.TeamStatsCardDifferenceHeader on q.GameTypeID equals tschd.GameAssortment
                         join tscd in npb.TeamStatsCardDifference on new { p1 = q.GameInfoSSNpb.HomeTeamID.Value, p2 = tschd.TeamStatsCardDifferenceHeaderId } equals new { p1 = tscd.TeamCD, p2 = tscd.TeamStatsCardDifferenceHeaderId }
                         join tscd1 in npb.TeamStatsCardDifferenceInfo on new { p1 = tscd.TeamStatsCardDifferenceId, p2 = q.GameInfoSSNpb.VisitorTeamID.Value } equals new { p1 = tscd1.TeamStatsCardDifferenceId, p2 = tscd1.TeamsOpponentCD } into tmp1
                         from tc1 in tmp1.DefaultIfEmpty()
                         join tscdh1 in npb.TeamStatsCardDifferenceHeader on q.GameTypeID equals tscdh1.GameAssortment
                         join tscd2 in npb.TeamStatsCardDifference on new { p1 = q.GameInfoSSNpb.VisitorTeamID.Value, p2 = tscdh1.TeamStatsCardDifferenceHeaderId } equals new { p1 = tscd2.TeamCD, p2 = tscd2.TeamStatsCardDifferenceHeaderId }
                         join tscd3 in npb.TeamStatsCardDifferenceInfo on new { p1 = tscd2.TeamStatsCardDifferenceId, p2 = q.GameInfoSSNpb.HomeTeamID.Value } equals new { p1 = tscd3.TeamStatsCardDifferenceId, p2 = tscd3.TeamsOpponentCD } into tmp2
                         from tc2 in tmp2.DefaultIfEmpty()
                         select new GameInfoViewModelForNBP
                         {
                             GameID = q.GameInfoSSNpb.ID,
                             GameDate = q.GameInfoSSNpb.GameDate,
                             Time = q.GameInfoSSNpb.Time,
                             StadiumName = q.GameInfoSSNpb.StadiumName,
                             GameTypeID = q.GameTypeID,
                             GameTypeName = q.GameTypeName,
                             HomeTeamID = q.GameInfoSSNpb.HomeTeamID.Value,
                             HomeTeamName = q.GameInfoSSNpb.HomeTeamName ?? string.Empty,
                             HomeTeamNameS = q.GameInfoSSNpb.HomeTeamNameS ?? string.Empty,
                             HomeTeamIcon = ti.TeamIcon ?? string.Empty,
                             HomeTeamRanking = os1.Ranking != null ? os1.Ranking : 0,
                             HomeTeamWin = tc1.Win.Value != null ? tc1.Win.Value : 0,
                             HomeTeamR = 0,
                             HomeTeamScore = (from rgi in npb.RealGameInfoRootRGI
                                              join girgi in npb.GameInfoRGI on rgi.RealGameInfoRootRGIId equals girgi.RealGameInfoRootRGIId
                                              join srgi in npb.ScoreRGI on rgi.RealGameInfoRootRGIId equals srgi.RealGameInfoRootRGIId
                                              where rgi.Matchday == q.GameInfoSSNpb.GameDate && girgi.GameID == q.GameInfoSSNpb.ID && q.GameInfoSSNpb.HomeTeamID == srgi.TeamCD
                                              select srgi.TotalScore).FirstOrDefault(),
                             VisitorTeamID = q.GameInfoSSNpb.VisitorTeamID.Value,
                             VisitorTeamName = q.GameInfoSSNpb.VisitorTeamName ?? string.Empty,
                             VisitorTeamNameS = q.GameInfoSSNpb.VisitorTeamNameS ?? string.Empty,
                             VisitorTeamIcon = ti1.TeamIcon ?? string.Empty,
                             VisitorTeamRanking = os2.Ranking != null ? os2.Ranking : 0,
                             VisitorTeamWin = tc2.Win.Value != null ? tc2.Win.Value : 0,
                             VisitorTeamR = 0,
                             VisitorTeamScore = (from rgi in npb.RealGameInfoRootRGI
                                                 join girgi in npb.GameInfoRGI on rgi.RealGameInfoRootRGIId equals girgi.RealGameInfoRootRGIId
                                                 join srgi in npb.ScoreRGI on rgi.RealGameInfoRootRGIId equals srgi.RealGameInfoRootRGIId
                                                 where rgi.Matchday == q.GameInfoSSNpb.GameDate && girgi.GameID == q.GameInfoSSNpb.ID && q.GameInfoSSNpb.VisitorTeamID == srgi.TeamCD
                                                 select srgi.TotalScore).FirstOrDefault(),
                             Round = q.GameInfoSSNpb.Round.Value,
                         });

            }

            if (query != null)
            {
                var newQuery = query.ToList();

                IEnumerable<GameInfoModel> exceptedInfo = null;

                // 予想情報を取得
                // CHSTMLとGetGameInfoForTopで同じ値を取得してしまっている。
                // TDDO IsMobileDevice廃止
                // （PC版のDBアクセス箇所をコントローラ以下に移してGetGameInfoForTopを呼ぶようにする）
                if (memberId > 0 && this.HttpContext.Request.Browser.IsMobileDevice)
                {
                    exceptedInfo = MyPageCommon.GetGameInfoForTop(memberId, this.systemDatetimeService.TargetYear, this.systemDatetimeService.TargetMonth, 0, Constants.NPB_SPORT_ID);
                }

                var oddsService = new OddsService();

                foreach (var q in newQuery)
                {
                    var preStartingPichers = this.GetPlayerInfoSTByGameIDAndStatus(Constants.GAME_STATUS_PRE_GAME, q.GameID);
                    var playerInfoStarting = this.GetPlayerInfoSTByGameIDAndStatus(Constants.GAME_STATUS_DURING_GAME, q.GameID);
                    var winLosePitchers = this.GetPlayerInfoSTByGameIDAndStatus(Constants.GAME_STATUS_POST_GAME, q.GameID);

                    q.PreStartingPitcherH = preStartingPichers.FirstOrDefault(y => y.HV == 1);
                    q.PreStartingPitcherV = preStartingPichers.FirstOrDefault(y => y.HV == 2);
                    q.HomePlayerInfoStarting = playerInfoStarting.FirstOrDefault(y => y.HV == 1);
                    q.VisitorPlayerInfoStarting = playerInfoStarting.FirstOrDefault(y => y.HV == 2);
                    q.WinLosePitcherH = winLosePitchers.FirstOrDefault(y => y.HV == 1);
                    q.WinLosePitcherV = winLosePitchers.FirstOrDefault(y => y.HV == 2);

                    q.GameOddsInfoModel = oddsService.GetOddsInfoByGameID(Constants.NPB_SPORT_ID, q.GameID, memberId);

                    q.GameStatus = NpbCommon.GetStatusMatch(q.GameID, memberId.ToString());

                    if (exceptedInfo != null)
                    {
                        q.GameInfoModel = exceptedInfo.FirstOrDefault(e => e.GameID == q.GameID && e.SportsID == Constants.NPB_SPORT_ID);

                        // todo 本来ならば、MyPageCommon.GetGameInfoForTopにて設定するべき
                        if (q.GameInfoModel != null)
                        {
                            q.GameInfoModel.GameStatus = q.GameStatus;
                            q.GameInfoModel.HomeTeamID = q.HomeTeamID;
                            q.GameInfoModel.VisitorTeamID = q.VisitorTeamID;
                            if (q.GameInfoModel.WinTeamCd == q.GameInfoModel.HomeTeamID)
                            {
                                q.GameInfoModel.WinnerTeam = 1;
                            }
                            else if (q.GameInfoModel.WinTeamCd == q.GameInfoModel.VisitorTeamID)
                            {
                                q.GameInfoModel.WinnerTeam = 2;
                            }
                        }
                    }
                }

                query = newQuery.AsQueryable();
            }

            return query;
        }
        #endregion

        #region Get TeamInfoPSP By GameID
        /// <summary>
        /// Get pre starting picher of home team and visitor team by GameID. 
        /// </summary>
        /// <param name="gameID">GameID to define another games.</param>
        /// <returns>List of pre starting pitcher.</returns>
        public IEnumerable<TeamInfoPSP> GetTeamInfoPSPByGameID(int gameID)
        {
            var query = from gi in npb.GameInfoPSP
                        join ti in npb.TeamInfoPSP on gi.GameInfoPSPId equals ti.GameInfoPSPId
                        where gi.GameID == gameID
                        select ti;
            return query;
        }
        #endregion

        #region Get PlayerInfoST By GameID

        /// <summary>
        /// Get player take part in game By GameID and status..
        /// </summary>
        /// <param name="gameStatus"></param>
        /// <param name="gameId">GameID to define another games.</param>
        /// <returns>List of player in game.</returns>
        public IEnumerable<PlayerInfoInGame> GetPlayerInfoSTByGameIDAndStatus(int gameStatus, int gameId)
        {
            var query = default(IEnumerable<PlayerInfoInGame>);
            // 試合前：予告先発
            if (gameStatus == Constants.GAME_STATUS_PRE_GAME )
            {
                query = (from tip in npb.TeamInfoPSP
                         join gip in npb.GameInfoPSP on tip.GameInfoPSPId equals gip.GameInfoPSPId
                         where gip.GameID == gameId
                         select new PlayerInfoInGame
                         {
                             PlayerNameS = tip.PitcherNameS,
                             PlayerEra = tip.Era,
                             HV = tip.HV,
                             IsWIN = true
                         });
            }
            // 試合中：先発
            else if (gameStatus == Constants.GAME_STATUS_DURING_GAME)
            {
                query = (from st in npb.StartingGameST
                         join ti in npb.TeamInfoST on st.StartingGameSTId equals ti.StartingGameSTId
                         join pi in npb.PlayerInfoST on ti.TeamInfoSTId equals pi.TeamInfoSTId
                         join df in npb.DefenceLocationMST on pi.StartPosition equals df.DefenceLocationID
                         where st.GameID == gameId
                         where pi.StartPosition == Constants.PositionPither
                         select new PlayerInfoInGame
                         {
                             PlayerNameS = pi.PlayerNameS,
                             PlayerEra = pi.Era,
                             HV = ti.HV,
                             IsWIN = true
                         });
            }
            // 試合後：勝ち（or負け)投手
            else
            {
                query = (
                        from st in npb.StartingGameST
                        join ti in npb.TeamInfoST on st.StartingGameSTId equals ti.StartingGameSTId
                        join pi in npb.PlayerInfoST on ti.TeamInfoSTId equals pi.TeamInfoSTId
                        join wp in npb.WinPitcherGL on pi.PlayerID equals wp.PlayerID
                        join gei in npb.GameEndInfoGL on wp.GameEndInfoGLId equals gei.GameEndInfoGLId
                        join glg in npb.GameLiveGL on new { k1 = gei.GameLiveGLId, k2 = st.GameID } equals new { k1 = glg.GameLiveGLId, k2 = (int)glg.GameID }
                        where st.GameID == gameId
                        select new PlayerInfoInGame
                        {
                            PlayerNameS = pi.PlayerNameS,
                            PlayerEra = pi.Era,
                            HV = ti.HV,
                            IsWIN = true
                        }).Union(
                        from st in npb.StartingGameST
                        join ti in npb.TeamInfoST on st.StartingGameSTId equals ti.StartingGameSTId
                        join pi in npb.PlayerInfoST on ti.TeamInfoSTId equals pi.TeamInfoSTId
                        join lp in npb.LosePitcherGL on pi.PlayerID equals lp.PlayerID
                        join gei in npb.GameEndInfoGL on lp.GameEndInfoGLId equals gei.GameEndInfoGLId
                        join glg in npb.GameLiveGL on new { k1 = gei.GameLiveGLId, k2 = st.GameID } equals new { k1 = glg.GameLiveGLId, k2 = (int)glg.GameID }
                        where st.GameID == gameId
                        select new PlayerInfoInGame
                        {
                            PlayerNameS = pi.PlayerNameS,
                            PlayerEra = pi.Era,
                            HV = ti.HV,
                            IsWIN = false
                        });
            }

            return query;
        }
        #endregion

        #endregion

        #region Json Result

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

        #region Get Status By GameID
        /// <summary>
        /// Get status of game.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>Game status</returns>
        [HttpPost]
        public JsonResult GetStatusByGameID(int sportID, int gameID, string memberID)
        {
            //Must test time before game start 5 minutes.
            var gameStatus = 0;
          
            switch (sportID)
            {
                case Constants.NPB_SPORT_ID:
                    var query = npb.GameInfoSS.Where(x => x.ID == gameID).Select(x => new { x.GameDate, x.Time }).FirstOrDefault();
                    if (query != null)
                    {
                        //ゲームステータス１ってなんぞや？
                        var strTimeRemain = Utils.CalculateTimeRemainDisplayString(query.GameDate, query.Time);

                        gameStatus = (strTimeRemain.Contains("まで アト")) ? 1 : NpbCommon.GetStatusMatch(gameID.ToString());
                    }
                    else
                    {
                        //Continue get status to test.
                        gameStatus = NpbCommon.GetStatusMatch(gameID.ToString());
                    }

                    break;

                case Constants.MLB_SPORT_ID:
                    gameStatus = MlbCommon.GetStatusMatch(gameID, memberID);
                
                    break;
                
                case Constants.JLG_SPORT_ID:
                    gameStatus = JlgCommon.GetStatusMatch(gameID, memberID);
    
                    break;

                default:
                    break;
            }

            return Json(gameStatus, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Define User Login Or Not
        /// <summary>
        /// Define user login or not.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DefineLoginOrNot()
        {
            string result = string.Empty;
            if (Session["CurrentUser"] != null)
            {
                result = Session["CurrentUser"].ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public JsonResult GetPossesionPoint()
        {
            var memberId = (Convert.ToInt64(Session["CurrentUser"]));

            var pointInfoService = new PointInfoService(com);
            var memberModel = pointInfoService.GetMemberWithOnlinePoints(memberId, (short)DateTime.Now.Year, (short)DateTime.Now.Month);

            return Json(memberModel.PossesionPoint, JsonRequestBehavior.AllowGet);
        }


        #region Start Prediction
        /// <summary>
        ///Start prediction with default point : 100.
        ///Start transaction
        //      1.Test possession point must larger default point
        //      2.Insert to table ExpectPoint.
        //      3.Insert to table PointHistory.
        //      4.Update table point : decrease posession point
        //End transaction
        /// </summary>
        [HttpPost]
        public JsonResult StartPrediction(string expectTargetID, int gameID, int betSelectID, string memberID, int teamID, int gameDate)
        {
            bool isResult = false;
            long mID = 0;
            //Decrypt text
            long lgExtTarget = Convert.ToInt64(StringProtector.Unprotect(expectTargetID));
            int gID = gameID;
            using (var dbContextTransaction = com.Database.BeginTransaction())

            {
                try
                {
                    if (!string.IsNullOrEmpty(memberID))
                    {
                        mID = Convert.ToInt64(memberID);
                    }
                    //1.Test possession point must larger default point
                    //1.1 Get pointID that have time available with BetStartDate and BetEndDate. 
                    DateTime gDate = DateTime.ParseExact(gameDate.ToString(), "yyyyMMdd", null);
                    var pointID = com.Point.Where(m => m.MemberID == mID && m.BetStartDate <= gDate && m.BetEndDate >= gDate).Select(m => m.PointID).FirstOrDefault();

                    //1.2 Get Possession point for PointID and subtract possession with expectpoint.
                    var possPoint = com.Point.Where(m => m.PointID == pointID).FirstOrDefault();
                    var pointRemain = possPoint.PossesionPoint - Constants.POINT_DEFAULT;
                    
                    //Continue to step 2.
                    if (pointRemain > 0)
                    {
                        //2.Insert to table ExpectPoint.
                        //2.1.Test that record exists in db or not.
                        var query = (from et in com.ExpectTarget
                                     join ep in com.ExpectPoint on et.ExpectTargetID equals ep.ExpectTargetID
                                     where et.ExpectTargetID == lgExtTarget && et.UniqueID == gID && ep.PointID == possPoint.PointID
                                     select ep).FirstOrDefault();

                        //2.2.If not exist in ExpectPoint, insert new record
                        if (query == null)
                        {
                            ExpectPoint ep = new ExpectPoint();
                            ep.ExpectTargetID = lgExtTarget;
                            ep.PointID = pointID;
                            ep.ExpectPoint1 = Constants.POINT_DEFAULT;
                            ep.AcquisitionPoint = null;
                            ep.ClassClass = 2;
                            ep.UniqueID = teamID;
                            ep.BetSelectID = betSelectID;
                            ep.VorD = null;
                            ep.SituationStatus = 1;
                            ep.CreatedAccountID = memberID;
                            ep.CreatedDate = DateTime.Now;
                            ep.ModifiedAccountID = null;
                            ep.ModifiedDate = null;
                            com.ExpectPoint.Add(ep);
                        }
                        //2.3 Record exists : test user cancel prediction or not.
                        else
                        {
                            
                            if(query.SituationStatus == 2)
                            {
                                //Update expectPoint with default point, change status.
                                query.SituationStatus = 1;
                                query.BetSelectID = betSelectID;
                                query.ExpectPoint1 = Constants.POINT_DEFAULT;
                                query.UniqueID = teamID;
                                query.ModifiedAccountID = memberID;
                                query.ModifiedDate = DateTime.Now;
                            }
                            else if(query.SituationStatus == 1)
                            {
                                dbContextTransaction.Rollback();
                            }
                        }
                        com.SaveChanges();

                        //2.4 Get ExpectPointID from record that have been inserted.
                        var expectPointID = com.ExpectPoint.Where(m => m.ExpectTargetID == lgExtTarget && m.PointID == pointID
                            && m.UniqueID == teamID && m.BetSelectID == betSelectID && m.CreatedAccountID == memberID).Select(m => m.ExpectPointID).FirstOrDefault();

                        //3.Insert 2 record to table PointHistory
                        if (expectPointID != null)
                        {
                            //First record.
                            PointHistory his = new PointHistory();
                            his.PointId = pointID;
                            his.ExpectPointId = expectPointID;
                            his.CampaignId = null;
                            his.PointClass = 2;
                            his.Points = Constants.POINT_DEFAULT;
                            his.HistoryClass = 2;
                            his.AdjustmentClass = 2;
                            his.OperationClass = 1;
                            his.Status = true;
                            his.CreatedAccountID = memberID;
                            his.CreatedDate = DateTime.Now;
                            com.PointHistory.Add(his);

                            //Second record.
                            PointHistory his1 = new PointHistory();
                            his1.PointId = pointID;
                            his1.ExpectPointId = expectPointID;
                            his1.CampaignId = null;
                            his1.PointClass = 3;
                            his1.Points = Constants.POINT_DEFAULT;
                            his1.HistoryClass = 2;
                            his1.AdjustmentClass = 1;
                            his1.OperationClass = 1;
                            his1.Status = true;
                            his1.CreatedAccountID = memberID;
                            his1.CreatedDate = DateTime.Now;
                            com.PointHistory.Add(his1);
                        }

                        //4.Update table Point : decrease possession point
                        //4.1 Update table point with new possession point.
                        possPoint.PossesionPoint = pointRemain;
                        possPoint.ModifiedAccountID = memberID;
                        possPoint.ModifiedDate = DateTime.Now;

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
        #endregion
      
        #endregion
    }
}
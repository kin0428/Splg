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
 * Namespace	: Splg.Areas.Mlb.Controllers
 * Class		: MlbTopController
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebGrease.Css.Extensions;
using Splg.Areas.Mlb.Models;
using Splg.Areas.Mlb.Models.ViewModel;
using Splg.Areas.Mlb.Models.ViewModels;
using Splg.Areas.MyPage;
using Splg.Controllers;
using Splg.Models;
using Splg.Models.Game.InfoModel;
using Splg.Services.Game;
using Splg.Services.System;

#endregion

namespace Splg.Areas.Mlb.Controllers
{
    public class MlbTopController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Mlb to get db.
        /// </summary>
        MlbEntities mlb = new MlbEntities();
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();

        private SystemDatetimeService systemDatetimeService;
        #endregion

        public MlbTopController()
        {
            this.systemDatetimeService = new SystemDatetimeService();
        }

        #region Index
        // GET: Mlb/MlbTop
        public ActionResult Index()
        {
            MlbTopViewModel mlbTop = new MlbTopViewModel();
            mlbTop.ListTeamExpectationsDeviation = GetTop3DeviationByYear(1, DateTime.Now);
            mlbTop.ListTeamBetrayalDeviation = GetTop3DeviationByYear(2, DateTime.Now);
            mlbTop.MlbPostedList = PostedController.GetRecentPosts(MlbConstants.MLB_POST_TYPE, Constants.MLB_SPORT_ID, null, 1);
            return View(mlbTop);
        }
        #endregion


        #region Partial View

        #region Show game info
        /// <summary>
        /// Show game info in partial view.
        /// </summary>
        /// <returns>PartialView : _MlbGameInfo</returns>        
        [NoCache]
        public ActionResult ShowGameInfo(int? type, int? link, int? gameDate, string startDate, string endDate, int? teamID, int? gameID, string lstGameID)
        {
            //TODO typeとはなにか、コメントを入れる

            IEnumerable<GameInfoViewModelForMLB> lstGame = default(IEnumerable<GameInfoViewModelForMLB>);
            ViewBag.Type = type;
            ViewBag.Link = link;
            int? sDate = null;
            int? eDate = null;
            List<int> lstgID = new List<int>();
            long memberId = this.GetMemberId();

            
            if (type == 5)
            {
                int? gDate = mlb.DayGroup.Where(m => m.GameDateJPN > gameDate.Value).Min(m => m.GameDateJPN);
                lstGame = GetGameInfo(gDate, sDate, eDate, teamID, null, null, memberId);
            }
            else
            {
                if (!string.IsNullOrEmpty(startDate))
                    sDate = Convert.ToInt32(startDate);

                if (!string.IsNullOrEmpty(endDate))
                    eDate = Convert.ToInt32(endDate);
                //Convert string to list
                
                if (type == 1)
                {
                    lstGame = GetGameInfoByDate((int)gameDate, memberId);

                }else if (type == 6 && !string.IsNullOrEmpty(lstGameID))
                {
                    List<string> listofIDs = lstGameID.Split(',').ToList();
                    lstgID = listofIDs.Select(int.Parse).ToList();
                    lstGame = GetGameInfo(gameDate, sDate, eDate, teamID, gameID, lstgID, memberId);
                }
                else
                {
                    lstGame = GetGameInfo(gameDate, sDate, eDate, teamID, gameID, null, memberId);
                    
                }
            }

            lstGame.ForEach(x => x.ParameterInfo = new GameInfoViewModelForMLB.ParameterInfoModel
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


            return PartialView("_MlbGameInfo", lstGame);
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

        #region Get Game Info

        ///  <summary>
        ///  Get game info use for many page : 8-1, 8-2, 8-5-1, 8-6 Top and Bottom, Home,...
        ///  </summary>
        /// <param name="gameDate">GameDate.</param>
        ///  <param name="startDate">StartDate.</param>
        ///  <param name="endDate">EndDate.</param>
        ///  <param name="teamId">TeamID.</param>
        ///  <param name="gameId">GameID.</param>
        ///  <param name="lstGameId">List GameID.</param>
        /// <param name="memberId">MemberID</param>
        /// <returns>List game for each condition.</returns>
        public IEnumerable<GameInfoViewModelForMLB> GetGameInfo(int? gameDate, int? startDate, int? endDate, int? teamId, int? gameId, List<int> lstGameId, long memberId = -1)
        {
            
            bool isValue = true;
            if (lstGameId == null)
            {
                isValue = false;
                lstGameId = new List<int>();
            }
            var query = default(IEnumerable<GameInfoViewModelForMLB>);

            query = from ss in mlb.SeasonSchedule
                        join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                        join hti in mlb.TeamInfo on ss.HomeTeamID equals hti.TeamID
                        join vti in mlb.TeamInfo on ss.VisitorTeamID equals vti.TeamID
                        join hos in mlb.OfficialStatsMlb on ss.HomeTeamID equals hos.TeamID
                        join hdg in mlb.DivGroupMlb on hos.DivGroupMlbId equals hdg.DivGroupMlbId
                        join hlg in mlb.LeagueGroupMlb on hdg.LeagueGroupMlbId equals hlg.LeagueGroupMlbId
                        join vos in mlb.OfficialStatsMlb on ss.VisitorTeamID equals vos.TeamID
                        join vdg in mlb.DivGroupMlb on vos.DivGroupMlbId equals vdg.DivGroupMlbId
                        join vlg in mlb.LeagueGroupMlb on vdg.LeagueGroupMlbId equals vlg.LeagueGroupMlbId
                        join htim in mlb.TeamIconMlb on ss.HomeTeamID equals htim.TeamCD into htimt
                        from hi in htimt.DefaultIfEmpty()
                        join vtim in mlb.TeamIconMlb on ss.VisitorTeamID equals vtim.TeamCD into vtimt
                        from vi in vtimt.DefaultIfEmpty()
                        join rgi in mlb.RealGameInfo on ss.GameID equals rgi.GameID into regi
                        from rg in regi.DefaultIfEmpty()
                        where (gameDate == null || dg.GameDateJPN == gameDate) && (startDate == null || dg.GameDateJPN >= startDate)
                         && (endDate == null || dg.GameDateJPN <= endDate)
                         && (teamId == null || ss.HomeTeamID == teamId || ss.VisitorTeamID == teamId)
                         && (ss.GameID  == gameId || gameId == null)
                         && (ss.Time.Contains("0") || ss.Time.Contains("1") || ss.Time.Contains("2") || ss.Time.Contains("3") || 
                         ss.Time.Contains("4") || ss.Time.Contains("5") || ss.Time.Contains("6") || ss.Time.Contains("7") || 
                         ss.Time.Contains("8") || ss.Time.Contains("9") )
                          && (!isValue || lstGameId.Contains(ss.GameID))
                        orderby hti.LeagueID, hti.DivID, hti.TeamID, vti.TeamID
                        select new GameInfoViewModelForMLB
                        {
                            GameID = ss.GameID,
                            GameDate = (int)dg.GameDateJPN,
                            Time = ss.Time,
                            StadiumName = ss.StadiumName,
                            GameTypeID = (int)hti.LeagueID,
                            GameTypeName = hti.LeagueName,
                            LeagueID = (int)hti.LeagueID,
                            LeagueName = hti.LeagueName,
                            DivID = (int)hti.DivID,
                            DivName = hti.DivName,

                            HomeTeamID = ss.HomeTeamID.Value,
                            HomeTeamFullName = ss.HomeTeamFullName ?? string.Empty,
                            HomeTeamName = ss.HomeTeamName ?? string.Empty,
                            HomeTeamIcon = hi.TeamIcon,
                            HomeTeamRanking = hos.Ranking != null ? hos.Ranking : 0,
                            HomeTeamWin = hos.Win.Value != null ? hos.Win.Value : 0,
                            HomeTeamScore = rg.HomeScore,

                            VisitorTeamID = ss.VisitorTeamID.Value,
                            VisitorTeamFullName = ss.VisitorTeamFullName != null ? ss.VisitorTeamName : string.Empty,
                            VisitorTeamName = ss.VisitorTeamName ?? string.Empty,
                            VisitorTeamIcon = vi.TeamIcon,
                            VisitorTeamRanking = vos.Ranking != null ? vos.Ranking : 0,
                            VisitorTeamWin = vos.Win.Value != null ? vos.Win.Value : 0,
                            VisitorTeamScore = rg.VisitorScore,


                        };

            IEnumerable<GameInfoModel> expectedInfo = null;

            // 予想情報を取得
            // CHSTMLとGetGameInfoForTopで同じ値を取得してしまっている。
            // TDDO IsMobileDevice廃止
            // （PC版のDBアクセス箇所をコントローラ以下に移してGetGameInfoForTopを呼ぶようにする）
            if (memberId > 0 && this.HttpContext.Request.Browser.IsMobileDevice)
            {
                expectedInfo = MyPageCommon.GetGameInfoForTop(memberId, this.systemDatetimeService.TargetYear, this.systemDatetimeService.TargetMonth);
            }

            var oddsService = new OddsService();
            
            var newQuery = query.ToList();

            foreach (var q in newQuery)
            {
                q.GameOddsInfoModel = oddsService.GetOddsInfoByGameID(Constants.MLB_SPORT_ID, q.GameID, memberId);

                q.GameStatus = MlbCommon.GetStatusMatch(q.GameID, memberId.ToString());

                if (expectedInfo != null)
                {
                    q.GameInfoModel = expectedInfo.FirstOrDefault(e => e.GameID == q.GameID && e.SportsID == Constants.MLB_SPORT_ID);

                    // todo 本来ならば、MyPageCommon.GetGameInfoForTopにて設定するべき
                    if (q.GameInfoModel != null)
                    {
                        q.GameInfoModel.GameStatus = q.GameStatus;
                    }
                }

                q.PreStartingPitcher = this.GetPlayerInfoSTByGameIDAndStatus(Constants.GAME_STATUS_PRE_GAME, q.GameID).FirstOrDefault();
                q.StartingPitcher = this.GetPlayerInfoSTByGameIDAndStatus(Constants.GAME_STATUS_POST_GAME, q.GameID).FirstOrDefault();
            }

            query = newQuery.AsQueryable();

            return query;
        }
        #endregion

        #region Get GasmeInfo By Day


        public IEnumerable<GameInfoViewModelForMLB> GetGameInfoByDate(int gameDate, long memberId = -1)
        {
            var query = from ss in mlb.SeasonSchedule
                        join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                        join hti in mlb.TeamInfo on ss.HomeTeamID equals hti.TeamID
                        join vti in mlb.TeamInfo on ss.VisitorTeamID equals vti.TeamID
                        join hos in mlb.OfficialStatsMlb on ss.HomeTeamID equals hos.TeamID
                        join hdg in mlb.DivGroupMlb on hos.DivGroupMlbId equals hdg.DivGroupMlbId
                        join hlg in mlb.LeagueGroupMlb on hdg.LeagueGroupMlbId equals hlg.LeagueGroupMlbId
                        join vos in mlb.OfficialStatsMlb on ss.VisitorTeamID equals vos.TeamID
                        join vdg in mlb.DivGroupMlb on vos.DivGroupMlbId equals vdg.DivGroupMlbId
                        join vlg in mlb.LeagueGroupMlb on vdg.LeagueGroupMlbId equals vlg.LeagueGroupMlbId
                        join htim in mlb.TeamIconMlb on ss.HomeTeamID equals htim.TeamCD into htimt
                        from hi in htimt.DefaultIfEmpty()
                        join vtim in mlb.TeamIconMlb on ss.VisitorTeamID equals vtim.TeamCD into vtimt
                        from vi in vtimt.DefaultIfEmpty()
                        join rgi in mlb.RealGameInfo on ss.GameID equals rgi.GameID into regi
                        from rg in regi.DefaultIfEmpty()
                        where dg.GameDateJPN == gameDate
                         && (ss.Time.Contains("0") || ss.Time.Contains("1") || ss.Time.Contains("2") || ss.Time.Contains("3") ||
                         ss.Time.Contains("4") || ss.Time.Contains("5") || ss.Time.Contains("6") || ss.Time.Contains("7") ||
                         ss.Time.Contains("8") || ss.Time.Contains("9"))
                        orderby hti.LeagueID, hti.DivID, hti.TeamID, vti.TeamID
                        select new GameInfoViewModelForMLB
                        {
                            GameID = ss.GameID,
                            GameDate = (int)dg.GameDateJPN,
                            Time = ss.Time,
                            StadiumName = ss.StadiumName,
                            GameTypeID = (int)hti.LeagueID,
                            GameTypeName = hti.LeagueName,
                            LeagueID = (int)hti.LeagueID,
                            LeagueName = hti.LeagueName,
                            DivID = (int)hti.DivID,
                            DivName = hti.DivName,

                            HomeTeamID = ss.HomeTeamID.Value,
                            HomeTeamFullName = ss.HomeTeamFullName ?? string.Empty,
                            HomeTeamName = ss.HomeTeamName ?? string.Empty,
                            HomeTeamIcon = hi.TeamIcon,
                            HomeTeamRanking = hos.Ranking != null ? hos.Ranking : 0,
                            HomeTeamWin = hos.Win.Value != null ? hos.Win.Value : 0,
                            HomeTeamScore = rg.HomeScore,

                            VisitorTeamID = ss.VisitorTeamID.Value,
                            VisitorTeamFullName = ss.VisitorTeamFullName != null ? ss.VisitorTeamName : string.Empty,
                            VisitorTeamName = ss.VisitorTeamName ?? string.Empty,
                            VisitorTeamIcon = vi.TeamIcon,
                            VisitorTeamRanking = vos.Ranking != null ? vos.Ranking : 0,
                            VisitorTeamWin = vos.Win.Value != null ? vos.Win.Value : 0,
                            VisitorTeamScore = rg.VisitorScore,


                        };

            IEnumerable<GameInfoModel> expectedInfo = null;

            if (memberId > 0 )
            {
                expectedInfo = MyPageCommon.GetGameInfoForTop(memberId, this.systemDatetimeService.TargetYear, this.systemDatetimeService.TargetMonth, 0, Constants.MLB_SPORT_ID);
            }

            var oddsService = new OddsService();

            var newQuery = query.ToList();

            foreach (var q in newQuery)
            {
                q.GameOddsInfoModel = oddsService.GetOddsInfoByGameID(Constants.MLB_SPORT_ID, q.GameID, memberId);

                q.GameStatus = MlbCommon.GetStatusMatch(q.GameID, memberId.ToString());

                if (expectedInfo != null)
                {
                    q.GameInfoModel = expectedInfo.FirstOrDefault(e => e.GameID == q.GameID && e.SportsID == Constants.MLB_SPORT_ID);

                    if(q.GameInfoModel != null)
                        q.GameInfoModel.OddsInfoModel = q.GameOddsInfoModel;

                    // todo 本来ならば、MyPageCommon.GetGameInfoForTopにて設定するべき
                    if (q.GameInfoModel != null)
                    {
                        q.GameInfoModel.GameStatus = q.GameStatus;
                    }
                }

                q.PreStartingPitcher = this.GetPlayerInfoSTByGameIDAndStatus(Constants.GAME_STATUS_PRE_GAME, q.GameID).FirstOrDefault();
                q.StartingPitcher = this.GetPlayerInfoSTByGameIDAndStatus(Constants.GAME_STATUS_POST_GAME, q.GameID).FirstOrDefault();
            }

            query = newQuery.AsQueryable();

            return query;
        }

        #endregion

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
                queryFirst = com.Deviation.Where(m => m.StartYear == dateNow.Year && m.SportsID == Constants.MLB_SPORT_ID && m.ClassClass == classDeviation).OrderByDescending(m => m.ExpectationsDeviation).Take(3);
            }
            else
            {
                queryFirst = com.Deviation.Where(m => m.StartYear == dateNow.Year && m.SportsID == Constants.MLB_SPORT_ID && m.ClassClass == classDeviation).OrderByDescending(m => m.BetrayalDeviation).Take(3);
            }
            var result = default(IEnumerable<TeamRankingDeviation>);
            if (queryFirst != null)
            {
                var lstQuery = queryFirst.ToList();
                var querySecond = from q1 in lstQuery
                                  join q2 in mlb.TeamInfo on q1.UniqueID equals q2.TeamID
                                  join q3 in mlb.TeamIconMlb on q2.TeamID equals q3.TeamCD
                                  join q4 in mlb.OfficialStatsMlb on q1.UniqueID equals q4.TeamID
                                  select new TeamRankingDeviation
                                  {
                                      TeamID = q1.UniqueID,
                                      TeamName = q2.TeamName,
                                      TeamIcon = q3.TeamIcon,
                                      ExpectationsDeviation = q1.ExpectationsDeviation,
                                      BetrayalDeviation = q1.BetrayalDeviation,
                                      Ranking = q4.Ranking,
                                      LeagueName = q2.LeagueName
                                  };
                if (querySecond != null)
                {
                    result = querySecond;
                }
            }
            return result;
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

            if (gameStatus == Constants.GAME_STATUS_PRE_GAME ||
                gameStatus == Constants.GAME_STATUS_DURING_GAME)
            {
                query = (from ds in mlb.DailySchedule
                         where ds.GameID == gameId
                         select new PlayerInfoInGame
                         {
                             HomeStartingName = ds.HomeNoticeStarterName,
                             VisitorStartingName = ds.VisitorNoticeStarterName
                         });
            }
            else
            {
                query = (from ds in mlb.RealGameInfo
                         where ds.GameID == gameId
                         select new PlayerInfoInGame
                         {
                             HomeStartingName = ds.HomeStarterName,
                             VisitorStartingName = ds.VisitorStarterName
                         });
            }

            return query;
        }
        #endregion

        #endregion

        #region Json Result

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

    
    
    
    
    
    }
}
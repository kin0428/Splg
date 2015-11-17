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
 * Namespace    : Splg.Areas.Mlb.Controllers
 * Class        : MlbGameInformationController
 * Developer    : Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using Splg.Areas.Mlb.Models;
using Splg.Areas.Mlb.Models.ViewModel;
using Splg.Models;
using Splg.Models.Game.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Splg.Areas.Mlb.Models.ViewModels.InfosModel;
using Splg.Areas.Mlb.Models.ViewModels;
#endregion

namespace Splg.Areas.Mlb.Controllers
{
    public class MlbGameInformationController : Controller
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
        #endregion

        #region Index
        /// <summary>
        /// Method to call when loading this page.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <returns>Model in this view.</returns>
        public ActionResult Index(int? gameID)
        {
            if (gameID != null)
            {
                //Using TempData to store gameID from request to another request.
                TempData["gameID"] = gameID;
                TempData.Keep("gameID");


                //Get game status the first time.
                int gameStatus = MlbCommon.GetStatusMatch(gameID.ToString());
                //int gameStatus = 0;

                //Use Session to store Status : need store long time and many request.
                //"0=試合前、1=試合中、2=試合後
                Session["Status"] = gameStatus;
                @ViewBag.Status = gameStatus;

                MlbGameInformationViewModel gameInfo = new MlbGameInformationViewModel();

                //フォローユーザーの予想
                long memberId = 0;

                object currentUser = Session["CurrentUser"];
                if (currentUser != null)
                    memberId = Convert.ToInt64(currentUser.ToString());

                if (memberId > 0)
                {
                    gameInfo.FollowMembersBetToDraw = Utils.GetExpectingMembers(com, (int)gameID, memberId, 4, Constants.MLB_SPORT_ID, 1);
                    gameInfo.FollowMembersBetToDraw = Utils.GetExpectingMembers(com, (int)gameID, memberId, 4, Constants.MLB_SPORT_ID, 3);
                    gameInfo.FollowMembersBetToLose = Utils.GetExpectingMembers(com, (int)gameID, memberId, 4, Constants.MLB_SPORT_ID, 2);
                }

                gameInfo.GameInSeasonSchedule = GetGameInfoCommonByGameID(gameID.Value);
                gameInfo.ListRightContentGames = GetGameInfoRightContentByStatus(gameStatus, gameID.Value);

                //この試合の投稿記事
                //TODO 共通メソッド待ち
                gameInfo.PostedInfo = Splg.Controllers.PostedController.GetRecentPosts(2, Constants.MLB_SPORT_ID, null, 1);
                return View(gameInfo);
            }
            return View();
        }
        #endregion

        #region Partial View

        #region Show GameInfo PlayerInfo
        /// <summary>
        /// Show PlayerInfo of selected game in partial view.
        /// </summary>
        /// <returns>PartialView : _MlbGameInfoPlayerInfo</returns>        
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 15)]
        public ActionResult ShowGameInfoPlayerInfo(int? gameID)
        {
            if (gameID != null)
            {
                //Get status from this session, this session must be not null. 
                int status = (int)Session["Status"];

                //Set status for viewbag to use 1 request.
                ViewBag.Status = status;

                MlbGameInformationViewModel gameInfo = new MlbGameInformationViewModel();
                gameInfo.GameInSeasonSchedule = GetGameInfoCommonByGameID(gameID.Value);
                if (gameInfo.GameInSeasonSchedule != null)
                {
                    var game = getGameInfo((int)gameID);

                    if (game != null)
                    {
                        if (game.HomeScore != null)
                        {
                            gameInfo.HomeScore = (int)game.HomeScore;
                        }
                        else
                            gameInfo.HomeScore = 0;
                        if (game.VisitorScore != null)
                        {
                            gameInfo.VisitorScore = (int)game.VisitorScore;
                        }
                        else
                            gameInfo.VisitorScore = 0;                        
                        gameInfo.Inning = 0;
                    }

                    //"0=試合前、1=試合中、2=試合後
                    gameInfo.ListPlayerInfo = GetPlayerInfoSTByGameIDAndStatus(status, gameID.Value);
                    if (status == 0)
                    {
                        gameInfo.ListPreStartingPitcher = GetTeamInfoPSPByGameID(gameID.Value);
                        gameInfo.List5GamesInHistory = GetLastNGameInfoFromHistoryByGameID(gameID.Value, gameInfo.GameInSeasonSchedule.HomeTeamID, gameInfo.GameInSeasonSchedule.VisitorTeamID);
                    }
                    else if (status == 1)
                    {
                        gameInfo.ListGameScoreOngoing = GetGameScoreOnGoingByGameID(gameID.Value);
                        gameInfo.ListGameText = GetAllGameTextsByGameID(gameID.Value);
                    }
                    else
                    {
                        gameInfo.ListGameText = GetAllGameTextsByGameID(gameID.Value);
                        gameInfo.ListGameScoreEnd = GetGameScoreEndByGameID(gameID.Value);
                        gameInfo.GameRoundComment = GetGameCommentByGameID(gameID.Value);
                        gameInfo.ListPitcher = GetPitcherInfoInGameLiveByGameID(gameID.Value);
                        gameInfo.ListHomerun = GetHomerunInGameLiveByGameID(gameID.Value);
                        gameInfo.ListReliefInfoes = GetReliefInfoInGameLiveByGameID(gameID.Value);
                        ViewBag.GameResult = GetGameResultByGameID(gameID.Value);
                    }
                }
                else
                {
                    gameInfo = null;
                }
                return PartialView("_MlbGameInfoPlayerInfo", gameInfo);
            }
            else
            {
                return PartialView("_MlbGameInfoPlayerInfo", null);
            }
        }
        #endregion

        #region Show Game In RightContent
        /// <summary>
        /// Show game info in right content.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>List game in date.</returns>
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 15)]
        public ActionResult ShowGameInRightContent(int? gameID)
        {
            if (gameID != null)
            {
                //Get status from this session, this session must be not null. 
                int status = (int)Session["Status"];

                IEnumerable<GameInfoViewModel> lstGame = GetGameInfoRightContentByStatus(status, gameID.Value);

                return PartialView("_MlbRightGames", lstGame);
            }
            else
            {
                return PartialView("_MlbRightGames", null);
            }
        }
        #endregion

        #endregion

        #region Json Result

        #region Get Game Status By GameID
        /// <summary>
        /// Get status of game: load or not load partialview _MlbGameInfoPlayerInfo.cshtml.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>0: Not load _MlbGameInfoPlayerInfo.cshtml, status not changed.</returns>
        /// <returns>1: Must load _MlbGameInfoPlayerInfo.cshtml, status changed.</returns>
        [HttpPost]
        //        public JsonResult GetGameStatusByGameID(int gameID)
        public JsonResult GetGameStatusByGameID(int? gameID)
        {
            List<string> lstResult = new List<string>();
            var result = false;
            var statusNew = -1;
            if (gameID != null)
            {
                //Random rnd = new Random();
                //int gameStatus = rnd.Next(0, 3);
                int gameStatus = MlbCommon.GetStatusMatch(gameID.ToString());
                statusNew = gameStatus;
                if (Session["Status"] == null || statusNew != (int)Session["Status"])
                {
                    //Update new status to session.
                    Session["Status"] = statusNew;
                    result = true;
                }
            }
            lstResult.Add(result.ToString());
            lstResult.Add(Session["Status"].ToString());
            return Json(lstResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Utilities

        #region Get TeamInfoPSP By GameID 予想先発情報_チーム情報
        /// <summary>
        /// Get pre starting picher of home team and visitor team by GameID. 
        /// </summary>
        /// <param name="gameID">GameID to define another games.</param>
        /// <returns>List of pre starting pitcher.</returns>
        public IEnumerable<MlbTeamInfoPSPModel> GetTeamInfoPSPByGameID(int gameID)
        {
            List<MlbTeamInfoPSPModel> result = new List<MlbTeamInfoPSPModel>();

            var query = (from rg in mlb.DailySchedule where rg.GameID == gameID select rg).FirstOrDefault();

            if (query != null)
            {
                MlbTeamInfoPSPModel homePitcher = new MlbTeamInfoPSPModel
                {
                    PitcherName = query.HomeNoticeStarterName,
                    PitcherArm = query.HomeNoticeStarterArm,
                    HV = 1
                };
                MlbTeamInfoPSPModel visitorPitcher = new MlbTeamInfoPSPModel
                {
                    PitcherName = query.VisitorNoticeStarterName,
                    PitcherArm = query.VisitorNoticeStarterArm,
                    HV = 2
                };

                result.Add(homePitcher);
                result.Add(visitorPitcher);
            }

            return result;
        }
        #endregion

        #region Get PlayerInfoST By GameID １試合スタメン＆ベンチ_選手情報 MLBには存在しない
        /// <summary>
        /// Get player take part in game By GameID and status..
        /// </summary>
        /// <param name="gameID">GameID to define another games.</param>
        /// <returns>List of player in game.</returns>
        public IEnumerable<PlayerInfoInGame> GetPlayerInfoSTByGameIDAndStatus(int gameStatus, int gameID)
        {
            var query = default(IEnumerable<PlayerInfoInGame>);
            return query;
        }
        #endregion

        #region Get Last 5 GameInfo From History By GameID　過去の直接対決の勝敗？
        /// <summary>
        /// Get 5 game info that 2 team took part in.
        /// Need HomeTeamID, VisitorTeamID, DateJPN to get data.
        /// </summary>
        /// <param name="gameID">Game id need get 2 teams info.</param>
        /// <returns>List game store in history.</returns>
        public IEnumerable<GameInHistory> GetLastNGameInfoFromHistoryByGameID(int gameID, int hTeamID, int vTeamID)
        {
            List<GameInHistory> result = new List<GameInHistory>();

            if (hTeamID == 0 || vTeamID == 0)
                return null;

            var date = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));

            //過去の試合を取得する
            //TODO ダブルヘッダの２試合目の予想の時、１試合目の存在を考慮していない？
            var games = (from ds in mlb.RealGameInfo
                         join dh in mlb.RealGameInfoHeader on ds.RealGameInfoHeaderId equals dh.RealGameInfoHeaderId
                         where ds.HomeTeamID == hTeamID && ds.VisitorTeamID == vTeamID &&
                         ds.GameStateID == 4 &&
                         dh.GameDateJPN < date
                         orderby dh.GameDateJPN descending
                         select new GameInHistory
                         {

                             DateJPN = (int)dh.GameDateJPN,
                             HomeErrors = ds.HomeErrors,
                             HomeHits = ds.HomeHits,
                             HomeScore = ds.HomeScore,
                             HomeStarterArm = ds.HomeStarterArm,
                             HomeStarterName = ds.HomeStarterName,
                             HomeStarterNum = ds.HomeStarterNum,
                             HomeTeamFullName = ds.HomeTeamFullName,
                             HomeTeamID = ds.HomeTeamID,
                             HomeTeamName = ds.HomeTeamName,
                             VisitorErrors = ds.VisitorErrors,
                             VisitorHits = ds.VisitorHits,
                             VisitorScore = ds.VisitorScore,
                             VisitorStarterArm = ds.VisitorStarterArm,
                             VisitorStarterName = ds.VisitorStarterName,
                             VisitorStarterNum = ds.VisitorStarterNum,
                             VisitorTeamFullName = ds.VisitorTeamFullName,
                             VisitorTeamID = ds.VisitorTeamID,
                             VisitorTeamName = ds.VisitorTeamName
                         }
                         ).Take(6);

            if (games == null)
                return null;
            else
                return games.Take(6);

        }
        #endregion

        #region Get GameInfo Common By GameID
        /// <summary>
        //        var lines = from m in com.Member 
        //                    from f in com.FollowList.Where(x => x.FollowerMemberID == memberId && x.MemberID == m.MemberId).DefaultIfEmpty()
        /// Get game info common : home team name, visitor team name,... by gameID. 
        /// </summary>
        /// <param name="gameID">Game ID</param>
        /// <returns>Game info.</returns>
        public GameInfoViewModel GetGameInfoCommonByGameID(int gameID)
        {
            var result = (from ss in mlb.SeasonSchedule
                          join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                          join ti in mlb.TeamIconMlb on ss.HomeTeamID equals ti.TeamCD into temp1
                          from tih in temp1.DefaultIfEmpty()
                          join ti in mlb.TeamIconMlb on ss.VisitorTeamID equals ti.TeamCD into temp2
                          from tiv in temp2.DefaultIfEmpty()
                          where ss.GameID == gameID
                          select new GameInfoViewModel
                          {

                              GameID = gameID,
                              GameDate = (int)dg.GameDateJPN,
                              Time = ss.Time,
                              HomeTeamID = ss.HomeTeamID.Value,
                              HomeTeamName = ss.HomeTeamFullName,
                              HomeTeamNameS = ss.HomeTeamName,
                              HomeTeamIcon = tih.TeamIcon,
                              VisitorTeamID = ss.VisitorTeamID.Value,
                              VisitorTeamName = ss.VisitorTeamFullName,
                              VisitorTeamNameS = ss.VisitorTeamName,
                              VisitorTeamIcon = tiv.TeamIcon
                          }).FirstOrDefault();

            return result;
        }
        #endregion

        #region Get Game Score OnGoing By GameID        Mlbには情報はない RealGameInfoは？
        /// <summary>
        /// Get score in game ongoing by gameID.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <returns>List score of 2 team : hometeam and visitorteam.</returns>
        public IEnumerable<ScoreInGame> GetGameScoreOnGoingByGameID(int gameID)
        {
            List<ScoreInGame> query = new List<ScoreInGame>();
            return query;
        }
        #endregion

        #region Get All GameTexts By GameID         Mlbには情報はない
        /// <summary>
        /// Get text in game that ongoing by gameID.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>All text in game.</returns>
        public IEnumerable<GameText> GetAllGameTextsByGameID(int gameID)
        {
            List<GameText> query = new List<GameText>();
            return query;
        }
        #endregion

        #region Get Game Score End By GameID        Mlbには情報はない
        /// <summary>
        /// Get score when game end by gameID.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>List game score.</returns>
        public IEnumerable<ScoreInGame> GetGameScoreEndByGameID(int gameID)
        {
            List<ScoreInGame> query = new List<ScoreInGame>();
            return query;
        }
        #endregion

        #region Get Game Comment By GameID        Mlbには情報はない
        /// <summary>
        /// Get round and game comment by gameID.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <returns>Round and Game Comment.</returns>
        public GameRoundComment GetGameCommentByGameID(int gameID)
        {
            GameRoundComment query = new GameRoundComment();
            return query;
        }
        #endregion

        #region Get PitcherInfo In GameLive By GameID
        /// <summary>
        /// Get win pitcher, lose pitcher, and save pitcher info when end game.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <returns>Player Info.</returns>
        public IEnumerable<PitcherGameEndInfo> GetPitcherInfoInGameLiveByGameID(int gameID)
        {
            List<PitcherGameEndInfo> result = new List<PitcherGameEndInfo>();

            return result;
        }
        #endregion

        #region Get Homerun In GameLive By GameID
        /// <summary>
        /// Get homerun info when game end by GameID.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <returns>List homerun info./returns>
        public IEnumerable<HomerunGameEndInfo> GetHomerunInGameLiveByGameID(int gameID)
        {
            List<HomerunGameEndInfo> query = new List<HomerunGameEndInfo>();

            return query;
        }
        #endregion

        #region Get ReliefInfo In GameLive By GameID
        /// <summary>
        /// Get relief info : pitcher and catcher when game end by GameID.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>List player : pitchers and catchers.</returns>
        public IEnumerable<ReliefInfoGameEndInfo> GetReliefInfoInGameLiveByGameID(int gameID)
        {
            List<ReliefInfoGameEndInfo> query = new List<ReliefInfoGameEndInfo>();
            return query;
        }
        #endregion

        #region Get Game Result By GameID
        /// <summary>
        /// Get game result when game end :
        ///     1 : Hometeam.
        ///     2 : Visitorteam.
        ///     3 : Draw.
        ///     4 : Game pending.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <returns>Game result : win - lose - draw - pending.</returns>
        public int GetGameResultByGameID(int gameID)
        {
            int intResult = 0;
            var info = (from rg in mlb.RealGameInfo
                        where rg.GameID == gameID
                        select rg).FirstOrDefault();

            if (info != null)
            {
                //試合結果ID ResultID 1=ホーム勝ち、2=ビジター勝ち、3=引き分け、4=中止				
                if (info.GameStateID == 4)
                {
                    if (info.HomeScore > info.VisitorScore)
                        intResult = 1;
                    else if (info.HomeScore < info.VisitorScore)
                        intResult = 2;
                    else
                        intResult = 3;

                }
                else
                {
                    intResult = 4;
                }
            }

            return intResult;
        }
        #endregion

        #region Get GameInfo In RightContent BeforeStart By Status, GameID
        /// <summary>
        /// Get game info in right content before start by GameID.
        /// </summary>
        /// <param name="status">0:  other then 0:</param>
        /// <param name="gameID">GameID</param>
        /// <returns>Game Info.</returns>
        public IEnumerable<GameInfoViewModel> GetGameInfoRightContentByStatus(int status, int gameID)
        {
            IEnumerable<GameInfoViewModel> query = default(IEnumerable<GameInfoViewModel>);


            int gameIDSub = Convert.ToInt32(gameID.ToString().Substring(0, 8));

            if (status == 0)
            {
                query = from ss in mlb.SeasonSchedule
                        join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                        where dg.GameDateJPN == gameIDSub && ss.HomeTeamID != gameIDSub && ss.VisitorTeamID != gameIDSub
                        select new GameInfoViewModel
                        {
                            GameID = ss.GameID,
                            HomeTeamID = ss.HomeTeamID.Value,
                            VisitorTeamID = ss.VisitorTeamID.Value,
                            HomeTeamName = ss.HomeTeamName,
                            VisitorTeamName = ss.VisitorTeamName,
                            Round = 0,  //情報がない
                            GameDate = (int)dg.GameDate,
                            Time = ss.Time
                        };
            }
            else
            {
                query = from rg in mlb.RealGameInfo
                        join rgh in mlb.RealGameInfoHeader on rg.RealGameInfoHeaderId equals rgh.RealGameInfoHeaderId
                        where rgh.GameDate == gameIDSub
                         && rg.GameID != gameID
                        select new GameInfoViewModel
                        {
                            GameID = rg.GameID,
                            HomeTeamID = rg.HomeTeamID.Value,
                            VisitorTeamID = rg.VisitorTeamID.Value,
                            HomeTeamName = rg.HomeTeamName,
                            VisitorTeamName = rg.VisitorTeamName,
                            Round = 0,
                            GameDate = rgh.GameDate.Value,
                            HomeTeamScore = rg.HomeScore,
                            VisitorTeamScore = rg.VisitorScore,
                            GameSituationID = MlbCommon.GetNpbGameSetSituationCode((short)rg.GameStateID),
                            Inning = 0,
                            BottomTop = ""
                        };
            }

            return query;
        }


        private RealGameInfo getGameInfo(int gameID)
        {
            var query = (from rg in mlb.RealGameInfo
                                       where rg.GameID == gameID
                                       orderby rg.CreatedDate descending
                                       select rg).FirstOrDefault();
            return query;
        }
        #endregion

        #endregion
    }
}
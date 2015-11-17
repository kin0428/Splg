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
 * Class		: NpbGameInformationController
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using Splg.Areas.Npb.Models;
using Splg.Areas.Npb.Models.ViewModel;
using Splg.Models;
using Splg.Models.Game.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
#endregion

namespace Splg.Areas.Npb.Controllers
{
    public class NpbGameInformationController : Controller
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
                int gameStatus = NpbCommon.GetStatusMatch(gameID.ToString()); 
                //int gameStatus = 0;

                //Use Session to store Status : need store long time and many request.
                Session["Status"] = gameStatus;
                @ViewBag.Status = gameStatus;

                NpbGameInfoViewModel gameInfo = new NpbGameInfoViewModel();

                //フォローユーザーの予想
                long memberId = 0;

                object currentUser = Session["CurrentUser"];
                if (currentUser != null)
                    memberId = Convert.ToInt64(currentUser.ToString());

                if (memberId > 0)
                {
                    gameInfo.FollowMembersBetToWin = Utils.GetExpectingMembers(com, (int)gameID, memberId, 4, Constants.NPB_SPORT_ID, 1);
                    gameInfo.FollowMembersBetToDraw = Utils.GetExpectingMembers(com, (int)gameID, memberId, 4, Constants.NPB_SPORT_ID, 3);
                    gameInfo.FollowMembersBetToLose = Utils.GetExpectingMembers(com, (int)gameID, memberId, 4, Constants.NPB_SPORT_ID, 2);
                }
                else
                {
                    gameInfo.FollowMembersBetToWin = Utils.GetAllUserPrediction(com, (int)gameID, 4, Constants.NPB_SPORT_ID, 1);
                    gameInfo.FollowMembersBetToDraw = Utils.GetAllUserPrediction(com, (int)gameID, 4, Constants.NPB_SPORT_ID, 3);
                    gameInfo.FollowMembersBetToLose = Utils.GetAllUserPrediction(com, (int)gameID, 4, Constants.NPB_SPORT_ID, 2);
                }

                gameInfo.GameInSeasonSchedule = GetGameInfoCommonByGameID(gameID.Value);
                gameInfo.ListRightContentGames = GetGameInfoRightContentByDate(gameID.Value);   
                gameInfo.PostedInfo = Splg.Controllers.PostedController.GetRecentPosts(2, Constants.NPB_SPORT_ID, null, 1);
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
        /// <returns>PartialView : _NpbGameInfoPlayerInfo</returns>        
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 15)]
        public ActionResult ShowGameInfoPlayerInfo(int? gameID)
        {
            if (gameID != null)
            {
                //Get status from this session, this session must be not null. 
                int status = (int)Session["Status"];

                //Set status for viewbag to use 1 request.
                ViewBag.Status = status;

                NpbGameInfoViewModel gameInfo = new NpbGameInfoViewModel();
                gameInfo.GameInSeasonSchedule = GetGameInfoCommonByGameID(gameID.Value);
                if (gameInfo.GameInSeasonSchedule != null)
                {
                    gameInfo.ListPlayerInfo = GetPlayerInfoSTByGameIDAndStatus(status, gameID.Value);
                    if (status == 0)
                    {
                        gameInfo.ListPreStartingPitcher = GetTeamInfoPSPByGameID(gameID.Value);
                        gameInfo.List5GamesInHistory = GetLast5GameInfoFromHistoryByGameID(gameID.Value, gameInfo.GameInSeasonSchedule.HomeTeamID, gameInfo.GameInSeasonSchedule.VisitorTeamID);
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
                        gameInfo.ListGameTextScore = GetGameTextScoreByGameID(gameID.Value);
                        ViewBag.GameResult = GetGameResultByGameID(gameID.Value);
                    }                    
                }
                else
                {
                    gameInfo = null;
                    ViewBag.Status = -1;
                }
                return PartialView("_NpbGameInfoPlayerInfo", gameInfo);
            }
            else
            {
                return PartialView("_NpbGameInfoPlayerInfo", null);
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
                IEnumerable<GameInfoViewModel> lstGame = GetGameInfoRightContentByDate(gameID.Value);
                return PartialView("_NpbRightGames", lstGame);
            }
            else
            {
                return PartialView("_NpbRightGames", null);
            }
        }
        #endregion

        #endregion

        #region Json Result

        #region Get Game Status By GameID
        /// <summary>
        /// Get status of game: load or not load partialview _NpbGameInfoPlayerInfo.cshtml.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>0: Not load _NpbGameInfoPlayerInfo.cshtml, status not changed.</returns>
        /// <returns>1: Must load _NpbGameInfoPlayerInfo.cshtml, status changed.</returns>
        [HttpPost]
        public JsonResult GetGameStatusByGameID(int gameID)
        {
            List<string> lstResult = new List<string>();
            var result = false;
            var statusNew = -1;
            if (gameID != null)
            {
                //Random rnd = new Random();
                //int gameStatus = rnd.Next(0, 3);               
                int gameStatus = NpbCommon.GetStatusMatch(gameID.ToString());
                statusNew = gameStatus;
                if (Session["Status"] != null && statusNew != (int)Session["Status"])
                {
                    //Update new status to session.
                    Session["Status"] = statusNew;
                    result = true;
                }
            }
            lstResult.Add(result.ToString());
            if (Session["Status"] != null)
                lstResult.Add(Session["Status"].ToString());
            return Json(lstResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Utilities

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
        /// <param name="gameID">GameID to define another games.</param>
        /// <returns>List of player in game.</returns>
        public IEnumerable<PlayerInfoInGame> GetPlayerInfoSTByGameIDAndStatus(int gameStatus, int gameID)
        {
            var query = default(IEnumerable<PlayerInfoInGame>);
            if (gameStatus == 0 || gameStatus == 2)
            {
                query = (from st in npb.StartingGameST
                         join ti in npb.TeamInfoST on st.StartingGameSTId equals ti.StartingGameSTId
                         join pi in npb.PlayerInfoST on ti.TeamInfoSTId equals pi.TeamInfoSTId
                         join df in npb.DefenceLocationMST on pi.StartPosition equals df.DefenceLocationID
                         where st.GameID == gameID
                         select new PlayerInfoInGame
                         {
                             PlayerInfoStarting = pi,
                             TeamID = ti.TeamID,
                             HV = ti.HV,
                             PositionName = df.NameS,
                             PlayerID = pi.PlayerID,
                             PositionID = df.DefenceLocationID
                         }).OrderBy(x => x.PlayerInfoStarting.CreatedDate).GroupBy(x => x.PlayerID).Select(x => x.FirstOrDefault());
            }
            else
            {
                query = (from go in npb.GameOrderGO
                        join to in npb.TeamInfoGO on go.GameOrderGOId equals to.GameOrderGOId
                        join pgo in npb.PlayerInfoGO on to.TeamInfoGOId equals pgo.TeamInfoGOId
                        join df in npb.DefenceLocationMST on pgo.Position equals df.DefenceLocationID
                        where go.GameID == gameID
                        select new PlayerInfoInGame
                        {
                            PlayerInfoGameOrder = pgo,
                            TeamID = to.TeamID,
                            HV = to.HV,
                            PositionName = df.NameS,
                            PlayerID = pgo.PlayerID,
                            PositionID = df.DefenceLocationID
                        }).OrderBy(x => x.PlayerInfoGameOrder.CreatedDate).GroupBy(x => x.PlayerID).Select(x => x.FirstOrDefault());;
            }
            return query;
        }
        #endregion

        #region Get Last 5 GameInfo From History By GameID
        /// <summary>
        /// Get 5 game info that 2 team took part in.
        /// Need HomeTeamID, VisitorTeamID, DateJPN to get data.
        /// </summary>
        /// <param name="gameID">Game id need get 2 teams info.</param>
        /// <returns>List game store in history.</returns>
        public IEnumerable<GameInHistory> GetLast5GameInfoFromHistoryByGameID(int gameID, int hTeamID, int vTeamID)
        {
            var query = default(IEnumerable<GameInHistory>);
            if (hTeamID != 0 && vTeamID != 0)
            {
                var strGameID = gameID.ToString().Substring(0,8);
                var date = Convert.ToInt32(strGameID);
                
                //Get old games in history.
                query = (from giss in npb.GameInfoSSN
                                  where ((giss.HTeamID == hTeamID || giss.HTeamID == vTeamID) && (giss.VTeamID.Value == vTeamID || giss.VTeamID == hTeamID) && giss.DateJPN < date)
                                  orderby giss.DateJPN descending
                                  select new GameInHistory
                                  {
                                      GameInfoSSN = giss,
                                      OddsHomeTeam = 0,
                                      OddsVisitor = 0,
                                      OddsDraw = 0
                                  }).Take(5).ToList();                               
            }
            return query;
        }
        #endregion

        #region Get GameInfo Common By GameID
        /// <summary>
        /// Get game info common : home team name, visitor team name,... by gameID. 
        /// </summary>
        /// <param name="gameID">Game ID</param>
        /// <returns>Game info.</returns>
        public GameInfoViewModel GetGameInfoCommonByGameID(int gameID)
        {
            var query = (from gi in npb.GameInfoSS
                         join ti in npb.TeamIconNpb on gi.HomeTeamID equals ti.TeamCD
                         join tiv in npb.TeamIconNpb on gi.VisitorTeamID equals tiv.TeamCD
                         where gi.ID == gameID
                         select new GameInfoViewModel
                         {
                             GameID = gi.ID,
                             GameDate = gi.GameDate,
                             Time = gi.Time,
                             HomeTeamID = gi.HomeTeamID.Value,
                             HomeTeamName = gi.HomeTeamName,
                             HomeTeamNameS = gi.HomeTeamNameS,
                             HomeTeamIcon = ti.TeamIcon,
                             VisitorTeamID = gi.VisitorTeamID.Value,
                             VisitorTeamName = gi.VisitorTeamName,
                             VisitorTeamNameS = gi.VisitorTeamNameS,
                             VisitorTeamIcon = tiv.TeamIcon
                         }).FirstOrDefault();
            return query;
        }
        #endregion

        #region Get Game Score OnGoing By GameID
        /// <summary>
        /// Get score in game ongoing by gameID.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <returns>List score of 2 team : hometeam and visitorteam.</returns>
        public IEnumerable<ScoreInGame> GetGameScoreOnGoingByGameID(int gameID)
        {
            var query = from gi in npb.GameInfoGTE
                             join ti in npb.TeamInfoGTE on gi.GameInfoGTEId equals ti.GameInfoGTEId into tmp
                             from tit in tmp.DefaultIfEmpty()
                             where gi.GameID == gameID 
                             select new ScoreInGame
                             {
                                 HV = tit.HV,
                                 TeamID = tit.ID,
                                 NameS = tit.NameS,
                                 Runs = tit.R.Value,
                                 Hits = tit.H.Value,
                                 Err = tit.E.Value,
                                 TeamInfoGTEID = tit.TeamInfoGTEId,
                                 Inning = (gi.Inning.Value == null ? 0 : gi.Inning.Value)
                             };           
            return query;
        }
        #endregion        

        #region Get GameTextScore By GameID
        /// <summary>
        /// Get text score when game end by GameID.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>All text that have scores.</returns>
        public IEnumerable<GameText> GetGameTextScoreByGameID(int gameID)
        {
            var flag = (short)1;
            var query = from gi in npb.GameInfoGTE
                        join txti in npb.TextInfoGTE on gi.GameInfoGTEId equals txti.GameInfoGTEId
                        join bat in npb.BatGTE on txti.TextInfoGTEId equals bat.TextInfoGTEId
                        join tigte in npb.TextGTE on bat.BatGTEId equals tigte.BatGTEId
                        where gi.GameID == gameID && tigte.F == flag
                        select new GameText
                        {
                            Round = txti.ID.ToString().Substring(0, 1),
                            RoundName = (txti.Name == null ? string.Empty : txti.Name),                                                       
                            TextOfTeam = (tigte.Text == null ? string.Empty : tigte.Text),
                            ID = txti.ID
                        };
            return query;
        }
        #endregion

        #region Get All GameTexts By GameID
        /// <summary>
        /// Get text in game that ongoing by gameID.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>All text in game.</returns>
        public IEnumerable<GameText> GetAllGameTextsByGameID(int gameID)
        {
            var query = default(IEnumerable<GameText>);
            
            var queryFirst = from gi in npb.GameInfoGTE
                    join txti in npb.TextInfoGTE on gi.GameInfoGTEId equals txti.GameInfoGTEId
                    join bat in npb.BatGTE on txti.TextInfoGTEId equals bat.TextInfoGTEId
                    join tigte in npb.TextGTE on bat.BatGTEId equals tigte.BatGTEId                   
                    where gi.GameID == gameID && txti.ID != 0
                    select new GameText
                    {
                        GameInfoGTEID = gi.GameInfoGTEId,
                        Round = txti.ID.ToString().Substring(0, 1),
                        RoundName = (txti.Name == null ? string.Empty : txti.Name),                     
                        TextOfTeam = (tigte.Text == null ? string.Empty : tigte.Text),                       
                        ID = txti.ID,
                        
                    };

            //Get last digit of ID.
            List<GameText> newList = new List<GameText>();
            foreach (var item in queryFirst.ToList())
            {
                GameText gText = new GameText();
                gText = item;
                gText.ID = Convert.ToInt32(item.ID.ToString().Substring(item.ID.ToString().Length - 1, 1));              
                newList.Add(gText);
            }

            query = from q1 in newList
                    join ti in npb.TeamInfoGTE on q1.GameInfoGTEID equals ti.GameInfoGTEId
                    where ti.HV != q1.ID
                    select new GameText
                    {
                        GameInfoGTEID = q1.GameInfoGTEID,
                        Round = q1.Round,
                        RoundName = q1.RoundName,
                        TeamID = ti.ID,
                        TeamNameS = ti.NameS,
                        TextOfTeam = q1.TextOfTeam,
                        ID = q1.ID,
                    };
            return query;
        }
        #endregion

        #region Get Game Score End By GameID
        /// <summary>
        /// Get score when game end by gameID.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>List game score.</returns>
        public IEnumerable<ScoreInGame> GetGameScoreEndByGameID(int gameID)
        {
            var query = default(IEnumerable<ScoreInGame>);
            var queryFirst = from gigte in npb.GameInfoGTE
                             join tigte in npb.TeamInfoGTE on gigte.GameInfoGTEId equals tigte.GameInfoGTEId
                             where gigte.GameID == gameID
                             select new
                             {
                                 tigte.ID,
                                 tigte.E,
                                 tigte.H
                             };
            if(!queryFirst.Any())
            {
                query = from gi in npb.GameInfoCO
                        join ti in npb.TeamInfoCO on gi.GameInfoCOId equals ti.GameInfoCOId                      
                        where gi.GameID == gameID
                        select new ScoreInGame
                        {
                            TeamID = ti.TeamID,
                            NameS = ti.TeamNameS,
                            HV = ti.HomeVisitor,
                            Runs = ti.TotalScore.Value,
                            Hits = 0,
                            Err = 0,
                            Inning = (gi.Inning.Value == null ? 0 : gi.Inning.Value),
                            TeamInfoCOID = ti.TeamInfoCOId
                        };
            }
            else
            {
                query = from gi in npb.GameInfoCO
                        join ti in npb.TeamInfoCO on gi.GameInfoCOId equals ti.GameInfoCOId
                        join q1 in queryFirst on ti.TeamID equals q1.ID
                        where gi.GameID == gameID
                        select new ScoreInGame
                        {
                            TeamID = ti.TeamID,
                            NameS = ti.TeamNameS,
                            HV = ti.HomeVisitor,
                            Runs = ti.TotalScore.Value,
                            Hits = q1.H.Value,
                            Err = q1.E.Value,
                            Inning = (gi.Inning.Value == null ? 0 : gi.Inning.Value),
                            TeamInfoCOID = ti.TeamInfoCOId
                        };
            }           
            return query;
        }
        #endregion

        #region Get Game Comment By GameID
        /// <summary>
        /// Get round and game comment by gameID.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <returns>Round and Game Comment.</returns>
        public GameRoundComment GetGameCommentByGameID(int gameID)
        {
            var tmp1 = (short)1;
            var tmp2 = (short)2;
            var query = (from gi in npb.GameInfoCO
                         join ti in npb.TeamInfoCO on gi.GameInfoCOId equals ti.GameInfoCOId
                         join ti1 in npb.TeamInfoCO on gi.GameInfoCOId equals ti1.GameInfoCOId
                         where gi.GameID == gameID && ti.HomeVisitor == tmp1 && ti1.HomeVisitor == tmp2
                         select new GameRoundComment
                         {
                             HomeTeamNameS = ti.TeamNameS,
                             VisitorTeamNameS = ti1.TeamNameS,
                             Round = gi.Round.Value,
                             GameComment = (gi.GameComment == null ? string.Empty : gi.GameComment),
                             Atendance = gi.Atendance.Value,
                             StartTime = gi.StartTime.Value,
                             EndTime = gi.EndTime.Value,
                             Inning = gi.Inning.Value,
                             TB = gi.TB.Value,
                             TotalScoreHTeam = ti.TotalScore.Value,
                             TotalScoreVTeam = ti1.TotalScore.Value
                         }).FirstOrDefault();
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
            var queryWin = from gl in npb.GameLiveGL
                           join ge in npb.GameEndInfoGL on gl.GameLiveGLId equals ge.GameLiveGLId
                           join wp in npb.WinPitcherGL on ge.GameEndInfoGLId equals wp.GameEndInfoGLId
                           join pi in npb.PersonInfo on wp.PlayerID equals pi.ID into tmp1
                           from pi1 in tmp1.DefaultIfEmpty()
                           join di in npb.Directory on pi1.DirectoryId equals di.DirectoryId into tmp2
                           from di1 in tmp2.DefaultIfEmpty()
                           where gl.GameID == gameID
                           select new PitcherGameEndInfo
                           {
                               TeamID = (di1.TeamID == null ? -1 : di1.TeamID),
                               TeamNameS = (di1.TeamNameS == null ? string.Empty : di1.TeamNameS),
                               PlayerID = wp.PlayerID,
                               PlayerNameS = wp.PlayerNameS,
                               Win = wp.Win.Value,
                               Lose = wp.Lose.Value,
                               Save = wp.Save.Value,
                               Type = 1
                           };

            var queryLose = from gl in npb.GameLiveGL
                            join ge in npb.GameEndInfoGL on gl.GameLiveGLId equals ge.GameLiveGLId
                            join wp in npb.LosePitcherGL on ge.GameEndInfoGLId equals wp.GameEndInfoGLId
                            join pi in npb.PersonInfo on wp.PlayerID equals pi.ID into tmp1
                            from pi1 in tmp1.DefaultIfEmpty()
                            join di in npb.Directory on pi1.DirectoryId equals di.DirectoryId into tmp2
                            from di1 in tmp2.DefaultIfEmpty()
                            where gl.GameID == gameID
                            select new PitcherGameEndInfo
                            {
                                TeamID = (di1.TeamID == null ? -1 : di1.TeamID),
                                TeamNameS = (di1.TeamNameS == null ? string.Empty : di1.TeamNameS),
                                PlayerID = wp.PlayerID,
                                PlayerNameS = wp.PlayerNameS,
                                Win = wp.Win.Value,
                                Lose = wp.Lose.Value,
                                Save = wp.Save.Value,
                                Type = 2
                            };

            var querySave = from gl in npb.GameLiveGL
                            join ge in npb.GameEndInfoGL on gl.GameLiveGLId equals ge.GameLiveGLId
                            join wp in npb.SavePitcherGL on ge.GameEndInfoGLId equals wp.GameEndInfoGLId
                            join pi in npb.PersonInfo on wp.PlayerID equals pi.ID into tmp1
                            from pi1 in tmp1.DefaultIfEmpty()
                            join di in npb.Directory on pi1.DirectoryId equals di.DirectoryId into tmp2
                            from di1 in tmp2.DefaultIfEmpty()
                            where gl.GameID == gameID
                            select new PitcherGameEndInfo
                            {
                                TeamID = (di1.TeamID == null ? -1 : di1.TeamID),
                                TeamNameS = (di1.TeamNameS == null ? string.Empty : di1.TeamNameS),
                                PlayerID = wp.PlayerID,
                                PlayerNameS = wp.PlayerNameS,
                                Win = wp.Win.Value,
                                Lose = wp.Lose.Value,
                                Save = wp.Save.Value,
                                Type = 3
                            };

            var query = queryWin.Union(queryLose);
            var result = query.Union(querySave);
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
            var query = from gl in npb.GameLiveGL
                        join hr in npb.HomerunInfoGL on gl.GameLiveGLId equals hr.GameLiveGLId
                        join pi in npb.PlayerGL on hr.HomerunInfoGLId equals pi.HomerunInfoGLId
                        join de in npb.DetailsGL on pi.PlayerGLId equals de.DetailsGLId
                        join si in npb.ScoreInfoGL on gl.GameLiveGLId equals si.GameLiveGLId
                        join pgl in npb.TeamGL on si.ScoreInfoGLId equals pgl.ScoreInfoGLId
                        where gl.GameID == gameID && pgl.HV == hr.HV
                        select new HomerunGameEndInfo
                        {
                            TeamID = (pgl.ID == null ? -1 : pgl.ID),
                            TeamNameS = (pgl.NameS == null ? string.Empty : pgl.NameS),
                            HV = hr.HV,
                            PlayerID = pi.ID,
                            PlayerNameS = pi.NameS,
                            TotalC = de.TotalC
                        };
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
            var queryFirst = from gl in npb.GameLiveGL
                             join ri in npb.ReliefInfoGL on gl.GameLiveGLId equals ri.GameLiveGLId
                             join pi in npb.PitcherGL on ri.ReliefInfoGLId equals pi.ReliefInfoGLId
                             join si in npb.ScoreInfoGL on gl.GameLiveGLId equals si.GameLiveGLId
                             join pgl in npb.TeamGL on si.ScoreInfoGLId equals pgl.ScoreInfoGLId
                             where gl.GameID == gameID && ri.HV == pgl.HV
                             select new ReliefInfoGameEndInfo
                             {
                                 PlayerID = pi.ID,
                                 TeamID = pgl.ID,
                                 TeamNameS = pgl.NameS,
                                 NameS = pi.NameS,
                                 Type = 1,
                                 HV = ri.HV
                             };
            var querySecond = from gl in npb.GameLiveGL
                              join ri in npb.ReliefInfoGL on gl.GameLiveGLId equals ri.GameLiveGLId
                              join ct in npb.CatcherGL on ri.ReliefInfoGLId equals ct.ReliefInfoGLId
                              join si in npb.ScoreInfoGL on gl.GameLiveGLId equals si.GameLiveGLId
                              join pgl in npb.TeamGL on si.ScoreInfoGLId equals pgl.ScoreInfoGLId
                              where gl.GameID == gameID && ri.HV == pgl.HV
                              select new ReliefInfoGameEndInfo
                              {
                                  PlayerID = ct.ID,
                                  TeamID = pgl.ID,
                                  TeamNameS = pgl.NameS,
                                  NameS = ct.NameS,
                                  Type = 2,
                                  HV = ri.HV
                              };
            var query = queryFirst.Union(querySecond);
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
            var query = (from gi in npb.GameInfoGTE
                         join ge in npb.GameEndInfoGTE on gi.GameInfoGTEId equals ge.GameInfoGTEId
                         where gi.GameID == gameID
                         select ge.ResultID).FirstOrDefault();
            intResult = query;
            return intResult;
        }
        #endregion        

        #region Get GameInfo In RightContent By GameID
        /// <summary>
        /// Get all games info in right content by date of GameID.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <returns>Game Info.</returns>
        public IEnumerable<GameInfoViewModel> GetGameInfoRightContentByDate(int gameID)
        {
            int gameIDSub = Convert.ToInt32(gameID.ToString().Substring(0, 8));
            var query = default(IEnumerable<GameInfoViewModel>);           
                query = from gi in npb.GameInfoSS
                        where gi.GameDate == gameIDSub && gi.ID != gameID
                        select new GameInfoViewModel
                        {
                            GameID = gi.ID,
                            HomeTeamID = gi.HomeTeamID.Value,
                            VisitorTeamID = gi.VisitorTeamID.Value,
                            HomeTeamName = gi.HomeTeamNameS,
                            VisitorTeamName = gi.VisitorTeamNameS,
                            Round = gi.Round.Value,
                            GameDate = gi.GameDate,
                            Time = gi.Time
                        };                 
            return query;
        }
        #endregion

        #endregion
    }
}
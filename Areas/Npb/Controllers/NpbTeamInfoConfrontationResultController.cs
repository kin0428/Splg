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
 * Class		: NpbTeamInfoConfrontationResultControllernh 
 * Developer	: Nguyen Minh Kiet
 * Updated date : 2015-03-06
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Areas.Npb.Models;
using Splg.Areas.Npb.Models.ViewModel;
using Splg.Areas.Npb.Models.ViewModel.InfosModel;
#endregion

namespace Splg.Areas.Npb.Controllers
{
    public class NpbTeamInfoConfrontationResultController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        NpbEntities npb = new NpbEntities();
        #endregion

        #region "Action Result"
        // GET: Npb/NpbTeamInfoConfrontationResult
        public ActionResult Index(string teamID)
        {
            int inTeamCD = int.MinValue;
            int.TryParse(teamID, out inTeamCD);
            DateTime inMMYYYY = DateTime.Now.Date;

            NpbTeamInfoConfrontationResultViewModel npbTeamInfoConfrontationResultViewModel = new NpbTeamInfoConfrontationResultViewModel();
            ///
            ///Get TeamInfoMST
            ///
            npbTeamInfoConfrontationResultViewModel.TeamInfo = GetTeamInfoMSTByTeamCD(inTeamCD);
            ///
            ///Get TeamInfoMSTs
            ///
            npbTeamInfoConfrontationResultViewModel.TeamInfoMSTs = GetTeamInfoMSTs();
            ///
            ///Get TeamStatsCardDifferenceInfos
            ///
            ///
            ///Set Default ResultGameInfos with teamsOpponentCD 
            ///Set Default month is month on date system.
            ///
            int inTeamsOpponentCD = int.MinValue;
            foreach (var item in npbTeamInfoConfrontationResultViewModel.TeamInfoMSTs)
            {
                if (item.TeamCD != inTeamCD)
                {
                    inTeamsOpponentCD = (int)item.TeamCD;
                    npbTeamInfoConfrontationResultViewModel.TeamsOpponent = GetTeamInfoMSTByTeamCD(inTeamsOpponentCD);
                    break;
                }
            }
            npbTeamInfoConfrontationResultViewModel.TeamStatsCardDifferenceInfos = GetTeamStatsCardDifferenceInfosByTeamCDAndTeamsOpponentCD(inTeamCD, inTeamsOpponentCD);
            ///
            ///Get ResultGameInfos
            ///  
            npbTeamInfoConfrontationResultViewModel.ResultGameInfos = GetResultGameInfos(inTeamCD, inTeamsOpponentCD, inMMYYYY);
            /////
            /////   Return ViewBag
            /////
            ViewBag.TeamID = inTeamCD;
            ViewBag.MonthFocus = inMMYYYY;
            ViewBag.TeamsOpponentCD = inTeamsOpponentCD;
            ViewBag.TeamInfoMenuTabActive = (int)NpbConstants.TeamInfoMenu.TabActive_3;
            return View(npbTeamInfoConfrontationResultViewModel);
        }
        #endregion

        #region Typed PartialViewResult

        // GET:  Npb/NpbTeamInfoConfrontationResult/GetTeamStatsCardDifferenceInfos/ 
        [HttpPost]
        public PartialViewResult GetTeamStatsCardDifferenceInfos(string teamID, string teamsOpponentCD)
        {
            int inTeamCD = int.MinValue;
            int inTeamsOpponentCD = int.MinValue;
            Int32.TryParse(teamID, out inTeamCD);
            Int32.TryParse(teamsOpponentCD, out inTeamsOpponentCD);
            DateTime inMMYYYY = DateTime.Now.Date;
            NpbTeamInfoConfrontationResultViewModel npbTeamInfoConfrontationResultViewModel = new NpbTeamInfoConfrontationResultViewModel();
            ///
            ///Get TeamInfoMST
            ///
            npbTeamInfoConfrontationResultViewModel.TeamsOpponent = GetTeamInfoMSTByTeamCD(inTeamsOpponentCD);
            ///
            ///Get TeamStatsCardDifferenceInfos
            ///
            npbTeamInfoConfrontationResultViewModel.TeamStatsCardDifferenceInfos = GetTeamStatsCardDifferenceInfosByTeamCDAndTeamsOpponentCD(inTeamCD, inTeamsOpponentCD);
            ///
            ///Get ResultGameInfos
            ///          
            npbTeamInfoConfrontationResultViewModel.ResultGameInfos = GetResultGameInfos(inTeamCD, inTeamsOpponentCD, inMMYYYY);
        
            /////
            /////   Return ViewBag
            /////
            ViewBag.TeamID = inTeamCD;
            ViewBag.MonthFocus = inMMYYYY;
            ViewBag.TeamsOpponentCD = inTeamsOpponentCD;
            return PartialView("_NpbTeamInfoConfrontationResult", npbTeamInfoConfrontationResultViewModel);      
        }

        // GET:  Npb/NpbTeamInfoConfrontationResult/GetTeamStatsCardDifferenceInfos/ 
        [HttpPost]
        public PartialViewResult GetResultGameInfosByMonth(string teamID, string teamsOpponentCD, string month)
        {
            int inTeamCD = int.MinValue;
            int inTeamsOpponentCD = int.MinValue;
            DateTime inMMYYYY = DateTime.Parse(month);
            Int32.TryParse(teamID, out inTeamCD);
            Int32.TryParse(teamsOpponentCD, out inTeamsOpponentCD);

            NpbTeamInfoConfrontationResultViewModel npbTeamInfoConfrontationResultViewModel = new NpbTeamInfoConfrontationResultViewModel();
            ///
            ///Get ResultGameInfos
            ///              
            npbTeamInfoConfrontationResultViewModel.ResultGameInfos = GetResultGameInfos(inTeamCD, inTeamsOpponentCD, inMMYYYY);

            /////
            /////   Return ViewBag
            /////
            ViewBag.TeamID = inTeamCD;
            ViewBag.MonthFocus = inMMYYYY;
            ViewBag.TeamsOpponentCD = inTeamsOpponentCD;
            return PartialView("_NpbTeamInfoResultGameInfos", npbTeamInfoConfrontationResultViewModel);
        }

        #endregion

        #region Get TeamInfoMST
        /// <summary>
        /// Join 2 table TeamInfoMSTs,TeamIconMSTs  to get :
        /// 1. TeamInfoMSTs : TeamCD, Team, ShortNameTeam  
        /// 2. TeamIconMSTs : TeamIcon
        /// </summary>       
        /// <returns>List リーグIDが、1 ,2 を抽出し、1からのデータをチームID順に表示する。 </returns>
        public NpbTeamInfoConfrontationResultInfos GetTeamInfoMSTByTeamCD(int inTeamCD)
        {
            NpbTeamInfoConfrontationResultInfos varTeamInfoMST = (from teamInfoMSTs in npb.TeamInfoMST
                                                                  join teamIconMSTs in npb.TeamIconNpb on teamInfoMSTs.TeamCD equals teamIconMSTs.TeamCD
                                                                  where teamInfoMSTs.TeamCD == inTeamCD
                                                                  select new NpbTeamInfoConfrontationResultInfos
                                                                  {
                                                                      TeamCD = teamInfoMSTs.TeamCD,
                                                                      Team = teamInfoMSTs.Team,
                                                                      TeamIcon = teamIconMSTs.TeamIcon,
                                                                      ShortNameTeam = teamInfoMSTs.ShortNameTeam
                                                                  }).FirstOrDefault();
            return varTeamInfoMST;
        }

        #endregion

        #region Get TeamInfoMSTs
        /// <summary>
        /// Join 2 table TeamInfoMSTs,TeamIconMSTs  to get :
        /// 1. TeamInfoMSTs : TeamCD, Team, ShortNameTeam  
        /// 2. TeamIconMSTs : TeamIcon
        /// </summary>       
        /// <returns>List リーグIDが、1 ,2 を抽出し、1からのデータをチームID順に表示する。 </returns>
        public IEnumerable<NpbTeamInfoConfrontationResultInfos> GetTeamInfoMSTs()
        {
            IEnumerable<NpbTeamInfoConfrontationResultInfos> varTeamInfoMSTs = (from teamInfoMSTs in npb.TeamInfoMST
                                                                                join teamIconMSTs in npb.TeamIconNpb on teamInfoMSTs.TeamCD equals teamIconMSTs.TeamCD
                                                                                where (teamInfoMSTs.LeagueID == 1
                                                                                      || teamInfoMSTs.LeagueID == 2)
                                                                                orderby teamIconMSTs.SortOrd, teamInfoMSTs.TeamCD
                                                                                select new NpbTeamInfoConfrontationResultInfos
                                                                   {
                                                                       TeamCD = teamInfoMSTs.TeamCD,
                                                                       Team = teamInfoMSTs.Team,
                                                                       TeamIcon = teamIconMSTs.TeamIcon,
                                                                       ShortNameTeam = teamInfoMSTs.ShortNameTeam
                                                                   });
            return varTeamInfoMSTs;
        }

        #endregion

        #region Get TeamStatsCardDifferenceInfos
        /// <summary>
        /// Join 1 table TeamStatsCardDifferenceInfoes  to get :
        /// 1. TeamStatsCardDifferenceInfoes : TeamsOpponentCD ,Game, Run, StrikeOut,StolenBase, Win,  PointLost, BaseOnBall, Error, Lose, Hit, Homerun
        /// HitByPitch, BattingAverage, BattingAverage, Draw, DoublePlay, EarnedRunAverage.
        /// </summary>       
        /// <returns>List チーム別対戦成績でクリックされたチームID と、対戦チームID：TeamsOpponentCD が同じデータをアクセスした結果を表示 </returns>
        public IEnumerable<NpbTeamInfoConfrontationResultInfos> GetTeamStatsCardDifferenceInfosByTeamCDAndTeamsOpponentCD(int inTeamCD, int inTeamsOpponentCD)
        {
            IEnumerable<NpbTeamInfoConfrontationResultInfos> varTeamStatsCardDifferenceInfoes = (from teamStatsCardDifferenceInfoes in npb.TeamStatsCardDifferenceInfo
                                                                                                 join teamStatsCardDifferences in npb.TeamStatsCardDifference on teamStatsCardDifferenceInfoes.TeamStatsCardDifferenceId equals teamStatsCardDifferences.TeamStatsCardDifferenceId
                                                                                                 join teamStatsCardDifferenceHeaders in npb.TeamStatsCardDifferenceHeader on teamStatsCardDifferences.TeamStatsCardDifferenceHeaderId equals teamStatsCardDifferenceHeaders.TeamStatsCardDifferenceHeaderId
                                                                                                 where teamStatsCardDifferences.TeamCD == inTeamCD
                                                                                                    && teamStatsCardDifferenceInfoes.TeamsOpponentCD == inTeamsOpponentCD
                                                                                                 orderby teamStatsCardDifferenceHeaders.Matchday
                                                                                                 select new NpbTeamInfoConfrontationResultInfos
                                                                                                 {
                                                                                                     TeamsOpponentCD = teamStatsCardDifferenceInfoes.TeamsOpponentCD,
                                                                                                     GameDate = teamStatsCardDifferenceHeaders.Matchday.ToString(),
                                                                                                     Game = teamStatsCardDifferenceInfoes.Game,
                                                                                                     Run = teamStatsCardDifferenceInfoes.Run,
                                                                                                     StrikeOut = teamStatsCardDifferenceInfoes.StrikeOut,
                                                                                                     StolenBase = teamStatsCardDifferenceInfoes.StolenBase,
                                                                                                     Win = teamStatsCardDifferenceInfoes.Win,
                                                                                                     PointLost = teamStatsCardDifferenceInfoes.PointLost,
                                                                                                     BaseOnBall = teamStatsCardDifferenceInfoes.BaseOnBall,
                                                                                                     Error = teamStatsCardDifferenceInfoes.Error,
                                                                                                     Lose = teamStatsCardDifferenceInfoes.Lose,
                                                                                                     Hit = teamStatsCardDifferenceInfoes.Hit,
                                                                                                     Homerun = teamStatsCardDifferenceInfoes.Homerun,
                                                                                                     HitByPitch = teamStatsCardDifferenceInfoes.HitByPitch,
                                                                                                     BattingAverage = teamStatsCardDifferenceInfoes.BattingAverage,
                                                                                                     Draw = teamStatsCardDifferenceInfoes.Draw,
                                                                                                     DoublePlay = teamStatsCardDifferenceInfoes.DoublePlay,
                                                                                                     EarnedRunAverage = teamStatsCardDifferenceInfoes.EarnedRunAverage
                                                                                                 });
            return varTeamStatsCardDifferenceInfoes;
        }

        #endregion

        #region Get ResultGameInfos
        /// <summary>
        /// Join 1 table RealGameInfoRootRGIs, GameInfoRGIs,GameResultInfoRGIs to get :
        /// 1. TeamStatsCardDifferenceInfoes : GameID ,HomeTeam, ShortDate, GameDate, ShortNameStadium, WinTeamCD,
        /// WinTeamSymbol, WinTeamChar, ResultTotalScore, WinningPlayerID, WinningShortNamePlayer, LosingPlayerID
        /// LosingShortNamePlayer, StartTime, EndTime
        /// </summary>       
        /// <returns> </returns>                                    
        public IEnumerable<NpbTeamInfoConfrontationResultInfos> GetResultGameInfos(int inHomeTeam, int inVisitorTeam, DateTime inMMYYYY)
        {
            int today = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            var varGameStatus = GetResultGameInfosStatus(inHomeTeam, inVisitorTeam,DateTime.Now);

            IEnumerable<NpbTeamInfoConfrontationResultInfos> varResultGameInfos = (from seasonScheduleSS in npb.SeasonScheduleSS
                                                                                   join monthGroupSS in npb.MonthGroupSS on seasonScheduleSS.SeasonScheduleSSId equals monthGroupSS.SeasonScheduleSSId
                                                                                   join gameInfoSS in npb.GameInfoSS on monthGroupSS.MonthGroupSSId equals gameInfoSS.MonthGroupSSId 
                                                                                   join gameInfoRGIs in npb.GameInfoRGI on gameInfoSS.ID equals gameInfoRGIs.GameID into gameIfRGI
                                                                                   from gameIf in gameIfRGI.DefaultIfEmpty()
                                                                                   join realGameInfoRootRGIs in npb.RealGameInfoRootRGI on gameIf.RealGameInfoRootRGIId equals realGameInfoRootRGIs.RealGameInfoRootRGIId into realGameIf
                                                                                   from realGame in realGameIf.DefaultIfEmpty()
                                                                                   join gameResultInfoRGI in npb.GameResultInfoRGI on gameIf.RealGameInfoRootRGIId equals gameResultInfoRGI.RealGameInfoRootRGIId into gameResultIf
                                                                                   from gameResult in gameResultIf.DefaultIfEmpty()
                                                                                   join winningPitcherRGI in npb.WinningPitcherRGI on gameResult.GameResultInfoRGIId equals winningPitcherRGI.GameResultInfoRGIId into winning
                                                                                   from winningPitcher in winning.DefaultIfEmpty()
                                                                                   join losingPitcherRGI in npb.LosingPitcherRGI on gameResult.GameResultInfoRGIId equals losingPitcherRGI.GameResultInfoRGIId into losing
                                                                                   from losingPitcher in losing.DefaultIfEmpty()
                                                                                   join savingPitcherRGI in npb.SavingPitcherRGI on gameResult.GameResultInfoRGIId equals savingPitcherRGI.GameResultInfoRGIId into saving
                                                                                   from savingPitcher in saving.DefaultIfEmpty()
                                                                                   where                                                                                                                                                          
                                                                                   ((gameInfoSS.HomeTeamID ==inHomeTeam &&  gameInfoSS.VisitorTeamID ==inVisitorTeam)
                                                                                      || (gameInfoSS.HomeTeamID ==inVisitorTeam && gameInfoSS.VisitorTeamID ==inHomeTeam))                                                                                                                                                                       
                                                                                    && (seasonScheduleSS.Year == inMMYYYY.Year)                                                                                                                                                  
                                                                                   select new NpbTeamInfoConfrontationResultInfos
                                                                                   {
                                                                                       GameID = gameInfoSS.ID,
                                                                                       HomeTeam = inHomeTeam,
                                                                                       VisitorTeam = inVisitorTeam,
                                                                                       ShortDate = realGame.Matchday == null ?  gameInfoSS.GameDate.ToString():realGame.Matchday.ToString(),
                                                                                       GameDate = realGame.Matchday == null ? gameInfoSS.GameDate.ToString() : realGame.Matchday.ToString(),
                                                                                       ShortNameStadium = string.IsNullOrEmpty(gameIf.ShortNameStadium) ? gameInfoSS.StadiumNameS : gameIf.ShortNameStadium,
                                                                                       WinTeamCD = gameResult.WinTeamCD,
                                                                                       WinTeamSymbol = gameResult.WinTeamCD == inHomeTeam ? "○" : gameResult.WinTeamCD == inVisitorTeam ? "●" : "△",
                                                                                       WinTeamChar = gameResult.WinTeamCD == inHomeTeam ? "勝" : gameResult.WinTeamCD == inVisitorTeam ? "負" : "-",
                                                                                       HomeTotalScore = (from scoreRGIs in npb.ScoreRGI
                                                                                                         where (scoreRGIs.RealGameInfoRootRGIId == realGame.RealGameInfoRootRGIId
                                                                                                              && gameIf.HomeTeam == scoreRGIs.TeamCD)
                                                                                                         select scoreRGIs).FirstOrDefault().TotalScore.Value,
                                                                                       VisitorTotalScore = (from scoreRGIs in npb.ScoreRGI
                                                                                                            where (scoreRGIs.RealGameInfoRootRGIId == realGame.RealGameInfoRootRGIId
                                                                                                                 && gameIf.VisitorTeam == scoreRGIs.TeamCD)
                                                                                                            select scoreRGIs).FirstOrDefault().TotalScore,
                                                                                       InningHomeTotalScore = (from teamInfoGTS in npb.TeamInfoGTS
                                                                                                               join gameInfoGTS in npb.GameInfoGTS on teamInfoGTS.GameInfoGTSId equals gameInfoGTS.GameInfoGTSId
                                                                                                               where (gameIf.GameID == gameInfoGTS.GameID
                                                                                                                 && teamInfoGTS.ID == inHomeTeam)
                                                                                                               select teamInfoGTS).FirstOrDefault().R.Value,
                                                                                       InningVisitorTotalScore = (from teamInfoGTS in npb.TeamInfoGTS
                                                                                                                  join gameInfoGTS in npb.GameInfoGTS on teamInfoGTS.GameInfoGTSId equals gameInfoGTS.GameInfoGTSId
                                                                                                                  where (gameIf.GameID == gameInfoGTS.GameID
                                                                                                                 && teamInfoGTS.ID == inVisitorTeam)
                                                                                                                  select teamInfoGTS).FirstOrDefault().R.Value,
                                                                                       WinningPlayerID = winningPitcher.PlayerCD,
                                                                                       WinningShortNamePlayer = winningPitcher.ShortNamePlayer,
                                                                                       LosingPlayerID = losingPitcher.PlayerCD,
                                                                                       LosingShortNamePlayer = losingPitcher.ShortNamePlayer,
                                                                                       SavingPlayerID = savingPitcher.PlayerCD,
                                                                                       SavingShortNamePlayer = savingPitcher.ShortNamePlayer,
                                                                                       StartTime = gameInfoSS.Time,
                                                                                       InMonth = monthGroupSS.No,
                                                                                       InYear = seasonScheduleSS.Year
                                                                                   } into gameInfor                                                                                
                                                                                   select gameInfor
                                                                                   ).ToList();         
            var result = default(IEnumerable<NpbTeamInfoConfrontationResultInfos>);
            if (varGameStatus == null || varResultGameInfos == null)
                return result;
        
            var query = (from gameInfos in varResultGameInfos
                         join gameStatus in varGameStatus on gameInfos.GameID equals gameStatus.GameID                                          
                         select new NpbTeamInfoConfrontationResultInfos
           {
               GameStatus = gameStatus != null ? gameStatus.GameStatus : null,
               GameID = gameInfos.GameID,
               HomeTeam = gameInfos.HomeTeam,
               VisitorTeam = gameInfos.VisitorTeam,
               ShortDate = gameInfos.GameDate !=null?gameInfos.GameDate.ToString():string.Empty,
               GameDate = gameInfos.GameDate != null ? gameInfos.GameDate.ToString() : string.Empty,
               ShortNameStadium = gameInfos.ShortNameStadium,
               WinTeamCD = gameStatus != null ? gameStatus.GameStatus == 2 ? gameInfos.WinTeamCD : null : null,
               WinTeamSymbol = gameStatus != null ? gameStatus.GameStatus == 2 ? (gameInfos.WinTeamCD == inHomeTeam ? "○" : gameInfos.WinTeamCD == inVisitorTeam ? "●" : "△") : "-" : "",
               WinTeamChar = gameStatus != null ? gameStatus.GameStatus == 2 ? (gameInfos.WinTeamCD == inHomeTeam ? "勝" : gameInfos.WinTeamCD == inVisitorTeam ? "負" : "-") : "-" : "",
               NameLink = gameStatus != null ? gameStatus.GameStatus == 0 ? "" : "結果" : "",
               ViewTotalScore = gameStatus != null ? gameStatus.GameStatus == 0 ? "試合前" :
                             gameStatus.GameStatus == 1 ?
                            (gameInfos.InningHomeTotalScore == null ? "0" : gameInfos.InningHomeTotalScore.ToString()) + 
                            (gameInfos.InningVisitorTotalScore == null ? "-0" : " - " + gameInfos.InningVisitorTotalScore.ToString()) :
                            (gameInfos.HomeTotalScore == null ? "0" :  gameInfos.HomeTotalScore.ToString()) +
                            (gameInfos.VisitorTotalScore == null ? "-0" : "-" + gameInfos.VisitorTotalScore.ToString()) : "試合前",
               AndSymbol = gameStatus != null ? gameStatus.GameStatus == 2 && gameInfos.SavingPlayerID != null ? "S" : "" : "",
               WinningPlayerID = gameStatus != null ? gameStatus.GameStatus == 2 ? gameInfos.WinningPlayerID : null : null,
               WinningShortNamePlayer = gameStatus != null ? gameStatus.GameStatus == 2 ? gameInfos.WinningShortNamePlayer : null : null,
               LosingPlayerID = gameStatus != null ? (gameStatus.GameStatus == 2 && gameInfos.SavingPlayerID != null) ? gameInfos.LosingPlayerID : null : null,
               LosingShortNamePlayer = gameStatus != null ? gameStatus.GameStatus == 2 ? gameInfos.LosingShortNamePlayer : null : null,
               SavingPlayerID = gameStatus != null ? gameStatus.GameStatus == 2 ? gameInfos.SavingPlayerID : null : null,
               SavingShortNamePlayer = gameStatus != null ? gameStatus.GameStatus == 2 ? gameInfos.SavingShortNamePlayer : null : null,
               StartTime = gameInfos.StartTime,
               InMonth = gameInfos.InMonth,
               InYear = gameInfos.InYear
           }).ToList();               
            return query;
        }
        #endregion

        #region Get GetResultGameInfosStatus
        /// <summary>
        /// Join 1 table RealGameInfoRootRGIs, GameInfoRGIs,GameResultInfoRGIs to get :
        /// 1. TeamStatsCardDifferenceInfoes : GameID ,HomeTeam, ShortDate, GameDate, ShortNameStadium, WinTeamCD,
        /// WinTeamSymbol, WinTeamChar, ResultTotalScore, WinningPlayerID, WinningShortNamePlayer, LosingPlayerID
        /// LosingShortNamePlayer, StartTime, EndTime
        /// </summary>       
        /// <returns> </returns>                                    
        public List<NpbTeamInfoConfrontationResultInfos> GetResultGameInfosStatus(int inHomeTeam, int inVisitorTeam, DateTime inMMYYYY)
        {
            int today = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
            List<NpbTeamInfoConfrontationResultInfos> gameStatus = (from seasonScheduleSS in npb.SeasonScheduleSS
                                                                           join monthGroupSS in npb.MonthGroupSS on seasonScheduleSS.SeasonScheduleSSId equals monthGroupSS.SeasonScheduleSSId
                                                                           join gameInfoSS in npb.GameInfoSS on monthGroupSS.MonthGroupSSId equals gameInfoSS.MonthGroupSSId
                                                                           join gameInfoRGIs in npb.GameInfoRGI on gameInfoSS.ID equals gameInfoRGIs.GameID into gameIfRGI
                                                                           from gameInfo in gameIfRGI.DefaultIfEmpty()
                                                                           join realGameInfoRootRGIs in npb.RealGameInfoRootRGI on gameInfo.RealGameInfoRootRGIId equals realGameInfoRootRGIs.RealGameInfoRootRGIId into realGameIf
                                                                           from realGame in realGameIf.DefaultIfEmpty()
                                                                           join gameResultInfoRGI in npb.GameResultInfoRGI on realGame.RealGameInfoRootRGIId equals gameResultInfoRGI.RealGameInfoRootRGIId into gameResultIf
                                                                           from gameResult in gameResultIf.DefaultIfEmpty()
                                                                    where
                                                                    ((gameInfoSS.HomeTeamID == inHomeTeam && gameInfoSS.VisitorTeamID == inVisitorTeam) || (gameInfoSS.HomeTeamID == inVisitorTeam && gameInfoSS.VisitorTeamID == inHomeTeam))                                                               
                                                                   && (inMMYYYY == null || seasonScheduleSS.Year == inMMYYYY.Year)                                                                                                                                
                                                                           select new NpbTeamInfoConfrontationResultInfos
                                                                           {
                                                                               GameStatus = (gameInfo.GameID == null) ? 0 : (today < realGame.Matchday ? 0 : (!string.IsNullOrEmpty(gameInfo.GameSetSituationCD) ? 2 : gameInfo.Inning == 0 ? 0 : 1)),
                                                                               GameID = gameInfoSS.ID,
                                                                               GameDate =realGame.Matchday!=null?realGame.Matchday.ToString(): gameInfoSS.GameDate.ToString(),
                                                                               HomeTeam = gameInfoSS.HomeTeamID,
                                                                               VisitorTeam = gameInfoSS.VisitorTeamID
                                                                           }).ToList();

          
            return gameStatus;
        }
        #endregion
    }
}
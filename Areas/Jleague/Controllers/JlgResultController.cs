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
 * Namespace	: Splg.Areas.Jleague.Controllers
 * Class		: JlgResultController
 * Developer	: Nguyen Minh Kiet
 * Updated date : 2015-04-16
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Core.Extend;
namespace Splg.Areas.Jleague.Controllers
{
    #region Menu config description
    /// <summary>
    /// KietNM / 2015-04-11
    ///                  Menu main jleague 
    /// ViewBag.JleagueMenu : Using choice JleagueMenu
    /// ViewBag.JleagueMenu = 1 => Jleague Home 画面設計書 (JリーグTOP画面）: 10-1
    /// ViewBag.JleagueMenu = 2 => Jleague J1Top 画面設計書 (J1TOP画面）: 10-2
    /// ViewBag.JleagueMenu = 3 => Jleague J2Top 画面設計書 (J2TOP画面）: 10-3
    /// ViewBag.JleagueMenu = 4 => Jleague NabiscoTop 画面設計書 (ナビスコCTOP画面）: 10-4
    /// ViewBag.JleagueMenu = 5 => Jleague JlgNews 画面設計書 (ニュースリスト(Jリーグ)画面）: 10-6

    ///                  Menu sub jleague 
    /// ViewBag.JleagueSubMenu : Using choice sub Jleague
    /// ViewBag.JleagueSubMenu = 1 => Jleague Home JリーグTOP : 10_2; 10_3
    /// ViewBag.JleagueSubMenu = 2 => Jleague 日程・結果 10_2_1 ; 10_3_1
    /// ViewBag.JleagueSubMenu = 3 => Jleague 順位表: 10-2_2 ; 10_3_2
    /// ViewBag.JleagueSubMenu = 4 => Jleague 戦績表: 10_2_3 ; 10_3_3
    /// ViewBag.JleagueSubMenu = 5 => Jleague 個人成績: 10_2_4 ; 10_3_4
    /// ViewBag.JleagueSubMenu = 6 => Jleague チーム情報: 10-2_5 ; 10_3_5
    /// </summary>
    #endregion

    public class JlgResultController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        JlgEntities jlg = new JlgEntities();
        #endregion

        #region "Action Result"
        //
        // GET: /Jleague/JlgResult/
        public ActionResult Index()
        {
            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);

            ViewBag.JleagueMenu = jType == 1 ? 2 : jType == 2 ? 3 : jType == 3 ? 4 : int.MinValue;         
            ViewBag.JleagueSubMenu = 4;
            ViewBag.JType = jType;

            int inTeamCd = int.MinValue;
            int inGameKind = jType == 1 ? JlgConstants.JLG_GAMEKIND_J1 : jType == 2 ? JlgConstants.JLG_GAMEKIND_J2 : jType == 3 ? JlgConstants.JLG_GAMEKIND_NABISCO : int.MinValue;
            int inputDate = DateTime.Now.ParseToInt();
            int SeasonID = JlgCommon.GetSeasonID(inputDate,jType);
            
            var model = new JlgResultViewModel
            {
                JlgResultTeamInfos = GetTeamInfosResult(null, inGameKind, null)
            };

            if (model.JlgResultTeamInfos != null && model.JlgResultTeamInfos.Any())
            {
                var item = model.JlgResultTeamInfos.FirstOrDefault();
                inTeamCd = item.TeamID;
            }
            model.JlgResultTeamInfo = GetTeamInfoByTeamID(inTeamCd);

            model.JlgResultGameInfos = GetGameInfosResult(null, inGameKind, inTeamCd, null, null, null);

            model.CurrentSeasonID = SeasonID;
            return View(model);
        }
        #endregion

        #region Typed PartialViewResult

        [HttpPost]
        public PartialViewResult GetJleagueGameInfosResult(string teamID, string sJtype,int? seasonID = null)
        {
            int jType = sJtype.Equals("2") ? 1 : sJtype.Equals("3") ? 2 : 3;
          
            ViewBag.JleagueMenu = jType == 1 ? 2 : 3;
            ViewBag.JleagueSubMenu = 4;
            ViewBag.JType = jType;

            int inTeamCd = int.MinValue;
            int inGameKind = jType == 1 ? JlgConstants.JLG_GAMEKIND_J1 : jType == 2 ? JlgConstants.JLG_GAMEKIND_J2 : jType == 3 ? JlgConstants.JLG_GAMEKIND_NABISCO : int.MinValue;
            Int32.TryParse(teamID, out inTeamCd);
            var model = new JlgResultViewModel
            {
                JlgResultTeamInfo = GetTeamInfoByTeamID(inTeamCd),
                JlgResultGameInfos = GetGameInfosResult(seasonID, inGameKind, inTeamCd, null, null, null)
            };

            return PartialView("_JleagueGameInfosResult", model);
        }

        #endregion

        #region  GetTeamInfo By TeamID
        public JlgTeamInfos GetTeamInfoByTeamID(int teamID)
        {
            JlgTeamInfos varTeamInfoMST = (from teamInfoTE in jlg.TeamInfoTE
                                           join teamIconJlg in jlg.TeamIconJlg on teamInfoTE.TeamID equals teamIconJlg.TeamCD into tmIcon
                                           from teamIcon in tmIcon.DefaultIfEmpty()
                                           where teamInfoTE.TeamID == teamID
                                           select new JlgTeamInfos
                                           {
                                               ////Team infors
                                               TeamID = teamInfoTE.TeamID,
                                               TeamName = teamInfoTE.TeamName,
                                               TeamNameS = teamInfoTE.TeamNameS,
                                               TeamNo = teamInfoTE.TeamNo,
                                               TeamIcon = teamIcon.TeamIcon
                                           }).FirstOrDefault();
            return varTeamInfoMST;
        }

        #endregion

        #region Get GetTeamInfosResult
        /// <summary>
        /// Join 7 table GameKindTeamGT, CategoryGT, TeamInfoGT ,TeamInfoTE, SecGameRecordReportMR,GameCategoryMR,GameInfoMR  to get :
        /// Return game infors, FirstTeam infors, team SecondTeam infors
        /// </summary>       
        /// <returns>List data </returns>
        public IEnumerable<JlgGameInfos> GetGameInfosResult(int? inSeasonID, int? inGameKind, int? inTeamCD, int? inGameID, int? inHomeTeamID, int? inVisitorTeamID)
        {
            IEnumerable<JlgGameInfos> infos = (from gameKind in jlg.GameKindTeamGT
                                               join category in jlg.CategoryGT on gameKind.GameKindTeamGTId equals category.GameKindTeamGTId
                                               join team in jlg.TeamInfoGT on category.CategoryGTId equals team.CategoryGTId
                                               join tblSecGame in
                                                   (
                                                        from secGame in jlg.SecGameRecordReportMR
                                                        join categoryMR in jlg.GameCategoryMR on secGame.SecGameRecordReportMRId equals categoryMR.SecGameRecordReportMRId
                                                        join gameMR in jlg.GameInfoMR on categoryMR.GameCategoryMRId equals gameMR.GameCategoryMRId
                                                        join teamHV1 in jlg.TeamInfoTE on secGame.TeamID equals teamHV1.TeamID
                                                        join teamHV1Icon in jlg.TeamIconJlg on teamHV1.TeamID equals teamHV1Icon.TeamCD into teamHVIcon1
                                                        from teamIconHV1 in teamHVIcon1.DefaultIfEmpty()
                                                        join teamHV2 in jlg.TeamInfoTE on gameMR.ID equals teamHV2.TeamID
                                                        join teamHV2Icon in jlg.TeamIconJlg on teamHV2.TeamID equals teamHV2Icon.TeamCD into teamHVIcon2
                                                        from teamIconHV2 in teamHVIcon2.DefaultIfEmpty()
                                                        where (inGameKind == null || secGame.GameKindID == inGameKind)
                                                              && (inSeasonID == null || categoryMR.SeasonID == inSeasonID)
                                                              && (inTeamCD == null || inTeamCD == secGame.TeamID)
                                                        select new JlgGameInfos
                                                        {
                                                            ////GameInfos
                                                            TeamID = secGame.TeamID,
                                                            ID = gameMR.ID,
                                                            SeasonID = categoryMR.SeasonID,
                                                            SeasonName = categoryMR.SeasonName,
                                                            Sec = gameMR.Sec,
                                                            GameID = gameMR.GameID.Value,
                                                            GameDate = gameMR.GameID.Value,
                                                            HV = gameMR.HV.Value,
                                                            GameNameS = gameMR.NameS,
                                                            StadiumID = gameMR.StadiumID.Value,
                                                            StadiumNameS = gameMR.StadiumNameS,
                                                            GameResult = gameMR.GameResult,
                                                            GameResultSymbol = gameMR.GameResult == "W" ? "○" : gameMR.GameResult == "L" ? "●" : gameMR.GameResult == "D" ? "△" : "",
                                                            Score = gameMR.Score.Value,
                                                            Lost = gameMR.Lost.Value,
                                                            //// FirstTeam infors
                                                            FirstTeamID = teamHV1.TeamID ,
                                                            FirstTeamName =  teamHV1.TeamName ,
                                                            FirstTeamNameS = teamHV1.TeamNameS,
                                                            FirstTeamNo =teamHV1.TeamNo ,
                                                            FirstTeamIcon =  teamIconHV1.TeamIcon ,
                                                            ////SecondTeam infors
                                                            SecondTeamID =teamHV2.TeamID,
                                                            SecondTeamNo = teamHV2.TeamNo,
                                                            SecondTeamName = teamHV2.TeamName,
                                                            SecondTeamNameS =teamHV2.TeamNameS,
                                                            SecondTeamIcon = teamIconHV2.TeamIcon,
                                                            HomeTeamID = gameMR.HV.Value == 1 ? secGame.TeamID : gameMR.ID,
                                                            VisitorTeamID = gameMR.HV.Value == 2 ? secGame.TeamID : gameMR.ID
                                                        }).AsEnumerable()
                                               on team.TeamID equals tblSecGame.TeamID.Value  into tblGame
                                               from gameResult in tblGame.DefaultIfEmpty()
                                               where (inGameKind == null || category.GameKindID == inGameKind)                                              
                                               && (category.SeasonID == gameResult.SeasonID)
                                               && (inSeasonID == null || category.SeasonID == inSeasonID)
                                               && (inTeamCD == null || team.TeamID == inTeamCD)
                                               && (inGameID == null || gameResult.TeamID == inGameID)
                                               && (inHomeTeamID == null || gameResult.HomeTeamID == inHomeTeamID)
                                               && (inVisitorTeamID == null || gameResult.VisitorTeamID == inVisitorTeamID)
                                               orderby gameResult.Sec
                                               select new JlgGameInfos
                                               {
                                                   ////GameInfos
                                                   TeamID = gameResult.TeamID,
                                                   ID = gameResult.ID,                                                 
                                                   SeasonID = gameResult.SeasonID,
                                                   SeasonName = gameResult.SeasonName,
                                                   Sec = gameResult.Sec,
                                                   GameID = gameResult.GameID,
                                                   GameDate = gameResult.GameID,
                                                   HV = gameResult.HV,
                                                   GameNameS = gameResult.GameNameS,
                                                   StadiumID = gameResult.StadiumID,
                                                   StadiumNameS = gameResult.StadiumNameS,
                                                   GameResult = gameResult.GameResult,
                                                   GameResultSymbol = gameResult.GameResultSymbol,
                                                   Score = gameResult.Score,
                                                   Lost = gameResult.Lost,
                                                   //// FirstTeam infors
                                                   FirstTeamID = gameResult.FirstTeamID,
                                                   FirstTeamName = gameResult.FirstTeamName,
                                                   FirstTeamNameS = gameResult.FirstTeamNameS,
                                                   FirstTeamNo = gameResult.FirstTeamNo,
                                                   FirstTeamIcon = gameResult.FirstTeamIcon,
                                                   ////SecondTeam infors
                                                   SecondTeamID = gameResult.SecondTeamID,
                                                   SecondTeamNo = gameResult.SecondTeamNo,
                                                   SecondTeamName = gameResult.SecondTeamName,
                                                   SecondTeamNameS = gameResult.SecondTeamNameS,
                                                   SecondTeamIcon = gameResult.SecondTeamIcon,
                                                   HomeTeamID = gameResult.HomeTeamID,
                                                   VisitorTeamID = gameResult.VisitorTeamID
                                               }).OrderBy(x => x.SeasonID)
                                                 .ThenBy(x => x.Sec)
                                                 .ToList();

            return infos;
        }
        #endregion

        #region Get GetTeamInfosResult
        /// <summary>
        /// Join 5 table GameKindTeamGT, CategoryGT, TeamInfoGT ,TeamInfoTE, TeamIconJlg  to get :
        /// Return teamhome infors
        /// Parameter list TeamID, TeamName, TeamNameS  , TeamNo, TeamIcon
        /// </summary>       
        /// <returns>List data </returns>
        public IEnumerable<JlgTeamInfos> GetTeamInfosResult(int? inSeasonID, int? inGameKind, int? inTeamCD)
        {
            IEnumerable<JlgTeamInfos> infos = (from gameKind in jlg.GameKindTeamGT
                                               join category in jlg.CategoryGT on gameKind.GameKindTeamGTId equals category.GameKindTeamGTId
                                               join team in jlg.TeamInfoGT on category.CategoryGTId equals team.CategoryGTId
                                               join teamInfoTE in jlg.TeamInfoTE on team.TeamID equals teamInfoTE.TeamID into teamInfo
                                               from teamTE in teamInfo.DefaultIfEmpty()
                                               join teamIconJlg in jlg.TeamIconJlg on team.TeamID equals teamIconJlg.TeamCD into tmIcon
                                               from teamIcon in tmIcon.DefaultIfEmpty()
                                               where (inGameKind == null || category.GameKindID == inGameKind)
                                               && (inSeasonID == null || category.SeasonID == inSeasonID)
                                                 && (inTeamCD == null || team.TeamID == inTeamCD)
                                               select new JlgTeamInfos
                                                {
                                                    ////Team infors
                                                    TeamID = team.TeamID,
                                                    TeamName = team.TeamName,
                                                    TeamNameS = teamTE.TeamNameS,
                                                    TeamNo = teamTE.TeamNo,
                                                    TeamIcon = teamIcon.TeamIcon
                                                }).Distinct().ToList();

            return infos;
        }
        #endregion
    }
}
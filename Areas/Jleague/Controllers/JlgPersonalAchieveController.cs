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
 * Class		: JlgPersonalAchieveController
 * Developer	: Nguyen Minh Kiet
 * Updated date : 2015-03-24
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.Jleague.Models.ViewModel;

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

    public class JlgPersonalAchieveController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        JlgEntities jlg = new JlgEntities();
        #endregion
        // GET: Jlg/JlgPersonalAchieve
        #region "Action Result"
        public ActionResult Index()
        {            
            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
            ViewBag.JleagueMenu = jType == 1 ? 2 : jType == 2 ? 3 : jType == 3 ? 4 : int.MinValue;
            ViewBag.JleagueSubMenu = 5;
            JlgPersonalAchieveViewModel model = new JlgPersonalAchieveViewModel();
            model.JlgPersonalInfos = GetPersonalAchieveInfos(jType == 1 ? JlgConstants.JLG_GAMEKIND_J1 : jType == 2 ? JlgConstants.JLG_GAMEKIND_J2 : jType == 3?JlgConstants.JLG_GAMEKIND_NABISCO:int.MinValue);  
            ////
            //// Set active menu
            //// ViewBag.JleagueMenu = 2 
            //// If jType == 1 then View Info J1  else view info J2 end;
            ////     
            return View(model);
        }

        #endregion

        #region Get GetPersonalAchieveInfos
        /// <summary>
        /// Join 4 table RankGoalReportRG, RankInfoRG, PlayerInfoDI ,PlayerInfoPS   to get :
        /// 1. RankInfoRG : PlayerID, PlayerNames, TeamID, TeamNameS, Goal, Shoot,  Time,  GoalPK
        /// 2.PlayerInfoDI : Position
        /// 3.PlayerInfoPS : Yellow , Red
        /// 4. GoalShoot = Goal/Shoot
        /// 5. GoalTime = Goal/(Time*90)
        /// </summary>       
        /// <returns>List data </returns>
        public IEnumerable<JlgPersonalAchieveInfos> GetPersonalAchieveInfos(int inGameKind)
        {
            IEnumerable<JlgPersonalAchieveInfos> infos = (from goalReport in jlg.RankGoalReportRG
                                                          join info in jlg.RankInfoRG on goalReport.RankGoalReportRGId equals info.RankGoalReportRGId
                                                          join playerInfoDI in jlg.PlayerInfoDI on info.PlayerID equals playerInfoDI.PlayerID into playerInfo
                                                          from player in playerInfo.DefaultIfEmpty()
                                                          join playerInfoPS in jlg.PlayerInfoPS on info.PlayerID equals playerInfoPS.PlayerID into infoPS
                                                          from playerPS in infoPS.DefaultIfEmpty()
                                                          join playerStatsReportPS in jlg.PlayerStatsReportPS on playerPS.PlayerStatsReportPSId equals playerStatsReportPS.PlayerStatsReportPSId
                                                          where goalReport.GameKindID == inGameKind
                                                          && playerStatsReportPS.GameKindID == inGameKind
                                                          orderby info.Ranking
                                                          select new JlgPersonalAchieveInfos
                                                         {
                                                             Ranking = info.Ranking,
                                                             PlayerID = info.PlayerID,
                                                             PlayerNames = info.PlayerNameS,
                                                             PlayerName = info.PlayerName,
                                                             TeamID = info.TeamID,
                                                             TeamNameS = info.TeamNameS,
                                                             Goal = info.Goal,
                                                             Shoot = info.Shoot,
                                                             Time = info.Time,
                                                             Position = player.Position,
                                                             GoalPK = info.GoalPK,
                                                             Game = info.Game,
                                                             Yellow = playerPS.Yellow,
                                                             Red = playerPS.Red,
                                                             RateGoalShoot = (info.Shoot == null || info.Shoot.Value == 0) ? null : info.Goal / (info.Shoot * 1m),
                                                             RateGoalTime = (info.Time == null || info.Time.Value == 0) ? null : info.Goal / (info.Time * 90m)
                                                         }).Distinct();
            return infos;
        }
        #endregion
    }
}
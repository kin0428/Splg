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
 * Namespace	: Splg.Areas.Jlg.Controllers
 * Class		: JlgTeamInfoScheduleResultController
 * Developer	: Nguyen Ho Long Hai
 * Updated date : 2015-03-31
 */
#endregion

using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Splg.Areas.Jleague.Controllers
{
    public class JlgTeamInfoScheduleResultController : Controller
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

        #region Global Properties
        //Create context to get value from db.         
        JlgEntities jlg = new JlgEntities();
        #endregion

        // GET: Jleague/JlgTeamInfoScheduleResult
        public ActionResult Index(int teamId)
        {
            ViewBag.TeamName = jlg.TeamInfoTE.Where(x => x.TeamID == teamId).Select(x => x.TeamName).FirstOrDefault();

            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
            ViewBag.JleagueMenu = jType == 1 ? 2 : 3;
            ViewBag.JleagueSubMenu = 6;
            ViewBag.JleagueTeamMenu = 2;
            ViewBag.TeamInfoMenuTabActive = (int)JlgConstants.TeamInfoMenu.TabActive_2;

            ViewBag.JType = jType;
            ViewBag.TeamId = teamId;
            int gameKindID = 0;
            switch (jType)
            {
                case 1:
                    gameKindID = 2;
                    break;
                case 2:
                    gameKindID = 6;
                    break;
            }
            int[] Season117 = { 2, 4, 5 };
            int[] Season1834 = { 3, 4, 5 };
            ViewBag.HomeTeamName = jlg.TeamInfoGT.FirstOrDefault(p => p.TeamID == teamId).TeamName;
            var query = (from gameSchedule in jlg.GameSchedule
                         join gameCategory in jlg.GameCategory on gameSchedule.GameScheduleId equals gameCategory.GameScheduleId
                         join scheduleInfo in jlg.ScheduleInfo on gameCategory.GameCategoryId equals scheduleInfo.GameCategoryId
                         where gameSchedule.GameKindID == gameKindID && (scheduleInfo.HomeTeamID.HasValue && scheduleInfo.HomeTeamID.Value == teamId)
                         && (scheduleInfo.OccasionNo <= 17 ? Season117.Contains(gameCategory.SeasonID) : Season1834.Contains(gameCategory.SeasonID))
                         select new JlgTeamInfoScheduleResultViewModel
                         {
                             GameID = scheduleInfo.GameID,
                             GameDate = scheduleInfo.GameDate.HasValue ? scheduleInfo.GameDate.Value : default(short),
                             StadiumName = scheduleInfo.StadiumName,
                             HomeTeamID = scheduleInfo.HomeTeamID.HasValue ? scheduleInfo.HomeTeamID.Value : default(short),
                             HomeIcon = (from teamIconJlg in jlg.TeamIconJlg where teamIconJlg.TeamCD == teamId select teamIconJlg.TeamIcon).FirstOrDefault(),
                             AwayTeamID = scheduleInfo.AwayTeamID.HasValue ? scheduleInfo.AwayTeamID.Value : default(short),
                             AwayTeamName = scheduleInfo.AwayTeamName,
                             OccasionNo = scheduleInfo.OccasionNo.HasValue ? scheduleInfo.OccasionNo.Value : default(short),
                             GroupID = scheduleInfo.GroupID,
                             GameResult = (from resultInfoT5 in jlg.ResultInfoT5
                                           join team5thGameReportT5 in jlg.Team5thGameReportT5 on resultInfoT5.Team5thGameReportT5Id equals team5thGameReportT5.Team5thGameReportT5Id
                                           where team5thGameReportT5.TeamID == teamId && (scheduleInfo.GameDate.HasValue && scheduleInfo.GameDate.Value == resultInfoT5.GameDate)
                                           select resultInfoT5.GameResult).FirstOrDefault(),
                             ScoreLose = (from resultInfoT5 in jlg.ResultInfoT5
                                          join team5thGameReportT5 in jlg.Team5thGameReportT5 on resultInfoT5.Team5thGameReportT5Id equals team5thGameReportT5.Team5thGameReportT5Id
                                          where team5thGameReportT5.TeamID == teamId && (scheduleInfo.GameDate.HasValue && scheduleInfo.GameDate.Value == resultInfoT5.GameDate)
                                          select resultInfoT5.Score.HasValue && resultInfoT5.Lose.HasValue ? resultInfoT5.Score.Value + "-" + resultInfoT5.Lose.Value : string.Empty).FirstOrDefault(),

                         }).OrderByDescending(p => p.GameDate).ToList();
           
            return View(query);
        }
    }
}
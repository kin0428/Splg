#region Author Information
/*
 * Developer	: Tran Sy Huynh
 * 
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
#endregion

namespace Splg.Areas.Jleague.Controllers
{
    public class JlgRightOrderController : Controller
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

        JlgEntities jlg = new JlgEntities();
        // GET: Jleague/JlgRightRecentNews
        public ActionResult ShowJlgRightStanding(int gameType, int jType)
        {
            ViewBag.JType = jType;
            if (jType == 3)
                return PartialView("_JlgRightStanding", GetNabiscoOrder(gameType));
            return PartialView("_JlgRightStanding", GetJOrder(gameType));
        }

        private IEnumerable<JlgJ12OrderViewModel> GetJOrder(int gameType)
        {
            var query = (from rrrt in jlg.RankReportRT
                         join rirt in jlg.RankInfoRT on rrrt.RankReportRTId equals rirt.RankReportRTId
                         join si in jlg.SeasonInfo on rrrt.SeasonID equals si.SeasonID
                         join tite in jlg.TeamInfoTE on rirt.TeamID equals tite.TeamID
                         where (rrrt.GameKindID == gameType)
                         orderby si.SeasonID descending, rirt.Ranking
                         select new JlgJ12OrderViewModel
                         {
                             SeasonID = (int)rrrt.SeasonID,
                             SeasonName = rrrt.SeasonName,
                             TeamID = (int)rirt.TeamID,
                             RankingInfo = rirt,
                             TeamName = tite.TeamName,
                             GroupID = rirt.GroupID
                         } into j_ranking
                         select j_ranking).Take(5);
            return query;
        }
        private IEnumerable<JlgJ12OrderViewModel> GetNabiscoOrder(int gameType)
        {
            var query = from rrrt in jlg.RankReportRT
                        join rirt in jlg.RankInfoRT on rrrt.RankReportRTId equals rirt.RankReportRTId
                        join si in jlg.SeasonInfo on rrrt.SeasonID equals si.SeasonID
                        join tite in jlg.TeamInfoTE on rirt.TeamID equals tite.TeamID
                        where (rrrt.GameKindID == gameType)
                        orderby si.SeasonID descending, rirt.Ranking
                        select new JlgJ12OrderViewModel
                        {
                            SeasonID = (int)rrrt.SeasonID,
                            SeasonName = rrrt.SeasonName,
                            TeamID = (int)rirt.TeamID,
                            RankingInfo = rirt,
                            TeamName = tite.TeamName,
                            GroupID = rirt.GroupID
                        } into nabisco_ranking
                        where (nabisco_ranking.GroupID == "A" || nabisco_ranking.GroupID == "B")
                        select nabisco_ranking;
            return query;
        }
    }
}
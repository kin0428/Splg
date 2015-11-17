#region Author Information
/*
 * Developer    : Tran Sy Huynh
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Areas.Jleague;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Core.Extend;


namespace Splg.Areas.Jleague.Controllers
{
    public class JlgJOrderController : Controller
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
        // GET: Jleague/JlgJ12Order
        public ActionResult Index()
        {
            // Todo:Viewに追加すべきですよね・・・
            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);

            // 日付けの取得
            int dateInput = DateTime.Now.ParseToInt();
            // 直近のシーズンを取得
            ViewBag.CurrentSeasonID = JlgCommon.GetSeasonID(dateInput, jType);

            ViewBag.JleagueMenu = jType == 1 ? 2 : 3;
            ViewBag.JleagueSubMenu = 3;
            ViewBag.JType = jType;
            if (jType == 1)
            {
                return View(GetJOrder(JlgConstants.JLG_GAMEKIND_J1));
            }
            else if (jType == 2)
            {
                return View(GetJOrder(JlgConstants.JLG_GAMEKIND_J2));
            }
            else
            {
                return View(GetJOrder(JlgConstants.JLG_GAMEKIND_NABISCO));
            }
        }


        public ActionResult JlgNabiscoOrder()
        {
            return View(GetNabiscoOrder(JlgConstants.JLG_GAMEKIND_NABISCO));
        }
        private IEnumerable<JlgJ12OrderViewModel> GetJOrder(int gameKind)
        {
            var query = from rrrt in jlg.RankReportRT
                        join rirt in jlg.RankInfoRT on rrrt.RankReportRTId equals rirt.RankReportRTId
                        join si in jlg.SeasonInfo on rrrt.SeasonID equals si.SeasonID
                        join tite in jlg.TeamInfoTE on rirt.TeamID equals tite.TeamID
                        join ti in jlg.TeamIconJlg on tite.TeamID equals ti.TeamCD into tmp_icon
                        from icon in tmp_icon.DefaultIfEmpty()
                        where rrrt.GameKindID == gameKind
                        orderby si.SeasonID, rirt.Ranking
                        select new JlgJ12OrderViewModel
                        {
                            SeasonID = si.SeasonID,
                            TeamID = tite.TeamID,
                            RankingInfo = rirt,
                            TeamName = tite.TeamName,
                            TeamIcon = icon.TeamIcon
                        }
                            into j12_ranking
                            select j12_ranking;

            return query;
        }
        private IEnumerable<JlgJ12OrderViewModel> GetNabiscoOrder(int gameKind)
        {
            var query = (from rrrt in jlg.RankReportRT
                         join rirt in jlg.RankInfoRT on rrrt.RankReportRTId equals rirt.RankReportRTId
                         join si in jlg.SeasonInfo on rrrt.SeasonID equals si.SeasonID
                         join tite in jlg.TeamInfoTE on rirt.TeamID equals tite.TeamID
                         join ti in jlg.TeamIconJlg on tite.TeamID equals ti.TeamCD into tmp_icon
                         from icon in tmp_icon.DefaultIfEmpty()
                         where rrrt.GameKindID == gameKind
                         orderby si.SeasonID, rirt.Ranking
                         select new JlgJ12OrderViewModel
                         {
                             SeasonID = si.SeasonID,
                             TeamID = tite.TeamID,
                             RankingInfo = rirt,
                             TeamName = tite.TeamName,
                             TeamIcon = icon.TeamIcon,
                             GroupID = rirt.GroupID
                         } into nabisco_ranking
                         where (nabisco_ranking.GroupID == "A" || nabisco_ranking.GroupID == "B")
                         select nabisco_ranking);
            return query;
        }

        /// <summary>
        /// get gameid
        /// </summary>
        /// <param name="dateInput"></param>
        /// <param name="gameKindId"></param>
        /// <returns>return array gameId</returns>
        [HttpPost]
        public JsonResult GetCurrentSeason(int jtype)
        {
            int dateInput = DateTime.Now.ParseToInt();
            // 直近のシーズンを取得
            int SeasonID = JlgCommon.GetSeasonID(dateInput, jtype);


            return Json(SeasonID, JsonRequestBehavior.AllowGet);

        }


    }
}
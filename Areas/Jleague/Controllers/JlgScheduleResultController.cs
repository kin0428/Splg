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
 * Namespace	: Splg.Areas.jlg.Controllers
 * Class		: jlgScheduleResultController
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Controllers;
using Splg.Models;
using System.Web.UI;
using Splg.Models.Game.ViewModel;
using Splg.Core.Extend;
using Splg.Services.Game;
using Splg.Core.Constant;

namespace Splg.Areas.Jleague.Controllers
{
    public class jlgScheduleResultController : Controller
    {
        #region Property
        JlgEntities jlgEntities = new JlgEntities();
        ComEntities comEntities = new ComEntities();
        #endregion

        #region Constant
        /// <summary>
        /// Jリーグメニュー用Key@Dictionary
        /// </summary>
        private static readonly string JLeagueMenuKey = "JLeagueMenu";

        /// <summary>
        /// ページ名用Key@Dictionary
        /// </summary>
        private static readonly string PageNameKey = "PageName";

        /// <summary>
        /// ページタイトル用Key@Dictionary
        /// </summary>
        private static readonly string PageTitleKey = "PageTitle";

        /// <summary>
        /// ゲーム種別用Key@Dictionary
        /// </summary>
        private static readonly string GameKindKey = "GameKind";

        /// <summary>
        /// ゲーム種別名用Key@Dictionary
        /// </summary>
        private static readonly string GameKindNameKey = "GameKindName";
        #endregion

        public ActionResult Index()
        {
            var jlgService = new JlgService();

            var jType = jlgService.GetJlgType(Request.Url.AbsoluteUri);

            //Jリーグ初期表示用viewModel
            var jlgScheduleResultViewModel = new JlgScheduleResultViewModel();

            jlgScheduleResultViewModel.JType = jType;

            var gameKindDictionary = GetGameKind(jType);

            var pageSettingsDictionary = GetPageSettings(jType);

            jlgScheduleResultViewModel.JLeagueMenu = Convert.ToInt32(pageSettingsDictionary[JLeagueMenuKey]);

            jlgScheduleResultViewModel.JLeagueSubMenu = 2;

            jlgScheduleResultViewModel.PageName = Convert.ToString(pageSettingsDictionary[PageNameKey]);

            jlgScheduleResultViewModel.PageTitle = Convert.ToString(pageSettingsDictionary[PageTitleKey]);

            jlgScheduleResultViewModel.GameKind = Convert.ToInt32(gameKindDictionary[GameKindKey]);

            jlgScheduleResultViewModel.GameKindName = Convert.ToString(gameKindDictionary[GameKindNameKey]);

            jlgScheduleResultViewModel.OccasionNo = jlgService.GetOccasionNo(DateTime.Now.ParseToInt(), jlgScheduleResultViewModel.GameKind);

            jlgScheduleResultViewModel.MaxOccasionNo = GetMaxOccasionNoByGameKind(jlgScheduleResultViewModel.GameKind);

            jlgScheduleResultViewModel.SeasonId = jlgService.GetSeasonId(DateTime.Now.ParseToInt(), jlgScheduleResultViewModel.GameKind);

            if (jType == JlgConst.JType.Jleaguecup)
            {
                //Todo:決勝ラウンド開放までの一時対応
                jlgScheduleResultViewModel.SeasonId = 4;
                jlgScheduleResultViewModel.OccasionNo = 1;

                //最終ラウンド用
                var queryNb = (from si in jlgEntities.ScheduleInfo
                               join gcat in jlgEntities.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                               join gs in jlgEntities.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                               where gs.GameKindID == JlgConstants.JLG_GAMEKIND_NABISCO
                               where si.GameCategoryId == JlgConstants.JLG_GAMECATEGORY_NABISCO_FINAL
                               select new JlgScheduleResultNabiscoInfoModel
                               {
                                   finalRoundName = si.RoundName,
                                   finalRound = (int)si.Round,
                                   finalOccasion = (int)si.OccasionNo
                               }).ToList();
                List<JlgScheduleResultNabiscoInfoModel> ScheduleInfoNb = queryNb;
                var FinalCnt = GetFinalOcasionCnt();
                jlgScheduleResultViewModel.finalCnt = (int)FinalCnt;
            }

            return View(jlgScheduleResultViewModel);

        }

        public ActionResult ShowScheduleAndResultMenu(JlgScheduleResultViewModel jlgScheduleResultViewModel)
        { 
            var viewName = string.Empty;

            switch (jlgScheduleResultViewModel.JType)
            {
                case JlgConst.JType.J1:
                    return PartialView("_J1ScheduleAndResultMenu", jlgScheduleResultViewModel);
                case JlgConst.JType.J2:
                    return PartialView("_J2ScheduleAndResultMenu", jlgScheduleResultViewModel);
                case JlgConst.JType.Jleaguecup:
                    return PartialView("_NabiscoScheduleAndResultMenu", jlgScheduleResultViewModel);
                default:
                    return null;
            }
        }

        /// <summary>
        /// ゲーム種別取得
        /// </summary>
        private Dictionary<String, object> GetGameKind(JlgConst.JType jType)
        {
            var gameKind = new Dictionary<string, object>();

            switch (jType)
            {
                case JlgConst.JType.J2:
                    gameKind.Add(GameKindKey, JlgConstants.JLG_GAMEKIND_J2);
                    gameKind.Add(GameKindNameKey, JlgConstants.JLG_LEAGUE_NAME_J2);

                    break;

                case JlgConst.JType.Jleaguecup:
                    gameKind.Add(GameKindKey, JlgConstants.JLG_GAMEKIND_NABISCO);
                    gameKind.Add(GameKindNameKey, JlgConstants.JLG_LEAGUE_NAME_NABISCO);

                    break;
                default:
                    //これ、j1をデフォルトにしてる理由ってなんだろう？遠藤さんにインタビューの必要有
                    gameKind.Add(GameKindKey, JlgConstants.JLG_GAMEKIND_J1);
                    gameKind.Add(GameKindNameKey, JlgConstants.JLG_LEAGUE_NAME_J1);

                    break;
            }

            return gameKind;
        }

        /// <summary>
        /// ページ設定取得
        /// </summary>
        private Dictionary<String, object> GetPageSettings(JlgConst.JType jType)
        {
            var pageSettings = new Dictionary<string, object>();

            var pageTitle = "日程・結果";

            switch (jType)
            {
                case JlgConst.JType.J2:
                    pageSettings.Add(JLeagueMenuKey, 3);
                    pageSettings.Add(PageNameKey, Splg.Areas.Jleague.JlgConstants.JLG_J2_SDL);
                    pageSettings.Add(PageTitleKey, pageTitle);

                    break;

                case JlgConst.JType.Jleaguecup:
                    pageSettings.Add(JLeagueMenuKey, 4);
                    pageSettings.Add(PageNameKey, Splg.Areas.Jleague.JlgConstants.JLG_NB_SDL);
                    pageSettings.Add(PageTitleKey, pageTitle);

                    break;
                default:
                    //これ、j1をデフォルトにしてる理由ってなんだろう？遠藤さんにインタビューの必要有
                    pageSettings.Add(JLeagueMenuKey, 2);
                    pageSettings.Add(PageNameKey, Splg.Areas.Jleague.JlgConstants.JLG_J1_SDL);
                    pageSettings.Add(PageTitleKey, pageTitle);

                    break;
            }

            return pageSettings;
        }


        //private int GetSeasonId(int gameKindId)
        //{
        //    var query = (from scheduleInfo in jlgEntities.ScheduleInfo
        //                 join gameCategory in jlgEntities.GameCategory on scheduleInfo.GameCategoryId equals gameCategory.GameCategoryId
        //                 join gs in jlgEntities.GameSchedule on gameCategory.GameScheduleId equals gs.GameScheduleId
        //                 where gs.GameKindID == gameKindId
        //                 orderby scheduleInfo.GameDate
        //                 select gameCategory).FirstOrDefault();

        //    return (query == null) ? 0 : query.SeasonID;
        //}
        
        #region 現在日付以降の節、試合日、Round、1roundあたりの節の数を取得する、1シーズンあたりのRoundの数を取得する:GetOccasionNo、GetGameDate、GetRound、GetOccasionCnt、GetRoundCnt
        /// <summary>
        /// 現在日付以降の節を取得する
        /// </summary>
        /// <param name="dateInput"></param>
        /// <returns></returns>
        [HttpPost]
        public int GetOccasionNo(int dateInput, int gameKindId)
        {
            //dateInput = 20150309;
            var query = (from si in jlgEntities.ScheduleInfo
                         join gcat in jlgEntities.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                         join gs in jlgEntities.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                         where (si.GameDate >= dateInput)
                         where gs.GameKindID == gameKindId
                         orderby si.GameDate
                         select si.OccasionNo).FirstOrDefault();

            if (query != null)
            {
                return (int)query;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 現在日付以降のroundを取得する
        /// </summary>
        /// <param name="dateInput"></param>
        /// <returns></returns>
        [HttpPost]
        public int GetRound(int dateInput, int gameKindId)
        {
            //dateInput = 20150309;
            var query = (from si in jlgEntities.ScheduleInfo
                         join gcat in jlgEntities.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                         join gs in jlgEntities.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                         where (si.GameDate >= dateInput)
                         where gs.GameKindID == gameKindId
                         orderby si.GameDate
                         select si.Round).FirstOrDefault();

            if (query != null)
            {
                return (int)query;
            }
            else
            {
                return 0;
            }
        }

        public int GetMaxOccasionNoByGameKind(int gameKind)
        {
            //節の最大値を取得;
            var query = (from si in jlgEntities.ScheduleInfo
                            join gcat in jlgEntities.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                            join gs in jlgEntities.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                            where gs.GameKindID == gameKind
                            select si.OccasionNo).Max();

            return (int)query;
        }

        public int GetRoundCnt(int gameKind)
        {
            //節の最大値を取得;
            var query = (from si in jlgEntities.ScheduleInfo
                         join gcat in jlgEntities.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                         join gs in jlgEntities.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                         where gs.GameKindID == gameKind
                         select si.Round).Max();

            return (int)query;
        }
        public int GetFinalOcasionCnt()
        {
            //節の最大値を取得;
            var query = (from si in jlgEntities.ScheduleInfo
                         join gcat in jlgEntities.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                         join gs in jlgEntities.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                         where gs.GameKindID == JlgConstants.JLG_GAMEKIND_NABISCO
                         where si.GameCategoryId == JlgConstants.JLG_GAMECATEGORY_NABISCO_FINAL
                         select si.OccasionNo).Max();

            if (query != null)
            {
                return (int)query;

            } else {
                return 0;

            }
        }
        #endregion
        /// <summary>
        /// シーズン最後試合日を取得する
        /// </summary>
        /// <param name="dateInput">基準日</param>
        /// <returns></returns>
        [HttpPost]
        public int GetLastGameDate(int gameKindId)
        {
            // 初期値をセット
            int? query = 0;

            // 引数によって場合分け
            query = (from si in jlgEntities.ScheduleInfo
                        join gcat in jlgEntities.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                        join gs in jlgEntities.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                        where gs.GameKindID == gameKindId
                        orderby si.GameDate descending
                        select si.GameDate).FirstOrDefault();

            if (query != null)
            {
                return (int)query;
            }
            else
            {
                return 0;
            }
        }
    }
}
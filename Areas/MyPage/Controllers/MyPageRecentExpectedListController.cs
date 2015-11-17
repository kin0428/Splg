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
 * Namespace	: Splg.Areas.MyPage.Controllers
 * Class		: MyPageExpectedListController
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using Splg.Areas.Jleague.Controllers;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.Mlb.Controllers;
using Splg.Areas.Mlb.Models;
using Splg.Areas.MyPage;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.Npb.Controllers;
using Splg.Areas.Npb.Models;
using Splg.Models;
using Splg.Models.Game.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Models.Game.InfoModel;
using Splg.Controllers;
using Splg.Services.Point;

#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageRecentExpectedListController : Controller
    {
        private const int CLASSCLASS_TEAM = 2;
        private const int CLASSCLASS_GAME = 4;

        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();

        NpbEntities npb = new NpbEntities();
        JlgEntities jlg = new JlgEntities();
        MlbEntities mlb = new MlbEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //MyPageEntities mypage = new MyPageEntities();

        #endregion

        /// <summary>
        /// GET: /mypage/expectedlist/
        /// </summary>
        public ActionResult Index()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            MyPageRecentExpectedListViewModel viewModel = new MyPageRecentExpectedListViewModel();

            Int64 memberId = GetMemberID();

            #region 年月用変数の設定
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);
            int tYear = today.Year;
            int tMonth = today.Month;
            int tDay = today.Day;
            int yYear = yesterday.Year;
            int yMonth = yesterday.Month;
            int yDay = yesterday.Day;
            #endregion

            viewModel.Today = today;
            viewModel.Yesterday = yesterday;

            viewModel.GameInfoTdy = MyPageCommon.GetGameInfo(memberId, tYear, tMonth, tDay);
            viewModel.GameInfoYdy = MyPageCommon.GetGameInfo(memberId, yYear, yMonth, yDay);
            return View(viewModel);
        }

        #region 予想リスト表示(SP)
        public ActionResult ShowExpectedList(int target_year, int target_month, int target_date)
        {
            IEnumerable<GameInfoModel> expectedList;
            Int64 memberId = GetMemberID();

            expectedList = MyPageCommon.GetGameInfo(memberId, target_year, target_month, target_date);


            return PartialView("_MyPageExpectedListInfo", expectedList);

        }
        #endregion
        #region 予想リスト表示(PC)
        public ActionResult ShowExpectedListForPC(int target_year, int target_month, int target_date)
        {
            IEnumerable<GameInfoModel> expectedList;
            Int64 memberId = GetMemberID();

            expectedList = MyPageCommon.GetGameInfo(memberId, target_year, target_month, target_date);


            return PartialView("_MyPageRecentExpectedListInfo", expectedList);

        }
        #endregion

        /// <summary>
        /// 予想をやめる
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult ExpectCancel(MyExpectedListInfoModel.ExpectChangeModel ViewModel)
        {
            Int64 memberID = GetMemberID();

            bool isResult = MyPageCommon.IsExpectCancel(ViewModel, memberID);

            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 予想ポイント入力
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult ExpectPoint(MyExpectedListInfoModel.ExpectChangeModel ViewModel)
        {
            Int64 memberID = GetMemberID();
            MyPageJsonResultModel result = new MyPageJsonResultModel();

            result = MyPageCommon.UpdateExpectPoint(ViewModel, memberID);

            return Json(result, JsonRequestBehavior.AllowGet);

        }
      
        #region memberIDの取得
        private Int64 GetMemberID()
        {
            Int64 memberId = -1;
            object currentUser = Session["CurrentUser"];

            if (currentUser != null)
                memberId = Convert.ToInt64(currentUser.ToString());

            //debug
            //memberId = 2;

            return memberId;
        }
        #endregion
    }
}
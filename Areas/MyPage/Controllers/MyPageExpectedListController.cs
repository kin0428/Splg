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
using Splg.Areas.Jleague.Models;
using Splg.Areas.Mlb.Models;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.Npb.Models;
using Splg.Models;
using Splg.Models.Game.InfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageExpectedListController : Controller
    {

        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        static ComEntities com = new ComEntities();

        static NpbEntities npb = new NpbEntities();
        static JlgEntities jlg = new JlgEntities();
        static MlbEntities mlb = new MlbEntities();
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

            MyPageExpectedListViewModel viewModel = new MyPageExpectedListViewModel();

            Int64 memberId = GetMemberID();

            #region 年月用変数の設定
            DateTime dtToday = DateTime.Today;
            viewModel.LastYear = dtToday.AddYears(-1).Year.ToString(); // 昨年
            viewModel.ThisYear = dtToday.Year.ToString(); // 今年
            viewModel.NextYear = dtToday.AddYears(1).Year.ToString(); // 来年
            viewModel.ThisMonth = dtToday.Month.ToString(); // 今月
            int iYear = dtToday.Year;
            int iMonth = dtToday.Month;
            int i;
            for (i = 1; i <= 3; i++)
            {
                if (i == 2) // 必ず真中が対象年になる
                {
                    viewModel.YearListClass[i - 1] = "class=active"; //　アクティブ月のとき'class="active"'、そうでないとき''
                }
                else
                {
                    viewModel.YearListClass[i - 1] = ""; //　アクティブ月のとき'class="active"'、そうでないとき''
                }
            }
            for (i = 1; i <= 12; i++)
            {
                if (i == iMonth)
                {
                    viewModel.MonthListClass[i - 1] = "class=active"; //　アクティブ月のとき'class="active"'、そうでないとき''
                }
                else
                {
                    viewModel.MonthListClass[i - 1] = ""; //　アクティブ月のとき'class="active"'、そうでないとき''
                }
            }
            #endregion

            viewModel.GameInfo = MyPageCommon.GetGameInfo(memberId, iYear, iMonth);

            return View(viewModel);
        }

        /// <summary>
        /// 予想リスト表示
        /// </summary>
        /// <param name="target_year"></param>
        /// <param name="target_month"></param>
        /// <param name="target_date"></param>
        /// <returns></returns>
        public ActionResult ShowExpectedList(int target_year, int target_month, int target_date = 0)
        {
            IEnumerable<GameInfoModel> expectedList;
            Int64 memberId = GetMemberID();

            expectedList = MyPageCommon.GetGameInfo(memberId, target_year, target_month, target_date);


            return PartialView("_MyPageExpectedListInfo", expectedList);

        }

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
        public Int64 GetMemberID()
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
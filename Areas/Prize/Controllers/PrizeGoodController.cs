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
 * Namespace	: Splg.Areas.Prize.Controllers
 * Class		: PrizeTopController
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using System;
using System.Web.Mvc;
using Splg.Models;
using Splg.Areas.Prize.Models;
using Splg.Areas.Prize.Models.ViewModel;
using Splg.Services.Prize;
using Splg.Services.Point;
using Splg.Models.PointInfo.InfoModel;
using Splg.Core.Services;
#endregion



namespace Splg.Areas.Prize.Controllers
{
    public class PrizeGoodController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        PrizeEntities prize = new PrizeEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //PrizeEntities prize = new PrizeEntities();
        #endregion


        /// <summary>
        ///  GET: /prize/{YYYYMM}/
        /// </summary>
        /// <param name="targetYearMonth"></param>
        /// <param name="rallyGoodsId"></param>
        /// <returns></returns>
        public ActionResult Index(string targetYearMonth, long rallyGoodsId)
        {
            var prizeInfoService = new RallyService(prize,com);
            var pointInfoService = new PointInfoService(com);
            long memberId;
            
            PrizeGoodViewModel prizeGoods = new PrizeGoodViewModel();

            prizeGoods.RallyGoods = prizeInfoService.GetRallyGoodsViewModelByRallyGoodsId(rallyGoodsId);
            prizeGoods.RallyGoodsRemarks = prizeInfoService.GetRallyGoodsRemarksViewModel(rallyGoodsId);
            prizeGoods.RallyGoodsRemarksText = prizeInfoService.GetRallyGoodsRemarksTextViewModel(rallyGoodsId);
            prizeGoods.RallyGoodsRemarksLink = prizeInfoService.GetRallyGoodsRemarksLinkViewModel(rallyGoodsId);
            // ログインチェックとメンバーID取得
            prizeGoods.IsLogined = UserService.IsLogined(Session);
            if (prizeGoods.IsLogined)
            {
                memberId = UserService.GetMemberIdAtLong(Session);
                prizeGoods.AvailablePoint = pointInfoService.GetAvailablePointByMemberId(memberId);
                prizeGoods.EntryCount = prizeInfoService.GetEntryCount(memberId, rallyGoodsId);
            }

            return View(prizeGoods);
        }

        /// <summary>
        /// 現在の所持ポイントの取得処理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPossessionPoints()
        {
            PointInfoModel result = new PointInfoModel();

            int memberId = 0;
            object currentUser = Session["CurrentUser"];
            if (currentUser != null)
                memberId = Convert.ToInt32(currentUser.ToString());
            var pointInfoService = new PointInfoService(com);
            int points = pointInfoService.GetOnlinePoint(memberId);

            result.PossesionPoint = points;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 応募のエラーチェック(エラーメッセージを返す）
        /// </summary>
        [HttpPost]
        public JsonResult IsEntry(int rallyGoodId,string entryCount)
        {
            long memberId = UserService.GetMemberIdAtLong(Session);

            var rallyService = new RallyService(prize, com);

            string result = rallyService.IsEntryPrizeCompetition(memberId, rallyGoodId, entryCount);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 所持ポイントを使用して景品を申し込む処理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EntryPrize(int rallyGoodId, int entryCount,short entryMethod)
        {
            long memberId = UserService.GetMemberIdAtLong(Session);

            var prizeInfoService = new RallyService(prize, com);

            bool result = prizeInfoService.EntryPrizeCompetition(memberId, rallyGoodId, entryCount,entryMethod);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>

	}
}
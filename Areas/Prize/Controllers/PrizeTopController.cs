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

using Splg.Areas.Prize.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Controllers;
using Splg.Models;
using System.Web.UI;
using Splg.Models.Game.ViewModel;
using Splg.Areas.Prize.Models.ViewModel;
using Splg.Services.Prize;
using Splg.Services.Point;
using Splg.Core.Services;


namespace Splg.Areas.Prize.Controllers
{
    public class PrizeTopController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        private ComEntities ComEntities = new ComEntities();

        #endregion

        /// <summary>
        /// GET: /Prize/PrizeTop/ 
        /// </summary>
        public ActionResult Index()
        {
            var prizeTopViewModel = new PrizeTopViewModel();

            if (UserService.IsLogined(Session))
            {
                var pointInfoService = new PointInfoService(ComEntities);

                prizeTopViewModel.AvailablePoint = pointInfoService.GetAvailablePointByMemberId(UserService.GetMemberIdAtLong(Session));
            }

            var prizeEntities = new PrizeEntities();

            var rallyService = new RallyService(prizeEntities);

            prizeTopViewModel.RallyViewModel = rallyService.GetRallyViewModelByToday(DateTime.Now);

            if (prizeTopViewModel.RallyViewModel == null)
            {
                //大会情報期間外パターン
                return View("RallyNotFound");
            }
            else
            {
                prizeTopViewModel.RallyGoodsModel = rallyService.GetRallyGoodsViewModelsByRallyId(prizeTopViewModel.RallyViewModel.RallyId);
                
                //過去大会履歴リスト（一旦非表示らしいので、コメントアウト。処理は作成済）
                //prizeTopViewModel.RallyHistories = rallyService.GetRallyViewModelAtPrevious(prizeTopViewModel.RallyViewModel.RallyId);

                return View(prizeTopViewModel);
            }
        }
	}
}
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
 * Class		: MyPageTopController
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using System;
using System.Web.Mvc;
using Splg.Models;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.MyPage.Service;
using Splg.Services.System;

#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageFollowingController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();

        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //MyPageEntities mypage = new MyPageEntities();

        // todo: interfaceへ変更
        private MyPageFollowingService  workerService;

        private SystemDatetimeService systemDatetimeService;

        #endregion

        public MyPageFollowingController()
        {
            // todo インスタンス管理
            this.workerService = new MyPageFollowingService(this.com);
            this.systemDatetimeService = new SystemDatetimeService();
        }

        /// <summary>
        /// ログインユーザのMemberIDを取得
        /// </summary>
        /// <returns></returns>
        private long GetLoginMemberId()
        {
            // HACK: 共通化されるまでの仮メソッドです

            long memberId = 0;

            object currentUser = Session["CurrentUser"];
            if (currentUser != null)
            {
                memberId = Convert.ToInt64(currentUser.ToString());
            }

            return memberId;
        }

        /// <summary>
        /// GET: /mypage/following/
        /// </summary>
        public ActionResult Index()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定(フォーム認証部分も本来なら見るべき。)
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            long memberId = this.GetLoginMemberId();

            var viewModel = this.workerService.GetViewModel(memberId,
                                                          0,
                                                          MyPageFollowersViewModel.INITIAL_SIZE,
                                                          this.systemDatetimeService.TargetYear,
                                                          this.systemDatetimeService.TargetMonth);

            return View(viewModel);
        }

        /// <summary>
        /// もっと見る取得
        /// </summary>
        /// <param name="currentCount">現在表示しているレコード件数</param>
        /// <returns>Json形式のActionResult</returns>
        public ActionResult GetMoreFollowings(int currentCount)
        {
            long memberId = this.GetLoginMemberId();

            var viewModel = this.workerService.GetViewModel(memberId,
                                                         currentCount,
                                                         MyPageFollowingViewModel.INITIAL_PAGE_SIZE,
                                                         this.systemDatetimeService.TargetYear,
                                                         this.systemDatetimeService.TargetMonth);
            
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 対象メンバーをフォローする
        /// </summary>
        /// <param name="followingMemberId"></param>
        [HttpPost]
        public ActionResult Follow(long followingMemberId)
        {
            string result = null;

            try
            {
                long memberId = this.GetLoginMemberId();
                Utils.follow(memberId, followingMemberId);

                result = "success";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// フォロー対象からフォローを外す
        /// </summary>
        /// <param name="followingMemberId"></param>
        [HttpPost]
        public ActionResult UnFollow(long followingMemberId)
        {
            string result = null;

            try
            {
                long memberId = this.GetLoginMemberId();

                Utils.unfollow(memberId, followingMemberId);

                result = "success";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

	}
}
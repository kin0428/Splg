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
 * Namespace	: Splg.Areas.User.Controllers
 * Class		: UserTopController
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using System;
using System.Web.Mvc;
using Splg.Models;
using Splg.Areas.User.Models.ViewModel;
using Splg.Areas.User.Service;
using Splg.Services.System;

#endregion

namespace Splg.Areas.User.Controllers
{
    public class UserFollowingController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //UserEntities User = new UserEntities();

        // todo: interfaceへ変更
        private UserFollowingService workerService;

        private SystemDatetimeService systemDatetimeService;

        #endregion

        public UserFollowingController()
        {
            // todo インスタンス管理
            this.workerService = new UserFollowingService(this.com);
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

        //
        // GET: /User/UserTop/
        public ActionResult Index(long memberId)
        {
            Member member = Utils.GetMember(memberId);

            ViewBag.OtherMemberID = memberId;
            ViewBag.OtherMemberNickName = member.Nickname;

            var viewModel = this.workerService.GetViewModel(memberId,
                                                       this.GetLoginMemberId(),
                                                       0,
                                                       UserFollowersViewModel.INITIAL_SIZE,
                                                       this.systemDatetimeService.TargetYear,
                                                       this.systemDatetimeService.TargetMonth);

            viewModel.MemberId = memberId;
            viewModel.Nickname = member.Nickname;

            return View(viewModel);
        }

        /// <summary>
        /// もっと見る取得
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="currentCount">現在表示しているレコード件数</param>
        /// <returns>Json形式のActionResult</returns>
        public ActionResult GetMoreFollowings(long memberId, int currentCount)
        {
            var viewModel = this.workerService.GetViewModel(memberId,
                                                this.GetLoginMemberId(),
                                                currentCount,
                                                UserFollowingViewModel.INITIAL_PAGE_SIZE,
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
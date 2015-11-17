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
using Splg.Areas.MyPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Controllers;
using Splg.Models;
using System.Web.UI;
using Splg.Models.Game.ViewModel;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Models.ViewModel;
#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageSettingDeleteAccountController : Controller
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
        #endregion

        /// <summary>
        /// GET: /mypage/setting/deleteaccount/
        /// </summary>
        public ActionResult Index()
        {
            ////Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            var viewModel = new MyPageSettingDeleteAccountViewModel();

            var url = Request.Path;
            string[] subUrls = url.Split('/');

            if (subUrls != null && subUrls.Length > 4 && subUrls[4] == "execute")
            {
               return Execute();
            }

            Int64 memberId = GetMemberID();
            if (memberId > 0)
            {
                    var member = (from m in com.Member
                                    where m.MemberId == memberId
                                    select m).FirstOrDefault();
                    if (member != null)
                    {
                        viewModel.Nickname = member.Nickname;
                    }
            }

            return View(viewModel);
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


        #region アカウントの削除
        /// <summary>
        /// アカウントの削除
        /// </summary>
        /// <returns>   none
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Execute()
        {
            var viewModel = new MyPageSettingDeleteAccountViewModel();

            try
            {
                Int64 memberID = GetMemberID();
                var member = (from m in com.Member
                              where m.MemberId == memberID
                              select m).FirstOrDefault();

                if (member != null)
                {
                    member.Status = Constants.MEMBER_STATUS_LEFT;
                    member.ExitTime = DateTime.Now;
                    member.ModifiedAccountID = memberID.ToString();
                    member.ModifiedDate = member.ExitTime;

                    int rs = com.SaveChanges();

                    if (rs > 0)
                    {

                        Session["CurrentUser"] = null;
                        Session["UserInfo"] = null;

                        HttpCookie cookie = HttpContext.Request.Cookies.Get("auth_cookie");
                        if (HttpContext.Request.Cookies["auth_cookie"] != null)
                        {
                            cookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(cookie);
                        }

                        return View("done");
                    }
                }

                viewModel.HasError = true;
                viewModel.Message = "会員情報が見つかりませんでした。";

                return View(viewModel);

            }
            catch (Exception ex)
            {
                viewModel.HasError = true;
                viewModel.Message = "予期せぬエラーが起こりました。";

                return View(viewModel);
            }
        }
        #endregion


    }

}
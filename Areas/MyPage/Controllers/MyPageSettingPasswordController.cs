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
    public class MyPageSettingPasswordController : Controller
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
        /// GET: /mypage/setting/password/
        /// </summary>
        public ActionResult Index()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定(フォーム認証部分も本来なら見るべき。)
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            var viewModel = new MyPageSettingPasswordViewModel();
            viewModel.SettingAddress = new MyPageSettingPasswordViewModel.SettingPasswordModel();
            viewModel.SettingAddress.MemberRegisterInfo = new MemberRegistViewModel();

            Int64 memberId = GetMemberID();
            if (memberId > 0)
            {
                if (Session["UserInfo"] != null)
                {
                    var userInfo = Session["UserInfo"] as MemberRegistViewModel;
                    viewModel.SettingAddress.MemberRegisterInfo.Email = userInfo.Email;
                    viewModel.SettingAddress.MemberRegisterInfo.Password = userInfo.Password;
                    viewModel.SettingAddress.MemberRegisterInfo.IsSNS = userInfo.IsSNS;
                    viewModel.SettingAddress.ErrorMessage = "";
                }
                else
                {
                    var member = (from m in com.Member
                                  where m.MemberId == memberId
                                  select m).FirstOrDefault();
                    if (member != null)
                    {
                        viewModel.SettingAddress.MemberRegisterInfo.Email = member.Mail;
                        viewModel.SettingAddress.MemberRegisterInfo.Password = member.Password;
                        // viewModel.MemberRegisterInfo.IsSNS = ???
                        viewModel.SettingAddress.ErrorMessage = "";
                    }
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


        #region 新しいパスワード変更
        /// <summary>
        /// 新しいパスワード変更
        /// </summary>
        /// <returns>   none
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(string expass, string npass, string npass_confirm)
        //        [ValidateAntiForgeryToken]
        //        public JsonResult sendToNewAddress(MyPageSettingPasswordViewModel.SettingAddressModel ViewModel)
        {
            MyPageJsonResultModel result = new MyPageJsonResultModel();

            try
            {
                if (npass == string.Empty)
                {
                    result.HasError = true;
                    result.Message = "新しいパスワードを入力してください。";
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                if (npass.Length < 6)
                {
                    result.HasError = true;
                    result.Message = "パスワードは6文字以上32文字以下の半角英数字を入力下さい。";
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                if (npass_confirm == string.Empty || npass_confirm != npass)
                {
                    result.HasError = true;
                    result.Message = "入力されたパスワードが違います。";
                    return Json(result, JsonRequestBehavior.AllowGet);

                }

                var userInfo = Session["UserInfo"] as MemberRegistViewModel;

                if (userInfo == null)
                {
                    //Session["UserInfo"]が存在しないことが多い。要確認
                    //result.HasError = true;
                    //result.Message = "セッション情報が失われました。再度ログインしてください。";
                    //return Json(result, JsonRequestBehavior.AllowGet);

                }else if (userInfo.IsSNS)
                {

                    //あり得ないケース？
                    result.HasError = true;
                    result.Message = "SNSログインが設定されています。";
                    return Json(result, JsonRequestBehavior.AllowGet);

                }

                if (expass == string.Empty)
                {
                    result.HasError = true;
                    result.Message = "現在のパスワードを入力してください。";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                Int64 memberID = GetMemberID();
                var member = (from m in com.Member
                              where m.MemberId == memberID
                              select m).FirstOrDefault();

                if (member != null)
                {
                    // 現在と同じ
                    string compareString = Utils.MD5Hash(expass);
                    if (compareString != member.Password)
                    {
                        result.HasError = true;
                        result.Message = "パスワードが違います。";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    result.HasError = true;
                    result.Message = "不正なアクセスが検出されました。";
                    return Json(result, JsonRequestBehavior.AllowGet);

                }

                //パスワードの再設定
                if (member != null)
                {
                    member.Password = Utils.MD5Hash(npass);
                    int rs = com.SaveChanges();
                    if (rs > 0)
                    {
                        result.HasError = false;
                        result.Message = "新しいパスワードが設定されました。";

                        //Session["CurrentUser"] = null;
                        //HttpCookie cookie = HttpContext.Request.Cookies.Get("auth_cookie");
                        //if (HttpContext.Request.Cookies["auth_cookie"] != null)
                        //{
                        //    cookie.Expires = DateTime.Now.AddDays(-1);
                        //    Response.Cookies.Add(cookie);
                        //}
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // 現在と同じ
                result.HasError = true;
                result.Message = "システムにエラーが起きました。";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion



    }

}
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
    public class MyPageSettingAddressController : Controller
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
        /// GET: /mypage/setting/address/
        /// </summary>
        public ActionResult Index()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定(フォーム認証部分も本来なら見るべき。)
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            var viewModel = new MyPageSettingAddressViewModel();
            viewModel.SettingAddress = new MyPageSettingAddressViewModel.SettingAddressModel();
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

        //入力されたメールアドレスが現在のものと同じ場合、エラーメッセージno1を表示する
        //MemberテーブルのMailフィールドを検索し、すでに使用されている場合エラーメッセージno2を表示する
        //SNSログインでない場合、入力されたパスワードが正しいものかチェックを行い、正しくない場合エラーメッセージno3を表示する
        //上記に該当しない場合、定められた方法でトークンを発行し、
        //入力されたメールアドレス宛にトークンを含むURLを記載したメールを送信する。メール送信後画面を表示する
        //トークン情報を保持するテーブル：TokenControl,TokenAcInfo
        //トークンの有効期限：発行から24時間
        //トークン種別：メールアドレス変更
        //TODO:トークン種別の定義がDB設計書に無いため確認
        //TODO:仮登録されたメールアドレスの保存場所を確認

        #region メールアドレス変更
        /// <summary>
        /// メールアドレス変更
        /// </summary>
        /// <returns>   none
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangeAddress(string email, string password)
        //        [ValidateAntiForgeryToken]
        //        public JsonResult sendToNewAddress(MyPageSettingAddressViewModel.SettingAddressModel ViewModel)
        {
            Int64 memberID = GetMemberID();
            string entry_email = email; // ViewModel.MemberRegisterInfo.Email;
            string entry_password = password;// ViewModel.MemberRegisterInfo.Password;
            MyPageJsonResultModel result = new MyPageJsonResultModel();

            try
            {
                if (String.IsNullOrEmpty(entry_email))
                {
                    result.HasError = true;
                    result.Message = "メールアドレスを入力してください。";
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                if (String.IsNullOrEmpty(entry_password))
                {
                    result.HasError = true;
                    result.Message = "パスワードを入力してください。";
                    return Json(result, JsonRequestBehavior.AllowGet);

                }

                if (memberID > 0 && entry_email != null)
                {

                    var member = (from m in com.Member
                                  where m.MemberId == memberID
                                  select m).FirstOrDefault();
                    if (member != null)
                    {
                        if (member.Mail == entry_email)
                        {
                            result.HasError = true;
                            result.Message = "既に登録されているメールアドレスです。";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }

                    }

                    var member2 = (from m in com.Member
                                   where m.Mail == entry_email
                                   select m).FirstOrDefault();

                    if (member2 != null)
                    {
                        result.HasError = true;
                        result.Message = "既に登録されているメールアドレスです。";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (entry_password != null)
                    {
                        string compareString = Utils.MD5Hash(entry_password);
                        if (compareString != member.Password)
                        {
                            result.HasError = true;
                            result.Message = "パスワードが間違っています。";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }


                    //////////////////////////
                    string tokenID = "";
                    bool transactionResult = false;
                    string displayID = member.DisplayMemberId;
                    // If user is registered via SNS using this transaction
                    using (var userTransaction = com.Database.BeginTransaction())
                    {
                        try
                        {
                            var tokenAct = new TokenAcInfo
                            {
                                TokenID = Guid.NewGuid().ToString("N"),
                                TokenValue = entry_email      //メールアドレスを格納
                            };
                            tokenID = tokenAct.TokenID;
                            com.TokenAcInfo.Add(tokenAct);
                            com.SaveChanges();
                            var tokenControl = new TokenControl
                            {
                                TokenID = tokenAct.TokenID,
                                TokenClass = 3,                 // １・・会員本登録、２・・パスワード変更、３・・メールアドレス変更
                                Status = true,
                                MemberID = memberID,
                                IssueDate = System.DateTime.Now,
                                ExpireDate = System.DateTime.Now.AddDays(1)
                            };
                            com.TokenControl.Add(tokenControl);
                            com.SaveChanges();
                            userTransaction.Commit();

                            EmailSender emailUtil = new EmailSender();
                            // Set Email Subject and Email Template
                            string messageSubject = "【スポログ】メールアドレス変更確認メール";
                            string messageTemplate = "AddressChangeConfirm.html";
                            string tokenValue = com.TokenAcInfo.SingleOrDefault(t => t.TokenID.Equals(tokenID)).TokenValue;
                            string url = Url.RouteUrl("change_mail", new { uid = displayID, token = tokenID }, Request.Url.Scheme);
                            string routeURL = Url.RouteUrl("change_mail", new { uid = displayID, token = tokenID });
                            Dictionary<string, string> dicContent = new Dictionary<string, string>();
                            dicContent.Add("[URL]", url);
                            dicContent.Add("[routevalue]", routeURL);
                            dicContent.Add("[ExpirationTime]", DateTime.Now.AddHours(24).ToString());

                            // Send to receiver for account activation                    
                            if (emailUtil.SendEmail(entry_email, messageTemplate, messageSubject, dicContent).Equals("Success"))
                            {
                            }
                            else
                            {
                                result.HasError = true;
                                result.Message = "メール送信に失敗しました。";
                                return Json(result, JsonRequestBehavior.AllowGet);
                                result.Message = "メールが送れませんでした。";
                            }

                        }
                        catch
                        {
                            userTransaction.Rollback();
                            result.HasError = true;
                            result.Message = "システムエラー。";
                            return Json(result, JsonRequestBehavior.AllowGet);
                            result.Message = "メールが送れませんでした。";
                        }
                    }
                    //////////////////////
                }

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // 現在と同じ
                result.HasError = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Generate display ID for member
        /// <summary>
        /// Generate ID with 8 last characters from Guid
        /// </summary>
        /// <returns>Display ID with 8 characters</returns>
        private string GenerateDisplayID()
        {
            string displayID = Guid.NewGuid().ToString();
            //Get last 8 characters from generated Guid
            displayID = displayID.Length > 8 ? displayID.Substring(displayID.Length - 8, 8) : displayID;
            var memberObj = com.Member.SingleOrDefault(m => m.DisplayMemberId.Equals(displayID));
            // Check if generated ID is existed in database or not
            while (memberObj != null)
            {
                // Regenerate if ID is existed
                displayID = Guid.NewGuid().ToString();
                displayID = displayID.Length > 8 ? displayID.Substring(displayID.Length - 8, 8) : displayID;
                memberObj = com.Member.SingleOrDefault(m => m.DisplayMemberId.Equals(displayID));
            }
            return displayID;
        }
        #endregion

    }

}
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
 * Namespace	: Splg.Controllers
 * Class		: MemberController
 * Developer	: Nguyen Ho Long Hai
 * 
 */
#endregion

using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Splg.Models;
using Splg.Models.News;
using Splg.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TweetSharp;
using System.Data.SqlClient;
using System.Web.Configuration;
using Facebook;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Drive.v2;
using System.Data.Entity.Validation;
using System.IO.IsolatedStorage;
using System.Globalization;

namespace Splg.Controllers
{

    public class MemberController : Controller
    {
        #region Global Properties
        //Create context to get value from db.  
        ComEntities com = new ComEntities();
        private string fbAccessKey = string.Empty;
        private string fbSecretKey = string.Empty;
        private string twitterAccessKey = string.Empty;
        private string twitterSecretKey = string.Empty;
        #endregion

        public MemberController()
        {
            fbAccessKey = ComCommon.GetAccessKey(Constants.FACEBOOK);
            fbSecretKey = ComCommon.GetSecretKey(Constants.FACEBOOK);
            twitterAccessKey = ComCommon.GetAccessKey(Constants.TWITTER);
            twitterSecretKey = ComCommon.GetSecretKey(Constants.TWITTER);
        }

        #region FactoryMethod

        /// <summary>
        /// check email exits
        /// </summary>
        /// <param name="email"></param>
        /// <returns>json</returns>
        public JsonResult CheckEmailAvailability(string email)
        {
            bool data = ComCommon.CheckEmailAvailability(email);
            if (data)
            {
                return Json(Resources.Japanese.errorEmailExists, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  check email exit in ForgotPass
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult CheckEmailForgotPass(string email)
        {
            bool data = ComCommon.CheckEmailForgotPass(email);
            if (data)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Resources.Japanese.errorEmailWithoutExist, JsonRequestBehavior.AllowGet);
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime date)
        {
            GlobalizationSection globalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
            var cultureInfo = new System.Globalization.CultureInfo(globalizationSection.Culture.ToString());
            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            while (date.DayOfWeek != firstDayOfWeek)
            {
                date = date.AddDays(-1);
            }
            return date;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        private bool InsertNewLoginHistory(long memberID)
        {
            bool result = false;
            LoginHistory loginHistory = new LoginHistory();
            loginHistory.MemberID = memberID;
            loginHistory.LoginDate = System.DateTime.Now;
            com.LoginHistory.Add(loginHistory);
            if (com.SaveChanges() > 0)
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Register
        /// <summary>
        /// Load form register member
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Session["CurrentUser"] != null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// when form register submit check validation 
        /// </summary>
        /// <param name="model"></param>
        /// <returns> RedirectToAction RegisterReview </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(MemberRegistViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (Session["UserInfo"] != null)
                {
                    var obj = Session["UserInfo"] as MemberRegistViewModel;
                    if (string.IsNullOrEmpty(obj.Email))
                    {
                        obj.Email = model.Email;
                    }
                    obj.Password = model.Password;
                    obj.ConfirmPassword = model.ConfirmPassword;
                    Session["UserInfo"] = obj;
                }
                else
                {
                    Session["UserInfo"] = model;
                }
                await Task.Delay(50);
                return RedirectToAction("RegisterReview", new { typeID = 0 });
            }
            return View(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult RegisterReview(short typeId)
        {
            if (Session["CurrentUser"] != null)
            {
                return RedirectToActionPermanent("Index", "Home");

            }
            else
            {
                ViewBag.pageNO = Constants.REGISTER_REVIEW;
                MemberRegistViewModel model = new MemberRegistViewModel();
                if (Session["UserInfo"] != null)
                {
                    model = Session["UserInfo"] as MemberRegistViewModel;
                    model.Email = model.Email;
                    model.Password = model.Password;
                    model.ReceiveEmail = model.ReceiveEmail;
                    model.TypeID = typeId;
                }
                return View(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <Author></Author>
        /// <param name="model"></param>
        /// <returns></returns>  
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterReview(MemberRegistViewModel model)
        {
            if (Session["UserInfo"] != null)
            {
                model = Session["UserInfo"] as MemberRegistViewModel;
                using (var transaction = com.Database.BeginTransaction())
                {
                    try
                    {
                        //insert data to table Member
                        Member member = new Member();
                        member.DisplayMemberId = GenerateDisplayID();
                        member.Nickname = !string.IsNullOrEmpty(model.NickName) ? model.NickName : string.Format("spolog-{0}", member.DisplayMemberId);
                        member.ProfileImg = !string.IsNullOrEmpty(model.ImageLink) ? model.ImageLink : Constants.IMG_DEFAULT_PROFILE;
                        if (model.Gender.HasValue)
                        {
                            member.Gender = (short)model.Gender;
                        }
                        else if (model.Birthday.HasValue)
                        {
                            member.Birthday = model.Birthday.Value;
                            member.BirthdayYear = (short)model.Birthday.Value.Year;
                            member.BirthdayMonth = (short)model.Birthday.Value.Month;
                            member.BirthdayDay = (short)model.Birthday.Value.Day;
                        }
                        member.Mail = model.Email;
                        member.Password = Utils.MD5Hash(model.Password);
                        member.Status = model.TypeID;
                        member.LoginKeepFlg = model.RememberMe;
                        member.InterimRegistTime = DateTime.Now;
                        com.Member.Add(member);
                        com.SaveChanges();

                        //insert data to table TokenAcInfo
                        TokenAcInfo tokenAcInfo = new TokenAcInfo();
                        tokenAcInfo.TokenID = Guid.NewGuid().ToString("N");
                        tokenAcInfo.TokenValue = Utils.MD5Hash(Guid.NewGuid().ToString());
                        com.TokenAcInfo.Add(tokenAcInfo);
                        com.SaveChanges();

                        //insert data to table TokenControl
                        TokenControl tokenControl = new TokenControl();
                        tokenControl.TokenID = tokenAcInfo.TokenID;
                        tokenControl.TokenClass = 1;
                        tokenControl.Status = true;
                        tokenControl.MemberID = member.MemberId;
                        tokenControl.IssueDate = DateTime.Now;
                        tokenControl.ExpireDate = DateTime.Now.AddDays(1);
                        com.TokenControl.Add(tokenControl);
                        com.SaveChanges();
                        if (model.IsSNS)
                        {
                            //insert data to table SnsAuthInformation
                            SnsAuthInformation snsAuthInformation = new SnsAuthInformation();
                            snsAuthInformation.MemberID = member.MemberId;
                            snsAuthInformation.SnsMemberID = model.SNSID;
                            switch (model.SNSName)
                            {
                                case Constants.FACEBOOK:
                                    snsAuthInformation.SnsID = com.SnsCorpInformation.SingleOrDefault(p => p.SnsName == Constants.FACEBOOK).SnsID;
                                    snsAuthInformation.AccessKey = fbAccessKey;
                                    snsAuthInformation.SecretKey = fbSecretKey;
                                    break;
                                case Constants.TWITTER:
                                    snsAuthInformation.SnsID = com.SnsCorpInformation.SingleOrDefault(p => p.SnsName == Constants.TWITTER).SnsID;
                                    snsAuthInformation.AccessKey = twitterAccessKey;
                                    snsAuthInformation.SecretKey = twitterSecretKey;
                                    break;
                                case Constants.GOOGLE:
                                    snsAuthInformation.SnsID = com.SnsCorpInformation.SingleOrDefault(p => p.SnsName == Constants.GOOGLE).SnsID;
                                    snsAuthInformation.AccessKey = com.SnsCorpInformation.SingleOrDefault(p => p.SnsName == Constants.GOOGLE).AccessKey;
                                    snsAuthInformation.SecretKey = com.SnsCorpInformation.SingleOrDefault(p => p.SnsName == Constants.GOOGLE).SecretKey;
                                    break;
                            }
                            snsAuthInformation.ExpireDate = tokenControl.ExpireDate;
                            com.SnsAuthInformation.Add(snsAuthInformation);
                            com.SaveChanges();
                        }

                        //send mail 
                        EmailSender emailSender = new EmailSender();
                        string title = "【スポログ】ご登録確認メール";
                        string fileName = "RegistConfirm.html";
                        string tokenValue = com.TokenAcInfo.SingleOrDefault(t => t.TokenID.Equals(tokenAcInfo.TokenID)).TokenValue;
                        string url = Url.RouteUrl("active_by_mail", new { uid = member.DisplayMemberId, token = tokenValue }, Request.Url.Scheme);
                        string routeURL = Url.RouteUrl("active_by_mail", new { uid = member.DisplayMemberId, token = tokenValue });

                        Dictionary<string, string> dicContent = new Dictionary<string, string>();
                        dicContent.Add("[URL]", url);
                        dicContent.Add("[routevalue]", routeURL);   //TODO 使用していない？
                        dicContent.Add("[ExpirationTime]", DateTime.Now.AddHours(24).ToString());
                        string result = emailSender.SendEmail(model.Email, fileName, title, dicContent);

                        if (result.Equals(Constants.EMAIL_SEND))
                        {
                            //  Kill session after email is sent
                            Session.Remove("UserInfo");
                            // Insert new row in MailDeliverCond（メール配信条件） after send email successful
                            var memberID = com.Member.FirstOrDefault(m => m.Mail.Equals(model.Email) && m.Status == 0).MemberId;

                            //If select checkbox Receive email, add records to table MailDeliverCond.
                            if(model.ReceiveEmail)
                            {
                                List<MailDeliverCond> mailTypeList = new List<MailDeliverCond>();
                                foreach (var item in com.MailDeliverCondMaster)
                                {
                                    // Set value for new row in MailDeliverCond table
                                    var newMailCondRow = new MailDeliverCond
                                    {
                                        MemberID = memberID,
                                        MailDelivCondID = item.MailDelivCondID,
                                        CreatedDate = System.DateTime.Now
                                    };
                                    mailTypeList.Add(newMailCondRow);
                                }
                                //Insert to MailDeliverCond table
                                com.MailDeliverCond.AddRange(mailTypeList);
                            }
                          
                            com.SaveChanges();
                            transaction.Commit();

                            //Return to view request activation by email
                            return View("RegisterConfirmEmail");
                        }

                        //TODO EMAIL_SEND 以外の場合はどのようなときか？
                    }
                    catch (DbEntityValidationException e)
                    {
                        transaction.Rollback();
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                    }
                }
            }
            return View(model);
        }

        public ActionResult ReConfirmToken(string tokenID)
        {
            bool transactionResult = true;
            // Get display ID and email by tokenID
            var confirmInfo = (from member in com.Member
                               join tokenControl in com.TokenControl on member.MemberId equals tokenControl.MemberID
                               where tokenControl.TokenID.Equals(tokenID)
                               select new
                               {
                                   member.MemberId,
                                   member.DisplayMemberId,
                                   member.Mail
                               }).SingleOrDefault();
            // Set Email Subject and Email Template
            string messageSubject = "【スポログ】ご登録確認メール";
            string messageTemplate = "RegistConfirm.html";
            // Update new info for token
            using (var tokenTransaction = com.Database.BeginTransaction())
            {
                try
                {
                    // Insert new token to TokenControl table
                    var tokenControl = com.TokenControl.SingleOrDefault(t => t.TokenID.Equals(tokenID));
                    tokenControl.IssueDate = System.DateTime.Now;
                    tokenControl.ExpireDate = System.DateTime.Now.AddDays(1);
                    tokenControl.Status = true;
                    com.SaveChanges();

                    var tokenInfo = com.TokenAcInfo.SingleOrDefault(t => t.TokenID.Equals(tokenID));
                    tokenInfo.TokenValue = Utils.MD5Hash(Guid.NewGuid().ToString());
                    com.SaveChanges();

                    tokenTransaction.Commit();
                }
                catch
                {
                    transactionResult = false;
                    tokenTransaction.Rollback();
                }
            }
            EmailSender emailUtil = new EmailSender();
            // Set Email Subject and Email Template
            string tokenValue = com.TokenAcInfo.SingleOrDefault(t => t.TokenID.Equals(tokenID)).TokenValue;
            string url = Url.RouteUrl("active_by_mail", new { uid = confirmInfo.DisplayMemberId, token = tokenValue }, Request.Url.Scheme);
            string routeURL = Url.RouteUrl("active_by_mail", new { uid = confirmInfo.DisplayMemberId, token = tokenValue });
            Dictionary<string, string> dicContent = new Dictionary<string, string>();
            dicContent.Add("[URL]", url);
            dicContent.Add("[routevalue]", routeURL);
            dicContent.Add("[ExpirationTime]", DateTime.Now.AddHours(24).ToString());
            // Send to receiver for account activation                    
            if (emailUtil.SendEmail(confirmInfo.Mail, messageTemplate, messageSubject, dicContent).Equals("Success"))
            {
                return View("RegisterConfirmEmail");
            }
            return View();
        }
        //
        // GET: /Member/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string uid, string token)
        {
            if (Session["CurrentUser"] != null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            else
            {
                bool transactionResult = true;
                string dateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                DateTime dt = DateTime.Parse(dateTime);
                var tokenMember = (from tokenAct in com.TokenAcInfo
                                   join tokenControl in com.TokenControl
                                   on tokenAct.TokenID equals tokenControl.TokenID
                                   where tokenAct.TokenValue.Equals(token) && (tokenControl.ExpireDate.HasValue && tokenControl.ExpireDate.Value >= dt)
                                   select new
                                   {
                                       tokenControl.TokenClass,
                                       tokenControl.TokenID,
                                       tokenAct.TokenValue,
                                       tokenControl.ExpireDate

                                   }).FirstOrDefault();
                var user = com.Member.SingleOrDefault(u => u.DisplayMemberId.Equals(uid));
                if (tokenMember == null)
                {
                    ViewBag.TokenID = tokenMember.TokenID;
                    return View("TokenTimeOut");
                }
                if (user == null)
                {
                    return View("Error");
                }
                if (user != null && tokenMember != null)
                {
                    if (tokenMember.TokenClass == 2)
                    {
                        ViewBag.UserMail = user.Mail;
                        return View("ChangePassword");
                    }
                    else if (tokenMember.TokenClass == 1)
                    {
                        List<Point> point = new List<Point>();
                        int fundsPoint = int.Parse(com.SystemParamater.SingleOrDefault(sp => sp.Scode == 2).Spara);
                        DateTime currentTime = System.DateTime.Now;

                        #region Phase 1
                        //int delta = DayOfWeek.Monday - DateTime.Now.DayOfWeek;
                        //if (delta > 0)
                        //    delta -= 7;
                        //DateTime thisWeekStart = DateTime.Now.AddDays(delta);
                        //short givingYear = (short)GetFirstDayOfWeek(currentTime).Year;
                        //short givingMonth = (short)thisWeekStart.Month;
                        //short givingWeek = (short)Utils.GetWeekNumberOfMonth(currentTime);
                        //DateTime betStartDate = Utils.GetStartOfDay(thisWeekStart);
                        //DateTime betEndDate = Utils.GetEndOfDay(betStartDate.AddDays(6));
                        #endregion

                        #region Phase 2
                        //CucHTP Modified : 2015/06/23
                        //Phase 2 : Change weekly to monthly.
                        short givingYear = (short)currentTime.Year;
                        short givingMonth = (short)currentTime.Month;
                        short givingWeek = 0;
                        var firstDayOfMonth = new DateTime(currentTime.Year, currentTime.Month, 1);
                        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                        DateTime betStartDate = new DateTime(firstDayOfMonth.Year, firstDayOfMonth.Month, firstDayOfMonth.Day, 0, 0, 0);
                        DateTime betEndDate = new DateTime(lastDayOfMonth.Year, lastDayOfMonth.Month, lastDayOfMonth.Day, 23, 59, 59);                       
                        #endregion                        
                        
                        var point1 = new Point
                        {
                            MemberID = user.MemberId,
                            FundsPoint = fundsPoint,
                            PossesionPoint = fundsPoint,
                            GivingDate = currentTime,
                            GiveTargetYear = givingYear,
                            GiveTargetMonth = givingMonth,
                            GiveTargetWeek = givingWeek,
                            BetStartDate = betStartDate,
                            BetEndDate = betEndDate,
                            PayOffFlg = false,
                            CreatedAccountID = "Create member",
                            CreatedDate = currentTime
                        };
                        point.Add(point1);

                        #region Phase 1
                        //DateTime nextweek = betEndDate.AddDays(1);
                        //givingYear = (short)GetFirstDayOfWeek(nextweek).Year;
                        //givingMonth = (short)nextweek.Month;
                        //givingWeek = (short)Utils.GetWeekNumberOfMonth(nextweek);
                        //betStartDate = Utils.GetStartOfDay(nextweek);
                        //betEndDate = Utils.GetEndOfDay(betStartDate.AddDays(6));                        
                        //var point2 = new Point
                        //{
                        //    MemberID = user.MemberId,
                        //    FundsPoint = fundsPoint,
                        //    PossesionPoint = fundsPoint,
                        //    GivingDate = currentTime,
                        //    GiveTargetYear = givingYear,
                        //    GiveTargetMonth = givingMonth,
                        //    GiveTargetWeek = givingWeek,
                        //    BetStartDate = betStartDate,
                        //    BetEndDate = betEndDate,
                        //    PayOffFlg = false,
                        //    CreatedAccountID = "Create member",
                        //    CreatedDate = currentTime
                        //};
                        //point.Add(point2);
                        #endregion

                        #region Phase 2
                        //CucHTP Modified : 2015/06/23
                        //Phase 2 : Insert 1 record to PointHistory.
                        PointHistory pointHis = new PointHistory();
                        pointHis.PointClass = 1;
                        pointHis.Points = fundsPoint;
                        pointHis.HistoryClass = 1;
                        pointHis.AdjustmentClass = 1;
                        pointHis.OperationClass = 2;
                        pointHis.Status = true;
                        pointHis.CreatedAccountID = "Create member";
                        pointHis.CreatedDate = currentTime;
                        #endregion

                        using (var updateUserInfoTransaction = com.Database.BeginTransaction())
                        {
                            try
                            {
                                var statusActive = com.Member.SingleOrDefault(p => p.DisplayMemberId == uid).Status;
                                if (statusActive == 0)
                                {
                                    // Active account in order to be able to login to system
                                    user.Status = 1;
                                    //user.ModifiedDate_ = System.DateTime.Now;
                                    user.RegistTime = System.DateTime.Now;
                                    // Update user infomation

                                    com.Point.AddRange(point);
                                    com.SaveChanges();

                                    #region Phase 2
                                    //CucHTP Modified : 2015/06/23
                                    //Phase 2 : Insert 1 record to PointHistory.
                                    //Get pointID of record that have just inserted.
                                    var pointID = com.Point.Where(m => m.MemberID == user.MemberId && m.GiveTargetMonth == givingMonth && m.GiveTargetYear == givingYear).Select(m => m.PointID).FirstOrDefault();

                                    //Add new record to table PointHistory.
                                    pointHis.PointId = pointID;
                                    com.PointHistory.Add(pointHis);
                                    com.SaveChanges();
                                    #endregion                                    

                                    var tokenControl = com.TokenControl.SingleOrDefault(t => t.MemberID == user.MemberId);
                                    tokenControl.Status = false;

                                    // Update to database
                                    com.SaveChanges();
                                    //Commit all transactions
                                    updateUserInfoTransaction.Commit();
                                }
                                else
                                {
                                    return View("RegisterSuccess");
                                }
                            }
                            catch
                            {
                                transactionResult = false;
                                updateUserInfoTransaction.Rollback();
                            }
                        }
                        if (transactionResult)
                        {
                            var loginHistory = new LoginHistory
                            {
                                MemberID = user.MemberId,
                                LoginDate = System.DateTime.Now
                            };
                            com.LoginHistory.Add(loginHistory);
                            com.SaveChanges();
                            Session["UserInfo"] = null;
                            Session["CurrentUser"] = user.MemberId;
                            MyCookie.SetCookie(user.MemberId.ToString());
                            return View("RegisterSuccess");
                        }
                    }
                }
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        #endregion

        #region Facebook Login
        public ActionResult FacebookLogin()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = fbAccessKey,
                client_secret = fbSecretKey,
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email,user_hometown,user_birthday" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback()
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = fbAccessKey,
                client_secret = fbSecretKey,
                redirect_uri = RedirectUri.AbsoluteUri,
                code = Request.QueryString["code"]
            });
            var accessToken = result.access_token;
            var expires = result.expires;
            // Store the access token in the session for farther use
            Session["AccessToken"] = accessToken;
            // update the facebook client with the access token so 
            // we can make requests on behalf of the user
            fb.AccessToken = accessToken;
            // Get the user's information
            dynamic me = fb.Get("/me");
            MemberRegistViewModel model = new MemberRegistViewModel();
            model.Email = me.email;
            model.FullName = me.name;
            if (!string.IsNullOrEmpty(me.gender))
            {
                model.Gender = me.gender.Equals("male") ? 1 : 0;
            }
            if (!string.IsNullOrEmpty(me.birthday))
            {
                model.Birthday = Convert.ToDateTime(me.birthday);
            }
            model.ImageLink = GetPictureUrl(me.id); ;
            model.SNSName = Constants.FACEBOOK;
            model.IsSNS = true;
            model.SNSID = me.id;
            model.ReceiveEmail = true;
            model.NickName = me.name;
            model.Place = (me.hometown) != null ? me.hometown.name : string.Empty;

            if (ComCommon.CheckEmailAvailability(model.Email))
            {
                var member = ComCommon.GetMemberLogin(model.Email);
                if (InsertNewLoginHistory(member.MemberId))
                {
                    //write cookie user login
                    MyCookie.SetCookie(member.MemberId.ToString());
                    Session["CurrentUser"] = member.MemberId;
                    Session["Sns"] = 1;
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["UserInfo"] = model;
                return View("SocialNetworkInfo", model);
            }
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public static string GetPictureUrl(string faceBookId)
        {
            WebResponse response = null;
            string pictureUrl = Constants.IMG_DEFAULT_PROFILE;
            try
            {
                WebRequest request = WebRequest.Create(string.Format("https://graph.facebook.com/{0}/picture", faceBookId));
                response = request.GetResponse();
                pictureUrl = response.ResponseUri.ToString();
            }
            catch (Exception ex)
            {
                //? handle
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return pictureUrl;
        }
        #endregion

        #region Google Login
        public ActionResult GoogleLogin()
        {
            return RedirectToAction("LoginGoogleAsync", "Member");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<ActionResult> LoginGoogleAsync(CancellationToken cancellationToken)
        {

            var result = await new AuthorizationCodeMvcApp(this, new AppAuthFlowMetadata()).AuthorizeAsync(cancellationToken);
            if (result.Credential != null)
            {
                var plusService = new PlusService(new BaseClientService.Initializer
                {
                    ApplicationName = "Splg",
                    HttpClientInitializer = result.Credential,
                });
                //get the user basic information
                var me = plusService.People.Get("me").Execute();
                MemberRegistViewModel model = new MemberRegistViewModel();

                model.Email = me.Emails.SingleOrDefault().Value;
                model.FullName = me.DisplayName;
                if (!string.IsNullOrEmpty(me.Birthday))
                {
                    model.Birthday = Convert.ToDateTime(me.Birthday);
                }
                if (!string.IsNullOrEmpty(me.Gender))
                {
                    model.Gender = me.Gender.Equals("male") ? 1 : 0;
                }
                model.ImageLink = me.Image.Url;
                model.SNSName = Constants.GOOGLE;
                model.SNSID = me.Id;
                model.IsSNS = true;
                model.ReceiveEmail = true;
                model.NickName = me.Nickname;
                //clearn token google
                AppAuthFlowMetadata.GetFileDataStore();
                if (ComCommon.CheckEmailAvailability(model.Email))
                {
                    var member = ComCommon.GetMemberLogin(model.Email);
                    if (InsertNewLoginHistory(member.MemberId))
                    {
                        //write cookie user login
                        MyCookie.SetCookie(member.MemberId.ToString());
                        Session["CurrentUser"] = member.MemberId;
                        Session["Sns"] = 2;
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Session["UserInfo"] = model;
                    return View("SocialNetworkInfo", model);
                }

            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }
        #endregion

        #region Twitter Login
        //// Load default authencation key value for twitter
        public ActionResult TwitterLogin()
        {
            // Step 1 - Retrieve an OAuth Request Token
            TwitterService service = new TwitterService(twitterAccessKey, twitterSecretKey);
            string urlCallback = Url.Action("TwitterCallback", "Member", null, Request.Url.Scheme);
            // This is the registered callback URL
            OAuthRequestToken requestToken = service.GetRequestToken(urlCallback);
            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = service.GetAuthorizationUri(requestToken);
            return new RedirectResult(uri.ToString(), false /*permanent*/);
        }

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier)
        {
            var requestToken = new OAuthRequestToken { Token = oauth_token };
            // Step 3 - Exchange the Request Token for an Access Token
            TwitterService service = new TwitterService(twitterAccessKey, twitterSecretKey);
            OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauth_verifier);
            // Step 4 - User authenticates using the Access Token
            service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            TwitterUser twitterUser = service.VerifyCredentials(new VerifyCredentialsOptions());
            TwitterAccount account = service.GetAccountSettings();
            MemberRegistViewModel model = new MemberRegistViewModel();
            model.FullName = twitterUser.ScreenName;
            //model.Birthday = twitterUser.CreatedDate;
            model.Gender = null;
            model.ImageLink = twitterUser.ProfileImageUrl;
            model.SNSName = Constants.TWITTER;
            model.IsSNS = true;
            model.SNSID = twitterUser.Id.ToString();
            model.ReceiveEmail = true;
            model.NickName = twitterUser.Name;
            model.Place = twitterUser.Location;
            Int64 memberId = ComCommon.GetMemberIDInSnsAuthInfo(model.SNSID);
            if (memberId > 0)
            {
                if (InsertNewLoginHistory(memberId))
                {
                    //write cookie user login
                    MyCookie.SetCookie(memberId.ToString());
                    Session["CurrentUser"] = memberId;
                    Session["Sns"] = 3;
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["UserInfo"] = model;
                return View("SocialNetworkInfo", model);
            }
        }

        #endregion

        #region Login
        public ActionResult Login()
        {
            if (Session["CurrentUser"] != null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var member = ComCommon.GetMemberLogin(model.Email, model.Password);
                Int64 memberId = member.MemberId;
                if (InsertNewLoginHistory(memberId))
                {
                    //write cookie user login
                    MyCookie.SetCookie(memberId.ToString());
                    Session["CurrentUser"] = memberId;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }


        [HttpPost]
        public ActionResult LoginApp(LoginViewModel model)
        {
            if (Session["CurrentUser"] != null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            else
            {
                string success = "success";
                if (ModelState.IsValid)
                {
                    var member = ComCommon.GetMemberLogin(model.Email, model.Password);
                    string logoutUrl = Url.RouteUrl("2_3", null, Request.Url.Scheme);
                    if (InsertNewLoginHistory(member.MemberId))
                    {
                        //write cookie user login
                        MyCookie.SetCookie(member.MemberId.ToString());
                        Session["CurrentUser"] = member.MemberId;
                    }
                    return Json(success);
                }
                else
                {
                    return Json(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()).Where(m => m.Value.Any()));
                }
            }
        }
        #endregion

        #region Logout
        /// <summary>
        /// ログアウト確認
        /// </summary>
        [HttpGet]
        public ActionResult Logout()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                //301Redirect
                return RedirectToActionPermanent("Login", "Member");
            }

            return View();
        }

        /// <summary>
        /// ログアウト
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logout(string typeID)
        {
            //パラメーター:typeIDの用途が不明

            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                //301リダイレクト
                return RedirectToActionPermanent("Login", "Member");
            }

            Session.Remove("CurrentUser");
            Session.Remove("Sns");
            Response.Cookies[Constants.AUTH_COOKIE].Expires = DateTime.Now.AddYears(-2);
            return View("LogoutComplete");
        }
        #endregion

        #region Delete Account
        public ActionResult Delete()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DeleteAccount(string typeID)
        {
            var currenUser = Session["CurrentUser"] as string;
            long currentUserID = long.Parse(currenUser);
            var user = com.Member.SingleOrDefault(u => u.MemberId == currentUserID);
            if (user != null)
            {
                user.Status = short.Parse(typeID);
                int result = com.SaveChanges();
                if (result > 0)
                {
                    return View("DeleteComplete");
                }
                else
                {
                    return View("Error");
                }
            }
            return View();
        }
        #endregion

        #region Forgot Password
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = com.Database.BeginTransaction())
                {
                    try
                    {
                        //get one item object member
                        var objMember = com.Member.SingleOrDefault(p => p.Mail == model.Email);
                        //insert data to table TokenAcInfo
                        TokenAcInfo tokenAcInfo = new TokenAcInfo();
                        tokenAcInfo.TokenID = Guid.NewGuid().ToString("n");
                        tokenAcInfo.TokenValue = Utils.MD5Hash(Guid.NewGuid().ToString());
                        com.TokenAcInfo.Add(tokenAcInfo);
                        com.SaveChanges();

                        //insert data to table TokenControl
                        TokenControl tokenControl = new TokenControl();
                        tokenControl.TokenID = tokenAcInfo.TokenID;
                        tokenControl.TokenClass = 2;
                        tokenControl.Status = true;
                        tokenControl.MemberID = com.Member.FirstOrDefault(p => p.Mail == model.Email).MemberId;
                        tokenControl.IssueDate = System.DateTime.Now;
                        tokenControl.ExpireDate = System.DateTime.Now.AddDays(1);
                        com.TokenControl.Add(tokenControl);
                        com.SaveChanges();
                        transaction.Commit();

                        //send mail 
                        EmailSender emailSender = new EmailSender();
                        string title = "【スポログ】パスワード再設定のお手続き";
                        string fileName = "RegistConfirm.html";
                        string tokenValue = com.TokenAcInfo.SingleOrDefault(t => t.TokenID.Equals(tokenAcInfo.TokenID)).TokenValue;
                        string url = Url.RouteUrl("active_by_mail", new { uid = objMember.DisplayMemberId, token = tokenValue }, Request.Url.Scheme);
                        string routeURL = Url.RouteUrl("active_by_mail", new { uid = objMember.DisplayMemberId, token = tokenValue });

                        Dictionary<string, string> dicContent = new Dictionary<string, string>();
                        dicContent.Add("[URL]", url);
                        dicContent.Add("[routevalue]", routeURL);
                        dicContent.Add("[ExpirationTime]", DateTime.Now.AddHours(24).ToString());
                        string result = emailSender.SendEmail(model.Email, fileName, title, dicContent);

                        if (result.Equals(Constants.EMAIL_SEND))
                        {
                            return View("ForgotConfirmEmail");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return View(model);
        }
        #endregion Forgot Password

        #region Change Password
        //[AllowAnonymous]
        //public ActionResult ChangePassword()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = ViewBag.UserMai;
                var member = com.Member.SingleOrDefault(m => m.Mail.Equals(model.Email));
                if (member != null)
                {
                    member.Password = Utils.MD5Hash(model.Password);
                    com.SaveChanges();
                    return RedirectToAction("Login", "Member");
                }
            }
            return View(model);
        }
        #endregion

        #region Change EMail
        //
        // GET: /Member/ChangeEmail
        [AllowAnonymous]
        public async Task<ActionResult> ChangeEmail(string uid, string token)
        {
            //if (Session["CurrentUser"] != null)
            //{
            //    return RedirectToActionPermanent("Index", "Home");
            //}
            //else
            //{
                bool transactionResult = true;
                string dateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                DateTime dt = DateTime.Parse(dateTime);
                var tokenMember = (from tokenAct in com.TokenAcInfo
                                   join tokenControl in com.TokenControl
                                   on tokenAct.TokenID equals tokenControl.TokenID
                                   where tokenAct.TokenID.Equals(token) && (tokenControl.ExpireDate.HasValue && tokenControl.ExpireDate.Value >= dt)
                                   select new
                                   {
                                       tokenControl.TokenClass,
                                       tokenControl.TokenID,
                                       tokenAct.TokenValue,
                                       tokenControl.ExpireDate

                                   }).FirstOrDefault();

                if (tokenMember == null)
                {
                    ViewBag.TokenID = tokenMember.TokenID;
                    return View("TokenTimeOut");
                }
                if ( tokenMember != null)
                {
                    if (tokenMember.TokenClass == 3)
                    {
                        using (var updateUserInfoTransaction = com.Database.BeginTransaction())
                        {
                            try
                            {
                                var member = com.Member.SingleOrDefault(p => p.DisplayMemberId == uid);
                                if (member != null)
                                {
                                    //不明　要確認
                                    //var tokenControl = com.TokenControl.SingleOrDefault(t => t.MemberID == user.MemberId);
                                    //tokenControl.Status = false;

                                    member.Mail = tokenMember.TokenValue;

                                    // Update to database
                                    com.SaveChanges();
                                    //Commit all transactions
                                    updateUserInfoTransaction.Commit();
                                }
                                else
                                {
                                    return RedirectToActionPermanent("Index", "Home");
                                }
                            }
                            catch
                            {
                                transactionResult = false;
                                updateUserInfoTransaction.Rollback();
                            }
                        }
                    }
                }
                return View("ChangeEMailSuccess");
            }
        //}
        #endregion

    }
}
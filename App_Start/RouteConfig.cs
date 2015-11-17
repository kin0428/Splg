using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Splg
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.AppendTrailingSlash = true;
            routes.LowercaseUrls = true;

            var dataTokens = new RouteValueDictionary();
            var ns = new string[] { "Splg.Controllers" };
            dataTokens["Namespaces"] = ns;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //7-1	チーム情報　ニュース（プロ野球)
            //spolog.jp/{topic}/news/{newsItemID}/
            routes.MapRoute(
                 "7_1",
                 "{topic}/news/{newsItemID}/{sportID}/{uniqueID}/{teamID}",
                 new
                 {
                     controller = "NewsArticleCommon",
                     action = "Index",
                     uniqueID = UrlParameter.Optional,
                     teamID = UrlParameter.Optional
                 }
                 );
            #region 入会 - ログイン　-　ログアウト　-　退会　の　URL
            //2-1 入会（入力画面）
            //spolog.jp/signup/
            routes.MapRoute(
                "2_1",
                "signup/",
                new { controller = "Member", action = "Register" }
                );
            //2-1-1 入会（確認画面）
            //spolog.jp/signup/{typeID}
            routes.MapRoute(
                "2_1_1",
                "signup/{typeID}",
                new { controller = "Member", action = "RegisterReview", typeID = UrlParameter.Optional }
                );

            //2-2	ログイン
            //spolog.jp/login
            routes.MapRoute(
                "2_2",
                "login/",
                new { controller = "Member", action = "Login" }
                );
            //2-2	ログイン
            //spolog.jp/login
            routes.MapRoute("loginByApp","loginapp/",new { controller = "Member", action = "LoginApp" },new { httpMethod = new HttpMethodConstraint("POST") });
            //2-3 ログアウト（確認画面）
            //spolog.jp/logout
            routes.MapRoute(
                "2_3",
                "logout/",
                new { controller = "Member", action = "Logout" }
                );
            //2-3-1 ログアウト（完了画面）
            //spolog.jp/logout/{typeID}
            routes.MapRoute(
                "2_3_1",
                "logout/1",
                new { controller = "Member", action = "Logout" },
                new { httpMethod = new HttpMethodConstraint("POST") }
                );
            //2-4	退会（確認画面）
            //spolog.jp/delete
            routes.MapRoute(
                "2_4",
                "delete/",
                new { controller = "Member", action = "Delete" }
                );
            //2-4-1	ログイン
            //spolog.jp/delete/{typeID}
            routes.MapRoute(
                "2_4_1",
                "delete/{typeID}",
                new { controller = "Member", action = "DeleteAccount" },
                new { typeID = "d+" }
                );
            //メール　の　URL
            //spolog.jp/mail/active/{token}
            routes.MapRoute(
                "active_by_mail",
                "mail/active/{uid}/{token}",
                new { controller = "Member", action = "ConfirmEmail" }
                );

            //メール　の　変更
            //spolog.jp/mail/change/{token}
            routes.MapRoute(
                "change_mail",
                "mail/change/{uid}/{token}",
                new { controller = "Member", action = "ChangeEmail" }
                );

            //メール　の　URL
            //spolog.jp/forgotPassword
            routes.MapRoute(
               "forgot_password",
               "forgotpassword/",
               new { controller = "Member", action = "ForgotPassword" }
               );
            #endregion

            #region 投稿記事
            //5-1 投稿記事（投稿記事TOP）													
            //spolog.jp/user_article/
            routes.MapRoute(
                "5_1",
                 "user_article/",
                 new { controller = "UserArticle", action = "Index" },
                 null,
                 new string[] { "Splg.Controllers" }
                );
            //5-2
            //spolog.jp/user_article/{topicName}/topic
            routes.MapRoute(
                "5_2",
                 "user_article/{newsClassID}/topic",
                 new
                 {
                     controller = "UserArticle",
                     action = "SearchByCategory",
                     newsClassID = UrlParameter.Optional
                 },
                 new
                 {
                     newsClassID = @"\d+"
                 },
                 new string[] { "Splg.Controllers" }
                );
            //5-2
            //spolog.jp/user_article/{sportID}/sport
            routes.MapRoute(
                "5_2_by_sport",
                 "user_article/{sportID}/sport",
                 new
                 {
                     controller = "UserArticle",
                     action = "ViewArticleBySport",
                     sportID = UrlParameter.Optional
                 },
                 new
                 {
                     sportID = @"\d+"
                 },
                 new string[] { "Splg.Controllers" }
                );
            //5-3
            //spolog.jp/user_article/{topicName}/{id}
            routes.MapRoute(
                "5_3",
                 "user_article/{newsClassID}/{contributeID}",
                 new
                 {
                     controller = "UserArticle",
                     action = "SearchByCategory",
                     newsClassID = UrlParameter.Optional,
                     contributeID = UrlParameter.Optional
                 },
                 new
                 {
                     newsClassID = @"\d+"
                 },
                 new string[] { "Splg.Controllers" }
                );

            //5-3-1 #1295対応版の５－３
            //spolog.jp/user_article/show/{contributeID}/
            routes.MapRoute(
                "5-3-1",
                 "user_article/show/{contributeID}",
                 new
                 {
                     controller = "UserArticle",
                     action = "Show"
                 },
                 new string[] { "Splg.Controllers" }
            );

            //5-4 投稿記事　記事作成（入力画面）													
            //spolog.jp/user_article/new/{newsClassID}/{newsItemID}
            routes.MapRoute(
                 "5-4",
                 "user_article/new/{newsClassId}/{quoteUniqueId1}/{quoteUniqueId2}/{quoteUniqueId3}",
                 new
                 {
                     controller = "UserArticle",
                     action = "AddNewArticle",
                     quoteUniqueId2 = UrlParameter.Optional,
                     quoteUniqueId3 = UrlParameter.Optional
                 },
                 new string[] { "Splg.Controllers" }
                 );
            //5-4-1 投稿記事　記事作成（入力画面）													
            //spolog.jp/user_article/new/{newsClassID}/{newsItemID}
            routes.MapRoute(
                "5_4_1",
                "user_article/new/confirm",
                new
                {
                    controller = "UserArticle",
                    action = "ConfirmUserArticle",
                    page = "new"
                },
                new string[] { "Splg.Controllers" }
                );
            //5-4-1 投稿記事　記事作成（入力画面）													
            //spolog.jp/user_article/new/{newsClassID}/{newsItemID}
            routes.MapRoute(
                "5_4_2",
                "user_article/new/complete",
                new
                {
                    controller = "UserArticle",
                    action = "CompleteAddNewArticle",
                    page = "new"
                },
                new string[] { "Splg.Controllers" }
                );

            //5-4-3 投稿記事　記事作成（編集画面）
            //spolog.jp/user_article/edit
            routes.MapRoute(
                "5_4_3",
                 "user_article/edit",
                 new { controller = "UserArticle", action = "EditNewUserArticle" },
                 null,
                 new string[] { "Splg.Controllers" }
                );
            routes.MapRoute(
                "5_4_4",
                 "user_article/delete",
                 new { controller = "UserArticle", action = "DeleteNewUserArticle" },
                 null,
                 new string[] { "Splg.Controllers" }
                );
            //3-1-1	マイページ	
            //spolog.jp/mypage/changeyearmonth/{yearmonth}/
            routes.MapRoute(
              "5_4-1-1",
              "user_article/getmemberreportinfo/{memberId}/{yearmonth}",
                new { controller = "UserArticle", action = "GetMemberReportInfo" },
                null,
                new string[] { "Splg.Controllers" }
                );
            #endregion

            routes.MapRoute("search", "search/", new { controller = "Home", action = "GoogleCse" });
            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}", defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            #region Support
            //12-1
            routes.MapRoute("12_1", "faq/", new { controller = "Support", action = "Faq" });
            //12-2
            routes.MapRoute("12_2","rules/", new { controller = "Support", action = "Rules" });
            //12-3
            routes.MapRoute("12_3","contact/", new { controller = "Support", action = "Contact" });
            //12-4
            routes.MapRoute("12_4", "howto/", new { controller = "Support", action = "Howto" });
            #endregion
        }
    }
}

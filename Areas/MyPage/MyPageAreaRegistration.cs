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
 * Namespace	: Splg.Areas.MyPage
 * Class		: MyPageAreaRegistration
 * Developer	: Auto created by project 
 */
#endregion

using System.Web.Mvc;
using Splg.Core.Constant;

namespace Splg.Areas.MyPage
{
    public class MyPageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MyPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {


            //ModifiedDev : Nojima
            //ModifiedDate : 2015/04/12

            //3-1	マイページ	
            //spolog.jp/mypage/
            context.MapRoute(
              "MyPage_3-1",
              "mypage/",
              new
              {
                  Controller = "MyPageTop",
                  action = "Index"
              });
            //3-1-1	マイページ	
            //spolog.jp/mypage/changeyearmonth/{yearmonth}/
            context.MapRoute(
              "MyPage_3-1-1",
              "mypage/ChangeYearMonth/{yearmonth}/",
              new
              {
                  Controller = "MyPageTop",
                  action = "ChangeYearMonth"
              });

            //3-2	予想リスト	
            //spolog.jp/mypage/expectedlist
            context.MapRoute(
              "MyPage_3-2",
              "mypage/expectedlist/",
              new
              {
                  Controller = "MyPageExpectedList",
                  action = "Index"
              });
            //3-2_a	予想リスト	
            //spolog.jp/mypage/expectedlist/ShowExpectedList/
            context.MapRoute(
              "MyPage_3-2_a",
              "mypage/expectedlist/ShowExpectedList/",
              new
              {
                  Controller = "MyPageExpectedList",
                  action = "ShowExpectedList"
              });
            //3-2_b 予想リスト	キャンセル
            //spolog.jp/mypage/expectedlist/expectcancel/{game_id}/
            context.MapRoute(
              "MyPage_3-2_b",
              "mypage/expectedlist/ExpectCancel/{game_id}/",
              new
              {
                  Controller = "MyPageExpectedList",
                  action = "ExpectCancel"
              });
            //3-2_c 予想リスト	ポイント入力
            //spolog.jp/mypage/expectedlist/expectpoint/{game_id}/
            context.MapRoute(
              "MyPage_3-2_c",
              "mypage/expectedlist/ExpectPoint/{game_id}/",
              new
              {
                  Controller = "MyPageExpectedList",
                  action = "ExpectPoint"
              });

            //3-3	投稿記事	
            //spolog.jp/mypage/article/
            context.MapRoute(
              "MyPage_3-3",
              "mypage/article/",
              new
              {
                  Controller = "MyPageArticle",
                  action = "Index"
              });

            //3-3_a	投稿記事	
            //spolog.jp/mypage/article/
            context.MapRoute(
              "MyPage_3-3_a",
              "mypage/article/{currentCount}/",
              new
              {
                  Controller = "MyPageArticle",
                  action = "GetMoreArticles"
              });

            //3-4	グループリスト	
            //spolog.jp/mypage/group/
            context.MapRoute(
              "MyPage_3-4",
              "mypage/group/",
              new
              {
                  Controller = "MyPageGroupList",
                  action = "Index"
              });
            ////3-4_a	グループリスト	
            ////spolog.jp/mypage/group/
            //context.MapRoute(
            //  "MyPage_3-4_a",
            //  "mypage/group/{currentCount}/",
            //  new
            //  {
            //      Controller = "MyPageGroupList",
            //      action = "GetMoreGroups"
            //  });

            //3-4-1	グループページ	
            //spolog.jp/mypage/group/{group_id}/
            context.MapRoute(
              "MyPage_3-4-1",
              "mypage/group/{group_id}/detail",
              new
              {
                  Controller = "MyPageGroupDetails",
                  action = "Index",
                  GroupID = UrlParameter.Optional
              });
            //3-4-2	グループ作成（パーシャルビュー：試合情報）	
            //spolog.jp/mypage/group/{group_id}/detail/ShowGameInfo?type=1&gameDate=1&groupID=1
            context.MapRoute(
              "MyPage_3-4-1-1",
              "mypage/group/{group_id}/detail/ShowGameInfo/{type}/{gameDate}/{groupID}",
              new
              {
                  Controller = "MyPageGroupDetails",
                  action = "ShowGameInfo",
                  GRPUP_ID = UrlParameter.Optional,
                  TYPE = UrlParameter.Optional,
                  GAMEDATE = UrlParameter.Optional,
                  GROUPID = UrlParameter.Optional
              });

            //3-4-2	グループ作成	
            //spolog.jp/mypage/group/new/
            context.MapRoute(
              "MyPage_3-4-2",
              "mypage/group/new/",
              new
              {
                  Controller = "MyPageGroupNew",
                  action = "Index"
              });

            //3-4-3	グループ編集	
            //spolog.jp/mypage/group/{group_id}/edit/
            context.MapRoute(
              "MyPage_3-4-3",
              "mypage/group/{group_id}/edit/",
              new
              {
                  Controller = "MyPageGroupEdit",
                  action = "Index",
                  GroupID = UrlParameter.Optional
              });


            //3-5	フォローリスト	
            //spolog.jp/mypage/following/
            context.MapRoute(
              "MyPage_3-5",
              "mypage/following/",
              new
              {
                  Controller = "MyPageFollowing",
                  action = "Index"
              });

            //3-6_a	フォローリスト	
            //spolog.jp/mypage/following/
            context.MapRoute(
              "MyPage_3-5_a",
              "mypage/following/{currentCount}/",
              new
              {
                  Controller = "MyPageFollowing",
                  action = "GetMoreFollowings"
              });

            //3-5_b	フォローリスト ajax follow
            //spolog.jp/mypage/following/follow
            context.MapRoute(
              "MyPage_3-5_b",
              "mypage/following/follow/{followingMemberId}/",
              new
              {
                  Controller = "MyPageFollowing",
                  action = "follow"
              });

            //3-5_c	フォローリスト ajax unfollow	
            //spolog.jp/mypage/following/unfollow
            context.MapRoute(
              "MyPage_3-5_c",
              "mypage/following/unfollow/{followingMemberId}/",
              new
              {
                  Controller = "MyPageFollowing",
                  action = "unfollow"
              });

            //3-6	フォロワーリスト	
            //spolog.jp/mypage/followers/
            context.MapRoute(
              "MyPage_3-6",
              "mypage/followers/",
              new
              {
                  Controller = "MyPageFollowers",
                  action = "Index"
              });

            //3-6_a	フォロワーリスト	
            //spolog.jp/mypage/article/
            context.MapRoute(
              "MyPage_3-6_a",
              "mypage/followers/{currentCount}/",
              new
              {
                  Controller = "MyPageFollowers",
                  action = "GetMoreFollowers"
              });

            //3-6_b	フォロワーリスト ajax follow
            //spolog.jp/mypage/followers/follow
            context.MapRoute(
              "MyPage_3-6_b",
              "mypage/followers/follow/{followingMemberId}/",
              new
              {
                  Controller = "MyPageFollowers",
                  action = "follow"
              });

            //3-6_c	フォロワーリスト ajax unfollow	
            //spolog.jp/mypage/followers/unfollow
            context.MapRoute(
              "MyPage_3-6_c",
              "mypage/followers/unfollow/{followingMemberId}/",
              new
              {
                  Controller = "MyPageFollowers",
                  action = "unfollow"
              });

            //3-7	設定	
            //spolog.jp/mypage/setting/
            context.MapRoute(
              "MyPage_3-7",
              "mypage/setting/",
              new
              {
                  Controller = "MyPageSetting",
                  action = "Index"
              });

            //3-7-a	設定　保存
            //spolog.jp/mypage/setting/savesetting/
            context.MapRoute(
              "MyPage_3-7_a",
              "mypage/setting/savesetting/",
              new
              {
                  Controller = "MyPageSetting",
                  action = "SaveSetting"
              });

            //3-7_b	設定	プロファイル画像アップロード
            //spolog.jp/mypage/setting/uploadprofileimg/
            context.MapRoute(
              "MyPage_3-7_b",
              "mypage/setting/uploadprofileimg/",
              new
              {
                  Controller = "MyPageSetting",
                  action = "UploadProfileImage"
              });

            //3-7-1	メール設定	
            //spolog.jp/mypage/setting/savemailsetting/
            context.MapRoute(
              "MyPage_3-7-1",
              "mypage/setting/savemailsetting/",
              new
              {
                  Controller = "MyPageSetting",
                  action = "SaveMailSetting"
              });

            //3-7-1	設定　メールアドレス変更	
            //spolog.jp/mypage/setting/address/
            context.MapRoute(
              "MyPage_3-7-1-1",
              "mypage/setting/address/",
              new
              {
                  Controller = "MyPageSettingAddress",
                  action = "Index"
              });
            //3-7-1-1	設定　メールアドレス変更	
            //spolog.jp/mypage/setting/changeaddress/
            context.MapRoute(
              "MyPage_3-7-1-2",
              "mypage/setting/address/{email}/{password}/change/",
              new
              {
                  Controller = "MyPageSettingAddress",
                  action = "ChangeAddress"
              });

            //3-7-2	設定　パスワード変更	
            //spolog.jp/mypage/setting/password/
            context.MapRoute(
              "MyPage_3-7-2",
              "mypage/setting/password/",
              new
              {
                  Controller = "MyPageSettingPassword",
                  action = "Index"
              });

            //3-7-2	設定　パスワード変更	
            //spolog.jp/mypage/setting/password/
            context.MapRoute(
              "MyPage_3-7-2_a",
              "mypage/setting/password/{expass}/{npass}/{npass_confirm}/change/",
              new
              {
                  Controller = "MyPageSettingPassword",
                  action = "ChangePassword"
              });

            //3-7-3	設定　アカウントの削除	
            //spolog.jp/mypage/setting/password/
            context.MapRoute(
              "MyPage_3-7-3",
              "mypage/setting/deleteaccount/",
              new
              {
                  Controller = "MyPageSettingDeleteAccount",
                  action = "Index"
              });


            //3-7-3_a	設定　アカウントの削除	
            //spolog.jp/mypage/setting/password/
            context.MapRoute(
              "MyPage_3-7-3_a",
              "mypage/setting/deleteaccount/execute/",
              new
              {
                  Controller = "MyPageSettingDeleteAccount",
                  action = "Index"
              });

            //3-8	お知らせ	
            //spolog.jp/mypage/notice/
            context.MapRoute(
              "MyPage_3-8",
              "mypage/notice/",
              new
              {
                  Controller = "MyPageNotice",
                  action = "Index",
                  managementNoticeCount = UrlParameter.Optional,
                  userNoticeCount = UrlParameter.Optional
              });

            //3-8_a	運営からのお知らせのもっと見る取得
            //spolog.jp/mypage/notice/
            context.MapRoute(
              "MyPage_3-8_a",
              "mypage/notice/management/{managementnoticecount}/",
              new
              {
                  Controller = "MyPageNotice",
                  action = "GetMoreManagementNotices"
              });

            //3-8_b	○○さんへのお知らせのもっと見る取得	
            //spolog.jp/mypage/notice/
            context.MapRoute(
              "MyPage_3-8_b",
              "mypage/notice/user/{usernoticecount}/",
              new
              {
                  Controller = "MyPageNotice",
                  action = "GetMoreUserNotices"
              });


            //3-8_c	お知らせ既読取得	
            //spolog.jp/mypage/notice/
            context.MapRoute(
              "MyPage_3-8_c",
              "mypage/notice/alreadyread/{noticeDeliverySubjectId}/",
              new
              {
                  Controller = "MyPageNotice",
                  action = "SetNoticeAlreadyRead"
              });

            //3-9	ユーザー検索	
            //spolog.jp/user_search/
            context.MapRoute(
              "MyPage_3-9",
              "user_search/",
              new
              {
                  Controller = "UserSearch",
                  action = "Index"
              });


            //3-9	ユーザー検索	
            //spolog.jp/user_search/
            context.MapRoute(
              "MyPage_3-9_a",
              "user_search/{keyword}/{usersearchcount}/",
              new
              {
                  Controller = "UserSearch",
                  action = "search"
              });

            //3-10	予想結果	
            //spolog.jp/mypage/today/
            context.MapRoute(
              RouteNameConst.MyPageToday,
              "mypage/today/",
              new
              {
                  Controller = "MyPageRecentExpectedList",
                  action = "Index"
              });
            //3-10_a	予想リスト(PC)	
            //spolog.jp/mypage/today/ShowExpectedList/
            context.MapRoute(
              "MyPage_3-10_a",
              "mypage/today/ShowExpectedListForPC/",
              new
              {
                  Controller = "MyPageRecentExpectedList",
                  action = "ShowExpectedListForPC"
              });
            //3-10_b	予想リスト(SP)	
            //spolog.jp/mypage/today/ShowExpectedList/
            context.MapRoute(
              "MyPage_3-10_b",
              "mypage/today/ShowExpectedList/",
              new
              {
                  Controller = "MyPageRecentExpectedList",
                  action = "ShowExpectedList"
              });
            //3-10_c 予想リスト	キャンセル
            //spolog.jp/mypage/today/expectcancel/{game_id}/
            context.MapRoute(
              "MyPage_3-10_c",
              "mypage/today/ExpectCancel/{game_id}/",
              new
              {
                  Controller = "MyPageRecentExpectedList",
                  action = "ExpectCancel"
              });
            //3-10_d 予想リスト	ポイント入力
            //spolog.jp/mypage/today/expectpoint/{game_id}/
            context.MapRoute(
              "MyPage_3-10_d",
              "mypage/today/ExpectPoint/{game_id}/",
              new
              {
                  Controller = "MyPageRecentExpectedList",
                  action = "ExpectPoint"
              });

            //Using default for actionlink or action.
            context.MapRoute(
                "MyPage_default",
                "mypage/{controller}/{action}",
                new { action = "Index" }
            );

            //Using default for actionlink or action.
            context.MapRoute(
                "Test",
                "Test/",
                new
                {
                    Controller = "Test",
                    action = "Index"
                }
            );


        }
    }
}
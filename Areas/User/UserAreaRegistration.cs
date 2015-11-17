using System.Web.Mvc;
using Splg.Core.Constant;

namespace Splg.Areas.User
{
    public class UserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            // 4-1 他ユーザページ
            context.MapRoute(
                RouteNameConst.UserTopIndex,
                "user/{memberId}/",
                new
                {
                    Controller = "UserTop",
                    action = "Index"
             }).DataTokens["UseNamespaceFallback"] = false;

            //4-1-1	他ユーザー対象年月取得用
            context.MapRoute(
              RouteNameConst.UserTopChangeYearMonth,
              "user/ChangeYearMonth/{yearmonth}/{memberId}",
              new
              {
                  Controller = "UserTop",
                  action = "ChangeYearMonth"
              });

            // 4-2 他ユーザページ　予想リスト
            context.MapRoute(
                "User_4-2",
                "user/{memberId}/expectedlist/",
                new
                {
                    Controller = "UserExpectedlist",
                    action = "Index"
                }).DataTokens["UseNamespaceFallback"] = false;

            //4-2-1	予想リスト	
            //spolog.jp/mypage/expectedlist/changeyearmonth/{yearmonth}/
            context.MapRoute(
              "User_4-2-1",
              "user/expectedlist/ChangeYearMonth/{yearmonth}/{user_id}",
              new
              {
                  Controller = "UserExpectedList",
                  action = "ChangeYearMonth"
              });

            // 4-3 他ユーザページ　投稿記事
            context.MapRoute(
                "User_4-3",
                "user/{memberId}/article/",
                new
                {
                    Controller = "UserArticle",
                    action = "Index"
                }).DataTokens["UseNamespaceFallback"] = false;

            //4-3_a	投稿記事	
            //spolog.jp/user/article/
            context.MapRoute(
              "User_4-3_a",
              "user/{memberId}/article/{currentCount}/",
              new
              {
                  Controller = "UserArticle",
                  action = "GetMoreArticles"
              });

            // 4-4 他ユーザページ　フォローリスト
            context.MapRoute(
                "User_4-4",
                "user/{memberId}/following",
                new
                {
                    Controller = "UserFollowing",
                    action = "Index"
                }).DataTokens["UseNamespaceFallback"] = false;


            //4-4_a	フォローリスト	
            //spolog.jp/user/following/
            context.MapRoute(
              "User_4-4_a",
              "user/following/{memberId}/{currentCount}/",
              new
              {
                  Controller = "UserFollowing",
                  action = "GetMoreFollowings"
              });

            //4-4_b	フォローリスト ajax follow
            //spolog.jp/user/following/follow
            context.MapRoute(
              "User_4-4_b",
              "user/following/follow/{followingMemberId}/",
              new
              {
                  Controller = "UserFollowing",
                  action = "follow"
              });

            //4-4_c	フォローリスト ajax unfollow	
            //spolog.jp/user/following/unfollow
            context.MapRoute(
              "User_4-4_c",
              "user/following/unfollow/{followingMemberId}/",
              new
              {
                  Controller = "UserFollowing",
                  action = "unfollow"
              });

            // 4-5 他ユーザページ　フォロワーリスト
            context.MapRoute(
                "User_4-5",
                "user/{memberId}/followers",
                new
                {
                    Controller = "UserFollowers",
                    action = "Index"
                }).DataTokens["UseNamespaceFallback"] = false;

            //4-5_a	フォロワーリスト	
            //spolog.jp/user/article/
            context.MapRoute(
              "User_4-5_a",
              "user/followers/{memberId}/{currentCount}/",
              new
              {
                  Controller = "UserFollowers",
                  action = "GetMoreFollowers"
              });

            //4-5_b	フォロワーリスト ajax follow
            //spolog.jp/user/followers/follow
            context.MapRoute(
              "User_4-5_b",
              "user/followers/follow/{followingMemberId}/",
              new
              {
                  Controller = "UserFollowers",
                  action = "follow"
              });

            //4-5_c	フォロワーリスト ajax unfollow	
            //spolog.jp/user/followers/unfollow
            context.MapRoute(
              "User_4-5_c",
              "user/followers/unfollow/{followingMemberId}/",
              new
              {
                  Controller = "UserFollowers",
                  action = "unfollow"
              });
        }
    }
}
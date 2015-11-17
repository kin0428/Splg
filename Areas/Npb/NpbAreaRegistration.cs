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
 * Namespace	: Splg.Areas.Npb
 * Class		: NpbAreaRegistration
 * Developer	: Auto created by project 
 */
#endregion

using System.Web.Mvc;
using Splg.Core.Constant;

namespace Splg.Areas.Npb
{
    public class NpbAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Npb";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //ModifiedDev : CucHTP
            //ModifiedDate : 2015/03/11

            //8-1 プロ野球TOP	
            //spolog.jp/npb/
            context.MapRoute(
              "Npb_8-1",
              Constants.NPB_TOPIC_NAME + "/",
              new
              {
                  Controller = "NpbTop",
                  action = "Index"
              });


            //画面設計書 (日程・結果(プロ野球)画面）
            //8-2 日程・結果	
            //spolog.jp/npb/Schedule
            context.MapRoute(
             "Npb_8-2",
             Constants.NPB_TOPIC_NAME + "/schedule/",
             new
             {
                 Controller = "NpbScheduleResult",
                 action = "Index"
             });  

            //8-5 チーム情報（プロ野球）
            //spolog.jp/npb/teams/
            context.MapRoute(
               "Npb_8-5",
               Constants.NPB_TOPIC_NAME + "/teams/",
               new
               {
                   controller = "NpbTeamInformation",
                   action = "Index"
               }).DataTokens["UseNamespaceFallback"] = false;

            //8-5-1	チームトップ（プロ野球）
            //spolog.jp/npb/teams/(チームID)/
            context.MapRoute(
             "Npb_8-5-1",
             Constants.NPB_TOPIC_NAME + "/teams/{teamID}",
             new
             {
                 controller = "NpbTeamInfoTeamTop",
                 action = "Index",
                 teamID = UrlParameter.Optional
             }).DataTokens["UseNamespaceFallback"] = false;    
       
            //8-5-2	チーム情報　日程・結果（プロ野球）
            //spolog.jp/npb/teams/(チームID)/	schedule/
            context.MapRoute(
             "Npb_8-5-2",
             Constants.NPB_TOPIC_NAME + "/teams/{teamID}/schedule",
             new
             {
                 controller = "NpbTeamInfoDailyResult",
                 action = "Index",
                 teamID = UrlParameter.Optional
             }).DataTokens["UseNamespaceFallback"] = false;   

            //8-5-3	チーム情報　対戦成績（プロ野球）
            //spolog.jp/npb/teams/(チームID)/	stats/
            context.MapRoute(
            "Npb_8-5-3",
            Constants.NPB_TOPIC_NAME + "/teams/{teamID}/stats",
            new
            {
                controller = "NpbTeamInfoConfrontationResult",
                action = "Index",
                teamID = UrlParameter.Optional
            }).DataTokens["UseNamespaceFallback"] = false;   

            //8-5-4	チーム情報　投手（プロ野球）
            //8-5-5	チーム情報　捕手・野手（プロ野球）
            //8-5-6	チーム情報　監督・スタッフ（プロ野球）
            //spolog.jp/npb/teams/(チームID)/	memberlist/(タイプID)/
            context.MapRoute(
            "Npb_8-5-4-5-6",
            Constants.NPB_TOPIC_NAME + "/teams/{teamID}/memberlist/{typeID}/",
            new
            {
               controller = "NpbTeamInfoPlayer",
               action = "Index",
               teamID = UrlParameter.Optional,
               typeID = UrlParameter.Optional
            }).DataTokens["UseNamespaceFallback"] = false;

            //8-5-7	チーム情報　ニュース（プロ野球)
            //spolog.jp/npb/teams/(チームID)/	news/
            context.MapRoute(
                 "Npb_8-5-7",
                 Constants.NPB_TOPIC_NAME + "/teams/{teamId}/news",
                 new { controller = "NpbTeamInfoNews", action = "Index", tmpID = UrlParameter.Optional }
                 ).DataTokens["UseNamespaceFallback"] = false;

            //8-5-8	チーム情報　投手選手詳細（プロ野球）
            //spolog.jp/npb/players/(選手ID)/(タイプID)/
            context.MapRoute(
                "Npb_8-5-8",
                Constants.NPB_TOPIC_NAME + "/players/{teamID}/{playerID}/1",
                new { 
                    controller = "NpbTeamInfoPitcherDetail", 
                    action = "Index", 
                    teamID = UrlParameter.Optional, 
                    playerID = UrlParameter.Optional }
                ).DataTokens["UseNamespaceFallback"] = false;


            //8-5-8_2	チーム情報　投手選手詳細（プロ野球）
            //spolog.jp/npb/players/(選手ID)/(タイプID)/
            context.MapRoute(
                "Npb_8-5-8_2",
                Constants.NPB_TOPIC_NAME + "/players/{playerID}/1/",
                new
                {
                    controller = "NpbTeamInfoPitcherDetail",
                    action = "Show",
                    teamID = UrlParameter.Optional,
                    playerID = UrlParameter.Optional
                }
                ).DataTokens["UseNamespaceFallback"] = false;


            //8-5-9	チーム情報　打者選手詳細（プロ野球）
            //spolog.jp/npb/players/(選手ID)/
            context.MapRoute(
               "Npb_8-5-9",
               Constants.NPB_TOPIC_NAME + "/players/{teamID}/{playerID}/2",
               new
               {
                   controller = "NpbTeamInfoHittingDetail",
                   action = "Index",
                   playerID = UrlParameter.Optional
               }
               ).DataTokens["UseNamespaceFallback"] = false;


            //8-5-9_2	チーム情報　打者選手詳細（プロ野球）
            //spolog.jp/npb/players/(選手ID)/
            context.MapRoute(
               "Npb_8-5-9_2",
               Constants.NPB_TOPIC_NAME + "/players/{playerID}/2/",
               new
               {
                   controller = "NpbTeamInfoHittingDetail",
                   action = "Show",
                   playerID = UrlParameter.Optional
               }
               ).DataTokens["UseNamespaceFallback"] = false;



            //8-6 試合情報（プロ野球）／試合前・試合中・試合後
            //spolog.jp/npb/game/(試合ID)/
            context.MapRoute(
                RouteNameConst.NpbGameDetail,
                Constants.NPB_TOPIC_NAME + "/game/{gameID}/",
                new { controller = "NpbGameInformation", action = "Index", gameID = UrlParameter.Optional }
                ).DataTokens["UseNamespaceFallback"] = false;

            //8-7	ニュースリスト（プロ野球）
            //spolog.jp/npb/News/
            context.MapRoute(
                 "Npb_8-7",
                 Constants.NPB_TOPIC_NAME + "/news/",
                 new { controller = "NpbNewsList", action = "Index" }
                 ).DataTokens["UseNamespaceFallback"] = false;
            //8-3	順位表（プロ野球）
            //spolog.jp/npb/Standings/
            context.MapRoute(
                 "Npb_8-3",
                 Constants.NPB_TOPIC_NAME + "/standings/",
                 new { controller = "NpbOrder", action = "Index" }
                 ).DataTokens["UseNamespaceFallback"] = false;

            //8-4	順位表（プロ野球）
            //spolog.jp/npb/Standings/
            context.MapRoute(
                 "Npb_8-4",
                 Constants.NPB_TOPIC_NAME + "/stats/",
                 new { controller = "NpbPersonalResult", action = "Index" }
                 ).DataTokens["UseNamespaceFallback"] = false;
            //Using default for actionlink or action.
            context.MapRoute(
                "Npb_default",
                Constants.NPB_TOPIC_NAME + "/{controller}/{action}",
                new { action = "Index" }
            );
        }
    }
}
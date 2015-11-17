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
 * Namespace	: Splg.Areas.Mlb
 * Class		: MlbAreaRegistration
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

using System.Web.Mvc;
using Splg.Core.Constant;

namespace Splg.Areas.Mlb
{
    public class MlbAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Mlb";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "Mlb_default",
            //    "Mlb/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            //9-1	MLBTOP
            context.MapRoute(
              "Mlb_9-1",
              Constants.MLB_TOPIC_NAME + "/",
              new
              {
                  Controller = "MlbTop",
                  action = "Index"
              });

            //9-2	日程・結果（MLB）
            context.MapRoute(
             "Mlb_9-2",
             Constants.MLB_TOPIC_NAME + "/schedule/",
             new
             {
                 Controller = "MlbScheduleResult",
                 action = "Index",
             });
            //9-2_a 日程・結果（MLB）
            context.MapRoute(
              "Mlb_9-2_a",
              Constants.MLB_TOPIC_NAME + "/mlbtop/showgameinfo/",
              new
              {
                  Controller = "MlbTop",
                  action = "ShowGameInfo",
                  teamID = UrlParameter.Optional
              });

            //9-3	順位表（MLB）
            context.MapRoute(
                 "Mlb_9-3",
                 Constants.MLB_TOPIC_NAME + "/standings/",
                 new {
                     controller = "MlbOrder", 
                     action = "Index" 
                 }).DataTokens["UseNamespaceFallback"] = false;

            //9-5	チーム情報（MLB）
            context.MapRoute(
               "Mlb_9-5",
               Constants.MLB_TOPIC_NAME + "/teams/",
               new
               {
                   controller = "MlbTeamInformation",
                   action = "Index"
               }).DataTokens["UseNamespaceFallback"] = false;
            
            //9-5-1	チーム情報　チームトップ（MLB）
            context.MapRoute(
             "Mlb_9-5-1",
             Constants.MLB_TOPIC_NAME + "/teams/{teamID}",
             new
             {
                 controller = "MlbTeamInfoTeamTop",
                 action = "Index",
                 teamID = UrlParameter.Optional
             }).DataTokens["UseNamespaceFallback"] = false;  
            
            //9-5-2	チーム情報　日程・結果（MLB）
            context.MapRoute(
             "Mlb_9-5-2",
             Constants.MLB_TOPIC_NAME + "/teams/{teamID}/schedule",
             new
             {
                 controller = "MlbTeamInfoDailyResult",
                 action = "Index",
                 teamID = UrlParameter.Optional
             }).DataTokens["UseNamespaceFallback"] = false;
            
            //9-5-3	チーム情報　対戦成績（MLB）
            context.MapRoute(
            "Mlb_9-5-3",
            Constants.MLB_TOPIC_NAME + "/teams/{teamID}/stats",
            new
            {
                controller = "MlbTeamInfoConfrontationResult",
                action = "Index",
                teamID = UrlParameter.Optional
            }).DataTokens["UseNamespaceFallback"] = false;
            
            //9-5-6	チーム情報　ニュース（MLB）
            context.MapRoute(
                 "Mlb_9-5-6",
                 Constants.MLB_TOPIC_NAME + "/teams/{teamId}/news",
                 new { controller = "MlbTeamInfoNews", action = "Index", tmpID = UrlParameter.Optional }
                 ).DataTokens["UseNamespaceFallback"] = false;

            //9-7	試合情報（MLB）／試合前・試合中・試合後
            context.MapRoute(
                RouteNameConst.MlbGameDetail,
                Constants.MLB_TOPIC_NAME + "/game/{gameID}/",
                new { controller = "MlbGameInformation", action = "Index", gameID = UrlParameter.Optional  }
                ).DataTokens["UseNamespaceFallback"] = false;

            //9-8	ニュースリスト（MLB）
            context.MapRoute(
                 "Mlb_9-8",
                 Constants.MLB_TOPIC_NAME + "/news/",
                 new { controller = "MlbNewsList", action = "Index" }
                 ).DataTokens["UseNamespaceFallback"] = false;

            //Using default for actionlink or action.
            context.MapRoute(
                "Mlb_default",
                Constants.MLB_TOPIC_NAME + "/{controller}/{action}",
                new { action = "Index" }
            );
        
        }
    }
}
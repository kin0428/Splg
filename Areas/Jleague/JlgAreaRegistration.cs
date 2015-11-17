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
* Namespace    : Splg.Areas.Jleague
* Class        : JlgAreaRegistration
* Developer    : KietNM
* Updated date : 2015-03-25
*/
#endregion

using System.Web.Mvc;
using Splg.Core.Constant;

namespace Splg.Areas.Jleague
{
    public class JlgAreaRegistration : AreaRegistration
    {

        public override string AreaName
        {
            get
            {
                return "Jleague";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
            "Jleague_10_1",
            Constants.JLG_TOPIC_NAME + "/",
            new
            {
                Controller = "JlgTop",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_2",
            Constants.JLG_TOPIC_NAME + "/j1/",
            new
            {
                Controller = "JlgTop",
                action = "JTop"
            });
            // 日程・結果（J1)
            context.MapRoute(
            "Jleague_10_2_1",
            Constants.JLG_TOPIC_NAME + "/j1/schedule/",
            new
            {
                Controller = "JlgScheduleResult",
                action = "Index"
            });
            //10-2_a 日程・結果（JlgTop）
            context.MapRoute(
              "Jlg_10-2_a",
              "Jleague/JlgTop/ShowGameInfo/",
              new
              {
                  Controller = "JlgTop",
                  action = "ShowGameInfo",
                  teamID = UrlParameter.Optional
              });

            /// 10-3-2, 10-2-2, 10-4-2 
            /// Same controller JlgJOrder
            /// difference action/view
            ///  - Huynh -
            context.MapRoute(
            "Jleague_10_2_2",
            Constants.JLG_TOPIC_NAME + "/j1/standings",
            new
            {
                Controller = "JlgJOrder",
                action = "Index"
            });

            //KietNM: 2015-04-13
            //画面設計書 (戦績表(J1)画面）
            //10.2.3
            context.MapRoute(
            "Jleague_10_2_3",
            Constants.JLG_TOPIC_NAME + "/j1/matrix",
            new
            {
                Controller = "JlgResult",
                action = "Index"
            });

            //KietNM: 2015-04-13
            //画面設計書 (個人成績(J1)画面）
            //10.2.4
            context.MapRoute(
            "Jleague_10_2_4",
            Constants.JLG_TOPIC_NAME + "/j1/stats",
            new
            {
                Controller = "JlgPersonalAchieve",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_2_5",
            Constants.JLG_TOPIC_NAME + "/j1/teams",
            new
            {
                Controller = "JlgTeamInfo",
                action = "Index"
            });

            //チーム情報　チームトップ（J1）
            context.MapRoute(
            "Jleague_10_2_5_1",
            Constants.JLG_TOPIC_NAME + "/j1/teams/{teamId}/",
            new
            {
                Controller = "JlgTeamInfoTop",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_2_5_2",
            Constants.JLG_TOPIC_NAME + "/j1/teams/{teamId}/schedule",
            new
            {
                Controller = "JlgTeamInfoScheduleResult",
                action = "Index"
            });

            //チーム情報　対戦成績（J1）
            context.MapRoute(
            "Jleague_10_2_5_3",
            Constants.JLG_TOPIC_NAME + "/j1/teams/{teamId}/stats",
            new
            {
                Controller = "JlgTeamInfoConfrontationResult",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_2_5_4",
            Constants.JLG_TOPIC_NAME + "/j1/teams/{teamId}/memberlist",
            new
            {
                Controller = "JlgTeamInfoPlayer",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_2_5_5",
            Constants.JLG_TOPIC_NAME + "/j1/teams/{teamID}/news/",
            new
            {
                Controller = "JlgNews",
                action = "TeamInfoNews"
            });

            context.MapRoute(
            "Jleague_10_2_5_6",
            Constants.JLG_TOPIC_NAME + "/players/{playerId}/",
            new
            {
                Controller = "JlgTeamInfoPlayerDetail",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_2_6",
            Constants.JLG_TOPIC_NAME + "/j1/news",
            new
            {
                Controller = "JlgNews",
                action = "JlgNews"
            });

            context.MapRoute(
            "Jleague_10_3",
            Constants.JLG_TOPIC_NAME + "/j2/",
            new
            {
                Controller = "JlgTop",
                action = "JTop"
            });
            // 日程・結果（J2)
            context.MapRoute(
            "Jleague_10_3_1",
            Constants.JLG_TOPIC_NAME + "/j2/schedule/",
            new
            {
                Controller = "JlgScheduleResult",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_3_2",
            Constants.JLG_TOPIC_NAME + "/j2/standings",
            new
            {
                Controller = "JlgJOrder",
                action = "Index"
            });

            //KietNM: 2015-04-13
            //画面設計書 (戦績表(J1)画面）
            //10.3.3
            context.MapRoute(
            "Jleague_10_3_3",
            Constants.JLG_TOPIC_NAME + "/j2/matrix",
            new
            {
                Controller = "JlgResult",
                action = "Index"
            });

            //KietNM: 2015-04-13
            //画面設計書 (個人成績(J1)画面）
            //10.3.4
            context.MapRoute(
            "Jleague_10_3_4",
            Constants.JLG_TOPIC_NAME + "/j2/stats",
            new
            {
                Controller = "JlgPersonalAchieve",
                action = "Index"
            });
            context.MapRoute(
            "Jleague_10_3_5",
            Constants.JLG_TOPIC_NAME + "/j2/teams",
            new
            {
                Controller = "JlgTeamInfo",
                action = "Index"
            });

            //チーム情報　チームトップ（J2）
            context.MapRoute(
            "Jleague_10_3_5_1",
            Constants.JLG_TOPIC_NAME + "/j2/teams/{teamId}/",
            new
            {
                Controller = "JlgTeamInfoTop",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_3_5_2",
            Constants.JLG_TOPIC_NAME + "/j2/teams/{teamId}/schedule",
            new
            {
                Controller = "JlgTeamInfoScheduleResult",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_3_5_3",
            Constants.JLG_TOPIC_NAME + "/j2/teams/{teamId}/stats/",
            new
            {
                Controller = "JlgTeamInfoConfrontationResult",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_3_5_4",
            Constants.JLG_TOPIC_NAME + "/j2/teams/{teamId}/memberlist/",
            new
            {
                Controller = "JlgTeamInfoPlayer",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_3_5_6",
            Constants.JLG_TOPIC_NAME + "/players/{playerId}/",
            new
            {
                Controller = "JlgTeamInfoPlayerDetail",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_3_5_5",
            Constants.JLG_TOPIC_NAME + "/j2/teams/{teamID}/news",
            new
            {
                Controller = "JlgNews",
                action = "TeamInfoNews"
            });

            context.MapRoute(
            "Jleague_10_3_6",
            Constants.JLG_TOPIC_NAME + "/j2/news",
            new
            {
                Controller = "JlgNews",
                action = "JlgNews"
            });

            context.MapRoute(
            "Jleague_10_4",
            Constants.JLG_TOPIC_NAME + "/jleaguecup/",
            new
            {
                Controller = "JlgTop",
                action = "NabiscoTop"
            });

            // 日程・結果（ナビスコカップ)
            context.MapRoute(
            "Jleague_10_4_1",
            Constants.JLG_TOPIC_NAME + "/jleaguecup/schedule/",
            new
            {
                Controller = "JlgScheduleResult",
                action = "Index"
            });

            context.MapRoute(
            "Jleague_10_4_2",
            Constants.JLG_TOPIC_NAME + "/jleaguecup/standings",
            new
            {
                Controller = "JlgJOrder",
                action = "JlgNabiscoOrder"
            });

            context.MapRoute(
            "Jleague_10_4_4",
            Constants.JLG_TOPIC_NAME + "/jleaguecup/news",
            new
            {
                Controller = "JlgNews",
                action = "JlgNabiscoNews"
            });

            context.MapRoute(
            RouteNameConst.JlgGameDetail,
            Constants.JLG_TOPIC_NAME + "/game/{gameId}/",
            new
            {
                Controller = "JlgGameInformation",
                action = "Index"
            });


            /// 10-2-5-5, 10-2-6, 10-3-5-5, 10-3-6, 10-4-4, 10-6
            /// Same controller JlgNews
            /// difference action/view
            ///  - Huynh -
            context.MapRoute(
            "Jleague_10_6",
            Constants.JLG_TOPIC_NAME + "/news",
            new
            {
                Controller = "JlgNews",
                action = "Index"
            });


            //Using default for actionlink or action.
            context.MapRoute(
            "Jleague_default",
            Constants.JLG_TOPIC_NAME + "/{controller}/{action}",
            new { action = "Index" }
            );
        }
    }
}
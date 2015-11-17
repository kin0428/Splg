using System.Web.Mvc;
using Splg.Core.Constant;

namespace Splg.Areas.Prize
{
    public class PrizeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Prize";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //6-1	懸賞TOP	
            //spolog.jp/Prize/
            context.MapRoute(
              RouteNameConst.PrizeTop,
              "prize/",
              new
              {
                  Controller = "PrizeTop",
                  action = "Index"
              });

            //6-2	懸賞　賞品詳細
            //spolog.jp/prize/(YYYYMM)/(賞品ID)/
            context.MapRoute(
              RouteNameConst.PrizeDetal,
              "prize/{targetYearMonth}/{rallyGoodsId}/",
              new
              {
                  Controller = "PrizeGood",
                  action = "Index"
              });

            //6-2 Util1	懸賞　現在の所持ポイントの取得処理
            //spolog.jp/prize/(YYYYMM)/(賞品ID)/
            context.MapRoute(
              "Prize_6-2_Util_1",
              "prize/GetPossessionPoints/",
              new
              {
                  Controller = "PrizeGood",
                  action = "GetPossessionPoints"
              });

            //6-2 Util2	所持ポイントを使用して景品を申し込む処理
            //spolog.jp/prize/(YYYYMM)/(賞品ID)/
            context.MapRoute(
              "Prize_6-2_Util_2",
              "prize/BuyGood/{rallyGoodID}/",
              new
              {
                  Controller = "PrizeGood",
                  action = "BuyGood"
              });

            //6-2 Util3	所持ポイントを使用して抽選に応募する処理
            //spolog.jp/prize/(YYYYMM)/(賞品ID)/
            context.MapRoute(
              "Prize_6-2_Util_3",
              "prize/DrawGood/{rallyGoodID}",
              new
              {
                  Controller = "PrizeGood",
                  action = "DrawGood"
              });

            //6-2 Util4	応募口数入力チェック処理
            //spolog.jp/prize/(YYYYMM)/(賞品ID)/
            context.MapRoute(
              "Prize_6-2_Util_4",
              "prize/IsEntry/{rallyGoodID}/{entryCount}/",
              new
              {
                  Controller = "PrizeGood",
                  action = "IsEntry"
              });

            //6-2 Util5	応募処理
            //spolog.jp/prize/(YYYYMM)/(賞品ID)/
            context.MapRoute(
              "Prize_6-2_Util_5",
              "prize/EntryPrize/{rallyGoodID}/{entryCount}/{entryMethod}/",
              new
              {
                  Controller = "PrizeGood",
                  action = "EntryPrize"
              });
            
            //6-3	懸賞　大会詳細
            //spolog.jp/prize/(YYYYMM)/	
            //context.MapRoute(
            //  "Prize_6-3",
            //  "prize/{YYYYMM}/",
            //  new
            //  {
            //      Controller = "PrizeRally",
            //      action = "Index"
            //  });

            //6-4	懸賞　大会ポイントランキング
            //spolog.jp/Prize/rally
            //context.MapRoute(
            //  "Prize_6-4",
            //  "prize/ranking/",
            //  new
            //  {
            //      Controller = "PrizeRanking",
            //      action = "Index"
            //  });

            context.MapRoute(
                "Prize_default",
                "Prize/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
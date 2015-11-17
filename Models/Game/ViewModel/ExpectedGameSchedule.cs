using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;
using Splg.Core.Constant;

namespace Splg.Models.Game.ViewModel
{
    public class ExpectedGameSchedule
    {
        /// <summary>
        /// ステータス
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 入力ポイント数
        /// </summary>
        public int InputPoint { get; set; }
        
        /// <summary>
        /// 試合情報ビューモデル
        /// </summary>
        public GameInfoViewModel GameInfoViewModel {get;set;}

        /// <summary>
        /// 試合カードタイトル
        /// </summary>
        public string GameCardTitle { get; set; }
        
        /// <summary>
        /// スポーツ名
        /// </summary>
        public string SportsName { get; set; }

        /// <summary>
        /// 試合日付時間
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsDateAndDayOfWeekWithJapaneseSeparator)]
        public DateTime GameDate { get; set; }

        /// <summary>
        /// スポーツID
        /// </summary>
        public int SportsId { get; set; }

        /// <summary>
        /// オッズ
        /// </summary>
        public decimal Odds { get; set; }

        /// <summary>
        /// 予想ポイント
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]
        public int ExpectPoint { get; set; }

        /// <summary>
        /// 予想ポイントID
        /// </summary>
        public long ExpectPointID { get; set; }

        /// <summary>
        /// ポイントID
        /// </summary>
        public long PointId { get; set; }

        public DateTime BetStartDate { get; set; }

        /// <summary>
        /// 試合情報ルート名
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 予想表示アラート
        /// </summary>
        public string BetTargetAlert { get; set; }

        /// <summary>
        /// 自動生成Div用Id
        /// </summary>
        public string DivId { get; set; }

        /// <summary>
        /// 的中ポイント
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]
        public decimal GetPoint
        {
            get 
            {
                return Math.Round(ExpectPoint * Odds);
            }
        }

        /// <summary>
        /// 予想可能
        /// </summary>
        public bool IsExpectable
        {
            get
            {
                return (Status == 2 || Status == 3);
            }
        }

        /// <summary>
        /// 試合中
        /// </summary>
        public bool IsOnAir
        { 
            get
            {
                return (Status >= 6 && Status <= 9);
            }
        }

        /// <summary>
        /// 試合カードレンダリング用部分ビュー名称
        /// </summary>
        public string PartialViewName
        {
            get
            {
                if (IsOnAir)
                {
                    //試合中用部分ビュー
                    return "ExpextedGameCardAtDuring";
                }
                else
                {
                    //試合前用部分ニュー　Todo:Bet可:不可について切り離し
                    return "ExpextedGameCardAtBefore";
                
                }
            
            }

        }

    }
}
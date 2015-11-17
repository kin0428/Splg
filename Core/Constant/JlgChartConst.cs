using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Constant
{
    public class JlgChartConst
    {
        /// <summary>
        /// 得失点指標ラベル（PK）
        /// </summary>
        public static readonly string GoalIndexAtPkLabel = "PK";

        /// <summary>
        /// セットプレー直接
        /// </summary>
        public static readonly string GoalIndexAtSetPlayDirectlyLabel = "セットプレー直接";

        /// <summary>
        /// セットプレーから
        /// </summary>
        public static readonly string GoalIndexAtSetPlayLabel = "セットプレーから";

        /// <summary>
        /// クロスから
        /// </summary>
        public static readonly string GoalIndexAtCrossLabel = "クロスから";

        /// <summary>
        /// スルーパスから
        /// </summary>
        public static readonly string GoalIndexAtThroughPassLabel = "スルーパスから";

        /// <summary>
        /// ショートパスから
        /// </summary>
        public static readonly string GoalIndexAtShortPassLabel = "ショートパスから";

        /// <summary>
        /// ロングパスから
        /// </summary>
        public static readonly string GoalIndexAtLongPassLabel = "ロングパスから";

        /// <summary>
        /// ドリブルから
        /// </summary>
        public static readonly string GoalIndexAtDribbleLabel = "ドリブルから";

        /// <summary>
        /// こぼれ球から
        /// </summary>
        public static readonly string GoalIndexAtLooseBallLabel = "こぼれ球から";

        /// <summary>
        /// その他
        /// </summary>
        public static readonly string GoalIndexAtOtherLabel = "その他";

        /// <summary>
        /// 得失点時間帯（前半）
        /// </summary>
        public static readonly string GoalTimezoneAtFirstLabel = "前半";

        /// <summary>
        /// 得失点時間帯（後半）
        /// </summary>
        public static readonly string GoalTimezoneAtSecondLabel = "後半";

        /// <summary>
        /// パス成功率（パス成功）
        /// </summary>
        public static readonly string PassSucceedAtSuccessLabel = "パス成功";

        /// <summary>
        /// パス成功率（パス失敗）
        /// </summary>
        public static readonly string PassSucceedAtFailedLabel = "パス失敗";

        /// <summary>
        /// パス種類（ショートパス）
        /// </summary>
        public static readonly string PassPatternAtShortPassLabel = "ショートパス";

        /// <summary>
        /// パス種類（ミドルパス）
        /// </summary>
        public static readonly string PassPatternAtMiddlePassLabel = "ミドルパス";

        /// <summary>
        /// パス種類（ロングパス）
        /// </summary>
        public static readonly string PassPatternAtLongPassLabel = "ロングパス";

        

        /// <summary>
        /// 時間帯区分
        /// </summary>
        public enum TimeZoneDivision
        {
            /// <summary>
            /// 前半
            /// </summary>
            First = 1,

            /// <summary>
            /// 後半
            /// </summary>
            Second = 2,
        }

        /// <summary>
        /// チャートCSSclass名
        /// </summary>
        public enum ChartCssClassName
        {
            /// <summary>
            /// 円グラフ用Cssclass
            /// </summary>
            pie_chart,

            /// <summary>
            /// フォーメーション用Cssclass
            /// </summary>
            formation_chart
        }
        
                // Bubble
 
        /// <summary>
        ///バブルの大きさ算出用の係数
        /// </summary>
        public static readonly Decimal CoefficientOfRadius = 4;

        /// <summary>
        /// スマホ用の半径算出用Value（スマホ版は円の大きさ固定のため）
        /// </summary>
        public static readonly int ValueForMobile = 2;

        // Marker
   
        /// <summary>
        ///フォーメーション用基本半径 
        /// </summary>
        public static readonly Decimal FormationBaseRadius = 5;
        
        /// <summary>
        /// フォーメーションのマーカー用フォントサイズ 
        /// </summary>
        public static readonly Decimal FormationFontSize = 9;

        // XAxis
        
        /// <summary>
        /// X軸最小値（フォーメーション）
        /// </summary>
        public static readonly int FormationXAxisMinValue = -110;

        /// <summary>
        /// X軸最大値（フォーメーション） 
        /// </summary>
        public static readonly int FormationXAxisMaxValue = 110;

        // YAxis

        /// <summary>
        /// Y軸最小値（フォーメーション） 
        /// </summary>
        public static readonly int FormationYAxisMinValue = -170;
        
        /// <summary>
        /// Y軸最大値（フォーメーション）
        /// </summary>
        public static readonly int FormationYAxisMaxValue = 170;

        /// <summary>
        /// グリッド背景画像アドレス（フォーメーション）
        /// </summary>
        public static readonly string FormationGridBackGroundImageAddress = "/Mobile/Content/img/jleague/img_formation_default_2.png";

        /// <summary>
        /// 攻撃力表示用の基準値（ボール何個出すかを算出するための母数）
        /// </summary>
        public static readonly int AttackCBPScale = 10;
        /// <summary>
        /// 守備力表示用の基準値（ボール何個出すかを算出するための母数）
        /// </summary>
        public static readonly int DefenseCBPScale = 10;

        /// <summary>
        /// 攻撃力表示用のボールURL
        /// </summary>
        public static readonly string AttackImageUrl = "/Mobile/Content/img/jleague/mk_socstpower_of.png";

        /// <summary>
        /// 守備力表示用のボールURL
        /// </summary>
        public static readonly string DefenseImageUrl = "/Mobile/Content/img/jleague/mk_socstpower_df.png";

         /// <summary>
        /// フォーメーションのid
        /// </summary>
        public static readonly string FormationContainerID = "Formation";
        
        /// <summary>
        /// 前節のスターティングメンバーの「前節の」って文字
        /// </summary>
        public static readonly string PreviousMember = "前節の";
        
        /// <summary>
        /// 予想スターティングメンバーの「予想」って文字
        /// </summary>
        public static readonly string ForecastMember = "予想";

        /// <summary>
        /// フォーメーションのバブルの色(ホーム）
        /// </summary>
        public static readonly string HomeFormationColor = "#FA8072";
        /// <summary>
        /// フォーメーションのバブルの色（アウェー）
        /// </summary>
        public static readonly string AwayFormationColor = "#00BFFF";
        /// <summary>
        /// デフォルトカラー(黒)
        /// </summary>
        public static readonly string DefaultColor = "#000000";
    }
}
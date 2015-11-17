using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;

namespace Splg.Models.PointInfo.InfoModel
{
    public class PointInfoModel
    {
        #region バッチ処理済のポイント情報

        int pointRank;

        /// <summary>
        /// バッチ処理済のポイントランク
        /// </summary>
        public int PointRank
        {
            get { return pointRank; }
            set { pointRank = value; }
        }

        #endregion

        #region オンラインのポイント情報

        //TODO long に修正
        public int PointID { get; set; }

        /// <summary>
        /// オンラインの原資ポイント
        /// </summary>
        public int FundsPoint { get; set; }

        /// <summary>
        /// オンラインの所持ポイント
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]
        public int PossesionPoint { get; set; }

        /// <summary>
        /// 精算済みポイント
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]
        public int PayOffPoints { get; set; }

        /// <summary>
        /// オンラインの応募可能ポイント
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]
        public int AvailablePoint
        {
            get
            {
                var availablePoint = PossesionPoint - Constants.ExcludedPoint;

                return (availablePoint < 0) ? 0 : availablePoint;

            }
        }

        /// <summary>
        /// フォーマットされたオンラインの精算済みポイント
        /// </summary>
        public string FormattedPayOffPoints
        {
            get
            {
                return PayOffPoints.ToString("#,0");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime BetStartDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BetEndDate { get; set; }

        #endregion

        #region 予想

        public Nullable<System.DateTime> LastExpectedPointDate { get; set; }

        /// <summary>
        /// 最終予想日時
        /// </summary>
        public string FormattedLastExpectedPointDate
        {
            get
            {
                string result = null;
                if (LastExpectedPointDate != null)
                    result = Utils.FormatDateTime(LastExpectedPointDate.Value);
                return result;
            }
        }

        #endregion
    }
}
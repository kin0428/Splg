using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;
using Splg.Core.Models.Flotr2.Dto.Shared.Properties;

namespace Splg.Core.Models.Flotr2.Dto.FormationChart
{
    public class FormationChartProperties
    {
        #region プレイヤーの情報（位置、名前、背番号）
        /// <summary>
        /// ホームチームポジションプロパティー群
        /// </summary>
        public ChartProperties HomePositionProperties { get; set; }

        /// <summary>
        /// アウェーチームポジションプロパティー群
        /// </summary>
        public ChartProperties AwayPositionProperties { get; set; }

        /// <summary>
        /// ホームチームプレイヤー名プロパティー群
        /// </summary>
        public ChartProperties HomePlayerNameProperties { get; set; }

        /// <summary>
        /// ホームチームプレイヤー背番号プロパティー群
        /// </summary>
        public ChartProperties HomePlayerNoProperties { get; set; }

        /// <summary>
        /// アウェーチームプレイヤー名プロパティー群
        /// </summary>
        public ChartProperties AwayPlayerNameProperties { get; set; }

        /// <summary>
        /// アウェーチームプレイヤー背番号プロパティー群
        /// </summary>
        public ChartProperties AwayPlayerNoProperties { get; set; }
        #endregion

        #region 共通プロパティ（X軸の最大、最小値とか、バックグラウンドイメージとか）
        /// <summary>
        /// 共通のグラフプロパティー群
        /// </summary>
        public ChartProperties SharedProperties { get; set; }

        #endregion
    }
}
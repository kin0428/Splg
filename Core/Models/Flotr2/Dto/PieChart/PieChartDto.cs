using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;
using Splg.Core.Models.Flotr2.Dto.Shared.Properties;
using Splg.Core.Constant;

namespace Splg.Core.Models.Flotr2.Dto.PieChart
{
    public class PieChartDto : IChartDto
    {
        public string FunctionName { get; set; }

        /// <summary>
        /// 円グラフコンテナー
        /// </summary>
        public PieChartContainer PieChartContainer { get; set; }

        /// <summary>
        /// 円グラフプロパティー群
        /// </summary>
        public ChartProperties PieChartProperties { get; set; }

        #region Constructor
        public PieChartDto()
        {
            PieChartContainer = new PieChartContainer();

            PieChartProperties = new ChartProperties();
        }
        #endregion
    }
}
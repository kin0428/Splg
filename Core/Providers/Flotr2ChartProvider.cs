using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;
using Splg.Core.Constant;
using Splg.Core.Services;

namespace Splg.Core.Providers
{
    public class Flotr2ChartProvider
    {
        #region Constructor
        public Flotr2ChartProvider()
        {

        }
        #endregion

        #region method
        public ChartResult GetChartResult(IChartDto chartDto, Flotr2Const.ChartType Type)
        {
            return GetChartService(Type).GetChartResult(chartDto);
        }

        /// <summary>
        /// ロギングサービス取得
        /// </summary>
        private IFlotr2ChartService GetChartService(Flotr2Const.ChartType Type)
        {
            switch (Type)
            {
                case Flotr2Const.ChartType.Pie:
                    return new PieChartService();
                case Flotr2Const.ChartType.Bar:
                    return new BarChartService();
                case Flotr2Const.ChartType.Bubble:
                    return new BubbleChartService();
                case Flotr2Const.ChartType.Formation:
                    return new FormationChartService();
                
                default:
                    return null;
            }
        }
        #endregion

    }
}
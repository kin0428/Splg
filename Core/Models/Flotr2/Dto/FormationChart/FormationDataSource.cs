using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Splg.Core.Models.Flotr2.Dto.Shared;

namespace Splg.Core.Models.Flotr2.Dto.FormationChart
{
    public class FormationDataSource : IChartDataSource
    {
        /// <summary>
        /// X座標
        /// </summary>
        public decimal X { get; set; }

        /// <summary>
        /// Y座標
        /// </summary>
        public decimal Y { get; set; }

        /// <summary>
        /// ラベリング
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// value(選手の攻撃力とか防御力とか)
        /// </summary>
        public int Value { get; set; }
    }

   
}

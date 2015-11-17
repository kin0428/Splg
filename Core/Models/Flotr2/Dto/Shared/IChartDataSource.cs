using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splg.Core.Models.Flotr2.Dto.Shared
{
    public interface IChartDataSource
    {
        /// <summary>
        /// X座標
        /// </summary>
        decimal X { get; set; }

        /// <summary>
        /// Y座標
        /// </summary>
        decimal Y { get; set; }

        /// <summary>
        /// ラベリング
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// 値
        /// </summary>
        int Value { get; set; }
    }
}

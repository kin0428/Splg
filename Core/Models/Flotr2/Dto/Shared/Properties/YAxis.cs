using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Models.Flotr2.Dto.Shared.Properties
{
    public class YAxis : IAxis
    {
        /// <summary>
        /// ラベリング表示可否
        /// </summary>
        public bool IsVisibleLabels { get; set; }

        /// <summary>
        /// 最小値（軸上）
        /// </summary>
        public decimal MinValue { get; set; }

        /// <summary>
        /// 最大値（軸上）
        /// </summary>
        public decimal MaxValue { get; set; }
    }
}
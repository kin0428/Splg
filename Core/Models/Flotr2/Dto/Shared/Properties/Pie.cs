using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Models.Flotr2.Dto.Shared.Properties
{
    public class Pie
    {
        /// <summary>
        /// 表示可否
        /// </summary>
        public bool IsVisible{get;set;}

        /// <summary>
        /// 円噴出量
        /// </summary>
        public decimal Explode { get; set; }

        /// <summary>
        /// 円表示比率（高さ基準）
        /// </summary>
        public decimal SizeRatio{get;set;}

        /// <summary>
        /// 円不透明度
        /// </summary>
        public decimal FillOpacity { get; set; }

    }
}
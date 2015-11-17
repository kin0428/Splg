using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Constant;

namespace Splg.Core.Models.Flotr2.Dto.Shared.Properties
{
    public class Legend
    {
        /// <summary>
        /// ポジション
        /// </summary>
        public Flotr2Const.LegendPosition position{get;set;}

        /// <summary>
        /// 背景色
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// 枠色
        /// </summary>
        public string BorderColor { get; set;}

    }
}
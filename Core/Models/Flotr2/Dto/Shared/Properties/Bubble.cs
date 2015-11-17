using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Models.Flotr2.Dto.Shared.Properties
{
    public class Bubble
    {
        /// <summary>
        /// 表示の可否
        /// </summary>
        public bool IsVisible{ get; set; }
        
        /// <summary>
        /// 円の大きさ
        /// </summary>
        public Decimal BaseRadius { get; set; }


    }
}
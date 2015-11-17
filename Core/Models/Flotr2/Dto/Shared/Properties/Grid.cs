using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Models.Flotr2.Dto.Shared.Properties
{
    public class Grid
    {
        /// <summary>
        /// 垂直ガイド線表示可否
        /// </summary>
        public bool IsVisibleVerticalLines{ get; set; }
        
        /// <summary>
        /// 水平ガイド線表示可否
        /// </summary>
        public bool IsVisibleHorizontalLines{ get; set; }

        /// <summary>
        /// 背景画像アドレス
        /// </summary>
        public string BackGroundImageAddress{get;set;}

        /// <summary>
        /// 外枠の幅
        /// </summary>
        public int GridOutlineWidth { get; set; }

    }
}
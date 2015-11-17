using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Models.Flotr2.Dto.Shared.Properties
{
    public class Marker
    {
        /// <summary>
        /// 表示の可否
        /// </summary>
        public bool IsVisible{ get; set; }
        
        /// <sumary>
        /// 相対位置設定
        /// </summary>
        public bool IsRelative { get; set; }

        /// <summary>
        /// フォントサイズ
        /// </summary>
        public Decimal FontSize { get; set; }

        /// <summary>
        /// ラベルフォーマット（ファンクション仕込める）
        /// </summary>
        public string LabelFormatter { get; set; }

    }
}
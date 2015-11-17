using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splg.Core.Models.Flotr2.Dto.Shared.Properties;
using Splg.Core.Constant;

namespace Splg.Core.Models.Flotr2.Dto.Shared
{
    public class ChartProperties
    {
        public ChartProperties()
        {
            XAxis = new XAxis()
            {
                IsVisibleLabels = false,
                MinValue = 0,
                MaxValue = 0                
            };

            YAxis = new YAxis()
            {
                IsVisibleLabels = false,
                MinValue = 0,
                MaxValue = 0
            };

            Mouse = new Mouse()
            {
                IsTrackable = false,
                TrackFormatter = string.Empty
            };

            Grid = new Grid()
            {
                IsVisibleHorizontalLines = false,
                IsVisibleVerticalLines = false,
                BackGroundImageAddress = string.Empty,
                GridOutlineWidth = 0
            };

            Colors = new List<string>();

            IsHtmlText = false;

            Resolution = 2;

            FontSize = 10.5m;

            ShadowSize = 0.5m;

            Pie = new Pie()
            {
                IsVisible = true,
                Explode = 0,
                SizeRatio = 0.9m,
                FillOpacity = 0.8m
            };

            Legend = new Legend()
            {
                position = Flotr2Const.LegendPosition.se,
                BackgroundColor = "#FFF",
                BorderColor = "#AAA"
            };

            Bubble = new Bubble() 
            { 
                IsVisible = false, 
                BaseRadius = 0m 
            };
            
            Marker = new Marker() 
            { 
                IsVisible = false, 
                FontSize = 10, 
                IsRelative = false, 
                LabelFormatter = string.Empty 
            };
        }

        /// <summary>
        /// X軸関連
        /// </summary>
        public XAxis XAxis { get; set; }

        /// <summary>
        /// Y軸関連
        /// </summary>
        public YAxis YAxis { get; set; }

        /// <summary>
        /// マウス関連
        /// </summary>
        public Mouse Mouse { get; set; }

        /// <summary>
        /// グリッド関連
        /// </summary>
        public Grid Grid { get; set; }

        /// <summary>
        /// 色（Container内のデータ件数と一致する必要があります。）
        /// </summary>
        public List<string> Colors { get; set; }

        /// <summary>
        /// Htmlテキストとして表示するか
        /// </summary>
        public bool IsHtmlText { get; set; }

        public int Resolution{get;set;}

        /// <summary>
        /// フォントサイズ
        /// </summary>
        public decimal FontSize { get; set; }

        /// <summary>
        /// 影サイズ
        /// </summary>
        public decimal ShadowSize { get; set; }

        /// <summary>
        /// 円チャート関連
        /// </summary>
        public Pie Pie { get; set; }

        /// <summary>
        /// 凡例
        /// </summary>
        public Legend Legend { get; set; }

        /// <summary>
        /// バブルチャート関連
        /// </summary>
        public Bubble Bubble { get; set; }

        /// <summary>
        /// マーカー関連
        /// </summary>
        public Marker Marker { get; set; }

    }
}

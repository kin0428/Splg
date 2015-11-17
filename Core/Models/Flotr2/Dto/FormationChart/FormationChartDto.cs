using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;

namespace Splg.Core.Models.Flotr2.Dto.FormationChart
{
    public class FormationChartDto : IChartDto
    {
        public FormationChartDto()
        {
            FormationChartContainer = new FormationChartContainer();
            FormationChartProperties = new FormationChartProperties();
        }

        /// <summary>
        /// フォーメーションコンテナー
        /// </summary>
        public FormationChartContainer FormationChartContainer { get; set; }

        /// <summary>
        /// フォーメーションプロパティー群
        /// </summary>
        public FormationChartProperties FormationChartProperties { get; set; }

        /// <summary>
        /// スターティングメンバーが前節なのか予想なのか確定したものなのか出しわけするための入れ物
        /// </summary>
        public string StartingTypeForDisplay { get; set; }

    }
}
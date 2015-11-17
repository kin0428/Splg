using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;

namespace Splg.Core.Models.Flotr2.Dto.Shared
{
    public abstract class AbstractChartContainer
    {
        /// <summary>
        /// 表示コンテナId
        /// </summary>
        public string ViewContainerId { get; set; }

        /// <summary>
        /// 表示コンテナクラス名
        /// </summary>
        public string ViewContainerClass { get; set; }


    }
}
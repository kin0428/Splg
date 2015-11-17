using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Models.Flotr2.Dto.Shared
{
    /// <summary>
    /// チャート表示用Dto
    /// </summary>
    public class ChartResult
    {
        /// <summary>
        /// チャート用スクリプト
        /// </summary>
        public string ChartScript { get; set; }

        /// <summary>
        /// チャートコンテナ設定
        /// </summary>
        public ChartContainerSettings ChartContainerSettings { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.Dto
{
    /// <summary>
    /// 試合結果（勝敗カウント用）
    /// </summary>
    public class JlgGameResultCounts
    {
        /// <summary>
        /// 勝利数
        /// </summary>
        public int WinCount { get; set; }

        /// <summary>
        /// 引き分け数
        /// </summary>
        public int DrawCount { get; set; }

        /// <summary>
        /// 敗北数
        /// </summary>
        public int LoseCount { get; set; }



    }
}
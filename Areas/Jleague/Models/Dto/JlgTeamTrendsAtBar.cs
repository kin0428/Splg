using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.Dto
{
    public class JlgTeamTrendsAtBar
    {
        /// <summary>
        /// ホーム勝利数
        /// </summary>
        public int HomeWinCount{get;set;}
            
        /// <summary>
        /// アウェイ勝利数
        /// </summary>
        public int AwayWinCount {get;set;}

        /// <summary>
        /// 平均ポゼッション率
        /// </summary>
        public int PossessionAverage {get;set;}

        /// <summary>
        /// 左クロス数
        /// </summary>
        public int LeftCrossCount{get;set;}

        /// <summary>
        /// 右クロス数
        /// </summary>
        public int RightCrossCount { get; set; }

        /// <summary>
        /// ボール奪取数
        /// </summary>
        public int　InterceptCount {get;set;}

        /// <summary>
        /// 代表経験者数
        /// </summary>
        public int NationalExperiencedCount { get; set; }

        /// <summary>
        /// 平均年齢
        /// </summary>
        public int AgeAverage { get; set; }

        /// <summary>
        /// 平均身長
        /// </summary>
        public int BodyHeightAverage { get; set; }
    }
}
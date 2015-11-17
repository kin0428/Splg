using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.Dto
{
    public class JlgRecentGameResult : IJlgRecentResults
    {
        /// <summary>
        /// 試合日
        /// </summary>
        public int? GameDate{ get; set; }

        /// <summary>
        /// 試合Id
        /// </summary>
        public int GameId{ get; set; }

        /// <summary>
        /// ホームチームId
        /// </summary>
        public int? HomeTeamId { get; set; }

        /// <summary>
        /// ホームチーム略名
        /// </summary>
        public string HomeTeamShortName{get;set;}

        /// <summary>
        /// ホームスコア
        /// </summary>
        public int? HomeScore { get; set; }

        /// <summary>
        /// アウェイチームId
        /// </summary>
        public int? AwayTeamId { get; set; }
        
        /// <summary>
        /// アウェイチーム略名
        /// </summary>
        public string AwayTeamShortName{get;set;}

        /// <summary>
        /// アウェイスコア
        /// </summary>
        public int? AwayScore { get; set; }
    }
}
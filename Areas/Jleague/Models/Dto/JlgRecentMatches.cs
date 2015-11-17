using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;

namespace Splg.Areas.Jleague.Models.Dto
{
    public class JlgRecentMatches : IJlgRecentResults
    {
        /// <summary>
        /// 試合日
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsAdWithSlashSeparatorForInt)]
        public int? GameDate { get; set; }

        /// <summary>
        /// ホームチームId
        /// </summary>
        public int? HomeTeamId { get; set; }

        /// <summary>
        /// ホームチーム略名
        /// </summary>
        public string HomeTeamShortName { get; set; }

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
        public string AwayTeamShortName { get; set; }

        /// <summary>
        /// アウェイスコア
        /// </summary>
        public int? AwayScore { get; set; }
    }
}
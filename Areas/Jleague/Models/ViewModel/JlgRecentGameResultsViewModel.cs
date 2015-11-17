using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.Jleague.Models.Dto;

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgRecentGameResultsViewModel
    {
        public int TargetHomeTeamId { get; set; }

        public int TargetAwayTeamId { get; set; }

        public JlgTeamSpec HomeTeamSpec { get; set; }

        public JlgTeamSpec AwayTeamSpec { get;set;}

        /// <summary>
        /// ホーム 直近の試合結果
        /// </summary>
        public IEnumerable<JlgRecentGameResult> HomeRecentGameResults { get; set; }

        /// <summary>
        /// アウェイ 直近の試合結果
        /// </summary>
        public IEnumerable<JlgRecentGameResult> AwayRecentGameResults { get; set; }

        /// <summary>
        /// ホーム　直近の試合結果（勝敗カウント用）
        /// </summary>
        public JlgGameResultCounts HomeRecentGameResultCounts { get; set; }

        /// <summary>
        /// アウェイ　直近の試合結果（勝敗カウント用）
        /// </summary>
        public JlgGameResultCounts AwayRecentGameResultCounts { get; set; }

    }
}
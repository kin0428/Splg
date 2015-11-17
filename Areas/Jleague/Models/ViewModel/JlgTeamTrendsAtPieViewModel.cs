using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.PieChart;
using Splg.Areas.Jleague.Models.Dto;

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgTeamTrendsAtPieViewModel
    {
        public int TargetHomeTeamId { get; set; }

        public int TargetAwayTeamId { get; set; }

        public JlgTeamSpec HomeTeamSpec { get; set; }

        public JlgTeamSpec AwayTeamSpec { get; set; }

        /// <summary>
        /// ホーム得点パターン用円グラフDto
        /// </summary>
        public PieChartDto HomeChartAtGoalPattern { get; set; }

        /// <summary>
        /// ホーム失点パターン用円グラフDto
        /// </summary>
        public PieChartDto HomeChartAtLostPattern { get; set; }

        /// <summary>
        /// ホーム時間別得点パターン用円グラフDto
        /// </summary>
        public PieChartDto HomeChartAtGoalGroupByTimeZone { get; set; }

        /// <summary>
        /// ホーム時間別失点パターン用円グラフDto
        /// </summary>
        public PieChartDto HomeChartAtLostGroupByTimeZone { get; set; }

        /// <summary>
        /// ホームパス成功率用円グラフDto
        /// </summary>
        public PieChartDto HomeChartAtPassSucceedAverage { get; set; }

        /// <summary>
        /// ホームパス距離別の総数・割合用円グラフDto
        /// </summary>
        public PieChartDto HomeChartAtPassPattern { get; set; }

        /// <summary>
        /// アウェイ得点パターン用円グラフDto
        /// </summary>
        public PieChartDto AwayChartAtGoalPattern { get; set; }

        /// <summary>
        /// アウェイ失点パターン用円グラフDto
        /// </summary>
        public PieChartDto AwayChartAtLostPattern { get; set; }

        /// <summary>
        /// アウェイ時間別得点パターン用円グラフDto
        /// </summary>
        public PieChartDto AwayChartAtGoalGroupByTimeZone { get; set; }

        /// <summary>
        /// アウェイ時間別失点パターン用円グラフDto
        /// </summary>
        public PieChartDto AwayChartAtLostGroupByTimeZone { get; set; }

        /// <summary>
        /// アウェイパス成功率用円グラフDto
        /// </summary>
        public PieChartDto AwayChartAtPassSucceedAverage { get; set; }

        /// <summary>
        /// アウェイパス距離別の総数・割合用円グラフDto
        /// </summary>
        public PieChartDto AwayChartAtPassPattern { get; set; }
    }
}
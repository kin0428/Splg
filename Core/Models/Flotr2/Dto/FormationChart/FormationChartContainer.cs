using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;

namespace Splg.Core.Models.Flotr2.Dto.FormationChart
{
    public class FormationChartContainer : AbstractChartContainer
    {

        /// <summary>
        /// ホームチームID
        /// </summary>
        public int HomeTeamID { get; set; }

        /// <summary>
        /// アウェーチームID
        /// </summary>
        public int AwayTeamID { get; set; }
           
        /// <summary>
        /// ホームチームプレイヤーID
        /// </summary>
        public List<string> HomePlayerIDList { get; set; }

        /// <summary>
        /// アウェーチームプレイヤーID
        /// </summary>
        public List<string> AwayPlayerIDList { get; set; }

        /// <summary>
        /// ホームチームポジションコンテナー
        /// </summary>
        public List<FormationDataSource> HomePositionContainer { get; set; }

        /// <summary>
        /// アウェーチームポジションコンテナー
        /// </summary>
        public List<FormationDataSource> AwayPositionContainer { get; set; }

        /// <summary>
        /// ホームチームプレイヤー名コンテナー
        /// </summary>
        public List<FormationDataSource> HomePlayerNameContainer { get; set; }

        /// <summary>
        /// ホームチームプレイヤー背番号コンテナー
        /// </summary>
        public List<FormationDataSource> HomePlayerNoContainer { get; set; }

        /// <summary>
        /// アウェーチームプレイヤー名コンテナー
        /// </summary>
        public List<FormationDataSource> AwayPlayerNameContainer { get; set; }

        /// <summary>
        /// アウェーチームプレイヤー背番号コンテナー
        /// </summary>
        public List<FormationDataSource> AwayPlayerNoContainer { get; set; }

    }


}
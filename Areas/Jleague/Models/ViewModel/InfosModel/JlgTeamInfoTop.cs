#region (c) 2015 Prime Labo - All rights reserved
/*                                      COPYRIGHT NOTICE
 * -------------------------------------------------------------------------------------
 * All materials (including but not limited to source code, compiled assemblies, images,
 * resources, etc.) are copyrighted to Prime Labo. No usage is allowed unless permitted 
 * by written consent. You may not use, reverse-engineer these materials under any
 * circumstances.
 * 
 *                                    PROJECT DESCRIPTION
 * -------------------------------------------------------------------------------------
 * Namespace	: Splg.Areas.Jleague.Models.ViewModel.InfosModel
 * Class		: JlgTeamInfoTop
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-04-02
 */
#endregion
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion
namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    /// <summary>
    /// Infos Model using at JlgTeamInfoTopViewModel
    /// </summary>
    public class JlgTeamInfoTop
    {
        /// <summary>
        /// チームID
        /// </summary>
        public Nullable<int> TeamID { get; set; }

        /// <summary>
        /// 出力＆ジャンプ
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// 出力
        /// </summary>
        public string TeamIcon { get; set; }

        /// <summary>
        /// 　リーグ名
        /// </summary>
        public string LeagueNameS { get; set; }

        /// <summary>
        /// 　リーグ順位
        /// </summary>
        public Nullable<int> Ranking { get; set; }

        ////
        /// <summary>
        /// 上記４条件で取得したレコードより、期待度偏差値 (ExpectationsDeviation)
        ///  裏切度偏差値(BetrayalDeviation)　を取得する。
        /// </summary>

        /// <summary>
        /// 　期待度偏差値
        /// </summary>
        public Nullable<decimal> ExpectationsDeviation { get; set; }

        /// <summary>
        /// 裏切度偏差値
        /// </summary>
        public Nullable<decimal> BetrayalDeviation { get; set; }

        /// <summary>
        /// 勝点
        /// </summary>
        public Nullable<int> Point { get; set; }

        /// <summary>
        /// 試合数
        /// </summary>         
        public Nullable<int> Game { get; set; }

        /// <summary>
        /// 勝数
        /// </summary>
        public Nullable<int> Win { get; set; }

        /// <summary>
        /// 引分数
        /// </summary>
        public Nullable<int> Lose { get; set; }

        /// <summary>
        /// 　敗数
        /// </summary>
        public Nullable<int> Draw { get; set; }

        /// <summary>
        /// 得点
        /// </summary>
        public Nullable<int> Score { get; set; }

        /// <summary>
        /// 失点
        /// </summary>
        public Nullable<int> Lost { get; set; }

        public Nullable<int> Differ { get; set; }

        /// <summary>
        /// "記事作成"
        /// スポーツID と　チームID:TeamID && SportsID をセットして
        ///5-4投稿記事　記事作成（入力画面）へジャンプ
        ///Com.Deviation
        /// </summary>
        public Nullable<int> SportsID { get; set; }
    }
}
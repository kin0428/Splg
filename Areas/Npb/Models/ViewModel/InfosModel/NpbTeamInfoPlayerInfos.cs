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
 * Namespace	: Splg.Areas.Npb.Models.ViewModel.InfosModel
 * Class		: NpbTeamInfoPlayerInfos
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-03-12
 */
#endregion
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion
namespace Splg.Areas.Npb.Models.ViewModel.InfosModel
{
    public class NpbTeamInfoPlayerInfos
    {
        public Nullable<int> TeamCD { get; set; }
        public string Num { get; set; }
        public string Name { get; set; }
        public string ShortNameTeam { get; set; }    
        public string EarnedRunAverage { get; set; }        
        public Nullable<int> Game { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> Lose { get; set; }
        public Nullable<int> Lost { get; set; }
        public Nullable<int> Save { get; set; }
        public string BattingAverage { get; set; }
        public Nullable<int> PlateAppearance { get; set; }
        public Nullable<int> AtBat { get; set; }
        public Nullable<int> Base{ get; set; }
        public Nullable<int> Hit { get; set; }
        public Nullable<int> RunsBattingIn { get; set; }
        public string StaffTitle { get; set; }
        public string BackNumber { get; set; }
        public Nullable<int> PlayerCD { get; set; }
        public Nullable<int> playerMSTCD { get; set; }
        public string PlayerName { get; set; }
        public Nullable<int> Age { get; set; }
        public string Career { get; set; }
        /// <summary>
        /// 投球回数を表示する場合、次の内容で出力する。
        /// 投球回数：InningsPitched (a)
        /// 投球回数三分の一
        ///     InningsPitched3rd
        ///      が	0	の時、空白				(b)	
	    ///         1	の時、1/3　　					
        ///         2 の時、2/3					      
        /// (a) + (b) と　表示する。    
        /// と表示する
        /// </summary> 
        public string Display { get; set; }
    }
}
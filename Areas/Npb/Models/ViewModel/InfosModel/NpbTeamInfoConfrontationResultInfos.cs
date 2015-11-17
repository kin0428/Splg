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
 * Class		: NpbTeamInfoConfrontationResultInfos
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-03-06
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
    public class NpbTeamInfoConfrontationResultInfos
    {
        public Nullable<int> TeamCD { get; set; }
        public string Team { get; set; }
        public string TeamIcon { get; set; }        
        public string ShortNameTeam { get; set; }
        public Nullable<int> TeamsOpponentCD { get; set; }
        public Nullable<int> Game { get; set; }
        public Nullable<int> Run { get; set; }
        public Nullable<int> StrikeOut { get; set; }
        public Nullable<int> StolenBase { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> PointLost { get; set; }
        public Nullable<int> BaseOnBall { get; set; }
        public Nullable<int> Error { get; set; }
        public Nullable<int> Lose { get; set; }
        public Nullable<int> Hit { get; set; }
        public Nullable<int> Homerun { get; set; }
        public Nullable<int> HitByPitch { get; set; }
        public string BattingAverage { get; set; }
        public Nullable<int> Draw { get; set; }
        public Nullable<int> DoublePlay { get; set; }
        public string EarnedRunAverage { get; set; } 
        
        ///
        ///Game Infor results view
        ///
        /// 

        ///GameStatus Result Game Infor
        /// GameStatus = 0 ;  GameStatus Name = "Schedule" 
        /// GameStatus = 1 ;  GameStatus Name = "Inning"
        /// GameStatus = 2 ;  GameStatus Name = "Final"
        ///

        public Nullable<int> GameStatus { get; set; }       

        ///　　日付			
        public string ShortDate { get; set; }

        public Nullable<int> GameID { get; set; }

        public string GameDate { get; set; }

        public Nullable<int> HomeTeam { get; set; }

        public Nullable<int> VisitorTeam { get; set; }

        public Nullable<int> WinTeamCD { get; set; }
        
        ///　　スコア　勝敗						
        public string WinTeamSymbol { get; set; }

        ///　　スコア　得点情報		
        ///　　
        public Nullable<int> InningHomeTotalScore { get; set; }

        public Nullable<int> InningVisitorTotalScore { get; set; }

        public Nullable<int> HomeTotalScore { get; set; }

        public Nullable<int> VisitorTotalScore { get; set; }
    	
        public string WinTeamChar { get; set; }

        public string NameLink { get; set; }

        public string AndSymbol { get; set; }

        public string ViewTotalScore { get; set; }

        ///　　責任投手　投手名					

        public Nullable<int> WinningPlayerID { get; set; }
        public string WinningShortNamePlayer { get; set; }
        public Nullable<int> LosingPlayerID { get; set; }
        public string LosingShortNamePlayer { get; set; }

        public Nullable<int> SavingPlayerID { get; set; }
        public string SavingShortNamePlayer { get; set; }

        ///　　　球場

        public string ShortNameStadium { get; set; }

        ///　　　　開始時間
        public string StartTime { get; set; }

        public Nullable<int> InMonth { get; set; }

        public Nullable<int> InYear { get; set; }
     
    }
}
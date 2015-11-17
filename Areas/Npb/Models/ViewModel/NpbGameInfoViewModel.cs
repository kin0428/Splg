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
 * Namespace	: Splg.Areas.Npb.Models.ViewModel
 * Class		: NpbGameInfoViewModel
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using Splg.Models;
using Splg.Models.Game.ViewModel;
using Splg.Models.News.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Npb.Models.ViewModel
{
    public class NpbGameInfoViewModel
    {
        public IEnumerable<TeamInfoPSP> ListPreStartingPitcher { get; set; }
        public IEnumerable<PlayerInfoInGame> ListPlayerInfo { get; set; }
        public IEnumerable<GameInHistory> List5GamesInHistory { get; set; }
        public GameInfoViewModel GameInSeasonSchedule { get; set; }
        public IEnumerable<ScoreInGame> ListGameScoreOngoing { get; set; }
        public IEnumerable<ScoreInGame> ListGameScoreEnd { get; set; }
        public IEnumerable<GameText> ListGameText { get; set; }
        public GameRoundComment GameRoundComment { get; set; }
        public IEnumerable<PitcherGameEndInfo> ListPitcher { get; set; }
        public IEnumerable<HomerunGameEndInfo> ListHomerun { get; set; }
        public IEnumerable<ReliefInfoGameEndInfo> ListReliefInfoes { get; set; }
        public IEnumerable<PostedInfoViewModel> PostedInfo { get; set; }
        public IEnumerable<GameInfoViewModel> ListRightContentGames { get; set; }
        public IEnumerable<GameText> ListGameTextScore { get; set; }
        public List<Member> FollowMembersBetToWin { get; set; }
        public List<Member> FollowMembersBetToLose { get; set; }
        public List<Member> FollowMembersBetToDraw { get; set; }
    }

    public class PlayerInfoInGame
    {
        public PlayerInfoST PlayerInfoStarting { get; set; }
        public PlayerInfoGO PlayerInfoGameOrder { get; set; }
        public int HV { get; set; }
        public int TeamID { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public int PlayerID { get; set; }
        public string PlayerNameS { get; set; }
        public string PlayerEra { get; set; }
        public bool IsWIN { get; set; }
    }

    public class GameInHistory
    {
        public GameInfoSSN GameInfoSSN { get; set; }
        private decimal oddsHomeTeam;

        public decimal OddsHomeTeam
        {
            get
            {
                decimal result = Math.Round(oddsHomeTeam, 1, MidpointRounding.AwayFromZero);
                return result;
            }
            set { oddsHomeTeam = value; }
        }
        private decimal oddsVisitor;

        public decimal OddsVisitor
        {
            get
            {
                decimal result = Math.Round(oddsVisitor, 1, MidpointRounding.AwayFromZero);
                return result;
            }
            set { oddsVisitor = value; }
        }
        private decimal oddsDraw;

        public decimal OddsDraw
        {
            get
            {
                decimal result = Math.Round(oddsDraw, 1, MidpointRounding.AwayFromZero);
                return result;
            }
            set { oddsDraw = value; }
        }
    }

    public class ScoreInGame
    {
        public int HV { get; set; }
        public int TeamID { get; set; }
        public string NameS { get; set; }      
        public int Runs { get; set; }
        public int Hits { get; set; }
        public int Err { get; set; }                 
        public long TeamInfoGTEID { get; set; }
        public long TeamInfoCOID { get; set; }
        public int Inning { get; set; }
    }    

    public class GameText
    {
        public string Round { get; set; }
        public string RoundName { get; set; }
        public int TeamID { get; set; }
        public string TeamNameS { get; set; }
        public string TextOfTeam { get; set; }
        public int  HV { get; set; }
        public int ID { get; set; }
        public long GameInfoGTEID { get; set; }        
    }

    public class GameRoundComment
    {
        public string HomeTeamNameS { get; set; }
        public string VisitorTeamNameS { get; set; }
        public int Round { get; set; }
        public string GameComment { get; set; }
        public int Atendance { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int Inning { get; set; }
        public int TB { get; set; }
        public int TotalScoreHTeam { get; set; }
        public int TotalScoreVTeam { get; set; }
    }

    public class PitcherGameEndInfo
    {
        public int Type { get; set; }
        public int PlayerID { get; set; }
        public string PlayerNameS { get; set; }
        public int Win { get; set; }
        public int Lose { get; set; }
        public int Save { get; set; }
        public string TeamNameS { get; set; }
        public int TeamID { get; set; }
    }

    public class HomerunGameEndInfo
    {
        public int TeamID { get; set; }
        public string TeamNameS { get; set; }
        public int PlayerID { get; set; }
        public string PlayerNameS { get; set; }
        public int HV { get; set; }
        public int TotalC {get; set;}
    }

    public class ReliefInfoGameEndInfo
    {
        public int TeamID { get; set; }
        public string TeamNameS { get; set; }     
        public string NameS { get; set; }
        public int PlayerID { get; set; }
        public int Type { get; set; }
        public int HV { get; set; }
    }
     
}
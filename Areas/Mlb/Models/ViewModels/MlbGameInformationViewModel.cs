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
 * Namespace	: Splg.Areas.Mlb.Models.ViewModel
 * Class		: MlbGameInfoViewModel
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using Splg.Areas.Mlb.Models.ViewModels.InfosModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models.Game.ViewModel;
using Splg.Models.News.ViewModel;
using Splg.Models;
#endregion


namespace Splg.Areas.Mlb.Models.ViewModel
{

    public class MlbGameInformationViewModel
    {
        public IEnumerable<MlbTeamInfoPSPModel> ListPreStartingPitcher { get; set; }
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
        public List<Member> FollowMembersBetToWin { get; set; }
        public List<Member> FollowMembersBetToLose { get; set; }
        public List<Member> FollowMembersBetToDraw { get; set; }
        public int HomeScore { get; set; }
        public int VisitorScore { get; set; }
        public int Inning { get; set; }
    }

    public class PlayerInfoInGame
    {
        public MlbPlayerInfoStModel PlayerInfoStarting { get; set; }
        public MlbPlayerInfoGOModel PlayerInfoGameOrder { get; set; }
        public int HV { get; set; }
        public int TeamID { get; set; }
        public int PositionID { get; set; }
        public string PositionName { get; set; }
        public int PlayerID { get; set; }
        public string HomeStartingName { get; set; }
        public string VisitorStartingName { get; set; }        
    }

    public class GameInHistory
    {
        public int DateJPN { get; set; }
        public int GameID { get; set; }
        public Nullable<int> HomeTeamID { get; set; }
        public string HomeTeamFullName { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeStarterName { get; set; }
        public string HomeStarterNum { get; set; }
        public Nullable<short> HomeStarterArm { get; set; }
        public Nullable<int> HomeScore { get; set; }
        public Nullable<int> HomeHits { get; set; }
        public Nullable<int> HomeErrors { get; set; }
        public Nullable<int> VisitorTeamID { get; set; }
        public string VisitorTeamFullName { get; set; }
        public string VisitorTeamName { get; set; }
        public string VisitorStarterName { get; set; }
        public string VisitorStarterNum { get; set; }
        public Nullable<short> VisitorStarterArm { get; set; }
        public Nullable<int> VisitorScore { get; set; }
        public Nullable<int> VisitorHits { get; set; }
        public Nullable<int> VisitorErrors { get; set; }
        public Nullable<int> GameStateID { get; set; }
    }

    public class ScoreInGame
    {
        public int HV { get; set; }
        public int TeamID { get; set; }
        public string NameS { get; set; }
        public string Score_Inn_1 { get; set; }
        public string Score_Inn_2 { get; set; }
        public string Score_Inn_3 { get; set; }
        public string Score_Inn_4 { get; set; }
        public string Score_Inn_5 { get; set; }
        public string Score_Inn_6 { get; set; }
        public string Score_Inn_7 { get; set; }
        public string Score_Inn_8 { get; set; }
        public string Score_Inn_9 { get; set; }
        public int Runs { get; set; }
        public int Hits { get; set; }
        public int Err { get; set; }
    }

    public class GameText
    {
        public string Round { get; set; }
        public string RoundName { get; set; }
        public int TeamID { get; set; }
        public string TeamNameS { get; set; }
        public string TextOfTeam { get; set; }
        public int HV { get; set; }
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
        public int TotalC { get; set; }
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
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
 * Class		: JlgGameInfos
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-04-14
 */
#endregion
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using Splg.Models.Game.InfoModel;
using Splg.Models.Members.InfoModel;
using Splg.Core.Constant;

#endregion
namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    /// <summary>
    /// Infos Model using at JlgResultViewModel
    /// </summary>
    public class JlgGameInfos
    {
        public int? TeamID { get; set; }
        public int? ID { get; set; }
        public int? SeasonID { get; set; }
        public string SeasonName { get; set; }
        public int? Sec { get; set; }
        public int GameID { get; set; }
        public int GameDate { get; set; }
        public int HV { get; set; }
        public int? HomeTeamID { get; set; }
        public int? VisitorTeamID { get; set; }
        public string GameNameS { get; set; }
        public int StadiumID { get; set; }
        public string StadiumName { get; set; }
        public string StadiumNameS { get; set; }
        public string GameResult { get; set; }
        public string GameResultSymbol { get; set; }
        public string Time { get; set; }
        public int GameKindID { get; set; }
        public string GameKindName { get; set; }
        public int? Score { get; set; }
        public int? Lost { get; set; }
        public int FirstTeamID { get; set; }
        public int FirstTeamNo { get; set; }
        public string FirstTeamName { get; set; }
        public string FirstTeamNameS { get; set; }
        public string FirstTeamIcon { get; set; }
        public int SecondTeamID { get; set; }
        public int SecondTeamNo { get; set; }
        public string SecondTeamName { get; set; }
        public string SecondTeamNameS { get; set; }
        public string SecondTeamIcon { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamNameS { get; set; }
        public string HomeTeamIcon { get; set; }
        public int HomeTeamRanking { get; set; }
        public int HomeTeamWin { get; set; }
        public int? HomeTeamR { get; set; }
        public string HomeTeamScore { get; set; }
        public int AwayTeamID { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamNameS { get; set; }
        public string AwayTeamIcon { get; set; }
        public int AwayTeamRanking { get; set; }
        public int AwayTeamWin { get; set; }
        public int? AwayTeamR { get; set; }
        public string AwayTeamScore { get; set; }

        public int OccasionNo { get; set; }
        public int Round { get; set; }

        public int? StateID { get; set; }
        public string StateName { get; set; }
        public short? HalfEndF { get; set; }
        public int? Half { get; set; }


        //Phase3#2000~2002 試合情報パネル対応
        public string LeagueNameS { 
            get{
                string result = null;
                int GameKindID = this.GameKindID;

                switch (GameKindID)
                {
                    case JlgConstants.JLG_GAMEKIND_J1:
                        result = JlgConstants.JLG_LEAGUE_NAME_J1;
                        break;
                    case JlgConstants.JLG_GAMEKIND_J2:
                        result = JlgConstants.JLG_LEAGUE_NAME_J2;
                        break;
                    case JlgConstants.JLG_GAMEKIND_NABISCO:
                        result = JlgConstants.JLG_LEAGUE_NAME_NABISCO;
                        break;
                }

                return result;
            }
        }

        /// <summary>
        /// この試合に予想している会員リスト
        /// </summary>
        public List<MemberModel> BetMembers { get; set; }

        private List<MemberModel> betHomeMembers;

        /// <summary>
        /// この試合のホームの勝ちに予想している会員リスト
        /// </summary>
        public List<MemberModel> BetHomeMembers
        {
            get
            {
                if (BetMembers == null)
                    return new List<MemberModel>();

                var result = BetMembers.Where(x => x.BetSelectID == (int)BetConst.BetSelectID.Home);

                if (result.Any())
                    return result.ToList(); 
                else
                    return new List<MemberModel>();

            }
        }

        private List<MemberModel> betAwayMembers;
        /// <summary>
        /// この試合のアウエイの勝ちに予想している会員リスト
        /// </summary>
        public List<MemberModel> BetAwayMembers
        {
            get
            {
                if (BetMembers == null)
                    return new List<MemberModel>();

                var result = BetMembers.Where(x => x.BetSelectID == (int)BetConst.BetSelectID.Visitor);

                if (result.Any())
                    return result.ToList();
                else
                    return new List<MemberModel>();
            }
        }
        
        private List<MemberModel> betDrawMembers;
        /// <summary>
        /// この試合の引き分けに予想している会員リスト
        /// </summary>
        public List<MemberModel> BetDrawMembers
        {
            get
            {
                if (BetMembers == null)
                    return new List<MemberModel>();

                var result = BetMembers.Where(x => x.BetSelectID == (int)BetConst.BetSelectID.Draw);

                if (result.Any())
                    return result.ToList();
                else
                    return new List<MemberModel>();
            }
        }

        public List<JlgScoreDetailsModel> ScoreDetails { get; set; }

        public List<JlgScoreDetailsModel> HomeScoreDetails
        {
            get
            {
                if (ScoreDetails != null)
                    return ScoreDetails.Where(x => x.Hv == 1).ToList();
                else return null;
            }
        }

        public List<JlgScoreDetailsModel> AwayScoreDetails
        {
            get
            {
                if (ScoreDetails != null)
                    return ScoreDetails.Where(x => x.Hv == 2).ToList();
                else return null;
            }
        }

        public List<JlgScoreDetailsModel> HomeFirstHalfScoreDetails
        {
            get
            {
                if (HomeScoreDetails != null)
                    return HomeScoreDetails.Where(x => x.StateID == 1 || x.StateID == 3 || x.StateID == 5).ToList();
                else return null;
            }
        }

        public List<JlgScoreDetailsModel> HomeSecondHalfScoreDetails
        {
            get
            {
                if (HomeScoreDetails != null)
                    return HomeScoreDetails.Where(x => x.StateID == 2 || x.StateID == 4 || x.StateID == 6).ToList();
                else return null;
            }
        }

        public List<JlgScoreDetailsModel> AwayFirstHalfScoreDetails
        {
            get
            {
                if (AwayScoreDetails != null)
                    return AwayScoreDetails.Where(x => x.StateID == 1 || x.StateID == 3 || x.StateID == 5).ToList();
                else return null;
            }
        }

        public List<JlgScoreDetailsModel> AwaySecondHalfScoreDetails
        {
            get
            {
                if (AwayScoreDetails != null)
                    return AwayScoreDetails.Where(x => x.StateID == 2 || x.StateID == 4 || x.StateID == 6).ToList();
                else return null;
            }
        }

        public int HomeFirstHalfScore
        {
            get
            {
                int result = 0;

                if (HomeFirstHalfScoreDetails != null)
                    return HomeFirstHalfScoreDetails.Count();

                return result;
            }
        }

        public int HomeSecondHalfScore
        {
            get
            {
                int result = 0;

                if (HomeSecondHalfScoreDetails != null)
                    return HomeSecondHalfScoreDetails.Count();

                return result;
            }
        }

        public int AwayFirstHalfScore
        {
            get
            {
                int result = 0;

                if (AwayFirstHalfScoreDetails != null)
                    return AwayFirstHalfScoreDetails.Count();

                return result;
            }
        }

        public int AwaySecondHalfScore
        {
            get
            {
                int result = 0;

                if (AwaySecondHalfScoreDetails != null)
                    return AwaySecondHalfScoreDetails.Count();

                return result;
            }
        }

        public int HomeScore
        {
            get
            {
                return HomeFirstHalfScore + HomeSecondHalfScore;
            }
        }

        public int AwayScore
        {
            get
            {
                return AwayFirstHalfScore + AwaySecondHalfScore;
            }
        }

        // todo ViewModelへ切り出したい
        //Phase3#2000~2002 試合情報パネル対応
        GameInfoModel gameInfoModel = null;
        public GameInfoModel GameInfoModel
        {
            get {
                return gameInfoModel;
            }
            set
            {
                gameInfoModel = value;
                if (gameInfoModel != null && string.IsNullOrEmpty(gameInfoModel.HomeTeamS))
                    gameInfoModel.HomeTeamS = this.HomeTeamNameS;
                if (gameInfoModel != null && string.IsNullOrEmpty(gameInfoModel.VisitorTeamS))
                    gameInfoModel.VisitorTeamS = this.AwayTeamNameS;
            }
        }

        public int GameStatus { get; set; }

        public GameOddsInfoModel GameOddsInfoModel { get; set; }

        public ParameterInfoModel ParameterInfo { get; set; }

        public class ParameterInfoModel
        {
            public int? ParameterType { get; set; }
            public int? Link { get; set; }
            public int? GameDate { get; set; }
            public int? OccasionNo { get; set; }
            public int? TeamId { get; set; }
            public int? GameId { get; set; }
            public string LstGameId { get; set; }
            public int? LeagueType { get; set; }
            public string LeagueNameS { get; set; }
            public int? Round { get; set; }
            public bool WithScoreDetail { get; set; }
        }
    }

    /// <summary>
    /// Use to get db with many conditions.
    /// </summary>
    public class JlgGameInfoByCondidtion
    {
        public ScheduleInfo ScheduleInfoJlg { get; set; }
        public int GameKindID { get; set; }
        public string GameKindName { get; set; }
        public int? SeasonId { get; set; }

        private string gameTimetoStr;

        public string GameTimetoStr
        {
            get {
                if (!String.IsNullOrEmpty(gameTimetoStr))
                    return gameTimetoStr.PadLeft(4, '0');

                return gameTimetoStr; 
            }
            set { gameTimetoStr = value; }
        }
    }


}

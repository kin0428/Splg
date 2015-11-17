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
 * Namespace	: Splg.Models.Game.ViewModel
 * Class		: GameInfoViewModel
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using System;
using Splg.Areas.Npb.Models;
using Splg.Models.Game.InfoModel;
#endregion

namespace Splg.Models.Game.ViewModel
{
    public class GameInfoViewModel
    {
        public int GameID { get; set; }
        public int GameDate { get; set; }

        private string time;
        public string Time
        {
            get
            {
                string result = "2359";
                if (!String.IsNullOrEmpty(time)) 
                {
                    if (time.Trim() == "未定")
                        time = result;

                    return time;
                }

                return result;
            }
            set { time = value; }
        }

        public int GameKindID { get; set; }
        public int GameTypeID { get; set; }
        public string GameTypeName { get; set; }
        public int LeagueID { get; set; }
        public string LeagueName { get; set; }
        public int DivID { get; set; }
        public string DivName { get; set; }
        public string StadiumName { get; set; }
        public int HomeTeamID { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamFullName { get; set; }
        public string HomeTeamNameS { get; set; }
        public string HomeTeamUrl { get; set; }
        private string homeTeamIcon;
        public string HomeTeamIcon
        {
            get
            {
                string result = "/Content/News/PN_UTF8/photo/default.png";
                if (!String.IsNullOrEmpty(homeTeamIcon))
                {
                    if (!homeTeamIcon.StartsWith("/") && !homeTeamIcon.StartsWith("~"))
                        homeTeamIcon = "/" + homeTeamIcon;

                    return homeTeamIcon;
                }

                return result;
            }
            set { homeTeamIcon = value; }
        }


        public int HomeTeamRanking { get; set; }
        public int? HomeTeamWin { get; set; }
        public int? HomeTeamScore { get; set; }
        public string HomeTeamPitcherNum { get; set; }
        public string HomeTeamPitcherName { get; set; }
        public int? HomeTeamR { get; set; }
        public string VisitorTeamNameS { get; set; }
        public int VisitorTeamID { get; set; }
        public string VisitorTeamFullName { get; set; }
        public string VisitorTeamName { get; set; }
        public string VisitorTeamUrl { get; set; }
        private string vistorTeamIcon;
        public string VisitorTeamIcon
        {
            get
            {
                string result = "/Content/News/PN_UTF8/photo/default.png";
                if (!String.IsNullOrEmpty(vistorTeamIcon))
                {
                    if (!vistorTeamIcon.StartsWith("/") && !vistorTeamIcon.StartsWith("~"))
                        vistorTeamIcon = "/" + vistorTeamIcon;

                    return vistorTeamIcon;
                }

                return result;
            }
            set { vistorTeamIcon = value; }
        }

        public int VisitorTeamRanking { get; set; }
        public int? VisitorTeamWin { get; set; }
        public string VisitorTeamPitcherNum { get; set; }
        public string VisitorPitcherName { get; set; }
        public int? VisitorTeamScore { get; set; }
        public int? VisitorTeamR { get; set; }
        public int Status { get; set; }
        public int Round { get; set; }
        public string GameSituationID { get; set; }
        public int Inning { get; set; }
        public string BottomTop { get; set; }

        public ExpectationInfoModel ExpectationInfo { get; set; }

    }

    public class ScoreGameInfo
    {
        public int GameID { get; set; }
        public int TeamID { get; set; }
        public int? R { get; set; }
        public int? Inning { get; set; }
        public int? TB { get; set; }
    }

    /// <summary>
    /// Use to get db with many conditions.
    /// </summary>
    public class GameInfoSSByCondidtion
    {
        public GameInfoSS GameInfoSSNpb { get; set; }
        public int GameTypeID { get; set; }
        public string GameTypeName { get; set; }
    }

    public class GameOddsInfo
    {
        public long ExpectTargetID { get; set; }

        private decimal homeTeamOdds;

        public decimal HomeTeamOdds
        {
            get {
                decimal result = Math.Round(homeTeamOdds, 1, MidpointRounding.AwayFromZero);
                return result; 
            }
            set { homeTeamOdds = value; }
        }

        private decimal visitorTeamOdds;

        public decimal VisitorTeamOdds
        {
            get
            {
                decimal result = Math.Round(visitorTeamOdds, 1, MidpointRounding.AwayFromZero);
                return result;
            }

            set { visitorTeamOdds = value; }
        }

        private decimal drawOdds;

        public decimal DrawOdds
        {
            get
            {
                decimal result = Math.Round(drawOdds, 1, MidpointRounding.AwayFromZero);
                return result;
            }
            set { drawOdds = value; }
        }
        public int? BetSelectedID { get; set; }
    }
}
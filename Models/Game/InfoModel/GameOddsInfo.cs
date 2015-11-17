using System;

namespace Splg.Models.Game.InfoModel
{
    public class GameOddsInfoModel
    {
        public long ExpectTargetID { get; set; }

        private decimal homeTeamOdds;

        public decimal HomeTeamOdds
        {
            get
            {
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
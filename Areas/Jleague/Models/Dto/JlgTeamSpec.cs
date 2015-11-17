using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.Dto
{
    public class JlgTeamSpec
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public string TeamShortName { get; set; }

        public string TeamIconUrl { get; set; }

        public double TeamAttackCBP { get; set; }

        public double TeamDefenseCBP { get; set; }

        public string FormationName { get; set; }
    }
}
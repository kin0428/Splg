using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models.Dto;

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgRecentMatchesViewModel
    {
        public int TargetHomeTeamId { get; set; }

        public int TargetAwayTeamId { get; set; }

        public JlgTeamSpec HomeTeamSpec { get; set; }

        public JlgTeamSpec AwayTeamSpec { get; set; }

        public IEnumerable<JlgRecentMatches> RecentMatches { get; set; }

        public JlgGameResultCounts RecentMatchesCounts { get; set; }
    }
}
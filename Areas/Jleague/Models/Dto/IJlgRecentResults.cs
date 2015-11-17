using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splg.Areas.Jleague.Models.Dto
{
    public interface IJlgRecentResults
    {
        int? GameDate { get; set; }

        int? HomeTeamId { get; set; }

        string HomeTeamShortName { get; set; }

        int? HomeScore { get; set; }

        int? AwayTeamId { get; set; }

        string AwayTeamShortName { get; set; }

        int? AwayScore { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgTop3RankingViewModel
    {
        public IEnumerable<JlgOfficialStatsModel> J1League { get; set; }
        public IEnumerable<JlgOfficialStatsModel> J2League { get; set; }
        public IEnumerable<JlgOfficialStatsModel> Nabisco { get; set; }
    }
}
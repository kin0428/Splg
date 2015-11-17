using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Models.ViewModel
{
    public class HomeRightContentViewModel
    {
        public IEnumerable<Prize> NewPrizes { get; set; }
        public IEnumerable<PointRankingViewModel> PointRankings { get; set; }
    }
}
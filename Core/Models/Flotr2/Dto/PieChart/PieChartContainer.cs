using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;

namespace Splg.Core.Models.Flotr2.Dto.PieChart
{
    public class PieChartContainer : AbstractChartContainer
    {
        public List<PieDataSource> DataSources { get; set; }
    }


}
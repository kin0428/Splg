using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Core.Models.Flotr2.Dto.PieChart;
using Splg.Core.Models.Flotr2.Dto.FormationChart;
using Splg.Core.Providers;
using Splg.Core.Constant;

namespace Splg.Controllers
{
    public class ChartController : Controller
    {
        public ActionResult ShowPieChart(PieChartDto pieChartDto)
        {
            var flotr2ChartProvider = new Flotr2ChartProvider();

            var chartResult =  flotr2ChartProvider.GetChartResult(pieChartDto, Flotr2Const.ChartType.Pie);

            return PartialView("~/Views/Chart/Mobile/_Flotr2Chart.cshtml", chartResult);
        }

        public ActionResult ShowFormationChart(FormationChartDto formationChartDto)
        {
            var flotr2ChartProvider = new Flotr2ChartProvider();

            var chartResult = flotr2ChartProvider.GetChartResult(formationChartDto, Flotr2Const.ChartType.Formation);

            return PartialView("~/Views/Chart/Mobile/_Flotr2Chart.cshtml", chartResult);
        }
    }
}
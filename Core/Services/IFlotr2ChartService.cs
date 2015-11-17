using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splg.Core.Models.Flotr2.Dto.Shared;

namespace Splg.Core.Services
{
    public interface IFlotr2ChartService
    {
        ChartResult GetChartResult(IChartDto chartDto);
    }
}

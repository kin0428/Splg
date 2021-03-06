﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;
using Splg.Core.Models.Flotr2.Resources;

namespace Splg.Core.Services
{
    public class BubbleChartService : IFlotr2ChartService
    {
        public ChartResult GetChartResult(IChartDto chartDto)
        {
            var targetScript = Flotr2ScriptResource.BubbleChartScriptTemplete;

            return new ChartResult() { ChartScript = targetScript, ChartContainerSettings = new ChartContainerSettings() };
        }
    }
}
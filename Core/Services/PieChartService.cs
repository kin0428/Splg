using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;
using Splg.Core.Models.Flotr2.Dto.PieChart;
using Splg.Core.Models.Flotr2.Resources;
using Splg.Core.Models.Flotr2.ScriptParameter;

namespace Splg.Core.Services
{
    public class PieChartService : AbstractChartService,IFlotr2ChartService 
    {
        #region Property
        /// <summary>
        /// 円グラフDto
        /// </summary>
        public PieChartDto PieChartDto { get; set; }
        #endregion

        #region method
        /// <summary>
        /// チャートスクリプト取得
        /// </summary>
        public ChartResult GetChartResult(IChartDto chartDto)
        {
            var targetScript = Flotr2ScriptResource.PieChartScriptTemplete;

            PieChartDto = (PieChartDto)chartDto;

            //ファンクション設定
            targetScript = InjectParameter(targetScript, FunctionParameterKey.FunctionName, PieChartDto.FunctionName);

            //コンテナー設定
            targetScript = InjectScriptParameterByChartContainer(targetScript, PieChartDto.PieChartContainer);

            //プロパティ設定
            targetScript = InjectParameterToProperties(targetScript, PieChartDto.PieChartProperties);

            return new ChartResult()
                    { 
                        ChartScript = targetScript,
                        ChartContainerSettings = new ChartContainerSettings() 
                                                    { 
                                                        ChartContainerId = PieChartDto.PieChartContainer.ViewContainerId,
                                                        ChartContainerClass =  PieChartDto.PieChartContainer.ViewContainerClass 
                                                    } 
                    };
        }

        /// <summary>
        /// チャートコンテナー設定
        /// </summary>
        private string InjectScriptParameterByChartContainer(string targetScript, PieChartContainer pieChartContainer)
        {
            var result = targetScript;

            //コンテナーId設定
            result = InjectParameter(result, ContainerParameterKey.ContainerId, pieChartContainer.ViewContainerId);

            //データソース設定
            var dataSourceTemplete = Flotr2ScriptPartsResource.PieChartDataSourceTemplete;

            var containerScript = new System.Text.StringBuilder(); ;

            const string dataTemplete = "[[@x, @y]]";

            foreach (var item in pieChartContainer.DataSources)
            {
                var dataSource = dataSourceTemplete;

                dataSource = InjectParameter(dataSource, ContainerParameterKey.Data, dataTemplete.Replace("@x", item.X.ToString()).Replace("@y", item.Y.ToString()));

                dataSource = InjectParameter(dataSource, ContainerParameterKey.Label, item.Label);

                containerScript.AppendLine(dataSource);
            }

            result = InjectParameter(result, ContainerParameterKey.Container, containerScript.ToString());

            return result;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;
using Splg.Core.Models.Flotr2.Dto.FormationChart;
using Splg.Core.Models.Flotr2.Resources;
using Splg.Core.Models.Flotr2.ScriptParameter;
using Splg.Core.Extend;
using Splg.Core.Constant;

namespace Splg.Core.Services
{
    public class FormationChartService : AbstractChartService, IFlotr2ChartService 
    {
        #region Property
        /// <summary>
        /// フォーメーションDto
        /// </summary>
        public FormationChartDto FormationChartDto { get; set; }
        #endregion

        #region method
        /// <summary>
        /// チャートスクリプト取得
        /// </summary>
        public ChartResult GetChartResult(IChartDto chartDto)
        {
            var targetScript = Flotr2ScriptResource.FormationChartScriptTemplete;
            FormationChartDto = (FormationChartDto)chartDto;

            // コンテナの設定
            targetScript = InjectScriptContainer(targetScript, FormationChartDto);

            // 共通部分の情報
            var partialScript = Flotr2ScriptPartsResource.FormationSharedTemplete;
            //プロパティ設定
            partialScript = InjectParameterToProperties(partialScript, FormationChartDto.FormationChartProperties.SharedProperties);
                    
            // ターゲットスクリプトへの反映
            targetScript = InjectParameter(targetScript, Flotr2Const.FormationSheredTemplete, partialScript);

            return new ChartResult()
            {
                ChartScript = targetScript,
                ChartContainerSettings = new ChartContainerSettings()
                {
                    ChartContainerId = FormationChartDto.FormationChartContainer.ViewContainerId,
                    ChartContainerClass = FormationChartDto.FormationChartContainer.ViewContainerClass
                }
            };
        }

        /// <summary>
        /// コンテナーのstring生成
        /// </summary>
        private string InjectScriptContainer(string targetScript, FormationChartDto formationChartDto)
        {
            var result = targetScript;

            //コンテナーId設定
            result = InjectParameter(result, ContainerParameterKey.ContainerId, FormationChartDto.FormationChartContainer.ViewContainerId);

            List<string> containerScript = new List<string>();

            // ホームチームの情報
            // フォーメーションの情報
            // 位置を示す円（バブルチャート）
            var partialScript = Flotr2ScriptPartsResource.FormationBubblesTemplete;
            //コンテナー設定(バブルチャートの位置)
            partialScript = InjectScriptParameterByDataSource(partialScript, FormationChartDto.FormationChartContainer.HomePositionContainer);
            //プロパティ設定(バブルチャート関連）
            partialScript = InjectParameterToProperties(partialScript, FormationChartDto.FormationChartProperties.HomePositionProperties);

            // MouseON時ファンクションの設定
            partialScript = InjectParameterToTrackFormatter(partialScript, FormationChartDto.FormationChartContainer.HomePlayerIDList, FormationChartDto.FormationChartContainer.HomePositionContainer, Flotr2Const.Home);

            // コンテナーの中身を追加
            containerScript.Add(partialScript);

            // 選手名のマーカー
            partialScript = Flotr2ScriptPartsResource.FormationMarkersTemplete;
            //コンテナー設定(マーカーの位置）
            partialScript = InjectScriptParameterByDataSource(partialScript, FormationChartDto.FormationChartContainer.HomePlayerNameContainer);
            //プロパティ設定(マーカー関連）
            partialScript = InjectParameterToProperties(partialScript, FormationChartDto.FormationChartProperties.HomePlayerNameProperties);

            // Label表示のファンクション設定
            partialScript = InjectParameterToLabelFormatter(partialScript, FormationChartDto.FormationChartContainer.HomePlayerNameContainer, Flotr2Const.HomePlayerName);

            // コンテナーの中身を追加
            containerScript.Add(partialScript);

            // 選手背番号のマーカー
            partialScript = Flotr2ScriptPartsResource.FormationMarkersTemplete;
            //コンテナー設定(マーカーの位置）
            partialScript = InjectScriptParameterByDataSource(partialScript, FormationChartDto.FormationChartContainer.HomePlayerNoContainer);
            //プロパティ設定(マーカー関連）
            partialScript = InjectParameterToProperties(partialScript, FormationChartDto.FormationChartProperties.HomePlayerNoProperties);

            // Label表示のファンクション設定
            partialScript = InjectParameterToLabelFormatter(partialScript, FormationChartDto.FormationChartContainer.HomePlayerNoContainer, Flotr2Const.HomePlayerNo);

            // コンテナーの中身を追加
            containerScript.Add(partialScript);


            // アウェーチームの情報
            // フォーメーションの情報            
            // 位置を示す円（バブルチャート）
            partialScript = Flotr2ScriptPartsResource.FormationBubblesTemplete;
            //コンテナー設定(バブルチャートの位置)
            partialScript = InjectScriptParameterByDataSource(partialScript, FormationChartDto.FormationChartContainer.AwayPositionContainer);
            //プロパティ設定(バブルチャート関連）
            partialScript = InjectParameterToProperties(partialScript, FormationChartDto.FormationChartProperties.AwayPositionProperties);

            // MouseON時ファンクションの設定
            partialScript = InjectParameterToTrackFormatter(partialScript, FormationChartDto.FormationChartContainer.AwayPlayerIDList, FormationChartDto.FormationChartContainer.AwayPositionContainer, Flotr2Const.Away);

            // コンテナーの中身を追加
            containerScript.Add(partialScript);

            // 選手名のマーカー
            partialScript = Flotr2ScriptPartsResource.FormationMarkersTemplete;
            //コンテナー設定
            partialScript = InjectScriptParameterByDataSource(partialScript, FormationChartDto.FormationChartContainer.AwayPlayerNameContainer);
            //プロパティ設定
            partialScript = InjectParameterToProperties(partialScript, FormationChartDto.FormationChartProperties.AwayPlayerNameProperties);

            // Label表示のファンクション設定
            partialScript = InjectParameterToLabelFormatter(partialScript, FormationChartDto.FormationChartContainer.AwayPlayerNameContainer,Flotr2Const.AwayPlayerName);

            // コンテナーの中身を追加
            containerScript.Add(partialScript);

            // 選手背番号のマーカー
            partialScript = Flotr2ScriptPartsResource.FormationMarkersTemplete;
            //コンテナー設定
            partialScript = InjectScriptParameterByDataSource(partialScript, FormationChartDto.FormationChartContainer.AwayPlayerNoContainer);
            //プロパティ設定
            partialScript = InjectParameterToProperties(partialScript, FormationChartDto.FormationChartProperties.AwayPlayerNoProperties);

            // Label表示のファンクション設.
            partialScript = InjectParameterToLabelFormatter(partialScript, FormationChartDto.FormationChartContainer.AwayPlayerNoContainer, Flotr2Const.AwayPlayerNo);

            // コンテナーの中身を追加
            containerScript.Add(partialScript);

            // カンマ区切りに展開したコンテナーを反映
            result = InjectParameter(result, ContainerParameterKey.Container, containerScript.CommaSeparated());

            return result;

        }
        
        /// <summary>
        /// データソース設定
        /// </summary>
        private string InjectScriptParameterByDataSource(string targetScript, List<FormationDataSource> FormationDataSourceList)
        {
            var result = targetScript;

            List<string> DataSourceScript = new List<string>() ;

            const string dataTemplete = "[@x, @y, @Value]";

            foreach (var item in FormationDataSourceList)
            {
                DataSourceScript.Add(dataTemplete.Replace("@x", item.X.ToString()).Replace("@y", item.Y.ToString()).Replace("@Value", item.Value.ToString()));
            }

            result = InjectParameter(result, ContainerParameterKey.Data,  DataSourceScript.CommaSeparated().Enclosed(ExtendConst.SquareBracketEnclosedFormat));

            return result;
        }
        
        /// <summary>
        /// //MouseON時のファンクション設定
        /// </summary>
        private string InjectParameterToTrackFormatter(string targetScript, List<string> playerIDList, List<FormationDataSource> FormationDataSourceList, string suffix)
        {
            var result = targetScript;

            //MouseON時のテンプレート
            var formatterScript = Flotr2ScriptPartsResource.FormationTrackFormatterFunction;

            // 変数名を置換
            formatterScript = InjectParameter(formatterScript, ContainerParameterKey.Suffix, suffix);

            // プレイヤーIDリストを置換
            formatterScript = InjectParameter(formatterScript, ContainerParameterKey.PlayerIDList, playerIDList.CommaSeparated().Enclosed(ExtendConst.SquareBracketEnclosedFormat));

            // ラベルのリストを作成
            List<string> valueList = new List<string>();

            foreach (var item in FormationDataSourceList)
            {
                valueList.Add(item.Value.ToString());
            }
            // バリュー（攻撃力とか守備力）リストを置換
            formatterScript = InjectParameter(formatterScript, ContainerParameterKey.ValueList, valueList.CommaSeparated().Enclosed(ExtendConst.SquareBracketEnclosedFormat));

            result = InjectParameter(result, PropertiesParameterKey.MouseTrackFormatter, formatterScript);

            return result;
        }

        /// <summary>
        /// ラベル表示時のファンクション設定
        /// </summary>
        private string InjectParameterToLabelFormatter(string targetScript, List<FormationDataSource> FormationDataSourceList, string suffix)
        {
            var result = targetScript;

            //ラベル表示時のテンプレート
            var formatterScript = Flotr2ScriptPartsResource.FormationLabelFormatterFunction;          

            // 変数名を置換
            formatterScript = InjectParameter(formatterScript, ContainerParameterKey.Suffix, suffix);

            // ラベルのリストを作成
            List<string> labelList = new List<string>();

            foreach (var item in FormationDataSourceList)
            {
                labelList.Add(item.Label);
            }

            // ラベルリストを置換
            formatterScript = InjectParameter(formatterScript, ContainerParameterKey.Data, labelList.singleQuoteEnclosed().CommaSeparated().Enclosed(ExtendConst.SquareBracketEnclosedFormat));

            result = InjectParameter(result, PropertiesParameterKey.MarkersLabelFormatter, formatterScript);

            return result;
        }


        #endregion
    }
}
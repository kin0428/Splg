using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Models.Flotr2.Dto.Shared;
using Splg.Core.Models.Flotr2.Resources;
using Splg.Core.Models.Flotr2.ScriptParameter;
using Splg.Core.Extend;
using Splg.Core.Constant;

namespace Splg.Core.Services
{
    public abstract class AbstractChartService
    {
        public string InjectParameterToProperties(string targetScript, ChartProperties chartProperties)
        {
            var result = targetScript;

            //IsHtmlText
            result = InjectParameter(result, PropertiesParameterKey.IsHtmlText, chartProperties.IsHtmlText.ToString().ToLower());

            //FontSize
            result = InjectParameter(result, PropertiesParameterKey.FontSize, chartProperties.FontSize.ToString());

            //Resolution
            result = InjectParameter(result, PropertiesParameterKey.Resolution, chartProperties.Resolution.ToString());

            //ShadowSize
            result = InjectParameter(result, PropertiesParameterKey.ShadowSize, chartProperties.ShadowSize.ToString());

            //Colors
            result = InjectParameter(result, PropertiesParameterKey.Colors, chartProperties.Colors.singleQuoteEnclosed().CommaSeparated());

            //Grid
            result = InjectParameter(result, PropertiesParameterKey.GridIsVisibleHorizontalLines, chartProperties.Grid.IsVisibleHorizontalLines.ToString().ToLower());
            result = InjectParameter(result, PropertiesParameterKey.GridIsVisibleVerticalLines, chartProperties.Grid.IsVisibleVerticalLines.ToString().ToLower());
            result = InjectParameter(result, PropertiesParameterKey.GridBackGroundImageAddress, chartProperties.Grid.BackGroundImageAddress.ToString().Enclosed(ExtendConst.SingleQuoteEnclosedFormat));
            result = InjectParameter(result, PropertiesParameterKey.GridOutlineWidth, chartProperties.Grid.GridOutlineWidth.ToString());

            //XAxis
            result = InjectParameter(result, PropertiesParameterKey.XAxisIsVisibleLabels, chartProperties.XAxis.IsVisibleLabels.ToString().ToLower());
            result = InjectParameter(result, PropertiesParameterKey.XAxisMinValue, chartProperties.XAxis.MinValue.ToString());
            result = InjectParameter(result, PropertiesParameterKey.XAxisMaxValue, chartProperties.XAxis.MaxValue.ToString());

            //YAxis
            result = InjectParameter(result, PropertiesParameterKey.YAxisIsVisibleLabels, chartProperties.YAxis.IsVisibleLabels.ToString().ToLower());
            result = InjectParameter(result, PropertiesParameterKey.YAxisMinValue, chartProperties.YAxis.MinValue.ToString());
            result = InjectParameter(result, PropertiesParameterKey.YAxisMaxValue, chartProperties.YAxis.MaxValue.ToString());

            //Pie
            result = InjectParameter(result, PropertiesParameterKey.PieIsVisible, chartProperties.Pie.IsVisible.ToString().ToLower());
            result = InjectParameter(result, PropertiesParameterKey.PieExplode, chartProperties.Pie.Explode.ToString());
            result = InjectParameter(result, PropertiesParameterKey.PieSizeRatio, chartProperties.Pie.SizeRatio.ToString());
            result = InjectParameter(result, PropertiesParameterKey.PieFillOpacity, chartProperties.Pie.FillOpacity.ToString());

            //Mouse
            result = InjectParameter(result, PropertiesParameterKey.MouseIsTrackable, chartProperties.Mouse.IsTrackable.ToString().ToLower());

            //Legend
            result = InjectParameter(result, PropertiesParameterKey.LegendPosition, chartProperties.Legend.position.ToString());
            result = InjectParameter(result, PropertiesParameterKey.LegendBackgroundColor, chartProperties.Legend.BackgroundColor);
            result = InjectParameter(result, PropertiesParameterKey.LegendBorderColor, chartProperties.Legend.BorderColor);

            //Bubble
            result = InjectParameter(result, PropertiesParameterKey.BubblesIsVisible, chartProperties.Bubble.IsVisible.ToString().ToLower());
            result = InjectParameter(result, PropertiesParameterKey.BubblesBaseRadius, chartProperties.Bubble.BaseRadius.ToString());

            //Marker
            result = InjectParameter(result, PropertiesParameterKey.MarkersIsVisible, chartProperties.Marker.IsVisible.ToString().ToLower());
            result = InjectParameter(result, PropertiesParameterKey.MarkersIsRelative, chartProperties.Marker.IsRelative.ToString().ToLower());
            result = InjectParameter(result, PropertiesParameterKey.MarkersFontSize, chartProperties.Marker.FontSize.ToString());

            return result;
        }

        public string InjectParameter(string targetScript, string parameterName, string injectValue)
        {
            return targetScript.Replace(parameterName, injectValue);
        }
    }
}
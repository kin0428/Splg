using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models;
using Splg.Core.Models.Flotr2.Dto.PieChart;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Areas.Jleague.Models.Dto;
using Splg.Core.Constant;
using Splg.Core.Models.Flotr2.Dto.Shared;
using Splg.Core.Models.Flotr2.Dto.Shared.Properties;
using Splg.Core.Constant;

namespace Splg.Services.Game
{
    public class JlgChartService
    {
        #region Property
        /// <summary>
        ///Jlg用Entity
        /// </summary>
        JlgEntities JlgEntities = new JlgEntities();
        #endregion

        #region method

        /// <summary>
        /// チーム傾向＠円グラフビューモデル取得
        /// </summary>
        public JlgTeamTrendsAtPieViewModel GetjlgTeamTrendsAtPieViewModel(int homeTeamId, int awayTeamId)
        {
            var jlgTeamTrendsAtPieViewModel = new JlgTeamTrendsAtPieViewModel();

            var jlgService = new JlgService();

            jlgTeamTrendsAtPieViewModel.TargetHomeTeamId = homeTeamId;
            jlgTeamTrendsAtPieViewModel.TargetAwayTeamId = awayTeamId;
            jlgTeamTrendsAtPieViewModel.HomeTeamSpec = jlgService.GetTeamSpecByTeamId(homeTeamId);
            jlgTeamTrendsAtPieViewModel.AwayTeamSpec = jlgService.GetTeamSpecByTeamId(awayTeamId);

            var homeTeamStats = GetTeamStats(homeTeamId);
            var awayTeamStats = GetTeamStats(awayTeamId);

            jlgTeamTrendsAtPieViewModel.HomeChartAtGoalPattern = PickChartAtGoalPattern(homeTeamStats, "HomePieChartAtGoalPattern", "LoadHomePieChartAtGoalPattern");
            jlgTeamTrendsAtPieViewModel.HomeChartAtLostPattern = PickChartAtLostPattern(homeTeamStats, "HomePieChartAtLostPattern", "LoadHomePieChartAtLostPattern");

            jlgTeamTrendsAtPieViewModel.AwayChartAtGoalPattern = PickChartAtGoalPattern(awayTeamStats, "AwayPieChartAtGoalPattern", "LoadAwayPieChartAtGoalPattern");
            jlgTeamTrendsAtPieViewModel.AwayChartAtLostPattern = PickChartAtLostPattern(awayTeamStats, "AwayPieChartAtLostPattern", "LoadAwayPieChartAtLostPattern");

            var homeGoalTimeZoneList = GetGoalTimeZoneList(homeTeamId);
            var homeGoalTimeZoneFirst = homeGoalTimeZoneList.Where(m => m.TimeZoneDivision == (int)JlgChartConst.TimeZoneDivision.First).FirstOrDefault();
            var homeGoalTimeZoneSecond = homeGoalTimeZoneList.Where(m => m.TimeZoneDivision == (int)JlgChartConst.TimeZoneDivision.Second).FirstOrDefault();

            var awayGoalTimeZoneList = GetGoalTimeZoneList(awayTeamId);
            var awayGoalTimeZoneFirst = awayGoalTimeZoneList.Where(m => m.TimeZoneDivision == (int)JlgChartConst.TimeZoneDivision.First).FirstOrDefault();
            var awayGoalTimeZoneSecond = awayGoalTimeZoneList.Where(m => m.TimeZoneDivision == (int)JlgChartConst.TimeZoneDivision.Second).FirstOrDefault();

            jlgTeamTrendsAtPieViewModel.HomeChartAtGoalGroupByTimeZone = PickChartAtGoalTimeZone(homeGoalTimeZoneFirst, homeGoalTimeZoneSecond, "HomePieChartAtGoalTimeZone", "LoadHomePieChartAtGoalTimeZone");
            jlgTeamTrendsAtPieViewModel.HomeChartAtLostGroupByTimeZone = PickChartAtLostTimeZone(homeGoalTimeZoneFirst, homeGoalTimeZoneSecond, "HomePieChartAtLostTimeZone", "LoadHomePieChartAtLostTimeZone");

            jlgTeamTrendsAtPieViewModel.AwayChartAtGoalGroupByTimeZone = PickChartAtGoalTimeZone(awayGoalTimeZoneFirst, awayGoalTimeZoneSecond, "AwayPieChartAtGoalTimeZone", "LoadAwayPieChartAtGoalTimeZone");
            jlgTeamTrendsAtPieViewModel.AwayChartAtLostGroupByTimeZone = PickChartAtLostTimeZone(awayGoalTimeZoneFirst, awayGoalTimeZoneSecond, "AwayPieChartAtLostTimeZone", "LoadAwayPieChartAtLostTimeZone");

            jlgTeamTrendsAtPieViewModel.HomeChartAtPassSucceedAverage = PickChartAtPassSucceedAverage(homeTeamStats, "HomePieChartAtPassSucceedAverage", "LoadHomePieChartAtPassSucceedAverage");
            jlgTeamTrendsAtPieViewModel.HomeChartAtPassPattern = PickChartAtPassPattern(homeTeamStats, "HomePieChartAtPassPattern", "LoadHomePieChartAtPassPattern");

            jlgTeamTrendsAtPieViewModel.AwayChartAtPassSucceedAverage = PickChartAtPassSucceedAverage(awayTeamStats, "AwayPieChartAtPassSucceedAverage", "LoadAwayPieChartAtPassSucceedAverage");
            jlgTeamTrendsAtPieViewModel.AwayChartAtPassPattern = PickChartAtPassPattern(awayTeamStats, "AwayPieChartAtPassPattern", "LoadAwayPieChartAtPassPattern");

            return jlgTeamTrendsAtPieViewModel;
        }

        /// <summary>
        /// 得失点指標取得
        /// </summary>
        private TeamStats GetTeamStats(int teamId)
        {
            return 
                (
                    from stats in JlgEntities.TeamSeasonStats
                    where (stats.TeamID == teamId)
                    select new TeamStats
                    {
                        GoalIndex = new GoalIndex()
                        {
                            AtPk = stats.PKGoalCount,
                            AtSetPlayDirectly = stats.SetPlayDirectGoalCount,
                            AtSetPlay = stats.SetPlayGoalCount,
                            AtCross = stats.CrossGoalCount,
                            AtThroughPass = stats.ThroughPassGoalCount,
                            AtShortPass = stats.ShortPassGoalCount,
                            AtLongPass = stats.LongPassGoalCount,
                            AtDribble = stats.DribbleGoalCount,
                            AtLooseBall = stats.LooseBallGoalCount,
                            AtOther = stats.OtherGoalCount
                        },
                        LostIndex = new GoalIndex()
                        {
                            AtPk = stats.PKLostCount,
                            AtSetPlayDirectly = stats.SetPlayDirectLostCount,
                            AtSetPlay = stats.SetPlayLostCount,
                            AtCross = stats.CrossLostCount,
                            AtThroughPass = stats.ThroughPassLostCount,
                            AtShortPass = stats.ShortPassLostCount,
                            AtLongPass = stats.LongPassLostCount,
                            AtDribble = stats.DribbleLostCount,
                            AtLooseBall = stats.LooseBallLostCount,
                            AtOther = stats.OtherLostCount
                        },
                        PassIndex = new PassIndex() 
                        { 
                            Pass = stats.PassCount,
                            PassSucceed = stats.PassSuccessCount,
                            ShortPassCount = stats.ShortPassCount,
                            MiddlePassCount = stats.MiddlePassCount,
                            LongPassCount = stats.LongPassCount
                        }
                    }
                  ).FirstOrDefault();
        }

        /// <summary>
        /// 時間帯別得失点指標リスト取得
        /// </summary>
        private IEnumerable<GoalTimeZone> GetGoalTimeZoneList(int teamId)
        {
            return (
                        from head in JlgEntities.TeamInfoTT
                        join body in JlgEntities.ResultInfoTT on head.TeamInfoTTId equals body.TeamInfoTTId
                        where (head.TeamID == teamId)
                        select new GoalTimeZone
                        {
                            TimeZoneDivision = body.TimeDivision,
                            Goal = body.Goal,
                            Lost = body.Lost

                        }
                  ).ToList();
        }

        /// <summary>
        /// 得点パターンチャートDto抽出
        /// </summary>
        private PieChartDto PickChartAtGoalPattern(TeamStats teamStats, string viewContainerId, string functionName)
        {
             var tmp = new []
             {
                new {Label = JlgChartConst.GoalIndexAtPkLabel, X = 0, Y = teamStats.GoalIndex.AtPk, Value = 0},
                new {Label = JlgChartConst.GoalIndexAtSetPlayDirectlyLabel, X = 0, Y = teamStats.GoalIndex.AtSetPlayDirectly, Value = 0 },
                new {Label = JlgChartConst.GoalIndexAtSetPlayLabel, X = 0, Y = teamStats.GoalIndex.AtSetPlay, Value = 0},
                new {Label = JlgChartConst.GoalIndexAtCrossLabel, X = 0, Y = teamStats.GoalIndex.AtCross, Value = 0},
                new {Label = JlgChartConst.GoalIndexAtThroughPassLabel, X = 0, Y = teamStats.GoalIndex.AtThroughPass, Value = 0 },
                new {Label = JlgChartConst.GoalIndexAtShortPassLabel, X = 0, Y = teamStats.GoalIndex.AtShortPass, Value = 0  },
                new {Label = JlgChartConst.GoalIndexAtLongPassLabel, X = 0, Y = teamStats.GoalIndex.AtLongPass, Value = 0 },
                new {Label = JlgChartConst.GoalIndexAtDribbleLabel, X = 0, Y = teamStats.GoalIndex.AtDribble, Value = 0},
                new {Label = JlgChartConst.GoalIndexAtLooseBallLabel,X = 0, Y = teamStats.GoalIndex.AtLooseBall, Value = 0},
                new {Label = JlgChartConst.GoalIndexAtOtherLabel,X = 0, Y = teamStats.GoalIndex.AtOther, Value = 0}  
             };

            var items = tmp.OrderByDescending(m => m.Y).ToList();

            var pieChartDto = new PieChartDto()
            {
                FunctionName = functionName,
                PieChartContainer = new PieChartContainer()
                                        {
                                            ViewContainerId = viewContainerId,

                                            //Todo:コンテナ設定用CssClass
                                            ViewContainerClass = JlgChartConst.ChartCssClassName.pie_chart.ToString(),
                                            DataSources = new List<PieDataSource>()
                                            {
                                                new PieDataSource(){ Label = items[0].Label, X = items[0].X, Y = items[0].Y, Value = items[0].Value },
                                                new PieDataSource(){ Label = items[1].Label, X = items[1].X, Y = items[1].Y, Value = items[1].Value },
                                                new PieDataSource(){ Label = items[2].Label, X = items[2].X, Y = items[2].Y, Value = items[2].Value },
                                                new PieDataSource(){ Label = items[3].Label, X = items[3].X, Y = items[3].Y, Value = items[3].Value },
                                                new PieDataSource(){ Label = items[4].Label, X = items[4].X, Y = items[4].Y, Value = items[4].Value },
                                                new PieDataSource(){ Label = items[5].Label, X = items[5].X, Y = items[5].Y, Value = items[5].Value },
                                                new PieDataSource(){ Label = items[6].Label, X = items[6].X, Y = items[6].Y, Value = items[6].Value },
                                                new PieDataSource(){ Label = items[7].Label, X = items[7].X, Y = items[7].Y, Value = items[7].Value },
                                                new PieDataSource(){ Label = items[8].Label, X = items[8].X, Y = items[8].Y, Value = items[8].Value },
                                                new PieDataSource(){ Label = items[9].Label, X = items[9].X, Y = items[9].Y, Value = items[9].Value },
                                            }
                                        },
                PieChartProperties = new ChartProperties()
                                         {
                                             IsHtmlText = false,
                                             ShadowSize = 0.5m,
                                             FontSize = 10.5m,
                                             Resolution = 2,
                                             Colors = new List<string>() { "#550000","#d5006a","#aa0000","#ff8080","#ffd5d5","#800000","#ff5555","#d50000","#ffaaaa","#ff0000" },
                                             Legend = new Legend() { position = Flotr2Const.LegendPosition.se, BackgroundColor = "#FFF", BorderColor = "#AAA" },
                                             Pie = new Pie() { IsVisible = true, Explode = 0, SizeRatio = 0.9m, FillOpacity = 0.8m },
                                             XAxis = new XAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                                             YAxis = new YAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                                             Grid = new Grid() { IsVisibleHorizontalLines = false, IsVisibleVerticalLines = false, BackGroundImageAddress = string.Empty, GridOutlineWidth = 0 },
                                             Mouse = new Mouse() { IsTrackable = false, TrackFormatter = string.Empty },
                                             Bubble = new Bubble() { IsVisible = false, BaseRadius = 0m },
                                             Marker = new Marker() { IsVisible = false, FontSize = 10, IsRelative = false, LabelFormatter = string.Empty },
                                         }
            };
      
            return pieChartDto;
        }

        /// <summary>
        /// 失点パターンチャートDto抽出
        /// </summary>
        private PieChartDto PickChartAtLostPattern(TeamStats teamStats, string viewContainerId, string functionName)
        {
            var tmp = new[]{
                new {Label = JlgChartConst.GoalIndexAtPkLabel, X = 0, Y = teamStats.LostIndex.AtPk, Value = 0},
                new {Label = JlgChartConst.GoalIndexAtSetPlayDirectlyLabel, X = 0, Y = teamStats.LostIndex.AtSetPlayDirectly, Value = 0 },
                new {Label = JlgChartConst.GoalIndexAtSetPlayLabel, X = 0, Y = teamStats.LostIndex.AtSetPlay, Value = 0},
                new {Label = JlgChartConst.GoalIndexAtCrossLabel, X = 0, Y = teamStats.LostIndex.AtCross, Value = 0},
                new {Label = JlgChartConst.GoalIndexAtThroughPassLabel, X = 0, Y = teamStats.LostIndex.AtThroughPass, Value = 0 },
                new {Label = JlgChartConst.GoalIndexAtShortPassLabel, X = 0, Y = teamStats.LostIndex.AtShortPass, Value = 0  },
                new {Label = JlgChartConst.GoalIndexAtLongPassLabel, X = 0, Y = teamStats.LostIndex.AtLongPass, Value = 0 },
                new {Label = JlgChartConst.GoalIndexAtDribbleLabel, X = 0, Y = teamStats.LostIndex.AtDribble, Value = 0},
                new {Label = JlgChartConst.GoalIndexAtLooseBallLabel,X = 0, Y = teamStats.LostIndex.AtLooseBall, Value = 0},
                new {Label = JlgChartConst.GoalIndexAtOtherLabel,X = 0, Y = teamStats.LostIndex.AtOther, Value = 0}  
             };

            var items = tmp.OrderByDescending(m => m.Y).ToList();

            var pieChartDto = new PieChartDto()
            {
                FunctionName = functionName,
                PieChartContainer = new PieChartContainer()
                {
                    ViewContainerId = viewContainerId,

                    //Todo:コンテナ設定用CssClass
                    ViewContainerClass = JlgChartConst.ChartCssClassName.pie_chart.ToString(),
                    DataSources = new List<PieDataSource>()
                    {
                        new PieDataSource(){ Label = items[0].Label, X = items[0].X, Y = items[0].Y, Value = items[0].Value },
                        new PieDataSource(){ Label = items[1].Label, X = items[1].X, Y = items[1].Y, Value = items[1].Value },
                        new PieDataSource(){ Label = items[2].Label, X = items[2].X, Y = items[2].Y, Value = items[2].Value },
                        new PieDataSource(){ Label = items[3].Label, X = items[3].X, Y = items[3].Y, Value = items[3].Value },
                        new PieDataSource(){ Label = items[4].Label, X = items[4].X, Y = items[4].Y, Value = items[4].Value },
                        new PieDataSource(){ Label = items[5].Label, X = items[5].X, Y = items[5].Y, Value = items[5].Value },
                        new PieDataSource(){ Label = items[6].Label, X = items[6].X, Y = items[6].Y, Value = items[6].Value },
                        new PieDataSource(){ Label = items[7].Label, X = items[7].X, Y = items[7].Y, Value = items[7].Value },
                        new PieDataSource(){ Label = items[8].Label, X = items[8].X, Y = items[8].Y, Value = items[8].Value },
                        new PieDataSource(){ Label = items[9].Label, X = items[9].X, Y = items[9].Y, Value = items[9].Value },
                    }
                },
                PieChartProperties = new ChartProperties()
                {
                    IsHtmlText = false,
                    ShadowSize = 0.5m,
                    FontSize = 10.5m,
                    Resolution = 2,
                    Colors = new List<string>() { "#002b55", "#2b95ff", "#0055aa", "#80bfff", "#d5eaff", "#004080", "#55aaff", "#006ad5", "#aad5ff", "#0080ff" },
                    Legend = new Legend() { position = Flotr2Const.LegendPosition.se, BackgroundColor = "#FFF", BorderColor = "#AAA" },
                    Pie = new Pie() { IsVisible = true, Explode = 0, SizeRatio = 0.9m, FillOpacity = 0.8m },
                    XAxis = new XAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                    YAxis = new YAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                    Grid = new Grid() { IsVisibleHorizontalLines = false, IsVisibleVerticalLines = false, BackGroundImageAddress = string.Empty, GridOutlineWidth = 0 },
                    Mouse = new Mouse() { IsTrackable = false, TrackFormatter = string.Empty },
                    Bubble = new Bubble() { IsVisible = false, BaseRadius = 0m },
                    Marker = new Marker() { IsVisible = false, FontSize = 10, IsRelative = false, LabelFormatter = string.Empty },
                }
            };

            return pieChartDto;
        }

        /// <summary>
        /// 時間帯別得点数チャートDto抽出
        /// </summary>
        private PieChartDto PickChartAtGoalTimeZone(GoalTimeZone first, GoalTimeZone second, string viewContainerId, string functionName)
        {
            var tmp = new[]{
                new {Label = JlgChartConst.GoalTimezoneAtFirstLabel, X = 0, Y = first.Goal, Value = 0},
                new {Label = JlgChartConst.GoalTimezoneAtSecondLabel, X = 0, Y = second.Goal, Value = 0 },
             };

            var items = tmp.OrderByDescending(m => m.Y).ToList();

            var pieChartDto = new PieChartDto()
            {
                FunctionName = functionName,
                PieChartContainer = new PieChartContainer()
                {
                    ViewContainerId = viewContainerId,

                    //Todo:コンテナ設定用CssClass
                    ViewContainerClass = JlgChartConst.ChartCssClassName.pie_chart.ToString(),
                    DataSources = new List<PieDataSource>()
                                      {
                                          new PieDataSource(){ Label = items[0].Label, X = items[0].X, Y = items[0].Y, Value = items[0].Value },
                                          new PieDataSource(){ Label = items[1].Label, X = items[1].X, Y = items[1].Y, Value = items[1].Value },  
                                      }
                },
                PieChartProperties = new ChartProperties()
                {
                    IsHtmlText = false,
                    ShadowSize = 0.5m,
                    FontSize = 10.5m,
                    Resolution = 2,
                    Colors = new List<string>() { "#ff5555", "#ff8080"},
                    Legend = new Legend() { position = Flotr2Const.LegendPosition.se, BackgroundColor = "#FFF", BorderColor = "#AAA" },
                    Pie = new Pie() { IsVisible = true, Explode = 0, SizeRatio = 0.9m, FillOpacity = 0.8m },
                    XAxis = new XAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                    YAxis = new YAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                    Grid = new Grid() { IsVisibleHorizontalLines = false, IsVisibleVerticalLines = false, BackGroundImageAddress = string.Empty, GridOutlineWidth = 0 },
                    Mouse = new Mouse() { IsTrackable = false, TrackFormatter = string.Empty },
                    Bubble = new Bubble() { IsVisible = false, BaseRadius = 0m },
                    Marker = new Marker() { IsVisible = false, FontSize = 10, IsRelative = false, LabelFormatter = string.Empty },
                }
            };

            return pieChartDto;
        }

        /// <summary>
        /// 時間帯別失点数チャートDto抽出
        /// </summary>
        private PieChartDto PickChartAtLostTimeZone(GoalTimeZone first, GoalTimeZone second, string viewContainerId, string functionName)
        {
            var tmp = new[]{
                new {Label = JlgChartConst.GoalTimezoneAtFirstLabel, X = 0, Y = first.Lost, Value = 0},
                new {Label = JlgChartConst.GoalTimezoneAtSecondLabel, X = 0, Y = second.Lost, Value = 0 },
             };

            var items = tmp.OrderByDescending(m => m.Y).ToList();

            var pieChartDto = new PieChartDto()
            {
                FunctionName = functionName,
                PieChartContainer = new PieChartContainer()
                {
                    ViewContainerId = viewContainerId,

                    //Todo:コンテナ設定用CssClass
                    ViewContainerClass = JlgChartConst.ChartCssClassName.pie_chart.ToString(),
                    DataSources = new List<PieDataSource>()
                                      {
                                          new PieDataSource(){ Label = items[0].Label, X = items[0].X, Y = items[0].Y, Value = items[0].Value },
                                          new PieDataSource(){ Label = items[1].Label, X = items[1].X, Y = items[1].Y, Value = items[1].Value },  
                                      }
                },
                PieChartProperties = new ChartProperties()
                {
                    IsHtmlText = false,
                    ShadowSize = 0.5m,
                    FontSize = 10.5m,
                    Resolution = 2,
                    Colors = new List<string>() { "#0080ff", "#aad5ff" },
                    Legend = new Legend() { position = Flotr2Const.LegendPosition.se, BackgroundColor = "#FFF", BorderColor = "#AAA" },
                    Pie = new Pie() { IsVisible = true, Explode = 0, SizeRatio = 0.9m, FillOpacity = 0.8m },
                    XAxis = new XAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                    YAxis = new YAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                    Grid = new Grid() { IsVisibleHorizontalLines = false, IsVisibleVerticalLines = false, BackGroundImageAddress = string.Empty, GridOutlineWidth = 0 },
                    Mouse = new Mouse() { IsTrackable = false, TrackFormatter = string.Empty },
                    Bubble = new Bubble() { IsVisible = false, BaseRadius = 0m },
                    Marker = new Marker() { IsVisible = false, FontSize = 10, IsRelative = false, LabelFormatter = string.Empty },
                }
            };

            return pieChartDto;
        }

        /// <summary>
        /// パス成功率用Dto抽出
        /// </summary>
        private PieChartDto PickChartAtPassSucceedAverage(TeamStats teamStats, string viewContainerId, string functionName)
        {
            var tmp = new[]{
                new {Label = JlgChartConst.PassSucceedAtSuccessLabel, X = 0, Y = teamStats.PassIndex.PassSucceed, Value = 0},
                new {Label = JlgChartConst.PassSucceedAtFailedLabel, X = 0, Y = (teamStats.PassIndex.PassFailed), Value = 0 },
             };

            var items = tmp.OrderByDescending(m => m.Y).ToList();

            var pieChartDto = new PieChartDto()
            {
                FunctionName = functionName,
                PieChartContainer = new PieChartContainer()
                {
                    ViewContainerId = viewContainerId,
                    ViewContainerClass = JlgChartConst.ChartCssClassName.pie_chart.ToString(),
                    DataSources = new List<PieDataSource>()
                                      {
                                          new PieDataSource(){ Label = items[0].Label, X = items[0].X, Y = items[0].Y, Value = items[0].Value },
                                          new PieDataSource(){ Label = items[1].Label, X = items[1].X, Y = items[1].Y, Value = items[1].Value },
                                      }
                },
                PieChartProperties = new ChartProperties()
                {
                    IsHtmlText = false,
                    ShadowSize = 0.5m,
                    FontSize = 10.5m,
                    Resolution = 2,
                    Colors = new List<string>() {"#00ff00", "#005500"},
                    Legend = new Legend() { position = Flotr2Const.LegendPosition.se, BackgroundColor = "#FFF", BorderColor = "#AAA" },
                    Pie = new Pie() { IsVisible = true, Explode = 0, SizeRatio = 0.9m, FillOpacity = 0.8m },
                    XAxis = new XAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                    YAxis = new YAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                    Grid = new Grid() { IsVisibleHorizontalLines = false, IsVisibleVerticalLines = false, BackGroundImageAddress = string.Empty, GridOutlineWidth = 0 },
                    Mouse = new Mouse() { IsTrackable = false, TrackFormatter = string.Empty },
                    Bubble = new Bubble() { IsVisible = false, BaseRadius = 0m },
                    Marker = new Marker() { IsVisible = false, FontSize = 10, IsRelative = false, LabelFormatter = string.Empty },
                }
            };

            return pieChartDto;
        }

        /// <summary>
        /// パス割合Dto抽出
        /// </summary>
        private PieChartDto PickChartAtPassPattern(TeamStats teamStats, string viewContainerId, string functionName)
        {
            var tmp = new[]{
                new {Label = JlgChartConst.PassPatternAtLongPassLabel, X = 0, Y = teamStats.PassIndex.LongPassCount, Value = 0},
                new {Label = JlgChartConst.PassPatternAtMiddlePassLabel, X = 0, Y = (teamStats.PassIndex.MiddlePassCount), Value = 0 },
                new {Label = JlgChartConst.PassPatternAtShortPassLabel, X = 0, Y = (teamStats.PassIndex.ShortPassCount), Value = 0 },
             };

            var items = tmp.OrderByDescending(m => m.Y).ToList();

            var pieChartDto = new PieChartDto()
            {
                FunctionName = functionName,
                PieChartContainer = new PieChartContainer()
                {
                    ViewContainerId = viewContainerId,
                    ViewContainerClass = JlgChartConst.ChartCssClassName.pie_chart.ToString(),
                    DataSources = new List<PieDataSource>()
                                      {
                                          new PieDataSource(){ Label = items[0].Label, X = items[0].X, Y = items[0].Y, Value = items[0].Value },
                                          new PieDataSource(){ Label = items[1].Label, X = items[1].X, Y = items[1].Y, Value = items[1].Value },
                                          new PieDataSource(){ Label = items[2].Label, X = items[2].X, Y = items[2].Y, Value = items[2].Value },
                                      }
                },
                PieChartProperties = new ChartProperties()
                {
                    IsHtmlText = false,
                    ShadowSize = 0.5m,
                    FontSize = 10.5m,
                    Resolution = 2,
                    Colors = new List<string>() { "#00ff00", "#00aa00","#aaffaa" },
                    Legend = new Legend() { position = Flotr2Const.LegendPosition.se, BackgroundColor = "#FFF", BorderColor = "#AAA" },
                    Pie = new Pie() { IsVisible = true, Explode = 0, SizeRatio = 0.9m, FillOpacity = 0.8m },
                    XAxis = new XAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                    YAxis = new YAxis() { MinValue = 0, MaxValue = 1, IsVisibleLabels = false },
                    Grid = new Grid() { IsVisibleHorizontalLines = false, IsVisibleVerticalLines = false, BackGroundImageAddress = string.Empty, GridOutlineWidth = 0 },
                    Mouse = new Mouse() { IsTrackable = false, TrackFormatter = string.Empty },
                    Bubble = new Bubble() { IsVisible = false, BaseRadius = 0m },
                    Marker = new Marker() { IsVisible = false, FontSize = 10, IsRelative = false, LabelFormatter = string.Empty },
                }
            };

            return pieChartDto;
        }

        #endregion
    }
}
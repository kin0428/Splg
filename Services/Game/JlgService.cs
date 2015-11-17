using Splg.Areas.Jleague;
using System.Collections.Generic;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Splg.Core.Constant;
using Splg.Areas.Jleague.Models.Dto;
using Splg.Core.Models.Flotr2.Dto.Shared;
using Splg.Core.Extend;
using Splg.Models.News.ViewModel;
using Splg.Core.Models.Flotr2.Dto.Shared.Properties;


using Splg.Core.Models.Flotr2.Dto.FormationChart;

namespace Splg.Services.Game
{
    public class JlgService : IGameService
    {
        #region Property
        /// <summary>
        ///Jlg用Entity
        /// </summary>
        JlgEntities JlgEntities = new JlgEntities();

        /// <summary>
        /// Com用Entity
        /// </summary>
        ComEntities ComEntities = new ComEntities();
        #endregion

        #region method
        public int GetOccasionNo(int dateInput, int gameKindId)
        {
            var query = (from si in JlgEntities.ScheduleInfo
                         join gcat in JlgEntities.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                         join gs in JlgEntities.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                         where (si.GameDate >= dateInput)
                         where gs.GameKindID == gameKindId
                         orderby si.GameDate
                         select si.OccasionNo).FirstOrDefault();

            return (query == null) ? 0 :(int)query;
        }

        public int GetSeasonId(int dateInput, int gameKindId)
        {
            var query = (from scheduleInfo in JlgEntities.ScheduleInfo
                         join gameCategory in JlgEntities.GameCategory on scheduleInfo.GameCategoryId equals gameCategory.GameCategoryId
                         join gameSchedule in JlgEntities.GameSchedule on gameCategory.GameScheduleId equals gameSchedule.GameScheduleId
                         where (scheduleInfo.GameDate >= dateInput)
                         where gameSchedule.GameKindID == gameKindId
                         orderby scheduleInfo.GameDate
                         select gameCategory).FirstOrDefault();

            return (query == null) ? 0 : query.SeasonID;
        }

        /// <summary>
        /// JlgType取得
        /// </summary>
        /// <remarks>
        /// Enum使いたいので、スプラウトメソッド作成。
        /// Todo:旧版の破棄(利用箇所が多いので、順次対応)
        /// </remarks>
        public JlgConst.JType GetJlgType(string url)
        {
            var target = url.ToLower();

            if (target.Contains("/j1"))
            {
                return JlgConst.JType.J1;
            }

            if (target.Contains("/j2"))
            {
                return JlgConst.JType.J2;
            }

            if (target.Contains("/jleaguecup"))
            {
                return JlgConst.JType.Jleaguecup;
            }

            return JlgConst.JType.Empty;
        }

        /// <summary>
        /// 直近の試合結果取得
        /// </summary>
        public IEnumerable<JlgRecentGameResult> GetRecentGameResults(int teamId, int gameId)
        {            
            if (teamId == 0)
            {
                return default(IEnumerable<JlgRecentGameResult>);
            }

            return  
                (
                    from schedule in JlgEntities.ScheduleInfo
                    join home in JlgEntities.ResultInfoT5 on new { gameDate = (int)schedule.GameDate, teamId = schedule.HomeTeamID } equals new { gameDate = (home.GameDate), teamId = home.ID }
                    join away in JlgEntities.ResultInfoT5 on new { gameDate = (int)schedule.GameDate, teamId = schedule.AwayTeamID } equals new { gameDate = (away.GameDate), teamId = away.ID }
                    where (schedule.GameID < gameId)
                    where (schedule.HomeTeamID == (int?)teamId || schedule.AwayTeamID == (int?)teamId)
                    select new JlgRecentGameResult
                    {
                        GameDate = schedule.GameDate,
                        GameId = schedule.GameID,
                        HomeTeamId = home.ID,
                        HomeTeamShortName = home.NameS,
                        HomeScore = home.Score,
                        AwayTeamId = away.ID,
                        AwayTeamShortName = away.NameS,
                        AwayScore = away.Score
                    }
                ).OrderByDescending(schedule => schedule.GameDate).Take(5).ToList();
        }

        /// <summary>
        /// 直近の直接対決結果取得
        /// </summary>
        public IEnumerable<JlgRecentMatches> GetRecentMatches(int HomeTeamId, int AwayTeamId, int gameDate)
        {
            if (HomeTeamId == 0 || AwayTeamId == 0 || gameDate == 0)
            {
                return default(IEnumerable<JlgRecentMatches>);
            }

            return (
                        from result in JlgEntities.ResultInfoHE
                        where (result.GameDate < gameDate)
                        where (
                                (result.HomeTeamID == HomeTeamId && result.AwayTeamID == AwayTeamId) ||
                                (result.HomeTeamID == AwayTeamId && result.AwayTeamID == HomeTeamId)
                              )
                        select new JlgRecentMatches
                        {
                            GameDate = result.GameDate,
                            HomeTeamId = result.HomeTeamID,
                            HomeTeamShortName = result.HomeTeamNameS,
                            HomeScore = result.HomeScore,
                            AwayTeamId = result.AwayTeamID,
                            AwayTeamShortName = result.AwayTeamNameS,
                            AwayScore = result.AwayScore
                        }
                    ).Distinct().OrderByDescending(result => result.GameDate).Take(5).ToList();
        }

        /// <summary>
        /// チーム仕様取得（Key:チームId）
        /// </summary>
        public JlgTeamSpec GetTeamSpecByTeamId(int teamId,int gameId = 0)
        {
            var result = new JlgTeamSpec(); 
            result = 
                (
                    from teamInfoTE in JlgEntities.TeamInfoTE
                    join teamCBP in JlgEntities.TeamCBP on teamInfoTE.TeamID equals teamCBP.TeamID 
                    join teamIconJlg in JlgEntities.TeamIconJlg on teamInfoTE.TeamID equals teamIconJlg.TeamCD into tmIcon
                    from teamIcon in tmIcon.DefaultIfEmpty()
                    where teamInfoTE.TeamID == teamId
                    select new JlgTeamSpec
                    {
                        TeamId = teamInfoTE.TeamID,
                        TeamName = teamInfoTE.TeamName,
                        TeamShortName = teamInfoTE.TeamNameS,
                        TeamIconUrl = teamIcon.TeamIcon,
                        TeamAttackCBP = teamCBP.AttackCBP,
                        TeamDefenseCBP = teamCBP.DefenseCBP
                    }
                ).FirstOrDefault();
            if (gameId > 0)
            {
                result.FormationName = GetFormationName(teamId, gameId);
            }

            return result;
        }

        /// <summary>
        /// ホームチーム直近試合結果計算処理
        /// </summary>
        public JlgGameResultCounts CalculateJlgRecentGameResultCounts(int TargetTeamId, IEnumerable<IJlgRecentResults> targetRecentGameResults)
        {
            var jlgRecentGameResultCounts = new JlgGameResultCounts();


            var homeItems = targetRecentGameResults.Where(m => m.HomeTeamId == TargetTeamId).ToList();

            foreach (var item in homeItems)
            {
                IncrementRecentGameResultCounts(jlgRecentGameResultCounts, item.HomeScore.Value, item.AwayScore.Value);
            }

            var awayItems = targetRecentGameResults.Where(m => m.AwayTeamId == TargetTeamId).ToList();

            foreach (var item in awayItems)
            {
                IncrementRecentGameResultCounts(jlgRecentGameResultCounts, item.AwayScore.Value, item.HomeScore.Value);
            }

            return jlgRecentGameResultCounts;
        }

        /// <summary>
        /// 直近試合結果インクリメント
        /// </summary>
        private static void IncrementRecentGameResultCounts(JlgGameResultCounts jlgRecentGameResultCounts, int ownScore, int enemiesScore)
        {
            if (ownScore > enemiesScore)
            {
                jlgRecentGameResultCounts.WinCount += 1;
            }
            else if (ownScore == enemiesScore)
            {
                jlgRecentGameResultCounts.DrawCount += 1;
            }
            else if (ownScore < enemiesScore)
            {
                jlgRecentGameResultCounts.LoseCount += 1;
            }
        }

        /// <summary>
        /// チームの傾向（棒グラフ部分）取得
        /// </summary>
        public JlgTeamTrendsAtBar GetTeamTrendsAtBar(int teamId,int gameDate)
        { 
            var jlgTeamTrendsAtBar =  new JlgTeamTrendsAtBar();

            var teamGameStats = GetTeamGameStatsByTeamIdAndGameDate(teamId, gameDate);

            jlgTeamTrendsAtBar.HomeWinCount = teamGameStats.Where(m => m.HomeAwayClass == 1 && m.GameResultID == 1).Count();

            jlgTeamTrendsAtBar.AwayWinCount = teamGameStats.Where(m => m.HomeAwayClass == 2 && m.GameResultID == 1).Count();

            jlgTeamTrendsAtBar.PossessionAverage = (int)teamGameStats.Average(m => m.PossessionRatio);
            
            jlgTeamTrendsAtBar.LeftCrossCount = teamGameStats.Select(m => m.LeftSideCrossCount).Sum();
            
            jlgTeamTrendsAtBar.RightCrossCount = teamGameStats.Select(m => m.RightSideCrossCount).Sum();
            
            jlgTeamTrendsAtBar.InterceptCount = teamGameStats.Select(m => m.InterceptCount).Sum();
            
            jlgTeamTrendsAtBar.NationalExperiencedCount = GetNationalExperiencedCountByTeamId(teamId);

            var playerInfoDI = GetPlayerInfoDIByTeamId(teamId);

            jlgTeamTrendsAtBar.AgeAverage = (int)playerInfoDI.Where(m => m.Birthday != null).Average(m => m.Birthday.Value.ParseDatetime().GetYearDiff(gameDate.ParseDatetime()));

            jlgTeamTrendsAtBar.BodyHeightAverage = (int)playerInfoDI.Where(m => m.Height != null).Average(m => (int)m.Height);

            return jlgTeamTrendsAtBar;
        
        }

        /// <summary>
        /// 試合スタッツ取得（Key:チームId,ゲーム日付）
        /// </summary>
        public IEnumerable<TeamGameStats> GetTeamGameStatsByTeamIdAndGameDate(int teamId, int gameDate)
        {
            return JlgEntities.TeamGameStats.Where(m => m.TeamID == teamId && m.GameDate < gameDate).ToList();
        }

        /// <summary>
        /// 代表経験者数取得
        /// </summary>
        private int GetNationalExperiencedCountByTeamId(int teamId)
        {
            return  
                (   
                    from head in JlgEntities.DirectoryDA
                    join body in JlgEntities.PlayerInfoDA on head.DirectoryDAId equals body.DirectoryDAId
                    where (head.TeamID == teamId && body.Acap > 0 && body.Class == 1)
                    select body       
                ).Count();
        }

        /// <summary>
        /// 選手年鑑取得
        /// </summary>
        private IEnumerable<PlayerInfoDI> GetPlayerInfoDIByTeamId(int teamId)
        {
            return 
                (
                    from head in JlgEntities.DirectoryDI
                    join body in JlgEntities.PlayerInfoDI on head.DirectoryDIId equals body.DirectoryDIId
                    where (head.TeamID == teamId)
                    select body
                ).ToList();
        }

        /// <summary>
        /// チーム用関連記事取得
        /// </summary>
        public List<RelatedArticle> GetRelatedArticles(int teamId)
        {
            //ニュース最新5件取得
            var teamFiveNews = GetTeamNewsList(teamId, JlgConstants.JLG_SPORT_ID, Constants.TEAM_TOPIC_CLASSIFICATION).OrderByDescending(m => m.DeliveryDate).Take(5).ToList();

            //投稿記事最新5件取得
            var teamFivePosted = Splg.Controllers.PostedController.GetRecentPosts(3, Constants.JLG_SPORT_ID, teamId, 1).OrderByDescending(m => m.ContributeDate).Take(5).ToList();

            var articleMerge = new List<RelatedArticle>();

            articleMerge.AddRange(teamFiveNews.Select(m => new RelatedArticle { ArticleKind = JlgConst.ArticleKind.News, PublishedDate = m.DeliveryDate, Title = m.Headline, ArticleRouteName = RouteNameConst.NewsDetail, IconsUrl = NewsConst.SpologNewsIconUrl, IconsRouteName = string.Empty, Key = m.NewsItemID, ArticleOwner = m.SentFrom, TopicId = 0, OwnersMemberId = 0 }).ToList());

            articleMerge.AddRange(teamFivePosted.Select(m => new RelatedArticle { ArticleKind = JlgConst.ArticleKind.UserPosts, PublishedDate = (DateTime)m.ContributeDate, Title = m.Title, ArticleRouteName = RouteNameConst.UserArticleByContributeId, IconsUrl = m.ProfileImg, IconsRouteName = RouteNameConst.UserTopIndex, Key = m.ContributeId, ArticleOwner = m.Nickname, TopicId = m.TopicID, OwnersMemberId = m.MemberId }).ToList());

            return articleMerge.OrderByDescending(m => m.PublishedDate).Take(5).ToList();
         }

        /// <summary>
        /// チームニュースリスト取得
        /// </summary>
        private IEnumerable<NewsInfoViewModel> GetTeamNewsList(int teamId, int sportID, int classification)
        {
            IEnumerable<NewsInfoViewModel> result = default(IEnumerable<NewsInfoViewModel>);

            var query = (from brief in ComEntities.BriefNews
                         join topic in ComEntities.NewsTopic on brief.NewsItemID equals topic.NewsItemID
                         join tm in ComEntities.TopicMaster on topic.TopicID equals tm.TopicID
                         join photo in ComEntities.PhotoNews on brief.NewsItemID equals photo.NewsItemID into br_photo
                         //join itpc in news.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                         from tmp in br_photo.DefaultIfEmpty()
                         where (tm.ClassificationType == classification
                         && tm.SportID == sportID
                         && tm.UniqueID == teamId
                             //&& itpc.IptcSubjectCode == Constants.JLEAGUE_ITPCSUBJECTCODE
                         && brief.Status == Constants.NEWS_VALID_STATUS
                         && brief.CarryLimitDate >= DateTime.Now)
                         select new NewsInfoViewModel
                         {
                             NewsItemID = brief.NewsItemID,
                             DeliveryDate = brief.DeliveryDate,
                             Headline = brief.Headline,
                             newstext = brief.newstext,
                             Content = tmp.Content,
                             Duid = tmp.Duid,
                             SentFrom = brief.SentFrom,
                             SubHeadline = brief.SubHeadline
                         } into news_photo
                         where (news_photo.Duid == Constants.IMAGE_THUMNAIL_DUID || news_photo.Duid == null)
                         select news_photo).Distinct().ToList();
            result = query;
            return result.OrderByDescending(x => x.DeliveryDate);
        }

        /// <summary>
        ///  フォーメーション（With攻撃力）取得
        /// </summary>
        public FormationChartDto GetFormationInfoWithAttack(int GameID)
        {
            // 返り値のための入れ物
            var formationChartDto = new FormationChartDto();

            // ID,classセット
            formationChartDto.FormationChartContainer.ViewContainerId = JlgChartConst.FormationContainerID;
            formationChartDto.FormationChartContainer.ViewContainerClass = JlgChartConst.ChartCssClassName.formation_chart.ToString();
            // 表示用文言(予想）
            if (JlgCommon.GetStatusMatch(GameID.ToString()) == 0)
            {
                formationChartDto.StartingTypeForDisplay = JlgChartConst.ForecastMember;
            }
            else
            {
                formationChartDto.StartingTypeForDisplay = String.Empty;
            }

            // チームID取得
            int homeTeamID = GetHomeTeamID( GameID);
            int awayTeamID = GetAwayTeamID( GameID);
            // チームID投入
            formationChartDto.FormationChartContainer.HomeTeamID = homeTeamID;
            formationChartDto.FormationChartContainer.AwayTeamID = awayTeamID;

            // フォーメーション情報がない場合のGameIDの入れ物
            int homeGameID = GameID;
            int awayGameID = GameID;

            // フォーメーションの情報がまだない場合
            if (JlgEntities.FormationInfo.Where(x => x.GameID == GameID).Count() == 0)
            {
                homeGameID = GetPreGameID(GameID, homeTeamID);
                awayGameID = GetPreGameID(GameID, awayTeamID);
                // 表示用文言(前節)
                formationChartDto.StartingTypeForDisplay = JlgChartConst.PreviousMember;
            }

            // コンテナーのデータ群取得
                // ホームチームのデータ取得
                List<JlgFormationInfo> homeformationInfo = GetJlgFormationInfo(homeGameID, homeTeamID);

                    // ホームチームフォーメーションの配置、攻撃力のデータ取得
                    formationChartDto.FormationChartContainer.HomePositionContainer = GetFormationPositionWithAttack(homeformationInfo);
                    // ホームチームフォーメーションの選手名取得
                    formationChartDto.FormationChartContainer.HomePlayerNameContainer = GetFormationPlayerNameWithAttack(homeformationInfo);                                 
                    // ホームチームフォーメーションの選手背番号取得
                    formationChartDto.FormationChartContainer.HomePlayerNoContainer = GetFormationPlayerNoWithAttack(homeformationInfo);
                    // ホームチームフォーメーションの選手ID取得
                    formationChartDto.FormationChartContainer.HomePlayerIDList = GetFormationPlayerIDList(homeformationInfo);

                // アウェーチームのデータ取得
                List<JlgFormationInfo> awayformationInfo = GetJlgFormationInfo(awayGameID, awayTeamID);

                    // アウェーチームフォーメーションのデータ取得
                    formationChartDto.FormationChartContainer.AwayPositionContainer = GetFormationPositionWithAttack(awayformationInfo);
                    // アウェーチームフォーメーションの選手名取得
                    formationChartDto.FormationChartContainer.AwayPlayerNameContainer = GetFormationPlayerNameWithAttack(awayformationInfo);
                    // アウェーチームフォーメーションの選手背番号取得
                    formationChartDto.FormationChartContainer.AwayPlayerNoContainer = GetFormationPlayerNoWithAttack(awayformationInfo);
                    // アウェーチームフォーメーションの選手ID取得
                    formationChartDto.FormationChartContainer.AwayPlayerIDList = GetFormationPlayerIDList(awayformationInfo);

            // プロパティのデータ群取得
                formationChartDto.FormationChartProperties = GetFormationChartProperties();

            // 返り値セット
            return formationChartDto;
        }

        /// <summary>
        /// フォーメーション（With守備力）取得
        /// </summary>
        public FormationChartDto GetFormationInfoWithDefense(int GameID)
        {
            // 返り値のための入れ物
            var formationChartDto = new FormationChartDto();

            // ID,classセット
            formationChartDto.FormationChartContainer.ViewContainerId = JlgChartConst.FormationContainerID;
            formationChartDto.FormationChartContainer.ViewContainerClass = JlgChartConst.ChartCssClassName.formation_chart.ToString();


            // チームID取得
            int homeTeamID = GetHomeTeamID(GameID);
            int awayTeamID = GetAwayTeamID(GameID);
            // チームID投入
            formationChartDto.FormationChartContainer.HomeTeamID = homeTeamID;
            formationChartDto.FormationChartContainer.AwayTeamID = awayTeamID;

            // フォーメーション情報がない場合のGameIDの入れ物
            int homeGameID = GameID;
            int awayGameID = GameID;

            // 表示用文言(予想）
            if (JlgCommon.GetStatusMatch(GameID.ToString()) == 0)
            {
                // フォーメーションの情報がまだない場合
                if (JlgEntities.FormationInfo.Where(x => x.GameID == GameID).Count() == 0)
                {
                    homeGameID = GetPreGameID(GameID, homeTeamID);
                    awayGameID = GetPreGameID(GameID, awayTeamID);
                    // 表示用文言(前節)
                    formationChartDto.StartingTypeForDisplay = JlgChartConst.PreviousMember;
                }
                else
                {
                    formationChartDto.StartingTypeForDisplay = JlgChartConst.ForecastMember;
                }
            }
            else
            {
                formationChartDto.StartingTypeForDisplay = String.Empty;
            }

            // コンテナーのデータ群取得
                // ホームチームのデータ取得
                List<JlgFormationInfo> homeformationInfo = GetJlgFormationInfo(homeGameID, homeTeamID);

                    // ホームチームフォーメーションの配置、攻撃力のデータ取得
                    formationChartDto.FormationChartContainer.HomePositionContainer = GetFormationPositionWithDefense(homeformationInfo);
                    // ホームチームフォーメーションの選手名取得
                    formationChartDto.FormationChartContainer.HomePlayerNameContainer = GetFormationPlayerNameWithDefense(homeformationInfo);
                    // ホームチームフォーメーションの選手背番号取得
                    formationChartDto.FormationChartContainer.HomePlayerNoContainer = GetFormationPlayerNoWithDefense(homeformationInfo);
                    // ホームチームフォーメーションの選手ID取得
                    formationChartDto.FormationChartContainer.HomePlayerIDList = GetFormationPlayerIDList(homeformationInfo);

                // アウェーチームのデータ取得
                List<JlgFormationInfo> awayformationInfo = GetJlgFormationInfo(awayGameID, awayTeamID);

                    // アウェーチームフォーメーションのデータ取得
                    formationChartDto.FormationChartContainer.AwayPositionContainer = GetFormationPositionWithDefense(awayformationInfo);
                    // アウェーチームフォーメーションの選手名取得
                    formationChartDto.FormationChartContainer.AwayPlayerNameContainer = GetFormationPlayerNameWithDefense(awayformationInfo);
                    // アウェーチームフォーメーションの選手背番号取得
                    formationChartDto.FormationChartContainer.AwayPlayerNoContainer = GetFormationPlayerNoWithDefense(awayformationInfo);
                    // アウェーチームフォーメーションの選手ID取得
                    formationChartDto.FormationChartContainer.AwayPlayerIDList = GetFormationPlayerIDList(awayformationInfo);

            // プロパティのデータ群取得
                formationChartDto.FormationChartProperties = GetFormationChartProperties();

            // 返り値セット
            return formationChartDto;
        }

        /// <summary>
        /// フォーメーション（攻撃力含、モバイル用）取得
        /// </summary>
        public FormationChartDto GetFormationInfoWithAttackForMobile(int GameID)
        {
            // 返り値のための入れ物
            var formationChartDto = new FormationChartDto();

            // ID,classセット
            formationChartDto.FormationChartContainer.ViewContainerId = JlgChartConst.FormationContainerID;
            formationChartDto.FormationChartContainer.ViewContainerClass = JlgChartConst.ChartCssClassName.formation_chart.ToString();

            // チームID取得
            int homeTeamID = GetHomeTeamID(GameID);
            int awayTeamID = GetAwayTeamID(GameID);
            // チームID投入
            formationChartDto.FormationChartContainer.HomeTeamID = homeTeamID;
            formationChartDto.FormationChartContainer.AwayTeamID = awayTeamID;

            // フォーメーション情報がない場合のGameIDの入れ物
            int homeGameID = GameID;
            int awayGameID = GameID;

            // 表示用文言(予想）
            if (JlgCommon.GetStatusMatch(GameID.ToString()) == 0)
            {
                // フォーメーションの情報がまだない場合
                if (JlgEntities.FormationInfo.Where(x => x.GameID == GameID).Count() == 0)
                {
                    homeGameID = GetPreGameID(GameID, homeTeamID);
                    awayGameID = GetPreGameID(GameID, awayTeamID);
                    // 表示用文言(前節)
                    formationChartDto.StartingTypeForDisplay = JlgChartConst.PreviousMember;
                }
                else
                {
                    formationChartDto.StartingTypeForDisplay = JlgChartConst.ForecastMember;
                }
            }
            else
            {
                formationChartDto.StartingTypeForDisplay = String.Empty;
            }

            // コンテナーのデータ群取得
                // ホームチームのデータ取得
                List<JlgFormationInfo> homeformationInfo = GetJlgFormationInfo(homeGameID, homeTeamID);

                    // ホームチームフォーメーションの配置、攻撃力のデータ取得
                    formationChartDto.FormationChartContainer.HomePositionContainer = GetFormationPositionWithAttackForMobile(homeformationInfo);
                    // ホームチームフォーメーションの選手名取得
                    formationChartDto.FormationChartContainer.HomePlayerNameContainer = GetFormationPlayerNameForMobile(homeformationInfo);
                    // ホームチームフォーメーションの選手背番号取得
                    formationChartDto.FormationChartContainer.HomePlayerNoContainer = GetFormationPlayerNoForMobile(homeformationInfo);
                    // ホームチームフォーメーションの選手ID取得
                    formationChartDto.FormationChartContainer.HomePlayerIDList = GetFormationPlayerIDList(homeformationInfo);

                // アウェーチームのデータ取得
                List<JlgFormationInfo> awayformationInfo = GetJlgFormationInfo(awayGameID, awayTeamID);
                
                    // アウェーチームフォーメーションのデータ取得
                    formationChartDto.FormationChartContainer.AwayPositionContainer = GetFormationPositionWithAttackForMobile(awayformationInfo);
                    // アウェーチームフォーメーションの選手名取得
                    formationChartDto.FormationChartContainer.AwayPlayerNameContainer = GetFormationPlayerNameForMobile(awayformationInfo);
                    // アウェーチームフォーメーションの選手背番号取得
                    formationChartDto.FormationChartContainer.AwayPlayerNoContainer = GetFormationPlayerNoForMobile(awayformationInfo);
                    // アウェーチームフォーメーションの選手ID取得
                    formationChartDto.FormationChartContainer.AwayPlayerIDList = GetFormationPlayerIDList(awayformationInfo);

            // プロパティのデータ群取得
                formationChartDto.FormationChartProperties = GetFormationChartProperties();

            // 返り値セット
            return formationChartDto;
        }

        /// <summary>
        /// フォーメーション（守備力含、モバイル用）取得
        /// </summary>
        public FormationChartDto GetFormationInfoWithDefenseForMobile(int GameID)
        {
            // 返り値のための入れ物
            var formationChartDto = new FormationChartDto();

            // ID,classセット
            formationChartDto.FormationChartContainer.ViewContainerId = JlgChartConst.FormationContainerID;
            formationChartDto.FormationChartContainer.ViewContainerClass = JlgChartConst.ChartCssClassName.formation_chart.ToString();


            // チームID取得
            int homeTeamID = GetHomeTeamID(GameID);
            int awayTeamID = GetAwayTeamID(GameID);
            // チームID投入
            formationChartDto.FormationChartContainer.HomeTeamID = homeTeamID;
            formationChartDto.FormationChartContainer.AwayTeamID = awayTeamID;

            // フォーメーション情報がない場合のGameIDの入れ物
            int homeGameID = GameID;
            int awayGameID = GameID;
            // 表示用文言(予想）
            if (JlgCommon.GetStatusMatch(GameID.ToString()) == 0)
            {
                // フォーメーションの情報がまだない場合
                if (JlgEntities.FormationInfo.Where(x => x.GameID == GameID).Count() == 0)
                {
                    homeGameID = GetPreGameID(GameID, homeTeamID);
                    awayGameID = GetPreGameID(GameID, awayTeamID);
                    // 表示用文言(前節)
                    formationChartDto.StartingTypeForDisplay = JlgChartConst.PreviousMember;
                }
                else
                {
                    formationChartDto.StartingTypeForDisplay = JlgChartConst.ForecastMember;
                }
            }
            else
            {
                formationChartDto.StartingTypeForDisplay = String.Empty;
            }

            // コンテナーのデータ群取得
                // ホームチームのデータ取得
                List<JlgFormationInfo> homeformationInfo = GetJlgFormationInfo(homeGameID,homeTeamID);

                    // ホームチームフォーメーションの配置、攻撃力のデータ取得
                    formationChartDto.FormationChartContainer.HomePositionContainer = GetFormationPositionWithDefenseForMobile(homeformationInfo);
                    // ホームチームフォーメーションの選手名取得
                    formationChartDto.FormationChartContainer.HomePlayerNameContainer = GetFormationPlayerNameForMobile(homeformationInfo);
                    // ホームチームフォーメーションの選手背番号取得
                    formationChartDto.FormationChartContainer.HomePlayerNoContainer = GetFormationPlayerNoForMobile(homeformationInfo);
                    // ホームチームフォーメーションの選手ID取得
                    formationChartDto.FormationChartContainer.HomePlayerIDList = GetFormationPlayerIDList(homeformationInfo);

                // アウェーチームのデータ取得
                List<JlgFormationInfo> awayformationInfo = GetJlgFormationInfo(awayGameID,awayTeamID);

                    // アウェーチームフォーメーションのデータ取得
                    formationChartDto.FormationChartContainer.AwayPositionContainer = GetFormationPositionWithDefenseForMobile(awayformationInfo);
                    // アウェーチームフォーメーションの選手名取得
                    formationChartDto.FormationChartContainer.AwayPlayerNameContainer = GetFormationPlayerNameForMobile(awayformationInfo);
                    // アウェーチームフォーメーションの選手背番号取得
                    formationChartDto.FormationChartContainer.AwayPlayerNoContainer = GetFormationPlayerNoForMobile(awayformationInfo);
                    // アウェーチームフォーメーションの選手ID取得
                    formationChartDto.FormationChartContainer.AwayPlayerIDList = GetFormationPlayerIDList(awayformationInfo);

            // プロパティのデータ群取得
            formationChartDto.FormationChartProperties = GetFormationChartProperties();

            // 返り値セット
            return formationChartDto;
        }

        /// <summary>
        /// フォーメーションの情報をまとめて取る（対象ゲームに絞ったやつ）
        /// </summary> 
        public List<JlgFormationInfo> GetJlgFormationInfo(int GameID,int TeamID)
        {
            var result = (from fi in JlgEntities.FormationInfo
                          join pi in JlgEntities.PlayerCBP on new { k1 = fi.TeamID, k2 = fi.PlayerID } equals new { k1 = pi.TeamID, k2 = pi.PlayerID }
                          where fi.GameID == GameID
                          where fi.TeamID == TeamID
                          orderby fi.PlayerID
                          select new JlgFormationInfo
                          {
                              PlayerID = fi.PlayerID,
                              X = fi.X,
                              Y = fi.Y,
                              PlayerName = fi.PlayerName,
                              PlayerUniformNo = fi.PlayerUniformNo,
                              PlayerPosition = fi.PlayerPosition,
                              AttackCBP = pi.AttackCBP,
                              DefenseCBP = pi.DefenseCBP,
                              ValueForMobile = JlgChartConst.ValueForMobile.ToString()
                          }
                        ).ToList();
            return result;
        }
        /// <summary>
        /// チームのフォーメーション名を返す
        /// </summary>
        public string GetFormationName(int TeamID,int GameID)
        {                    
            var formationName = JlgEntities.FormationInfo.Where(x => x.TeamID == TeamID).Where(x => x.GameID == GameID).Select(x => x.FormationName).DefaultIfEmpty(String.Empty).First();

            var result = formationName == null?String.Empty:formationName;

            return result;
        }
        /// <summary>
        /// homeのチームIDを返す
        /// </summary>
        public int GetHomeTeamID(int GameID)
        {
            int? homeTeamID = JlgEntities.ScheduleInfo.Where(m => m.GameID == GameID).Select(m => m.HomeTeamID).FirstOrDefault();

            int result = homeTeamID == null ? 0 : (int)homeTeamID;

            return result;
        }
        /// <summary>
        /// awayのチームIDを返す
        /// </summary>
        public int GetAwayTeamID(int GameID)
        {
            int? awayTeamID = JlgEntities.ScheduleInfo.Where(m => m.GameID == GameID).Select(m => m.AwayTeamID).FirstOrDefault();

            int result = awayTeamID == null ? 0 : (int)awayTeamID;

            return result;
        }

        /// <summary>
        /// チームフォーメーション（攻撃力含）を返す
        /// </summary>
        public List<FormationDataSource> GetFormationPositionWithAttack(List<JlgFormationInfo> FormationInfo)
        {
            var result = (from fi in FormationInfo
                         select new FormationDataSource()
                         {
                              X = fi.X,
                              Y = fi.Y,
                              Label = fi.AttackCBP.ToString(),
                              Value = (int)(fi.AttackCBP / JlgChartConst.AttackCBPScale)
                         }
                         ).ToList();
            return result;
        }
        /// <summary>
        /// チームフォーメーション（守備力含）を返す
        /// </summary>
        public List<FormationDataSource> GetFormationPositionWithDefense(List<JlgFormationInfo> FormationInfo)
        {
            var result = (from fi in FormationInfo
                          select new FormationDataSource
                          {
                              X = fi.X,
                              Y = fi.Y,
                              Label = fi.DefenseCBP.ToString(),
                              Value = (int)(fi.DefenseCBP / JlgChartConst.DefenseCBPScale)
                          }
                        ).ToList();

            return result;
        }
        /// <summary>
        /// チームフォーメーション（攻撃力含,モバイル用）を返す
        /// </summary> 
        public List<FormationDataSource> GetFormationPositionWithAttackForMobile(List<JlgFormationInfo> FormationInfo)
        {
            var result = (from fi in FormationInfo
                          select new FormationDataSource
                          {
                              X = fi.X,
                              Y = fi.Y,
                              Label = fi.AttackCBP.ToString(),
                              Value = JlgChartConst.ValueForMobile
                          }
                        ).ToList();

            return result;
        }
        /// <summary>
        /// チームフォーメーション（守備力含,モバイル用）を返す
        /// </summary>
        public List<FormationDataSource> GetFormationPositionWithDefenseForMobile(List<JlgFormationInfo> FormationInfo)
        {
            var result = (from fi in FormationInfo
                          select new FormationDataSource
                          {
                              X = fi.X,
                              Y = fi.Y,
                              Label = fi.DefenseCBP.ToString(),
                              Value = JlgChartConst.ValueForMobile
                          }
                        ).ToList();

            return result;
        }

        /// <summary>
        /// チームプレイヤー名（攻撃力含）を返す
        /// </summary>
        public List<FormationDataSource> GetFormationPlayerNameWithAttack(List<JlgFormationInfo> FormationInfo)
        {
            var result = (from fi in FormationInfo
                          select new FormationDataSource
                          {
                              X = fi.X,
                              Y = fi.Y - JlgChartConst.CoefficientOfRadius + (JlgChartConst.CoefficientOfRadius * (Decimal)fi.AttackCBP) * (fi.Y / Math.Abs(fi.Y)),
                              Label = fi.PlayerName
                          }
                          ).ToList();

            return result;
        }
        /// <summary>
        /// ちーむプレイヤー名（守備力含）を返す
        /// </summary>
        public List<FormationDataSource> GetFormationPlayerNameWithDefense(List<JlgFormationInfo> FormationInfo)
        {
            var result = (from fi in FormationInfo
                          select new FormationDataSource
                          {
                              X = fi.X,
                              Y = fi.Y - JlgChartConst.CoefficientOfRadius + (JlgChartConst.CoefficientOfRadius * (Decimal)fi.DefenseCBP) * (fi.Y / Math.Abs(fi.Y)),
                              Label = fi.PlayerName
                          }
                          ).ToList();

            return result;
        }
        /// <summary>
        ///  チームプレイヤー名（モバイル用）を返す
        /// </summary>
        public List<FormationDataSource> GetFormationPlayerNameForMobile(List<JlgFormationInfo> FormationInfo)
        {
            var result = (from fi in FormationInfo
                          select new FormationDataSource
                          {
                              X = fi.X,
                              Y = fi.Y - JlgChartConst.CoefficientOfRadius + (JlgChartConst.CoefficientOfRadius * JlgChartConst.ValueForMobile) * (fi.Y / Math.Abs(fi.Y)),
                              Label = fi.PlayerName
                          }
                          ).ToList();

            return result;
        }

        /// <summary>
        /// チームプレイヤー背番号（攻撃力含）を返す
        /// </summary>
        public List<FormationDataSource> GetFormationPlayerNoWithAttack(List<JlgFormationInfo> FormationInfo)
        {
            var result = (from fi in FormationInfo
                          select new FormationDataSource
                          {
                              X = fi.X,
                              Y = fi.Y - (JlgChartConst.CoefficientOfRadius * (Decimal)fi.AttackCBP) / 2,
                              Label = fi.PlayerUniformNo
                          }
                          ).ToList();

            return result;
        }
        /// <summary>
        /// チームプレイヤー背番号（守備力含）を返す
        /// </summary>
        public List<FormationDataSource> GetFormationPlayerNoWithDefense(List<JlgFormationInfo> FormationInfo)
        {
            var result = (from fi in FormationInfo
                          select new FormationDataSource
                          {
                              X = fi.X,
                              Y = fi.Y - (JlgChartConst.CoefficientOfRadius * (Decimal)fi.DefenseCBP) / 2,
                              Label = fi.PlayerUniformNo
                          }
                          ).ToList();

            return result;
        }
        /// <summary>
        /// チームプレイヤー背番号（モバイル用）を返す
        /// </summary>
        public List<FormationDataSource> GetFormationPlayerNoForMobile(List<JlgFormationInfo> FormationInfo)
        {
            var result = (from fi in FormationInfo
                          select new FormationDataSource
                          {
                              X = fi.X,
                              Y = fi.Y - (JlgChartConst.CoefficientOfRadius * JlgChartConst.ValueForMobile) / 2,
                              Label = fi.PlayerUniformNo
                          }
                          ).ToList();

            return result;
        }
  
        /// <summary>
        /// チームプレイヤーIDリストを返す
        /// </summary>
        public List<string> GetFormationPlayerIDList(List<JlgFormationInfo> FormationInfo)
        {
             var result = FormationInfo.Select(x=>x.PlayerID.ToString()).ToList();

             return result;
        }
        /// <summary>
        /// フォーメーション配置、攻撃力のデータ表示用バブルのプロパティーを返す
        /// </summary>
        public FormationChartProperties GetFormationChartProperties()
        {
            FormationChartProperties result = new FormationChartProperties();

            // ホームチームのデータ取得
            // ホームチームフォーメーション配置、攻撃力のデータ表示用バブルのプロパティー
            result.HomePositionProperties = GetFormationPositionProperties();
            // ホームチームフォーメーションの選手名マーカー用のプロパティー
            result.HomePlayerNameProperties = GetFormationMarkerProperties();
            // ホームチームフォーメーションの選手名マーカー用のプロパティー
            result.HomePlayerNoProperties = GetFormationMarkerProperties();

            // アウェーチームのデータ取得
            // アウェーチームフォーメーション配置、攻撃力のデータ表示用バブルのプロパティー
            result.AwayPositionProperties = GetFormationPositionProperties();
            // ホームチームフォーメーションの選手名マーカー用のプロパティー
            result.AwayPlayerNameProperties = GetFormationMarkerProperties();
            // ホームチームフォーメーションの選手名マーカー用のプロパティー
            result.AwayPlayerNoProperties = GetFormationMarkerProperties();

            // 共通項目のプロパティー
            result.SharedProperties = GetFormationSharedProperties();

            return result;
        }

        /// <summary>
        /// フォーメーション配置、攻撃力のデータ表示用バブルのプロパティーを返す
        /// </summary>
        public ChartProperties GetFormationPositionProperties()
        {
            ChartProperties result = new ChartProperties();

            // Bubble
            result.Bubble.IsVisible = true;
            result.Bubble.BaseRadius = JlgChartConst.FormationBaseRadius;

            // Mouse
            result.Mouse.IsTrackable = true;

            return result;
        }

        /// <summary>
        /// フォーメーション選手名マーカー用のプロパティーを返す
        /// </summary>
        public ChartProperties GetFormationMarkerProperties()
        {
            ChartProperties result = new ChartProperties();

            // Marker
            result.Marker.IsVisible = true;
            result.Marker.IsRelative = false;
            result.Marker.FontSize = JlgChartConst.FormationFontSize;

            return result;
        }

        /// <summary>
        /// フォーメーション共通のプロパティーを返す
        /// </summary>
        public ChartProperties GetFormationSharedProperties()
        {
            ChartProperties result = new ChartProperties();

            // XAxis
            result.XAxis.MinValue = JlgChartConst.FormationXAxisMinValue;
            result.XAxis.MaxValue = JlgChartConst.FormationXAxisMaxValue;
            result.XAxis.IsVisibleLabels = false;

            result.YAxis = new YAxis();
            // YAxis
            result.YAxis.MinValue = JlgChartConst.FormationYAxisMinValue;
            result.YAxis.MaxValue = JlgChartConst.FormationYAxisMaxValue;
            result.YAxis.IsVisibleLabels = false;

            result.Grid = new Grid();
            // Grid
            result.Grid.IsVisibleHorizontalLines = false;
            result.Grid.IsVisibleVerticalLines = false;
            result.Grid.BackGroundImageAddress = JlgChartConst.FormationGridBackGroundImageAddress;

            // Colors
            result.Colors = new List<string>() { JlgChartConst.HomeFormationColor, JlgChartConst.DefaultColor, JlgChartConst.DefaultColor, JlgChartConst.AwayFormationColor, JlgChartConst.DefaultColor, JlgChartConst.DefaultColor };

            return result;
        }

        /// <summary>
        /// 欠場選手情報
        /// </summary>
        /// <param name="teamID"></param>
        /// <returns></returns>
        public List<JlgPlayerGameInfoModel> GetInjuredPlayers(int teamID)
        {
            List<JlgPlayerGameInfoModel> members = new List<JlgPlayerGameInfoModel>();

            //欠場選手情報
            var query = (from pip in JlgEntities.PlayerInfoPL                                             //選手情報マスタ
                         join pii in JlgEntities.PlayerInfoIR on pip.PlayerID equals pii.PlayerID         //欠場選手情報
                         join tii in JlgEntities.TeamInfoIR on pii.TeamInfoIRId equals tii.TeamInfoIRId   //欠場選手情報_チーム情報
                         where tii.TeamID == teamID
                         orderby pii.Pos
                         select new JlgPlayerGameInfoModel
                         {
                             ID = pii.PlayerID,
                             NameS = pii.PlayerNameS,
                             Name = pii.PlayerName,
                             Pos = pii.Pos,
                             Uni = pip.UniformNo
                         }
                        );

            if (query.Any())
            {
                members = query.ToList();
                members.Sort();
            }

            return members;

        }

        /// <summary>
        /// 出場停止選手情報
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="teamID"></param>
        /// <returns></returns>
        public List<JlgPlayerGameInfoModel> GetSuspendedPlayers(int gameID, int teamID)
        {
            List<JlgPlayerGameInfoModel> members = new List<JlgPlayerGameInfoModel>();

            //出場停止選手情報
            var query = (from pip in JlgEntities.PlayerInfoPL                                                         //選手情報マスタ
                         join sis in JlgEntities.SuspensionInfoSR on pip.PlayerID equals sis.PlayerID                 //出場停止情報
                         join gir in JlgEntities.GameInfoSR on sis.SuspensionInfoSRId equals gir.SuspensionInfoSRId   //出場停止試合
                         where gir.GameID == gameID && sis.TeamID == teamID
                         orderby pip.Position
                         select new JlgPlayerGameInfoModel
                         {
                             ID = sis.PlayerID,
                             NameS = sis.PlayerNameS,
                             Name = sis.PlayerName,
                             Pos = pip.Position,
                             Uni = pip.UniformNo
                         }
                         );

            if (query.Any())
            {
                members = query.ToList();
                members.Sort();
            }

            return members;

        }


        /// <summary>
        /// スターティングメンバーの情報を取得
        /// </summary> 
        public JlgGameInfoViewModel GetStartingInformation(int GameID)
        {
            JlgGameInfoViewModel result = new JlgGameInfoViewModel();

            // チーム関連情報取得
            int homeTeamID = GetHomeTeamID(GameID);
            int awayTeamID = GetAwayTeamID(GameID);
            // フォーメーション情報がない場合のGameIDの入れ物
            int homeGameID = GameID;
            int awayGameID = GameID;

            // 表示用文言(予想）
            if (JlgCommon.GetStatusMatch(GameID.ToString()) == 0)
            {
                // フォーメーションの情報がまだない場合
                if (JlgEntities.FormationInfo.Where(x => x.GameID == GameID).Count() == 0)
                {
                    homeGameID = GetPreGameID(GameID, homeTeamID);
                    awayGameID = GetPreGameID(GameID, awayTeamID);
                    // 表示用文言(前節)
                    result.FormationChartDto.StartingTypeForDisplay = JlgChartConst.PreviousMember;
                }
                else
                {
                    result.FormationChartDto.StartingTypeForDisplay = JlgChartConst.ForecastMember;
                }
            }
            else
            {
                result.FormationChartDto.StartingTypeForDisplay = String.Empty;
            }
 
            // チーム関連情報取得
            result.homeTeamSpec = GetTeamSpecByTeamId(homeTeamID, homeGameID);
            result.awayTeamSpec = GetTeamSpecByTeamId(awayTeamID, awayGameID);

            // プレイヤー関連情報取得
            result.homePlayerSpec = GetJlgFormationInfo(homeGameID, homeTeamID);
            result.awayPlayerSpec = GetJlgFormationInfo(awayGameID, awayTeamID);

            //欠場選手情報
            result.HomeInjuredMembers = GetInjuredPlayers(homeTeamID);
            result.AwayInjuredMembers = GetInjuredPlayers(awayTeamID);

            //出場停止選手情報
            result.HomeSuspendedMembers = GetSuspendedPlayers(GameID, homeTeamID);
            result.AwaySuspendedMembers = GetSuspendedPlayers(GameID, awayTeamID);

            return result;
        }
        /// <summary>
        /// スターティング情報、フォーメーション情報を取得（攻撃面ForPC）
        /// </summary>
        public JlgGameInfoViewModel GetStartingAndFormationWithAttack(int GameID)
        {
            JlgGameInfoViewModel result = new JlgGameInfoViewModel();

            // スターティングメンバーの情報を取得
            result = GetStartingInformation(GameID);

            // フォーメーション情報を取得（攻撃面を反映）
            result.FormationChartDto = GetFormationInfoWithAttack(GameID);

            // 返り値を返す
            return result;
        }

        /// <summary>
        /// スターティング情報、フォーメーション情報を取得（守備面ForPC）
        /// </summary>
        public JlgGameInfoViewModel GetStartingAndFormationWithDefense(int GameID)
        {
            JlgGameInfoViewModel result = new JlgGameInfoViewModel();

            // スターティングメンバーの情報を取得
            result = GetStartingInformation(GameID);

            // フォーメーション情報を取得（守備面を反映）
            result.FormationChartDto = GetFormationInfoWithDefense(GameID);

            // 返り値を返す
            return result;
        }

        /// <summary>
        /// スターティング情報、フォーメーション情報を取得（攻撃面ForSP）
        /// </summary>
        public JlgGameInfoViewModel GetStartingAndFormationWithAttackForMobile(int GameID)
        {
            JlgGameInfoViewModel result = new JlgGameInfoViewModel();

            // スターティングメンバーの情報を取得
            result = GetStartingInformation(GameID);

            // フォーメーション情報を取得（攻撃面を反映）
            result.FormationChartDto = GetFormationInfoWithAttackForMobile(GameID);

            // 返り値を返す
            return result;
        }

        /// <summary>
        /// スターティング情報、フォーメーション情報を取得（守備面ForSP）
        /// </summary>
        public JlgGameInfoViewModel GetStartingAndFormationWithDefenseForMobile(int GameID)
        {
            JlgGameInfoViewModel result = new JlgGameInfoViewModel();

            // スターティングメンバーの情報を取得
            result = GetStartingInformation(GameID);

            // フォーメーション情報を取得（守備面を反映）
            result.FormationChartDto = GetFormationInfoWithDefenseForMobile(GameID);

            // 返り値を返す
            return result;
        }
        
        /// <summary>
        /// 前節の試合IDを取得する
        /// </summary>
        public int GetPreGameID(int GameID,int TeamID)
        {
            int result = 0;
            var query = JlgEntities.ScheduleInfo.Where(x => x.HomeTeamID == TeamID || x.AwayTeamID == TeamID).Where(x => x.GameID < GameID).OrderByDescending(x=>x.GameID).Select(x=>x.GameID);

            if (query.Count() > 0)
            {
                result = query.FirstOrDefault();
            }

            return result;
        }

        /// <summary>
        /// 一試合選手速報(スターティング）
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="hv"></param>
        /// <param name="startingMembers"></param>
        /// <param name="subMembers"></param>
        /// <returns></returns>
        public List<JlgMembersInGame> GetStartingPlayerGameInfos(int gameID, int hv)
        {
            var result = new List<JlgMembersInGame>();
            //スターティングメンバー情報
            result = (from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                        join st in JlgEntities.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                        join sti in JlgEntities.StartingInfoLP on st.StartingInfoLPId equals sti.StartingInfoLPId //一試合選手速報_先発情報
                        join pi in JlgEntities.PlayerInfoLPST on sti.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                        where pr.GameID == gameID
                        && st.HV == hv
                        select new JlgMembersInGame
                        {
                            HV = hv,
                            ID = pi.ID,
                            NameS = pi.NameS,
                            Name = pi.Name,
                            Goal = pi.Goal,
                            Card = pi.Card,
                            IsStartingMember = true,
                            Pos = pi.Pos,
                            Uni = pi.Uni,
                            Yellow = pi.Yellow,
                        }).ToList();
            foreach (var res in result)
            {
                res.CardInfos = GetStartingPlayerGameDetailCardInfos(gameID, res.ID, hv);
                res.GoalInfos = GetStartingPlayerGameDetailGoalInfos(gameID, res.ID, hv);
                res.InInfos = GetStartingPlayerGameDetailInInfos(gameID, res.ID, hv);
                res.OutInfos = GetStartingPlayerGameDetailOutInfos(gameID, res.ID, hv);
            }

            result.Sort();

            return result;
        }
        /// <summary>
        /// 一試合選手速報(サブ）
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="hv"></param>
        /// <param name="startingMembers"></param>
        /// <param name="subMembers"></param>
        /// <returns></returns>
        public List<JlgMembersInGame> GetSubPlayerGameInfos(int gameID, int hv)
        {
            var result = new List<JlgMembersInGame>();

            //ベンチ入り選手情報 サブメンバー情報
            result = (from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                   join be in JlgEntities.BenchInfoLP on pr.PlayerReportLPId equals be.PlayerReportLPId  //一試合選手速報_控え情報
                                   join sti in JlgEntities.BenchInfoLP on be.BenchInfoLPId equals sti.BenchInfoLPId //一試合選手速報_先発情報
                                   join pi in JlgEntities.PlayerInfoLPBE on sti.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                   where pr.GameID == gameID
                                   && be.HV == hv
                                   select new JlgMembersInGame
                                   {
                                       HV = hv,
                                       ID = pi.ID,
                                       NameS = pi.NameS,
                                       Name = pi.Name,
                                       Goal = pi.Goal,
                                       Card = pi.Card,
                                       IsStartingMember = false,
                                       Pos = pi.Pos,
                                       Uni = pi.Uni,
                                       Yellow = pi.Yellow,
                                   }).ToList();

            foreach (var res in result)
            {
                res.CardInfos = GetSubPlayerGameDetailCardInfos(gameID, res.ID, hv);
                res.GoalInfos = GetSubPlayerGameDetailGoalInfos(gameID, res.ID, hv);
                res.InInfos = GetSubPlayerGameDetailInInfos(gameID, res.ID, hv);
                res.OutInfos = GetSubPlayerGameDetailOutInfos(gameID, res.ID, hv);
            }

            result.Sort();

            return result;
        }
        /// <summary>
        /// Getスターティング選手情報（警告・退場)
        /// </summary>
        public List<JlgPlayerGameDetailInfoModel> GetStartingPlayerGameDetailCardInfos(int gameID,int PlayerID, int hv)
        {
            var result = new List<JlgPlayerGameDetailInfoModel>();
            // Homeの場合
            if (hv == 1)
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join wilg in JlgEntities.WarningInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.HomeTeamID, k3 = pi.ID } equals new { k1 = wilg.GameReportLGId, k2 = wilg.TeamID, k3 = (int)wilg.PlayerID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.HomeTeamID,
                                StateName = wilg.StateName,
                                Half = (int)wilg.Half,
                                Time = (int)wilg.Time,
                                PlayerId = (int)wilg.PlayerID,
                                PlayerName = wilg.PlayerName,
                                PlayerNameS = wilg.PlayerNameS,
                                Divide = (short)wilg.Divide,
                                SecondF = (short)wilg.SecondF
                            }).ToList();
                result.Sort();
            }
            else
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join wilg in JlgEntities.WarningInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.AwayTeamID, k3 = pi.ID } equals new { k1 = wilg.GameReportLGId, k2 = wilg.TeamID, k3 = (int)wilg.PlayerID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.AwayTeamID,
                                StateName = wilg.StateName,
                                Half = (int)wilg.Half,
                                Time = (int)wilg.Time,
                                PlayerId = (int)wilg.PlayerID,
                                PlayerNameS = wilg.PlayerNameS,
                                Divide = (short)wilg.Divide,
                                SecondF = (short)wilg.SecondF
                            }).ToList();
                result.Sort();
            }
            return result;
        }
        /// <summary>
        /// Getスターティング選手情報（得点)
        /// </summary>
        public List<JlgPlayerGameDetailInfoModel> GetStartingPlayerGameDetailGoalInfos(int gameID,int PlayerID, int hv)
        {
            var result = new List<JlgPlayerGameDetailInfoModel>();
            // Homeの場合
            if (hv == 1)
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join gil in JlgEntities.GoalInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.HomeTeamID, k3 = pi.ID } equals new { k1 = gil.GameReportLGId, k2 = gil.ScoreTeamID, k3 = (int)gil.GPlayerID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.HomeTeamID,
                                StateName = gil.StateName,
                                Half = (int)gil.Half,
                                Time = (int)gil.Time,
                                PlayerId = (int)gil.GPlayerID,
                                PlayerName = gil.GPlayerName,
                                PlayerNameS = gil.GPlayerNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            else
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join gil in JlgEntities.GoalInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.AwayTeamID, k3 = pi.ID } equals new { k1 = gil.GameReportLGId, k2 = gil.ScoreTeamID, k3 = (int)gil.GPlayerID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.AwayTeamID,
                                StateName = gil.StateName,
                                Half = (int)gil.Half,
                                Time = (int)gil.Time,
                                PlayerId = (int)gil.GPlayerID,
                                PlayerName = gil.GPlayerName,
                                PlayerNameS = gil.GPlayerNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            return result;
        }
        /// <summary>
        /// Getスターティング選手情報（途中出場)
        /// </summary>
        public List<JlgPlayerGameDetailInfoModel> GetStartingPlayerGameDetailInInfos(int gameID,int PlayerID, int hv)
        {
            var result = new List<JlgPlayerGameDetailInfoModel>();
            // Homeの場合
            if (hv == 1)
            {
                        result = (
                                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in JlgEntities.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in JlgEntities.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join cil in JlgEntities.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                                            join pil in JlgEntities.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.InID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            where pi.ID == PlayerID
                                            where cil.HV == hv
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                TeamId = (int)pr.HomeTeamID,
                                                StateName = pil.StateName,
                                                Half = (int)pil.Half,
                                                Time = (int)pil.Time,
                                                PlayerId = (int)pil.InID,
                                                PlayerName = pil.InName,
                                                PlayerNameS = pil.InNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            }).ToList();
                result.Sort();
            }
            else
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join cil in JlgEntities.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                            join pil in JlgEntities.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.InID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            where cil.HV == hv
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.AwayTeamID,
                                StateName = pil.StateName,
                                Half = (int)pil.Half,
                                Time = (int)pil.Time,
                                PlayerId = (int)pil.InID,
                                PlayerName = pil.InName,
                                PlayerNameS = pil.InNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            return result;
        }
        /// <summary>
        /// Getスターティング選手情報（途中交代)
        /// </summary>
        public List<JlgPlayerGameDetailInfoModel> GetStartingPlayerGameDetailOutInfos(int gameID,int PlayerID, int hv)
        {
            var result = new List<JlgPlayerGameDetailInfoModel>();
            // Homeの場合
            if (hv == 1)
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join cil in JlgEntities.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                            join pil in JlgEntities.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.OutID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            where cil.HV == hv
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.HomeTeamID,
                                StateName = pil.StateName,
                                Half = (int)pil.Half,
                                Time = (int)pil.Time,
                                PlayerId = (int)pil.OutID,
                                PlayerName = pil.OutName,
                                PlayerNameS = pil.OutNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            else
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join cil in JlgEntities.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                            join pil in JlgEntities.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.OutID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            where cil.HV == hv
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.AwayTeamID,
                                StateName = pil.StateName,
                                Half = (int)pil.Half,
                                Time = (int)pil.Time,
                                PlayerId = (int)pil.OutID,
                                PlayerName = pil.OutName,
                                PlayerNameS = pil.OutNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            return result;
        }

        /// <summary>
        /// Getサブ選手情報（警告・退場)
        /// </summary>
        public List<JlgPlayerGameDetailInfoModel> GetSubPlayerGameDetailCardInfos(int gameID,int PlayerID, int hv)
        {
            var result = new List<JlgPlayerGameDetailInfoModel>();
            // Homeの場合
            if (hv == 1)
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join wilg in JlgEntities.WarningInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.HomeTeamID, k3 = pi.ID } equals new { k1 = wilg.GameReportLGId, k2 = wilg.TeamID, k3 = (int)wilg.PlayerID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.HomeTeamID,
                                StateName = wilg.StateName,
                                Half = (int)wilg.Half,
                                Time = (int)wilg.Time,
                                PlayerId = (int)wilg.PlayerID,
                                PlayerName = wilg.PlayerName,
                                PlayerNameS = wilg.PlayerNameS,
                                Divide = (short)wilg.Divide,
                                SecondF = (short)wilg.SecondF
                            }).ToList();
                result.Sort();
            }
            else
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join wilg in JlgEntities.WarningInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.AwayTeamID, k3 = pi.ID } equals new { k1 = wilg.GameReportLGId, k2 = wilg.TeamID, k3 = (int)wilg.PlayerID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.AwayTeamID,
                                StateName = wilg.StateName,
                                Half = (int)wilg.Half,
                                Time = (int)wilg.Time,
                                PlayerId = (int)wilg.PlayerID,
                                PlayerName = wilg.PlayerName,
                                PlayerNameS = wilg.PlayerNameS,
                                Divide = (short)wilg.Divide,
                                SecondF = (short)wilg.SecondF
                            }).ToList();
                result.Sort();
            }
            return result;
        }
        /// <summary>
        /// Getサブ選手情報（得点)
        /// </summary
        public List<JlgPlayerGameDetailInfoModel> GetSubPlayerGameDetailGoalInfos(int gameID,int PlayerID, int hv)
        {
            var result = new List<JlgPlayerGameDetailInfoModel>();
            // Homeの場合
            if (hv == 1)
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join gil in JlgEntities.GoalInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.HomeTeamID, k3 = pi.ID } equals new { k1 = gil.GameReportLGId, k2 = gil.ScoreTeamID, k3 = (int)gil.GPlayerID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.HomeTeamID,
                                StateName = gil.StateName,
                                Half = (int)gil.Half,
                                Time = (int)gil.Time,
                                PlayerId = (int)gil.GPlayerID,
                                PlayerName = gil.GPlayerName,
                                PlayerNameS = gil.GPlayerNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            else
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join gil in JlgEntities.GoalInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.AwayTeamID, k3 = pi.ID } equals new { k1 = gil.GameReportLGId, k2 = gil.ScoreTeamID, k3 = (int)gil.GPlayerID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.AwayTeamID,
                                StateName = gil.StateName,
                                Half = (int)gil.Half,
                                Time = (int)gil.Time,
                                PlayerId = (int)gil.GPlayerID,
                                PlayerName = gil.GPlayerName,
                                PlayerNameS = gil.GPlayerNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            return result;
        }
        /// <summary>
        /// Getサブ選手情報（途中出場)
        /// </summary
        public List<JlgPlayerGameDetailInfoModel> GetSubPlayerGameDetailInInfos(int gameID,int PlayerID, int hv)
        {
            var result = new List<JlgPlayerGameDetailInfoModel>();
            // Homeの場合
            if (hv == 1)
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join cil in JlgEntities.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                            join pil in JlgEntities.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.InID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            where cil.HV == hv
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.HomeTeamID,
                                StateName = pil.StateName,
                                Half = (int)pil.Half,
                                Time = (int)pil.Time,
                                PlayerId = (int)pil.InID,
                                PlayerName = pil.InName,
                                PlayerNameS = pil.InNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            else
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join cil in JlgEntities.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                            join pil in JlgEntities.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.InID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            where cil.HV == hv
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.AwayTeamID,
                                StateName = pil.StateName,
                                Half = (int)pil.Half,
                                Time = (int)pil.Time,
                                PlayerId = (int)pil.InID,
                                PlayerName = pil.InName,
                                PlayerNameS = pil.InNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            return result;
        }
        /// <summary>
        /// Getサブ選手情報（途中交代)
        /// </summary
        public List<JlgPlayerGameDetailInfoModel> GetSubPlayerGameDetailOutInfos(int gameID,int PlayerID, int hv)
        {
            var result = new List<JlgPlayerGameDetailInfoModel>();
            // Homeの場合
            if (hv == 1)
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join cil in JlgEntities.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                            join pil in JlgEntities.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.OutID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            where cil.HV == hv
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.HomeTeamID,
                                StateName = pil.StateName,
                                Half = (int)pil.Half,
                                Time = (int)pil.Time,
                                PlayerId = (int)pil.OutID,
                                PlayerName = pil.OutName,
                                PlayerNameS = pil.OutNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            else
            {
                result = (
                            from pr in JlgEntities.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                            join st in JlgEntities.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                            join pi in JlgEntities.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                            join grl in JlgEntities.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                            join cil in JlgEntities.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                            join pil in JlgEntities.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.OutID }  // 一試合選手速報_選手情報（先発情報）
                            where pr.GameID == gameID
                            where pi.ID == PlayerID
                            where cil.HV == hv
                            select new JlgPlayerGameDetailInfoModel
                            {
                                TeamId = (int)pr.AwayTeamID,
                                StateName = pil.StateName,
                                Half = (int)pil.Half,
                                Time = (int)pil.Time,
                                PlayerId = (int)pil.OutID,
                                PlayerName = pil.OutName,
                                PlayerNameS = pil.OutNameS,
                                Divide = 0,
                                SecondF = 0
                            }).ToList();
                result.Sort();
            }
            return result;
        }

        #endregion
    }
}
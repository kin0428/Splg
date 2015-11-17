#region  Author Information
/*  
 * 
 * Createdted      : Tran Sy Huynh
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models;
using Splg.Areas.Npb.Models;
using Splg.Areas.Jleague.Models;
using Splg.Models.News.ViewModel;
using Splg.Areas.Mlb.Models;
#endregion

namespace Splg.Controllers
{
    public class PostedController
    {

        #region Get Topic List
        /// <summary>
        /// Get all topic that satisfy the conditions
        /// </summary>
        /// <param name=""></param>
        /// <returns>List of topicmaster that has the same id of news</returns>
        private static List<TopicMaster> GetTopicList(int type = 0, int? sportID = null, int? teamID = null, int? classificationType = null)
        {
            ComEntities news = new ComEntities();
            var topicList = new List<TopicMaster>();
            switch(type)
            {
                case 0:
                case 6:
                    topicList = (from topicmaster in news.TopicMaster
                             where (topicmaster.SportID == sportID.Value)
                             select topicmaster).ToList();
                    break;
                case 2:
                    topicList = (from topicmaster in news.TopicMaster
                                 where (topicmaster.SportID == sportID.Value && topicmaster.ClassificationType == classificationType.Value)
                                 select topicmaster).ToList();
                    break;
                case 1:
                //topicList = (from topicmaster in news.TopicMaster
                //             where (topicmaster.ClassificationType == classificationType.Value && topicmaster.UniqueID == teamID.Value)
                //             select topicmaster).ToList();
                //    break;
                case 3:
                topicList = (from topicmaster in news.TopicMaster
                             where (topicmaster.SportID == sportID.Value && topicmaster.ClassificationType == classificationType.Value && topicmaster.UniqueID == teamID.Value)
                             select topicmaster).ToList();
                    break;
                case 4:
                case 5:
                case 9:
                case 10:
                    topicList = new List<TopicMaster>(news.TopicMaster);
                    break;
                case 7:
                case 8:
                    topicList = (from topicmaster in news.TopicMaster
                                 where (topicmaster.TopicID == sportID.Value)
                                 select topicmaster).ToList();
                    break;
            }
            return topicList;
        }
        #endregion

        #region Get Topic List
        /// <summary>
        /// Get all members that follow the current login user
        /// 
        /// </summary>
        /// <param name="withLoginMember">0 : Not add Login member, 1: add with login member, 2: with login member only</param>
        /// <returns>List of members that follow the current login user</returns>
        private static List<Member> GetFollorMemberList(int withLoginMember = 0, int inPutMemberId = 0)
        {
            ComEntities news = new ComEntities();
            HttpContext context = HttpContext.Current;
            var followMemberList = default(List<Member>);
            if(withLoginMember == 3 && inPutMemberId > 0)
            {
                followMemberList = (from m in news.Member
                                    where m.MemberId == inPutMemberId
                                    select m).ToList();
                return followMemberList;
            }
            if (context.Session["CurrentUser"] != null)
            {
                long memberID = Convert.ToInt64(context.Session["CurrentUser"].ToString());
                switch(withLoginMember)
                {
                    case 1:
                        followMemberList = (from f in news.FollowList
                                           join m in news.Member on f.MemberID equals m.MemberId
                                           where f.FollowerMemberID == memberID || m.MemberId == memberID
                                           select m).ToList();
                        break;
                    case 2:
                        followMemberList = (from m in news.Member
                                           where m.MemberId == memberID
                                           select m).ToList();
                        break;
                    default:
                        followMemberList = (from f in news.FollowList
                                           join m in news.Member on f.MemberID equals m.MemberId
                                           where f.FollowerMemberID == memberID
                                           select m).ToList();
                        break;
                }
            }
            else
            {
                followMemberList = new List<Member>(news.Member);
            }
            return followMemberList;
        }
        #endregion

        #region Get Related Posts
        /// <summary>
        /// Get all post that realted to a brief news
        /// </summary>
        public static IEnumerable<PostedInfoViewModel> GetRecentPosts(int type = 0, int? sportID = null, int? teamID = null, int? classificationType = null)
        {
            ComEntities com = new ComEntities();
            NpbEntities npb = new NpbEntities();
            JlgEntities jlg = new JlgEntities();
            MlbEntities mlb = new MlbEntities();

            short currentYear = Convert.ToInt16(DateTime.Now.Year);
            var topicList = GetTopicList(type, sportID, teamID, classificationType);
            var memberList = (from quot in com.QuotTopic
                              join cont in com.Contribution on quot.ContributeID equals cont.ContributeId
                              join contOrg in com.ContributionQuotOrg on cont.ContributeId equals contOrg.ContributeId
                              join brief in com.BriefNews on new { c1 = contOrg.QuoteUniqueId1, c2 = contOrg.Status, c3 = contOrg.NewsClassId } equals new { c1 = brief.NewsItemID, c2 = (short)1, c3 = (long)1 } into brief_news
                              from br in brief_news.DefaultIfEmpty()
                              //join mr in com.MonthlyResults on new { g1 = contOrg.QuoteUniqueId2, g2 = contOrg.Status, g3 = contOrg.NewsClassId, g4 = contOrg.QuoteUniqueId1, g5 = currentYear, g6 = sportID.Value }
                              //  equals new { g1 = (long)mr.ReleVantMonth, g2 = (short)1, g3 = (long)6, g4 = (long)mr.MemberID, g5 = mr.ReleVantYear, g6 = mr.SportsID } into monthlyResult
                              //from mlr in monthlyResult.DefaultIfEmpty()
                              select new
                              {
                                  TopicID = quot.TopicID,
                                  ContributeId = quot.ContributeID,
                                  ContributeDate = cont.ContributeDate,
                                  ModifiedDate = cont.ModifiedDate,
                                  Title = cont.Title,
                                  ContributedPicture = cont.ContributedPicture,
                                  Body = cont.Body,
                                  MemberId = cont.MemberId,
                                  NewsClassId = contOrg.NewsClassId,
                                  QuoteUniqueId1 = contOrg.QuoteUniqueId1,
                                  QuoteUniqueId2 = contOrg.QuoteUniqueId2,
                                  QuoteUniqueId3 = contOrg.QuoteUniqueId3,
                                  Status = contOrg.Status,
                                  //ReleVantYear = (mlr != null ? mlr.ReleVantYear : default(Nullable<short>)),
                                  //ReleVantMonth = (mlr != null ? mlr.ReleVantMonth : default(Nullable<short>)),
                                  //ExpectNumber = (mlr != null ? mlr.ExpectNumber : default(Nullable<int>)),
                                  //CorrectPercent = (mlr != null ? mlr.CorrectPercent : default(Nullable<short>)),
                                  //CorrectPoint = (mlr != null ? mlr.CorrectPoint : default(Nullable<int>)),
                                HeadLine = (br != null ? br.Headline : null),
                                SentFrom = (br != null ? br.SentFrom : null)
                              } into member_list
                              select member_list).ToList();
            //Member
            var followMemberList = default(List<Member>);
            switch(type)
            {
                case 9:
                    followMemberList = GetFollorMemberList(2); 
                    break;
                case 5:
                case 6:
                case 7:
                    followMemberList = GetFollorMemberList();
                    break;
                case 10: //sportID save memberId in this case
                    followMemberList = GetFollorMemberList(3, sportID ?? 0);
                    break;
                default:
                    followMemberList = new List<Member>(com.Member);
                    break;
            }
            // total views of post
            var contributedReadingSum = (from r in com.ContributionReading
                                         select r).GroupBy(x => x.ContributeId).Select(
                                        l => new
                                        {
                                            ContributeID = l.Key,
                                            ReadingSum = l.Count()
                                        }).ToList();

            // Npb info
            var npbTeamList = new List<TeamInfoMST>(npb.TeamInfoMST);
            var npbPlayerList = new List<PlayerInfoMST>(npb.PlayerInfoMST);
            var npbGameSSList = new List<GameInfoSS>(npb.GameInfoSS);
            // Jlg Info
            var jlgLeagueInfoList = new List<LeagueInfo>(jlg.LeagueInfo);
            var jlgTeamInfoTEList = new List<TeamInfoTE>(jlg.TeamInfoTE);
            var jlgPlayerInfoDIList = new List<PlayerInfoDI>(jlg.PlayerInfoDI);
            var jlgScheduleInfoList = new List<ScheduleInfo>(jlg.ScheduleInfo);
            // Mlb Info
            var mlbTeamInfoList = new List<TeamInfo>(mlb.TeamInfo);
            var mlbScheduleInfoList = (from ss in mlb.SeasonSchedule
                                      join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                                      select new
                                      {
                                          GameID = ss.GameID,
                                          GameDate = dg.GameDate,
                                          HomeTeamFullName = ss.HomeTeamFullName,
                                          VisitorTeamFullName = ss.VisitorTeamFullName
                                      } into gameSS
                                      select gameSS).Distinct().ToList();

            var result = default(IEnumerable<PostedInfoViewModel>);
            if (topicList == null || memberList == null)
                return result;
            var monthlyResultSum = (from m in com.MonthlyResults
                                    group m by new { m.MemberID, m.ReleVantMonth, m.ReleVantYear } into monthGroup
                                    select new
                                    {
                                        MemberID = monthGroup.Key.MemberID,
                                        ReleVantMonth = monthGroup.Key.ReleVantMonth,
                                        ReleVantYear = monthGroup.Key.ReleVantYear,
                                        ExpectNumber = monthGroup.Sum(x => x.ExpectNumber),
                                        CorrectPercent = monthGroup.Sum(x => (x.CorrectPercent * x.ExpectNumber)),
                                        CorrectPoint = monthGroup.Sum(x => x.CorrectPoint)
                                    }).ToList();
            
            var query = from topic in topicList
                         join mb in memberList on topic.TopicID equals mb.TopicID
                         join m in followMemberList on mb.MemberId equals m.MemberId
                         join c in contributedReadingSum on mb.ContributeId equals c.ContributeID into q_readings
                         from views in q_readings.DefaultIfEmpty()
                         join team in npbTeamList on new { c1 = (mb.NewsClassId == 2 ? mb.QuoteUniqueId3 : mb.QuoteUniqueId2), c2 = mb.Status, c3 = (mb.NewsClassId == 2 || mb.NewsClassId == 3) ? 1 : 2, c4 = mb.QuoteUniqueId1 } equals new { c1 = (long)team.TeamCD, c2 = (short)1, c3 = 1, c4 = (long)1 } into npb_team
                         from npbTeam in npb_team.DefaultIfEmpty()
                         join player in npbPlayerList on new { p1 = mb.QuoteUniqueId3, p2 = mb.Status, p3 = mb.NewsClassId, p4 = mb.QuoteUniqueId1 } equals new { p1 = (long)player.PlayerCD, p2 = (short)1, p3 = (long)3, p4 = (long)1 } into npb_player
                         from npbPlayer in npb_player.DefaultIfEmpty()
                         join game in npbGameSSList on new { g1 = mb.QuoteUniqueId2, g2 = mb.Status, g3 = (mb.NewsClassId == 4 || mb.NewsClassId == 5) ? 1 : 2, g4 = mb.QuoteUniqueId1 } equals new { g1 = (long)game.ID, g2 = (short)1, g3 = 1, g4 = (long)1 } into npb_game
                         from npbGame in npb_game.DefaultIfEmpty()
                         join league in jlgLeagueInfoList on new { l1 = mb.QuoteUniqueId2, l2 = mb.Status, l3 = mb.NewsClassId, l4 = mb.QuoteUniqueId1 } equals new { l1 = (long)league.LeagueID, l2 = (short)1, l3 = (long)2, l4 = (long)2 } into jlg_league
                         from jlgLeague in jlg_league.DefaultIfEmpty()
                         join jlgT in jlgTeamInfoTEList on new { t1 = (mb.NewsClassId == 2 ? mb.QuoteUniqueId3 : mb.QuoteUniqueId2), t2 = mb.Status, t3 = (mb.NewsClassId == 2 || mb.NewsClassId == 3) ? 1 : 2, t4 = mb.QuoteUniqueId1 } equals new { t1 = (long)jlgT.TeamID, t2 = (short)1, t3 = 1, t4 = (long)2 } into jlg_team
                         from jlgTeam in jlg_team.DefaultIfEmpty()
                         join jlgP in jlgPlayerInfoDIList on new { t1 = mb.QuoteUniqueId3, t2 = mb.Status, t3 = mb.NewsClassId, t4 = mb.QuoteUniqueId1 } equals new { t1 = (long)jlgP.PlayerID, t2 = (short)1, t3 = (long)3, t4 = (long)2 } into jlg_player
                         from jlgPlayer in jlg_player.DefaultIfEmpty()
                         join jlgSc in jlgScheduleInfoList on new { t1 = mb.QuoteUniqueId2, t2 = mb.Status, t3 = (mb.NewsClassId == 4 || mb.NewsClassId == 5) ? 1 : 2, t4 = mb.QuoteUniqueId1 } equals new { t1 = (long)jlgSc.GameID, t2 = (short)1, t3 = 1, t4 = (long)2 } into jlg_schedule
                         from jlgSchedule in jlg_schedule.DefaultIfEmpty()
                         join mlbT in mlbTeamInfoList on new { t1 = (mb.NewsClassId == 2 ? mb.QuoteUniqueId3 : mb.QuoteUniqueId2), t2 = mb.Status, t3 = (mb.NewsClassId == 2 || mb.NewsClassId == 3) ? 1 : 2, t4 = mb.QuoteUniqueId1 } equals new { t1 = (long)mlbT.TeamID, t2 = (short)1, t3 = 1, t4 = (long)3 } into mlb_team
                         from mlbTeam in mlb_team.DefaultIfEmpty()
                         join mlbSS in mlbScheduleInfoList on new { t1 = mb.QuoteUniqueId2, t2 = mb.Status, t3 = (mb.NewsClassId == 4 || mb.NewsClassId == 5) ? 1 : 2, t4 = mb.QuoteUniqueId1 } equals new { t1 = (long)mlbSS.GameID, t2 = (short)1, t3 = 1, t4 = (long)3 } into mlb_schedule
                         from mlbSchedule in mlb_schedule.DefaultIfEmpty()
                         join mr in monthlyResultSum on new { g1 = (mb.QuoteUniqueId2 % 100), g2 = mb.Status, g3 = mb.NewsClassId, g4 = mb.QuoteUniqueId1, g5 = (mb.QuoteUniqueId2 / 100) }
                          equals new { g1 = (long)mr.ReleVantMonth, g2 = (short)1, g3 = (long)6, g4 = (long)mr.MemberID, g5 = (long)mr.ReleVantYear } into monthlyResult
                        from mlr in monthlyResult.DefaultIfEmpty()
                        select new PostedInfoViewModel
                         {
                             SportID = sportID ?? 0,
                             TopicID = (int)topic.TopicID,
                             ContributeId = mb.ContributeId,
                             ContributeDate = mb.ContributeDate,
                             ModifiedDate = mb.ModifiedDate,
                             Title = mb.Title,
                             ContributedPicture = mb.ContributedPicture,
                             Body = mb.Body,
                             MemberId = m.MemberId,
                             Nickname = m.Nickname,
                             ProfileImg = m.ProfileImg,
                             NewsClassId = mb.NewsClassId,
                             QuoteUniqueId1 = mb.QuoteUniqueId1,
                             QuoteUniqueId2 = mb.QuoteUniqueId2,
                             QuoteUniqueId3 = mb.QuoteUniqueId3,
                             Status = mb.Status,
                             ReleVantYear = (mlr != null ? mlr.ReleVantYear : default(Nullable<Int16>)),
                             ReleVantMonth = (mlr != null ? mlr.ReleVantMonth : default(Nullable<Int16>)),
                             ExpectNumber = (mlr != null ? mlr.ExpectNumber : default(Nullable<Int32>)),
                             CorrectPercent = (mlr != null ? (short)mlr.CorrectPercent : default(Nullable<Int16>)),
                             CorrectPoint = (mlr != null ? mlr.CorrectPoint : default(Nullable<Int32>)),
                             HeadLine = mb.HeadLine,
                             SentFrom = mb.SentFrom,
                             NpbShortNameLeague = (npbTeam != null ? npbTeam.ShortNameLeague : null),
                             NpbTeam = (npbTeam != null ? npbTeam.Team : null),
                             NpbPlayer = (npbPlayer != null ? npbPlayer.Player : null),
                             NpbHomeTeamName = (npbGame != null ? npbGame.HomeTeamName : null),
                             NpbGameDate = (npbGame != null ? npbGame.GameDate : default(Nullable<int>)),
                             NpbVisitorTeamName = (npbGame != null ? npbGame.VisitorTeamName : null),
                             JlgLeagueNameS = (jlgLeague != null ? jlgLeague.LeagueNameS : null),
                             JlgTeamName = (jlgTeam != null ? jlgTeam.TeamName : null),
                             JlgPlayerName = (jlgPlayer != null ? jlgPlayer.PlayerName : null),
                             JlgHomeTeamName = (jlgSchedule != null ? jlgSchedule.HomeTeamName : null),
                             JlgGameDate = (jlgSchedule != null ? jlgSchedule.GameDate : default(Nullable<int>)),
                             JlgAwayTeamName = (jlgSchedule != null ? jlgSchedule.AwayTeamName : null),
                             MlbLeagueName = (mlbTeam != null ? mlbTeam.LeagueName : null),
                             MlbTeamName = (mlbTeam != null ? mlbTeam.TeamFullName : null),
                             MlbHomeTeamName = (mlbSchedule != null ? mlbSchedule.HomeTeamFullName : null),
                             MlbGameDate = (mlbSchedule != null ? mlbSchedule.GameDate : default(Nullable<int>)),
                             MlbAwayTeamName = (mlbSchedule != null ? mlbSchedule.VisitorTeamFullName : null),
                             Views = ((views != null) ? views.ReadingSum : 0),
                             PostMonth = mb.ContributeDate.Value.Month,
                             PostYear = mb.ContributeDate.Value.Year
                         } into posted_info
                         select posted_info;
            if (query != null)
                result = query.GroupBy(c => c.ContributeId).Select(p => p.OrderBy(t => t.TopicID).FirstOrDefault());
                
            return result.OrderByDescending(p => p.ContributeDate);
          
        }
        #endregion

        #region Get Total View of each article
        public static int GetTotalViewOfPost(long contributedId)
        {
            ComEntities com = new ComEntities();
            var contributedReadingSum = (from r in com.ContributionReading
                                         select r).GroupBy(x => x.ContributeId).Select(
                                        l => new
                                        {
                                            ContributeID = l.Key,
                                            ReadingSum = l.Count()
                                        }).ToList();

            var result = (from c in contributedReadingSum
                         where c.ContributeID == contributedId
                         select c).FirstOrDefault();
            if (result != null)
                return result.ReadingSum;
            return 0;
        }
        #endregion

        #region Get Posted original article content (投稿元記事　内容) to string
        /// <summary>
        /// 
        /// </summary>
        /// <param name="needParse">Time need parse.</param>
        /// <returns>week day.</returns>
        public static string GetPostContent(PostedInfoViewModel post)
        {
            string contentDisplayed = string.Empty;
            if (post.Status == 1)
            {
                switch (post.NewsClassId)
                {
                    case 1:
                        contentDisplayed = string.Format("{0} {1}", post.HeadLine, post.SentFrom);
                        break;
                    case 2:
                        switch (post.QuoteUniqueId1)
                        {
                            //NPB
                            case 1:
                                contentDisplayed = string.Format("{0} {1}", post.NpbShortNameLeague, post.NpbTeam);
                                break;
                            //J-League
                            case 2:
                                contentDisplayed = string.Format("{0} {1}", post.JlgLeagueNameS, post.JlgTeamName);
                                break;
                            //MLB
                            case 3:
                                contentDisplayed = string.Format("{0} {1}", post.MlbLeagueName, post.MlbTeamName);
                                break;
                            case 4:
                                break;
                        }
                        break;
                    case 3:
                        switch (post.QuoteUniqueId1)
                        {
                            //NPB
                            case 1:
                                contentDisplayed = string.Format("{0} {1}", post.NpbTeam, post.NpbPlayer);
                                break;
                            //J-League
                            case 2:
                                contentDisplayed = string.Format("{0} {1}", post.JlgTeamName, post.JlgPlayerName);
                                break;
                            //MLB
                            case 3:
                                contentDisplayed = post.MlbTeamName;
                                break;
                            case 4:
                                break;
                        }
                        break;
                    case 4:
                        switch (post.QuoteUniqueId1)
                        {
                            //NPB
                            case 1:
                                if(post.NpbGameDate != null && !string.IsNullOrEmpty(post.NpbHomeTeamName) && !string.IsNullOrEmpty(post.NpbVisitorTeamName))
                                { 
                                    DateTime gameDate = DateTime.ParseExact(string.Format("{0}", post.NpbGameDate), "yyyyMMdd", null);
                                    switch (post.QuoteUniqueId3)
                                    {
                                        case 1:
                                            contentDisplayed = string.Format("{0} {1} vs {2} {3}", gameDate.ToString("MM/dd"), post.NpbHomeTeamName, post.NpbVisitorTeamName, post.NpbHomeTeamName);
                                            break;
                                        case 2:
                                            contentDisplayed = string.Format("{0} {1} vs {2} {3}", gameDate.ToString("MM/dd"), post.NpbHomeTeamName, post.NpbVisitorTeamName, post.NpbVisitorTeamName);
                                            break;
                                        case 3:
                                            contentDisplayed = string.Format("{0} {1} vs {2} 引分け", gameDate.ToString("MM/dd"), post.NpbHomeTeamName, post.NpbVisitorTeamName);
                                            break;
                                    }
                                }
                                break;
                            //J-League
                            case 2:
                                if(post.JlgGameDate != null && !string.IsNullOrEmpty(post.JlgHomeTeamName) && !string.IsNullOrEmpty(post.JlgAwayTeamName))
                                {
                                    DateTime jlgGameDate = DateTime.ParseExact(string.Format("{0}", post.JlgGameDate), "yyyyMMdd", null);
                                    switch (post.QuoteUniqueId3)
                                    {
                                        case 1:
                                            contentDisplayed = string.Format("{0} {1} vs {2} {3}", jlgGameDate.ToString("MM/dd"), post.JlgHomeTeamName, post.JlgAwayTeamName, post.JlgHomeTeamName);
                                            break;
                                        case 2:
                                            contentDisplayed = string.Format("{0} {1} vs {2} {3}", jlgGameDate.ToString("MM/dd"), post.JlgHomeTeamName, post.JlgAwayTeamName, post.JlgAwayTeamName);
                                            break;
                                        case 3:
                                            contentDisplayed = string.Format("{0} {1} vs {2} 引分け", jlgGameDate.ToString("MM/dd"), post.JlgHomeTeamName, post.JlgAwayTeamName);
                                            break;
                                    }
                                }
                                break;
                            //MLB
                            case 3:
                                if (post.MlbGameDate != null && !string.IsNullOrEmpty(post.MlbHomeTeamName) && !string.IsNullOrEmpty(post.MlbAwayTeamName))
                                {
                                    DateTime mlbGameDate = DateTime.ParseExact(string.Format("{0}", post.MlbGameDate), "yyyyMMdd", null);
                                    switch (post.QuoteUniqueId3)
                                    {
                                        case 1:
                                            contentDisplayed = string.Format("{0} {1} vs {2} {3}", mlbGameDate.ToString("MM/dd"), post.MlbHomeTeamName, post.MlbAwayTeamName, post.MlbHomeTeamName);
                                            break;
                                        case 2:
                                            contentDisplayed = string.Format("{0} {1} vs {2} {3}", mlbGameDate.ToString("MM/dd"), post.MlbHomeTeamName, post.MlbAwayTeamName, post.MlbAwayTeamName);
                                            break;
                                        case 3:
                                            contentDisplayed = string.Format("{0} {1} vs {2} 引分け", mlbGameDate.ToString("MM/dd"), post.MlbHomeTeamName, post.MlbAwayTeamName);
                                            break;
                                    }
                                }
                                break;
                            case 4:
                                break;
                        }
                        break;
                    case 5:
                        string strDate = string.Empty;
                        switch (post.QuoteUniqueId1)
                        {
                            //NPB
                            case 1:
                                if (post.NpbGameDate != null)
                                    strDate = DateTime.ParseExact(string.Format("{0}", post.NpbGameDate.Value), "yyyyMMdd", null).ToString("MM/dd");
                                if (!string.IsNullOrEmpty(strDate) && !string.IsNullOrEmpty(post.NpbHomeTeamName) && !string.IsNullOrEmpty(post.NpbVisitorTeamName))
                                    contentDisplayed = string.Format("{0} {1} vs {2}", strDate, post.NpbHomeTeamName, post.NpbVisitorTeamName);
                                break;
                            //J-League
                            case 2:
                                if (post.JlgGameDate != null)
                                    strDate = DateTime.ParseExact(string.Format("{0}", post.JlgGameDate.Value), "yyyyMMdd", null).ToString("MM/dd");
                                if (!string.IsNullOrEmpty(strDate) && !string.IsNullOrEmpty(post.JlgHomeTeamName) && !string.IsNullOrEmpty(post.JlgAwayTeamName))
                                    contentDisplayed = string.Format("{0} {1} vs {2}", strDate, post.JlgHomeTeamName, post.JlgAwayTeamName);
                                break;
                            //MLB
                            case 3:
                                if (post.MlbGameDate != null)
                                    strDate = DateTime.ParseExact(string.Format("{0}", post.MlbGameDate.Value), "yyyyMMdd", null).ToString("MM/dd");
                                if (!string.IsNullOrEmpty(strDate) && !string.IsNullOrEmpty(post.MlbHomeTeamName) && !string.IsNullOrEmpty(post.MlbAwayTeamName))
                                    contentDisplayed = string.Format("{0} {1} vs {2}", strDate, post.MlbHomeTeamName, post.MlbAwayTeamName);
                                break;
                            case 4:
                                break;
                        }
                        break;
                    case 6:
                        switch (post.QuoteUniqueId3)
                        {
                            //的中率
                            case 1:
                                double correctPercent = 0;
                                if(post.ExpectNumber > 0)
                                {
                                    correctPercent = (double)post.CorrectPercent / (double)post.ExpectNumber;
                                }
                                contentDisplayed = string.Format("（{0}月）{1}%", post.ReleVantMonth, correctPercent.ToString(".00"));
                                break;
                            //的中ポイント
                            case 2:
                                contentDisplayed = string.Format("（{0}月）{1}Pt", post.ReleVantMonth, post.CorrectPoint);
                                break;
                            //予想数
                            case 3:
                                contentDisplayed = string.Format("（{0}月）{1}回", post.ReleVantMonth, post.ExpectNumber);
                                break;
                        }
                        break;
                }
            }
            return contentDisplayed;
        }
        #endregion
    }
}
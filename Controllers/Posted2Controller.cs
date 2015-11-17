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
    public class Posted2Controller : PostedController
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
            switch (type)
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
        private static List<Member> GetFollorMemberList(int withLoginMember = 0)
        {
            ComEntities news = new ComEntities();
            HttpContext context = HttpContext.Current;
            var followMemberList = default(List<Member>);
            if (context.Session["CurrentUser"] != null)
            {
                long memberID = Convert.ToInt64(context.Session["CurrentUser"].ToString());
                switch (withLoginMember)
                {
                    case 1:
                        followMemberList = (from f in news.FollowList
                                            join m in news.Member on f.FollowerMemberID equals m.MemberId
                                            where f.MemberID == memberID || m.MemberId == memberID
                                            select m).ToList();
                        break;
                    case 2:
                        followMemberList = (from m in news.Member
                                            where m.MemberId == memberID
                                            select m).ToList();
                        break;
                    default:
                        followMemberList = (from f in news.FollowList
                                            join m in news.Member on f.FollowerMemberID equals m.MemberId
                                            where f.MemberID == memberID
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
        /// <returns>List of topicmaster that has the same id of news</returns>
        public static IEnumerable<PostedInfoViewModel> GetRecentPosts(int type = 0, int? sportID = null, int? teamID = null, int? classificationType = null)
        {
            if (sportID != Constants.NPB_SPORT_ID && sportID != Constants.MLB_SPORT_ID && sportID != Constants.JLG_SPORT_ID)
            {
               return  PostedController.GetRecentPosts(type, sportID, teamID, classificationType);
            }


            //下記はPostedControllerと同じ動作を意図していますが、
            //必要なＤＢアクセスのみに絞るため、後半SportIDで切り分けています

            var result = default(IEnumerable<PostedInfoViewModel>);

            ComEntities com = new ComEntities();

            short currentYear = Convert.ToInt16(DateTime.Now.Year);
            var topicList = GetTopicList(type, sportID, teamID, classificationType);

            if (topicList == null)
                return result;

            var memberList = (from quot in com.QuotTopic
                              join cont in com.Contribution on quot.ContributeID equals cont.ContributeId
                              join contOrg in com.ContributionQuotOrg on cont.ContributeId equals contOrg.ContributeId
                              join brief in com.BriefNews on new { c1 = contOrg.QuoteUniqueId1, c2 = contOrg.Status, c3 = contOrg.NewsClassId } equals new { c1 = brief.NewsItemID, c2 = (short)1, c3 = (long)1 } into brief_news
                              from br in brief_news.DefaultIfEmpty()
                              join mr in com.MonthlyResults on new { g1 = contOrg.QuoteUniqueId2, g2 = contOrg.Status, g3 = contOrg.NewsClassId, g4 = contOrg.QuoteUniqueId1, g5 = currentYear, g6 = sportID.Value }
                                equals new { g1 = (long)mr.ReleVantMonth, g2 = (short)1, g3 = (long)6, g4 = (long)mr.MemberID, g5 = mr.ReleVantYear, g6 = mr.SportsID } into monthlyResult
                              from mlr in monthlyResult.DefaultIfEmpty()
                              select new MemberModel
                              {
                                  TopicID = quot.TopicID,
                                  ContributeId = quot.ContributeID,
                                  ContributeDate = cont.ContributeDate,
                                  Title = cont.Title,
                                  ContributedPicture = cont.ContributedPicture,
                                  Body = cont.Body,
                                  MemberId = cont.MemberId,
                                  NewsClassId = contOrg.NewsClassId,
                                  QuoteUniqueId1 = contOrg.QuoteUniqueId1,
                                  QuoteUniqueId2 = contOrg.QuoteUniqueId2,
                                  QuoteUniqueId3 = contOrg.QuoteUniqueId3,
                                  Status = contOrg.Status,
                                  ReleVantYear = (mlr != null ? mlr.ReleVantYear : default(Nullable<short>)),
                                  ReleVantMonth = (mlr != null ? mlr.ReleVantMonth : default(Nullable<short>)),
                                  ExpectNumber = (mlr != null ? mlr.ExpectNumber : default(Nullable<int>)),
                                  CorrectPercent = (mlr != null ? mlr.CorrectPercent : default(Nullable<short>)),
                                  CorrectPoint = (mlr != null ? mlr.CorrectPoint : default(Nullable<int>)),
                                  HeadLine = (br != null ? br.Headline : null),
                                  SentFrom = (br != null ? br.SentFrom : null)
                              } into member_list
                              select member_list).ToList();

            if (memberList == null)
                return result;

            //Member
            var followMemberList = default(List<Member>);
            switch (type)
            {
                case 9:
                    followMemberList = GetFollorMemberList(2);
                    break;
                case 5:
                case 6:
                case 7:
                    followMemberList = GetFollorMemberList();
                    break;
                default:
                    followMemberList = new List<Member>(com.Member);
                    break;
            }

            // total views of post
            var contributedReadingSum = (from r in com.ContributionReading
                                         select r).GroupBy(x => x.ContributeId).Select(
                                        l => new ContributedReadingSum
                                        {
                                            ContributeID = l.Key,
                                            ReadingSum = l.Count()
                                        }).ToList();
            
            //必要なＤＢアクセスのみに絞るため、後半SportIDで切り分けています
            switch (sportID)
            {
                case Constants.NPB_SPORT_ID:
                    result = getNpb(type, topicList, memberList, followMemberList, contributedReadingSum);
                    break;
                case Constants.MLB_SPORT_ID:
                    result = getMlb(type, topicList, memberList, followMemberList, contributedReadingSum);
                    break;
                case Constants.JLG_SPORT_ID:
                    result = getJlg(type, topicList, memberList, followMemberList, contributedReadingSum);
                    break;
            }

            if (result == null)
                return null;
            else
            {
                try
                {
                    result = result.GroupBy(c => c.ContributeId).Select(p => p.OrderBy(t => t.TopicID).FirstOrDefault());
                    return result.OrderByDescending(p => p.ContributeDate);
                }
                catch
                {
                    return null;
                }
            }

        }
        #endregion

        private static IEnumerable<PostedInfoViewModel> getNpb(int? sportID,
            List<TopicMaster> topicList, List<MemberModel> memberList, List<Member> followMemberList, List<ContributedReadingSum> contributedReadingSum)
        {
            // Npb info
            NpbEntities npb = new NpbEntities();
            var npbTeamList = new List<TeamInfoMST>(npb.TeamInfoMST);
            var npbPlayerList = new List<PlayerInfoMST>(npb.PlayerInfoMST);
            var npbGameSSList = new List<GameInfoSS>(npb.GameInfoSS);


            var query = (from topic in topicList
                         join mb in memberList on topic.TopicID equals mb.TopicID
                         join m in followMemberList on mb.MemberId equals m.MemberId
                         join c in contributedReadingSum on mb.ContributeId equals c.ContributeID into q_readings
                         from views in q_readings.DefaultIfEmpty()
                         join team in npbTeamList
                         on new { c1 = (mb.NewsClassId == 2 ? mb.QuoteUniqueId3 : mb.QuoteUniqueId2), c2 = mb.Status, c3 = (mb.NewsClassId == 2 || mb.NewsClassId == 3) ? 1 : 2, c4 = mb.QuoteUniqueId1 }
                         equals new { c1 = (long)team.TeamCD, c2 = (int)1, c3 = 1, c4 = (long)1 } into npb_team
                         from npbTeam in npb_team.DefaultIfEmpty()
                         join player in npbPlayerList
                         on new { p1 = mb.QuoteUniqueId3, p2 = mb.Status, p3 = mb.NewsClassId, p4 = mb.QuoteUniqueId1 }
                         equals new { p1 = (long)player.PlayerCD, p2 = (int)1, p3 = (long)3, p4 = (long)1 } into npb_player
                         from npbPlayer in npb_player.DefaultIfEmpty()
                         join game in npbGameSSList
                         on new { g1 = mb.QuoteUniqueId2, g2 = mb.Status, g3 = (mb.NewsClassId == 4 || mb.NewsClassId == 5) ? 1 : 2, g4 = mb.QuoteUniqueId1 }
                         equals new { g1 = (long)game.ID, g2 = (int)1, g3 = 1, g4 = (long)1 } into npb_game
                         from npbGame in npb_game.DefaultIfEmpty()
                         select new PostedInfoViewModel
                         {
                             SportID = sportID ?? 0,
                             TopicID = (int)topic.TopicID,
                             ContributeId = mb.ContributeId,
                             ContributeDate = mb.ContributeDate,
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
                             Status = (short)mb.Status,
                             ReleVantYear = mb.ReleVantYear,
                             ReleVantMonth = mb.ReleVantMonth,
                             ExpectNumber = mb.ExpectNumber,
                             CorrectPercent = mb.CorrectPercent,
                             CorrectPoint = mb.CorrectPoint,
                             HeadLine = mb.HeadLine,
                             SentFrom = mb.SentFrom,
                             NpbShortNameLeague = (npbTeam != null ? npbTeam.ShortNameLeague : null),
                             NpbTeam = (npbTeam != null ? npbTeam.Team : null),
                             NpbPlayer = (npbPlayer != null ? npbPlayer.Player : null),
                             NpbHomeTeamName = (npbGame != null ? npbGame.HomeTeamName : null),
                             NpbGameDate = (npbGame != null ? npbGame.GameDate : default(Nullable<int>)),
                             NpbVisitorTeamName = (npbGame != null ? npbGame.VisitorTeamName : null),
                             Views = ((views != null) ? views.ReadingSum : 0),
                             PostMonth = mb.ContributeDate.Value.Month,
                             PostYear = mb.ContributeDate.Value.Year
                         } into posted_info
                         select posted_info).Distinct(new PostedInfoViewModelComparer()).ToList();

            return query;
        }

        private static IEnumerable<PostedInfoViewModel> getMlb(int? sportID,
            List<TopicMaster> topicList, List<MemberModel> memberList, List<Member> followMemberList, List<ContributedReadingSum> contributedReadingSum)
        {
            // Mlb Info
            MlbEntities mlb = new MlbEntities();
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

            var query = (from topic in topicList
                         join mb in memberList on topic.TopicID equals mb.TopicID
                         join m in followMemberList on mb.MemberId equals m.MemberId
                         join c in contributedReadingSum on mb.ContributeId equals c.ContributeID into q_readings
                         from views in q_readings.DefaultIfEmpty()
                         join mlbT in mlbTeamInfoList
                         on new { t1 = (mb.NewsClassId == 2 ? mb.QuoteUniqueId3 : mb.QuoteUniqueId2), t2 = mb.Status, t3 = (mb.NewsClassId == 2 || mb.NewsClassId == 3) ? 1 : 2, t4 = mb.QuoteUniqueId1 }
                         equals new { t1 = (long)mlbT.TeamID, t2 = (int)1, t3 = 1, t4 = (long)3 } into mlb_team
                         from mlbTeam in mlb_team.DefaultIfEmpty()
                         join mlbSS in mlbScheduleInfoList on new { t1 = mb.QuoteUniqueId2, t2 = mb.Status, t3 = (mb.NewsClassId == 4 || mb.NewsClassId == 5) ? 1 : 2, t4 = mb.QuoteUniqueId1 }
                         equals new { t1 = (long)mlbSS.GameID, t2 = (int)1, t3 = 1, t4 = (long)3 } into mlb_schedule
                         from mlbSchedule in mlb_schedule.DefaultIfEmpty()
                         select new PostedInfoViewModel
                         {
                             SportID = sportID ?? 0,
                             TopicID = (int)topic.TopicID,
                             ContributeId = mb.ContributeId,
                             ContributeDate = mb.ContributeDate,
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
                             Status = (short)mb.Status,
                             ReleVantYear = mb.ReleVantYear,
                             ReleVantMonth = mb.ReleVantMonth,
                             ExpectNumber = mb.ExpectNumber,
                             CorrectPercent = mb.CorrectPercent,
                             CorrectPoint = mb.CorrectPoint,
                             HeadLine = mb.HeadLine,
                             SentFrom = mb.SentFrom,
                             MlbLeagueName = (mlbTeam != null ? mlbTeam.LeagueName : null),
                             MlbTeamName = (mlbTeam != null ? mlbTeam.TeamFullName : null),
                             MlbHomeTeamName = (mlbSchedule != null ? mlbSchedule.HomeTeamFullName : null),
                             MlbGameDate = (mlbSchedule != null ? mlbSchedule.GameDate : default(Nullable<int>)),
                             MlbAwayTeamName = (mlbSchedule != null ? mlbSchedule.VisitorTeamFullName : null),
                             Views = ((views != null) ? views.ReadingSum : 0),
                             PostMonth = mb.ContributeDate.Value.Month,
                             PostYear = mb.ContributeDate.Value.Year
                         } into posted_info
                         select posted_info).Distinct(new PostedInfoViewModelComparer()).ToList();

            return query;
        }

        private static IEnumerable<PostedInfoViewModel> getJlg(int? sportID,
            List<TopicMaster> topicList, List<MemberModel> memberList, List<Member> followMemberList, List<ContributedReadingSum> contributedReadingSum)
        {
            // Jlg Info
            JlgEntities jlg = new JlgEntities();
            var jlgLeagueInfoList = new List<LeagueInfo>(jlg.LeagueInfo);
            var jlgTeamInfoTEList = new List<TeamInfoTE>(jlg.TeamInfoTE);
            var jlgPlayerInfoDIList = new List<PlayerInfoDI>(jlg.PlayerInfoDI);
            var jlgScheduleInfoList = new List<ScheduleInfo>(jlg.ScheduleInfo);

            var query = (from topic in topicList
                         join mb in memberList on topic.TopicID equals mb.TopicID
                         join m in followMemberList on mb.MemberId equals m.MemberId
                         join c in contributedReadingSum on mb.ContributeId equals c.ContributeID into q_readings
                         from views in q_readings.DefaultIfEmpty()
                         join league in jlgLeagueInfoList
                         on new { l1 = mb.QuoteUniqueId2, l2 = mb.Status, l3 = mb.NewsClassId, l4 = mb.QuoteUniqueId1 }
                         equals new { l1 = (long)league.LeagueID, l2 = (int)1, l3 = (long)2, l4 = (long)2 } into jlg_league
                         from jlgLeague in jlg_league.DefaultIfEmpty()
                         join jlgT in jlgTeamInfoTEList
                         on new { t1 = (mb.NewsClassId == 2 ? mb.QuoteUniqueId3 : mb.QuoteUniqueId2), t2 = mb.Status, t3 = (mb.NewsClassId == 2 || mb.NewsClassId == 3) ? 1 : 2, t4 = mb.QuoteUniqueId1 }
                         equals new { t1 = (long)jlgT.TeamID, t2 = (int)1, t3 = 1, t4 = (long)2 } into jlg_team
                         from jlgTeam in jlg_team.DefaultIfEmpty()
                         join jlgP in jlgPlayerInfoDIList
                         on new { t1 = mb.QuoteUniqueId3, t2 = mb.Status, t3 = mb.NewsClassId, t4 = mb.QuoteUniqueId1 }
                         equals new { t1 = (long)jlgP.PlayerID, t2 = (int)1, t3 = (long)3, t4 = (long)2 } into jlg_player
                         from jlgPlayer in jlg_player.DefaultIfEmpty()
                         join jlgSc in jlgScheduleInfoList
                         on new { t1 = mb.QuoteUniqueId2, t2 = mb.Status, t3 = (mb.NewsClassId == 4 || mb.NewsClassId == 5) ? 1 : 2, t4 = mb.QuoteUniqueId1 }
                         equals new { t1 = (long)jlgSc.GameID, t2 = (int)1, t3 = 1, t4 = (long)2 } into jlg_schedule
                         from jlgSchedule in jlg_schedule.DefaultIfEmpty()
                         select new PostedInfoViewModel
                         {
                             SportID = sportID ?? 0,
                             TopicID = (int)topic.TopicID,
                             ContributeId = mb.ContributeId,
                             ContributeDate = mb.ContributeDate,
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
                             Status = (short)mb.Status,
                             ReleVantYear = mb.ReleVantYear,
                             ReleVantMonth = mb.ReleVantMonth,
                             ExpectNumber = mb.ExpectNumber,
                             CorrectPercent = mb.CorrectPercent,
                             CorrectPoint = mb.CorrectPoint,
                             HeadLine = mb.HeadLine,
                             SentFrom = mb.SentFrom,
                             JlgLeagueNameS = (jlgLeague != null ? jlgLeague.LeagueNameS : null),
                             JlgTeamName = (jlgTeam != null ? jlgTeam.TeamName : null),
                             JlgPlayerName = (jlgPlayer != null ? jlgPlayer.PlayerName : null),
                             JlgHomeTeamName = (jlgSchedule != null ? jlgSchedule.HomeTeamName : null),
                             JlgGameDate = (jlgSchedule != null ? jlgSchedule.GameDate : default(Nullable<int>)),
                             JlgAwayTeamName = (jlgSchedule != null ? jlgSchedule.AwayTeamName : null),
                             Views = ((views != null) ? views.ReadingSum : 0),
                             PostMonth = mb.ContributeDate.Value.Month,
                             PostYear = mb.ContributeDate.Value.Year
                         } into posted_info
                         select posted_info).Distinct(new PostedInfoViewModelComparer()).ToList();

            return query;
        }


        public class ContributedReadingSum
        {

            public long ContributeID { get; set; }
            public int ReadingSum { get; set; }
        }

        public class MemberModel
        {

            public long TopicID { get; set; }
            public long ContributeId { get; set; }
            public DateTime? ContributeDate { get; set; }
            public string Title { get; set; }
            public string ContributedPicture { get; set; }
            public string Body { get; set; }
            public long MemberId { get; set; }
            public long NewsClassId { get; set; }
            public long QuoteUniqueId1 { get; set; }
            public long QuoteUniqueId2 { get; set; }
            public long QuoteUniqueId3 { get; set; }
            public int Status { get; set; }
            public Nullable<short> ReleVantYear { get; set; }
            public Nullable<short> ReleVantMonth { get; set; }
            public Nullable<int> ExpectNumber { get; set; }
            public Nullable<short> CorrectPercent { get; set; }
            public Nullable<int> CorrectPoint { get; set; }
            public string HeadLine { get; set; }
            public string SentFrom { get; set; }

        }
    }
}
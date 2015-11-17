using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Splg.Areas.Jleague;
using Splg.Areas.Jleague.Controllers;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.Mlb;
using Splg.Areas.Mlb.Controllers;
using Splg.Areas.Mlb.Models;
using Splg.Areas.Mlb.Models.ViewModel;
using Splg.Areas.Npb.Models.ViewModel;
using Splg.Areas.Npb.Controllers;
using Splg.Areas.Npb.Models;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Core.Extend;
using Splg.Models;
using Splg.Models.Game.InfoModel;
using Splg.Models.Game.ViewModel;
using Splg.Services.Game;
using Splg.Core.Constant;

namespace Splg.Areas.MyPage
{

    public class MyPageCommon
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        static ComEntities com = new ComEntities();

        static NpbEntities npb = new NpbEntities();
        static JlgEntities jlg = new JlgEntities();
        static MlbEntities mlb = new MlbEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //MyPageEntities mypage = new MyPageEntities();

        #endregion

        public const int CLASSCLASS_TEAM = 2;
        public const int CLASSCLASS_GAME = 4;

        public static IEnumerable<MyPageGroupListViewModel.GroupListInfo> GetGroupLists(ComEntities com, Int64 memberId)
        {
            List<MyPageGroupListViewModel.GroupListInfo> result = new List<MyPageGroupListViewModel.GroupListInfo> { };

            if (memberId > 0)
            {
                var query = (from g in com.Groups
                             join gm in com.GroupMember
                             on g.GroupID equals gm.GroupID
                             where gm.MemberID == memberId //自分が所属するグループを表示
                             && g.Status != false
                             orderby g.CreatedDate descending
                             select new MyPageGroupListViewModel.GroupListInfo
                             {
                                 GroupID = g.GroupID,
                                 GroupName = g.GroupName,
                                 NumberOfMember = (from m in com.GroupMember where m.GroupID == g.GroupID select m).Count()
                             }
                            );

                result = query.ToList();
            }

            return result;
        }

        public static string GetFolowerQeery(long memberId, int year, int month)
        {
            string query = "SELECT MemberID," +
                            "       ISNULL(SUM(PayOffPoints), 0) AS PayOffPoints," +
                            "       ISNULL(SUM(fundspoint), 0) AS FundsPoint," +
                            "       ISNULL(SUM(possesionpoint), 0) AS PossesionPoint," +
                            "       MAX(ExpectPoint_CreatedDate) AS LastExpectedPointDate " +
                            "FROM [Splg].[Com].[Point] p " +
                            "LEFT JOIN" +
                            "  (SELECT PointID," +
                            "          MAX(CreatedDate) AS ExpectPoint_CreatedDate" +
                            "   FROM [Splg].[Com].[ExpectPoint]" +
                            "   GROUP BY PointID) e ON p.PointID = e.PointID " +
                            "WHERE p.PayOffFlg = '1'" +
                            "  AND p.MemberID IN" +
                            "    (SELECT MemberID" +
                            "     FROM [Splg].[Com].[FollowList]" +
                            "     WHERE FollowerMemberID = " + memberId +
                            "  AND p.GiveTargetYear =  " + year +
                            "  AND p.GiveTargetMonth = " + month +
                            " GROUP BY p.memberid";
            return query;
        }


        public static void SendMail(long memberID, string title, string body)
        {
            var com = new ComEntities();
            using (var transaction = com.Database.BeginTransaction())
            {

                //send mail 
                var emailSender = new EmailSender();

                try
                {
                    string email = (from m in com.Member where m.MemberId == memberID select m.Mail).FirstOrDefault();

                    string result = emailSender.SendEmail2(email, body, title, null);

                    if (result.Equals(Constants.EMAIL_SEND))
                    {
                        List<MailDeliverCond> mailTypeList = new List<MailDeliverCond>();
                        foreach (var item in com.MailDeliverCondMaster)
                        {
                            // Set value for new row in MailDeliverCond table
                            var newMailCondRow = new MailDeliverCond
                            {
                                MemberID = memberID,
                                MailDelivCondID = item.MailDelivCondID,
                                CreatedDate = System.DateTime.Now
                            };
                            mailTypeList.Add(newMailCondRow);
                        }
                        //Insert to MailDeliverCond table
                        com.MailDeliverCond.AddRange(mailTypeList);
                        com.SaveChanges();

                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }

        }
        // Todo Serviceに移動

        /// <summary>
        /// 予想一覧向け試合情報の取得
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="target_year"></param>
        /// <param name="target_month"></param>
        /// <param name="target_date"></param>
        /// <returns></returns>
        public static IEnumerable<GameInfoModel> GetGameInfo(Int64 memberId, int target_year, int target_month, int target_date = 0, int sportsId = 0)
        {

            bool articleDispFlg;

            List<GameInfoModel> result;

            OddsService oddsService = new OddsService();

            #region 情報取得する日付の開始と終了を取得し、投稿記事のリンク表示フラグを設定する
            DateTime startDate;
            DateTime endDate;
            switch (target_date)
            {
                // 日付指定がない場合
                case 0:
                    // 当月最初の日付を取得
                    startDate = Convert.ToDateTime(target_year + "/" + target_month + "/01 00:00:00");
                    // 当月の最後の日付を取得
                    endDate = Convert.ToDateTime(target_year + "/" + target_month + "/01 23:59:59").AddMonths(1).AddDays(-1);
                    // 投稿記事を表示
                    articleDispFlg = true;
                    break;
                // 日付指定の場合
                default:
                    // 指定日の前日を取得
                    startDate = Convert.ToDateTime(target_year + "/" + target_month + "/" + target_date + " 00:00:00");
                    // 指定日を取得
                    endDate = Convert.ToDateTime(target_year + "/" + target_month + "/" + target_date + " 23:59:59");
                    // 投稿記事を表示
                    articleDispFlg = false;
                    break;
            }

            int startDateInt = startDate.ParseToInt();
            int endDateInt = endDate.ParseToInt();
            #endregion

            //NPBゲームの取得
            List<GameInfoModel> nbpGames = null;
            IEnumerable<GameInfoViewModelForNBP> npbTmp = null;

            if (sportsId == 0 || sportsId == Constants.NPB_SPORT_ID)
            {
                //予想情報の取得
                var npbExpectTargets = GetPointOfUser(memberId, Constants.NPB_SPORT_ID, startDate, endDate);

                if (npbExpectTargets != null)
                {
                    var npbGameIds = (from t in npbExpectTargets select t.GameID).Distinct().ToList();
                    NpbTopController NpbCtl = new NpbTopController();
                    npbTmp = NpbCtl.GetGameInfo(null, startDateInt, endDateInt, null, null, npbGameIds);
                }

                //TODO GetGameInfoで日付を指定するからには、NULLだった場合の対処が必要ではないか？
                if (npbTmp != null)
                {
                    nbpGames = (from g in npbTmp.ToList()
                                join e in npbExpectTargets on g.GameID equals e.GameID
                                select new GameInfoModel
                                {
                                    TargetYear = target_year,
                                    TargetMonth = target_month,
                                    StartScheduleDate = e.StartScheduleDate,
                                    SportsID = e.SportsID,
                                    ExpectTargetID = e.ExpectTargetID,
                                    GameID = g.GameID,
                                    TeamName = e.BetSelectID == 1 ? g.HomeTeamName : e.BetSelectID == 2 ? g.VisitorTeamName : "引き分け",
                                    TeamNameS = e.BetSelectID == 1 ? g.HomeTeamNameS : e.BetSelectID == 2 ? g.VisitorTeamNameS : "引き分け",
                                    Url = "/npb/game/" + g.GameID + "",
                                    UrlArticle = "/mypage/article/",
                                    HomeTeamName = g.HomeTeamName,
                                    HomeTeamID = g.HomeTeamID,
                                    HomeTeamS = g.HomeTeamNameS,
                                    VisitorTeamName = g.VisitorTeamName,
                                    VisitorTeamS = g.VisitorTeamNameS,
                                    VisitorTeamID = g.VisitorTeamID,
                                    HomeTeamIcon = g.HomeTeamIcon,
                                    VisitorTeamIcon = g.VisitorTeamIcon,
                                    HomeTeamRanking = g.HomeTeamRanking,
                                    VisitorTeamRanking = g.VisitorTeamRanking,
                                    HomeTeamWin = g.HomeTeamWin,
                                    VisitorTeamWin = g.VisitorTeamWin,
                                    HomeTeamUrl = "/npb/teams/" + g.HomeTeamID + "",
                                    VisitorTeamUrl = "/npb/teams/" + g.VisitorTeamID + "",
                                    HomeScore = Convert.ToString(g.HomeTeamScore),
                                    VisitorScore = Convert.ToString(g.VisitorTeamScore),
                                    GameDateScheduled = g.GameDate,
                                    GameTimeScheduled = g.Time,
                                    OddsInfoModel = oddsService.GetOddsInfoByGameID(Constants.NPB_SPORT_ID, g.GameID, memberId),
                                    PreForeRunnerNameSH = g.PreForeRunnerNameSH,
                                    PreForeRunnerEraH = g.PreForeRunnerEraH,
                                    PreForeRunnerNameSV = g.PreForeRunnerNameSV,
                                    PreForeRunnerEraV = g.PreForeRunnerEraV,
                                    ForeRunnerNameSH = g.ForeRunnerNameSH,
                                    ForeRunnerEraH = g.ForeRunnerEraH,
                                    ForeRunnerNameSV = g.ForeRunnerNameSV,
                                    ForeRunnerEraV = g.ForeRunnerEraV,
                                    WinLoseNameSH = g.WinLoseNameSH,
                                    WinLoseEraH = g.WinLoseEraH,
                                    WinLoseNameSV = g.WinLoseNameSV,
                                    WinLoseEraV = g.WinLoseEraV,
                                    IsWinH = g.IsWinH,
                                    IsWinV = g.IsWinV,
                                    WinLoseTextH = g.WinLoseTextH,
                                    WinLoseTextV = g.WinLoseTextV,

                                    WinLose = Convert.ToInt16(e.VorD), //勝敗1:勝ち 2:負け,
                                    BetSelectID = e.BetSelectID,
                                    SituationStatus = e.SituationStatus,
                                    ExpectPoint1 = e.ExpectPoint,
                                    StartDt = startDate

                                }).ToList();
                }
            }

            //MLBゲームの取得
            List<GameInfoModel> mlbGames = null;
            IEnumerable<GameInfoViewModelForMLB> mlbTmp = null;

            if (sportsId == 0 || sportsId == Constants.MLB_SPORT_ID)
            {
                var mlbExpectTargets = GetPointOfUser(memberId, Constants.MLB_SPORT_ID, startDate, endDate);

                if (mlbExpectTargets != null)
                {
                    MlbTopController MlbCtl = new MlbTopController();
                    var mlbGameIds = (from t in mlbExpectTargets select t.GameID).Distinct().ToList();

                    if (mlbGameIds != null)
                        mlbTmp = MlbCtl.GetGameInfo(null, startDateInt, endDateInt, null, null, mlbGameIds);
                }

                //TODO GetGameInfoで日付を指定するからには、NULLだった場合の対処が必要ではないか？
                if (mlbTmp != null)
                {
                    mlbGames = (from g in mlbTmp.ToList()
                                join e in mlbExpectTargets on g.GameID equals e.GameID
                                select new GameInfoModel
                                {
                                    TargetYear = target_year,
                                    TargetMonth = target_month,
                                    StartScheduleDate = e.StartScheduleDate,
                                    SportsID = e.SportsID,
                                    ExpectTargetID = e.ExpectTargetID,
                                    GameID = g.GameID,
                                    TeamName = e.BetSelectID == 1 ? g.HomeTeamName : e.BetSelectID == 2 ? g.VisitorTeamName : "引き分け",
                                    TeamNameS = e.BetSelectID == 1 ? g.HomeTeamNameS : e.BetSelectID == 2 ? g.VisitorTeamNameS : "引き分け",
                                    Url = "/mlb/game/" + g.GameID + "",
                                    UrlArticle = "/mypage/article/",
                                    HomeTeamName = g.HomeTeamName,
                                    HomeTeamID = g.HomeTeamID,
                                    HomeTeamS = g.HomeTeamNameS,
                                    VisitorTeamName = g.VisitorTeamName,
                                    VisitorTeamS = g.VisitorTeamNameS,
                                    VisitorTeamID = g.VisitorTeamID,
                                    HomeTeamIcon = g.HomeTeamIcon,
                                    VisitorTeamIcon = g.VisitorTeamIcon,
                                    HomeTeamRanking = g.HomeTeamRanking,
                                    VisitorTeamRanking = g.VisitorTeamRanking,
                                    HomeTeamWin = g.HomeTeamWin,
                                    VisitorTeamWin = g.VisitorTeamWin,
                                    HomeTeamUrl = "/mlb/teams/" + g.HomeTeamID + "",
                                    VisitorTeamUrl = "/mlb/teams/" + g.VisitorTeamID + "",
                                    HomeScore = Convert.ToString(g.HomeTeamScore),
                                    VisitorScore = Convert.ToString(g.VisitorTeamScore),
                                    GameDateScheduled = g.GameDate,
                                    GameTimeScheduled = g.Time,
                                    OddsInfoModel = oddsService.GetOddsInfoByGameID(Constants.MLB_SPORT_ID, g.GameID, memberId),
                                    PreForeRunnerNameSH = g.PreForeRunnerNameSH,
                                    PreForeRunnerNameSV = g.PreForeRunnerNameSV,
                                    ForeRunnerNameSH = g.ForeRunnerNameSH,
                                    ForeRunnerNameSV = g.ForeRunnerNameSV,

                                    WinLose = Convert.ToInt16(e.VorD), //勝敗1:勝ち 2:負け,
                                    BetSelectID = e.BetSelectID,
                                    SituationStatus = e.SituationStatus,
                                    ExpectPoint1 = e.ExpectPoint,
                                    StartDt = startDate

                                }).ToList();
                }
            }


            //JLGゲームの取得
            List<GameInfoModel> jlgGames = null;
            IEnumerable<JlgGameInfos> jlgTmp = null;

            if (sportsId == 0 || sportsId == Constants.JLG_SPORT_ID)
            {
                var jlgExpectTargets = GetPointOfUser(memberId, Constants.JLG_SPORT_ID, startDate, endDate);

                if (jlgExpectTargets != null)
                {
                    JlgTopController jlgCtl = new JlgTopController();
                    var jlgGameIds = (from t in jlgExpectTargets select t.GameID).Distinct().ToList();

                    if (jlgGameIds != null)
                        jlgTmp = jlgCtl.GetGameInfoByDate(null, startDateInt, endDateInt, null, null, jlgGameIds);
                }

                //TODO GetGameInfoで日付を指定するからには、NULLだった場合の対処が必要ではないか？
                if (jlgTmp != null)
                {
                    jlgGames = (from g in jlgTmp.ToList()
                                join e in jlgExpectTargets on g.GameID equals e.GameID
                                select new GameInfoModel
                                {
                                    TargetYear = target_year,
                                    TargetMonth = target_month,
                                    StartScheduleDate = e.StartScheduleDate,
                                    SportsID = e.SportsID,
                                    ExpectTargetID = e.ExpectTargetID,
                                    GameID = g.GameID,
                                    TeamName = e.BetSelectID == 1 ? g.HomeTeamName : e.BetSelectID == 2 ? g.AwayTeamName : "引き分け",
                                    TeamNameS = e.BetSelectID == 1 ? g.HomeTeamNameS : e.BetSelectID == 2 ? g.AwayTeamNameS : "引き分け",
                                    Url = "/jleague/game/" + g.GameID + "",
                                    UrlArticle = "/mypage/article/",
                                    HomeTeamName = g.HomeTeamName,
                                    HomeTeamID = Convert.ToInt32(g.HomeTeamID),
                                    HomeTeamS = g.HomeTeamNameS,
                                    VisitorTeamName = g.AwayTeamName,
                                    VisitorTeamS = g.AwayTeamNameS,
                                    VisitorTeamID = g.AwayTeamID,
                                    HomeTeamIcon = g.HomeTeamIcon,
                                    VisitorTeamIcon = g.AwayTeamIcon,
                                    HomeTeamRanking = g.HomeTeamRanking,
                                    VisitorTeamRanking = g.AwayTeamRanking,
                                    HomeTeamWin = g.HomeTeamWin,
                                    VisitorTeamWin = g.AwayTeamWin,
                                    HomeTeamUrl = "/jleague/teams/" + g.HomeTeamID + "",
                                    VisitorTeamUrl = "/jleague/teams/" + g.AwayTeamID + "",
                                    HomeScore = g.HomeTeamScore.ToString(),
                                    VisitorScore = g.AwayTeamScore.ToString(),
                                    GameDateScheduled = g.GameDate,
                                    GameTimeScheduled = g.Time,
                                    OddsInfoModel = oddsService.GetOddsInfoByGameID(Constants.JLG_SPORT_ID, g.GameID, memberId),

                                    WinLose = Convert.ToInt16(e.VorD), //勝敗1:勝ち 2:負け,
                                    BetSelectID = e.BetSelectID,
                                    SituationStatus = e.SituationStatus,
                                    ExpectPoint1 = e.ExpectPoint,
                                    StartDt = startDate
                                }).ToList();
                }
            }

            ScoreGameInfo lstScoreHome;
            ScoreGameInfo lstScoreVisitor;

            if (nbpGames != null)
            {
                foreach (var g in nbpGames)
                {
                    // 投稿記事表示フラグ
                    g.ArticleDispFlg = articleDispFlg;

                    //実際の試合日時
                    var giSS = (from gameInfo in npb.GameInfoSS
                                where gameInfo.ID == g.GameID
                                select gameInfo).FirstOrDefault();
                    if (giSS != null)
                    {
                        g.GameDateActual = giSS.GameDate;
                        g.GameTimeActual = giSS.Time;
                    }

                    //スコア
                    lstScoreHome = NpbCommon.GetTeamInfoGTSByGameIDTeamID(g.GameID, g.HomeTeamID);
                    lstScoreVisitor = NpbCommon.GetTeamInfoGTSByGameIDTeamID(g.GameID, g.VisitorTeamID);
                    if (lstScoreHome != null && lstScoreVisitor != null)
                    {
                        g.BottomTop = Utils.GetRoundName(lstScoreHome.TB);
                        g.Inning = lstScoreHome.Inning == null ? 0 : (int)lstScoreHome.Inning;
                    }

                    g.GameStatus = NpbCommon.GetStatusMatch(g.GameID, memberId.ToString());
                    //g.GameStatusSimple = NpbCommon.GetStatusMatch(g.GameID.ToString());

                    if (g.GameStatus == 6 || g.GameStatus == 7)
                    {
                        g.GameDateTime = "試合中 " + g.Inning + g.BottomTop;

                    }

                    // 試合の勝利チーム
                    var winteam = (from gi in npb.GameInfoRGI
                                   join ri in npb.GameResultInfoRGI
                               on gi.RealGameInfoRootRGIId equals ri.RealGameInfoRootRGIId
                                   where
                                   gi.GameID == g.GameID   // 試合
                                   select ri).FirstOrDefault();
                    if (winteam != null)
                    {

                        var win_teamcd = winteam.WinTeamCD;
                        if (win_teamcd == g.HomeTeamID)
                        {
                            // g.TeamName = g.HomeTeamName;
                            // g.TeamNameS = g.HomeTeamS;
                            g.WinnerTeam = 1;
                        }
                        else if (win_teamcd == g.VisitorTeamID)
                        {
                            // g.TeamName = g.VisitorTeamName;
                            // g.TeamNameS = g.VisitorTeamS;
                            g.WinnerTeam = 2;
                        }
                    }
                }
            }

            if (mlbGames != null)
            {
                foreach (var g in mlbGames)
                {
                    // 投稿記事表示フラグ
                    g.ArticleDispFlg = articleDispFlg;

                    //実際の試合日時
                    var ssi = (from ss in mlb.SeasonSchedule
                               join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                               where ss.GameID == g.GameID
                               select new
                               {
                                   GameDate = dg.GameDateJPN,
                                   Time = ss.Time
                               }).FirstOrDefault();

                    if (ssi != null)
                    {
                        g.GameDateActual = ssi.GameDate;
                        g.GameTimeActual = ssi.Time;
                    }

                    //イニング情報とスコアを得る
                    lstScoreHome = MlbCommon.GetTeamInfoGTSByGameIDTeamIDHome(g.GameID, g.HomeTeamID);
                    lstScoreVisitor = MlbCommon.GetTeamInfoGTSByGameIDTeamIDVisitor(g.GameID, g.VisitorTeamID);

                    if (lstScoreHome != null && lstScoreVisitor != null)
                    {
                        g.BottomTop = Utils.GetRoundName(lstScoreHome.TB);
                        g.Inning = lstScoreHome.Inning == null ? 0 : (int)lstScoreHome.Inning;
                    }

                    //ゲームの状態
                    g.GameStatus = MlbCommon.GetStatusMatch(g.GameID, memberId.ToString());
                    //g.GameStatusSimple = MlbCommon.GetStatusMatch(g.GameID.ToString());

                    if (g.GameStatus == 6 || g.GameStatus == 7)
                    {
                        g.GameDateTime = "試合中";

                    }
                    // 試合の勝利チームを得る
                    // MlbTeamInfoDailyResultController.csから引用
                    var query = from realGameInfo in mlb.RealGameInfo
                                join seasonSchedule in mlb.SeasonSchedule on realGameInfo.GameID equals seasonSchedule.GameID
                                join dayGroup in mlb.DayGroup on seasonSchedule.DayGroupId equals dayGroup.DayGroupId
                                where (dayGroup.GameDateJPN.Value == g.GameDateScheduled) && (realGameInfo.GameID == g.GameID && realGameInfo.HomeTeamID == g.HomeTeamID)
                                select new
                                {
                                    WinTeamCD = realGameInfo.HomeScore > realGameInfo.VisitorScore ? realGameInfo.HomeTeamID : (realGameInfo.HomeScore < realGameInfo.VisitorScore ? realGameInfo.VisitorTeamID : null),
                                    WinTeamName = realGameInfo.HomeScore > realGameInfo.VisitorScore ? realGameInfo.HomeTeamName : (realGameInfo.HomeScore < realGameInfo.VisitorScore ? realGameInfo.VisitorTeamName : null)
                                };
                    if (query != null)
                    {
                        if (query.FirstOrDefault() != null)
                        {
                            var mlb_winteam = query.FirstOrDefault().WinTeamCD;
                            if (mlb_winteam == g.HomeTeamID)
                            {
                                //  g.TeamName = g.HomeTeamName;
                                //  g.TeamNameS = g.HomeTeamS;
                                g.WinnerTeam = 1;
                            }
                            else if (mlb_winteam == g.VisitorTeamID)
                            {
                                //  g.TeamName = g.VisitorTeamName;
                                //  g.TeamNameS = g.VisitorTeamS;
                                g.WinnerTeam = 2;
                            }
                        }
                    }

                }
            }

            if (jlgGames != null)
            {
                foreach (var g in jlgGames)
                {
                    // 投稿記事表示フラグ
                    g.ArticleDispFlg = articleDispFlg;

                    //実際の試合日時
                    lstScoreHome = JlgCommon.GetTeamInfoGTSByGameIDTeamID(g.GameID, g.HomeTeamID);
                    lstScoreVisitor = JlgCommon.GetTeamInfoGTSByGameIDTeamID(g.GameID, g.VisitorTeamID);

                    var grlg = (from gameReportLG in jlg.GameReportLG
                                where gameReportLG.GameID == g.GameID
                                select gameReportLG).FirstOrDefault();

                    if (grlg != null)
                    {
                        g.GameDateActual = grlg.GameDate;
                        g.GameTimeActual = grlg.StartTime.ToString();
                    }

                    //ゲームの状態
                    g.GameStatus = JlgCommon.GetStatusMatch(g.GameID, memberId.ToString());
                    //g.GameStatusSimple = JlgCommon.GetStatusMatch(g.GameID.ToString());

                    if (g.GameStatus == 6 || g.GameStatus == 7)
                    {
                        g.GameDateTime = "試合中";
                    }

                    // 試合の勝利チームを得る
                    int WinTeamCD = lstScoreHome.R > lstScoreVisitor.R ? g.HomeTeamID : lstScoreHome.R < lstScoreVisitor.R ? g.VisitorTeamID : 0;
                    if (WinTeamCD == g.HomeTeamID) // Home勝ち点３
                    {
                        // ホーム勝ちののBetSelectID
                        g.WinnerTeam = (int)BetConst.BetSelectID.Home;
                    }
                    else if (WinTeamCD == g.VisitorTeamID)
                    {
                        // アウェー勝ちののBetSelectID
                        g.WinnerTeam = (int)BetConst.BetSelectID.Visitor;
                    }
                    else
                    {
                        // 引き分けのBetSelectID
                        g.WinnerTeam = (int)BetConst.BetSelectID.Draw;
                    }
                }
            }


            List<GameInfoModel> allGames = new List<GameInfoModel> { };

            if (nbpGames != null)
                allGames.AddRange(nbpGames);
            if (mlbGames != null)
                allGames.AddRange(mlbGames);
            if (jlgGames != null)
                allGames.AddRange(jlgGames);

            result = allGames.OrderByDescending(s => s.usingDate).ThenByDescending(t => t.usingTime).ToList();

            return result;
        }



        /// <summary>
        /// サイトTopとXXXTopコントローラ向け試合情報の取得
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="target_year"></param>
        /// <param name="target_month"></param>
        /// <param name="target_date"></param>
        /// <returns></returns>
        public static IEnumerable<GameInfoModel> GetGameInfoForTop(Int64 memberId, int target_year, int target_month, int target_date = 0, int sportsId = 0)
        {
            #region 情報取得する日付の開始と終了を取得し、投稿記事のリンク表示フラグを設定する
            DateTime startDate;
            DateTime endDate;
            switch (target_date)
            {
                // 日付指定がない場合
                case 0:
                    // 当月最初の日付を取得
                    startDate = Convert.ToDateTime(target_year + "/" + target_month + "/01 00:00:00");
                    // 当月の最後の日付を取得
                    endDate = Convert.ToDateTime(target_year + "/" + target_month + "/01 23:59:59").AddMonths(1).AddDays(-1);
                    break;
                // 日付指定の場合
                default:
                    // 指定日の前日を取得
                    startDate = Convert.ToDateTime(target_year + "/" + target_month + "/" + target_date + " 00:00:00");
                    // 指定日を取得
                    endDate = Convert.ToDateTime(target_year + "/" + target_month + "/" + target_date + " 23:59:59");
                    break;
            }

            #endregion

            var oddsService = new OddsService();

            #region NPBゲームの取得

            List<GameInfoModel> nbpGames = null;

            if (sportsId == 0 || sportsId == Constants.NPB_SPORT_ID)
            {
                //予想情報の取得
                var npbExpectTargets = GetPointOfUser(memberId, Constants.NPB_SPORT_ID, startDate, endDate);

                nbpGames = (from e in npbExpectTargets
                            select new GameInfoModel
                            {
                                TargetYear = target_year,
                                TargetMonth = target_month,
                                StartScheduleDate = e.StartScheduleDate,
                                SportsID = e.SportsID,
                                ExpectTargetID = e.ExpectTargetID,
                                GameID = e.GameID,
                                UrlArticle = "/mypage/article/",

                                WinLose = Convert.ToInt16(e.VorD), //勝敗1:勝ち 2:負け,
                                BetSelectID = e.BetSelectID,
                                SituationStatus = e.SituationStatus,
                                ExpectPoint1 = e.ExpectPoint,
                                StartDt = startDate,
                                OddsInfoModel = oddsService.GetOddsInfoByGameID(e.SportsID, e.GameID, memberId),
                                MyPointInfoModel = ConvertToGamePointInfoModel(e)

                            }).ToList();

                if (nbpGames != null)
                {
                    foreach (var g in nbpGames)
                    {
                        // 試合の勝利チーム
                        var winteam = (from gi in npb.GameInfoRGI
                                       join ri in npb.GameResultInfoRGI
                                       on gi.RealGameInfoRootRGIId equals ri.RealGameInfoRootRGIId
                                       where gi.GameID == g.GameID   // 試合
                                       select ri).FirstOrDefault();
                        if (winteam != null)
                        {
                            g.WinTeamCd = winteam.WinTeamCD;
                        }
                    }
                }
            }

            #endregion

            #region MLBゲームの取得

            List<GameInfoModel> mlbGames = null;

            if (sportsId == 0 || sportsId == Constants.MLB_SPORT_ID)
            {
                var mlbExpectTargets = GetPointOfUser(memberId, Constants.MLB_SPORT_ID, startDate, endDate);

                mlbGames = (from e in mlbExpectTargets
                            select new GameInfoModel
                            {
                                TargetYear = target_year,
                                TargetMonth = target_month,
                                StartScheduleDate = e.StartScheduleDate,
                                SportsID = e.SportsID,
                                ExpectTargetID = e.ExpectTargetID,
                                GameID = e.GameID,
                                UrlArticle = "/mypage/article/",

                                WinLose = Convert.ToInt16(e.VorD), //勝敗1:勝ち 2:負け,
                                BetSelectID = e.BetSelectID,
                                SituationStatus = e.SituationStatus,
                                ExpectPoint1 = e.ExpectPoint,
                                StartDt = startDate,
                                OddsInfoModel = oddsService.GetOddsInfoByGameID(e.SportsID, e.GameID, memberId),
                                MyPointInfoModel = ConvertToGamePointInfoModel(e)

                            }).ToList();

                if (mlbGames != null)
                {
                    var mlbEntities = new MlbEntities();

                    foreach (var g in mlbGames)
                    {
                        
                        // チームID,試合日を取得
                        var ss = (mlbEntities.SeasonSchedule.FirstOrDefault(s => s.GameID == g.GameID));
                        if (ss != null)
                        {
                            if (ss.HomeTeamID != null) g.HomeTeamID = (int)ss.HomeTeamID;
                            if (ss.VisitorTeamID != null) g.VisitorTeamID = (int)ss.VisitorTeamID;

                            var gg = mlbEntities.DayGroup.FirstOrDefault(x => x.DayGroupId == ss.DayGroupId);
                            if (gg != null) if (gg.GameDateJPN != null) g.GameDateScheduled = (int) gg.GameDateJPN;
                        }

                        // 試合の勝利チームを得る
                        // MlbTeamInfoDailyResultController.csから引用
                        var query = from realGameInfo in mlb.RealGameInfo
                                    join seasonSchedule in mlb.SeasonSchedule on realGameInfo.GameID equals seasonSchedule.GameID
                                    join dayGroup in mlb.DayGroup on seasonSchedule.DayGroupId equals dayGroup.DayGroupId
                                    where (dayGroup.GameDateJPN.Value == g.GameDateScheduled) && (realGameInfo.GameID == g.GameID && realGameInfo.HomeTeamID == g.HomeTeamID)
                                    select new
                                    {
                                        WinTeamCD = realGameInfo.HomeScore > realGameInfo.VisitorScore ? realGameInfo.HomeTeamID : (realGameInfo.HomeScore < realGameInfo.VisitorScore ? realGameInfo.VisitorTeamID : null),
                                        WinTeamName = realGameInfo.HomeScore > realGameInfo.VisitorScore ? realGameInfo.HomeTeamName : (realGameInfo.HomeScore < realGameInfo.VisitorScore ? realGameInfo.VisitorTeamName : null)
                                    };

                        if (query == null) continue;

                        var winteam = query.FirstOrDefault();
                        if (winteam != null)
                        {
                            if (winteam.WinTeamCD == g.HomeTeamID)
                            {
                                g.WinnerTeam = 1;
                            }
                            else if (winteam.WinTeamCD == g.VisitorTeamID)
                            {
                                g.WinnerTeam = 2;
                            }
                        }
                    }
                }
            }

            #endregion

            #region JLGゲームの取得

            List<GameInfoModel> jlgGames = null;

            if (sportsId == 0 || sportsId == Constants.JLG_SPORT_ID)
            {
                var jlgExpectTargets = GetPointOfUser(memberId, Constants.JLG_SPORT_ID, startDate, endDate);

                jlgGames = (from e in jlgExpectTargets
                            select new GameInfoModel
                            {
                                TargetYear = target_year,
                                TargetMonth = target_month,
                                StartScheduleDate = e.StartScheduleDate,
                                SportsID = e.SportsID,
                                ExpectTargetID = e.ExpectTargetID,
                                GameID = e.GameID,
                                UrlArticle = "/mypage/article/",

                                WinLose = Convert.ToInt16(e.VorD), //勝敗1:勝ち 2:負け,
                                BetSelectID = e.BetSelectID,
                                SituationStatus = e.SituationStatus,
                                ExpectPoint1 = e.ExpectPoint,
                                StartDt = startDate,
                                OddsInfoModel = oddsService.GetOddsInfoByGameID(e.SportsID, e.GameID, memberId),
                                MyPointInfoModel = ConvertToGamePointInfoModel(e)

                            }).ToList();

                if (jlgGames != null)
                {
                    var jlgEntities = new JlgEntities();

                    foreach (var g in jlgGames)
                    {
                        // チームIDを取得
                        var si = jlgEntities.ScheduleInfo.FirstOrDefault(x => x.GameID == g.GameID);
                        if (si != null)
                        {
                            if (si.HomeTeamID != null) g.HomeTeamID = (int) si.HomeTeamID;
                            if (si.AwayTeamID != null) g.VisitorTeamID = (int) si.AwayTeamID;
                        }

                        //実際の試合日時
                        var lstScoreHome = JlgCommon.GetTeamInfoGTSByGameIDTeamID(g.GameID, g.HomeTeamID);
                        var lstScoreVisitor = JlgCommon.GetTeamInfoGTSByGameIDTeamID(g.GameID, g.VisitorTeamID);
                    
                        // 試合の勝利チームを得る
                        int winTeamCd = lstScoreHome.R > lstScoreVisitor.R ? g.HomeTeamID : lstScoreHome.R < lstScoreVisitor.R ? g.VisitorTeamID : 0;
                        if (winTeamCd == g.HomeTeamID) // Home勝ち点３
                        {
                            // ホーム勝ちののBetSelectID
                            g.WinnerTeam = (int)BetConst.BetSelectID.Home;
                        }
                        else if (winTeamCd == g.VisitorTeamID)
                        {
                            // アウェー勝ちののBetSelectID
                            g.WinnerTeam = (int)BetConst.BetSelectID.Visitor;
                        }
                        else
                        {
                            // 引き分けのBetSelectID
                            g.WinnerTeam = (int)BetConst.BetSelectID.Draw;
                        }
                    }
                }
            }

            #endregion


            var allGames = new List<GameInfoModel>();

            if (nbpGames != null)
                allGames.AddRange(nbpGames);
            if (mlbGames != null)
                allGames.AddRange(mlbGames);
            if (jlgGames != null)
                allGames.AddRange(jlgGames);

            return allGames;
        }


        #region Get Point Of User
        /// <summary>
        /// Get point of user when user login.   <-SCpied from NpbRightContentController.GetPointOfUser
        /// </summary>
        /// <returns>List Game predicted.</returns
        public static List<MyExpectedListInfoModel.MyPointInfoModel> GetPointOfUser(long memberID, int sportsId, DateTime startDate, DateTime endDate)
        {

            //月単位で特定
            List<long> lstPointID = com.Point.Where(m => m.MemberID == memberID && m.GiveTargetYear == (short)startDate.Year && m.GiveTargetMonth == (short)startDate.Month).Select(m => m.PointID).ToList();
            //日限定
            List<long> lstPointIDDaily = com.Point.Where(m => m.MemberID == memberID && DbFunctions.TruncateTime(m.BetStartDate) <= startDate.Date && DbFunctions.TruncateTime(m.BetEndDate) >= endDate.Date).Select(m => m.PointID).ToList();

            //Get these games that not end : not start or ongoing.
            var expectations = (from et in com.ExpectTarget
                                join ep in com.ExpectPoint on et.ExpectTargetID equals ep.ExpectTargetID
                                join p in com.Point on ep.PointID equals p.PointID
                                where (lstPointID.Contains(ep.PointID) || lstPointIDDaily.Contains(ep.PointID))
                                && ep.SituationStatus != 2                 //状況ステータス １・・予想、２・・キャンセル、３・・中止、４・・結果確定
                                && et.SportsID == sportsId
                                select new MyExpectedListInfoModel.MyPointInfoModel
                                {
                                    SportsID = sportsId,
                                    PointID = p.PointID,
                                    ExpectPoint = ep.ExpectPoint1,
                                    ExpectPointID = ep == null ? 0 : ep.ExpectPointID,
                                    BetStartDate = p.BetStartDate,
                                    GiveTargetMonth = p.GiveTargetMonth,
                                    BetSelectID = (ep == null ? 0 : (ep.BetSelectID.Value == null ? 0 : ep.BetSelectID.Value)),
                                    GameID = et.UniqueID,

                                    Odds = (ep == null || et == null) ? (decimal)0 : (from od in com.OddsInfo
                                                                                      join bsm in com.BetSelectMaster on od.BetSelectMasterID equals bsm.BetSelectMasterID
                                                                                      where ep.BetSelectID.ToString() == bsm.BetSelectMasterID.ToString() && od.ClassificationType == 2
                                                                                          && od.ExpectTargetID == et.ExpectTargetID
                                                                                      select od.Odds).FirstOrDefault(),
                                    StartScheduleDate = et == null ? DateTime.MinValue : (et.StartScheduleDate.Value == null ? DateTime.MinValue : et.StartScheduleDate.Value),


                                    AcquisitionPoint = ep == null ? 0 : ep.AcquisitionPoint,
                                    ClassClass = ep == null ? 0 : ep.ClassClass,
                                    ExpectTargetID = et == null ? 0 : et.ExpectTargetID,
                                    FixBetSelectID = et == null ? 0 : et.FixBetSelectID,
                                    SituationStatus = ep == null ? (short)0 : ep.SituationStatus,    //状況ステータス １・・予想、２・・キャンセル、３・・中止、４・・結果確定

                                    Status = et == null ? (short)0 : et.Status,
                                    VorD = ep == null ? (short)0 : ep.VorD
                                }).ToList();

            return expectations;
        }
        #endregion

        /// <summary>
        /// Model変換
        /// </summary>
        /// <param name="myPointInfoModel"></param>
        /// <returns></returns>
        public static GamePointInfoModel ConvertToGamePointInfoModel(MyExpectedListInfoModel.MyPointInfoModel myPointInfoModel)
        {
            if (myPointInfoModel == null) return null;

            return new GamePointInfoModel
            {
                ExpectPointID = myPointInfoModel.ExpectPointID,
                ExpectTargetID = myPointInfoModel.ExpectTargetID,
                SportsID = myPointInfoModel.SportsID,
                Status = myPointInfoModel.Status,
                FixBetSelectID = myPointInfoModel.FixBetSelectID,
                PointID = myPointInfoModel.PointID,
                ExpectPoint = myPointInfoModel.ExpectPoint,
                AcquisitionPoint = myPointInfoModel.AcquisitionPoint,
                ClassClass = myPointInfoModel.ClassClass,
                GameID = myPointInfoModel.GameID,
                BetSelectID = myPointInfoModel.BetSelectID,
                VorD = myPointInfoModel.VorD,
                SituationStatus = myPointInfoModel.SituationStatus,
                BetStartDate = myPointInfoModel.BetStartDate,
                StartScheduleDate = myPointInfoModel.StartScheduleDate,
                Odds = myPointInfoModel.Odds,
                GiveTargetMonth = myPointInfoModel.GiveTargetMonth
            };
        }

        #region ポイント入力時のBetポイント変更処理
        public static MyPageJsonResultModel UpdateExpectPoint(MyExpectedListInfoModel.ExpectChangeModel ViewModel, Int64 memberID)
        {
            int sportsID = ViewModel.SportsID;
            int gameID = ViewModel.GameID;
            int newExpectPoints = ViewModel.ExpectPoint;
            int oldExpectPoints = ViewModel.OldExpectPoint;
            MyPageJsonResultModel result = new MyPageJsonResultModel();

            string errMsg = "";
            errMsg = ValidChkInputPoint(ViewModel, memberID);

            if (errMsg != "")
            {
                result.HasError = true;
                result.Message = errMsg;

            }
            else
            {
                using (var dbContextTransaction = com.Database.BeginTransaction())
                {
                    try
                    {
                        if (gameID > 0 && memberID > 0)
                        {
                            var expectPointRecord = (from ep in com.ExpectPoint
                                                     join et in com.ExpectTarget
                                                     on ep.ExpectTargetID equals et.ExpectTargetID
                                                     join pt in com.Point
                                                     on ep.PointID equals pt.PointID
                                                     where et.UniqueID == gameID
                                                     && et.ClassClass == CLASSCLASS_GAME
                                                     && et.SportsID == sportsID
                                                     && pt.MemberID == memberID
                                                     select ep).FirstOrDefault();

                            if (expectPointRecord != null)
                            {
                                //TODO SituationStatus 要確認（!=2でなくていいの？）
                                expectPointRecord.SituationStatus = 1; // １・・予想、２・・キャンセル、３・・中止、４・・結果確定


                                var newPHexpect = new PointHistory
                                {
                                    PointId = expectPointRecord.PointID,
                                    ExpectPointId = expectPointRecord.ExpectPointID,
                                    CampaignId = null,
                                    PointClass = 2, //１・・原資、２・・所持、３・・予想、４・・獲得、５・・精算			
                                    Points = newExpectPoints,
                                    HistoryClass = 2,// 1・・付与、2・・予想、3・・獲得、4・・予想失敗、5・・精算、6・・キャンセル、7・・払い戻し、8・・失効			
                                    AdjustmentClass = 2, // １・・加算、２・・減算			
                                    OperationClass = 1, // １・・フロント、２・・バッチ、３・・バックオフィス			
                                    Status = true, // ０・・エラー　　１・・正常終了　			
                                    CreatedAccountID = memberID.ToString(),
                                    CreatedDate = DateTime.Now
                                };
                                com.PointHistory.Add(newPHexpect);
                                var newPHpossesion = new PointHistory
                                {
                                    PointId = expectPointRecord.PointID,
                                    ExpectPointId = expectPointRecord.ExpectPointID,
                                    CampaignId = null,
                                    PointClass = 3, //１・・原資、２・・所持、３・・予想、４・・獲得、５・・精算			
                                    Points = newExpectPoints,
                                    HistoryClass = 2,// 1・・付与、2・・予想、3・・獲得、4・・予想失敗、5・・精算、6・・キャンセル、7・・払い戻し、8・・失効			
                                    AdjustmentClass = 1, // １・・加算、２・・減算			
                                    OperationClass = 1, // １・・フロント、２・・バッチ、３・・バックオフィス			
                                    Status = true, // ０・・エラー　　１・・正常終了　			
                                    CreatedAccountID = memberID.ToString(),
                                    CreatedDate = DateTime.Now
                                };

                                com.PointHistory.Add(newPHpossesion);

                                var upd_pt = (from pt in com.Point
                                              where pt.PointID == expectPointRecord.PointID
                                              && pt.MemberID == memberID
                                              select pt).FirstOrDefault();
                                if (upd_pt != null)
                                {
                                    upd_pt.PossesionPoint = upd_pt.PossesionPoint - (newExpectPoints - oldExpectPoints);
                                }
                                expectPointRecord.ExpectPoint1 = newExpectPoints;

                                com.SaveChanges();
                                dbContextTransaction.Commit();
                            }

                        }
                        else
                        {
                            result.HasError = true;
                            //Rollback transaction.
                            dbContextTransaction.Rollback();
                        }
                    }
                    catch (Exception)
                    {
                        // TODO:とりあえずログ出力
                        // JS呼出時の集約例外処理の対応後に集約例外処理へthrow,ここでのRollbackは行わないように修正する
                        result.HasError = true;
                        dbContextTransaction.Rollback();
                    }
                }
            }

            return result;

        }
        #endregion

        #region 入力ポイントのバリデーションチェック
        public static string ValidChkInputPoint(MyExpectedListInfoModel.ExpectChangeModel ViewModel, Int64 memberID)
        {
            int sportsID = ViewModel.SportsID;
            int gameID = ViewModel.GameID;

            int oldExpectPoint = ViewModel.OldExpectPoint;
            int newExpectPoints = ViewModel.ExpectPoint;

            // ポイント情報取得
            var points = com.Point.Where(m => m.MemberID == memberID
            && m.GiveTargetYear == DateTime.Now.Year
            && m.GiveTargetMonth == DateTime.Now.Month).Select(m => m);

            string result = "";

            /// 1 ベット不可
            /// 2=5分前以前、ベットなし  3=5分前以前、ベットあり
            /// 3=5分前以降、ベットなし  4=5分前以前、ベットあり
            /// 6=6=試合中、ベットなし 7=試合中、ベットあり
            /// 8=試合終了、ベットなし 9=試合終了、ベットあり
            /// 10:試合中止
            /// 

            // Common.jsのロジックでは、NpbTopControllerのGetStatusByGameIDを使用しているので判定方法が若干違う
            int gameStatus = Utils.GetStatusByGameID(sportsID, gameID, (long)memberID);
            if (gameStatus >= 4)
            {
                result = "締切り時刻を過ぎたため変更できません。";
                return result;
            }

            if (newExpectPoints > 1000000)
            {
                result = "ひとつの予想にかけられる上限は100万ポイントまでです。";
                return result;
            }

            if (points == null)
            {
                result = "所持ポイント以上は入力できません。";
                return result;
            }

            var point = points.ToList().FirstOrDefault();

            if (point == null)
            {
                result = "所持ポイント以上は入力できません。";
                return result;
            }

            int actualTotalPoint = point.PossesionPoint + oldExpectPoint;

            if (newExpectPoints > actualTotalPoint)
            {
                result = "所持ポイント以上は入力できません。";
                return result;
            }

            if (newExpectPoints < 100)
            {
                result = "予想には100ポイント以上必要です。";
                return result;

            }
            // いるのでしょうか・・・
            if (newExpectPoints == point.PossesionPoint)
            {
                return result;
            }

            return result;
        }
        #endregion

        #region 予想キャンセル
        public static bool IsExpectCancel(MyExpectedListInfoModel.ExpectChangeModel ViewModel, Int64 memberID)
        {
            bool isResult = false;
            int sports_id = ViewModel.SportsID;
            int gameID = ViewModel.GameID;
            int gamePoints = ViewModel.ExpectPoint;

            using (var dbContextTransaction = com.Database.BeginTransaction())
            {
                try
                {
                    if (gameID > 0 && memberID > 0)
                    {
                        List<long> lstPointID = com.Point.Where(m => m.MemberID == memberID).Select(m => m.PointID).ToList();

                        //Get these games that not end : not start or ongoing.
                        var upd_ep = (from et in com.ExpectTarget
                                      join ep in com.ExpectPoint on et.ExpectTargetID equals ep.ExpectTargetID
                                      where lstPointID.Contains(ep.PointID)
                                    && et.UniqueID == gameID
                                 && et.SportsID == sports_id
                                 && et.ClassClass == MyPageCommon.CLASSCLASS_GAME
                                      select ep).FirstOrDefault();

                        if (upd_ep != null)
                        {
                            upd_ep.SituationStatus = 2; // １・・予想、２・・キャンセル、３・・中止、４・・結果確定


                            var newPHexpect = new PointHistory
                            {
                                PointId = upd_ep.PointID,
                                ExpectPointId = upd_ep.ExpectPointID,
                                CampaignId = null,
                                PointClass = 3, //１・・原資、２・・所持、３・・予想、４・・獲得、５・・精算			
                                Points = upd_ep.ExpectPoint1,
                                HistoryClass = 6,// 1・・付与、2・・予想、3・・獲得、4・・予想失敗、5・・精算、6・・キャンセル、7・・払い戻し、8・・失効			
                                AdjustmentClass = 2, // １・・加算、２・・減算			
                                OperationClass = 1, // １・・フロント、２・・バッチ、３・・バックオフィス			
                                Status = true, // ０・・エラー　　１・・正常終了　			
                                CreatedAccountID = memberID.ToString(),
                                CreatedDate = DateTime.Now
                            };
                            com.PointHistory.Add(newPHexpect);
                            var newPHpossesion = new PointHistory
                            {
                                PointId = upd_ep.PointID,
                                ExpectPointId = upd_ep.ExpectPointID,
                                CampaignId = null,
                                PointClass = 2, //１・・原資、２・・所持、３・・予想、４・・獲得、５・・精算			
                                Points = upd_ep.ExpectPoint1,
                                HistoryClass = 6,// 1・・付与、2・・予想、3・・獲得、4・・予想失敗、5・・精算、6・・キャンセル、7・・払い戻し、8・・失効			
                                AdjustmentClass = 1, // １・・加算、２・・減算			
                                OperationClass = 1, // １・・フロント、２・・バッチ、３・・バックオフィス			
                                Status = true, // ０・・エラー　　１・・正常終了　			
                                CreatedAccountID = memberID.ToString(),
                                CreatedDate = DateTime.Now
                            };
                            com.PointHistory.Add(newPHpossesion);

                            var upd_pt = (from pt in com.Point
                                          where pt.PointID == upd_ep.PointID
                                          && pt.MemberID == memberID
                                          select pt).FirstOrDefault();
                            if (upd_pt != null)
                            {
                                upd_pt.PossesionPoint = upd_pt.PossesionPoint + upd_ep.ExpectPoint1;
                            }

                            com.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                        isResult = true;
                    }
                    else
                    {
                        //Rollback transaction.
                        dbContextTransaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }

                return isResult;
            }
        }
        #endregion
    }
}
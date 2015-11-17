using System;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Mlb.Models;
using Splg.Core.Extend;
using Splg.Models;
using Splg.Models.Game.ViewModel;
using Splg.Services.Game;
using Splg.Services.Point;
using Splg.Services.System;

namespace Splg.Areas.Npb.Models
{
    public static class NpbCommon
    {
        #region Hai Nguyen

        /// <summary>
        /// Check status match npb team info schedule
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="type"></param>
        /// <returns>
        /// 0 = Match not yet started
        /// 1 = Match ongoing
        /// 2 = Match finish
        /// 3 = Match delay
        /// </returns>
        public static int GetStatusMatch(string gameId, int type = 0)
        {
            int result = 0;

            //ゲームIDがNullでない場合
            if (!string.IsNullOrEmpty(gameId))
            {
                var npb = new NpbEntities();
                var query = (from a in npb.RealGameInfoRootRGI
                             join b in npb.GameInfoRGI on a.RealGameInfoRootRGIId equals b.RealGameInfoRootRGIId
                             where b.GameID.ToString().Equals(gameId)
                             select (int?)a.Matchday).FirstOrDefault();
                DateTime matchDate;
                if (query.HasValue)
                {
                    matchDate = Utils.ConvertStrToDatetime(query.Value.ToString());
                    if (DateTime.Now < matchDate)
                    {
                        result = 0;
                    }
                    else if (DateTime.Now >= matchDate)
                    {

                        var gameInfoRGI = npb.GameInfoRGI.FirstOrDefault(p => p.GameID.ToString().Equals(gameId));
                        if (gameInfoRGI != null)
                        {
                            if (string.IsNullOrEmpty(gameInfoRGI.GameSetSituationCD))
                            {
                                if (gameInfoRGI.Inning.Value > 0)
                                {
                                    result = 1;
                                }
                                else
                                {
                                    result = 0;
                                }
                            }
                            else if (Convert.ToInt32(gameInfoRGI.GameSetSituationCD) == 2)
                            {
                                result = 2;
                            }
                            else
                            {
                                //check type =1 use in 8-5-2 team schedule
                                if (type != 0)
                                {
                                    result = 3;
                                }
                            }
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 試合状況を取得
        /// </summary>
        /// <param name="gameId">試合ID</param>
        /// <param name="strMemberId">会員ID</param>
        /// <returns>
        ///  0: 試合情報なし？
        ///  1: 受付前
        ///  2: 5分前以前、ベットなし
        ///  3: 5分前以前、ベットあり
        ///  4: 5分前以降、ベットなし
        ///  5: 5分前以降、ベットあり
        ///  6: 試合中、ベットなし
        ///  7: 試合中、ベットあり
        ///  8: 試合終了、ベットなし
        ///  9: 試合終了、ベットあり
        /// 10: 試合中止
        /// </returns>
        public static int GetStatusMatch(int gameId, string strMemberId)
        {
            //Todo:判定が煩雑なので、ロジック整理の必要有り

            var memberId = !string.IsNullOrEmpty(strMemberId) ? Convert.ToInt64(strMemberId) : 0;

            if (memberId > 0)
            {
                // ポイントテーブル存在判定
                var systemDatetimeService = new SystemDatetimeService();
                var pointInfoService = new PointInfoService(new ComEntities());
                var isExist = pointInfoService.IsExistsPoint(memberId, systemDatetimeService.SystemDate);
                if (!isExist) return 1;
            }
            
            // BET情報を取得
            var oddsService = new OddsService();
            var oddinfo = oddsService.GetOddsInfoByGameID(Constants.NPB_SPORT_ID, gameId, memberId);
            if (oddinfo.ExpectTargetID == 0) return 1;

            var isBet = oddinfo.BetSelectedID != null; // BET有無

            var npb = new NpbEntities();

            // 年間試合スケジュール_試合情報 を取得
            var gameInfoSS = npb.GameInfoSS.FirstOrDefault(x => x.ID == gameId);

            if (gameInfoSS == null) return 0;

            // 全試合情報を取得
            string gameSetSituationCd = null; // 試合終了状況コード
            var gameInfoRGI = npb.GameInfoRGI.FirstOrDefault(x => x.GameID == gameId);
            if (gameInfoRGI != null)
            {
                gameSetSituationCd = gameInfoRGI.GameSetSituationCD;
            }

            if (!string.IsNullOrEmpty(gameSetSituationCd) && (gameSetSituationCd == "0")) // 中止(試合前)
            {
                return 10;
            }
        
            var gameDate = Utils.ConvertStrToDatetime(gameInfoSS.GameDate.ToString());
            var hours = Convert.ToInt32(gameInfoSS.Time.Substring(0, 2));
            var minute = Convert.ToInt32(gameInfoSS.Time.Substring(2, 2));

            var gameStartDateAndTime = new DateTime(gameDate.Year, gameDate.Month, gameDate.Day, hours, minute, 0);
                
            //予想可能日付閾値(今日日付基準の月末日取得)
            var expectableEnd = DateTime.Now.EndOfTheMonthWithTime();

            // 試合開始時間前
            if (DateTime.Now < gameStartDateAndTime)
            {
                //Case 1:Can not bet
                if (gameStartDateAndTime > expectableEnd)
                {
                    return 1;
                }

                //予想可能限界日付時間（Todo:5分前の定義が、マジックナンバー）
                var limitTime = gameStartDateAndTime.AddMinutes(-5);

                if (DateTime.Now < limitTime)   //締切時間前
                {
                    return !isBet ? 2 : 3;
                }

                return !isBet ? 4 : 5;  // 締切時間以降
            }

            // 試合開始時間以降
            if (gameInfoRGI == null)
            {
                return !isBet ? 4 : 5;
            }

            if (!string.IsNullOrEmpty(gameSetSituationCd) && (gameSetSituationCd == "9")) // 中止(試合中)
            {
                return 10;
            }

            if (gameSetSituationCd == "2" ||
                gameSetSituationCd == "4" ||
                gameSetSituationCd == "5" ||
                gameSetSituationCd == "8" ||
                gameSetSituationCd == "A" ||
                gameSetSituationCd == "B" ||
                gameSetSituationCd == "C" ||
                gameSetSituationCd == "D")
            {
                return !isBet ? 8 : 9;
            }

            if (!string.IsNullOrEmpty(gameSetSituationCd))
            {
                return 10;
            }

            int inning = gameInfoRGI.Inning ?? default(short);   // 現イニング

            if (inning == 0)    // 試合前
            {
                return !isBet ? 4 : 5;
            }

            if (inning >= 1)    // 試合中
            {
                return !isBet ? 6 : 7;
            }

            return 0;
        }

        /// <summary>
        /// Get month of gamedate by current year
        /// </summary>
        /// <returns>list month</returns>
        public static int GetMonthOfGameDate(int month)
        {
            //Get first date of month
            var firstDateOfMonth = new DateTime(DateTime.Now.Year, month, 1);
            //Get first monday date of month
            var firstMondayOfMonth = firstDateOfMonth.AddDays((DayOfWeek.Monday < firstDateOfMonth.DayOfWeek ? 7 : 0) + DayOfWeek.Monday - firstDateOfMonth.DayOfWeek);
            //Get last monday of month
            var lastMondayOfMonth = Utils.GetLastMondayOfMonth(month, DateTime.Now.Year);
            //Get last sunday of month
            var lastSundayOfMonth = lastMondayOfMonth.AddDays(6);
            //format value 
            int startDate = int.Parse(firstMondayOfMonth.ToShortDateString().Replace("/", ""));
            int endDate = int.Parse(lastSundayOfMonth.ToShortDateString().Replace("/", ""));

            NpbEntities npb = new NpbEntities();
            var query = (from a in npb.GameInfoSS
                         join b in npb.TeamInfoMST on a.HomeTeamID equals b.TeamCD
                         join c in npb.TeamInfoMST on a.VisitorTeamID equals c.TeamCD
                         where (a.GameDate >= startDate && a.GameDate <= endDate) && (b.TeamCD != 397 || c.TeamCD != 397)
                         select a).Count();
            return query;
        }

        #endregion

        #region CucHTP

        #region Get TeamInfoGTS By GameID TeamID
        /// <summary>
        /// Get score in table GameTextScore By GameID and TeamID.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <param name="teamID">TeamID</param>
        /// <returns>Score if exists or 0 if not exist.</returns>
        public static ScoreGameInfo GetTeamInfoGTSByGameIDTeamID(int gameID, int teamID)
        {
            NpbEntities npb = new NpbEntities();
            var query = (from gi in npb.GameInfoGTS
                         join ti in npb.TeamInfoGTS on gi.GameInfoGTSId equals ti.GameInfoGTSId
                         where gi.GameID == gameID && ti.ID == teamID
                         select new ScoreGameInfo
                         {
                             GameID = gi.GameID,
                             TeamID = ti.ID,
                             Inning = gi.Inning == null ? 0 : gi.Inning,
                             TB = gi.TB == null ? 0 : gi.TB,
                             R = ti.R
                         }).FirstOrDefault();
            ScoreGameInfo scoreInfo = new ScoreGameInfo();
            if (query == null)
            {
                scoreInfo.GameID = gameID;
                scoreInfo.TeamID = teamID;
                scoreInfo.Inning = 0;
                scoreInfo.TB = 0;
                scoreInfo.R = 0;
                return scoreInfo;
            }
            else
            {
                return query;
            }
        }

        #endregion

        #region Get OddsInfo By GameID

        /// <summary>
        /// Get OddsInfo By GameID : if not login, only show odds, not define user prediction.
        /// </summary>
        /// <param name="gameId">GameID.</param>
        /// <param name="memberId"></param>
        /// <returns>Odds : hometeam, visitorteam, and draw</returns>
        public static GameOddsInfo GetOddsInfoByGameID(int gameId, long memberId = 0)
        {
            // todo 共通サービスへ抽出できるのでは？

            ComEntities com = new ComEntities();
            var query = default(GameOddsInfo);

            //Used for user login.
            if (memberId == 0)
            {
                object currentUser = HttpContext.Current.Session["CurrentUser"];

                if (currentUser != null)
                    memberId = Convert.ToInt64(currentUser.ToString());
            }

            if (memberId > 0)
            {
                query = (from et in com.ExpectTarget
                         join od in com.OddsInfo on et.ExpectTargetID equals od.ExpectTargetID into tmp1
                         from odh in tmp1.DefaultIfEmpty()
                         join bsm in com.BetSelectMaster on odh.BetSelectMasterID equals bsm.BetSelectMasterID into bsmtp1
                         from bsmh in bsmtp1.DefaultIfEmpty()
                         join od1 in com.OddsInfo on et.ExpectTargetID equals od1.ExpectTargetID into tmp2
                         from odv in tmp2.DefaultIfEmpty()
                         join bsm1 in com.BetSelectMaster on odv.BetSelectMasterID equals bsm1.BetSelectMasterID into bsmtp2
                         from bsmv in bsmtp2.DefaultIfEmpty()
                         join od3 in com.OddsInfo on et.ExpectTargetID equals od3.ExpectTargetID into tmp3
                         from odd in tmp3.DefaultIfEmpty()
                         join bsm2 in com.BetSelectMaster on odd.BetSelectMasterID equals bsm2.BetSelectMasterID into bsmtp3
                         from bsmd in bsmtp3.DefaultIfEmpty()
                         where (et.SportsID == Constants.NPB_SPORT_ID && et.ClassClass == 4 && et.UniqueID == gameId)
                         && (odh == null || bsmh == null || bsmh.BetSelectID == 1 && odh.ClassificationType == 2)
                         && (odv == null || bsmv == null || bsmv.BetSelectID == 2 && odv.ClassificationType == 2)
                         && (odd == null || bsmd == null || bsmd.BetSelectID == 3 && odd.ClassificationType == 2)
                         select new GameOddsInfo
                         {
                             ExpectTargetID = et.ExpectTargetID,
                             HomeTeamOdds = (odh.Odds == null ? 0 : odh.Odds),
                             VisitorTeamOdds = (odv == null ? 0 : odv.Odds),
                             DrawOdds = (odd == null ? 0 : odd.Odds),
                             BetSelectedID = (from ep in com.ExpectPoint
                                              join p in com.Point on ep.PointID equals p.PointID
                                              where ep.ExpectTargetID == et.ExpectTargetID && p.MemberID == memberId && ep.SituationStatus == 1 //TODO SituationStatus 要確認（!=2でなくていいの？）
                                              select ep.BetSelectID).FirstOrDefault(),
                         }).FirstOrDefault();
            }
            else
            {
                //User not login.
                query = (from et in com.ExpectTarget
                         join od in com.OddsInfo on et.ExpectTargetID equals od.ExpectTargetID into tmp1
                         from odh in tmp1.DefaultIfEmpty()
                         join bsm in com.BetSelectMaster on odh.BetSelectMasterID equals bsm.BetSelectMasterID into bsmtp1
                         from bsmh in bsmtp1.DefaultIfEmpty()
                         join od1 in com.OddsInfo on et.ExpectTargetID equals od1.ExpectTargetID into tmp2
                         from odv in tmp2.DefaultIfEmpty()
                         join bsm1 in com.BetSelectMaster on odv.BetSelectMasterID equals bsm1.BetSelectMasterID into bsmtp2
                         from bsmv in bsmtp2.DefaultIfEmpty()
                         join od3 in com.OddsInfo on et.ExpectTargetID equals od3.ExpectTargetID into tmp3
                         from odd in tmp3.DefaultIfEmpty()
                         join bsm2 in com.BetSelectMaster on odd.BetSelectMasterID equals bsm2.BetSelectMasterID into bsmtp3
                         from bsmd in bsmtp3.DefaultIfEmpty()
                         where (et.SportsID == Constants.NPB_SPORT_ID && et.ClassClass == 4 && et.UniqueID == gameId)
                         && (odh == null || bsmh == null || bsmh.BetSelectID == 1 && odh.ClassificationType == 2)
                         && (odv == null || bsmv == null || bsmv.BetSelectID == 2 && odv.ClassificationType == 2)
                         && (odd == null || bsmd == null || bsmd.BetSelectID == 3 && odd.ClassificationType == 2)
                         select new GameOddsInfo
                         {
                             ExpectTargetID = et.ExpectTargetID,
                             HomeTeamOdds = (odh.Odds == null ? 0 : odh.Odds),
                             VisitorTeamOdds = (odv == null ? 0 : odv.Odds),
                             DrawOdds = (odd == null ? 0 : odd.Odds),
                             BetSelectedID = 0,
                         }).FirstOrDefault();
            }

            if (query == null)
            {
                //Create new class to store 3 odds with value 0.
                var odds = new GameOddsInfo
                {
                    ExpectTargetID = 0,
                    HomeTeamOdds = 0,
                    VisitorTeamOdds = 0,
                    DrawOdds = 0,
                    BetSelectedID = 0
                };
                query = odds;
            }

            return query;
        }
        #endregion

        #region Get GameInfo By GameID
        /// <summary>
        /// Get gameinfo : name, date, time of game.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>GameInfo need get.</returns>
        public static GameInfoViewModel GetGameInfoByGameID(int sportID, int gameID)
        {
            var query = default(GameInfoViewModel);
            //Pro Baseball.
            if (sportID == 1)
            {
                NpbEntities npb = new NpbEntities();
                query = (from gi in npb.GameInfoSS
                         where gi.ID == gameID
                         select new GameInfoViewModel
                         {
                             GameID = gameID,
                             HomeTeamID = gi.HomeTeamID.Value,
                             HomeTeamName = gi.HomeTeamName,
                             HomeTeamNameS = gi.HomeTeamNameS,
                             VisitorTeamID = gi.VisitorTeamID.Value,
                             VisitorTeamName = gi.VisitorTeamName,
                             VisitorTeamNameS = gi.VisitorTeamNameS,
                             GameDate = gi.GameDate,
                             Time = gi.Time
                         }).FirstOrDefault();
            }
            //Jleague
            else if (sportID == 2)
            {
                JlgEntities Jlg = new JlgEntities();
                query = (from si in Jlg.ScheduleInfo
                         join gcat in Jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                         join gs in Jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                         where si.GameID == gameID
                         select new GameInfoViewModel
                         {
                             GameKindID = gs.GameKindID,
                             GameID = gameID,
                             HomeTeamID = si.HomeTeamID.Value,
                             HomeTeamName = si.HomeTeamName,
                             HomeTeamNameS = si.HomeTeamNameS,
                             VisitorTeamID = si.AwayTeamID.Value,
                             VisitorTeamName = si.AwayTeamName,
                             VisitorTeamNameS = si.AwayTeamNameS,
                             GameDate = si.GameDate.Value,
                             Time = si.GameTime.ToString(),
                         }).FirstOrDefault();
            }
            //MLB.
            else if (sportID == 3)
            {
                MlbEntities mlb = new MlbEntities();
                query = (from ss in mlb.SeasonSchedule
                         join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                         where ss.GameID == gameID
                         select new GameInfoViewModel
                         {
                             GameID = gameID,
                             HomeTeamID = ss.HomeTeamID.Value,
                             HomeTeamName = ss.HomeTeamName,
                             HomeTeamNameS = ss.HomeTeamName,
                             VisitorTeamID = ss.VisitorTeamID.Value,
                             VisitorTeamName = ss.VisitorTeamName,
                             VisitorTeamNameS = ss.VisitorTeamName,
                             GameDate = dg.GameDateJPN == null ? 0 : dg.GameDateJPN.Value,
                             Time = ss.Time
                         }).FirstOrDefault();
            }
            return query;
        }
        #endregion

        #region Get PossionPoint By PointID
        /// <summary>
        /// Get PossionPoint By PointID.
        /// </summary>
        public static int GetPossessionPointByPointID(long pointID)
        {
            // todo 共通サービスへ抽出できるのでは？

            var comEntities = new ComEntities();

            return comEntities.Point.Where(m => m.PointID == pointID).Select(m => m.PossesionPoint).FirstOrDefault();
            
        }
        #endregion

        #region Get GameResult By GameID
        /// <summary>
        /// Get game result in right content by gameID
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>Result of game : score.</returns>
        public static GameInfoViewModel GetGameResultInRightContent(int gameID)
        {
            NpbEntities npb = new NpbEntities();
            var query = (from rgi in npb.RealGameInfoRootRGI
                         join gi in npb.GameInfoRGI on rgi.RealGameInfoRootRGIId equals gi.RealGameInfoRootRGIId
                         join tm in npb.TeamInfoMST on gi.HomeTeam equals tm.TeamCD
                         join tm1 in npb.TeamInfoMST on gi.VisitorTeam equals tm1.TeamCD
                         join si1 in npb.ScoreRGI on rgi.RealGameInfoRootRGIId equals si1.RealGameInfoRootRGIId into tmp1
                         from si11 in tmp1.DefaultIfEmpty()
                         join si2 in npb.ScoreRGI on rgi.RealGameInfoRootRGIId equals si2.RealGameInfoRootRGIId into tmp2
                         from si22 in tmp2.DefaultIfEmpty()
                         where gi.GameID == gameID && (si11 == null || si11.HomeVisitor == Constants.HOMETEAM) && (si22 == null || si22.HomeVisitor == Constants.VISITORTEAM)
                         select new GameInfoViewModel
                         {
                             GameID = gi.GameID,
                             HomeTeamID = gi.HomeTeam,
                             VisitorTeamID = gi.VisitorTeam.Value,
                             HomeTeamName = tm.ShortNameTeam,
                             VisitorTeamName = tm1.ShortNameTeam,
                             Round = gi.Round.Value,
                             GameDate = rgi.Matchday.Value,
                             HomeTeamScore = (si11.TotalScore.Value == null ? 0 : si11.TotalScore.Value),
                             VisitorTeamScore = (si22.TotalScore.Value == null ? 0 : si22.TotalScore.Value),
                             GameSituationID = gi.GameSetSituationCD,
                             Inning = gi.Inning.Value,
                             BottomTop = gi.BottomTop
                         }).FirstOrDefault();
            return query;
        }
        #endregion

        #region Get Status For Point By GameID
        /// <summary>
        /// Get status used for point by gameID.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>Game Status.</returns>
        public static int GetStatusForPointByGameID(int gameID)
        {
            int result = 0;

            if (gameID == null) return result;

            NpbEntities npb = new NpbEntities();
            var query = (from a in npb.RealGameInfoRootRGI
                join b in npb.GameInfoRGI on a.RealGameInfoRootRGIId equals b.RealGameInfoRootRGIId
                where b.GameID == gameID
                select (int?)a.Matchday).FirstOrDefault();
            DateTime matchDate;

            if (query.HasValue)
            {
                matchDate = Utils.ConvertStrToDatetime(query.Value.ToString());
                if (DateTime.Now < matchDate)
                {
                    result = 0;
                }
                else if (DateTime.Now >= matchDate)
                {
                    var queryTime = npb.GameInfoSS.Where(x => x.ID == gameID).Select(x => new { x.GameDate, x.Time }).FirstOrDefault();
                    var gameInfoRGI = (from a in npb.GameInfoRGI
                        where a.GameID == gameID
                        select a).FirstOrDefault();

                    if (gameInfoRGI != null)
                    {
                        if (string.IsNullOrEmpty(gameInfoRGI.GameSetSituationCD))
                        {
                            if (gameInfoRGI.Inning.Value > 0)
                            {
                                result = 1;
                            }
                            else
                            {
                                //Test 5 minutes before game start.
                                if (queryTime != null)
                                {
                                    var remainType = Utils.CalculateTimeRemain(queryTime.GameDate, queryTime.Time);
                                    result = (remainType == 2 || remainType == 3) ? 1 : 0;
                                }
                                else
                                {
                                    result = 0;
                                }
                            }
                        }
                        else if (Convert.ToInt32(gameInfoRGI.GameSetSituationCD) == 2)
                        {
                            result = 2;
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            else
            {
                var queryTime = npb.GameInfoSS.Where(x => x.ID == gameID).Select(x => new { x.GameDate, x.Time }).FirstOrDefault();
                if (queryTime != null)
                {
                    var gDate = Utils.ConvertStrToDatetime(queryTime.GameDate.ToString());
                    if (gDate.Date == DateTime.Now.Date)
                    {
                        var remainType = Utils.CalculateTimeRemain(queryTime.GameDate, queryTime.Time);
                        result = (remainType == 2 || remainType == 3) ? 1 : 0;
                    }
                    else if (gDate.Date < DateTime.Now.Date)
                    {
                        result = 2;
                    }
                }

            }

            return result;
        }
        #endregion

        #region Get TeamID For GameText By GameID
        /// <summary>
        /// Get TeamID For GameText By GameID
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <param name="BottomTop">BottomTop : 1:表 = 先攻 = ビジター
        ///                                     2:裏 = 後攻 = ホーム</param>
        /// <returns>TeamName</returns>
        public static string GetTeamIDForGameText(int gameID, int BottomTop)
        {
            if (gameID != null)
            {
                NpbEntities npb = new NpbEntities();
                var query = (from gi in npb.GameInfoGTE
                             join ti in npb.TeamInfoGTE on gi.GameInfoGTEId equals ti.GameInfoGTEId
                             where gi.GameID == gameID && ti.HV != BottomTop
                             select ti.NameS).FirstOrDefault();
                return query;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #endregion
    }
}
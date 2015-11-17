using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Mlb.Models;
using Splg.Areas.Mlb.Models.ViewModels.InfosModel;
using Splg.Core.Extend;
using Splg.Models;
using Splg.Models.Game.ViewModel;
using Splg.Services.Game;
using Splg.Services.Point;
using Splg.Services.System;

namespace Splg.Areas.Mlb
{
    public static class MlbCommon
    {
        /// <summary>
        /// NPB 準拠のGameSetSituationCodeを返却する。
        /// 試合中か試合後かの判断が必要な時はGetStatusMatchメソッドで確認すること。
        /// </summary>
        /// <param name="mlbGameState">MLBのGameState</param>
        /// <returns>空白:試合中または試合前 2=正常終了　7=サスペンデッド 9=中止（試合中）</returns>
        public static string GetNpbGameSetSituationCode(short? mlbGameState)
        {
            string result = "";

            //"NPB GameSetSituationCD
            //2=正常終了、4=降雨コールド、5=日没コールド
            //7=サスペンデッド、8=没収試合、9=中止（試合中）、0=中止（試合前）
            //A=降雪コールド、B=濃霧コールド、C=強風コールド、D=点差コールド
            //空白=試合中または試合前(試合中か試合前かの判定は現イニングが0が1以上か)
            switch (mlbGameState)
            {
                //MLB GameState 4=試合終了、10=中止、11=サスペンデッド			
                case null: //試合中または試合前
                    result = "";
                    break;
                case 0: //試合中または試合前
                    result = "";
                    break;
                case 4: //試合終了
                    result = "2";
                    break;
                case 10: //中止
                    result = "9";
                    break;
                case 11: //サスペンデッド
                    result = "7";
                    break;
            }

            return result.ToString();

        }

        /// <summary>
        /// NPB 準拠のGameStateIDを返却する。
        /// 試合中か試合後かの判断が必要な時はGetStatusMatchメソッドで確認すること。
        /// </summary>
        /// <param name="mlbGameState">MLBのGameState</param>
        /// <returns>空白:試合中または試合前
        /// 0=試合前　4=試合終了 8=試合中中止</returns>
        public static int GetNpbGameStateID(short? mlbGameState)
        {
            int result = 0;

            //"NPB GameStateID
            //0=試合前、1=試合中、2=試合開始遅延、3=試合中断、4=試合終了 8=試合中中止、9=試合前中止
            switch (mlbGameState)
            {
                //MLB GameState 4=試合終了、10=中止、11=サスペンデッド			
                case null: //試合前
                    result = 0;
                    break;
                case 0: //試合前
                    result = 0;
                    break;
                case 4: //試合終了
                    result = 4;
                    break;
                case 10: //中止 試合前か後かの判断はできない。
                    result = 8;
                    break;
                case 11: //サスペンデッド
                    result = 8;
                    break;
            }

            return result;

        }

        #region GetStatusMatch
        /// <summary>
        ///  試合後かどうかを判断する
        ///  "0=試合前、1=試合中、2=試合後
        /// </summary>
        /// <param name="dateCheck"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static int GetStatusMatch(string gameId)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(gameId))
            {
                MlbEntities mlb = new MlbEntities();
                var realGameInfos = (from rgi in mlb.RealGameInfo //全試合速報
                                     join rgih in mlb.RealGameInfoHeader
                                     on rgi.RealGameInfoHeaderId equals rgih.RealGameInfoHeaderId
                                     join ss in mlb.SeasonSchedule on rgi.GameID equals ss.GameID
                                     join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                                     where rgi.GameID.ToString().Equals(gameId)
                                     select new
                                     {
                                         GameDate = rgih.GameDateJPN,
                                         date = rgih.GameDate,
                                         GameTime = ss.Time
                                     }).FirstOrDefault(); //Order by ha?


                if (realGameInfos != null)
                {
                    string dateStr = realGameInfos.GameDate.ToString();
                    string timeStr = (realGameInfos.GameTime == null ? "0000" : realGameInfos.GameTime.ToString()).PadLeft(4, '0');
                    DateTime matchDate = DateTime.ParseExact(dateStr + timeStr, "yyyyMMddHHmm", null);

                    if (DateTime.Now < matchDate)
                    {
                        result = 0;         //―＞試合前
                    }
                    else if (DateTime.Now >= matchDate)
                    {
                        //開始時刻後

                        //試合速報を確認する
                        var gameInfoRGI = (from rgi in mlb.RealGameInfo
                                           where rgi.GameID.ToString() == gameId
                                           select rgi).FirstOrDefault();

                        //MLB GameState 4=試合終了、10=中止、11=サスペンデッド			
                        int gameStatus = Convert.ToInt32(gameInfoRGI.GameStateID);

                        if (gameInfoRGI != null)
                        {
                            //試合速報が存在する
                            if (gameStatus == null || gameStatus == 0)   //試合中                 
                            {
                             //   if (gameInfoRGI.Inning.Value >= 0) 
                             //   {
                                result = 1;   //―＞試合中                       
                             //   }
                            }
                            else if (gameStatus == 4) //正常終了
                            {
                                result = 2;   //―＞試合後                     
                            }
                            else if (gameStatus == 10)  //試合中中止
                            {
                                result = 2;  //―＞試合後                 
                            }
                            else if (gameStatus == 11)  //試合開始遅延
                            {
                                result = 0;  //―＞試合前
                            }
                        }
                        else
                        {
                            //試合速報が存在しない
                            result = 0;      //―＞試合前
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
            var oddinfo = oddsService.GetOddsInfoByGameID(Constants.MLB_SPORT_ID, gameId, memberId);
            if (oddinfo.ExpectTargetID == 0) return 1;

            var checkBet = oddinfo.BetSelectedID != null;  // BET有無

            var mlb = new MlbEntities();
            var gameInfos = (from ss in mlb.SeasonSchedule //年間試合スケジュール_試合情報
                             join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                             where ss.GameID == gameId
                             select new MlbGameInfos
                             {
                                 GameDateJPN = dg.GameDateJPN, 
                                 Time = ss.Time
                             }).First();

            if (gameInfos == null) return 0;

            var gameDate = Utils.ConvertStrToDatetime(gameInfos.GameDateJPN.ToString());
            var gameStartDateAndTime = DateTime.MaxValue; // 未定
            int hours = 0;
            int minute = 0;

            string time = gameInfos.Time.Trim();
            //When   time.ToString().Length <2  then  error
            //When time is 0930-> Int32.TryParse  to 900 then error because hours:90 and minute:0
            if (time.Length > 1)
                hours = Convert.ToInt32(time.Substring(0, 2));                 
            //When   time.ToString().Length <4  then  error
            if (time.Length > 2)
                minute = Convert.ToInt32(time.Substring(2, time.Length - 2));            
            //End edit
            gameStartDateAndTime = new DateTime(gameDate.Year, gameDate.Month, gameDate.Day, hours, minute, 0);

            //予想可能日付閾値(今日日付基準の月末日取得)
            var expectableEnd = DateTime.Now.EndOfTheMonthWithTime();

            if (DateTime.MaxValue.Equals(gameStartDateAndTime) || DateTime.Now < gameStartDateAndTime)
            {
                //Case 1:Can not bet
                if (gameStartDateAndTime > expectableEnd)
                {
                    return  1;  // 1:ベット不可
                }

                var limitTime = gameStartDateAndTime.AddMinutes(-5);
                  
                if (DateTime.Now < limitTime)
                {
                    return !checkBet ? 2 : 3;   // 2:5分前以前、ベットなし  3:5分前以前、ベットあり
                }

                return !checkBet ? 4 : 5;   // 3:5分前以降、ベットなし  4:5分前以前、ベットあり
            }

            var realGameInfos = from rgi in mlb.RealGameInfo where rgi.GameID == gameId select rgi;

            if (realGameInfos != null)
            {
                if (realGameInfos.FirstOrDefault() != null)
                {
                    //空白:試合中または試合前 2=正常終了　7=サスペンデッド 9=中止（試合中）
                    string gameSetSituationCd = GetNpbGameSetSituationCode((short)realGameInfos.FirstOrDefault().GameStateID);

                    //"0=試合前、1=試合中、2=試合後
                    int gameStatus = GetStatusMatch(gameId.ToString());

                    //int inning = realGameInfos.FirstOrDefault().Inning.HasValue ? realGameInfos.FirstOrDefault().Inning.Value : default(short);
                    if ( (!string.IsNullOrEmpty(gameSetSituationCd)) && ((gameSetSituationCd == "8") ||(gameSetSituationCd == "9")))
                    {
                        return 10;
                    }
                        
                    if (gameSetSituationCd == "2")
                    {
                        return !checkBet ? 8 : 9;   // 8:試合終了、ベットなし 9:試合終了、ベットあり
                    }

                    if (string.IsNullOrEmpty(gameSetSituationCd) && gameStatus > 0   /*inning >= 1*/)
                    {
                        return !checkBet ? 6 : 7;   // 6:試合中、ベットなし 7:試合中、ベットあり
                    }

                    return 10;                      // 10:試合中止
                }

                return !checkBet ? 6 : 7;           // 6:試合中、ベットなし 7:試合中、ベットあり
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

            MlbEntities mlb = new MlbEntities();
            var query = (from ss in mlb.SeasonSchedule
                         join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                         join ht in mlb.TeamInfo on ss.HomeTeamID equals ht.TeamID
                         join vt in mlb.TeamInfo on ss.VisitorTeamID equals vt.TeamID
                         where (dg.GameDateJPN >= startDate && dg.GameDateJPN <= endDate)
                         select ss).Count();
            return query;
        }

        #endregion

        #region CucHTP

        #region Get TeamInfoGTS By GameID TeamIDHome
        /// <summary>
        /// Get score in table GameTextScore By GameID and TeamID.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <param name="teamID">TeamID</param>
        /// <returns>Score if exists or 0 if not exist.</returns>
        public static ScoreGameInfo GetTeamInfoGTSByGameIDTeamIDHome(int gameID, int teamID)
        {
            MlbEntities mlb = new MlbEntities();
            var query = (from rgi in mlb.RealGameInfo
                         where rgi.GameID == gameID && rgi.HomeTeamID == teamID
                         select new ScoreGameInfo
                         {
                             GameID = rgi.GameID,
                             TeamID = teamID,
                             Inning = null,
                             TB = null,
                             R = rgi.HomeScore
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

        #region Get TeamInfoGTS By GameID TeamIDVisitor
        /// <summary>
        /// Get score in table GameTextScore By GameID and TeamID.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <param name="teamID">TeamID</param>
        /// <returns>Score if exists or 0 if not exist.</returns>
        public static ScoreGameInfo GetTeamInfoGTSByGameIDTeamIDVisitor(int gameID, int teamID)
        {
            MlbEntities mlb = new MlbEntities();
            var query = (from rgi in mlb.RealGameInfo
                         where rgi.GameID == gameID && rgi.VisitorTeamID == teamID
                         select new ScoreGameInfo
                         {
                             GameID = rgi.GameID,
                             TeamID = teamID,
                             Inning = null,
                             TB = null,
                             R = rgi.VisitorScore
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
        /// <param name="gameID">GameID.</param>
        /// <returns>Odds : hometeam, visitorteam, and draw</returns>
        public static GameOddsInfo GetOddsInfoByGameID(int gameID, long memberID = 0)
        {
            ComEntities com = new ComEntities();
            var query = default(GameOddsInfo);

            //Used for user login.
            if (memberID == 0)
            {
                object currentUser = HttpContext.Current.Session["CurrentUser"];

                if (currentUser != null)
                    memberID = Convert.ToInt64(currentUser.ToString());
            }

            if (memberID > 0)
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
                         where (et.SportsID == Constants.MLB_SPORT_ID && et.ClassClass == 4 && et.UniqueID == gameID)
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
                                              where ep.ExpectTargetID == et.ExpectTargetID && p.MemberID == memberID && ep.SituationStatus == 1 //TODO SituationStatus 要確認（!=2でなくていいの？）
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
                         where (et.SportsID == Constants.MLB_SPORT_ID && et.ClassClass == 4 && et.UniqueID == gameID)
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
                GameOddsInfo odds = new GameOddsInfo();
                odds.ExpectTargetID = 0;
                odds.HomeTeamOdds = 0;
                odds.VisitorTeamOdds = 0;
                odds.DrawOdds = 0;
                odds.BetSelectedID = 0;
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
            if (sportID == Constants.MLB_SPORT_ID) ////////////////////////////////////////
            {
                MlbEntities mlb = new MlbEntities();
                
             query = (from ss in mlb.SeasonSchedule
                              join dg in mlb.DayGroup on ss.DayGroupId equals dg.DayGroupId
                              where ss.GameID == gameID
                         select new GameInfoViewModel
                         {
                             GameID = gameID,
                             HomeTeamName = ss.HomeTeamName,
                             VisitorTeamName = ss.VisitorTeamName,
                             GameDate = dg.GameDateJPN == null?0:dg.GameDateJPN.Value,
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
        /// <param name="pointID">PointID</param>
        /// <returns>PossessionPoint.</returns>
        public static int GetPossionPointByPointID(long pointID)
        {
            ComEntities com = new ComEntities();
            int possessionPoint = 0;
            possessionPoint = com.Point.Where(m => m.PointID == pointID).Select(m => m.PossesionPoint).FirstOrDefault();
            return possessionPoint;
        }
        #endregion

        #endregion

        public static List<Models.JapanesePlayers> GetExistingJapanesePlayers(IEnumerable<JapanesePlayers> JapanesePlayers, string year)
        {
            List<JapanesePlayers>  existingJapanesePlayers = new List<JapanesePlayers>();

            foreach (JapanesePlayers p in JapanesePlayers)
            {
                string seleratedYears = "";
                seleratedYears = parseYears(p.Years, seleratedYears);

                if (seleratedYears.Contains(year))
                    existingJapanesePlayers.Add(p);
            }

            return existingJapanesePlayers;
        }


        public static List<Models.JapanesePlayers> GetExistedJapanesePlayers(IEnumerable<JapanesePlayers> JapanesePlayers, string year)
        {
            List<JapanesePlayers>  existedJapanesePlayers = new List<JapanesePlayers>();

            foreach (JapanesePlayers p in JapanesePlayers)
            {
                string seleratedYears = "";
                seleratedYears = parseYears(p.Years, seleratedYears);

                if (!seleratedYears.Contains(year))
                    existedJapanesePlayers.Add(p);
            }

            return existedJapanesePlayers;
        }


        /// <summary>
        /// Mlb.JapanesePlayers「チーム情報_在籍した日本人」のYears「所属年」から、所属の各年を取り出す。
        /// 「所属年」の仕様は「連続する年は「-」、離れた年は「,」区切り」。
        /// データ上は、年が二桁の場合と四桁の場合がある
        /// </summary>
        /// <param name="yearsStr"></param>
        /// <param name="seleratedYears"></param>
        /// <returns>コンマセパレートされた年（４桁）</returns>
        private static string parseYears(string yearsStr, string seleratedYears)
        {
            string[] yearsByCommma = yearsStr.Split(',');

            foreach (string elm1 in yearsByCommma)
            {
                string[] yearsByHyphen = elm1.Split('-');
                if (yearsByHyphen.Length > 1)
                {
                    int startYear = 0;
                    int endYear = 0;
                    string startYearStr = yearsByHyphen[0];
                    string endYearStr = yearsByHyphen[1];

                    if (startYearStr.Trim().Length == 0)
                        startYear = 0;
                    else
                        startYear = int.Parse(startYearStr.Trim());

                    if (startYear > 0 && startYear < 1000)
                        startYear = startYear + 2000;

                    if (endYearStr.Trim().Length == 0)
                        endYear = 0;
                    else
                        endYear = int.Parse(endYearStr.Trim());

                    if (endYear > 0 && endYear < 1000)
                        endYear = endYear + 2000;

                    if (endYear == 0) endYear = System.DateTime.Now.Year; //2011- のケース
                    if (startYear == 0) startYear = endYear;              //-2011のケース

                    for (int i = startYear; i <= endYear; i++)
                    {
                        seleratedYears += i.ToString() + ",";
                    }

                }
                else
                    seleratedYears += yearsByHyphen[0] + ",";

            }
            return seleratedYears;
        }
    }
}
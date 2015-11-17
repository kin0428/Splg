using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Core.Extend;
using Splg.Models;
using Splg.Models.Game.ViewModel;
using Splg.Services.Game;
using Splg.Services.Point;
using Splg.Services.System;

namespace Splg.Areas.Jleague
{
    public  class JlgCommon
    {
        // Sy Huynh
        public static  int GetJlgType(string url)
        {
            if (url.ToLower().Contains("/j1"))
                return 1;
            if (url.ToLower().Contains("/j2"))
                return 2;
            if (url.ToLower().Contains("/jleaguecup"))
                return 3;
            return 0;
        }

        #region Get PageNo from url
        // Return the PageNo: 10-1, 10-2,... based on url

        public static string GetJlgPageNo(List<string> subUrlList)
        {
            int subUrlCount = subUrlList.Count();
            if (subUrlCount <= 1)
                return "10-1";
            List<string> pageNoList = new List<string>();
            pageNoList.Add("10");
            switch(subUrlList.ElementAt(1).ToLower())
            {
                case "j1":
                    pageNoList.Add("2");
                    break;
                case "j2":
                    pageNoList.Add("3");
                    break;
                case "jleaguecup":
                    pageNoList.Add("4");
                    break;
                case "game":
                    pageNoList.Add("5");
                    break;
                case "news":
                    pageNoList.Add("6");
                    break;
                case "players":
                    pageNoList.Add("2-5-6");
                    // pending...
                    break;
            }
            if(subUrlCount <= 2)
            {
                return string.Join("-", pageNoList.ToArray());
            }
            switch (subUrlList.ElementAt(2).ToLower())
            {
                case "schedule":
                    pageNoList.Add("1");
                    break;
                case "standings":
                    pageNoList.Add("2");
                    break;
                case "matrix":
                    pageNoList.Add("3");
                    break;
                case "stats":
                    pageNoList.Add("4");
                    break;
                case "teams":
                    pageNoList.Add("5");
                    if (subUrlCount >= 4)
                    {
                        if(subUrlCount == 4)
                        {
                            pageNoList.Add("1");
                        }
                        else 
                        {
                            switch (subUrlList.ElementAt(4).ToLower())
                            {
                                case "schedule":
                                    pageNoList.Add("2");
                                    break;
                                case "stats":
                                    pageNoList.Add("3");
                                    break;
                                case "memberlist":
                                    pageNoList.Add("4");
                                    break;
                                case "news":
                                    pageNoList.Add("5");
                                    break;
                            }
                        }
                    }
                    break;
                case "news":
                    if (pageNoList.Last().Equals("4"))
                        pageNoList.Add("4");
                    else
                        pageNoList.Add("6");
                    break;
                case "players":
                    pageNoList.Add("5-6");
                    // pending...
                    break;
                default:
                    return string.Join("-", pageNoList.ToArray());
            }

            return string.Join("-", pageNoList.ToArray());
        }
        #endregion
        /// <summary>
        /// NPB 準拠のGameSetSituationCodeを返却する。
        /// 試合中か試合後かの判断が必要な時はGetStatusMatchメソッドで確認すること。
        /// </summary>
        /// <param name="JlgGameState">JlgのGameState</param>
        /// <returns>空白:試合中または試合前 2=正常終了　7=サスペンデッド 9=中止（試合中）</returns>
        public static string GetNpbGameSetSituationCode(short? JlgGameState)
        {
            string result = "";

            //"NPB GameSetSituationCD
            //2=正常終了、4=降雨コールド、5=日没コールド
            //7=サスペンデッド、8=没収試合、9=中止（試合中）、0=中止（試合前）
            //A=降雪コールド、B=濃霧コールド、C=強風コールド、D=点差コールド
            //空白=試合中または試合前(試合中か試合前かの判定は現イニングが0が1以上か)
            switch (JlgGameState)
            {
                //Jlg GameState 4=試合終了、10=中止、11=サスペンデッド			
                case null: //試合中または試合前
                    result = "";
                    break;
                case 0: //試合中または試合前
                    result = "";
                    break;
                case 3: //試合終了
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
        /// <param name="JlgGameState">JlgのGameState</param>
        /// <returns>空白:試合中または試合前
        /// 0=試合前　4=試合終了 8=試合中中止</returns>
        public static string GetNpbGameStateID(short? JlgGameState)
        {
            string result = "";

            //"NPB GameStateID
            //0=試合前、1=試合中、3=試合終了 8=試合中中止、9=試合前中止
            switch (JlgGameState)
            {
                //Jlg GameState 3=試合終了			
                case 1: //試合前
                    result = "0";
                    break;
                case 2: //試合中
                case 5: // サスペンテッド
                    result = "1";
                    break;
                case 3: //試合終了
                case 4: //没収試合
                case 8: //没収試合
                case 9: //没収試合
                    result = "4";

                    break;
                case 6: //試合中止（試合中）
                case 7: //試合中止（試合前）
                    result = "9";
                    break;
                default: // デフォルトで処理(試合前状態にする)
                    result = "";
                    break;
            }

            return result;

        }

        #region GetStatusMatch
        /// <summary>
        ///  試合後かどうかを判断する
        ///  "3=試合後
        /// </summary>
        /// <param name="dateCheck"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static int GetStatusMatch(string gameId)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(gameId))
            {
                JlgEntities Jlg = new JlgEntities();
                var scheduleInfos = (from sdi in Jlg.ScheduleInfo //全試合速報
                                     where sdi.GameID.ToString().Equals(gameId)
                                     select new {
                GameDate = sdi.GameDate,
                GameTime = sdi.GameTime
            }).FirstOrDefault(); //Order by ha?

                if (scheduleInfos != null)
                {

                    string dateStr = scheduleInfos.GameDate.ToString();
                    string timeStr = (scheduleInfos.GameTime == null ? "0000" : scheduleInfos.GameTime.ToString()).PadLeft(4, '0');
                    DateTime matchDate = DateTime.ParseExact(dateStr + timeStr, "yyyyMMddHHmm", null);

                    if (DateTime.Now < matchDate)
                    {
                        result = 0;         //―＞試合前
                    }
                    else if (DateTime.Now >= matchDate)
                    {
                        //開始時刻後

                        //試合速報を確認する
                        var realGameInfos = (from sr in Jlg.StatsReportLS
                                           where sr.GameID.ToString() == gameId
                                           select sr ).FirstOrDefault();

                        if (realGameInfos != null)
                        {
                            //Jlg GameState 4=試合終了、
                            string gameSetSituationCD = GetNpbGameStateID((short)realGameInfos.SituationID);

      
                            //試合速報が存在する
                            if (gameSetSituationCD == "1")   //試合中                 
                            {
                                //   if (gameInfoRGI.Inning.Value >= 0) 
                                //   {
                                result = 1;   //―＞試合中                       
                                //   }
                            }
                            else if (gameSetSituationCD == "4") //正常終了
                            {
                                result = 2;   //―＞試合後                     
                            }
                            else if (gameSetSituationCD == "9")  //試合中中止
                            {
                                result = 9;  //―＞試合後                 
                            }
                            else 
                            {
                                result = 0;  //―＞試合前
                            }
                        }
                        else
                        {
                            //試合速報が存在しない
                            result = 0;   //―＞試合前                       
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
            var oddinfo = oddsService.GetOddsInfoByGameID(Constants.JLG_SPORT_ID, gameId, memberId);
            if (oddinfo.ExpectTargetID == 0) return 1;

            var checkBet = oddinfo.BetSelectedID != null;  // BET有無

            var jlg = new JlgEntities();
            var gameInfos = (from si in jlg.ScheduleInfo //年間試合スケジュール_試合情報
                             where si.GameID == gameId
                             select new JlgGameInfoByCondidtion
                             {
                                ScheduleInfoJlg = si,
                                GameTimetoStr = si.GameTime.ToString()
                             }).First();

            int result = 0;

            if (gameInfos == null) return result;

            var gameDate = Utils.ConvertStrToDatetime(gameInfos.ScheduleInfoJlg.GameDate.ToString());

            var gameStartDateAndTime = DateTime.MaxValue; // "未定"やnullがありうる
            int time;
            if (int.TryParse(gameInfos.GameTimetoStr, out time))
            {
                var hours = Convert.ToInt32(time.ToString().Substring(0, 2));
                var minute = Convert.ToInt32(time.ToString().Substring(2, 2));

                gameStartDateAndTime = new DateTime(gameDate.Year, gameDate.Month, gameDate.Day, hours, minute, 0);
            }

            //予想可能日付閾値(今日日付基準の月末日取得)
            var expectableEnd = DateTime.Now.EndOfTheMonthWithTime();

            if (DateTime.Now < gameStartDateAndTime)
            {
                //Case 1:Can not bet
                if (gameStartDateAndTime > expectableEnd)
                {
                    return 1;                             // 1:ベット不可
                }
                
                DateTime limitTime = gameStartDateAndTime.AddMinutes(-5);
            
                if (DateTime.Now < limitTime)
                {
                    return !checkBet ? 2 : 3;         // 2:5分前以前、ベットなし  3:5分前以前、ベットあり
                }
            
                return !checkBet ? 4 : 5;         // 3:5分前以降、ベットなし  4:5分前以前、ベットあり
            }

            // 一試合個人スタッツ_ヘッダー情報
            var realGameInfos = from sr in jlg.StatsReportLS where sr.GameID == gameId select sr;

            if (realGameInfos != null)
            {
                if (realGameInfos.FirstOrDefault() != null)
                {
                    //空白:試合中または試合前 2=正常終了　7=サスペンデッド 9=中止（試合中）
                    string gameSetSituationCd = GetNpbGameStateID((short)realGameInfos.FirstOrDefault().SituationID);

                    if ((!string.IsNullOrEmpty(gameSetSituationCd)) && ((gameSetSituationCd == "8") ||(gameSetSituationCd == "9")))
                    {
                        return 10;
                    }

                    switch (gameSetSituationCd)
                    {
                        case "0":
                            return !checkBet ? 4 : 5;         // 4:試合前、ベットなし 5:試合前、ベットあり
                        case "1":
                            return !checkBet ? 6 : 7;         // 8:試合中、ベットなし 7:試合中、ベットあり
                        case "4":
                            return !checkBet ? 8 : 9;         // 8:試合終了、ベットなし 9:試合終了、ベットあり
                    }

                    return 10;                    // 10:試合中止
                }

                return !checkBet ? 4 : 5;         // 3:5分前以降、ベットなし  4:5分前以前、ベットあり
            }

            return !checkBet ? 4 : 5;         // 3:5分前以降、ベットなし  4:5分前以前、ベットあり
        }

        #endregion

        /// <summary>
        /// 現在日付以降のシーズンを取得する
        /// </summary>
        /// <param name="dateInput"></param>
        /// <returns></returns>
        public static int GetSeasonID(int dateInput, int jtype)
        {
            JlgEntities jlg = new JlgEntities();

            int gameKindId = 0;
            if (jtype == 1)
            {
                gameKindId = JlgConstants.JLG_GAMEKIND_J1;
            }
            else if (jtype == 2)
            {
                gameKindId = JlgConstants.JLG_GAMEKIND_J2;
            }
            else
            {
                gameKindId = JlgConstants.JLG_GAMEKIND_NABISCO;
            }

            //dateInput = 20150309;
            var query = (from si in jlg.ScheduleInfo
                         join gcat in jlg.GameCategory on si.GameCategoryId equals gcat.GameCategoryId
                         join gs in jlg.GameSchedule on gcat.GameScheduleId equals gs.GameScheduleId
                         where (si.GameDate >= dateInput)
                         where gs.GameKindID == gameKindId
                         orderby si.GameDate
                         select gcat).FirstOrDefault();

            return (query == null) ? 1 : query.SeasonID;
        }

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
                         where (et.SportsID == Constants.JLG_SPORT_ID && et.ClassClass == 4 && et.UniqueID == gameID)
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
                         where (et.SportsID == Constants.JLG_SPORT_ID && et.ClassClass == 4 && et.UniqueID == gameID)
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

        #region Get TeamInfoGTS By GameID TeamID
        /// <summary>
        /// Get score in table GameTextScore By GameID and TeamID.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <param name="teamID">TeamID</param>
        /// <returns>Score if exists or 0 if not exist.</returns>
        public static ScoreGameInfo GetTeamInfoGTSByGameIDTeamID(int gameID, int teamID)
        {
            JlgEntities Jlg = new JlgEntities();
            var query = (from grl in Jlg.GameReportLG 
                         join til in Jlg.TeamInfoLG on grl.GameReportLGId equals til.GameReportLGId
                         where grl.GameID == gameID && til.ID == teamID
                         select new ScoreGameInfo
                         {
                             GameID = grl.GameID,
                             TeamID = teamID,
                             R = til.Score

                         }).FirstOrDefault();

            ScoreGameInfo scoreInfo = new ScoreGameInfo();
            if (query == null)
            {
                scoreInfo.GameID = gameID;
                scoreInfo.TeamID = teamID;
                return scoreInfo;
            }
            else
            {
                return query;
            }
        }

        #endregion
    }
}
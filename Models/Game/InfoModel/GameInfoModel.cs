using System;
namespace Splg.Models.Game.InfoModel
{
    public class GameInfoModel
    {
        /// <summary>
        /// 
        /// TODO 中止になったら賭けたポイントはどうなる？Lose？
        /// 
        /// 0 = Match not yet started
        /// 1 = Match ongoing
        /// 2 = Match finish
        /// 3 = Match delay
        /// </summary>
        public int GameStatusSimple { 
            
            get{
                if (this.GameStatus < 6)
                    return 0;

                if (this.GameStatus < 8)
                    return 1;

                return 2;
            }  
        }

        /// <summary>
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
        /// </summary>
        public int GameStatus { get; set; }
        public DateTime? StartScheduleDate;
        public int SportsID;
        public DateTime StartDt;
        public int TargetYear;      // 年
        public int TargetMonth;     // 月

        public long ExpectTargetID;

        //試合開始日（実際の試合日を優先）
        public int? usingDate
        {
            get
            {
                if (GameDateActual != null)
                    return GameDateActual;
                else
                    return GameDateScheduled;
            }

        }

        public string FormattedUsingDateTime
        {
            get
            {
                DateTime dt = DateTime.ParseExact(usingDate.ToString() + (usingTime == null ? "0000" : usingTime), "yyyyMMddHHmm", null);
                return Utils.FormatDateTime(dt);

            }
        }
        //試合開始時間（実際の試合日を優先）
        public string usingTime
        {
            get
            {
                if (GameDateActual != null)
                    return GameTimeActual;
                else
                    return GameTimeScheduled;
            }

        }

        public int GameDateScheduled;
        public string GameTimeScheduled;
        public int? GameDateActual;
        public string GameTimeActual;

        string gameDatetime;
        /// <summary>
        /// 表示日時
        /// </summary>
        public string GameDateTime
        {
            set
            {
                gameDatetime = value;
            }
            get
            {
                if (gameDatetime == null)
                    return GameDatetimeyyyyMMddHHmm.ToString("M/d(ddd) HH:mm");
                else
                    return gameDatetime;
            }
        }
        // 日時
        public string Date
        {
            get
            {
                return GameDatetimeyyyyMMddHHmm.ToString("M月d日");
            }
        }
        // 日時
        public string Time
        {
            get
            {
                return GameDatetimeyyyyMMddHHmm.ToString("HH:mm");
            }
        }

        public DateTime GameDatetimeyyyyMMddHHmm
        {
            get
            {
                return DateTime.ParseExact(usingDate.ToString() + (usingTime == null ? "0000" : usingTime), "yyyyMMddHHmm", null);
            }
        }

        public int GameID;
        /// <summary>
        /// ゲーム名
        /// </summary>
        public string GameName
        {
            get
            {
                return HomeTeamName + " vs " + VisitorTeamName;
            }
        }


        public int HomeTeamID;
        public string HomeTeamName;
        private string homeTeamS;
        public string HomeTeamUrl;
        private string homeTeamIcon;
        public string HomeTeamIcon
        {
            get
            {
                string result = "/Content/News/PN_UTF8/photo/default.png";
                if (!String.IsNullOrEmpty(homeTeamIcon))
                {
                    if (!homeTeamIcon.StartsWith("/") && !homeTeamIcon.StartsWith("~"))
                        homeTeamIcon = "/" + homeTeamIcon;

                    return homeTeamIcon;
                }

                return result;
            }
            set { homeTeamIcon = value; }
        }

        public string HomeTeamS
        {
            get
            {
                if (String.IsNullOrEmpty(homeTeamS))
                    return HomeTeamName;
                return homeTeamS;
            }
            set { homeTeamS = value; }
        }
        public int VisitorTeamID;
        public string VisitorTeamName;
        private string visitorTeamS;
        public string VisitorTeamUrl;
        private string vistorTeamIcon;
        public string VisitorTeamIcon
        {
            get
            {
                string result = "/Content/News/PN_UTF8/photo/default.png";
                if (!String.IsNullOrEmpty(vistorTeamIcon))
                {
                    if (!vistorTeamIcon.StartsWith("/") && !vistorTeamIcon.StartsWith("~"))
                        vistorTeamIcon = "/" + vistorTeamIcon;

                    return vistorTeamIcon;
                }

                return result;
            }
            set { vistorTeamIcon = value; }
        }        
        public string VisitorTeamS
        {
            get
            {
                if (String.IsNullOrEmpty(visitorTeamS))
                    return VisitorTeamName;
                return visitorTeamS;
            }
            set { visitorTeamS = value; }
        }

        public int Situation;       // 1;試合中 2:終了
        public string TeamName;     // 予想・的中チーム
        public string TeamNameS;
        public string Url;          // 試合URL
        public string UrlArticle;   // 記事作成URL
        public string HomeScore;
        public string VisitorScore;

        /// <summary>
        /// 1:home 2:visitor
        /// </summary>
        /// <remarks>
        /// 1:WinTeamCd = HomeTeamID
        /// 2:WinTeamCd = VisitorTeamID
        /// </remarks>
        public int WinnerTeam;

        public int WinTeamCd;

        public int Inning;
        public string BottomTop;
        public int? HomeTeamWin;
        public int? VisitorTeamWin;
        public int HomeTeamRanking { get; set; }
        public int VisitorTeamRanking { get; set; }

        public string PreForeRunnerNameSH { get; set; }
        public string PreForeRunnerEraH { get; set; }
        public string PreForeRunnerNameSV { get; set; }
        public string PreForeRunnerEraV { get; set; }
        public string ForeRunnerNameSH { get; set; }
        public string ForeRunnerEraH { get; set; }
        public string ForeRunnerNameSV { get; set; }
        public string ForeRunnerEraV { get; set; }
        public string WinLoseNameSH { get; set; }
        public string WinLoseEraH { get; set; }
        public string WinLoseNameSV { get; set; }
        public string WinLoseEraV { get; set; }
        public bool IsWinH { get; set; }
        public bool IsWinV { get; set; }
        public string WinLoseTextH { get; set; }
        public string WinLoseTextV { get; set; }

        public int WinLose;         // 予想結果 ０・・無効、１・・勝ち、２・・負け
        public int SituationStatus; // １・・予想、２・・キャンセル、３・・中止、４・・結果確定


        public int Points
        {
            get
            {
                if (SituationStatus != 2)    // １・・予想、２・・キャンセル、３・・中止、４・・結果確定
                {
                    return ExpectPoint1;
                }
                else return 0;

            }

        }          // 予想ポイント
        public Nullable<int> BetSelectID;
        public int ExpectPoint1;

        public bool BetWin
        {
            get
            {
                return (BetSelectID == WinnerTeam);
            }
        }

        public decimal Odds
        {

            get
            {

                decimal reault = (decimal)0;

                if (OddsInfoModel != null)
                {
                    switch (BetSelectID)
                    {
                        case 1:
                            reault = OddsInfoModel.HomeTeamOdds;
                            break;
                        case 2:
                            reault = OddsInfoModel.VisitorTeamOdds;
                            break;
                        case 3:
                            reault = OddsInfoModel.DrawOdds;
                            break;

                    }
                }

                reault = Math.Round(reault, 1, MidpointRounding.AwayFromZero);

                return reault;

            }


        }   
        
        // オッズ
        public GameOddsInfoModel OddsInfoModel;

        /// <summary>
        /// 獲得ポイント(予想ポイント×オッズ）
        /// </summary>
        public long WinPoints
        {
            get
            {
                return (long)((decimal)Points * (decimal)Odds);
            }
        }

        public string ExpectionResult
        {
            get
            {
                string result = "";
                if (BetSelectID == 1)
                {
                    result = HomeWinText;
                }
                else if (BetSelectID == 2)
                {
                    if (SportsID == 2)
                    {   //JLG
                        result = VisitorWinText;
                    }
                    else
                    {
                        result = VisitorWinText;
                    }
                }
                else if (BetSelectID == 3)
                {
                    result = DrawText;
                }
                return result;
            }
        }
        #region ホームの勝ちとかの文言制御（そのうちチームだけじゃなくなるよね）
        // ホームの勝ち
        public string HomeWinText
        {
            get
            {
                string result = "";
                switch(SportsID)
                {
                    case Constants.NPB_SPORT_ID:
                    case Constants.MLB_SPORT_ID:
                    case Constants.JLG_SPORT_ID:
                    default:
                       result = HomeTeamS + "の勝ち";
                       break;
                }
                return result;
            }
        }
        // 引き分け
        public string DrawText
        {
            get
            {
                string result = "";
                switch (SportsID)
                {
                    case Constants.NPB_SPORT_ID:
                    case Constants.MLB_SPORT_ID:
                    case Constants.JLG_SPORT_ID:
                    default:
                        result = "引き分け";
                        break;
                }
                return result;
            }
        }
        // ビジター（アウェイ）の勝ち
        public string VisitorWinText
        {
            get
            {
                string result = "";
                switch (SportsID)
                {
                    case Constants.NPB_SPORT_ID:
                    case Constants.MLB_SPORT_ID:
                        result = VisitorTeamS + "の勝ち";
                        break;
                    case Constants.JLG_SPORT_ID:
                        result = VisitorTeamS + "の勝ち";
                        break;
                    default:
                        result = VisitorTeamS + "の勝ち";
                        break;
                }
                return result;
            }
        }

        #endregion
        #region 試合情報カードのスタイル制御
        // ホームの勝ち
        public string betHomeClass
        {
            get
            {
                string result = "";
                if (BetSelectID == 1)
                {
                    result = "green";
                }
                else if (BetSelectID == 2)
                {
                    result = "";
                }
                else if (BetSelectID == 3)
                {
                    result = "";
                }
                return result;
            }
        }
        public string betHomeOddsClass
        {
            get
            {
                string result = "";
                if (BetSelectID == 1)
                {
                    result = "green";
                }
                else if (BetSelectID == 2)
                {
                    result = "gray";
                }
                else if (BetSelectID == 3)
                {
                    result = "gray";
                }
                return result;
            }
        }
        public string betHomeBtnClass
        {
            get
            {
                string result = "";
                if (BetSelectID == 1)
                {
                    result = "change hasbet";
                }
                else if (BetSelectID == 2)
                {
                    result = "disable";
                }
                else if (BetSelectID == 3)
                {
                    result = "disable";
                }
                return result;
            }
        }
        // ビジター（アウェイ）の勝ち
        public string betVisitorClass
        {
            get
            {
                string result = "";
                if (BetSelectID == 1)
                {
                    result = "";
                }
                else if (BetSelectID == 2)
                {
                    result = "green";
                }
                else if (BetSelectID == 3)
                {
                    result = "";
                }
                return result;
            }
        }
        public string betVisitorOddsClass
        {
            get
            {
                string result = "";
                if (BetSelectID == 1)
                {
                    result = "gray";
                }
                else if (BetSelectID == 2)
                {
                    result = "green";
                }
                else if (BetSelectID == 3)
                {
                    result = "gray";
                }
                return result;
            }
        }
        public string betVisitorBtnClass
        {
            get
            {
                string result = "";
                if (BetSelectID == 1)
                {
                    result = "disable";
                }
                else if (BetSelectID == 2)
                {
                    result = "change hasbet";
                }
                else if (BetSelectID == 3)
                {
                    result = "disable";
                }
                return result;
            }
        }
        // 引き分け
        public string betDrawClass
        {
            get
            {
                string result = "";
                if (BetSelectID == 1)
                {
                    result = "";
                }
                else if (BetSelectID == 2)
                {
                    result = "";
                }
                else if (BetSelectID == 3)
                {
                    result = "green";
                }
                return result;
            }
        }
        public string betDrawOddsClass
        {
            get
            {
                string result = "";
                if (BetSelectID == 1)
                {
                    result = "gray";
                }
                else if (BetSelectID == 2)
                {
                    result = "gray";
                }
                else if (BetSelectID == 3)
                {
                    result = "green";
                }
                return result;
            }
        }
        public string betDrawBtnClass
        {
            get
            {
                string result = "";
                if (BetSelectID == 1)
                {
                    result = "disable";
                }
                else if (BetSelectID == 2)
                {
                    result = "disable";
                }
                else if (BetSelectID == 3)
                {
                    result = "change hasbet";
                }
                return result;
            }
        }
        #endregion
        #region スコア情報
        // 引き分け
        public string DispHomeScore
        {
            get
            {
                string result = "";
                switch (SportsID)
                {
                    case Constants.NPB_SPORT_ID:
                    case Constants.JLG_SPORT_ID:
                        result = HomeScore;
                        break;
                    case Constants.MLB_SPORT_ID:
                    default:
                        result = "　";
                        break;
                }

                return result;
            }
        }
        public string DispInning
        {
            get
            {
                string result = "";
                switch (SportsID)
                {
                    case Constants.NPB_SPORT_ID:
                        result = Inning + BottomTop;
                        break;
                    case Constants.JLG_SPORT_ID:
                    case Constants.MLB_SPORT_ID:
                    default:
                        result = "試合中";
                        break;
                }
                return result;
            }
        }
        public string DispVisitorScore
        {
            get
            {
                string result = "";
                switch (SportsID)
                {
                    case Constants.NPB_SPORT_ID:
                    case Constants.JLG_SPORT_ID:
                        result = VisitorScore;
                        break;
                    case Constants.MLB_SPORT_ID:
                    default:
                        result = "　";
                        break;
                }
                return result;
            }
        }
        #endregion

        /// <summary>
        /// コメント
        /// </summary>
        public string Comment
        {
            get
            {
                string result = "オッズは" + Odds + "倍で、";

                if (BetWin)
                {
                    result += WinPoints + "ptを得ました。";
                }
                else
                {
                    if (this.GameStatusSimple == 2)
                    {
                        result += "的中しませんでした。";
                    }
                    else
                    {
                        result += "的中すると" + WinPoints + "ptになります。";
                    }
                }

                return result;

            }


        }

        /// <summary>
        /// 短いコメント
        /// </summary>
        public string CommentS
        {
            get
            {
                string result = "";
                if (BetWin)
                {
                    result += WinPoints + "ptを得ました。";
                }
                else
                {
                    if (this.GameStatusSimple == 2)
                    {
                        result += "的中しませんでした。";
                    }
                    else
                    {
                        result += "的中すると" + WinPoints + "ptになります。";
                    }
                }
                return result;
            }
        }

        public GamePointInfoModel MyPointInfoModel;

        public Boolean ArticleDispFlg;  // 投稿記事の部分を表示するか否か


    }


}
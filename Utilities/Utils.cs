using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.IO;
using System.Drawing;
using Splg.Areas.Npb.Models;
using Splg.Areas.Jleague;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Mlb;
using Splg.Areas.Mlb.Models;
using Splg.Models;
using Splg.Models.Game.ViewModel;
using Splg.Areas.MyPage.Models.InfoModel;

namespace Splg
{
    public static class Utils
    {
        //==========================Hai Nguyen========================//
        public static string FormatStringToAsterisk(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var arr = input.ToCharArray();

                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = '*';
                }

                return new string(arr);
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayInput"></param>
        /// <returns></returns>
        public static string GetMonthAndDayOfWeek(DateTime dayInput)
        {
            string result = string.Empty;
            if (dayInput != null)
            {
                //Get culture from we.config and 
                GlobalizationSection globalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
                var cultureInfo = new System.Globalization.CultureInfo(globalizationSection.Culture.ToString());
                var dateTimeInfo = cultureInfo.DateTimeFormat;
                var dayNames = dateTimeInfo.DayNames;

                var dayOfWeek = dayNames[(int)dayInput.Date.DayOfWeek];
                result = dayInput.ToString("M月d日") + "(" + dayOfWeek.Substring(0, 1) + ")";
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetWeekOfMonth(int year, int month)
        {
            DayOfWeek wkstart = DayOfWeek.Monday;
            DateTime first = new DateTime(year, month, 1);
            int firstwkday = (int)first.DayOfWeek;
            int otherwkday = (int)wkstart;
            int offset = ((otherwkday + 7) - firstwkday) % 7;
            double weeks = (double)(DateTime.DaysInMonth(year, month) - offset) / 7d;
            return (int)Math.Ceiling(weeks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }

        /// <summary>
        /// Convert string to datetime
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static DateTime ConvertStrToDatetime(string inputString)
        {
            Regex format = new Regex("(?<year>^[0-9]{4})(?<month>[0-9]{2})(?<day>[0-9]{2})");
            Match m = format.Match(inputString);
            int year = int.Parse(m.Groups["year"].Value);
            int month = int.Parse(m.Groups["month"].Value);
            int day = int.Parse(m.Groups["day"].Value);
            DateTime date = new DateTime(year, month, day);
            return date;
        }

        /// <summary>
        /// Convert string to datetime
        /// </summary>
        /// <param name="inputString">YYYYMMDD</param>
        /// <returns>MM/DD</returns>
        public static string ConvertStrToShortUSDatetime(string inputString)
        {
            Regex format = new Regex("(?<year>^[0-9]{4})(?<month>[0-9]{2})(?<day>[0-9]{2})");
            Match m = format.Match(inputString);
            int year = int.Parse(m.Groups["year"].Value);
            int month = int.Parse(m.Groups["month"].Value);
            int day = int.Parse(m.Groups["day"].Value);
            string date = month + "/" + day;
            return date;
        }

        public static string GetDayOfWeek(int year, int month, int week, int day)
        {
            DateTime date = new DateTime(year, month, 1);
            int firstDay = date.Day;

            int dataTemp = 0;
            if (firstDay == 1)
            {
                dataTemp += 7 * (week - 1);
            }
            else
            {
                dataTemp += (7 - firstDay + 1) + 7 * (week - 1);
            }

            DateTime result = new DateTime(year, month, dataTemp);
            int y = result.Year;
            string m = result.Month < 10 ? "0" + result.Month : result.Month.ToString();
            string d = result.Day < 10 ? "0" + result.Day : result.Day.ToString();

            return string.Format("{0}{1}{2}", result.Year, m, d);
        }

        /// <summary>
        /// get last monday of week
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMondayOfLastWeek(DateTime dt)
        {
            var dateDayOfWeek = (int)dt.DayOfWeek;
            if (dateDayOfWeek == 0)
            {
                dateDayOfWeek = dateDayOfWeek + 7;
            }
            var alterNumber = dateDayOfWeek - ((dateDayOfWeek * 2) - 1);
            DateTime mondayCurrentWeek = dt.AddDays(alterNumber);
            DateTime mondayLastWeek = mondayCurrentWeek.AddDays(-7);
            return mondayLastWeek;
        }

        /// <summary>
        /// get last monday of month
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>date of last monday</returns>
        public static DateTime GetLastMondayOfMonth(int month, int year)
        {
            DateTime dt;
            dt = (month < 12) ? new DateTime(year, month + 1, 1) : new DateTime(year + 1, 1, 1);
            dt = dt.AddDays(-1);
            while (dt.DayOfWeek != DayOfWeek.Monday)
            {
                dt = dt.AddDays(-1);
            }
            return dt;
        }

       //==========================End ========================//


        //*****************************************************************************************************************//
        // Author : CucHTP
        // Created Date : 02/05/2015
        //*****************************************************************************************************************//

        #region Utils - CucHTP

        #region Parse LocalTime To JapanTime
        /// <summary>
        /// Parse time from local time to japan time with format : HH時mm分
        /// </summary>
        /// <param name="needParse">Time need parse.</param>
        /// <returns>Time that parsed same as format.</returns>
        public static string ParseLocalTimeToJapanTime(DateTime needParse)
        {
            string result = string.Empty;
            if (needParse != null)
            {
                //Get culture from we.config and 
                GlobalizationSection globalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
                var cultureInfo = new System.Globalization.CultureInfo(globalizationSection.Culture.ToString());
                var dateTimeInfo = cultureInfo.DateTimeFormat;
                var dayNames = dateTimeInfo.DayNames;

                var dayOfWeek = dayNames[(int)needParse.Date.DayOfWeek];
                var dayJapan = needParse.ToString("d日") + "(" + dayOfWeek.Substring(0, 1) + ")";
                result = dayJapan + needParse.ToString("HH時mm分");
            }
            return result;
        }
        #endregion

        #region Get NewsImage Location
        /// <summary>
        /// Get location folder for photo news.
        /// </summary>
        /// <param name="filename">Image filename.</param>
        /// <returns>String of location.</returns>
        public static string GetNewsImageLocation(string filename, bool isNewsDetail = false)
        {
            string imageLocation = string.Empty;
            string folderLocation = string.Empty;
            folderLocation = Constants.IMAGE_DIRECTORY;
            imageLocation = folderLocation + filename;
            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(imageLocation)) && !isNewsDetail)
            {
                imageLocation = folderLocation + Constants.IMAGE_DEFAULT;
            }
            return imageLocation;
        }
        #endregion

        #region Parse LocalDate To Short JapanDate
        /// <summary>
        /// Parse LocalDate To Short JapanDate : not include year.
        /// </summary>
        public static string ParseLocalDateToShortJapanDate(DateTime needParse)
        {
            string result = string.Empty;
            if (needParse != null)
            {
                //Get culture from we.config and 
                GlobalizationSection globalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
                var cultureInfo = new System.Globalization.CultureInfo(globalizationSection.Culture.ToString());
                var dateTimeInfo = cultureInfo.DateTimeFormat;
                var dayNames = dateTimeInfo.DayNames;

                var dayOfWeek = dayNames[(int)needParse.Date.DayOfWeek];
                result = needParse.ToString("M月d日") + "(" + dayOfWeek.Substring(0, 1) + ")" + needParse.ToString("HH時mm分") + "配信";
            }
            return result;
        }
        #endregion

        #region Parse LocalDate To Short "MM月dd日"
        /// <summary>
        /// Parse LocalDate To Short JapanDate : not include year.
        /// </summary>
        public static string ParseLocalDateToShortJapanDateMMdd(DateTime needParse)
        {
            string result = string.Empty;
            if (needParse != null)
            {
                //Get culture from we.config and 
                GlobalizationSection globalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
                var cultureInfo = new System.Globalization.CultureInfo(globalizationSection.Culture.ToString());
                var dateTimeInfo = cultureInfo.DateTimeFormat;
                var dayNames = dateTimeInfo.DayNames;

                var dayOfWeek = dayNames[(int)needParse.Date.DayOfWeek];
                result = needParse.ToString("MM月dd日") + "(" + dayOfWeek.Substring(0, 1) + ")" + "の試合";
            }
            return result;
        }
        #endregion

        #region Parse LocalDate To Short "MM月"
        /// <summary>
        /// Parse LocalDate To Short JapanDate : not include year.
        /// </summary>
        public static string ParseLocalDateToShortJapanDateMM(DateTime needParse)
        {
            string result = string.Empty;
            if (needParse != null)
            {
                //Get culture from we.config and 
                GlobalizationSection globalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
                var cultureInfo = new System.Globalization.CultureInfo(globalizationSection.Culture.ToString());
                var dateTimeInfo = cultureInfo.DateTimeFormat;
                var dayNames = dateTimeInfo.DayNames;

                var dayOfWeek = dayNames[(int)needParse.Date.DayOfWeek];
                result = needParse.ToString("MM月") + "の試合";
            }
            return result;
        }
        #endregion

        #region Parse LocalDate To Long JapanDate
        /// <summary>
        /// Parse LocalDate To Long JapanDate : include year.
        /// </summary>
        /// Adding parametter [isWithWeek] default true to return a date without week name (by Huynh)
        /// 
        public static string ParseLocalDateToLongJapanDate(DateTime needParse, bool isWithWeek = true)
        {
            string result = string.Empty;
            if (needParse != null)
            {
                //Get culture from we.config and 
                GlobalizationSection globalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
                var cultureInfo = new System.Globalization.CultureInfo(globalizationSection.Culture.ToString());
                var dateTimeInfo = cultureInfo.DateTimeFormat;
                var dayNames = dateTimeInfo.DayNames;

                var dayOfWeek = dayNames[(int)needParse.Date.DayOfWeek];
                result = needParse.ToString("yyyy年M月d日") + "(" + dayOfWeek.Substring(0, 1) + ")" + needParse.ToString("HH時mm分");
                if (!isWithWeek)
                    result = needParse.ToString("yyyy年M月d日 HH時mm分");
            }
            return result;
        }
        #endregion

        #region Parse To JapanDate
        /// <summary>
        /// Parse string to japan date with include year or not.
        /// </summary>
        /// <param name="bFlag">True : include year, False : not include year.</param>
        /// <param name="needParse">String need parse.</param>
        /// <returns>String parsed.</returns>
        public static string ParseToJapanDate(bool bFlag, string needParse)
        {
            string result = string.Empty;
            if (needParse != null)
            {
                if (bFlag)
                {
                    result = DateTime.ParseExact(needParse, "yyyyMMdd", null).ToString("yyyy年MM月dd日");
                }
                else
                {
                    result = DateTime.ParseExact(needParse, "yyyyMMdd", null).ToString("MM/dd");
                }
            }
            return result;
        }
        #endregion

        #region Get PitchingArm Name
        /// <summary>
        /// Change pitching arm code to name
        /// </summary>
        /// <param name="pitchingArm">Pitching Arm code.</param>
        /// <returns>Pitching arm name.</returns>
        public static string GetPitchingArmName(int pitchingArm)
        {
            string pitchingName = string.Empty;
            if (pitchingArm == 1)
            {
                pitchingName = Constants.PITCHERARM_1;
            }
            else
            {
                pitchingName = Constants.PITCHERARM_2;
            }
            return pitchingName;
        }
        #endregion

        #region Get BattingType Name
        /// <summary>
        /// Change batting type code to name.
        /// </summary>
        /// <param name="battingType">Batting type code.</param>
        /// <returns>Batting type name.</returns>
        public static string GetBattingTypeName(int battingType)
        {
            string battingName = string.Empty;
            if (battingType == 1)
            {
                battingName = Constants.BATTINGTYPE_1;
            }
            else if (battingType == 2)
            {
                battingName = Constants.BATTINGTYPE_2;
            }
            else
            {
                battingName = Constants.BATTINGTYPE_3;
            }
            return battingName;
        }
        #endregion

        #region Get PitchingResult Name
        /// <summary>
        /// Get Pitching result name.
        /// </summary>
        /// <param name="pitchingResult">Pitching Result need change to name.</param>
        /// <returns>String changed to name.</returns>
        public static string GetPitchingResultName(int pitchingResult)
        {
            var result = string.Empty;
            if (pitchingResult != -1)
            {
                if (pitchingResult == 0)
                {
                    result = Constants.PITCHINGRESULT_0;
                }
                else if (pitchingResult == 1)
                {
                    result = Constants.PITCHINGRESULT_1;
                }
                else if (pitchingResult == 2)
                {
                    result = Constants.PITCHINGRESULT_2;
                }
                else if (pitchingResult == 3)
                {
                    result = Constants.PITCHINGRESULT_3;
                }
                else
                    result = Constants.PITCHINGRESULT_4;

            }
            return result;
        }
        #endregion

        #region Get HittingResult Name
        /// <summary>
        /// Convert hitting result code to hitting result name. 
        /// </summary>
        /// <param name="hittingResult">HittingResult</param>
        /// <returns>HittingResult Name.</returns>
        public static string GetHittingResultName(short? hittingResult, int? HTeamID, int? VTeamID, int teamID)
        {
            var result = string.Empty;
            if (HTeamID.Value == teamID)
            {
                if (hittingResult == 1)
                {
                    result = Constants.HITTINGRESULT_1;
                }
                else if (hittingResult == 2)
                {
                    result = Constants.HITTINGRESULT_2;
                }
            }
            else if (VTeamID == teamID)
            {
                if (hittingResult == 1)
                {
                    result = Constants.HITTINGRESULT_2;
                }
                else if (hittingResult == 2)
                {
                    result = Constants.HITTINGRESULT_1;
                }
            }
            if (hittingResult == 0)
            {
                result = Constants.HITTINGRESULT_0;
            }
            else if (hittingResult == 3)
            {
                result = Constants.HITTINGRESULT_3;
            }
            return result;
        }
        #endregion

        #region Get Position Name
        /// <summary>
        /// Change position type id to position type name.
        /// </summary>
        /// <param name="positionType">PositionTypeID</param>
        /// <returns>Position name.</returns>
        public static string GetPositionName(int? positionType)
        {
            string result = string.Empty;
            if (positionType == 1)
            {
                result = Constants.POSITIONTYPE_1;
            }
            else if (positionType == 2)
            {
                result = Constants.POSITIONTYPE_2;
            }
            else if (positionType == 3)
            {
                result = Constants.POSITIONTYPE_3;
            }
            else if (positionType == 4)
            {
                result = Constants.POSITIONTYPE_4;
            }
            return result;
        }
        #endregion

        #region Format InningPitched
        /// <summary>
        /// Format inning pitched : (a) + (b)
        /// (a) : inningsPitched
        /// (b) : inningsPitched3rd != null -> (b) : inningsPitched3rd/3
        ///       inningsPitched3rd == null   
        /// </summary>
        /// <param name="inningsPitched">inningsPitched</param>
        /// <param name="inningsPitched3rd">inningsPitched3rd</param>
        /// <returns>String formated.</returns>
        public static string FormatInningPitched(bool bFlag, string inningsPitched, string inningsPitched3rd)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(inningsPitched))
            {
                if (bFlag)
                {
                    if (string.IsNullOrEmpty(inningsPitched3rd) || Convert.ToInt32(inningsPitched3rd) == 0)
                    {
                        result = inningsPitched;
                    }
                    else
                    {
                        result = inningsPitched + " " + inningsPitched3rd + "/3";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(inningsPitched3rd))
                    {
                        result = inningsPitched;
                    }
                    else
                    {
                        result = inningsPitched + " " + inningsPitched3rd + "/3";
                    }
                }
            }
            return result;

        }
        #endregion

        #region Format Percentage
        /// <summary>
        /// Format percentage : number + %.
        /// </summary>
        /// <param name="strNeedFormat">String Need format.</param>
        /// <returns>String formatted.</returns>
        public static string FormatPercentage(string strNeedFormat)
        {
            decimal winPercen;
            var percentage = string.Empty;
            if (Decimal.TryParse(strNeedFormat, out winPercen))
            {
                winPercen = winPercen * 100;
                percentage = winPercen.ToString("#.##") + "%";
            }
            else
            {
                percentage = strNeedFormat;
            }
            return percentage;
        }
        #endregion

        #region Get Mound Name
        /// <summary>
        /// Change mound cd to mound name.
        /// </summary>
        /// <param name="moundCD">MoundCD.</param>
        /// <returns>Mound Name.</returns>
        public static string GetMoundName(int moundCD)
        {
            string result = string.Empty;
            if (moundCD == 0)
            {
                result = Constants.MOUND_0;
            }
            else if (moundCD == 1)
            {
                result = Constants.MOUND_1;
            }
            else if (moundCD == 2)
            {
                result = Constants.MOUND_2;
            }
            else
            {
                result = Constants.MOUND_3;
            }
            return result;
        }
        #endregion

        #region Hash Password using MD5 Algorithm
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        #endregion

        #region Get Round Name
        /// <summary>
        /// Change TB to round name.
        /// </summary>
        /// <param name="intTB">TB.MLBの場合はnullを配慮する</param>
        /// <returns>Round Name.</returns>
        public static string GetRoundName(int? intTB)
        {
            string strResult = string.Empty;
            if (intTB == 1)
            {
                strResult = Constants.ROUND_TOP;
            }
            else if (intTB == 2)
            {
                strResult = Constants.ROUND_BOTTOM;
            }
            else
            {
                strResult = Constants.STRING_ROUND;
            }
            return strResult;
        }
        #endregion

        #region Get TimeSpan Hours And Minutes
        /// <summary>
        /// Get time span from start time to end time and format Jpn.
        /// </summary>
        /// <param name="startTime">Start time.</param>
        /// <param name="endTime">End time.</param>
        /// <returns>Hours and Minutes formated.</returns>
        public static string GetTimeSpan(int startTime, int endTime)
        {
            string strResult = string.Empty;
            DateTime dateStart = DateTime.ParseExact(startTime.ToString(), "HHmmss", null);
            DateTime dateEnd = DateTime.ParseExact(endTime.ToString(), "HHmmss", null);
            if (dateEnd > dateStart)
            {
                TimeSpan tmp = dateEnd.Subtract(dateStart);
                strResult = tmp.Hours + "時間" + tmp.Minutes + "分";
            }
            return strResult;
        }
        #endregion

        #region Calculate Time Remain
        /// <summary>
        /// Calculate time remain, show date, time or minute belong to datetime now. 
        /// </summary>
        /// <param name="gameDate">Game Date.</param>
        /// <param name="gameTime">Game Time.</param>
        /// <returns>String time formatted.</returns>
        public static string CalculateTimeRemainDisplayString(int gameDate, string gameTime)
        {
            //Parse string time to datetime.
            DateTime gTime = DateTime.ParseExact(gameTime.PadLeft(4, '0'), "HHmm", null);
            //Parse int date to datetime.
            DateTime gDate = DateTime.ParseExact(gameDate.ToString(), "yyyyMMdd", null);

            DateTime gDateTime = new DateTime(gDate.Year, gDate.Month, gDate.Day, gTime.Hour, gTime.Minute, gTime.Second);
         
            // 受付可能は試合開始5分前まで
            gDateTime = gDateTime.AddMinutes(-5);

            int intNowDate = Convert.ToInt32(DateTime.Now.ToShortDateString().Replace("/", ""));
            TimeSpan timeSpanRemain = gDateTime.Subtract(DateTime.Now);
            string strTimeRemain = string.Empty;
            
            if (intNowDate < gameDate)
            {
                strTimeRemain = " " + Utils.ParseToJapanDate(false, gameDate.ToString());
            }
            else if (intNowDate == gameDate)
            {
                if (timeSpanRemain.Hours >= 1)
                {
                    strTimeRemain = "あと" + timeSpanRemain.ToString(@"h\時\間m\分");
                }
                else if (timeSpanRemain.Hours == 0)
                {
                    strTimeRemain = "あと" + timeSpanRemain.ToString(@"m\分");
                }
            }
            return strTimeRemain;
        }

        /// <summary>
        /// Calculate time remain, show date, time or minute belong to datetime now. 
        /// </summary>
        /// <param name="gameDate">Game Date.</param>
        /// <param name="gameTime">Game Time.</param>
        /// <returns>
        /// 1:24時間以上前
        /// 2:1時間以上
        /// 3:1時間以内
        /// </returns>
        public static int CalculateTimeRemain(int gameDate, string gameTime)
        {
            //Parse string time to datetime.
            DateTime gTime = DateTime.ParseExact(gameTime.PadLeft(4, '0'), "HHmm", null);
            //Parse int date to datetime.
            DateTime gDate = DateTime.ParseExact(gameDate.ToString(), "yyyyMMdd", null);
        
            DateTime gDateTime = new DateTime(gDate.Year, gDate.Month, gDate.Day, gTime.Hour, gTime.Minute, gTime.Second);

            // 受付可能は試合開始5分前まで
            gDateTime = gDateTime.AddMinutes(-5);

            int intNowDate = Convert.ToInt32(DateTime.Now.ToShortDateString().Replace("/", ""));
            TimeSpan timeSpanRemain = gDateTime.Subtract(DateTime.Now);

            // todo 戻り値はenumにしたい
            
            if (intNowDate < gameDate)
            {
                return 1;
            }

            if (intNowDate == gameDate)
            {
                if (timeSpanRemain.Hours >= 1)
                {
                    return 2;
                }

                if (timeSpanRemain.Hours == 0)
                {
                    return 3;
                }
            }

            return 0;
        }

        #endregion

        #region Total Weeks In Month
        /// <summary>
        /// Calculate total weeks in a month.
        /// </summary>
        /// <param name="date">Now Date.</param>
        /// <returns>Total week.s</returns>
        public static int TotalWeeksInMonth(int year, int month)
        {
            //extract the month
            //int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            //DateTime firstOfMonth = new DateTime(date.Year, date.Month, 1);
            ////days of week starts by default as Sunday = 0
            //int firstDayOfMonth = (int)firstOfMonth.DayOfWeek;
            //int weeksInMonth = (int)Math.Ceiling((firstDayOfMonth + daysInMonth) / 7.0);
            //return weeksInMonth;
            int totalMondays = 0;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                var day = new DateTime(year, month, i);
                if (day.DayOfWeek == DayOfWeek.Monday)
                {
                    totalMondays++;
                }
            }
            return totalMondays;
        }
        #endregion
        #region Get Week In Month
        /// <summary>
        /// Get week of date in month.
        /// </summary>
        /// <param name="date">Date need define week.</param>
        /// <returns>Week number.</returns>
        public static int GetWeekInMonth(DateTime date)
        {
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            int firstDay = (int)firstDayOfMonth.DayOfWeek;
            if (firstDay == 0)
            {
                firstDay = 7;
            }
            double d = (firstDay + date.Day - 1) / 7.0;
            return (int)Math.Ceiling(d);
        }
        #endregion
        #region Get Point Of Week In Month
        /// <summary>
        /// Get Point Of week of date in month.
        /// </summary>
        /// <param name="startDate">ポイントベースの月初の日付（月曜開始）</param>
        /// <param name="date">Date need define week.</param>
        /// <returns>Week number.</returns>
        public static int GetPointOfWeekInMonth(DateTime startDate, DateTime date)
        {
            // 対象年月を保護するためにstartDateの年月を取得
            int defYear = startDate.Year;
            int defMonth = startDate.Month;
            // 計算用に月初の日付取得(ポイントベース)
            int defDay = startDate.Day;
            // 対象日付の年月の取得
            int month = date.Month;

            // 何週目か確認する日付を取得
            DateTime targetDate = date;
            // 月末の日付を取得
            DateTime targetMonthLastdate = Convert.ToDateTime(defYear + "/" + defMonth + "/01 23:59:59").AddMonths(1).AddDays(-1);
            // 翌月にさしかかってた場合
            if (month != defMonth)
            {
                // 月末の日付をセット
                targetDate = targetMonthLastdate;
            }

            double d = (date.Day - (defDay - 1)) / 7.0;
            return (int)Math.Ceiling(d);
        }
        #endregion

        #region Format GameSituation With Inning
        /// <summary>
        /// Format Game Situation With Inning and bottom top.
        /// </summary>
        /// <param name="gameInfo">Game info</param>
        /// <returns>String with game status.</returns>
        public static string FormatGameSituationWithInning(GameInfoViewModel gameInfo)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(gameInfo.GameSituationID))
            {
                if (gameInfo.Inning == 0 || gameInfo.Inning == null)
                {
                    result = "開始";
                }
                else if (gameInfo.Inning >= 1)
                {
                    result = gameInfo.Inning + Constants.STRING_ROUND + gameInfo.BottomTop;
                }
            }
            else if (gameInfo.GameSituationID == "2")
            {
                result = "終了";
            }
            else if (gameInfo.GameSituationID == "4" || gameInfo.GameSituationID == "5" || gameInfo.GameSituationID == "A" ||
                gameInfo.GameSituationID == "B" || gameInfo.GameSituationID == "C")
            {
                result = "コールド";
            }
            else if (gameInfo.GameSituationID == "8")
            {
                result = "没収試合";
            }
            else
            {
                result = "試合中止";
            }
            return result;
        }
        #endregion

        #region Get All User Prediction
        /// <summary>
        /// Get all user that predicted for this game.
        /// </summary>
        /// <param name="com">Com context.</param>
        /// <param name="gameID">GameID.</param>
        /// <param name="sportID">SportID.</param>
        /// <param name="fixBetSelectID">FixBetSelectID : Team is selected.</param>
        /// <returns>List user bet for this team of this game.</returns>
        public static List<Member> GetAllUserPrediction(ComEntities com, int gameID, int classClass, int sportID, int fixBetSelectID)
        {
            List<Member> result = (from ep in com.ExpectPoint
                                   join et in com.ExpectTarget on ep.ExpectTargetID equals et.ExpectTargetID
                                   join m in com.Member on ep.CreatedAccountID equals m.MemberId.ToString()
                                   where et.UniqueID == gameID
                                   && et.UniqueID == (int)gameID
                                   && et.ClassClass == classClass  //分類種別
                                   && et.SportsID == sportID
                                   && ep.BetSelectID == fixBetSelectID  //BET選択肢ID
                                   select m).ToList();
            return result;
        }
        #endregion

        #region Format Profile Image Location
        /// <summary>
        /// Format Profile Image Location
        /// </summary>
        /// <param name="urlImage">Link image need format.</param>
        /// <returns>Link image that formatted.</returns>
        public static string FormatProfileImageLocation(string urlImage)
        {
            string strLocation = string.Empty;
            if (urlImage.Contains("~") && urlImage.IndexOf("~") == 0)
            {
                strLocation = urlImage.Replace("~", "");
            }
            else
            {
                if(urlImage.ToLower().Contains("http"))
                {
                    strLocation = urlImage;
                }
                else
                {
                    if (urlImage.IndexOf("/") != 0)
                    {
                        strLocation = string.Concat("/", urlImage);
                    }
                    else
                    {
                        strLocation = urlImage;
                    }
                }               
            }
            return strLocation;
        }
        #endregion

        #endregion

        // Sy Huynh
        #region Get game status based on Matchday, GameSetSituationCD, Inning
        // Orther version of Splg.Areas.Npb.Models.NpbCommon.GetStatusMatch(gameID)
        public static int GetGameStatus(int? matchday, string gameSetSituationCD, int? inning)
        {
            if (matchday == null)
                return -1;
            DateTime matchDate = Utils.ConvertStrToDatetime(matchday.ToString());
            if (DateTime.Now < matchDate)
            {
                return 0;
            }
            else if (DateTime.Now >= matchDate)
            {
                if (string.IsNullOrEmpty(gameSetSituationCD))
                {
                    if (inning != null)
                    {
                        if (inning.Value > 0)
                        {
                            return 1;
                        }
                        else if (inning.Value == 0)
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 2;
                }
            }
            return 0;
        }

        #endregion


        #region Get Status By GameID
        /// <summary>
        /// Get status of game.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>Game status</returns>
        public static int GetStatusByGameID(int sportID, int gameID, long memberID)
        {
            //Must test time before game start 5 minutes.
            int gameStatus = 0;
            if (gameID != null && sportID != null)
            {
                if (sportID == Constants.NPB_SPORT_ID)
                {
                    gameStatus = NpbCommon.GetStatusMatch(gameID, memberID.ToString());
                }
                else if (sportID == Constants.MLB_SPORT_ID)
                {
                    gameStatus = MlbCommon.GetStatusMatch(gameID, memberID.ToString());
                }
                else if (sportID == Constants.JLG_SPORT_ID)
                {
                    gameStatus = JlgCommon.GetStatusMatch(gameID, memberID.ToString());
                }
            }
            return gameStatus;
        }
        #endregion

        #region Get some words from string
        /// <summary>
        /// Get some words from string, put [...] at the end if string longer than 100 words.
        /// </summary>
        /// <param name="strInput">String Input.</param>
        /// <param name="wordsNumber">Number of word needed.</param>
        /// <returns>string that has been shorten.</returns>
        public static string ShortenString(string strInput, int wordsNumber)
        {
            string result = string.Empty;
            if (strInput.Count() <= wordsNumber)
            {
                result = strInput;
            }
            else
            {
                result = strInput.Substring(0, wordsNumber);
                result = string.Format("{0}...", result);
            }
            return result;
        }
        #endregion

        #region Parse a date-time To JapanTime in week day
        /// <summary>
        /// 
        /// </summary>
        /// <param name="needParse">Time need parse.</param>
        /// <returns>week day.</returns>
        public static string GetWeekDayToJapanTime(DateTime needParse)
        {
            string result = string.Empty;
            if (needParse != null)
            {
                GlobalizationSection globalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
                var cultureInfo = new System.Globalization.CultureInfo(globalizationSection.Culture.ToString());
                var dateTimeInfo = cultureInfo.DateTimeFormat;
                var dayNames = dateTimeInfo.DayNames;

                var dayOfWeek = dayNames[(int)needParse.Date.DayOfWeek];
                result = dayOfWeek.Substring(0, 1);
            }
            return result;
        }
        #endregion

        #region Parse a date-time To JapanTime by format "MM/dd (DayofWeek)"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="needParse">Time need parse.</param>
        /// <returns>week day.</returns>
        public static string GetDateMMddDayofWeekToJapanTime(DateTime needParse)
        {
            string result = string.Empty;
            if (needParse != null)
            {
                GlobalizationSection globalizationSection = WebConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
                var cultureInfo = new System.Globalization.CultureInfo(globalizationSection.Culture.ToString());
                var dateTimeInfo = cultureInfo.DateTimeFormat;
                var dayNames = dateTimeInfo.DayNames;
                var dayOfWeek = dayNames[(int)needParse.Date.DayOfWeek];
                result = needParse.Date.Month + "/" + needParse.Date.Day + " (" + dayOfWeek.Substring(0, 1) + ")";
            }
            return result;
        }
        #endregion

        #region Get Title, Description and Keywords to string array from
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>string array</returns>
        public static string[] GetNpbTDK(string url, string pageNo)
        {
            string mainUrl = url.Split('?').FirstOrDefault();
            string[] subUrls = mainUrl.Split('/');
            List<string> subUrlList = new List<string>();
            for (int i = 0; i < subUrls.Count(); i++)
            {
                if (!string.IsNullOrEmpty(subUrls[i]))
                {
                    subUrlList.Add(subUrls[i]);
                }
            }
            //var url = Context.Request.Path;
            PageInfo page = null;

            ComEntities tkd = new ComEntities();
            page = tkd.PageInfo.Find(pageNo);
            string title = string.Empty;
            string description = string.Empty;
            string keyWords = string.Empty;
            int userId, groupId;

            if (page != null)
            {
                title = page.Title;
                description = page.Description;
                keyWords = page.Keywords;
            }
            else
            {
                return new string[4];
            }
            int teamCD, playerCD, gameID;
            switch (pageNo)
            {
                case "8-1":
                case "8-2":
                case "8-3":
                case "8-4":
                case "8-5":
                case "8-7":
                    title = page.Title;
                    description = page.Description;
                    keyWords = page.Keywords;
                    break;
                case "8-5-1":
                case "8-5-2":
                case "8-5-3":
                case "8-5-4":
                case "8-5-5":
                case "8-5-6":
                case "8-5-7":
                    if (!string.IsNullOrEmpty(subUrls[3]) && int.TryParse(subUrls[3], out teamCD))
                    {
                        string teamName = GetTeamNameById(Constants.NPB_SPORT_ID, teamCD);
                        title = page.Title.Replace("(チーム名)", teamName);
                        description = page.Description.Replace("(チーム名)", teamName);
                        keyWords = page.Keywords.Replace("(チーム名)", teamName);
                    }

                    break;
                case "8-5-8":
                case "8-5-9":
                    if (!string.IsNullOrEmpty(subUrls[3]) && !string.IsNullOrEmpty(subUrls[4]) && int.TryParse(subUrls[3], out teamCD) && int.TryParse(subUrls[4], out playerCD))
                    {
                        NpbEntities npb = new NpbEntities();
                        var teamName = (from t in npb.TeamInfoMST
                                        where t.TeamCD == teamCD
                                        select t).FirstOrDefault();
                        var playerName = (from t in npb.PlayerInfoMST
                                          where t.PlayerCD == playerCD
                                          select t).FirstOrDefault();
                        title = page.Title.Replace("(チーム名略称)", (teamName != null) ? teamName.ShortNameTeam : string.Empty).Replace("(選手名)", (playerName != null) ? playerName.Player : string.Empty);
                        description = page.Description.Replace("(チーム名)", (teamName != null) ? teamName.Team : string.Empty).Replace("(選手名)", (playerName != null) ? playerName.Player : string.Empty);
                        keyWords = page.Keywords.Replace("(チーム名)", (teamName != null) ? teamName.Team : string.Empty).Replace("(選手名)", (playerName != null) ? playerName.Player : string.Empty);
                    }
                    break;
                case "8-6":
                case "8-6-1":
                case "8-6-2":
                    if (!string.IsNullOrEmpty(subUrls[3]) && int.TryParse(subUrls[3], out gameID))
                    {
                        NpbEntities npb = new NpbEntities();
                        var gameNames = (from g in npb.GameInfoSS
                                         where g.ID == gameID
                                         select g).FirstOrDefault();
                        if (gameNames != null)
                        {
                            DateTime gd = DateTime.ParseExact(string.Format("{0}", gameNames.GameDate), "yyyyMMdd", null);
                            string gameName = string.Format("{0} {1} vs {2}", gd.ToString("MM/dd"), gameNames.HomeTeamNameS, gameNames.VisitorTeamNameS);
                            title = page.Title.Replace("(YY/MM 対戦カード名)", gameName);
                            description = page.Description.Replace("(YY/MM 対戦カード名)", gameName);
                            keyWords = page.Keywords.Replace("(チーム名1)", gameNames.HomeTeamName).Replace("(チーム名2)", gameNames.VisitorTeamName);
                        }
                    }
                    break;
                case "7-1":
                    long newsItemId;
                    if (!string.IsNullOrEmpty(subUrls[3]) && long.TryParse(subUrls[3], out newsItemId))
                    {
                        ComEntities news = new ComEntities();
                        var newsDetail = (from n in news.BriefNews
                                          where n.NewsItemID == newsItemId && n.Status == Constants.NEWS_VALID_STATUS && n.CarryLimitDate >= DateTime.Now
                                          select n).FirstOrDefault();
                        if (newsDetail != null)
                        {
                            title = string.Format("{0} | スポログ", ShortenString(newsDetail.Headline, 25));
                            description = ShortenString(newsDetail.newstext, 100);
                            keyWords = subUrls[1];
                        }

                    }
                    break;
                default:
                    title = page.Title;
                    description = page.Description;
                    keyWords = page.Keywords;
                    break;
            }
            string robots = string.Empty;
            if (page.CanAutomatic != null)
            {
                if (page.CanAutomatic == 1)
                {
                    robots = "none";
                }
            }
            string[] result = new string[4];
            result[0] = title;
            result[1] = description;
            result[2] = keyWords;
            result[3] = robots;
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>string array</returns>
        public static string[] GetMlbTDK(string url, string pageNo)
        {
            string mainUrl = url.Split('?').FirstOrDefault();
            string[] subUrls = mainUrl.Split('/');
            List<string> subUrlList = new List<string>();
            for (int i = 0; i < subUrls.Count(); i++)
            {
                if (!string.IsNullOrEmpty(subUrls[i]))
                {
                    subUrlList.Add(subUrls[i]);
                }
            }
            PageInfo page = null;

            ComEntities tkd = new ComEntities();
            page = tkd.PageInfo.Find(pageNo);
            string title = string.Empty;
            string description = string.Empty;
            string keyWords = string.Empty;
            int userId, groupId;

            if (page != null)
            {
                title = page.Title;
                description = page.Description;
                keyWords = page.Keywords;
            }
            int teamCD, playerCD, gameID;
            //TODO Mlbに合わせて実装
            switch (pageNo)
            {
                case "9-5-1":
                case "9-5-2":
                case "9-5-6":
                    if (!string.IsNullOrEmpty(subUrls[3]) && int.TryParse(subUrls[3], out teamCD))
                    {
                        string teamName = GetTeamNameById(Constants.MLB_SPORT_ID, teamCD);
                        title = page.Title.Replace("(チーム名)", teamName);
                        description = page.Description.Replace("(チーム名)", teamName);
                        keyWords = page.Keywords.Replace("(チーム名)", teamName);
                    }

                    break;
                case "9-7":
                    if (!string.IsNullOrEmpty(subUrls[3]) && int.TryParse(subUrls[3], out gameID))
                    {
                        MlbEntities mlb = new MlbEntities();
                        var gameNames = (from g in mlb.SeasonSchedule
                                         join d in mlb.DayGroup on g.DayGroupId equals d.DayGroupId
                                         where g.GameID == gameID
                                         select g).FirstOrDefault();
                        var gameDate = (from g in mlb.SeasonSchedule
                                        join d in mlb.DayGroup on g.DayGroupId equals d.DayGroupId
                                        where g.GameID == gameID
                                        select d).FirstOrDefault();
                        if (gameNames != null && gameDate != null)
                        {
                            DateTime gd = DateTime.ParseExact(string.Format("{0}", gameDate.GameDateJPN), "yyyyMMdd", null);
                            string gameName = string.Format("{0} {1} vs {2}", gd.ToString("MM/dd"), gameNames.HomeTeamName, gameNames.VisitorTeamName);
                            title = page.Title.Replace("(YY/MM 対戦カード名)", gameName);
                            description = page.Description.Replace("(YY/MM 対戦カード名)", gameName);
                            keyWords = page.Keywords.Replace("(チーム名1)", gameNames.HomeTeamName).Replace("(チーム名2)", gameNames.VisitorTeamName);
                        }
                    }
                    break;
                case "7-1":
                    long newsItemId;
                    if (!string.IsNullOrEmpty(subUrls[3]) && long.TryParse(subUrls[3], out newsItemId))
                    {
                        ComEntities news = new ComEntities();
                        var newsDetail = (from n in news.BriefNews
                                          where n.NewsItemID == newsItemId && n.Status == Constants.NEWS_VALID_STATUS && n.CarryLimitDate >= DateTime.Now
                                          select n).FirstOrDefault();
                        if (newsDetail != null)
                        {
                            title = string.Format("{0} | スポログ", ShortenString(newsDetail.Headline, 25));
                            description = ShortenString(newsDetail.newstext, 100);
                            keyWords = subUrls[1];
                        }

                    }
                    break;
                default:
                    title = page.Title;
                    description = page.Description;
                    keyWords = page.Keywords;
                    break;
            }
            string robots = string.Empty;
            if (page.CanAutomatic != null)
            {
                if (page.CanAutomatic == 1)
                {
                    robots = "none";
                }
            }
            string[] result = new string[4];
            result[0] = title;
            result[1] = description;
            result[2] = keyWords;
            result[3] = robots;
            return result;
        }

        /// <summary>
        /// Title, description, keywords for JLeague
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>string array</returns>
        public static string[] GetJLeagueTDK(string url, string pageNo = "")
        {
            string mainUrl = url.Split('?').FirstOrDefault();
            string[] subUrls = mainUrl.Split('/');
            List<string> subUrlList = new List<string>();
            for (int i = 0; i < subUrls.Count(); i++)
            {
                if (!string.IsNullOrEmpty(subUrls[i]))
                {
                    subUrlList.Add(subUrls[i]);
                }
            }
            PageInfo page = null;

            ComEntities tkd = new ComEntities();
            page = tkd.PageInfo.Find(pageNo);
            string title = string.Empty;
            string description = string.Empty;
            string keyWords = string.Empty;
            int userId, groupId;

            if (page != null)
            {
                title = page.Title;
                description = page.Description;
                keyWords = page.Keywords;
            }
            int teamCD, playerCD, gameID;
            switch (pageNo)
            {
                case "10-2-5-1":
                case "10-2-5-2":
                case "10-2-5-3":
                case "10-2-5-4":
                case "10-2-5-5":
                case "10-3-5-1":
                case "10-3-5-2":
                case "10-3-5-3":
                case "10-3-5-4":
                case "10-3-5-5":
                    if (!string.IsNullOrEmpty(subUrlList.ElementAt(3)) && int.TryParse(subUrlList.ElementAt(3), out teamCD))
                    {
                        string teamName = GetTeamNameById(Constants.JLG_SPORT_ID, teamCD);
                        title = page.Title.Replace("(チーム名)", teamName);
                        description = page.Description.Replace("(チーム名)", teamName);
                        keyWords = page.Keywords.Replace("(チーム名)", teamName);
                    }
                    break;
                case "10-2-5-6":
                case "10-3-5-6":
                    int playerIdx = 0;
                    for (int i = 0; i < subUrlList.Count(); i++)
                    {
                        if (subUrlList.ElementAt(i).ToLower().Equals("players"))
                            playerIdx = i;
                    }
                    if (subUrlList.Count() > playerIdx + 1)
                    {
                        if (!string.IsNullOrEmpty(subUrlList.ElementAt(playerIdx + 1)) && int.TryParse(subUrlList.ElementAt(playerIdx + 1), out playerCD))
                        {
                            JlgEntities jlg = new JlgEntities();
                            var playerNames = (from p in jlg.PlayerInfoDI
                                               where p.PlayerID == playerCD
                                               select p).FirstOrDefault();
                            var teamNames = (from d in jlg.DirectoryDI
                                             join p in jlg.PlayerInfoDI on d.DirectoryDIId equals p.DirectoryDIId
                                             where p.PlayerID == playerCD
                                             select d).FirstOrDefault();
                            if (playerNames != null && teamNames != null)
                            {
                                // チーム名
                                string nameReplaced = string.Empty;
                                nameReplaced = teamNames.TeamName;
                                title = page.Title.Replace("(チーム名)", nameReplaced);
                                description = page.Description.Replace("(チーム名)", nameReplaced);
                                keyWords = page.Keywords.Replace("(チーム名)", nameReplaced);
                                // 選手名
                                nameReplaced = playerNames.PlayerName;
                                title = title.Replace("(選手名)", nameReplaced);
                                description = description.Replace("(選手名)", nameReplaced);
                                keyWords = keyWords.Replace("(選手名)", nameReplaced);
                            }
                        }
                    }
                    break;
                case "10-5":
                    if (!string.IsNullOrEmpty(subUrls[3]) && int.TryParse(subUrls[3], out gameID))
                    {
                        JlgEntities jlg = new JlgEntities();
                        var games = (from g in jlg.ScheduleInfo
                                     join gc in jlg.GameCategory on g.GameCategoryId equals gc.GameCategoryId
                                     join gs in jlg.GameSchedule on gc.GameScheduleId equals gs.GameScheduleId
                                     where (gs.GameKindID == JlgConstants.JLG_GAMEKIND_J1
                                     || gs.GameKindID == JlgConstants.JLG_GAMEKIND_J2
                                     || gs.GameKindID == JlgConstants.JLG_GAMEKIND_NABISCO)
                                     && g.GameID == gameID
                                     select new
                                     {
                                         GameKindID = gs.GameKindID,
                                         GameInfo = g
                                     } into game_info
                                     select game_info).FirstOrDefault();
                        if (games != null)
                        {
                            var occasionReplaced = string.Format("第{0}節", games.GameInfo.OccasionNo);
                            var firstReplaced = string.Empty;
                            switch (games.GameKindID)
                            {
                                case JlgConstants.JLG_GAMEKIND_J1:
                                    firstReplaced = "J1" + occasionReplaced;
                                    break;
                                case JlgConstants.JLG_GAMEKIND_J2:
                                    firstReplaced = "J2" + occasionReplaced;
                                    break;
                                case JlgConstants.JLG_GAMEKIND_NABISCO:
                                    DateTime gd = DateTime.ParseExact(string.Format("{0}", games.GameInfo.GameDate), "yyyyMMdd", null);
                                    firstReplaced = string.Format("ナビスコカップ{0}", gd.ToString("MM/dd"));
                                    break;
                            }
                            var secondReplaced = string.Format("{0} vs {1}", games.GameInfo.HomeTeamNameS, games.GameInfo.AwayTeamNameS);
                            title = page.Title.Replace("(J1第○節／J2第○節／ナビスコカップYY/MM)", firstReplaced).Replace("(対戦カード名)", secondReplaced);
                            title = page.Title.Replace("(J1第○節／J2第○節／ナビスコカップMM/DD)", firstReplaced).Replace("(対戦カード名)", secondReplaced);
                            description = page.Description.Replace("(J1第○節／J2第○節／ナビスコカップYY/MM)", firstReplaced).Replace("(対戦カード名)", secondReplaced);
                            description = page.Description.Replace("(J1第○節／J2第○節／ナビスコカップMM/DD)", firstReplaced).Replace("(対戦カード名)", secondReplaced);
                            keyWords = page.Keywords.Replace("(第○節)", occasionReplaced).Replace("(チーム名1)", games.GameInfo.HomeTeamName).Replace("(チーム名2)", games.GameInfo.AwayTeamName);
                        }
                    }
                    break;
                default:
                    title = page.Title;
                    description = page.Description;
                    keyWords = page.Keywords;
                    break;
            }

            string robots = string.Empty;
            if (page.CanAutomatic != null)
            {
                if (page.CanAutomatic == 1)
                {
                    robots = "none";
                }
            }
            string[] result = new string[4];
            result[0] = title;
            result[1] = description;
            result[2] = keyWords;
            result[3] = robots;
            return result;
        }

        /// <summary>
        /// Title, description, keywords for 1-1, 1-2, 2-xxx, 5-xxx, 7-1
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>string array</returns>
        public static string[] GetHomeTDK(string url, string pageNO = "")
        {
            string mainUrl = url.Split('?').FirstOrDefault();
            string[] subUrls = mainUrl.Split('/');
            List<string> subUrlList = new List<string>();
            for (int i = 0; i < subUrls.Count(); i++)
            {
                if (!string.IsNullOrEmpty(subUrls[i]))
                {
                    subUrlList.Add(subUrls[i]);
                }
            }
            string pageNo = "";
            int subUrlCount = subUrlList.Count();
            if (!string.IsNullOrEmpty(pageNO))
            {
                pageNo = pageNO;
            }
            else
            {
                if (subUrlCount == 0)
                    pageNo = "1-1";
                else
                {
                    switch (subUrlList.ElementAt(0).ToLower())
                    {
                        case "search":
                            pageNo = "1-2";
                            break;
                        case "login":
                            pageNo = "2-2";
                            break;
                        case "logout":
                            pageNo = "2-3";
                            if (subUrlCount > 1)
                                pageNo = "2-3-1";
                            break;
                        case "delete":
                            pageNo = "2-4";
                            if (subUrlCount > 1)
                                pageNo = "2-4-1";
                            break;
                        case "user_article":
                            if (subUrlCount == 1)
                                pageNo = "5-1";
                            else
                            {
                                switch (subUrlList.ElementAt(1).ToLower())
                                {
                                    case "new":
                                        if (subUrlCount >= 4)
                                            pageNo = "5-4";
                                        break;
                                    case "edit":
                                        pageNo = "5-4-3";
                                        break;
                                    default:
                                        if (subUrlCount == 3)
                                        {
                                            if (subUrlList.ElementAt(2).ToLower().Equals("sport") || subUrlList.ElementAt(2).ToLower().Equals("topic"))
                                                pageNo = "5-2";
                                            else
                                                pageNo = "5-3";
                                        }
                                        break;
                                }
                            }
                            break;
                        case "mypage":
                            if (subUrlCount == 1)
                            {
                                pageNo = "3-1";
                            }
                            else
                            {
                                switch (subUrlList.ElementAt(1).ToLower())
                                {
                                    case "expectedlist":
                                        pageNo = "3-2";
                                        break;
                                    case "article":
                                        pageNo = "3-3";
                                        break;
                                    case "group":
                                        if (subUrlCount == 2)
                                            pageNo = "3-4";
                                        else
                                        {
                                            if (subUrlCount == 3)
                                            {
                                                if (subUrlList.ElementAt(2).ToLower().Equals("new"))
                                                    pageNo = "3-4-2";
                                                else
                                                    pageNo = "3-4-1";
                                            }
                                            else
                                            {
                                                if (subUrlList.ElementAt(3).ToLower().Equals("edit"))
                                                    pageNo = "3-4-3";
                                            }
                                        }
                                        break;
                                    case "following":
                                        pageNo = "3-5";
                                        break;
                                    case "followers":
                                        pageNo = "3-6";
                                        break;
                                    case "setting":
                                        if (subUrlCount == 2)
                                            pageNo = "3-7";
                                        else
                                        {
                                            if (subUrlList.ElementAt(2).ToLower().Equals("address"))
                                                pageNo = "3-7-1";
                                            else if (subUrlList.ElementAt(2).ToLower().Equals("address"))
                                                pageNo = "3-7-2";
                                        }
                                        break;
                                    case "notice":
                                        pageNo = "3-8";
                                        break;
                                }
                            }
                            break;
                        case "user":
                            if (subUrlCount == 2)
                            {
                                pageNo = "4-1";
                            }
                            else if (subUrlCount > 2)
                            {
                                switch (subUrlList.ElementAt(2).ToLower())
                                {
                                    case "expectedlist":
                                        pageNo = "4-2";
                                        break;
                                    case "article":
                                        pageNo = "4-3";
                                        break;
                                    case "following":
                                        pageNo = "4-4";
                                        break;
                                    case "followers":
                                        pageNo = "4-5";
                                        break;
                                }
                            }
                            break;
                        case "user_search":
                            pageNo = "3-9";
                            break;
                        case "prize":
                            if (subUrlCount == 1)
                            {
                                pageNo = "6-1";
                            }
                            else
                            {
                                if (subUrlCount == 2)
                                {
                                    if (subUrlList.ElementAt(1).ToLower().Equals("ranking"))
                                    {
                                        pageNo = "6-4";
                                    }
                                    else
                                    {
                                        pageNo = "6-3";
                                    }
                                }
                                else if (subUrlCount == 3)
                                {
                                    pageNo = "6-2";
                                }
                            }
                            break;
                        case "support":
                            if (subUrlCount > 1)
                            {
                                switch (subUrlList.ElementAt(1).ToLower())
                                {
                                    case "faq":
                                        pageNo = "12-1";
                                        break;
                                    case "rules":
                                        pageNo = "12-2";
                                        break;
                                    case "contact":
                                        pageNo = "12-3";
                                        break;
                                    case "howto":
                                        pageNo = "12-4";
                                        break;
                                }
                            }
                            break;
                        default:
                            //pageNo = "1-1";
                            //if (subUrlCount > 2 && subUrlList.ElementAt(1).ToLower().Equals("news"))
                            //    pageNo = "7-1";
                            break;
                    }
                }
            }
            if(string.IsNullOrEmpty(pageNo))
                return (new string[4]);
            ComEntities tkd = new ComEntities();
            var page = tkd.PageInfo.Find(pageNo);
            if (page == null)
                return (new string[4]);
            string title = string.Empty;
            string description = string.Empty;
            string keyWords = string.Empty;
            string robots = string.Empty;
            long newsclassID;
            switch (pageNo)
            {
                case "7-1":
                    long newsItemId;
                    if (!string.IsNullOrEmpty(subUrls[3]) && long.TryParse(subUrls[3], out newsItemId))
                    {
                        ComEntities news = new ComEntities();
                        var newsDetail = (from n in news.BriefNews
                                          where n.NewsItemID == newsItemId && n.Status == Constants.NEWS_VALID_STATUS && n.CarryLimitDate >= DateTime.Now
                                          select n).FirstOrDefault();
                        if (newsDetail != null)
                        {
                            title = string.Format("{0} | スポログ", ShortenString(string.Format("{0} {1}", newsDetail.Headline, newsDetail.SubHeadline), 25));
                            description = ShortenString(newsDetail.newstext, 100);
                            keyWords = subUrls[1];
                        }
                    }
                    break;
                case "5-2":
                    if (!string.IsNullOrEmpty(subUrls[2]) && long.TryParse(subUrls[2], out newsclassID))
                    {
                        string topicName = string.Empty;
                        if (subUrls.Count() > 3 && (subUrls[3].ToLower().Equals("sport")))
                        {
                            topicName = GetSportNameById(Convert.ToInt32(newsclassID));
                        }
                        else
                        {
                            ComEntities news = new ComEntities();
                            var newsClass = (from n in news.TopicMaster
                                             where n.TopicID == newsclassID
                                             select n).FirstOrDefault();
                            if (newsClass != null)
                            {
                                topicName = newsClass.TopicName;
                            }
                        }
                        title = page.Title.Replace("(トピック名)", topicName);
                        description = page.Description.Replace("(トピック名)", topicName);
                        keyWords = page.Keywords.Replace("(トピック名)", topicName);
                    }
                    break;
                case "5-3":
                    if (!string.IsNullOrEmpty(subUrls[2]) && long.TryParse(subUrls[2], out newsclassID))
                    {
                        ComEntities news = new ComEntities();
                        var topic = (from n in news.TopicMaster
                                     where n.TopicID == newsclassID
                                     select n).FirstOrDefault();
                        if (topic != null)
                        {
                            long contributedID;
                            if (!string.IsNullOrEmpty(subUrls[3]) && long.TryParse(subUrls[3], out contributedID))
                            {
                                var contribute = (from n in news.Contribution
                                                  where n.ContributeId == contributedID
                                                  select n).FirstOrDefault();
                                if (contribute != null)
                                {
                                    title = string.Format("{0} | スポログ", ShortenString(contribute.Title, 25));
                                    description = ShortenString(contribute.Body, 100);
                                    keyWords = page.Keywords.Replace("(トピック名)", topic.TopicName);

                                    if (page.CanAutomatic != null)
                                        if (page.CanAutomatic == 2 && contribute.Body.Count() < 100)
                                            robots = "none";
                                }
                            }
                        }
                    }
                    break;
                case "12-2":
                    title = page.Title;
                    description = ShortenString("スポログ利用規約（以下「本規約」といいます。）は、株式会社メディアーノ（以下「当社」といいます）が提供する「スポログ」情報提供サービス（以下「本サービス」といいます。）を利用するに当たり遵守しなければならない事項を定めたものであり、本サービスを利用する者（以下「利用者」といいます。）の全てに適用されるものとします。", 100);
                    keyWords = page.Keywords;
                    break;
                default:
                    title = page.Title;
                    description = page.Description;
                    keyWords = page.Keywords;
                    break;
            }

            if (page.CanAutomatic != null)
            {
                if (page.CanAutomatic == 1)
                {
                    robots = "none";
                }
            }
            string[] result = new string[4];
            result[0] = title;
            result[1] = description;
            result[2] = keyWords;
            result[3] = robots;
            return result;
        }


        /// <summary>
        /// MyPageのTDKを取得する
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>string array</returns>
        public static string[] GetMyPageTDK(string url, string pageNo)
        {
            PageInfo page = null;
            string mainUrl = url.Split('?').FirstOrDefault();
            string[] subUrls = mainUrl.Split('/');
            ComEntities tkd = new ComEntities();
            page = tkd.PageInfo.Find(pageNo);
            string title = string.Empty;
            string description = string.Empty;
            string keyWords = string.Empty;
            long userId, groupId;

            if (page != null)
            {
                title = page.Title;
                description = page.Description;
                keyWords = page.Keywords;
            }

            switch (pageNo)
            {
                case "3-1":
                    break;
                case "3-2":
                    break;
                case "3-3":
                    break;
                case "3-4":
                    break;
                case "3-4-1"://group_id
                    if (!string.IsNullOrEmpty(subUrls[3]) && long.TryParse(subUrls[3], out groupId))
                    {
                        ComEntities com = new ComEntities();
                        var group = (from n in com.Groups
                                     where n.GroupID == groupId
                                     select n).FirstOrDefault();
                        if (group != null)
                        {
                            title = page.Title.Replace("(グループ名)", group.GroupName);
                            //description = page.description.Replace("(グループ名)", group.GroupName);
                            //keyWords = page.keyWords.Replace("(グループ名)", group.GroupName);                         
                        }
                    }
                    break;
                    break;
                case "3-4-2":
                    break;
                case "3-4-3":
                    break;
                case "3-5":
                    break;
                case "3-6":
                    break;
                case "3-7":
                    break;
                case "3-7-1":
                    break;
                case "3-7-2":
                    break;
                case "3-8":
                    break;
                case "3-9":
                    break;
                case "3-10":
                    break;
            }
            string[] result = new string[3];
            result[0] = title;
            result[1] = description;
            result[2] = keyWords;
            return result;
        }

        /// <summary>
        /// UserのTDKを取得する
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>string array</returns>
        public static string[] GetUserTDK(string url, string pageNo, string nickName = null)
        {
            PageInfo page = null;

            ComEntities tkd = new ComEntities();
            page = tkd.PageInfo.Find(pageNo);
            string title = string.Empty;
            string description = string.Empty;
            string keyWords = string.Empty;
            int userId, groupId;

            if (page != null)
            {
                title = page.Title;
                description = page.Description;
                keyWords = page.Keywords;
            }

            if (!String.IsNullOrEmpty(title) && title.Contains("(ユーザー名)"))
            {
                title = title.Replace("(ユーザー名)", nickName);

            }
            switch (pageNo)
            {
                case "4-1":
                    break;
                case "4-2":
                    break;
                case "4-3":
                    break;
                case "4-4":
                    break;
                case "4-5":
                    break;
            }
            string[] result = new string[3];
            result[0] = title;
            result[1] = description;
            result[2] = keyWords;
            return result;
        }

        /// <summary>
        /// PrizeのTDKを取得する
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>string array</returns>
        public static string[] GetPrizeTDK(string url, string pageNo)
        {
            ComEntities tkd = new ComEntities();
            var page = tkd.PageInfo.Find(pageNo);
            string title = string.Empty;
            string description = string.Empty;
            string keyWords = string.Empty;
            int userId, prize_id;
            DateTime date;

            if (page != null)
            {
                title = page.Title;
                description = page.Description;
                keyWords = page.Keywords;
            }

            switch (pageNo)
            {
                case "6-1":
                    break;
                case "6-2"://{YYYYMM}/{prize_id}/
                    break;
                case "6-3"://{YYYYMM}/
                    break;
                case "6-4":
                    break;
            }
            string[] result = new string[3];
            result[0] = title;
            result[1] = description;
            result[2] = keyWords;
            return result;
        }

        #endregion

        #region Get Team Name based on teamID
        /// <summary>
        /// </summary>
        /// <param name="sportID">sport id.</param>
        /// <param name="teamID">team id.</param>
        /// <returns>Name of a team.</returns>
        public static string GetTeamNameById(int sportID, int teamID)
        {
            string result = "［チーム名］";
            if (sportID == Constants.NPB_SPORT_ID)
            {
                NpbEntities npb = new NpbEntities();
                var teamNames = (from t in npb.TeamInfoMST
                                 where t.TeamCD == teamID
                                 select t).FirstOrDefault();
                if (teamNames != null)
                    return teamNames.Team;
            }
            if (sportID == Constants.JLG_SPORT_ID)
            {
                JlgEntities jlg = new JlgEntities();
                var teamNames = (from t in jlg.TeamInfoTE
                                 where t.TeamID == teamID
                                 select t).FirstOrDefault();
                if (teamNames != null)
                    return teamNames.TeamName;
            }
            if (sportID == Constants.MLB_SPORT_ID)
            {
                MlbEntities mlb = new MlbEntities();
                var teamNames = (from t in mlb.TeamInfo
                                 where t.TeamID == teamID
                                 select t).FirstOrDefault();
                if (teamNames != null)
                    return teamNames.TeamName;
            }
            return result;
        }
        #endregion

        #region Get Game vs based on gameID
        /// <summary>
        /// </summary>
        /// <param name="sportID">sport id.</param>
        /// <param name="gameID">game id.</param>
        /// <returns>Name of a team.</returns>
        public static string GetGameNameById(int sportID, int gameID)
        {
            string result = "試合";
            if (sportID == Constants.NPB_SPORT_ID)
            {
                NpbEntities npb = new NpbEntities();
                var gameNames = (from g in npb.GameInfoSS
                                 where g.ID == gameID
                                 select g).FirstOrDefault();
                if (gameNames != null)
                {
                    DateTime gd = DateTime.ParseExact(string.Format("{0}", gameNames.GameDate), "yyyyMMdd", null);
                    result = string.Format("{0} {1} vs {2}", gd.ToString("yyyy/MM/dd"), gameNames.HomeTeamNameS, gameNames.VisitorTeamNameS);
                }
            }
            if (sportID == Constants.JLG_SPORT_ID)
            {
                JlgEntities jlg = new JlgEntities();
                var gameNames = (from g in jlg.ScheduleInfo
                                 where g.GameID == gameID
                                 select g).FirstOrDefault();
                if (gameNames != null)
                {
                    DateTime gd = DateTime.ParseExact(string.Format("{0}", gameNames.GameDate), "yyyyMMdd", null);
                    result = string.Format("{0} {1} vs {2}", gd.ToString("yyyy/MM/dd"), gameNames.HomeTeamNameS, gameNames.AwayTeamNameS);
                }
            }
            if (sportID == Constants.MLB_SPORT_ID)
            {
                MlbEntities mlb = new MlbEntities();
                var gameNames = (from g in mlb.SeasonSchedule
                                 join d in mlb.DayGroup on g.DayGroupId equals d.DayGroupId
                                 where g.GameID == gameID
                                 select new
                                 {
                                     GameName = g,
                                     GameDate = d
                                 }).FirstOrDefault();
                if (gameNames != null)
                {
                    DateTime gd = DateTime.ParseExact(string.Format("{0}", gameNames.GameDate.GameDateJPN), "yyyyMMdd", null);
                    result = string.Format("{0} {1} vs {2}", gd.ToString("MM/dd"), gameNames.GameName.HomeTeamName, gameNames.GameName.VisitorTeamName);
                }
            }
            return result;
        }
        #endregion

        #region Get News Title based on newsItemID
        /// <summary>
        /// </summary>
        /// <param name="newsItemId">news id.</param>
        /// <returns>News Title</returns>
        public static string GetNewsTitleById(long newsItemID)
        {
            string result = "タイトル";
            ComEntities news = new ComEntities();
            var newsTitle = (from br in news.BriefNews
                             where br.NewsItemID == newsItemID
                             select br).FirstOrDefault();
            if (newsTitle != null)
                return newsTitle.Headline;
            return result;
        }
        #endregion

        #region Get SportID based on ItpcSubjectCode
        /// <summary>
        /// </summary>
        /// <param name="itpcSubjectCode">itpcSubjectCode</param>
        /// <returns>sport ID</returns>
        public static int GetSportIDByItpcSubjectCode(int itpcSubjectCode)
        {
            switch (itpcSubjectCode)
            {
                case Constants.NPB_ITPCSUBJECTCODE:
                    return 1;
                case Constants.JLEAGUE_ITPCSUBJECTCODE:
                    return 2;
                case Constants.MLB_ITPCSUBJECTCODE:
                    return 3;
                case Constants.OVERSEA_SOCCER_ITPCSUBJECTCODE:
                    return 4;
            }

            return 0;
        }
        #endregion

        #region Image Processing

        public static string SaveImageToDirectory(string imageDataURL, string filePath)
        {
            string fileName = Guid.NewGuid().ToString();
            var base64Data = Regex.Match(imageDataURL, @"data:(?<mime>[\w/\-\.]+);(?<encoding>\w+),(?<data>.*)").Groups["data"].Value;
            var mimeData = Regex.Match(imageDataURL, @"data:(?<mime>[\w/\-\.]+);(?<encoding>\w+),(?<data>.*)").Groups["mime"].Value;
            var fileExtension = GetExtension(mimeData);
            var binData = Convert.FromBase64String(base64Data);
            string fullPath = "";
            MemoryStream ms = null;
            Image image = null;
            fileName = fileName + fileExtension;
            while (File.Exists(fullPath))
            {
                fileName = Guid.NewGuid().ToString() + fileExtension;
            }
            fullPath = filePath + fileName;
            if (!File.Exists(fullPath))
            {
                ms = new MemoryStream(binData, 0, binData.Length);
                // Convert byte[] to Image
                ms.Write(binData, 0, binData.Length);
                image = Image.FromStream(ms);
                image.Save(fullPath);
                image.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                ms.Dispose();
                ms.Close();
            }
            if (File.Exists(fullPath))
            {
                image.Dispose();
                ms.Close();
                return fileName;
            }
            return null;
        }

        private static string GetExtension(string mimeData)
        {
            string result = "";
            switch (mimeData)
            {
                case "image/jpeg":
                    result = ".jpeg";
                    break;
                case "image/jpg":
                    result = ".jpeg";
                    break;
                case "image/gif":
                    result = ".gif";
                    break;
                case "image/png":
                    result = ".png";
                    break;
                default:
                    break;
            }
            return result;
        }
        #endregion

        #region Format Time
        /// <summary>
        /// KietNM :2015-04-01
        /// Format Time form int "1800" to string "18:00"
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static string FormatTime(string strTime)
        {
            string result = string.Empty;
            if (strTime.Length > 2)
            {
                result = strTime.Substring(0, 2) + ":" + strTime.Substring(2);
            }
            return result;
        }

        #endregion

        #region Format DateTime

        /// <summary>
        /// ■表示フォーマット
        /// <1時間以内の場合>「○分前」と1分刻みで表示
        /// <1時間以上、当日以内の場合>「○時間前」と1時間刻みで表示
        /// <当日以降の場合>「{0}月{1}日({2}){3}時{4}分」と表示
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime datetime)
        {
            if (datetime == null)
                return null;

            string result = string.Empty;

            int seconds = (int)((TimeSpan)(DateTime.Now - datetime)).TotalSeconds;
            int days = seconds / (60 * 60 * 24);
            int hours = seconds / (60 * 60);
            int minutes = seconds / 60;

            if (days > 0 || days < 0)
            {
                return string.Format("{0}月{1}日({2}){3}時{4}分",
                datetime.Month.ToString(),
                datetime.Day.ToString(),
                Utils.GetWeekDayToJapanTime(datetime),
                datetime.Hour.ToString(),
                datetime.Minute.ToString());
            }

            if (hours > 0)
                return hours + "時間前";

            //if (minutes > 0)
            return minutes + "分前";

            //return seconds + "秒前";

        }

        #endregion

        #region Format Date

        /// <summary>
        /// 
        /// ■表示フォーマット
        /// <1時間以内の場合>「○分前」と1分刻みで表示
        /// <1時間以上、当日以内の場合>「○時間前」と1時間刻みで表示
        /// <当日以降の場合>「{0}月{1}日({2})」と表示
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime datetime)
        {
            if (datetime == null)
                return null;

            string result = string.Empty;

            result = string.Format("{0}月{1}日({2})",
            datetime.Month.ToString(),
            datetime.Day.ToString(),
            Utils.GetWeekDayToJapanTime(datetime));

            return result;
        }

        #endregion

        #region Get Member by memberId
        /// <summary>
        /// Get Member entity by memberId.
        /// </summary>
        /// <param name="memberId">Member's Id</param>
        public static Member GetMember(long memberId)
        {
            Member result = null;

            using (ComEntities com = new ComEntities())
            {
                result = (from m in com.Member
                          where m.MemberId == memberId
                          select m).FirstOrDefault();

            }

            return result;
        }

        #endregion

        #region Follow Unfollow

        /// <summary>
        /// 他の会員をフォローする
        /// </summary>
        /// <param name="followerMemberId">フォローする会員(ログインユーザ)</param>
        /// <param name="followingMemberId">フォローされる会員</param>
        public static void follow(long followerMemberId, long followingMemberId)
        {
            try
            {
                if (followerMemberId <= 0)
                    throw new Exception("フォローする側の会員IDが不正です。");

                if (followingMemberId <= 0)
                    throw new Exception("フォローされる側の会員IDが不正です。");

                if (followerMemberId == followingMemberId)
                    throw new Exception("フォローする側とされる側の会員IDが同じです。");


                using (ComEntities com = new ComEntities())
                {
                    using (var dbContextTransaction = com.Database.BeginTransaction())
                    {
                        try
                        {
                            FollowList f = new FollowList
                            {
                                MemberID = followingMemberId,
                                FollowerMemberID = followerMemberId,
                                CreatedDate = DateTime.Now
                            };

                            com.FollowList.Add(f);

                            com.SaveChanges();

                            //お知らせ配信対象(NoticeDeliverySubject)テーブルにメール送信対象データを登録する。

                            var noticeId = (from ni in com.NoticeInfo
                                            where ni.MailDelivCondID == 3
                                            select ni.NoticeId).FirstOrDefault();

                            NoticeDeliverySubject nds = new NoticeDeliverySubject
                            {
                                NoticeId = (int)noticeId,               //お知らせ(NoticeInfo)のMailDelivCondID = 3 のレコードのお知らせID(NoticeId)を設定
                                MemberId = (int)followingMemberId,       //フォローされた会員ＩＤ
                                ClassClass = 6,                         //フォロー
                                UniqueID = (int)followerMemberId,      //ログインユーザの会員ID
                                AlreadyReadFlg = false,
                                NoticeDeliveryStatus = 1,
                                CreatedAccountID = followerMemberId.ToString(),
                                CreatedDate = DateTime.Now
                            };

                            if (nds != null)
                            {
                                com.NoticeDeliverySubject.Add(nds);
                                com.SaveChanges();
                            }

                            NoticeInfo noticeInfo = (from ni in com.NoticeInfo
                                                     where ni.MailDelivCondID == 3 && ni.NoticeClass == 3
                                                     select ni).FirstOrDefault();
                            string title = noticeInfo.Title;
                            string body = noticeInfo.Body;

                            NoticeInfoForMyPage notice = new NoticeInfoForMyPage();

                            notice.ClassClass = NoticeInfoForMyPage.CLS_FOLLOW;
                            notice.MemberId = (int)followerMemberId;
                            notice.Follower = (from m in com.Member where m.MemberId == followerMemberId select m.Nickname).FirstOrDefault();
                            notice.Follow = (from m in com.Member where m.MemberId == followingMemberId select m.Nickname).FirstOrDefault();

                            notice.setTitle(NoticeInfoForMyPage.CLS_FOLLOW, title);
                            notice.setBody(NoticeInfoForMyPage.CLS_FOLLOW, body);

                            Splg.Areas.MyPage.MyPageCommon.SendMail(followingMemberId, notice.Title, notice.Body);


                            dbContextTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            throw ex;
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 他の会員へのフォローを外す
        /// </summary>
        /// <param name="followerMemberId">フォローする会員(ログインユーザ)</param>
        /// <param name="followingMemberId">フォローされる会員</param>
        public static void unfollow(long followerMemberId, long followingMemberId)
        {
            if (followerMemberId <= 0)
                throw new Exception("フォローする側の会員IDが不正です。");

            if (followingMemberId <= 0)
                throw new Exception("フォロー側の会員IDが不正です。");

            if (followerMemberId == followingMemberId)
                throw new Exception("フォロー側とされる側の会員IDが同じです。");

            using (ComEntities com = new ComEntities())
            {
                using (var dbContextTransaction = com.Database.BeginTransaction())
                {
                    try
                    {

                        if (followerMemberId > 0 && followingMemberId > 0)
                        {
                            FollowList followList = (from f in com.FollowList
                                                     where f.FollowerMemberID == followerMemberId
                                                     && f.MemberID == followingMemberId
                                                     select f).FirstOrDefault();

                            com.FollowList.Remove(followList);

                            com.SaveChanges();

                            //お知らせ配信対象の削除
                            NoticeDeliverySubject nds = (from n in com.NoticeDeliverySubject
                                                         where
                                                         n.MemberId == (int)followingMemberId             //フォローされた会員ＩＤ
                                                         && n.ClassClass == 6                    //フォロー
                                                         && n.UniqueID == (int)followerMemberId //自分の会員ID
                                                         && n.AlreadyReadFlg == false
                                                         && n.NoticeDeliveryStatus == 1
                                                         select n).FirstOrDefault();

                            if (nds != null)
                            {
                                com.NoticeDeliverySubject.Remove(nds);
                                com.SaveChanges();
                            }

                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }

                }
            }
        }

        #endregion

        #region Get end of day until 23:59:59.999 and start of day 00:00:00.000
        public static DateTime GetStartOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }
        public static DateTime GetEndOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 000);
        }
        #endregion

        #region フォローユーザーの予想の取得
        /// <summary>
        /// フォローユーザーの予想の取得
        /// </summary>
        /// <param name="com"></param>
        /// <param name="gameID"></param>
        /// <param name="memberId"></param>
        /// <param name="ClassClass"></param>
        /// <param name="SportsID"></param>
        /// <param name="FixBetSelectID"></param>
        /// <returns></returns>
        public static List<Member> GetExpectingMembers(ComEntities com, int gameID, long memberId, int ClassClass, int SportsID, int FixBetSelectID)
        {
            List<Member> result = (from fl in com.FollowList
                                   join ep in com.ExpectPoint on fl.MemberID.ToString() equals ep.CreatedAccountID
                                   join et in com.ExpectTarget on ep.ExpectTargetID equals et.ExpectTargetID
                                   join m in com.Member on fl.MemberID equals m.MemberId
                                   where fl.FollowerMemberID == memberId                                
                                   && et.UniqueID == (int)gameID
                                   && et.ClassClass == ClassClass  //分類種別
                                   && et.SportsID == SportsID
                                   && ep.BetSelectID == FixBetSelectID
                                   && ep.SituationStatus == 1//BET選択肢ID //TODO SituationStatus 要確認（!=2で見なくて良い？）
                                   select m).ToList();

            //Old query - CucHTP
            //List<Member> result = (from fl in com.FollowList
            //                       join ep in com.ExpectPoint on fl.FollowerMemberID.ToString() equals ep.CreatedAccountID
            //                       join et in com.ExpectTarget on ep.ExpectTargetID equals et.ExpectTargetID
            //                       join m in com.Member on fl.FollowerMemberID equals m.MemberId
            //                       where fl.MemberID == memberId
            //                       && et.UniqueID == (int)gameID
            //                       && et.ClassClass == ClassClass  //分類種別
            //                       && et.SportsID == SportsID
            //                       && ep.BetSelectID == FixBetSelectID
            //                       && ep.SituationStatus == 1//BET選択肢ID
            //                       select m).ToList();
            return result;
        }
        #endregion

        #region Get Number of Unread Notice
        public static int GetNumberOfUnreadNotice(int memberID)
        {
            ComEntities com = new ComEntities();
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
            int result = 0;
            var query = from ni in com.NoticeInfo
                        join nds in com.NoticeDeliverySubject on ni.NoticeId equals nds.NoticeId
                        where (ni.NoticeClass == 1 || ni.NoticeClass == 3)
                        && nds.AlreadyReadFlg == false
                        && nds.MemberId == memberID
                        && nds.CreatedDate >= oneMonthAgo
                        //&& ni.NoticeId <= 4  //PointsPtが付与の仕様確定待ちのため暫定で絞る
                        select nds;
            if (query != null)
                result = query.Count();
            else
                result = 0;

            return result;
        }
        #endregion

        #region Get Sport Name by sportID
        public static string GetSportNameById(int sportId)
        {
            switch (sportId)
            {
                case 1:
                    return "プロ野球";
                case 2:
                    return "Jリーグ";
                case 3:
                    return "MLB";
                case 4:
                    return "海外サッカー";
            }
            return string.Empty;
        }
        #endregion
    }
}

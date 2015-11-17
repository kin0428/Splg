using System;

namespace Splg.Services.System
{
    /// <summary>
    /// 日付の取得等を行うサービス
    /// </summary>
    public class SystemDatetimeService
    {
        /// <summary>
        /// システム日付を取得
        /// </summary>
        public DateTime SystemDate
        {
            // HACK:とりあえず,現在日時を返します
            get { return DateTime.Now; }
        }

        /// <summary>
        /// システム日時を取得
        /// </summary>
        public DateTime Now
        {
            // HACK:とりあえず,現在日時を返します
            get { return DateTime.Now; }
        }

        /// <summary>
        /// 対象年を取得
        /// </summary>
        /// <returns>対象年</returns>
        public int TargetYear
        {
            // HACK:とりあえず,現在日時の月を返します
            get { return this.SystemDate.Year; }
        }

        /// <summary>
        /// 対象月を取得
        /// </summary>
        /// <returns>対象年</returns>
        public int TargetMonth
        {
            // HACK:とりあえず,現在日時の月を返します
            get { return this.SystemDate.Month; }
        }
    }
}
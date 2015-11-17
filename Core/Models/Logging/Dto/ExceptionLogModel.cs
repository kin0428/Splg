using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Models.Logging.Dto
{
    public class ExceptionLogModel
    {
        /// <summary>
        /// 会員ID
        /// </summary>
        public long? MemberId { get; set; }

        /// <summary>
        /// セッションID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// コントローラー名
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// アクション名
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// ユーザーエージェント
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// リファラー
        /// </summary>
        public string UrlReferrer { get; set; }
    }
}
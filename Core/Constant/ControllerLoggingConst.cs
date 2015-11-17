using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Constant
{
    public class ControllerLoggingConst
    {
        /// <summary>
        /// ログタイプ
        /// </summary>
        public enum LoggingType
        { 
            /// <summary>
            /// アクションログ
            /// </summary>
            AppActionLog,

            /// <summary>
            /// 例外ログ
            /// </summary>
            ExceptionLog       
        }
    }
}
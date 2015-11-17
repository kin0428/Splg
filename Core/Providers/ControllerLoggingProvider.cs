using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Core.Constant;
using Splg.Core.Services;

namespace Splg.Core.Providers
{
    public class ControllerLoggingProvider
    {
        #region Constructor
        public ControllerLoggingProvider()
        {

        }
        #endregion

        #region method
        /// <summary>
        /// ログ書き込み
        /// </summary>
        public void WriteLog(ControllerContext controllerContext, ControllerLoggingConst.LoggingType Type)
        {
            GetLoggingService(Type).WriteLog(controllerContext);
        }

        /// <summary>
        /// ロギングサービス取得
        /// </summary>
        private ILoggingService GetLoggingService(ControllerLoggingConst.LoggingType Type)
        {
            switch (Type)
            {
                case ControllerLoggingConst.LoggingType.AppActionLog:
                    return new AppActionLogService();
                case ControllerLoggingConst.LoggingType.ExceptionLog:
                    return new ExceptionLogService();
                default:
                    return null;
            }
        }
        #endregion
    }
}
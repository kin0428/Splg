using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models;

namespace Splg.Core.Models.Logging
{
    public class LoggingDao
    {
        #region property
        private ComEntities ComEntities { get; set; }
        #endregion

        #region constructor
        public LoggingDao(ComEntities comEntities)
        {
            //Todo:Importでやりたい
            ComEntities = comEntities;
        }
        #endregion

        /// <summary>
        /// アプリアクションログ作成
        /// </summary>
        public void CreateAppActionLog(AppActionLog appActionLog)
        {
            ComEntities.AppActionLog.Add(appActionLog);        
        }

    }
}
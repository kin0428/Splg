using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.Dto
{
    /// <summary>
    /// 時間帯別得失点指標
    /// </summary>
    public class GoalTimeZone
    {
        /// <summary>
        /// 時間帯区分（1:前半 2:後半 3:延長前半 4:延長後半）
        /// </summary>
        public int TimeZoneDivision{get;set;}

        /// <summary>
        /// 得点数
        /// </summary>
	    public int Goal{get;set;}

        /// <summary>
        /// 失点数
        /// </summary>
        public int Lost{ get; set; }
    }
}
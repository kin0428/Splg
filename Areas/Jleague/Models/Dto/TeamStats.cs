using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models.Dto;

namespace Splg.Areas.Jleague.Models.Dto
{
    public class TeamStats
    {
        /// <summary>
        /// 得点指標
        /// </summary>
        public GoalIndex GoalIndex { get; set; }

        /// <summary>
        /// 失点指標
        /// </summary>
        public GoalIndex LostIndex { get; set; }

        /// <summary>
        /// パス指標
        /// </summary>
        public PassIndex PassIndex { get; set; }
    }
}
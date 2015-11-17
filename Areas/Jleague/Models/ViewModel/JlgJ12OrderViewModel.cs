#region Author Information
/*
 * Developer	: Tran Sy Huynh
 * 
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgJ12OrderViewModel
    {
        public RankInfoRT RankingInfo { get; set; }
        public int SeasonID { get; set; }
        public string SeasonName { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamIcon { get; set; }
        public string GroupID { get; set; }
    }
}
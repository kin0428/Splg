using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Mlb.Models.ViewModels.InfosModel
{
    public class MlbDivisionGroupModel
    {
        public int LeagueID { get; set; }
        public int DivID { get; set; }
        public long LeagueGroupMlbId { get; set; }
        public long DivGroupMlbId { get; set; }
        
    }
}
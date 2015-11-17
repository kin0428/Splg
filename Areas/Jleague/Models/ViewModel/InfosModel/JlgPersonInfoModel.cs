using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgPersonInfoModel
    {
        public int PlayerInfoREId { get; set; }
        public int TeamInfoREId { get; set; }
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string PlayerNameS { get; set; }
        public Nullable<int> StateID { get; set; }
        public string StateName { get; set; }
        public Nullable<int> Time { get; set; }
        public Nullable<int> Half { get; set; }
        public Nullable<short> OwnGoalF { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
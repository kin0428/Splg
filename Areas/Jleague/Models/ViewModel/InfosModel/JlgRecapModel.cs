using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgRecapModel
    {
        public Nullable<int> StartTime { get; set; }
        public Nullable<int> EndTime { get; set; }
        public string GameRecap { get; set; }
        public string PlayerName { get; set; }
        public string PlayerNameS { get; set; }
        public Nullable<int> StateID { get; set; }
        public string StateName { get; set; }
        public Nullable<int> Time { get; set; }
        public Nullable<int> Half { get; set; }

    }
}
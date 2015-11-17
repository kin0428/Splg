using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgPKInfoModel
    {

        public int HV { get; set; }
        public int No { get; set; }
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string PlayerNameS { get; set; }
        public string PlayerUni { get; set; }
        /// <summary>
        /// PK成功=1、失敗=0	
        /// </summary>
        public Nullable<short> SuccessF { get; set; }
        /// <summary>
        /// PK成功=○、失敗=×	
        /// </summary>
        public string Sign { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
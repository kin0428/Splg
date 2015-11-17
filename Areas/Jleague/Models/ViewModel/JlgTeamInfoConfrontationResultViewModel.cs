using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgTeamInfoConfrontationResultViewModel
    {

        public int Year { get; set; }
        public int SendDate { get; set; }
        public int GameKindID { get; set; }
        public string GameKindName { get; set; }
        public int TeamID { get; set; }
        public string TeamIcon { get; set; }
        public string TeamName { get; set; }
        public Nullable<int> Game { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> Lose { get; set; }
        public Nullable<int> Draw { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<int> Lost { get; set; }
        public Nullable<int> Shoot { get; set; }
        public Nullable<int> Assist { get; set; }
        public Nullable<int> FKD { get; set; }
        public Nullable<int> FKI { get; set; }
        public Nullable<int> CK { get; set; }
        public Nullable<int> SufferShoot { get; set; }
        public Nullable<int> Offside { get; set; }
        public Nullable<int> Yellow { get; set; }
        public Nullable<int> Red { get; set; }
        public Nullable<int> Point { get; set; }
        public Nullable<int> Time { get; set; }
       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgGameEndScroreInfoModel
    {


        //Jlg.GameStateLG
        public int TeamInfoLGId { get; set; }
        public int GameReportLGId { get; set; }
        public short HV { get; set; }
        public int ID { get; set; }
        public string NameS { get; set; }
        public string Director { get; set; }
        public string PostName { get; set; }
        public Nullable<int> BeforePoint { get; set; }
        public Nullable<int> AfterPoint { get; set; }
        public Nullable<int> FormationID { get; set; }
        public string FormationName { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<int> PKBScore { get; set; }
        public Nullable<int> PKScore { get; set; }
        public Nullable<int> PK { get; set; }
        public Nullable<int> GK { get; set; }
        public Nullable<int> Shoot { get; set; }
        public Nullable<int> Assist { get; set; }
        public Nullable<int> FKD { get; set; }
        public Nullable<int> FKI { get; set; }
        public Nullable<int> CK { get; set; }
        public Nullable<int> Yellow { get; set; }
        public Nullable<int> Red { get; set; }
        public Nullable<int> ShootGoal { get; set; }
        public Nullable<int> Offside { get; set; }
        public Nullable<int> Gain { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Mlb.Models.ViewModels.InfosModel
{
    public class MlbTeamInfoPSPModel
    {
        public long TeamInfoPSPId { get; set; }
        public long GameInfoPSPId { get; set; }
        public short HV { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameS { get; set; }
        public string Initial { get; set; }
        public Nullable<int> MainID { get; set; }
        public string MainNameE { get; set; }
        public string MainNameES { get; set; }
        public Nullable<short> PredictionF { get; set; }
        public Nullable<int> PitcherID { get; set; }
        public string PitcherName { get; set; }
        public string PitcherNameS { get; set; }
        public string PitcherNum { get; set; }
        public Nullable<short> PitcherArm { get; set; }
        public string PitcherArmStr {
            get
            {
                if (PitcherArm == 1)
                    return "右投げ";
                if (PitcherArm == 2)
                    return "右投げ";

                return "";
            }

        }
        public Nullable<int> IntervalDate { get; set; }
        public Nullable<int> GamePitched { get; set; }
        public Nullable<int> GameStarted { get; set; }
        public Nullable<int> Wins { get; set; }
        public Nullable<int> Losses { get; set; }
        public Nullable<int> Saves { get; set; }
        public string Era { get; set; }
        public Nullable<int> VsGamePitched { get; set; }
        public Nullable<int> VsGameStarted { get; set; }
        public Nullable<int> VsWins { get; set; }
        public Nullable<int> VsLosses { get; set; }
        public Nullable<int> VsSaves { get; set; }
        public string VsEra { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        
    }
}
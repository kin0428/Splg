using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Mlb.Models.ViewModels.InfosModel
{
    public class MlbPlayerInfoGOModel
    {

        public long PlayerInfoGOId { get; set; }
        public long TeamInfoGOId { get; set; }
        public short BatNo { get; set; }
        public short Position { get; set; }
        public int PlayerID { get; set; }
        public string BackNumber { get; set; }
        public string PlayerNameS { get; set; }
        public string PlayerNameL { get; set; }
        public Nullable<short> PitchingArm { get; set; }
        public Nullable<short> BattingType { get; set; }
        public string Avg { get; set; }
        public Nullable<int> Hr { get; set; }
        public Nullable<int> Rbi { get; set; }
        public Nullable<short> Condition { get; set; }
        public Nullable<int> TeamID { get; set; }
        public string TeamS { get; set; }
        public string TeamES { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    public class JlgPlayerInfoStModel
    {

        public long PlayerInfoSTId { get; set; }
        public long TeamInfoSTId { get; set; }
        public short StartBatNo { get; set; }
        public short StartPosition { get; set; }
        public short PositionType { get; set; }
        public int PlayerID { get; set; }
        public string BackNumber { get; set; }
        public string PlayerNameS { get; set; }
        public string PlayerNameL { get; set; }
        public Nullable<short> PitchingArm { get; set; }
        public Nullable<short> BattingType { get; set; }
        public Nullable<short> GameF { get; set; }
        public string Avg { get; set; }
        public Nullable<int> Hr { get; set; }
        public Nullable<int> Rbi { get; set; }
        public string Era { get; set; }
        public Nullable<int> TeamID { get; set; }
        public string TeamS { get; set; }
        public string TeamES { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
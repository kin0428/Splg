//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Splg.Areas.Npb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GameInfoGTE
    {
        public long GameInfoGTEId { get; set; }
        public int GameID { get; set; }
        public Nullable<int> GameDate { get; set; }
        public Nullable<int> GameTime { get; set; }
        public Nullable<int> StartTime { get; set; }
        public Nullable<int> Round { get; set; }
        public Nullable<int> GameKindID { get; set; }
        public string GameKindName { get; set; }
        public Nullable<int> StadiumID { get; set; }
        public string StadiumName { get; set; }
        public Nullable<short> DoubleHeaderF { get; set; }
        public Nullable<int> Attendance { get; set; }
        public string UmpireChief { get; set; }
        public string UmpireFirst { get; set; }
        public string UmpireSecond { get; set; }
        public string UmpireThird { get; set; }
        public string UmpireLeft { get; set; }
        public string UmpireRight { get; set; }
        public Nullable<short> GameStateID { get; set; }
        public Nullable<int> Inning { get; set; }
        public Nullable<short> TB { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

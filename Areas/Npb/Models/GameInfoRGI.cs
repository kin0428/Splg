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
    
    public partial class GameInfoRGI
    {
        public long GameInfoRGIId { get; set; }
        public long RealGameInfoRootRGIId { get; set; }
        public int GameID { get; set; }
        public int HomeTeam { get; set; }
        public string ShortNameHomeTeam { get; set; }
        public Nullable<int> VisitorTeam { get; set; }
        public string ShortNameVisitorTeam { get; set; }
        public Nullable<int> StadiumCD { get; set; }
        public string ShortNameStadium { get; set; }
        public Nullable<int> Round { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> Lose { get; set; }
        public Nullable<int> Draw { get; set; }
        public Nullable<int> Inning { get; set; }
        public string BottomTop { get; set; }
        public string GameSetSituationCD { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
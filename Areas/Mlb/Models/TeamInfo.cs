//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Splg.Areas.Mlb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TeamInfo
    {
        public long TeamInfoId { get; set; }
        public long TeamInfoMSTHeaderId { get; set; }
        public int TeamID { get; set; }
        public string TeamFullName { get; set; }
        public string TeamName { get; set; }
        public Nullable<int> LeagueID { get; set; }
        public string LeagueName { get; set; }
        public Nullable<int> DivID { get; set; }
        public string DivName { get; set; }
        public Nullable<int> FoundedYear { get; set; }
        public string OwnerName { get; set; }
        public string ManagerName { get; set; }
        public Nullable<int> WSTitles { get; set; }
        public Nullable<int> Pennants { get; set; }
        public Nullable<int> DivTitles { get; set; }
        public Nullable<int> WCBerths { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

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
    
    public partial class WinBest10
    {
        public long WinBest10Id { get; set; }
        public long WinBest10HeaderId { get; set; }
        public int Ranking { get; set; }
        public int PlayerCD { get; set; }
        public string Name { get; set; }
        public string ShortNamePlayer { get; set; }
        public string Num { get; set; }
        public Nullable<int> TeamCD { get; set; }
        public string ShortNameTeam { get; set; }
        public string TeamInitial { get; set; }
        public Nullable<int> Win { get; set; }
        public Nullable<int> GamePitched { get; set; }
        public string EarnedRunAverage { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

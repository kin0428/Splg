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
    
    public partial class HittingStats5thGameHeader
    {
        public long HittingStats5thGameHeaderId { get; set; }
        public int Year { get; set; }
        public int GameAssortment { get; set; }
        public string Season { get; set; }
        public string HomeTeam { get; set; }
        public string VisitorTeam { get; set; }
        public Nullable<int> Matchday { get; set; }
        public string GameCD { get; set; }
        public Nullable<int> TeamCD { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

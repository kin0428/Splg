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
    
    public partial class DSMEPInfoCO
    {
        public long DSMEPInfoCOId { get; set; }
        public long GameInfoCOId { get; set; }
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string PlayerNameS { get; set; }
        public string BackNumber { get; set; }
        public Nullable<int> TeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamNameS { get; set; }
        public string MEPReason { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
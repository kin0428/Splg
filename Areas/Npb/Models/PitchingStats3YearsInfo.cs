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
    
    public partial class PitchingStats3YearsInfo
    {
        public long PitchingStats3YearsInfoId { get; set; }
        public long PitchingStats3YearsId { get; set; }
        public int Year { get; set; }
        public int TeamID { get; set; }
        public string TeamS { get; set; }
        public string TeamES { get; set; }
        public string ERA { get; set; }
        public Nullable<int> G { get; set; }
        public Nullable<int> W { get; set; }
        public Nullable<int> L { get; set; }
        public Nullable<int> S { get; set; }
        public Nullable<int> HLD { get; set; }
        public Nullable<int> HP { get; set; }
        public Nullable<int> IP { get; set; }
        public Nullable<int> IP3 { get; set; }
        public Nullable<int> H { get; set; }
        public Nullable<int> HR { get; set; }
        public Nullable<int> SO { get; set; }
        public Nullable<int> BB { get; set; }
        public Nullable<int> HBP { get; set; }
        public Nullable<int> ER { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
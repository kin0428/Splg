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
    
    public partial class StatLast5Years
    {
        public long StatLast5YearsId { get; set; }
        public long TeamInfoId { get; set; }
        public int Year { get; set; }
        public string Stat { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

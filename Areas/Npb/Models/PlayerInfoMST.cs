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
    
    public partial class PlayerInfoMST
    {
        public long PlayerInfoMSTId { get; set; }
        public long PlayerInfoMSTHeaderId { get; set; }
        public int PlayerCD { get; set; }
        public string ShortNamePlayer { get; set; }
        public string Player { get; set; }
        public string UniformNO { get; set; }
        public Nullable<int> PitchField { get; set; }
        public string PitcherSide { get; set; }
        public string BatterSide { get; set; }
        public Nullable<int> SmartStart { get; set; }
        public Nullable<int> SmartEnd { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

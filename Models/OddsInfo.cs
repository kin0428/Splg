//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Splg.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OddsInfo
    {
        public long ExpectTargetID { get; set; }
        public long BetSelectMasterID { get; set; }
        public long SportID { get; set; }
        public short ClassificationType { get; set; }
        public int UniqueID { get; set; }
        public decimal Odds { get; set; }
        public short Status { get; set; }
        public Nullable<System.DateTime> FixDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

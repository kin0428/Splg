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
    
    public partial class PointHistory
    {
        public long PontHistoryId { get; set; }
        public Nullable<long> PointId { get; set; }
        public Nullable<long> ExpectPointId { get; set; }
        public Nullable<long> CampaignId { get; set; }
        public short PointClass { get; set; }
        public int Points { get; set; }
        public short AdjustmentClass { get; set; }
        public short OperationClass { get; set; }
        public bool Status { get; set; }
        public string CreatedAccountID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedAccountID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public short HistoryClass { get; set; }
    }
}
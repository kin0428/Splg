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
    
    public partial class PointBulkTemporal
    {
        public long PointBulkID { get; set; }
        public Nullable<long> MemberID { get; set; }
        public Nullable<short> PointClass { get; set; }
        public Nullable<short> GiveTargetYear { get; set; }
        public Nullable<short> GiveTargetMonth { get; set; }
        public Nullable<short> GiveTargetWeek { get; set; }
        public Nullable<System.DateTime> GivingDate { get; set; }
        public Nullable<long> CampaignId { get; set; }
        public Nullable<int> Points { get; set; }
        public Nullable<short> DisposalFlg { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

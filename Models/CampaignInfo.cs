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
    
    public partial class CampaignInfo
    {
        public long CampaignId { get; set; }
        public string CampaignTitle { get; set; }
        public short CampaignClass { get; set; }
        public Nullable<short> Condition { get; set; }
        public int Points { get; set; }
        public Nullable<System.DateTime> GiveTime { get; set; }
        public System.DateTime CampaignStartTime { get; set; }
        public System.DateTime CampaignEndTime { get; set; }
        public string CreatedAccountId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedAccountID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

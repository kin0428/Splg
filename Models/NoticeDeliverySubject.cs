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
    
    public partial class NoticeDeliverySubject
    {
        public int NoticeDeliverySubjectId { get; set; }
        public int NoticeId { get; set; }
        public int MemberId { get; set; }
        public short ClassClass { get; set; }
        public int UniqueID { get; set; }
        public Nullable<int> UniqueID2 { get; set; }
        public bool AlreadyReadFlg { get; set; }
        public Nullable<short> NoticeDeliveryStatus { get; set; }
        public string CreatedAccountID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedAccountID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> UniqueID3 { get; set; }
    }
}
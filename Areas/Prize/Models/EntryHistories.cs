//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Splg.Areas.Prize.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EntryHistories
    {
        public long EntryHistoryId { get; set; }
        public long PointId { get; set; }
        public long RallyGoodId { get; set; }
        public System.DateTime EntryDateTime { get; set; }
        public string CreatedAccountID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedAccountID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

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
    
    public partial class Contribution
    {
        public long ContributeId { get; set; }
        public long MemberId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ContributedPicture { get; set; }
        public Nullable<System.DateTime> ContributeDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
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
    
    public partial class RallyGoodsRemarksLink
    {
        public long RallyGoodsRemarksLinkId { get; set; }
        public long RallyGoodRemarksId { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public short DisplayOrder { get; set; }
        public string CreatedAccountID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedAccountID { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
}

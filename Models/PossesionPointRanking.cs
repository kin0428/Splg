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
    
    public partial class PossesionPointRanking
    {
        public long MemberID { get; set; }
        public int UpDownPoint { get; set; }
        public int UpDownRanking { get; set; }
        public long PossesionPointRankingID { get; set; }
        public System.DateTime TargetDate { get; set; }
        public int TargetPossesionPoint { get; set; }
        public int TargetRanking { get; set; }
        public string CreatedAccountID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedAccountID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
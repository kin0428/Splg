//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Splg.Areas.Jleague.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FormationReportLF
    {
        public int FormationReportLFId { get; set; }
        public int GameID { get; set; }
        public int GameDate { get; set; }
        public int GameKindID { get; set; }
        public string GameKindName { get; set; }
        public int SeasonID { get; set; }
        public string SeasonName { get; set; }
        public int TeamID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

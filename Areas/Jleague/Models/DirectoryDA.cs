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
    
    public partial class DirectoryDA
    {
        public int DirectoryDAId { get; set; }
        public int SendDate { get; set; }
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string TeamNameS { get; set; }
        public string TeamNameE { get; set; }
        public string TeamNameES { get; set; }
        public Nullable<int> PlayerC { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

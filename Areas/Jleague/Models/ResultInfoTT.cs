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
    
    public partial class ResultInfoTT
    {
        public int ResultInfoTTId { get; set; }
        public int TeamInfoTTId { get; set; }
        public int TimeDivision { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int Goal { get; set; }
        public int Lost { get; set; }
        public int Shoot { get; set; }
        public Nullable<int> Assist { get; set; }
        public int FKD { get; set; }
        public int FKI { get; set; }
        public int CK { get; set; }
        public int SufferShoot { get; set; }
        public int Offside { get; set; }
        public int Yellow { get; set; }
        public int Red { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}

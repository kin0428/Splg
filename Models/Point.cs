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
    
    public partial class Point
    {
        public long PointID { get; set; }
        public long MemberID { get; set; }
        public int FundsPoint { get; set; }
        public int PossesionPoint { get; set; }
        public System.DateTime GivingDate { get; set; }
        public short GiveTargetWeek { get; set; }
        public System.DateTime BetStartDate { get; set; }
        public System.DateTime BetEndDate { get; set; }
        public Nullable<long> PayOffPointsId { get; set; }
        public Nullable<bool> PayOffFlg { get; set; }
        public int PayOffPoints { get; set; }
        public Nullable<System.DateTime> PayOffTime { get; set; }
        public string CreatedAccountID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedAccountID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public short GiveTargetYear { get; set; }
        public short GiveTargetMonth { get; set; }
    }
}

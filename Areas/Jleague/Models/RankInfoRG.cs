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
    
    public partial class RankInfoRG
    {
        public int RankInfoRGId { get; set; }
        public int RankGoalReportRGId { get; set; }
        public int Ranking { get; set; }
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public string PlayerNameS { get; set; }
        public Nullable<int> TeamID { get; set; }
        public string TeamNameS { get; set; }
        public Nullable<int> Goal { get; set; }
        public Nullable<int> GoalFK { get; set; }
        public Nullable<int> GoalPK { get; set; }
        public Nullable<int> GoalCK { get; set; }
        public Nullable<int> GoalRight { get; set; }
        public Nullable<int> GoalLeft { get; set; }
        public Nullable<int> GoalHead { get; set; }
        public Nullable<int> GoalOther { get; set; }
        public Nullable<int> Shoot { get; set; }
        public Nullable<int> Game { get; set; }
        public Nullable<int> Time { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
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
    
    public partial class StatsReportLS
    {
        public int StatsReportLSId { get; set; }
        public int GameID { get; set; }
        public int GameDate { get; set; }
        public int GameKindID { get; set; }
        public string GameKindName { get; set; }
        public int SeasonID { get; set; }
        public string SeasonName { get; set; }
        public Nullable<int> StartTime { get; set; }
        public Nullable<int> StadiumID { get; set; }
        public string StadiumNameS { get; set; }
        public Nullable<short> DayNight { get; set; }
        public Nullable<int> StateID { get; set; }
        public string StateName { get; set; }
        public Nullable<int> Time { get; set; }
        public Nullable<int> Half { get; set; }
        public Nullable<int> SituationID { get; set; }
        public string SituationName { get; set; }
        public Nullable<short> GameFixF { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
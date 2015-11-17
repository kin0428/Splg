using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel 
{
    public class   JlgPlayerGameInfoModel : System.IComparable
    {

        public int PlayerInfoLPSTId { get; set; }
        //public int StartingInfoLPId { get; set; }
        public int PlayerInfoLPBEId { get; set; }
        public int BenchInfoLPId { get; set; }

        public int HV { get; set; }
        public bool IsStartingMember { get; set; }

        public int ID { get; set; }
        public string Name { get; set; }
        public string NameS { get; set; }
        public string Uni { get; set; }

        public int UniInt { get; set; }

        public string Pos { get; set; }
        public Nullable<int> No { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> Yellow { get; set; }
        public Nullable<int> TeamID { get; set; }
        public string TeamS { get; set; }
        public Nullable<int> Goal { get; set; }
        public Nullable<int> Card { get; set; }

        public int Change_StateID { get; set; }
        public string Change_StateName { get; set; }
        public Nullable<int> Change_Time { get; set; }  //開始相対時間(MM)、分換算
        public Nullable<int> Change_Half { get; set; }  //ハーフ開始相対時間(MM)、分換算
        public Nullable<short> Change_ExitF { get; set; }   //退場の場合=1、それ以外=0
        public Nullable<int> Change_InID { get; set; }
        public string Change_InName { get; set; }
        public string Change_InNameS { get; set; }
        public string Change_InUni { get; set; }
        public string Change_InPos { get; set; }
        public Nullable<int> Change_OutID { get; set; }
        public string Change_OutName { get; set; }
        public string Change_OutNameS { get; set; }
        public string Change_OutUni { get; set; }
        public string Change_OutPos { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (this.GetType() != obj.GetType())
                throw new ArgumentException("別の型とは比較できません。", "obj");

            JlgPlayerGameInfoModel obj2 = (JlgPlayerGameInfoModel)obj;
            int comp = this.Pos.CompareTo(obj2.Pos);
            if( comp != 0)
                return comp;

            int intVal1,intVal2;
            Int32.TryParse(this.Uni, out intVal1);
            Int32.TryParse(obj2.Uni, out intVal2);

            comp = intVal1 - intVal2;
            
            return comp;
        }

    }
}
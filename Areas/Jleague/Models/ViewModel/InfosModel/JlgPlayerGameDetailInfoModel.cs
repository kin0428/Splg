using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel 
{
    public class JlgPlayerGameDetailInfoModel : System.IComparable
    {

        public string Itype { get; set; }
        public int TeamId { get; set; }
        public string StateName { get; set; }
        public int Half { get; set; }

        public int Time { get; set; }

        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerNameS { get; set; }
        public short Divide { get; set; }
        public short SecondF { get; set; }
        public string PlayerNameK
        {
            get
            {
                string result = "";
                if (PlayerName != null)
                {
                    result = PlayerName == "不明" ? "O.G." : PlayerName;
                }
                return result;
            }
        }
        public string GPlayerNameSK
        {
            get
            {
                string result = "";
                if (PlayerNameS != null)
                {
                    result = PlayerNameS == "不明" ? "O.G." : PlayerNameS;
                }
                return result;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (this.GetType() != obj.GetType())
                throw new ArgumentException("別の型とは比較できません。", "obj");

            JlgPlayerGameDetailInfoModel obj2 = (JlgPlayerGameDetailInfoModel)obj;

            int comp = this.Time - obj2.Time;
            
            return comp;
        }

    }
}
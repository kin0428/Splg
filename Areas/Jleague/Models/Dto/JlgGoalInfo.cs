using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splg.Areas.Jleague.Models;

namespace Splg.Areas.Jleague.Models.Dto
{
    public class JlgGoalInfo : GoalInfoLG
    {
        public string GPlayerNameK
        {
            get
            {
                string result = "";
                if (GPlayerName != null)
                {
                    result = GPlayerName == "不明" ? "O.G." : GPlayerName;
                }
                return result;
            }
        }
        public string GPlayerNameSK
        {
            get
            {
                string result = "";
                if (GPlayerNameS != null)
                {
                    result = GPlayerNameS == "不明" ? "O.G." : GPlayerNameS;
                }
                return result;
            }
        }
    }
}

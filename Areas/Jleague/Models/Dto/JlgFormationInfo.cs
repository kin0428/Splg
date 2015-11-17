using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splg.Areas.Jleague.Models;
using Splg.Core.Constant;

namespace Splg.Areas.Jleague.Models.Dto
{
    public class JlgFormationInfo : FormationInfo
    {
        public string ValueForMobile { get; set; }

        public double AttackCBP { get; set; }

        public double DefenseCBP { get; set; }
        // 選手の攻撃力（表示用）
        public int AttackCBPForDisplay
        {
            get
            {
                int result = 0;
                result = (int)(AttackCBP / JlgChartConst.AttackCBPScale);
                return result;
            }
        }
        // 選手の守備力（表示用）
        public int DefenseCBPForDisplay
        {
            get
            {
                int result = 0;
                result = (int)(DefenseCBP / JlgChartConst.DefenseCBPScale);
                return result;
            }
        }
    }
}

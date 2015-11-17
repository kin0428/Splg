#region (c) 2015 Prime Labo - All rights reserved
/*                                      COPYRIGHT NOTICE
 * -------------------------------------------------------------------------------------
 * All materials (including but not limited to source code, compiled assemblies, images,
 * resources, etc.) are copyrighted to Prime Labo. No usage is allowed unless permitted 
 * by written consent. You may not use, reverse-engineer these materials under any
 * circumstances.
 * 
 *                                    PROJECT DESCRIPTION
 * -------------------------------------------------------------------------------------
 * Namespace	: Splg.Areas.Jleague.Models.ViewModel.InfosModel
 * Class		: JlgPersonalAchieveInfos
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-03-26
 */
#endregion
#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion
namespace Splg.Areas.Jleague.Models.ViewModel.InfosModel
{
    /// <summary>
    /// Infos Model using at JlgPersonalAchieveViewModel
    /// </summary>
    public class JlgPersonalAchieveInfos
    {
        public Nullable<int> Ranking { get; set; }
        public Nullable<int> PlayerID { get; set; }        
        public string PlayerNames { get; set; }
        public string PlayerName { get; set; }        
        public Nullable<int> TeamID { get; set; }  
        public string TeamNameS { get; set; }
        public string Position { get; set; }
        public Nullable<int> Goal { get; set; }
        public Nullable<int> GoalPK { get; set; }
        public Nullable<int> Shoot { get; set; }
        public Nullable<int> Game { get; set; }
        public Nullable<int> Time { get; set; }
        public Nullable<int> Yellow { get; set; }
        public Nullable<int> Red { get; set; }

        //２・シュート決定率　の　計算・表示方法
        // = Goal/Shoot
        public Nullable<decimal> RateGoalShoot { get; set; }

        //３・90分平均得点　の　計算・表示方法
        // = Goal/(Time*90)
        public Nullable<decimal> RateGoalTime { get; set; } 
    }
}
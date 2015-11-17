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
 * Class		: JlgTeamInfoPlayerInfos
 * Developer	: e-concier
 * Update date  : 2015-04-27
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
    public class JlgTeamInfoPlayerInfos : System.IComparable
    {
        public Nullable<int> TeamID { get; set; }
	    public Nullable<int> PlayerNo { get; set; }
	    public Nullable<int> PlayerID { get; set; }
	    public string RealName { get; set; }
	    public string RealNameK { get; set; }
	    public string PlayerName { get; set; }
	    public string PlayerNameS { get; set; }
	    public string PlayerNameKS { get; set; }
	    public string PlayerNameE { get; set; }
	    public string PlayerNameES { get; set; }
        public string UniformNO { get; set; }
	    public string Position { get; set; }
	    public Nullable<int> Class { get; set; }
	    public Nullable<int> NewcomerFlag { get; set; }
	    public string HomeTown { get; set; }
	    public string Blood { get; set; }
	    public Nullable<int> Height { get; set; }
	    public Nullable<int> Weight { get; set; }
	    public Nullable<int> Birthday { get; set; }
	    public string Career { get; set; }
	    public string IndividualTitle { get; set; }
	    public string PlayerTeamHistory { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (this.GetType() != obj.GetType())
                throw new ArgumentException("別の型とは比較できません。", "obj");

            JlgTeamInfoPlayerInfos obj2 = (JlgTeamInfoPlayerInfos)obj;
            int intVal1, intVal2;
            Int32.TryParse(this.UniformNO, out intVal1);
            Int32.TryParse(obj2.UniformNO, out intVal2);

            int comp = intVal1 - intVal2;

            return comp;


        }
    }
}
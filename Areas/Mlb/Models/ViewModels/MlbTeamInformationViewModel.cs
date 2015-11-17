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
 * Namespace	: Splg.Areas.Mlb.ViewModels
 * Class		: MlbTeamInformationViewModel
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region Using directivesusing System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Mlb.Models.ViewModels
{
    public class MlbTeamInformationViewModel
    {
        public TeamInfo TeamInfo { get; set; }
        public IEnumerable<JapanesePlayers> JapanesePlayers { get; set; }


        public string TeamIcon { get; set; }



        private List<JapanesePlayers> existingJapanesePlayers;
        public List<JapanesePlayers> getExistingJapanesePlayers(string year)
        {
            return MlbCommon.GetExistingJapanesePlayers(JapanesePlayers, year);
        }



        private List<JapanesePlayers> existedJapanesePlayers;
        public List<JapanesePlayers> getExistedJapanesePlayers(string year)
        {
            return MlbCommon.GetExistedJapanesePlayers(JapanesePlayers, year);
        }



    }
}
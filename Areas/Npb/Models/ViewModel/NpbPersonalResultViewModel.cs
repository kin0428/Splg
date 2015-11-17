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
 * Namespace	: Splg.Areas.Npb.Models.ViewModel
 * Class		: NpbPersonalResultViewModel
 * Developer	: Nguyen Minh Kiet
 * Update date  : 2015-03-04
 */
#endregion

#region Using directives
using Splg.Areas.Npb.Models.ViewModel.InfosModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Areas.Npb.Models.ViewModel
{
    /// <summary>
    /// Layer class to store news get from db.
    /// </summary>
    public class NpbPersonalResultViewModel
    {
        public IEnumerable<NpbPersonalResultInfos> SeBattingAverageBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> PaBattingAverageBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> SeHomerunBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> PaHomerunBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> SeRunBattedInBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> PaRunBattedInBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> SeWinBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> PaWinBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> SeEarnedRunAverageBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> PaEarnedRunAverageBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> SeSaveBest10 { get; set; }
        public IEnumerable<NpbPersonalResultInfos> PaSaveBest10 { get; set; }    
    }
}
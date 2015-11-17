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
 * Class		: MlbTeamInfoNewsViewModel
 * Developer	: e-concier
 * Updated date : 2015-04-10
 */
#endregion

#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models.News.ViewModel;
using PagedList;
#endregion

namespace Splg.Areas.Mlb.Models.ViewModels
{
    public class MlbTeamInfoNewsViewModel
    {

        public long NewsItemID { get; set; }
        public string Headline { get; set; }
        public string newstext { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Caption { get; set; }
        public string Content { get; set; }
        public string Duid { get; set; }
        public string SentFrom { get; set; }
        public string SubHeadline { get; set; }
        public int ItpcSubjectCode { get; set; }
        public int TotalViews { get; set; }
    }
}
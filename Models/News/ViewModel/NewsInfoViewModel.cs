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
 * Namespace	: Splg.Models.News.ViewModel
 * Class		: NewsInfoViewModel
 * Developer	: Huynh Thi Phuong Cuc
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace Splg.Models.News.ViewModel
{
    /// <summary>
    /// Layer class to store news get from db.
    /// </summary>
    public class NewsInfoViewModel
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
        public int TotalViewsInOneHour { get; set; }
    }
}
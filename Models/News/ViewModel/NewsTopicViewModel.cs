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
 * Class		: NewsTopicViewModel
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
    /// Layer class to store 2 part : 
    ///     Main news will be displayed
    ///     News related to main news
    ///     Topics related to main news
    /// get from db, use for each part on view.
    /// </summary>
    public class NewsTopicViewModel
    {
        public NewsInfoViewModel NewsDisplayed { get; set; }
        public IEnumerable<NewsInfoViewModel> RelatedNews { get; set; }
        public IEnumerable<TopicMaster> RelatedTopics { get; set; }
        public IEnumerable<PostedInfoViewModel> RelatedPosts { get; set; }
    }
}
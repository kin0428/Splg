#region  Author Information
/*  
 * 
 * Createdted      : Tran Sy Huynh
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models;
using Splg.Models.News.ViewModel;
#endregion

namespace Splg.Models.ViewModel
{
    public class HomeContentLeagueViewModel
    {
        public int SportID { get; set; }
        public IEnumerable<NewsInfoViewModel> HomeRecentNews { get; set; }
        public int GameID { get; set; }
        public IEnumerable<HomeRecentMatchViewModel> HomeRecentMatch { get; set; }
        public IEnumerable<PostedInfoViewModel> HomeRecentPost { get; set; }
    }
}
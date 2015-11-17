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
    public class HomeViewModel
    {
        public IEnumerable<NewsInfoViewModel> HomeTopNews { get; set; }
        public IEnumerable<HomeGameInfoViewModel> HomeGameInfoViewModels { get; set; }
        public IEnumerable<HomeContentLeagueViewModel> HomeContentLeagueViewModel { get; set; }
       
    }
}
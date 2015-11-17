#region Author Information
/*
 * Developer	: Tran Sy Huynh
 * 
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

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgJNewsViewModel
    {
        public string[] JList { get; set; }
        public PagedList.IPagedList<NewsInfoViewModel> TeamNewsList { get; set; }
    }
}
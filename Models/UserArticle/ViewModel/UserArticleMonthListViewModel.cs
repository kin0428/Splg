using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using Splg.Models.News.ViewModel;

namespace Splg.Models.UserArticle.ViewModel
{
    public class UserArticleMonthListViewModel
    {
        public int Year { get; set; }
        public List<int> YearList { get; set; }
        public List<int> MonthList { get; set; }

        public PagedList.IPagedList<PostedInfoViewModel> PostList { get; set; }

        public IEnumerable<TopicMaster> MostViewTopicList { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Splg.Models.UserArticle.ViewModel
{
    public class RelatedTopicViewModel
    {
        public long TopicId { get; set; }
        public string TopicName { get; set; }
        public string TopicURL { get; set; }
    }
    public class UserArticleRelatedTopicViewModel
    {
        public List<RelatedTopicViewModel> TopicList { get; set; }
        public string KeyWord { get; set; }
        public string SportName { get; set; }
        public string TeamName { get; set; }
        public string PlayerName { get; set; }
        public string GameName { get; set; }
        public int NewsClassID { get; set; }
        public long QuoteUniqueID1 { get; set; }
        public long? QuoteUniqueID2 { get; set; }
        public long? QuoteUniqueID3 { get; set; }
    }
    public class UserNewArticleViewModel
    {
        public string Title { get; set; }

        [AllowHtmlAttribute]
        public string PostContent { get; set; }
        public string DataURL { get; set; }
        public string ImageLink {get;set;}
        public string Nickname { get; set; }
        public DateTime PostDate { get; set; }
        public UserArticleRelatedTopicViewModel RelatedTopicsList { get; set; }
    }
}
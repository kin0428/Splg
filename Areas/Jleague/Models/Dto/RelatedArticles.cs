using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Constant;

namespace Splg.Areas.Jleague.Models.Dto
{
    public class RelatedArticles
    {
        public JlgConst.JType JLeagueType { get; set; }

        public List<RelatedArticle> Items { get; set; }
    }
}
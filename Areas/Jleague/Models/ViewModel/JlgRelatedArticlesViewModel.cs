using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Jleague.Models.Dto;
using Splg.Core.Constant;

namespace Splg.Areas.Jleague.Models.ViewModel
{
    public class JlgRelatedArticlesViewModel
    {
        public JlgConst.JType JLeagueType { get; set; }

        public int TargetHomeTeamId { get; set; }

        public int TargetAwayTeamId { get; set; }

        public JlgTeamSpec HomeTeamSpec { get; set; }

        public JlgTeamSpec AwayTeamSpec { get; set; }

        public RelatedArticles HomeRelatedArticles { get; set; }

        public RelatedArticles AwayRelatedArticles { get; set; }
    }

    


}
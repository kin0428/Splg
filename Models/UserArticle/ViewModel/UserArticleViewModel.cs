using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Models.News.ViewModel;
using Splg.Areas.Npb.Models.ViewModel;
using Splg.Areas.User.Models.InfoModel;
using Splg.Areas.Mlb.Models.ViewModels;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Areas.Jleague.Models;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;

namespace Splg.Models.UserArticle.ViewModel
{
    public class UserArticleInfoViewModel
    {
        public static string DefaultImageLink = @"~/Content/img/upload/contribution/dummy_2.png";

        public long ContributeId { get; set; }
        public long MemberId { get; set; }

        [Display(Name="タイトル")]
        [Required(ErrorMessage = AnnotationMessageConst.IsNotInputRequired)]
        [StringLength(50, ErrorMessage = AnnotationMessageConst.IsOverStringLength)]
        public string Title { get; set; }

        [Display(Name = "本文")]
        [Required(ErrorMessage = AnnotationMessageConst.IsNotInputRequired)]
        [StringLength(5000, ErrorMessage = AnnotationMessageConst.IsOverStringLength)]
        public string Body { get; set; }

        public string ImageLink { get; set; }

        public bool HasImageLink
        {
            get
            {
                if (String.IsNullOrEmpty(ImageLink))
                    return false;

                if (ImageLink.Equals(UserArticleInfoViewModel.DefaultImageLink))
                    return false;

                if (ImageLink.Equals("~/Content/img/upload/contribution/"))
                    return false;

                return true;
            }
        }

        public string OldImageLink { get; set; }
        public string ImageName { get; set; }
        public DateTime ContributeDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public long NewsClassId { get; set; }
        public short Status { get; set; }
        public long QuoteUniqueId1 { get; set; }
        public long QuoteUniqueId2 { get; set; }
        public long QuoteUniqueId3 { get; set; }
        public long TopicID { get; set; }
        public int SportID { get; set; }
        public string Nickname { get; set; }
        public string ProfileImage { get; set; }
        public List<RelatedTopicViewModel> RelatedTopicList { get; set; }
        public IEnumerable<PostedInfoViewModel> RelatedPostList { get; set; }
        public IEnumerable<PostedInfoViewModel> RightNewsestPosts { get; set; }
        //--------------------------------------------------------------------
        // NewsClassID = 1 => News post
        public NewsInfoViewModel NewsInfo { get; set; }
        //--------------------------------------------------------------------
        // NewsClassID = 2 => Team post
        public NpbTeamTopInfoViewModel NpbTeamInfo { get; set; }
        public MlbTeamInfoTeamTopViewModel MlbTeamInfo { get; set; }
        public JlgTeamInfoTopViewModel JlgTeamInfo { get; set; }
        public RankInfoRT RankInfoRT { get; set; } 
        //--------------------------------------------------------------------
        // NewsClassID = 3 => Player post
        public JlgPlayerInfoYear PlayerInfoYear { get; set; }
        public JlgPlayerInfoSum PlayerSum { get; set; } 
        // Folower member
        public FollowerMemberForUser FollowedUserInfo { get; set; }
        public string DataURL { get; set; }
    }
    public class UserArticleViewModel
    {
        public IEnumerable<UserArticleInfoViewModel> ArticleList { get; set; }
    }
}
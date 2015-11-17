using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models;
using Splg.Models.News;

namespace Splg.Models.News.ViewModel
{
    public class PostedInfoViewModel
    {
        public int Views { get; set; }
        public int PostYear { get; set; }
        public int PostMonth { get; set; }
        public int SportID { get; set; }
        public int TopicID { get; set; }
        public long ContributeId { get; set; }
        public DateTime? ContributeDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Title { get; set; }
        public string ContributedPicture { get; set; }
        public string Body { get; set; }
        public long MemberId { get; set; }
        public string Nickname { get; set; }
        public string ProfileImg { get; set; }
		public long NewsClassId { get; set; }
		public long QuoteUniqueId1 { get; set; }
		public long QuoteUniqueId2 { get; set; }
		public long QuoteUniqueId3 { get; set; }
        public short Status { get; set; }

        // if Status == 1 and NewsClassId == 1 => display news
        public string HeadLine { get; set; }
        public string SentFrom { get; set; }
        //=======================================================================
        // if Status == 1 and NewsClassId == 2 => display NPB/MLB/J-League information
        public string NpbShortNameLeague { get; set; }
        public string NpbTeam { get; set; }
        //----------------------------------------
        // add more information for J-League
        public string JlgLeagueNameS { get; set; }
        public string JlgTeamName { get; set; }
        //----------------------------------------
        // add more information for MLB
        public string MlbLeagueName { get; set; }
        public string MlbTeamName { get; set; }
        //======================================================================
        // if Status == 1 and NewsClassId == 3 => display NPB/MLB/J-League information
        public string NpbPlayer { get; set; }
        //----------------------------------------
        // add more information for MLB/J-League
        public string JlgPlayerName { get; set; }
        //======================================================================
        // if Status == 1 and NewsClassId == 4 => display NPB/MLB/J-League information
        public int? NpbGameDate { get; set; }
        public string NpbHomeTeamName { get; set; }
        public string NpbVisitorTeamName { get; set; }
        //----------------------------------------
        // add more information for J-League
        public int? JlgGameDate { get; set; }
        public string JlgHomeTeamName { get; set; }
        public string JlgAwayTeamName { get; set; }
        //----------------------------------------
        // add more information for MLB
        public int? MlbGameDate { get; set; }
        public string MlbHomeTeamName { get; set; }
        public string MlbAwayTeamName { get; set; }
        //======================================================================
        // if Status == 1 and NewsClassId == 5 => display NPB/MLB/J-League information
        // Data Applied above
        //======================================================================
        // if Status == 1 and NewsClassId == 6 => Need for spec
        //...... Need more spec
        //======================================================================
        public short? ReleVantYear { get; set; }
        public short? ReleVantMonth { get; set; }
        public int? ExpectNumber { get; set; }
        public short? CorrectPercent { get; set; }
        public int? CorrectPoint { get; set; }
    }


    class PostedInfoViewModelSimpleComparer : IEqualityComparer<PostedInfoViewModel>
    {
        public bool Equals(PostedInfoViewModel x, PostedInfoViewModel y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.ContributeId == y.ContributeId;
        }

        public int GetHashCode(PostedInfoViewModel postedInfo)
        {
            if (Object.ReferenceEquals(postedInfo, null)) return 0;
            int hashPostedInfoName = postedInfo.Title.GetHashCode();
            int hashPostedInfoCode = postedInfo.ContributeId.GetHashCode();
            return hashPostedInfoName ^ hashPostedInfoCode;
        } 
    }


    class PostedInfoViewModelComparer : IEqualityComparer<PostedInfoViewModel>
    {
        public bool Equals(PostedInfoViewModel x, PostedInfoViewModel y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return (
                x.SportID == y.SportID
                && x.TopicID == y.TopicID
                && x.ContributeId == y.ContributeId
                && x.ContributeDate == y.ContributeDate
                && x.Title == y.Title
                && x.Body == y.Body
                && x.MemberId == y.MemberId
                && x.Nickname == y.Nickname
                && x.ProfileImg == y.ProfileImg
                && x.NewsClassId == y.NewsClassId
                && x.QuoteUniqueId1 == y.QuoteUniqueId1
                && x.QuoteUniqueId2 == y.QuoteUniqueId2
                && x.QuoteUniqueId3 == y.QuoteUniqueId3
                && x.Status == y.Status
                && x.HeadLine == y.HeadLine
                && x.SentFrom == y.SentFrom
                && x.NpbShortNameLeague == y.NpbShortNameLeague
                && x.NpbTeam == y.NpbTeam
                && x.JlgLeagueNameS == y.JlgLeagueNameS
                && x.JlgTeamName == y.JlgTeamName
                && x.MlbLeagueName == y.MlbLeagueName
                && x.MlbTeamName == y.MlbTeamName
                && x.NpbPlayer == y.NpbPlayer
                && x.JlgPlayerName == y.JlgPlayerName
                && x.NpbGameDate == y.NpbGameDate
                && x.NpbHomeTeamName == y.NpbHomeTeamName
                && x.NpbVisitorTeamName == y.NpbVisitorTeamName
                && x.ReleVantYear == y.ReleVantYear
                && x.ReleVantMonth == y.ReleVantMonth
                && x.ExpectNumber == y.ExpectNumber
                && x.CorrectPercent == y.CorrectPercent
                && x.CorrectPoint == y.CorrectPoint
                && x.JlgGameDate == y.JlgGameDate
                && x.JlgHomeTeamName == y.JlgHomeTeamName
                && x.JlgAwayTeamName == y.JlgAwayTeamName
                && x.MlbGameDate == y.MlbGameDate
                && x.MlbHomeTeamName == y.MlbHomeTeamName
                && x.MlbAwayTeamName == y.MlbAwayTeamName
                );
        }
        public int GetHashCode(PostedInfoViewModel postedInfo)
        {
            if (Object.ReferenceEquals(postedInfo, null)) return 0;
            int hashPostedInfoName = postedInfo.Title.GetHashCode();
            int hashPostedInfoCode = postedInfo.ContributeId.GetHashCode();
            return hashPostedInfoName ^ hashPostedInfoCode;
        } 

    }
}
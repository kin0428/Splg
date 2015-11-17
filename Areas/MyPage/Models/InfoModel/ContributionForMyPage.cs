using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.MyPage.Models.InfoModel
{
    public class ContributionForMyPage
    {
        public Int64 ContributeId { get; set; }
        public Int64 MemberId { get; set; }
        public string Title { get; set; }
        public string TopicTitles { get; set; }
        public string Body { get; set; }
        public string ContributedPicture { get; set; }

        public Nullable<System.DateTime> ContributeDate { get; set; }

        public string FormattedContributeDateTime
        {
            get
            {
                string result = null;
                if (ContributeDate != null)
                    result = Utils.FormatDateTime(ContributeDate.Value);
                return result;
            }
        }

        public string FormattedContributeDate
        {
            get
            {
                string result = null;
                if (ContributeDate != null)
                    result = Utils.FormatDate(ContributeDate.Value);
                return result;
            }
        }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string FormattedModifiedDateTime
        {
            get
            {
                string result = null;
                if (ModifiedDate != null)
                    result = Utils.FormatDateTime(ModifiedDate.Value);
                return result;
            }
        }
    }
}
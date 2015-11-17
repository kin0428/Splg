﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.User.Models.InfoModel
{
    public class NoticeInfoForUser
    {
        public long NoticeId { get; set; }
        public short NoticeClass { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public System.DateTime DeliveryTime { get; set; }
        public System.DateTime NoticeDisplayEndTime { get; set; }
        public string TransitionsURL { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public Nullable<short> MailSendStatus { get; set; }
        public string CreatedAccountID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedAccountID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
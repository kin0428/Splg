using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.MyPage.Models.InfoModel
{
    public class NoticeInfoForMyPage
    {

        //0・・普通（関心無）、１・・スポーツ、２・・チーム、３・・選手、４・・試合、5・・リーグ、6・・フォロー、7・・グループ、
        //8・・原資ポイント付与、9・・ポイント精算	
        public const int CLS_NONE = 0;
        public const int CLS_SPORTS = 1;
        public const int CLS_TEAM = 2;
        public const int CLS_PLAYER = 3;
        public const int CLS_GAME = 4;
        public const int CLS_LEAGUE = 5;
        public const int CLS_FOLLOW = 6;
        public const int CLS_GROUP = 7;
        public const int CLS_POINT_GIVE = 8;
        public const int CLS_POINT_PAYOFF = 9;

        public const string KW_NICKNAME = "Nickname";
        public const string KW_DATE = "Date";
        public const string KW_HHMM = "hh:mm";

        public const string KW_FOLLOW_FOLLOWMEMBERID = "FollowMemberID"; //この順番で置き換え
        public const string KW_FOLLOW_FOLLOWER = "Follower";
        public const string KW_FOLLOW_FOLLOW = "Follow";

        public const string KW_GROUP_ADD = "AddGroup";
        public const string KW_GROUP_ID = "GroupID";
        public const string KW_GROUP_GRROUP = "Group";

        public const string KW_GAME_HOMETEAM = "HomeTeam";
        public const string KW_GAME_VISITORTEAM = "VisitorTeam";
        public const string KW_GAME_PLACE = "Place";
        public const string KW_GAME_ROUND = "Round";
        public const string KW_GAME_SPORT = "Sport";
        public const string KW_GAME_ID = "GameID";

        public const string KW_MONTH = "Month";
        public const string KW_WEEK = "Week";
        public const string KW_POINTS = "Points";

        public short ClassClass { get; set; }
        public int UniqueID { get; set; }
        public int? UniqueID2 { get; set; }
        public int? UniqueID3 { get; set; }


        public long NoticeId { get; set; }
        public int NoticeDeliverySubjectId { get; set; }
        public int MemberId { get; set; }
        public bool AlreadyReadFlg { get; set; }
        public short NoticeClass { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string NoticeBody { get; set; }

        public string Nickname { get; set; }

        public string AddGroup { get; set; }
        public string GroupID { get; set; }
        public string Group { get; set; }

        public string LeagueName { get; set; }
        public string HomeTeam { get; set; }
        public string HomeTeamS { get; set; }
        public string Place { get; set; }
        public string VisitorTeam { get; set; }
        public string VisitorTeamS { get; set; }
        public string Date { get; set; }
        public string Round { get; set; }

        public string GameID { get; set; }

        public string Follower { get; set; }
        public string Follow { get; set; }

        public int Month { get; set; }
        public int Week { get; set; }
        public int Points { get; set; }

        public void setTitle(int classclass,string  title)
        {
            if (string.IsNullOrEmpty(title))
                return;
            Title = GetReplacedString(classclass, title);
        }


        public void setBody(int classclass, string body)
        {
            if (string.IsNullOrEmpty(body))
                return;

            Body= GetReplacedString(classclass, body);

        }

        public void setNoticeBody(int classclass, string noticeBody)
        {
            if (string.IsNullOrEmpty(noticeBody))
                return;

            NoticeBody = GetReplacedString(classclass, noticeBody);

        }

        public System.DateTime DeliveryTime { get; set; }

        public string FormattedDeliveryTime
        {
            get
            {
                string result = null;
                if (DeliveryTime != null)
                    result = Utils.FormatDateTime(DeliveryTime);

                return result;
            }
        }

        public System.DateTime NoticeDisplayEndTime { get; set; }

        private string transitionsURL;
        public string TransitionsURL
        {
            get
            {
                if (String.IsNullOrEmpty(transitionsURL))
                {
                    transitionsURL = "#";
                }
                return transitionsURL;
            }
            set { transitionsURL = value; }
        }


        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public Nullable<short> MailSendStatus { get; set; }
        public string CreatedAccountID { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }


        public string FormattedCreatedDate
        {
            get
            {
                string result = null;
                if (CreatedDate != null)
                    result = Utils.FormatDateTime((DateTime)CreatedDate);

                return result;
            }
        }


        public string ModifiedAccountID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        private string GetReplacedString(int classclass, string result)
        {
            result = result.Replace(KW_NICKNAME, Nickname);
            result = result.Replace(KW_HHMM, FormattedCreatedDate);

            switch (classclass)
            {
                case CLS_SPORTS:
                    break;
                case CLS_TEAM:
                    break;
                case CLS_PLAYER:
                    break;
                case CLS_GAME:
                    result = result.Replace(KW_GAME_HOMETEAM, HomeTeamS);
                    result = result.Replace(KW_GAME_VISITORTEAM, VisitorTeamS);
                    result = result.Replace(KW_GAME_PLACE, Place);
                    result = result.Replace(KW_DATE, Date);
                    result = result.Replace(KW_GAME_SPORT, LeagueName);
                    result = result.Replace(KW_GAME_ROUND, Round);
                    result = result.Replace(KW_GAME_ID, GameID);
                    break;
                case CLS_LEAGUE:
                    break;
                case CLS_FOLLOW:
                    result = result.Replace(KW_FOLLOW_FOLLOWMEMBERID, MemberId.ToString());
                    result = result.Replace(KW_FOLLOW_FOLLOWER, Follower);
                    result = result.Replace(KW_FOLLOW_FOLLOW, Follow);
                    break;
                case CLS_GROUP:
                    result = result.Replace(KW_GROUP_ADD, AddGroup);
                    result = result.Replace(KW_GROUP_ID, GroupID);
                    result = result.Replace(KW_GROUP_GRROUP, Group);
                    break;
                case CLS_POINT_GIVE:
                    result = result.Replace(KW_MONTH, Month.ToString());
                    result = result.Replace(KW_WEEK, Week.ToString());
                    result = result.Replace(KW_POINTS, Points.ToString());
                    break;
                case CLS_POINT_PAYOFF:
                    result = result.Replace(KW_MONTH, Month.ToString());
                    result = result.Replace(KW_WEEK, Week.ToString());
                    break;
            }
            return result;
        }

    }
}
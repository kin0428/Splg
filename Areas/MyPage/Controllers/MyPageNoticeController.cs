#region (c) 2015 Prime Labo - All rights reserved
/*                                      COPYRIGHT NOTICE
 * -------------------------------------------------------------------------------------
 * All materials (including but not limited to source code, compiled assemblies, images,
 * resources, etc.) are copyrighted to Prime Labo. No usage is allowed unless permitted 
 * by written consent. You may not use, reverse-engineer these materials under any
 * circumstances.
 * 
 *                                    PROJECT DESCRIPTION
 * -------------------------------------------------------------------------------------
 * Namespace	: Splg.Areas.MyPage.Controllers
 * Class		: MyPageTopController
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using Splg.Areas.MyPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Controllers;
using Splg.Models;
using Splg.Areas.MyPage.Models.InfoModel;
using System.Web.UI;
using Splg.Models.Game.ViewModel;
using Splg.Areas.MyPage.Models.ViewModel;

using Splg.Areas.Jleague.Models;
using Splg.Areas.Npb.Models;
using Splg.Areas.Mlb.Models;
#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageNoticeController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //MyPageEntities mypage = new MyPageEntities();


        #endregion

        /// <summary>
        /// GET: /mypage/notice/
        /// </summary>
        public ActionResult Index()
        {

            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定(フォーム認証部分も本来なら見るべき。)
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            long memberId = 0;

            object currentUser = Session["CurrentUser"];
            if (currentUser != null)
                memberId = Convert.ToInt64(currentUser.ToString());

            MyPageNoticeViewModel viewModel = new MyPageNoticeViewModel();

            Member member = Utils.GetMember(memberId);
            viewModel.Nickname = member.Nickname;

            //運営からのお知らせ
            //SetManagementNotices(viewModel, 0, MyPageNoticeViewModel.MANAGEMENT_NOTICE_INITIAL_SIZE);

            //ユーザーへのお知らせ
            //SetUserNotices(viewModel, 0, MyPageNoticeViewModel.USER_NOTICE_INITIAL_SIZE);

            return View(viewModel);
        }


        /// <summary>
        /// 運営からのお知らせのもっと見る取得
        /// </summary>
        /// <param name="managementnoticecount">現在表示しているレコード件数</param>
        /// <returns>Json形式のActionResult</returns>
        public ActionResult GetMoreManagementNotices(int managementnoticecount)
        {
            MyPageNoticeViewModel viewModel = new MyPageNoticeViewModel();

            //2015/5/21のリリース対応
            return Json(viewModel, JsonRequestBehavior.AllowGet);

            //運営からのお知らせ
            if (managementnoticecount == 0)
                SetManagementNotices(viewModel, managementnoticecount, 3);
            else
                SetManagementNotices(viewModel, managementnoticecount, MyPageNoticeViewModel.INITIAL_PAGE_SIZE);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ○○さんへのお知らせのもっと見る取得
        /// </summary>
        /// <param name="usernoticecount">現在表示しているレコード件数</param>
        /// <returns>Json形式のActionResult</returns>
        public ActionResult GetMoreUserNotices(int usernoticecount)
        {
            MyPageNoticeViewModel viewModel = new MyPageNoticeViewModel();

            //ユーザーへのお知らせ
            if (usernoticecount == 0)
                SetUserNotices(viewModel, usernoticecount, MyPageNoticeViewModel.INITIAL_PAGE_SIZE);
            else
                SetUserNotices(viewModel, usernoticecount, MyPageNoticeViewModel.INITIAL_PAGE_SIZE);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 運営からのお知らせをDBから取得する処理
        /// </summary>
        /// <param name="pageSize">取得するレコード件数</param>
        /// <returns>NoticeInfoのIEnumerableオブジェクト</returns>
        private IEnumerable<NoticeInfoForMyPage> SetManagementNotices(MyPageNoticeViewModel viewModel, int skipCount, int retrieveCount)
        {
            IEnumerable<NoticeInfoForMyPage> result = new List<NoticeInfoForMyPage> { };

            long memberId = 0;

            object currentUser = Session["CurrentUser"];
            if (currentUser != null)
                memberId = Convert.ToInt64(currentUser.ToString());

            try
            {

                if (memberId > 0)
                {
                    //CreatedDateを一か月みる。CreatedDateはレコードを作成するときは必ず入れる。
                    DateTime dt = DateTime.Now.AddMonths(-1);

                    //NoticeInfoテーブルから条件 現在日時 < 通知表示終了日時(NoticeDisplayEndTime) のものを配信日時の新しい順

                    //              通知表示終了日時(NoticeDisplayEndTime) は本来NoticeDeliverySubject
                    //              当面は上記CreatedDateで判定する

                    //かつ、NoticeClassフィールドが1か3、
                    //かつNoticeDeliverySubjectにレコードが存在しないものを3件表示する
                    //現在の取得件数をhtmlにdata-managementnoticecount属性として保存する
                    //例：<ul data-managementnoticecount="3">

                    var lines = from n in com.NoticeInfo
                                from d in com.NoticeDeliverySubject.Where(x => x.NoticeId == n.NoticeId).DefaultIfEmpty()
                                where d.CreatedDate > dt
                                && (n.NoticeClass == 1 || n.NoticeClass == 3)
                                && d == null
                                orderby d.CreatedDate descending
                                select new NoticeInfoForMyPage
                                {
                                    AlreadyReadFlg = true,  //NoticeDeliverySubjectが存在しないためtrueにセット
                                    NoticeId = n.NoticeId,
                                    NoticeClass = n.NoticeClass,
                                    //TODO NoticeBody = n.NoticeBody,
                                    Title = n.Title,
                                    Body = n.Body,
                                    DeliveryTime = n.DeliveryTime, //将来NoticeDeliverySubjectに移動（未使用）
                                    NoticeDisplayEndTime = n.NoticeDisplayEndTime, //将来NoticeDeliverySubjectに移動（未使用）
                                    TransitionsURL = n.TransitionsURL,　
                                    MailCC = n.MailCC,
                                    MailBCC = n.MailBCC,
                                    MailSendStatus = n.MailSendStatus,
                                    CreatedAccountID = n.CreatedAccountID,
                                    CreatedDate = n.CreatedDate,
                                    ModifiedAccountID = n.ModifiedAccountID,
                                    ModifiedDate = n.ModifiedDate,


                                };

                    viewModel.ManagementNoticeTotalCount = lines.Count();
                    viewModel.ManagementNotices = lines.Skip(skipCount).Take(retrieveCount);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        /// <summary>
        /// ユーザーへのお知らせをDBから取得する処理
        /// </summary>
        /// <param name="pageSize">1ページあたりのレコード件数</param>
        /// <returns>NoticeInfoのIEnumerableオブジェクト</returns>
        private IEnumerable<NoticeInfoForMyPage> SetUserNotices(MyPageNoticeViewModel viewModel, int skipCount, int retrieveCount)
        {
            IEnumerable<NoticeInfoForMyPage> result = new List<NoticeInfoForMyPage> { };
            List<NoticeInfoForMyPage> result2 = new List<NoticeInfoForMyPage> { };

            long loginMemberId = 0;

            object currentUser = Session["CurrentUser"];
            if (currentUser != null)
                loginMemberId = Convert.ToInt64(currentUser.ToString());

            try
            {

                if (loginMemberId > 0)
                {
                    //CreatedDateを一か月みる。CreatedDateはレコードを作成するときは必ず入れる。
                    DateTime dt = DateTime.Now.AddMonths(-1);

                    //NoticeDeliverySubjectテーブルからセッションから取得した自分のMemberIdで検索しリストを取得する。
                    //NoticeInfoテーブルを上記で取得したレコードで お知らせID(NoticeID)が同一 
                    //かつ 現在日時 < 通知表示終了日時(NoticeDisplayEndTime)

                    //              通知表示終了日時(NoticeDisplayEndTime) は本来NoticeDeliverySubject
                    //              当面は上記CreatedDateで判定する

                    //かつ NoticeClassが1か3のものを10件取得
                    //現在の取得件数をhtmlにdata-usernoticecount属性として保存する
                    //例：<ul data-usernoticecount="10">
                    //var lines = from n in com.NoticeInfo
                    //            join d in com.NoticeDeliverySubject on n.NoticeId equals d.NoticeId
                    //            where d.CreatedDate >= dt
                    //            && (n.NoticeClass == 1 || n.NoticeClass == 3)
                    //            && d.MemberId == loginMemberId
                    //            //&& n.NoticeId <= 4  //PointsPtが付与の仕様確定待ちのため暫定で絞る
                    //            orderby d.CreatedDate descending
                    //            select new NoticeInfoForMyPage
                    //            {
                    //                NoticeId = n.NoticeId,
                    //                NoticeClass = n.NoticeClass,
                    //                NoticeDeliverySubjectId = d.NoticeDeliverySubjectId,
                    //                ClassClass = d.ClassClass,
                    //                MemberId = loginMemberId,
                    //                UniqueID = d.UniqueID,
                    //                UniqueID2 = d.UniqueID2,
                    //                //UniqueID3 = d.UniqueID3,
                    //                AlreadyReadFlg = d.AlreadyReadFlg,
                    //                Title = n.Title,
                    //                Body = n.Body,
                    //                //NoticeBody = n.NoticeBody,
                    //                DeliveryTime = n.DeliveryTime,  //将来NoticeDeliverySubjectに移動（未使用）
                    //                NoticeDisplayEndTime = n.NoticeDisplayEndTime, //将来NoticeDeliverySubjectに移動（未使用）
                    //                TransitionsURL = n.TransitionsURL, 
                    //                MailCC = n.MailCC,
                    //                MailBCC = n.MailBCC,
                    //                MailSendStatus = n.MailSendStatus,
                    //                CreatedAccountID = d.CreatedAccountID,
                    //                CreatedDate = d.CreatedDate,
                    //                ModifiedAccountID = d.ModifiedAccountID,
                    //                ModifiedDate = d.ModifiedDate

                    //            };


                    //ScheduleInfo 試合スケジュール_試合情報はHomeTeamIDがすでにわかっているので不要？
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("SELECT ");
                    sb.Append("n.NoticeId,n.NoticeClass,d.NoticeDeliverySubjectId,d.ClassClass,d.MemberId,d.UniqueID,d.UniqueID2,d.UniqueID3,d.AlreadyReadFlg, ");
                    sb.Append(" n.Title,n.Body,n.NoticeBody, n.DeliveryTime,n.NoticeDisplayEndTime,n.TransitionsURL,n.MailCC,n.MailBCC, n.MailSendStatus,d.CreatedAccountID, d.CreatedDate,  d.ModifiedAccountID, d.ModifiedDate ");
                    sb.Append("FROM "); 
                    sb.Append("[SPLG].[COM].[NOTICEINFO] n ");
                    sb.Append(" JOIN [SPLG].[COM].[NOTICEDELIVERYSUBJECT] d ");
                    sb.Append("ON n.NOTICEID = d.NOTICEID ");
                    sb.Append("WHERE ");
                    sb.Append("d.CreatedDate >= '" + dt + "' ");
                    sb.Append("AND ");
                    sb.Append("(n.NoticeClass = 1 OR n.NoticeClass = 3) ");
                    sb.Append("AND ");
                    sb.Append("d.MemberId = " + loginMemberId + " ");
                    sb.Append("ORDER BY ");
                    sb.Append("d.CreatedDate DESC ");

                    string query = sb.ToString();

                    var lines = com.Database.SqlQuery<NoticeInfoForMyPage>(@query).ToList<NoticeInfoForMyPage>();

                    foreach (NoticeInfoForMyPage n in lines)
                    {
                        switch (n.ClassClass)
                        {
                            case NoticeInfoForMyPage.CLS_SPORTS:
                                break;
                            case NoticeInfoForMyPage.CLS_TEAM:
                                break;
                            case NoticeInfoForMyPage.CLS_PLAYER:
                                break;
                            case NoticeInfoForMyPage.CLS_GAME:
                                setGameInfo(n);
                                break;
                            case NoticeInfoForMyPage.CLS_LEAGUE:
                                break;
                            case NoticeInfoForMyPage.CLS_FOLLOW:
                                setFollowInfo(n);
                                break;
                            case NoticeInfoForMyPage.CLS_GROUP:
                                setGroupInfo(n);
                                break;
                            case NoticeInfoForMyPage.CLS_POINT_GIVE:
                                setPointGiveInfo(n);
                                break;
                            case NoticeInfoForMyPage.CLS_POINT_PAYOFF:
                                setPointPayoffInfo(n);
                                break;

                        }

                        n.Nickname = (from gm in com.Member where gm.MemberId == n.MemberId select gm.Nickname).FirstOrDefault();

                        n.setTitle(n.ClassClass, n.Title);
                        n.setNoticeBody(n.ClassClass, n.NoticeBody);
                        n.setBody(n.ClassClass, n.Body);

                        result2.Add(n);

                    }

                    viewModel.UserNoticeTotalCount = lines.Count();
                    viewModel.UserNotices = result2.Skip(skipCount).Take(retrieveCount);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 予想した試合が開始しました！　hh:mm
        /// 予想した試合が終了しました！ hh:mm
        /// </summary>
        /// <param name="notice"></param>
        private void setGameInfo(NoticeInfoForMyPage notice){

            int sportsId = notice.UniqueID;
            int gameId = (int)notice.UniqueID2;

            GameInfoViewModel game = NpbCommon.GetGameInfoByGameID(sportsId,gameId);

            string sportsLetter = "";
            if(sportsId == Constants.NPB_SPORT_ID)
                sportsLetter = "npb";
            if(sportsId == Constants.MLB_SPORT_ID)
                sportsLetter = "mlb";
            if(sportsId == Constants.JLG_SPORT_ID)
                sportsLetter = "jleague";

            notice.TransitionsURL = "/" + sportsLetter + "/game/" + gameId + "/";
            notice.HomeTeam = game.HomeTeamName;
            notice.HomeTeamS = game.HomeTeamNameS;
            notice.VisitorTeam = game.VisitorTeamName;
            notice.VisitorTeamS = game.VisitorTeamNameS;
            notice.Place = game.StadiumName;

            if (sportsId == Constants.JLG_SPORT_ID)
            {
                Splg.Services.Game.JlgService jlsService = new Splg.Services.Game.JlgService();
                int occasionNo = jlsService.GetOccasionNo(game.GameDate, game.GameKindID);

                string leagueName = "";
                string periodStr = "";

                switch (game.GameKindID)
                {
                    case Splg.Areas.Jleague.JlgConstants.JLG_GAMEKIND_J1:
                        leagueName = "J１リーグ";
                        if (game.Round == 1)
                        {
                            periodStr = "1stステージ ";
                        }
                        else
                        {
                            periodStr = "2ndステージ ";
                        }

                        periodStr += "第" + occasionNo + "節";
                        break;
                    case Splg.Areas.Jleague.JlgConstants.JLG_GAMEKIND_J2:
                        leagueName = "J２リーグ";
                        periodStr = "第" + occasionNo + "節";
                        break;
                    case Splg.Areas.Jleague.JlgConstants.JLG_GAMEKIND_NABISCO:
                        leagueName = "ナビスコC";
                        if (game.Round == 1)
                        {
                            periodStr = "予選リーグ ";
                        }
                        else
                        {
                            periodStr = "決勝リーグ ";
                        }

                        periodStr += "第" + occasionNo + "節";
                        break;

                }
                notice.LeagueName = leagueName;
                notice.Round = periodStr;
            }
            else
            {
                notice.Round = game.Round.ToString();
            }

            notice.Date = game.GameDate.ToString();
        }

        private void setFollowInfo(NoticeInfoForMyPage notice){

            notice.TransitionsURL = "/user/" + notice.UniqueID + "/";
            notice.Follower = (from m in com.Member where m.MemberId == notice.UniqueID select m.Nickname).FirstOrDefault();
            notice.Follow = (from m in com.Member where m.MemberId == notice.MemberId select m.Nickname).FirstOrDefault();
        }

        private void setGroupInfo(NoticeInfoForMyPage notice)
        {
            notice.TransitionsURL = "/mypage/group/" + notice.UniqueID2 + "/detail";
            notice.Group = (from g in com.Groups where g.GroupID == notice.UniqueID2 select g.GroupName).FirstOrDefault();
            notice.AddGroup = (from gm in com.Member where gm.MemberId == notice.UniqueID select gm.Nickname).FirstOrDefault();
        }

        /// <summary>
        /// ポイント付与
        /// </summary>
        /// <param name="notice"></param>
        private void setPointGiveInfo(NoticeInfoForMyPage notice)
        {
            int month = notice.UniqueID;        //対象月
            int week = (int)notice.UniqueID2;   //対象週
            int points = (int)notice.UniqueID3;   //付与されたポイント

            notice.TransitionsURL = "/mypage/";
            notice.Month = month;
            notice.Week = week;
            notice.Points = points;
        }

        /// <summary>
        /// ポイントが精算された場合
        /// </summary>
        /// <param name="notice"></param>
        private void setPointPayoffInfo(NoticeInfoForMyPage notice)
        {
            int month = notice.UniqueID;        //対象月
            int week = (int)notice.UniqueID2;   //対象週

            notice.TransitionsURL = "/mypage/";
            notice.Month = month;
            notice.Week = week;
        }
        /// <summary>
        /// お知らせを既読にする処理
        /// </summary>
        /// <param name="managementnoticecount">現在表示しているレコード件数</param>
        /// <returns>Json形式のActionResult</returns>
        public ActionResult SetNoticeAlreadyRead(int noticeDeliverySubjectId)
        {
            MyPageJsonResultModel viewModel = new MyPageJsonResultModel();

            //お知らせを既読にする

            long memberId = 0;

            object currentUser = Session["CurrentUser"];
            if (currentUser != null && !String.IsNullOrEmpty(currentUser.ToString()))
                memberId = Convert.ToInt64(currentUser.ToString());

            if (memberId > 0)
            {
                using (var context = com.Database.BeginTransaction())
                {
                    try
                    {
                        var noticeDSubject = (from d in com.NoticeDeliverySubject
                                              where
                                               d.NoticeDeliverySubjectId == (long)noticeDeliverySubjectId && d.MemberId == memberId
                                              select d).FirstOrDefault();

                        // 既読フラグを立てる
                        if (noticeDSubject != null)
                        {
                            noticeDSubject.AlreadyReadFlg = MyPageConstant.ALREADY_READ;
                            // コミット
                            com.SaveChanges();
                            context.Commit();
                            // コミット後に件数を更新
                            int count = Utils.GetNumberOfUnreadNotice((int)memberId);
                            viewModel.Message = count.ToString();
                        }
                    }

                    catch (Exception ex)
                    {
                        context.Rollback();
                        viewModel.HasError = true;
                        throw ex;
                    }
                }
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

    }

}
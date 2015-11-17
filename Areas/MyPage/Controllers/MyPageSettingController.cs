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
using Splg.Areas.Jleague.Models;
using Splg.Areas.Mlb.Models;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.Npb.Models;
using Splg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageSettingController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();

        NpbEntities npb = new NpbEntities();
        JlgEntities jlg = new JlgEntities();
        MlbEntities mlb = new MlbEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //MyPageEntities mypage = new MyPageEntities();
        #endregion

        /// <summary>
        /// GET: /mypage/setting/
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定(フォーム認証部分も本来なら見るべき。)
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            var viewModel = new MyPageSettingViewModel();

            Int64 memberId = GetMemberID();

            viewModel.MemberSettingInfo = GetMemberSettingInfo(memberId);
            viewModel.PrefecturesInfo = GetPrefecturesInfo();
            viewModel.SportsInfo = GetSportsInfo();
            viewModel.SendMailInfo = GetSendMailInfo(memberId);

            return View(viewModel);
        }

        #region member情報の取得
        private IEnumerable<MyPageSettingViewModel.MemberSettingInfoModel> GetMemberSettingInfo(Int64 memberId)
        {
            List<MyPageSettingViewModel.MemberSettingInfoModel> result = new List<MyPageSettingViewModel.MemberSettingInfoModel> { };

            if (memberId > 0)
            {

                MyPageSettingViewModel.MemberSettingInfoModel mbi = new MyPageSettingViewModel.MemberSettingInfoModel();
                List<MyPageSettingViewModel.PrefecturesInfoModel> pfis = new List<MyPageSettingViewModel.PrefecturesInfoModel> { };

                #region メンバー情報

                var member = (from m in com.Member
                              where m.MemberId == memberId
                              select m).FirstOrDefault();
                if (member != null)
                {
                    mbi.MemberID = Convert.ToInt64(member.MemberId);
                    mbi.Nickname = member.Nickname;
                    mbi.ProfileImg = member.ProfileImg;
                    mbi.Gender = Convert.ToInt16(member.Gender); // 1：男性、2：女性、3：その他
                    if (mbi.Gender == 1) mbi.GenderCheckedMale = "checked";
                    if (mbi.Gender == 2) mbi.GenderCheckedFeMale = "checked";
                    mbi.BirthdayYear = (short)Convert.ToInt16(member.BirthdayYear);
                    mbi.BirthdayMonth = (short)Convert.ToInt16(member.BirthdayMonth);
                    mbi.BirthdayDay = (short)Convert.ToInt16(member.BirthdayDay);
                    mbi.PrefecturesID = (short)Convert.ToInt16(member.PrefecturesId);
                    mbi.Mail = member.Mail;

                }
                #endregion

                #region 好きなスポーツ
                mbi.LikeSportsID = new List<int>();

                var sports = (from s in com.LikeSports
                              join sm in com.SportsMaster
                              on s.SportsID equals sm.SportsID
                              where s.MemberID == member.MemberId
                              select sm);
                foreach (var sp in sports)
                {
                    mbi.LikeSportsID.Add(sp.SportsID);
                }
                #endregion

                #region 好きなチーム
                mbi.LikeTeam = new List<MyPageSettingViewModel.TeamInfoModel> { };
                var teams = (from t in com.LikeTeam
                             where t.MemberID == member.MemberId
                             select t);
                foreach (var tid in teams)
                {
                    MyPageSettingViewModel.TeamInfoModel ti = new MyPageSettingViewModel.TeamInfoModel();
                    ti.SportsID = tid.SportsID;
                    ti.TeamID = tid.TeamID;
                    ti.TeamName = GetTeamName(ti.SportsID, ti.TeamID);
                    mbi.LikeTeam.Add(ti);
                }
                #endregion

                result.Add(mbi);
            }

            return result;
        }
        #endregion

        #region 出身地情報の取得
        private IEnumerable<MyPageSettingViewModel.PrefecturesInfoModel> GetPrefecturesInfo()
        {
            List<MyPageSettingViewModel.PrefecturesInfoModel> result = new List<MyPageSettingViewModel.PrefecturesInfoModel> { };

            var prefecture = (from p in com.PrefectureMaster
                              orderby p.PrefecturesID
                              select p);
            if (prefecture != null)
            {
                foreach (var pi in prefecture)
                {
                    MyPageSettingViewModel.PrefecturesInfoModel pfi = new MyPageSettingViewModel.PrefecturesInfoModel();
                    pfi.PrefecturesID = pi.PrefecturesID;
                    pfi.PrefecturesName = pi.PrefecturesName;
                    result.Add(pfi);
                }
            }

            return result;
        }
        #endregion

        #region スポーツ情報の取得
        private IEnumerable<MyPageSettingViewModel.SportsInfoModel> GetSportsInfo()
        {
            List<MyPageSettingViewModel.SportsInfoModel> result = new List<MyPageSettingViewModel.SportsInfoModel> { };

            var Sport = (from p in com.SportsMaster
                         orderby p.SportsID
                         select p);
            if (Sport != null)
            {
                foreach (var pi in Sport)
                {
                    MyPageSettingViewModel.SportsInfoModel pfi = new MyPageSettingViewModel.SportsInfoModel();
                    pfi.SportsID = pi.SportsID;
                    pfi.SportsName = pi.SportsName;
                    pfi.Teams = new List<MyPageSettingViewModel.TeamInfoModel> { };
                    switch (pfi.SportsID)
                    {
                        case Constants.NPB_SPORT_ID:
                            // NPB
                            var nteam = (from tn in npb.TeamInfoMST
                                         where tn.LeagueID != 0
                                         orderby tn.TeamCD
                                         select tn);
                            foreach (var nt in nteam)
                            {
                                MyPageSettingViewModel.TeamInfoModel tim = new MyPageSettingViewModel.TeamInfoModel();
                                tim.SportsID = pfi.SportsID;
                                tim.TeamID = nt.TeamCD;
                                tim.TeamName = nt.Team;
                                pfi.Teams.Add(tim);
                            }
                            break;
                        case Constants.JLG_SPORT_ID:
                            // Jleague
                            var jteam = (from tj in jlg.TeamInfoGT
                                         join ct in jlg.CategoryGT
                                         on tj.CategoryGTId equals ct.CategoryGTId
                                         where
                                         (ct.GameKindID == 2 && ct.SeasonID == 2)   // J1
                                         ||
                                         (ct.GameKindID == 6)   // J2
                                         //||
                                         //(ct.GameKindID == 68)  // J3
                                         orderby ct.CategoryGTId, tj.TeamID
                                         select tj);
                            foreach (var nt in jteam)
                            {
                                MyPageSettingViewModel.TeamInfoModel tim = new MyPageSettingViewModel.TeamInfoModel();
                                tim.SportsID = pfi.SportsID;
                                tim.TeamID = nt.TeamID;
                                tim.TeamName = nt.TeamName;
                                pfi.Teams.Add(tim);
                            }
                            break;
                        case Constants.MLB_SPORT_ID:
                            // MLB
                            var mteam = (from tm in mlb.TeamInfo
                                         orderby tm.TeamID
                                         select tm);
                            foreach (var nt in mteam)
                            {
                                MyPageSettingViewModel.TeamInfoModel tim = new MyPageSettingViewModel.TeamInfoModel();
                                tim.SportsID = pfi.SportsID;
                                tim.TeamID = nt.TeamID;
                                tim.TeamName = nt.TeamName;
                                pfi.Teams.Add(tim);
                            }
                            break;
                        default:
                            break;
                    }
                    result.Add(pfi);
                }
            }

            return result;
        }
        #endregion

        #region チーム名の取得
        private string GetTeamName(int sport_id, int team_id)
        {
            string result = null;
            switch (sport_id)
            {
                case Constants.NPB_SPORT_ID:
                    // NPB
                    var nteam = (from tn in npb.TeamInfoMST
                                 where tn.TeamCD == team_id
                                 select tn).FirstOrDefault();
                    if (nteam != null)
                    {
                        result = nteam.Team;
                    }
                    break;
                case Constants.JLG_SPORT_ID:
                    // Jleague
                    var jteam = (from tj in jlg.TeamInfoTE
                                 where tj.TeamID == team_id
                                 select tj).FirstOrDefault();
                    if (jteam != null)
                    {
                        result = jteam.TeamName;
                    }
                    break;
                case Constants.MLB_SPORT_ID:
                    // MLB
                    var mteam = (from tm in mlb.TeamInfo
                                 where tm.TeamID == team_id
                                 select tm).FirstOrDefault();
                    if (mteam != null)
                    {
                        result = mteam.TeamName;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        #endregion

        #region メール設定情報の取得
        private IEnumerable<MyPageSettingViewModel.SendMailConditionInfoModel> GetSendMailInfo(long member_id)
        {
            List<MyPageSettingViewModel.SendMailConditionInfoModel> result = new List<MyPageSettingViewModel.SendMailConditionInfoModel> { };
            MyPageSettingViewModel.SendMailConditionInfoModel send = new MyPageSettingViewModel.SendMailConditionInfoModel();

            send.MailSendAtStart = 0;
            send.MailSendAtEnd = 0;

            var send_start = (from ms in com.MailDeliverCond
                              where ms.MemberID == member_id && ms.MailDelivCondID == 1
                              select ms).FirstOrDefault();
            if (send_start != null)
            {
                if (send_start.MailDelivCondID == 1)
                {
                    send.MailSendAtStart = 1;
                }
            }
            var send_end = (from ms in com.MailDeliverCond
                            where ms.MemberID == member_id && ms.MailDelivCondID == 2
                            select ms).FirstOrDefault();
            if (send_end != null)
            {
                if (send_end.MailDelivCondID == 2)
                {
                    send.MailSendAtEnd = 1;
                }
            }
            result.Add(send);
            return result;
        }
        #endregion

        #region memberIDの取得
        private Int64 GetMemberID()
        {
            Int64 memberId = -1;
            object currentUser = Session["CurrentUser"];

            if (currentUser != null)
                memberId = Convert.ToInt64(currentUser.ToString());

            return memberId;
        }
        #endregion

        #region Json Result

        #region Save Setting
        /// <summary>
        /// Save Setting
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult SaveSetting(MyPageSettingViewModel.MemberSettingInfoModel ViewModel)
        {
            Int64 memberID = GetMemberID();
            bool isResult = false;

            using (var dbContextTransaction = com.Database.BeginTransaction())
            {
                try
                {
                    if (memberID != null)
                    {
                        var update_member = (from m in com.Member
                                             where m.MemberId == memberID
                                             select m).FirstOrDefault();
                        update_member.MemberId = memberID;
                        update_member.Nickname = ViewModel.Nickname;
                        update_member.ProfileImg = ViewModel.ProfileImg;
                        update_member.Gender = ViewModel.Gender;
                        update_member.BirthdayYear = ViewModel.BirthdayYear;
                        update_member.BirthdayMonth = ViewModel.BirthdayMonth;
                        update_member.BirthdayDay = ViewModel.BirthdayDay;
                        update_member.Birthday = DateTime.Parse(ViewModel.BirthdayYear + "/" + ViewModel.BirthdayMonth + "/" + ViewModel.BirthdayDay);
                        update_member.PrefecturesId = ViewModel.PrefecturesID;
                        //com.SaveChanges();

                        var delQuery =
                        from ls in com.LikeSports
                        where ls.MemberID == memberID
                        select ls;

                        foreach (var ls in delQuery)
                        {
                            com.LikeSports.Remove(ls);
                        }

                        com.SaveChanges();


                        if (ViewModel.LikeSportsID != null)
                        {
                            for (var i = 0; i < ViewModel.LikeSportsID.Count; i++)
                            {
                                var newLikeSport = new LikeSports
                                {
                                    MemberID = memberID,
                                    SportsID = ViewModel.LikeSportsID[i],
                                    CreatedAccountID = memberID.ToString(),
                                    CreatedDate = DateTime.Now
                                };
                                com.LikeSports.Add(newLikeSport);
                            }
                            com.SaveChanges();
                        }



                        var delTerm =
                        from ls in com.LikeTeam
                        where ls.MemberID == memberID
                        select ls;

                        foreach (var t in delTerm)
                        {
                            com.LikeTeam.Remove(t);
                        }

                        com.SaveChanges();


                        if (ViewModel.LikeTeam != null)
                        {
                            for (var i = 0; i < ViewModel.LikeTeam.Count; i++)
                            {
                                var newLikeTeam = new LikeTeam
                                {
                                    MemberID = memberID,
                                    SportsID = ViewModel.LikeTeam[i].SportsID,
                                    TeamID = ViewModel.LikeTeam[i].TeamID,
                                    CreatedAccountID = memberID.ToString(),
                                    CreatedDate = DateTime.Now
                                };
                                com.LikeTeam.Add(newLikeTeam);
                            }
                        }

                        com.SaveChanges();

                        dbContextTransaction.Commit();
                        isResult = true;
                    }
                    else
                    {
                        //Rollback transaction.
                        dbContextTransaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                }
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region プロフィール画像のアップロード
        //
        //ここでは画像を保存するが、DBに反映しない。
        //（IE9はFile APIを利用できないので、プレビューの目的で一旦保存する。
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult UploadProfileImage()
        {
            MyPageJsonResultModel jsonResult = new MyPageJsonResultModel();

            string uploadDIr = "/Content/img/upload/member/";

            long memberID = long.Parse(Session["CurrentUser"].ToString());
            var member = (from m in com.Member
                          where m.MemberId == memberID
                          select m).FirstOrDefault();

            string rootPath = Server.MapPath("~" + uploadDIr);
            string directoryPath = System.IO.Path.GetFullPath(rootPath);

            if (System.IO.Directory.Exists(directoryPath))
                System.IO.Directory.CreateDirectory(directoryPath);

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase postedFile = Request.Files[file] as HttpPostedFileBase;
                if (postedFile.ContentLength == 0)
                    continue;

                string[] exts = postedFile.FileName.Split('.');
                string ext = exts[exts.Length - 1];
                string imageFileName = Utils.MD5Hash(member.Nickname + DateTime.Now.Ticks.ToString()) + "." + ext;

                postedFile.SaveAs(directoryPath + imageFileName);

                //クライントがわで１写真しか送ってこない
                jsonResult.FilePath =uploadDIr +  imageFileName;
                break;

            }

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", "MyPageSetting");
        }

        #endregion


        #region Save Mail Setting
        /// <summary>
        /// Save Mail Setting
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult SaveMailSetting(MyPageSettingViewModel.SendMailConditionInfoModel ViewModel)
        {
            Int64 memberID = GetMemberID();
            bool isResult = false;

            using (var dbContextTransaction = com.Database.BeginTransaction())
            {
                try
                {
                    if (memberID != null)
                    {
                        var del_mailstart = (from md in com.MailDeliverCond
                                             where md.MemberID == memberID && md.MailDelivCondID == 1
                                             select md).FirstOrDefault();
                        if (del_mailstart != null)
                        {
                            com.MailDeliverCond.Remove(del_mailstart);
                        }

                        var del_mailend = (from md in com.MailDeliverCond
                                           where md.MemberID == memberID && md.MailDelivCondID == 2
                                           select md).FirstOrDefault();

                        if (del_mailend != null)
                        {
                            com.MailDeliverCond.Remove(del_mailend);
                        }

                        if (ViewModel.MailSendAtStart != 0)
                        {
                            var newMailStart = new MailDeliverCond
                            {
                                MemberID = memberID,
                                MailDelivCondID = ViewModel.MailSendAtStart,
                                CreatedAccountID = memberID.ToString(),
                                CreatedDate = DateTime.Now
                            };
                            com.MailDeliverCond.Add(newMailStart);
                        }
                        if (ViewModel.MailSendAtEnd != 0)
                        {
                            var newMailEnd = new MailDeliverCond
                            {
                                MemberID = memberID,
                                MailDelivCondID = ViewModel.MailSendAtEnd,
                                CreatedAccountID = memberID.ToString(),
                                CreatedDate = DateTime.Now
                            };
                            com.MailDeliverCond.Add(newMailEnd);
                        }

                        com.SaveChanges();
                        dbContextTransaction.Commit();
                        isResult = true;
                    }
                    else
                    {
                        //Rollback transaction.
                        dbContextTransaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

    }
}
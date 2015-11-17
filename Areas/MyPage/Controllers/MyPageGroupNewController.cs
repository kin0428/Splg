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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Splg.Models;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Areas.MyPage.Service;
using Splg.Services.System;
using Splg.Models.Members.InfoModel;

#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageGroupNewController : Controller
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

        // todo: interfaceへ変更
        private MyPageGroupNewService workerService;

        private SystemDatetimeService systemDatetimeService;

        //private GroupInfoService groupInfoService;

        #endregion

        public MyPageGroupNewController()
        {
            // todo インスタンス管理
            this.workerService = new MyPageGroupNewService(this.com);
            this.systemDatetimeService = new SystemDatetimeService();
        }

        /// <summary>
        /// GET: /mypage/group/new
        /// </summary>
        public ActionResult Index()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }

            var viewModel = new MyPageGroupNewViewModel();

            long memberId = this.GetLoginMemberId();

            if (memberId <= 0) return View(viewModel);

            viewModel.NewGroup = new MyPageGroupNewViewModel.NewGroupModel();
            viewModel.NewGroup.MemberId = memberId;
            viewModel.NewGroup.GroupId = 0;
            viewModel.NewGroup.GroupName = "";
            viewModel.NewGroup.FollowerSearchString = "";

            viewModel = this.workerService.GetViewModel(viewModel.NewGroup, 
                                                        MyPageGroupEditViewModel.INITIAL_PAGE_SIZE,
                                                        this.systemDatetimeService.TargetYear,
                                                        this.systemDatetimeService.TargetMonth);

            return View(viewModel);
        }

        /// <summary>
        /// もっと見る取得
        /// </summary>
        ///// <param name="currentCount">現在表示しているレコード件数</param>
        /// <param name="newGroup"></param>
        /// <returns>Json形式のActionResult</returns>
        //        public ActionResult GetMoreGroups(int currentCount)
        [HttpPost]
        public JsonResult GetMoreFollowing(MyPageGroupNewViewModel.NewGroupModel newGroup)
        {
            MyPageGroupNewViewModel viewModel = new MyPageGroupNewViewModel();

            long memberId = this.GetLoginMemberId();

            if (newGroup != null)
            {
                viewModel.NewGroup = newGroup;
            }
            else
            {
                viewModel.NewGroup = new MyPageGroupNewViewModel.NewGroupModel
                {
                    GroupId = 0,
                    GroupName = "",
                    FollowerSearchString = "",
                    GroupMembers = new List<MemberModel>(),
                    FollowMembers = new List<MemberModel>(),
                    GroupMemberIdList = null
                };
            }

            viewModel.NewGroup.MemberId = memberId;

            if (memberId > 0)
            {
                viewModel = this.workerService.GetViewModel(viewModel.NewGroup,
                                                       MyPageGroupEditViewModel.INITIAL_PAGE_SIZE,
                                                       this.systemDatetimeService.TargetYear,
                                                       this.systemDatetimeService.TargetMonth);
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ログインユーザのMemberIDを取得
        /// </summary>
        /// <returns></returns>
        private long GetLoginMemberId()
        {
            // HACK: 共通化されるまでの仮メソッドです

            long memberId = 0;

            object currentUser = Session["CurrentUser"];
            if (currentUser != null)
            {
                memberId = Convert.ToInt64(currentUser.ToString());
            }

            return memberId;
        }

        #region Json Result

        #region SearchMember
        /// <summary>
        /// SearchMember
        /// </summary>
        /// <returns>MyPageGroupNewViewModel.MemberModel</returns>
        [HttpPost]
        public JsonResult SearchMember(MyPageGroupNewViewModel.NewGroupModel newGroup)
        {
            long memberId = GetLoginMemberId();
            MyPageGroupNewViewModel viewModel = new MyPageGroupNewViewModel();
            if (newGroup != null)
            {
                viewModel.NewGroup = newGroup;
            }
            else
            {
                viewModel.NewGroup = new MyPageGroupNewViewModel.NewGroupModel
                {
                    GroupId = 0,
                    GroupName = String.Empty,
                    FollowerSearchString = String.Empty,
                    GroupMembers = new List<MemberModel>(),
                    FollowMembers = new List<MemberModel>(),
                    GroupMemberIdList = null
                };
            }
          
            viewModel.NewGroup.MemberId = memberId;

            viewModel = this.workerService.GetViewModel(viewModel.NewGroup,
                                           MyPageGroupEditViewModel.INITIAL_PAGE_SIZE,
                                           this.systemDatetimeService.TargetYear,
                                           this.systemDatetimeService.TargetMonth);
            //GetFollowingMember(viewModel);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AddGroup
        /// <summary>
        /// AddGroup
        /// </summary>
        /// <returns>   none</returns>
        [HttpPost]
        public JsonResult AddGroup(MyPageGroupNewViewModel.AddGroupModel addGroupModel)
        {
            long loginMemberId = GetLoginMemberId();

            var group_name = addGroupModel.GroupName;

            try
            {
                /////////////////////////////////////////////////////////
                //入力チェック
                if (loginMemberId <= 0 && String.IsNullOrEmpty(group_name) )
                {

                    addGroupModel.ErrorMessage = "グループを作成できません。";
                    return Json(addGroupModel, JsonRequestBehavior.AllowGet);
                }

                var group = (from g in com.Groups
                                   where g.GroupName == group_name
                                   && g.MemberID == loginMemberId
                                    && g.Status == true
                                    select g).FirstOrDefault();

                if (group != null)
                {
                    addGroupModel.ErrorMessage = "同じ名前のグループがすでにあります。";
                    return Json(addGroupModel, JsonRequestBehavior.AllowGet);
                }

                /////////////////////////////////////////////////////////
                //新規グループを作成する
                var newGroup = new Groups
                {
                    GroupName = group_name,
                    MemberID = loginMemberId,
                    Status = true,             // ０・・無効　1・・有効
                    CreatedAccountID = loginMemberId.ToString()
                };
                com.Groups.Add(newGroup);
                com.SaveChanges();
                var group_id = (from g in com.Groups
                                where g.GroupName == group_name
                                &&
                                g.MemberID == loginMemberId
                                select g.GroupID).FirstOrDefault();

                addGroupModel.GroupID = Convert.ToInt32(group_id);

                /////////////////////////////////////////////////////////
                //グループメンバーの更新
                using (var dbContextTransaction = com.Database.BeginTransaction())
                {
                    try
                    {
                        if (addGroupModel.MemberIDs == null)
                        {
                            addGroupModel.MemberIDs = new List<long>();
                        }

                        //メンバーに自身を追加
                        addGroupModel.MemberIDs.Add(loginMemberId);

                        //通知メールのタイトルと本文を取得
                        NoticeInfo noticeInfo = (from ni in com.NoticeInfo
                                                 where ni.MailDelivCondID == 4 && ni.NoticeClass == 3
                                                 select ni).FirstOrDefault();
                        string title = noticeInfo.Title;
                        string body = noticeInfo.Body;

                        //追加メンバーの一覧からグループメンバーを新規作成
                        foreach (var newMemberId in addGroupModel.MemberIDs)
                        {
                            var newGroupMember = new GroupMember
                            {
                                GroupID = group_id,
                                MemberID = newMemberId,
                                CreatedAccountID = loginMemberId.ToString(),
                                CreatedDate = DateTime.Now
                                          
                            };
                            com.GroupMember.Add(newGroupMember);

                            //新規に追加する会員向けにメール通知
                            if (loginMemberId == newMemberId)
                                continue;

                            //追加する会員向けにメール通知
                            NoticeInfoForMyPage notice = new NoticeInfoForMyPage();

                            notice.ClassClass = NoticeInfoForMyPage.CLS_GROUP;
                            notice.AddGroup = (from gm in com.Member where gm.MemberId == loginMemberId select gm.Nickname).FirstOrDefault();
                            notice.GroupID = group_id.ToString();
                            notice.Group = (from g in com.Groups where g.GroupID == group_id select g.GroupName).FirstOrDefault();
                            notice.Nickname = (from gm in com.Member where gm.MemberId == loginMemberId select gm.Nickname).FirstOrDefault();

                            notice.setTitle(NoticeInfoForMyPage.CLS_GROUP, title);
                            notice.setBody(NoticeInfoForMyPage.CLS_GROUP, body);

                            MyPageCommon.SendMail(newMemberId, notice.Title, notice.Body);

                            //新規に追加する会員向けにお知らせ配信対象を生成
                            var newNotice = new NoticeDeliverySubject
                            {
                                NoticeId = Convert.ToInt32(noticeInfo.NoticeId),
                                MemberId = Convert.ToInt32(newMemberId),
                                ClassClass = 7,
                                UniqueID = Convert.ToInt32(loginMemberId),  //グループに入れた会員ＩＤを保存
                                UniqueID2 = Convert.ToInt32(group_id),
                                AlreadyReadFlg = false,
                                NoticeDeliveryStatus = 1,
                                CreatedAccountID = loginMemberId.ToString(),
                                CreatedDate = DateTime.Now
                            };
                            com.NoticeDeliverySubject.Add(newNotice);
                           
                                    
                        }
                        com.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
                    
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(addGroupModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetMemberInfo
        /// <summary>
        /// GetMemberInfo
        /// </summary>
        /// <returns>   MyPageGroupNewViewModel.MemberModel</returns>
        [HttpPost]
        public JsonResult GetMemberInfo(string memberId)
        {
            MemberModel isResult = null;
            //MyPageGroupNewViewModel.MemberModel isResult = null;

            long targetMemberId = Convert.ToInt64(memberId);
            isResult = this.workerService.GetMemberInfoData(targetMemberId,
                                                            this.systemDatetimeService.TargetYear,
                                                            this.systemDatetimeService.TargetMonth);
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #endregion

    }
}
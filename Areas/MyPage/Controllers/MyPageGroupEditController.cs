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
 * Class		: MyPageGroupEditController
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
    public class MyPageGroupEditController : Controller
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
        private MyPageGroupEditService workerService;

        private SystemDatetimeService systemDatetimeService;
        
        #endregion

        public MyPageGroupEditController()
        {
            // todo インスタンス管理
            this.workerService = new MyPageGroupEditService(this.com);
            this.systemDatetimeService = new SystemDatetimeService();
        }


        /// <summary>
        /// GET: /mypage/group/{id}/edit/
        /// </summary>
        /// <param name="group_id"></param>
        /// <param name="page"></param>
        public ActionResult Index(int group_id, int? page)
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定(フォーム認証部分も本来なら見るべき。)
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = string.Empty });
            }

            var viewModel = new MyPageGroupEditViewModel();

            long memberId = GetMemberID();

            if (memberId <= 0 || group_id <= 0) return View(viewModel);

            viewModel.NewGroup = new MyPageGroupEditViewModel.NewGroupModel {MemberId = memberId};

            var group = (com.Groups.Where(g => g.GroupID == group_id)).FirstOrDefault();

            if (group == null) return View(viewModel);

            viewModel.NewGroup.GroupID = group_id;
            viewModel.NewGroup.GroupName = group.GroupName;
            viewModel.NewGroup.FollowerSearchString = string.Empty;

            viewModel =  this.workerService.GetViewModel(
                viewModel.NewGroup,
                MyPageGroupEditViewModel.INITIAL_PAGE_SIZE,
                this.systemDatetimeService.TargetYear,
                this.systemDatetimeService.TargetMonth);

            return View(viewModel);
        }

        /// <summary>
        /// もっと見る取得
        /// </summary>
        /// <param name="new_group"></param>
        /// <returns>Json形式のActionResult</returns>
        [HttpPost]
        public JsonResult GetMoreFollowing(MyPageGroupEditViewModel.NewGroupModel new_group)
        {
            MyPageGroupEditViewModel viewModel = new MyPageGroupEditViewModel();

            long memberId = GetMemberID();

            if (new_group != null)
            {
                viewModel.NewGroup = new_group;
            }
            else
            {
                viewModel.NewGroup = new MyPageGroupEditViewModel.NewGroupModel
                {
                    GroupID = 0,
                    GroupName = string.Empty,
                    FollowerSearchString = string.Empty,
                    GroupMembers = new List<MemberModel>(),
                    FollowMembers = new List<MemberModel>(),
                    GroupMemberIDList = null
                };
            }

            viewModel.NewGroup.MemberId = memberId;

            if (memberId > 0)
            {
                viewModel = this.workerService.GetViewModel(
                   viewModel.NewGroup,
                   MyPageGroupEditViewModel.INITIAL_PAGE_SIZE,
                   this.systemDatetimeService.TargetYear,
                   this.systemDatetimeService.TargetMonth);
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        #region memberIDの取得
        private long GetMemberID()
        {
            long memberId = -1;
            object currentUser = Session["CurrentUser"];

            if (currentUser != null)
                memberId = Convert.ToInt64(currentUser.ToString());

            //debug
            //memberId = 2;

            return memberId;
        }
        #endregion

        #region Json Result

        #region SearchMember
        /// <summary>
        /// 検索する
        /// </summary>
        /// <param name="new_group"></param>
        /// <returns>   IEnumerable<MyPageGroupEditViewModel.MemberModel></returns>
        [HttpPost]
        public JsonResult SearchMember(MyPageGroupEditViewModel.NewGroupModel new_group)
        {
            long memberID = GetMemberID();
            MyPageGroupEditViewModel viewModel = new MyPageGroupEditViewModel();
            if (new_group != null)
            {
                viewModel.NewGroup = new_group;
            }
            else
            {
                viewModel.NewGroup = new MyPageGroupEditViewModel.NewGroupModel
                {
                    GroupID = 0,
                    GroupName = string.Empty,
                    FollowerSearchString = string.Empty,
                    GroupMembers = new List<MemberModel>(),
                    FollowMembers = new List<MemberModel>(),
                    GroupMemberIDList = null
                };
            }

            viewModel.NewGroup.MemberId = memberID;

           viewModel = this.workerService.GetViewModel(
                viewModel.NewGroup,
                MyPageGroupEditViewModel.INITIAL_PAGE_SIZE,
                this.systemDatetimeService.TargetYear,
                this.systemDatetimeService.TargetMonth);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UpdateGroup

        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="addGroupModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateGroup(MyPageGroupEditViewModel.AddGroupModel addGroupModel)
        {
            long loginMemberId = GetMemberID();

            var group_id = addGroupModel.GroupID;

            try
            {
                /////////////////////////////////////////////////////////
                //入力チェック
                if (loginMemberId <= 0 && group_id <= 0)
                {
                    addGroupModel.ErrorMessage = "グループを編集できません。";
                    return Json(addGroupModel, JsonRequestBehavior.AllowGet);
                }

                var group = (from g in com.Groups
                                    where g.GroupID == group_id
                                    select g).FirstOrDefault();
                if (group == null)
                {
                    addGroupModel.ErrorMessage = "グループがありません。";
                    return Json(addGroupModel, JsonRequestBehavior.AllowGet);
                }
                
                /////////////////////////////////////////////////////////
                //グループ情報を更新する
                group.GroupName = addGroupModel.GroupName;
                group.ModifiedAccountID = loginMemberId.ToString();
                group.ModifiedDate = DateTime.Now;

                /////////////////////////////////////////////////////////
                //グループメンバーの更新
                using (var dbContextTransaction = com.Database.BeginTransaction())
                {
                    try
                    {

                        //既存メンバーを一旦削除
                        //TODO 将来削除対象のみ削除するように変更
                        var existingMembers = from g in com.GroupMember
                                        where g.GroupID == group_id
                                        select g;

                        foreach (var gm in existingMembers)
                        {
                            com.GroupMember.Remove(gm);
                        }

                        com.SaveChanges();

                        //メンバーに自身を追加
                        if (addGroupModel.MemberIDs == null)
                            addGroupModel.MemberIDs = new List<long> { };

                        if (!addGroupModel.MemberIDs.Contains(loginMemberId))
                            addGroupModel.MemberIDs.Add(loginMemberId); 

                        //通知メールのタイトルと本文を取得
                        NoticeInfo noticeInfo = (from ni in com.NoticeInfo
                                                    where ni.MailDelivCondID == 4 && ni.NoticeClass == 3
                                                    select ni).FirstOrDefault();
                        string title = noticeInfo.Title;
                        string body = noticeInfo.Body;

                        //追加メンバーの一覧からグループメンバーを新規追加
                        //TODO 将来新規追加対象のみ追加するように変更
                        foreach (long newMemberId in addGroupModel.MemberIDs)
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
                            if (!IsNewMember(newMemberId, loginMemberId, existingMembers))
                                continue;
                          
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

        /// <summary>
        /// 判定対象のメンバーが既存グループメンバーに含まれない新規の追加か判定。
        /// ログイン会員の場合は新規とみなさない。
        /// </summary>
        /// <param name="newMemberId">判定対象のメンバー</param>
        /// <param name="loginMemberId">ログインメンバIDー</param>
        /// <param name="existingMembers">既存グループメンバー</param>
        /// <returns></returns>
        private bool IsNewMember(long newMemberId, long loginMemberId, IQueryable<GroupMember> existingMembers)
        {
            if (loginMemberId != newMemberId)
            {
                var m = existingMembers.FirstOrDefault(x => x.MemberID == newMemberId);
                //var m = existingMembers.Where(x => x.MemberID == newMemberId).FirstOrDefault();
                if (m == null)
                    return true;
            }

            return false;
        }

        #region GetMemberInfo
        /// <summary>
        /// GetMemberInfo
        /// </summary>
        /// <returns>   MyPageGroupEditViewModel.MemberModel</returns>
        [HttpPost]
        public JsonResult GetMemberInfo(string member_id)
        {
            MemberModel isResult = null;

            long memberId = Convert.ToInt64(member_id);
            isResult = this.workerService.GetMemberInfoData(memberId, this.systemDatetimeService.TargetYear, this.systemDatetimeService.TargetMonth);
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #endregion

    }
}

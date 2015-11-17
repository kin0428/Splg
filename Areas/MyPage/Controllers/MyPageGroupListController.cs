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
using System.Web.UI;
using Splg.Models.Game.ViewModel;
using Splg.Areas.MyPage.Models.ViewModel;
#endregion

namespace Splg.Areas.MyPage.Controllers
{
    public class MyPageGroupListController : Controller
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
        /// GET: /mypage/group/
        /// </summary>
        public ActionResult Index()
        {
            //Todo:認証処理 AOPでやりたい。 一旦Session変数を参照し判定
            if (Session["CurrentUser"] == null)
            {
                return RedirectToActionPermanent("Login", "Member", new { area = "" });
            }
            
            var viewModel = new MyPageGroupListViewModel();

            //DBから取得する
            SetLines(viewModel, 0, MyPageGroupListViewModel.INITIAL_SIZE);

            return View(viewModel);
        }


        /// <summary>
        /// もっと見る取得
        /// </summary>
        /// <param name="currentCount">現在表示しているレコード件数</param>
        /// <returns>Json形式のActionResult</returns>
//        public ActionResult GetMoreGroups(int currentCount)
        [HttpPost]
        public JsonResult GetMoreGroups(int currentCount)
        {
            MyPageGroupListViewModel viewModel = new MyPageGroupListViewModel();

            //DBから取得する処理
            SetLines(viewModel, currentCount, MyPageGroupListViewModel.INITIAL_PAGE_SIZE);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }


        private void SetLines(MyPageGroupListViewModel viewModel, int skipCount, int takeCount)
        {
            IEnumerable<MyPageGroupListViewModel.GroupListInfo> groupLists = new List<MyPageGroupListViewModel.GroupListInfo> { };

            try
            {
                Int64 memberId = GetMemberID();
                viewModel.MemberId = memberId;

                if (memberId > 0)
                {
                    groupLists = MyPageCommon.GetGroupLists(com, memberId);

                    //合計数
                    viewModel.TotalCount = groupLists.Count();

                    // 各グループのメンバー数画像を取得
                    foreach (MyPageGroupListViewModel.GroupListInfo gl in groupLists)
                    {
                        List<MyPageGroupListViewModel.GroupMemberProfile> profilemember = new List<MyPageGroupListViewModel.GroupMemberProfile> { };
                        var members =
                            from gm in com.Member
                            join ggm in com.GroupMember
                            on gm.MemberId equals ggm.MemberID
                            where ggm.GroupID == gl.GroupID
                            orderby ggm.CreatedDate descending
                            select gm;


                        foreach (var gm in members)
                        {
                            MyPageGroupListViewModel.GroupMemberProfile member = new MyPageGroupListViewModel.GroupMemberProfile();
                            member.MemberId = gm.MemberId;
                            member.Nickname = gm.Nickname;
                            member.ProfileImage = gm.ProfileImg;

                            //TODO おそらく不要
                            profilemember.Add(member);
                        }
                        gl.ProfileMember = profilemember;
                    }
                    //表示分読み込む
                    viewModel.GroupLists = groupLists.Skip(skipCount).Take(takeCount);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


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

        #region leave Group
        /// <summary>
        /// Leave Group
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult LeaveGroup(string group_id)
        {
            Int64 memberID = GetMemberID();
            bool isResult = false;
            int groupID = Convert.ToInt16(group_id);

                using (var dbContextTransaction = com.Database.BeginTransaction())
                {
                    try
                    {
                        if (groupID > 0 && memberID > 0)
                        {
                            var del_gm = (from gm in com.GroupMember
                                         where gm.GroupID == groupID &&  gm.MemberID == memberID
                                         select gm).FirstOrDefault();
                            com.GroupMember.Remove(del_gm);
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
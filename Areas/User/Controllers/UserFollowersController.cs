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
 * Namespace	: Splg.Areas.User.Controllers
 * Class		: UserTopController
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
using Splg.Areas.User.Models.ViewModel;
using Splg.Areas.User.Models.InfoModel;
using Splg.Areas.User.Service;
using Splg.Services.System;

#endregion

namespace Splg.Areas.User.Controllers
{
    public class UserFollowersController : Controller
    {
        // todo 不要ソース削除
        
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //UserEntities User = new UserEntities();

        // todo: interfaceへ変更
        private UserFollowersService workerService;

        private SystemDatetimeService systemDatetimeService;

        #endregion

        public UserFollowersController()
        {
            // todo インスタンス管理
            this.workerService = new UserFollowersService(this.com);
            this.systemDatetimeService = new SystemDatetimeService();
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

        /// <summary>
        /// GET: /User/UserTop/
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public ActionResult Index(long memberId)
        {
            //UserFollowersViewModel viewModel = new UserFollowersViewModel();

            Member targetMembermember = Utils.GetMember(memberId);
            //viewModel.MemberId = memberId;
            //viewModel.Nickname = targetMembermember.Nickname;

            ViewBag.OtherMemberID = memberId;
            ViewBag.OtherMemberNickName = targetMembermember.Nickname;

            var viewModel = this.workerService.GetViewModel(memberId,
                                                            this.GetLoginMemberId(),
                                                            0,
                                                            UserFollowersViewModel.INITIAL_SIZE,
                                                            this.systemDatetimeService.TargetYear,
                                                            this.systemDatetimeService.TargetMonth);

            viewModel.MemberId = memberId;
            viewModel.Nickname = targetMembermember.Nickname;

            ////DBから取得する
            //SetLines(viewModel, memberId, 0, UserFollowersViewModel.INITIAL_SIZE);

            return View(viewModel);
        }

        /// <summary>
        /// もっと見る取得
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="currentCount">現在表示しているレコード件数</param>
        /// <returns>Json形式のActionResult</returns>
        public ActionResult GetMoreFollowers(int memberId, int currentCount)
        {
            var viewModel = this.workerService.GetViewModel(memberId,
                                                    this.GetLoginMemberId(),
                                                    currentCount,
                                                    UserFollowersViewModel.INITIAL_PAGE_SIZE,
                                                    this.systemDatetimeService.TargetYear,
                                                    this.systemDatetimeService.TargetMonth);

            //UserFollowersViewModel viewModel = new UserFollowersViewModel();

            ////DBから取得する処理
            //SetLines(viewModel, memberId, currentCount, UserFollowersViewModel.INITIAL_PAGE_SIZE);

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DBから取得する処理
        /// </summary>
        /// <param name="skipCount">スキップする要素数</param>
        /// <param name="takeCount">返す要素数</param>
        /// <returns>FollowingMemberForUserのIEnumerableオブジェクト</returns>
        private void SetLines(UserFollowersViewModel viewModel, long other_member_id, int skipCount, int takeCount)
        {
            List<FollowerMemberForUser> followerMembers = new List<FollowerMemberForUser> { };

            try
            {

                if (other_member_id > 0)
                {
                    //FollowListテーブルからフォロワーの一覧を取得(f1)
                    //自分のMemberIdでFollowListテーブルのMemberIdを検索し、ProfileImgとNicknameを読み出す(m)

                    //FollowListテーブルから自分がフォローしているユーザーの一覧を取得(f2)
                    //該当行のユーザーをフォローしている場合、「フォロー中」を表示、そうでない場合は「フォローする」を表示する
                    var lines = from f1 in com.FollowList
                                join m in com.Member on f1.FollowerMemberID equals m.MemberId
                                where f1.MemberID == other_member_id
                                select new FollowerMemberForUser
                                {
                                    MemberId = f1.FollowerMemberID,
                                    Nickname = m.Nickname,
                                    ProfileImg = m.ProfileImg,
                                };

                    //合計数
                    viewModel.TotalCount = lines.Count();

                    //FollowListテーブルからフォロワーの一覧を取得
                    //フォロワーのMemberIdでPointテーブルから付与対象年月が現在月となるPointを抽出、
                    //さらにそれらに紐づく精算済みポイントを抽出し精算済ポイントフィールドを合算する。
                    //３で抽出したPointのPointIdでExpectedPointテーブルを検索し、
                    //日付のもっとも新しいものの登録日時を取得し、フォーマットに合わせて出力する

                    int year = DateTime.Now.Year;
                    int month = DateTime.Now.Month;

                    string query = "SELECT MemberID," +
                                    "       ISNULL(SUM(PayOffPoints), 0) AS PayOffPoints," +
                                    "       ISNULL(SUM(fundspoint), 0) AS FundsPoint," +
                                    "       ISNULL(SUM(possesionpoint), 0) AS PossesionPoint," +
                                    "       MAX(ExpectPoint_CreatedDate) AS LastExpectedPointDate " +
                                    "FROM [Splg].[Com].[Point] p " +
                                    "LEFT JOIN" +
                                    "  (SELECT PointID," +
                                    "          MAX(CreatedDate) AS ExpectPoint_CreatedDate" +
                                    "   FROM [Splg].[Com].[ExpectPoint]" +
                                    "   GROUP BY PointID) e ON p.PointID = e.PointID " +
                                    "WHERE " +
                                    "  p.GiveTargetYear =  " + year +
                                    "  AND p.GiveTargetMonth = " + month +
                                    "  AND p.MemberID IN" +
                                    "    (SELECT FollowerMemberID" +
                                    "     FROM [Splg].[Com].[FollowList]" +
                                    "     WHERE MemberID = " + other_member_id + ")" +
                                    " GROUP BY p.memberid";


                    var pointDatas = com.Database.SqlQuery<FollowerMemberForUser>(@query).ToList < FollowerMemberForUser>();


                    long loginMemberId = 0;

                    object currentUser = Session["CurrentUser"];
                    if (currentUser != null)
                        loginMemberId = Convert.ToInt64(currentUser.ToString());

                    foreach (FollowerMemberForUser m in lines)
                    {
                        //フォロワーリストに対して、ログインユーザーのフォロー状況を判別
                        m.IsFollowing = GetIsFollowing(loginMemberId, m.MemberId);

                        var p = pointDatas.SingleOrDefault(x => x.MemberId == m.MemberId);
                       
                        if (p != null && p.MemberId != 0)
                        {
                                m.PayOffPoints = p.PayOffPoints;
                                m.FundsPoint = p.FundsPoint;
                                m.PossesionPoint = p.PossesionPoint;
                                m.LastExpectedPointDate = p.LastExpectedPointDate;
                        }

                        m.IsLoginUser = (m.MemberId == loginMemberId);

                        //m.PayOffPoints = new Random().Next();  //debug
                        followerMembers.Add(m);

                    }

                    //当月の精算済みポイント合計で降順にする
                    var result =  from m in followerMembers orderby m.PayOffPoints descending select m;

                    //表示分読み込む
                    viewModel.FollowerMembers = result.Skip(skipCount).Take(takeCount);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region 他ユーザーのフォロワーをログインユーザーがフォローしているかどうか判別

        /// <summary>
        /// 他ユーザーのフォロワーをログインユーザーがフォローしているかどうか判別
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="memberId"></param>
        /// <returns>FollowingMemberForUserのIEnumerableオブジェクト</returns>
        private bool GetIsFollowing(long currentUserId, long memberId)
        {
            //bool result = false;

            var query = (from c in this.com.FollowList
                         where c.FollowerMemberID == currentUserId
                         where c.MemberID == memberId
                         select c
                        ).ToList();

            return query.Any();

            //if (query.Any())
            //{
            //    result = true;
            //}

            //return result;
        }
        #endregion
       

        /// <summary>
        /// 対象メンバーをフォローする
        /// </summary>
        /// <param name="followingMemberId"></param>
        [HttpPost]
        public ActionResult Follow(long followingMemberId)
        {
            string result = null;

            try
            {
                long memberId = this.GetLoginMemberId();
                //long memberId = 0;

                //object currentUser = Session["CurrentUser"];
                //if (currentUser != null)
                //    memberId = Convert.ToInt64(currentUser.ToString());

                Utils.follow(memberId, followingMemberId);

                result = "success";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// フォロー対象からフォローを外す
        /// </summary>
        /// <param name="followingMemberId"></param>
        [HttpPost]
        public ActionResult UnFollow(long followingMemberId)
        {
            string result = null;

            try
            {
                long memberId = this.GetLoginMemberId();
                //long memberId = 0;

                //object currentUser = Session["CurrentUser"];
                //if (currentUser != null)
                //    memberId = Convert.ToInt64(currentUser.ToString());

                Utils.unfollow(memberId, followingMemberId);

                result = "success";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

	}
}
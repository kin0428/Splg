using System.Collections.Generic;
using System.Linq;
using Splg.Areas.User.Models.InfoModel;
using Splg.Areas.User.Models.ViewModel;
using Splg.Models;
using Splg.Models.Members.InfoModel;
using Splg.Services.Members;
using Splg.Services.Point;
using WebGrease.Css.Extensions;

namespace Splg.Areas.User.Service
{
    /// <summary>
    /// 他ユーザ - フォロー 用コントローラのサービス
    /// </summary>
    public class UserFollowingService
    {
        private ComEntities dbContext;

        private FollowInfoService followInfoService;

        private PointInfoService pointService;

        public UserFollowingService(ComEntities dbContext)
        {
            // todo インスタンス管理
            this.dbContext = dbContext;
            this.followInfoService = new FollowInfoService(this.dbContext);
            this.pointService = new PointInfoService(this.dbContext);
        }

        /// <summary>
        /// ViewModelを取得
        /// </summary>
        /// <param name="targetMemberId">他ユーザの会員ID</param>
        /// <param name="loginMemberId">ログインユーザの会員ID</param>
        /// <param name="skipCount">スキップする要素数</param>
        /// <param name="takeCount">返す要素数</param>
        /// <param name="targetYear">付与対象年</param>
        /// <param name="targetMonth">付与対象月</param>
        /// <returns>UserFollowingViewModelオブジェクト</returns>
        public UserFollowingViewModel GetViewModel(long targetMemberId, long loginMemberId, int skipCount, int takeCount, int targetYear, int targetMonth)
        {
            // フォローの一覧を取得
            var followingMembers = this.followInfoService.GetFollowingMembers(targetMemberId).ToArray();

            // フォローのポイント情報を取得
            this.pointService.GetMembersWithOnlinePoints(followingMembers, targetYear, targetMonth);


            // 当月の精算済みポイント合計で降順にする
            // 表示分読み込む
            var targetFollowingMembers = followingMembers.OrderByDescending(x => x.PayOffPoints)
                                                         .Skip(skipCount)
                                                         .Take(takeCount);

            //フォローリストに対して、ログインユーザーのフォロー状況を判別
            targetFollowingMembers.ForEach(f => f.IsFollowing = this.IsFollowing(f.MemberId, loginMemberId));
       
            targetFollowingMembers.ForEach(f => { f.IsLoginUser = f.MemberId == loginMemberId; });

            // InfoModelへ変換
            var followingInfoModels = this.ConvertToInfoModel(targetFollowingMembers);

            var viewModel = new UserFollowingViewModel
            {
                TotalCount = followingMembers.Count(),
                FollowingMembers = followingInfoModels
            };

            return viewModel;
        }

        /// <summary>
        /// 他ユーザーのフォローユーザーをログインユーザーがフォローしているかどうか判別
        /// </summary>
        /// <param name="targetMemberId">対象ユーザの会員ID</param>
        /// <param name="loginMemberId">ログインユーザの会員ID</param>
        /// <returns>true:フォロー中</returns>
        private bool IsFollowing(long targetMemberId, long loginMemberId)
        {
            var query = (from c in this.dbContext.FollowList
                         where c.FollowerMemberID == loginMemberId
                         where c.MemberID == targetMemberId
                         select c
                        ).ToList();

            return query.Any();
        }

        /// <summary>
        /// InfoModelへ変換
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public IList<FollowingMemberForUser> ConvertToInfoModel(IEnumerable<MemberModel> members)
        {
            if (members == null)
            {
                return null;
            }

            return members.Select(x => new FollowingMemberForUser
            {
                MemberId = x.MemberId,
                Nickname = x.Nickname,
                ProfileImg = x.ProfileImg,
                IsFollowing = x.IsFollowing,
                IsLoginUser = x.IsLoginUser,
                PayOffPoints = x.PayOffPoints,
                FundsPoint = x.FundsPoint,
                PossesionPoint = x.PossesionPoint,
                LastExpectedPointDate = x.LastExpectedPointDate
            }).ToList();
        }
    }
}
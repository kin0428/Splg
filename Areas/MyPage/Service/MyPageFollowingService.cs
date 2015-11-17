using System.Collections.Generic;
using System.Linq;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Models;
using Splg.Models.Members.InfoModel;
using Splg.Services.Members;
using Splg.Services.Point;
using WebGrease.Css.Extensions;

namespace Splg.Areas.MyPage.Service
{
    /// <summary>
    /// マイページ - フォロー 用コントローラのサービス
    /// </summary>
    public class MyPageFollowingService
    {
        private ComEntities dbContext;

        private FollowInfoService followInfoService;

        private PointInfoService pointService;

        public MyPageFollowingService(ComEntities dbContext)
        {
            // todo インスタンス管理
            this.dbContext = dbContext;
            this.followInfoService = new FollowInfoService(this.dbContext);
            this.pointService = new PointInfoService(this.dbContext);
        }

        /// <summary>
        /// ViewModelを取得
        /// </summary>
        /// <param name="memberId">会員ID</param>
        /// <param name="skipCount">スキップする要素数</param>
        /// <param name="takeCount">返す要素数</param>
        /// <param name="targetYear">付与対象年</param>
        /// <param name="targetMonth">付与対象月</param>
        /// <returns>MyPageFollowingViewModelオブジェクト</returns>
        public MyPageFollowingViewModel GetViewModel(long memberId, int skipCount, int takeCount, int targetYear, int targetMonth)
        {
            // フォローの一覧を取得
            var followingMembers = this.followInfoService.GetFollowingMembers(memberId).ToArray();

            // フォローのポイント情報を取得
            this.pointService.GetMembersWithOnlinePoints(followingMembers, targetYear, targetMonth);


            // 当月の精算済みポイント合計で降順にする
            // 表示分読み込む
            var targetFollowingMembers = followingMembers.OrderByDescending(x => x.PayOffPoints)
                                                         .Skip(skipCount)
                                                         .Take(takeCount);

            //フォローリスト上はすべてfollowing
            targetFollowingMembers.ForEach(f => f.IsFollowing = true);

            // InfoModelへ変換
            var followingInfoModels = this.ConvertToInfoModel(targetFollowingMembers);

            var viewModel = new MyPageFollowingViewModel
            {
                TotalCount = followingMembers.Count(),
                FollowingMembers = followingInfoModels
            };

            return viewModel;
        }

        /// <summary>
        /// InfoModelへ変換
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public IList<FollowingMemberForMyPage> ConvertToInfoModel(IEnumerable<MemberModel> members)
        {
            if (members == null)
            {
                return null;
            }

            return members.Select(x => new FollowingMemberForMyPage
            {
                MemberId = x.MemberId,
                Nickname = x.Nickname,
                ProfileImg = x.ProfileImg,
                IsFollowing = x.IsFollowing,
                PayOffPoints = x.PayOffPoints,
                FundsPoint = x.FundsPoint,
                PossesionPoint = x.PossesionPoint,
                LastExpectedPointDate = x.LastExpectedPointDate
            }).ToList();
        }
    }
}
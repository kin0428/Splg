using System.Collections.Generic;
using System.Linq;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Models;
using Splg.Models.Members.InfoModel;
using Splg.Services.Members;
using Splg.Services.Point;

namespace Splg.Areas.MyPage.Service
{
    /// <summary>
    /// マイページ - フォロワー 用コントローラのサービス
    /// </summary>
    public class MyPageFollowersService
    {
        private ComEntities dbContext;

        private FollowInfoService followInfoService;

        private PointInfoService pointService;

        public MyPageFollowersService(ComEntities dbContext)
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
        /// <returns>FollowingMemberForMyPageオブジェクト</returns>
        public MyPageFollowersViewModel GetViewModel(long memberId, int skipCount, int takeCount, int targetYear, int targetMonth)
        {
            // フォロワーの一覧を取得
            var followers = this.followInfoService.GetFollowerMembers(memberId).ToArray();

            // フォロワーのポイント情報を取得
            this.pointService.GetMembersWithOnlinePoints(followers, targetYear, targetMonth);


            // 当月の精算済みポイント合計で降順にする
            // 表示分読み込む
            var targetFollowers = followers.OrderByDescending(x => x.PayOffPoints)
                                           .Skip(skipCount)
                                           .Take(takeCount);

            // InfoModelへ変換
            var followerInfoModels = this.ConvertToInfoModel(targetFollowers);

            var viewModel = new MyPageFollowersViewModel
            {
                TotalCount = followers.Count(),
                FollowerMembers = followerInfoModels
            };

            return viewModel;
        }

        /// <summary>
        /// InfoModelへ変換
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public IEnumerable<FollowerMemberForMyPage> ConvertToInfoModel(IEnumerable<MemberModel> members)
        {
            if (members == null)
            {
                return null;
            }

            return members.Select(x => new FollowerMemberForMyPage
                                            {
                                                MemberId = x.MemberId,
                                                Nickname = x.Nickname,
                                                ProfileImg = x.ProfileImg,
                                                IsFollowing = x.IsFollowing,
                                                PayOffPoints = x.PayOffPoints,
                                                FundsPoint =  x.FundsPoint,
                                                PossesionPoint = x.PossesionPoint,
                                                LastExpectedPointDate = x.LastExpectedPointDate
                                            });
        }
    }
}
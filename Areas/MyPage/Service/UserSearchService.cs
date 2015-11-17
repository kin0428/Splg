using System.Collections.Generic;
using System.Linq;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Models;
using Splg.Models.Members.InfoModel;
using Splg.Services.Point;

namespace Splg.Areas.MyPage.Service
{
    /// <summary>
    /// マイページ - ユーザー検索 用コントローラのサービス
    /// </summary>
    public class UserSearchService
    {
        private ComEntities dbContext;

        private PointInfoService pointService;

        public UserSearchService(ComEntities dbContext)
        {
            // todo インスタンス管理
            this.dbContext = dbContext;
            this.pointService = new PointInfoService(this.dbContext);
        }

        /// <summary>
        /// ViewModelを取得
        /// </summary>
        /// <returns>UserSearchViewModelオブジェクト</returns>
        public UserSearchViewModel GetViewModel()
        {
            var viewModel = new UserSearchViewModel
            {
                UserSearchMembers = new List<UserSearchMemberForMyPage>()
            };

            return viewModel;
        }

        /// <summary>
        /// ViewModelを取得
        /// </summary>
        /// <param name="memberId">会員ID</param>
        /// <param name="searchStr">アカウント名の検索文字列</param>
        /// <param name="skipCount">スキップする要素数</param>
        /// <param name="takeCount">返す要素数</param>
        /// <param name="targetYear">付与対象年</param>
        /// <param name="targetMonth">付与対象月</param>
        /// <returns>UserSearchViewModelオブジェクト</returns>
        public UserSearchViewModel GetViewModel(long memberId, string searchStr, int skipCount, int takeCount, int targetYear, int targetMonth)
        {
            // メンバの一覧を取得
            var members = (from m in this.dbContext.Member
                           from f in this.dbContext.FollowList.Where(x => x.FollowerMemberID == memberId && x.MemberID == m.MemberId).DefaultIfEmpty()
                           where m.MemberId != memberId &&
                                 m.Nickname.Contains(searchStr) &&
                                 m.Status == Constants.MEMBER_STATUS_REGISTERD
                          orderby m.Status   //Orderbyしないとskipできないため
                          select new MemberModel
                          {
                              MemberId = m.MemberId,
                              Nickname = m.Nickname,
                              ProfileImg = m.ProfileImg,
                              IsFollowing = (f.MemberID != null)
                          }).ToArray();

            // フォローのポイント情報を取得
            this.pointService.GetMembersWithOnlinePoints(members, targetYear, targetMonth);


            // 当月の精算済みポイント合計で降順にする
            // 表示分読み込む
            var targetMembers = members.OrderBy(x => x.MemberId)
                                       .Skip(skipCount)
                                       .Take(takeCount);

            // Model変換
            var followerInfoModels = this.ConvertToInfoModel(targetMembers);

            var viewModel = new UserSearchViewModel
            {
                TotalCount = members.Count(),
                UserSearchMembers = followerInfoModels
            };

            return viewModel;
        }

        /// <summary>
        /// Model変換
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public IEnumerable<UserSearchMemberForMyPage> ConvertToInfoModel(IEnumerable<MemberModel> members)
        {
            if (members == null)
            {
                return null;
            }

            return members.Select(x => new UserSearchMemberForMyPage
            {
                MemberId = x.MemberId,
                Nickname = x.Nickname,
                ProfileImg = x.ProfileImg,
                IsFollowing = x.IsFollowing,
                PayOffPoints = x.PayOffPoints,
                FundsPoint = x.FundsPoint,
                PossesionPoint = x.PossesionPoint,
                LastExpectedPointDate = x.LastExpectedPointDate
            });
        }
    }
}
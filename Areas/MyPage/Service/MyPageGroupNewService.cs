using System.Collections.Generic;
using System.Linq;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Models;
using Splg.Services.Members;
using Splg.Services.Point;
using Splg.Models.Members.InfoModel;

namespace Splg.Areas.MyPage.Service
{
    /// <summary>
    /// マイページ - グループ作成用コントローラのサービス
    /// </summary>
    public class MyPageGroupNewService
    {
        private ComEntities dbContext;

        private GroupInfoService groupInfoService;

        private FollowInfoService followInfoService;

        private PointInfoService pointInfoService;

        public MyPageGroupNewService(ComEntities dbContext)
        {
            // todo インスタンス管理
            this.dbContext = dbContext;
            this.groupInfoService = new GroupInfoService(this.dbContext);
            this.followInfoService = new FollowInfoService(this.dbContext);
            this.pointInfoService = new PointInfoService(this.dbContext);
        }

        /// <summary>
        /// ViewModelを取得
        /// </summary>
        /// <param name="newGroupModel">MyPageGroupNewViewModel.NewGroupModelオブジェクト</param>
        /// <param name="takeCount">返す要素数</param>
        /// <param name="targetYear">対象年</param>
        /// <param name="targetMonth">対象月</param>
        /// <returns>MyPageFollowingViewModelオブジェクト</returns>
        public MyPageGroupNewViewModel GetViewModel(MyPageGroupNewViewModel.NewGroupModel newGroupModel, int takeCount, int targetYear, int targetMonth)
        {
            if (newGroupModel == null) return new MyPageGroupNewViewModel();

            var groupMembers = newGroupModel.GroupMembers;

            if (newGroupModel.GroupMemberIdList == null)
            {
                // グループメンバ情報を取得
                groupMembers = this.groupInfoService.GetGroupMembers(newGroupModel.GroupId, newGroupModel.MemberId)
                    .Select(g => new MemberModel
                    {
                        MemberId = g.MemberId,
                        Nickname = g.Nickname,
                        ProfileImg = g.ProfileImg,
                        IsLoginUser = g.IsLoginUser
                    })
                    .OrderBy(x => x.MemberId)
                    .ToList();

                // グループメンバのポイント情報を取得
                this.pointInfoService.GetMembersWithOnlinePoints(groupMembers.ToArray(), targetYear, targetMonth);
            }

            // フォローメンバ情報を取得
            newGroupModel.FollowerSearchString = newGroupModel.FollowerSearchString ?? string.Empty;
            var followingMembers = this.followInfoService.GetFollowingMembers(newGroupModel.MemberId, newGroupModel.FollowerSearchString).ToArray();

            // フォローメンバのポイント情報を取得
            this.pointInfoService.GetMembersWithOnlinePoints(followingMembers, targetYear, targetMonth);

            // フォローメンバからグループメンバを除く
            List<MemberModel> notInGroupMembers;
            if (newGroupModel.GroupMemberIdList == null)
            {
                notInGroupMembers = followingMembers.Where(x => !(groupMembers.Select(y => y.MemberId)).Contains(x.MemberId)).ToList();
            }
            else
            {
                notInGroupMembers = followingMembers.Where(x => !(newGroupModel.GroupMemberIdList).Contains(x.MemberId)).ToList();
            }

            // 追加対象のフォローメンバを取得
            List<MemberModel> targetFollowingMembers;
            if (newGroupModel.FollowMemberIdList != null)
            {
                // 編集画面上ですでに表示されているメンバを除く
                targetFollowingMembers =
                    notInGroupMembers.Where(x => !(newGroupModel.FollowMemberIdList).Contains(x.MemberId))
                                     .Take(takeCount)
                                     .ToList();
            }
            else
            {
                targetFollowingMembers = notInGroupMembers.Take(takeCount).ToList();
            }

            // ViewModelへ設定
            var viewModel = new MyPageGroupNewViewModel
            {
                NewGroup = newGroupModel
            };
            viewModel.NewGroup.GroupMembers = groupMembers;
            viewModel.NewGroup.FollowMembers = targetFollowingMembers;
            viewModel.TotalCount = notInGroupMembers.Count;

            return viewModel;
        }

        /// <summary>
        /// メンバー情報の取得
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="targetYear"></param>
        /// <param name="targetMonth"></param>
        /// <returns></returns>
        public MemberModel GetMemberInfoData(long memberId, int targetYear, int targetMonth)
        {
            // メンバ情報を取得
            var member = (this.dbContext.Member.Where(m => m.MemberId == memberId)).FirstOrDefault();
            if (member == null) return new MemberModel
            {
                MemberId = memberId,
                Nickname = string.Empty,
                ProfileImg = string.Empty,
                PayOffPoints = 0
            };

            var memberModel = new MemberModel
            {
                MemberId = member.MemberId,
                Nickname = member.Nickname,
                ProfileImg = (member.ProfileImg ?? Core.resources.Resources.MemberProfileImgDefaultPath)
            };

            // ポイント情報を取得
            this.pointInfoService.GetMembersWithOnlinePoints(new[] { memberModel }, targetYear, targetMonth);

            return memberModel;
        }

    }
}
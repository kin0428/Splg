using System.Linq;
using Splg.Models;
using Splg.Services.Point;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Services.Members;
using Splg.Services.System;
using System.Collections.Generic;
using Splg.Areas.MyPage.Models.InfoModel;

namespace Splg.Areas.MyPage.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class MyPageGroupDetailsService
    {
        private ComEntities dbContext;

        private GroupInfoService groupInfoService;
        private PointInfoService pointInfoService;
        private SystemDatetimeService systemDatetimeService;

        public MyPageGroupDetailsService(ComEntities dbContext)
        {
            // todo インスタンス管理
            this.dbContext = dbContext;
            this.groupInfoService = new GroupInfoService(this.dbContext);
            this.pointInfoService = new PointInfoService(this.dbContext);
            this.systemDatetimeService = new SystemDatetimeService();
        }

        /// <summary>
        /// ViewModelを取得
        /// </summary>
        /// <param name="groupId">グループID</param>
        /// <param name="loginMemberId">ログイン会員ID</param>
        /// <param name="skipCount">スキップする要素数</param>
        /// <param name="takeCount">返す要素数</param>
        /// <param name="targetYear">付与対象年</param>
        /// <param name="targetMonth">付与対象月</param>
        /// <returns>MyPageGroupDetailsViewModelオブジェクト</returns>
        public MyPageGroupDetailsViewModel GetViewModel(long groupId, long loginMemberId,int skipCount, int takeCount, int targetYear, int targetMonth)
        {
            var viewModel = new MyPageGroupDetailsViewModel();

            //グループ情報を設定する
            viewModel.GroupInfo = new MyPageGroupModel();

            //グループ会員の一覧を取得
            var groupMembers = this.groupInfoService.GetGroupMembers(groupId, loginMemberId).ToArray();

            //グループ会員のポイント情報を取得
            this.pointInfoService.GetMembersWithOnlinePoints(groupMembers, targetYear, targetMonth);

            //グループ会員のランキング情報を取得
            int year = this.systemDatetimeService.TargetYear;
            int month = this.systemDatetimeService.TargetMonth;
            this.groupInfoService.GetRanking(groupId, year, month, groupMembers);

            viewModel.GroupInfo.MemberId = loginMemberId;
            viewModel.GroupInfo.GroupId = groupId;
            viewModel.GroupInfo.GroupName = (from g in dbContext.Groups where g.GroupID == groupId select g.GroupName).FirstOrDefault();
            viewModel.GroupInfo.GroupMembers = groupMembers.ToList();


            return viewModel;
        }

        /// <summary>
        /// ポイントランキングの取得
        /// </summary>
        /// <returns>   none</returns>
        public IEnumerable<MyPageGroupMemberModel> GetRanking(long groupId, int year, int month, MyPageGroupMemberModel[] members = null, long loginMemberId = 0)
        {
            return this.groupInfoService.GetRanking(groupId, year, month, members);
        }
    }
}
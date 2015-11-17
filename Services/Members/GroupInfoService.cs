using System.Collections.Generic;
using System.Linq;
using Splg.Models;
using Splg.Areas.MyPage.Models.ViewModel;
using Splg.Areas.MyPage.Models.InfoModel;
using Splg.Services.System;
using Splg.Services.Point;

namespace Splg.Services.Members
{
    /// <summary>
    /// グループの会員情報関連サービス
    /// </summary>
    public class GroupInfoService
    {
        // todo インスタンス管理
        private ComEntities dbContext;
        private SystemDatetimeService systemDatetimeService;
        private PointInfoService pointInfoService;

        public GroupInfoService(ComEntities dbContext)
        {
            this.dbContext = dbContext;
            this.pointInfoService = new PointInfoService(this.dbContext);
            this.systemDatetimeService = new SystemDatetimeService();
        }

        /// <summary>
        /// グループ会員の一覧を取得する。
        /// </summary>
        /// <param name="groupId">グループID</param>
        /// <param name="loginMemberId">ログイン会員ID。設定した場合、ログイン会員の要素のIsLoginUserが真になる。</param>
        /// <returns></returns>
        public IList<MyPageGroupMemberModel> GetGroupMembers(long groupId, long loginMemberId = 0)
        {
            var members = (from gm in dbContext.GroupMember
                           join m in dbContext.Member
                           on gm.MemberID equals m.MemberId
                           where gm.GroupID == groupId
                           orderby gm.CreatedDate descending   //TODO 要仕様確認
                           select new MyPageGroupMemberModel
                           {
                               MemberId = gm.MemberID,
                               Nickname = m.Nickname,
                               ProfileImg = m.ProfileImg,
                               IsLoginUser = loginMemberId == gm.MemberID
                           });

            return members.ToArray();
        }

        /// <summary>
        /// ポイントランキングの取得
        /// </summary>
        /// <returns>   none</returns>
        public IEnumerable<MyPageGroupMemberModel> GetRanking(long groupId, int targetYear, int targetMonth, MyPageGroupMemberModel[] members = null, long loginMemberId = 0)
        {
            var result = new List<MyPageGroupMemberModel>();

            if (members == null)
            {
                members = this.GetGroupMembers(groupId, loginMemberId).ToArray();

                //グループ会員のランキング情報を取得
                pointInfoService.GetMembersWithOnlinePoints(members, targetYear, targetMonth);
            }

            foreach (var member in members)
            {
                var monthlyResults = (from mr in dbContext.MonthlyResults
                                      where mr.MemberID == member.MemberId
                                      && mr.ReleVantYear == targetYear
                                      && mr.ReleVantMonth == targetMonth
                                      select mr);

                //[SportsID]ごとにレコードがあるのでサマッている
                if (monthlyResults != null)
                {
                    foreach (var monthlyResult in monthlyResults)
                    {
                        member.ExpectNumber += monthlyResult.ExpectNumber;
                        member.CorrectPoint += monthlyResult.ExpectNumber * monthlyResult.CorrectPercent;
                    }
                }

                result.Add(member);
            }

            return result.Take(MyPageGroupDetailsViewModel.RANKING_TOP_NUM);
        }
    }
}
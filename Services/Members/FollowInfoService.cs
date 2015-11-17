using System.Collections.Generic;
using System.Linq;
using Splg.Models;
using Splg.Models.Members.InfoModel;

namespace Splg.Services.Members
{
    /// <summary>
    /// 会員のフォロー，フォロワー情報関連サービス
    /// </summary>
    public class FollowInfoService
    {
        // todo インスタンス管理
        private ComEntities dbContext;

        public FollowInfoService(ComEntities dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// フォロワーの一覧を取得
        /// </summary>
        /// <param name="memberId">会員ID</param>
        /// <returns></returns>
        public IList<MemberModel> GetFollowerMembers(long memberId)
        {
            //FollowListテーブルからフォロワーの一覧を取得(f1)
            //自分のMemberIdでFollowListテーブルのMemberIdを検索し、ProfileImgとNicknameを読み出す(m)

            //FollowListテーブルから自分がフォローしているユーザーの一覧を取得(f2)
            //該当行のユーザーをフォローしている場合、「フォロー中」を表示、そうでない場合は「フォローする」を表示する

            var query = from f1 in this.dbContext.FollowList
                        join m in this.dbContext.Member on f1.FollowerMemberID equals m.MemberId
                        from f2 in this.dbContext.FollowList.Where(x => x.FollowerMemberID == memberId && x.MemberID == f1.FollowerMemberID).DefaultIfEmpty()
                        where f1.MemberID == memberId &&
                              m.Status == Constants.MEMBER_STATUS_REGISTERD
                        select new MemberModel
                        {
                            MemberId = f1.FollowerMemberID,
                            Nickname = m.Nickname,
                            ProfileImg = m.ProfileImg,
                            IsFollowing = (f2.MemberID != null)
                        };

            return query.ToList();
        }

        /// <summary>
        /// フォローの一覧を取得
        /// </summary>
        /// <param name="memberId">会員ID</param>
        /// <returns></returns>
        public IList<MemberModel> GetFollowingMembers(long memberId)
        {
            //自分のMemberIdでFollowListテーブルのFollowerMemberIdを検索し
            //FollowerMemberIdでMemberテーブルのMemberIdを検索し、ProfileImgとNickNameを読み出す

            var query = from c in this.dbContext.FollowList
                        join m in this.dbContext.Member on c.MemberID equals m.MemberId
                        where c.FollowerMemberID == memberId &&
                              m.Status == Constants.MEMBER_STATUS_REGISTERD
                        select new MemberModel
                        {
                            MemberId = c.MemberID,
                            Nickname = m.Nickname,
                            ProfileImg = m.ProfileImg,
                            IsFollowing = true
                        };

            return query.ToList();
        }

        /// <summary>
        /// フォローの一覧を取得
        /// </summary>
        /// <param name="memberId">会員ID</param>
        /// <param name="searchStr">アカウント名の検索文字列</param>
        /// <returns></returns>
        public IList<MemberModel> GetFollowingMembers(long memberId, string searchStr)
        {
            //自分のMemberIdでFollowListテーブルのFollowerMemberIdを検索し
            //FollowerMemberIdでMemberテーブルのMemberIdを検索し、ProfileImgとNickNameを読み出す

            var query = from c in this.dbContext.FollowList
                        join m in this.dbContext.Member on c.MemberID equals m.MemberId
                        where c.FollowerMemberID == memberId &&
                              m.Status == Constants.MEMBER_STATUS_REGISTERD &&
                              m.Nickname.Contains(searchStr)
                        select new MemberModel
                        {
                            MemberId = c.MemberID,
                            Nickname = m.Nickname,
                            ProfileImg = m.ProfileImg,
                            IsFollowing = true
                        };

            return query.ToList();
        }
    }
}
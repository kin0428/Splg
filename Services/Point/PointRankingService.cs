using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models;
using Splg.Models.ViewModel;

namespace Splg.Services.Point
{
    public class PointRankingService
    {
        #region property
        /// <summary>
        /// Com　DBContext
        /// </summary>
        private ComEntities ComEntities { get; set; }

        public DateTime TargetDate
        {
            get
            {
                return DateTime.Now.Date.AddDays(-1);
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// 所持ポイントランキングサービス
        /// </summary>
        public PointRankingService()
        {

        }

        /// <summary>
        /// 所持ポイントランキングサービス
        /// </summary>
        public PointRankingService(ComEntities comEntities)
        {
            SetComEntities(comEntities);
        }
        #endregion

        #region method
        /// <summary>
        /// DBContext設定
        /// </summary>
        public void SetComEntities(ComEntities comEntities)
        {
            ComEntities = comEntities;
        }

        /// <summary>
        /// ポイントランキング取得
        /// </summary>
        public IEnumerable<PointRankingViewModel> GetPointRankingsByPullCount(int getCount)
        {
            

            return (from ranking in ComEntities.PossesionPointRanking
                        join member in ComEntities.Member on ranking.MemberID equals member.MemberId
                        where ranking.TargetDate == TargetDate
                        select new PointRankingViewModel
                        {
                            MemberId = member.MemberId,
                            Nickname = member.Nickname,
                            Point = ranking.TargetPossesionPoint,
                            ProfileImage = member.ProfileImg,
                            Ranking = ranking.TargetRanking
                        } into PointRank
                        orderby PointRank.Ranking ascending
                        select PointRank).Take(getCount);
        }

        public IEnumerable<PointRankingViewModel> GetFollowPointRankingsByMemberId(long memberId,int getCount)
        {
            return (from follow in ComEntities.FollowList
                            join member     in ComEntities.Member on follow.MemberID equals member.MemberId
                            join ranking    in ComEntities.PossesionPointRanking on member.MemberId equals ranking.MemberID
                            where follow.FollowerMemberID == memberId && ranking.TargetDate == TargetDate
                            select new PointRankingViewModel
                            {
                                MemberId = member.MemberId,
                                Nickname = member.Nickname,
                                Point = ranking.TargetPossesionPoint,
                                ProfileImage = member.ProfileImg,
                                Ranking = ranking.TargetRanking
                            } into PointRank
                            orderby PointRank.Ranking ascending
                            select PointRank).Take(getCount);

         
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Splg.Areas.Jleague;
using Splg.Areas.Mlb;
using Splg.Areas.Npb.Models;
using Splg.Models;
using Splg.Models.Game.InfoModel;
using Splg.Models.Members.InfoModel;
using Splg.Core.Constant;

namespace Splg.Services.Game
{
    public class ExpectInfoService
    {
        private ComEntities comEntities;

        public ExpectInfoService(ComEntities comEntities)
        {
            this.comEntities = comEntities;
        }

        /// <summary>
        /// chech user is bet in match
        /// </summary>
        /// <param name="gameId">試合ID</param>
        /// <param name="memberId">会員ID</param>
        /// <param name="sportsId">スポ－ツID</param>
        /// <returns>True:Bet済 False:Bet未済</returns>
        public bool IsBetByGameIdAndMemberIdAndSportId(int gameId, long memberId, int sportsId)
        {
            var query = (from ep in comEntities.ExpectPoint
                         join et in comEntities.ExpectTarget on ep.ExpectTargetID equals et.ExpectTargetID
                         join p in comEntities.Point on ep.PointID equals p.PointID
                         where ep.SituationStatus != 2 && // 状況ステータス= 2:キャンセル
                               et.SportsID == sportsId &&
                               et.ClassClass == 4 &&     // 分類種別 = 4:試合
                               et.UniqueID == gameId &&
                               p.MemberID == memberId
                         select ep).ToList().Count;

            return query > 0;
        }

        /// <summary>
        /// 予想した試合が終了しているかどうかを返す
        /// </summary>
        /// <param name="memberId">会員ID</param>
        /// <param name="startDate">開始日</param>
        /// <returns>true:終了した試合有</returns>
        public bool IsExistsFinishedGame(long memberId, DateTime startDate)
        {
            //return this.GetExpectedGameInfo(memberId, startDate).Any(e => new[] { 8, 10 }.Contains(e.GameStatus));
            return true;
        }

        /// <summary>
        /// 予想ポイント情報を取得
        /// </summary>
        /// <param name="gameId">試合ID</param>
        /// <param name="memberId">会員ID</param>
        /// <param name="sportsId">スポ－ツID</param>
        /// <returns>ExpectPoint</returns>
        public ExpectPoint GetExpectPointInfo(int gameId, long memberId, int sportsId)
        {
            return (from ep in comEntities.ExpectPoint
                         join et in comEntities.ExpectTarget on ep.ExpectTargetID equals et.ExpectTargetID
                         join p in comEntities.Point on ep.PointID equals p.PointID
                         where ep.SituationStatus != 2 && // 状況ステータス= 2:キャンセル
                               et.SportsID == sportsId &&
                               et.ClassClass == 4 &&     // 分類種別 = 4:試合
                               et.UniqueID == gameId &&
                               p.MemberID == memberId
                         select ep).FirstOrDefault();
        }


        /// <summary>
        /// 予想ポイント情報を取得
        /// </summary>
        /// <param name="memberId">会員ID</param>
        /// <param name="fromDate">試合日付(FROM)</param>
        /// <param name="toDate">試合日付(TO)</param>
        /// <returns></returns>
        public IEnumerable<ExpectationInfoModel> GetExpectedGameInfo(long memberId, DateTime fromDate, DateTime toDate)
        {
            var query = (from et in comEntities.ExpectTarget
                         join ep in comEntities.ExpectPoint on et.ExpectTargetID equals ep.ExpectTargetID
                         join p in comEntities.Point on ep.PointID equals p.PointID
                         where ep.SituationStatus != 2
                         && et.ClassClass == 4      // 分類種別 = 4:試合
                         && p.MemberID == memberId
                         && et.StartScheduleDate >= fromDate
                         && et.StartScheduleDate <= toDate
                         select new ExpectationInfoModel
                         {
                             MemberID = p.MemberID,
                             SportID = et.SportsID == null ? 0 : et.SportsID,
                             PointID = (ep.PointID == null ? 0 : ep.PointID),
                             ExpectPointID = (ep.ExpectPointID == null ? 0 : ep.ExpectPointID),
                             ExpectPoint = (ep.ExpectPoint1 == null ? 0 : ep.ExpectPoint1),
                             BetStartDate = p.BetStartDate,  //Phase2では不要
                             GiveTargetMonth = p.GiveTargetMonth,
                             BetSelectID = (ep.BetSelectID.Value == null ? 0 : ep.BetSelectID.Value),
                             GameID = (et.UniqueID == null ? 0 : et.UniqueID),
                             Odds = (from odds in comEntities.OddsInfo
                                     join bsm in comEntities.BetSelectMaster on odds.BetSelectMasterID equals bsm.BetSelectMasterID
                                     where
                                     ep.BetSelectID.ToString() == bsm.BetSelectID.ToString()
                                     && odds.ClassificationType == 2
                                     && odds.ExpectTargetID == et.ExpectTargetID
                                     select odds.Odds).FirstOrDefault(),

                             StartScheduleDate = et.StartScheduleDate.Value == null ? DateTime.MinValue : et.StartScheduleDate.Value
                         }).ToList();

            query.ForEach(q => q.GameStatus = this.GetGameStatus(q.MemberID, q.SportID, (int)q.GameID));

            return query.AsQueryable() ;

        }

        /// <summary>
        /// 試合状況ステータスを取得
        /// </summary>
        /// <param name="memberId">会員ID</param>
        /// <param name="sportId">スポーツID</param>
        /// <param name="gameId">試合ID</param>
        /// <returns></returns>
        public int GetGameStatus(long memberId, int sportId, int gameId)
        {
            int gameStatus = 0;
            
            switch (sportId)
            {
                case Constants.NPB_SPORT_ID:
                    gameStatus = NpbCommon.GetStatusMatch(gameId, memberId.ToString());
                    break;
                case Constants.MLB_SPORT_ID:
                    gameStatus = MlbCommon.GetStatusMatch(gameId, memberId.ToString());
                    break;
                case Constants.JLG_SPORT_ID:
                    gameStatus = JlgCommon.GetStatusMatch(gameId, memberId.ToString());
                    break;
                default:
                    break;
            }

            return gameStatus;
        }


        /// <summary>
        /// 試合に予想している会員の一覧を返す
        /// </summary>
        /// <param name="sportsId"></param>
        /// <param name="gameId"></param>
        /// <param name="betSelectID">１・・ホームの勝ち、２・・ビジターの勝ち　３・・引き分け</param>
        /// <param name="loginMemberId"></param>
        /// <param name="omitLoginMember"></param>
        /// <returns></returns>
        public List<MemberModel> GetBetMembers(int sportsId, int gameId, long loginMemberId, bool omitLoginMember = false)
        {
            var members = (from ep in comEntities.ExpectPoint
                           join et in comEntities.ExpectTarget on ep.ExpectTargetID equals et.ExpectTargetID
                           join pt in comEntities.Point on ep.PointID equals pt.PointID
                           join m in comEntities.Member on pt.MemberID equals m.MemberId
                           where
                           et.SportsID == sportsId
                           && et.ClassClass == 4      // 分類種別 = 4:試合
                           && et.UniqueID == gameId
                           && ep.SituationStatus != 2 // 状況ステータス= 2:キャンセル
                           && m.Status == 1
                           select new MemberModel
                           {
                               MemberId = pt.MemberID,
                               Nickname = m.Nickname,
                               ProfileImg = m.ProfileImg,
                               IsLoginUser = loginMemberId == pt.MemberID,
                               BetSelectID = ep.BetSelectID
                           });

            if(omitLoginMember)
                return members.Where(x => x.MemberId != loginMemberId).ToList();
            else
                return members.ToList();
        }
    }
}
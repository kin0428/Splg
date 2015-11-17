using System.Collections.Generic;
using System.Linq;
using Splg.Models;
using Splg.Models.Members.InfoModel;
using Splg.Models.Game.ViewModel;
using Splg.Services.System;
using System;
using Splg.Models.Game.InfoModel;

namespace Splg.Services.Point
{
    /// <summary>
    /// Point関連の共通サービス
    /// 各コントローラのサービスから呼び出す
    /// </summary>
    public class PointInfoService
    {
        // todo インスタンス管理
        private ComEntities DbContext{get;set;}

        public PointInfoService(ComEntities dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// 会員のポイント情報を取得
        /// </summary>
        /// <param name="memberModels">会員規定モデルの配列</param>
        /// <param name="targetYear">対象年</param>
        /// <param name="targetMonth">対象月</param>
        public void GetMembersWithOnlinePoints(MemberModel[] memberModels, int targetYear, int targetMonth)
        {
            // ポイント情報を取得
            var pointDatas = this.GetMembersWithOnlinePoints(memberModels.Select(x => x.MemberId).ToList(), targetYear, targetMonth);

            // メンバー情報とポイント情報をマージ
            foreach (var member in memberModels)
            {
                var poitdata = pointDatas.SingleOrDefault(x => x.MemberId == member.MemberId);

                if (poitdata == null || poitdata.MemberId == 0) continue;

                member.PayOffPoints = poitdata.PayOffPoints;
                member.FundsPoint = poitdata.FundsPoint;
                member.PossesionPoint = poitdata.PossesionPoint;
                member.LastExpectedPointDate = poitdata.LastExpectedPointDate;
            }
        }

        /// <summary>
        /// 会員のポイント情報を取得
        /// オンラインポイント情報とともに取得する
        /// </summary>
        /// <param name="memberIds">会員IDの配列</param>
        /// <param name="targetYear">対象年</param>
        /// <param name="targetMonth">対象月</param>
        /// <returns></returns>
        public IList<MemberModel> GetMembersWithOnlinePoints(List<long> memberIds, int targetYear, int targetMonth)
        {
            var members = new List<MemberModel>();

            foreach(long memberId in memberIds ){
                var member = this.GetMemberWithOnlinePoints(memberId, targetYear, targetMonth);
                if (member != null)
                    members.Add(member);
            }

            return members.ToArray();
        }

        /// <summary>
        /// 会員モデル取得
        /// オンラインポイント情報とともに取得する
        /// </summary>
        /// <remarks>
        /// Todo:会員モデルの全体取得
        /// </remarks>
        public MemberModel GetMemberWithOnlinePoints(long memberId, int targetYear, int targetMonth)
        {
            var point = this.DbContext.Point.Where(m => m.MemberID == memberId && m.GiveTargetYear == targetYear && m.GiveTargetMonth == targetMonth).
                OrderByDescending(x => new { x.GiveTargetWeek, x.CreatedDate }).FirstOrDefault();
            //Phase1ベースのデータ（2015６月最終週）への対応のため、存在する最後のポイントレコードを取得する

            //最終予想日時の取得
            //他の情報には関連は無く、単に最後の予想日時を取得する
            var expectPoint = (from ep in this.DbContext.ExpectPoint
                         join p in this.DbContext.Point on ep.PointID equals p.PointID
                         where p.MemberID == memberId
                         orderby ep.CreatedDate descending
                         select ep).FirstOrDefault();
            
            if (point == null)
            {
                return null;
            }

            var member = (from m in this.DbContext.Member
                          where m.MemberId == memberId
                          select m).FirstOrDefault();


            //Todo:会員モデルの全体取得
            return new MemberModel()
                           {
                               PointId = point.PointID,
                               MemberId =memberId,
                               PossesionPoint = point.PossesionPoint,
                               FundsPoint = point.FundsPoint,
                               PayOffPoints = point.PayOffPoints,
                               LastExpectedPointDate = expectPoint == null ? null : expectPoint.CreatedDate,
                               BetStartDate = point.BetStartDate,
                               BetEndDate = point.BetEndDate,
                               Nickname = member != null ? member.Nickname : null,
                               ProfileImg = member != null ? member.ProfileImg : null
                           };
        }

        /// <summary>
        /// 会員IDで今月の予想情報を取得する。
        /// もともとNPBエリア（NpbRightContentController）にあった共通メソッドだが、
        /// Phase1,２仕様どちらでも予想した試合を本日日付からで判定できない。
        /// そのため、下記に変更する。
        /// ※ExpectTargetを基点にSituationStatus＝１のその会員の試合をすべて表示する。
        /// 最初に試合表示パネルから予想する時点では、パラメータのGameIDから試合日時が取れるので
        /// バッチ処理で更新されたPointテーブルのデータに従う形でPointテーブルのレコードは正確に特定できる。
        /// そのため、ExpectTargetを基点にすべて表示でPhase２切り替え時も問題ない。
        /// </summary>
        /// <returns>List Game predicted.</returns>
        public PointInfoViewModel GetExpectedPointInfo(int memberID, bool showCurrentPointInfoOnly = false)
        {
            PointInfoViewModel result = new PointInfoViewModel();

            SystemDatetimeService systemDatetimeService = new SystemDatetimeService();

            var memberPoint = this.GetMemberWithOnlinePoints(memberID,systemDatetimeService.TargetYear,systemDatetimeService.TargetMonth);

            if (memberPoint == null)
                return null;

            result.PointInfoModel = new Models.PointInfo.InfoModel.PointInfoModel();
            result.PointInfoModel.PointID = (int)memberPoint.PointId;
            result.PointInfoModel.PossesionPoint = memberPoint.PossesionPoint;
            result.PointInfoModel.FundsPoint = memberPoint.FundsPoint;
            result.PointInfoModel.PayOffPoints = memberPoint.PayOffPoints;
            result.PointInfoModel.BetStartDate = memberPoint.BetStartDate;
            result.PointInfoModel.BetEndDate = memberPoint.BetEndDate;

            //Get these games that not end : not start or ongoing.
            if (!showCurrentPointInfoOnly)
            {
                var expectations = from et in DbContext.ExpectTarget
                                   join ep in DbContext.ExpectPoint on et.ExpectTargetID equals ep.ExpectTargetID
                                   join p in DbContext.Point on ep.PointID equals p.PointID
                                   where
                                   (et.FixBetSelectID == 0 || et.FixBetSelectID == null)
                                   && ep.SituationStatus == 1 //TODO SituationStatus 要確認（終了していない試合のみ対象で良い？）
                                   && p.MemberID == memberID
                                   orderby et.StartScheduleDate, et.SportsID
                                   select new ExpectationInfoModel
                              {
                                  SportID = et.SportsID == null ? 0 : et.SportsID,
                                  PointID = (ep.PointID == null ? 0 : ep.PointID),
                                  ExpectPointID = (ep.ExpectPointID == null ? 0 : ep.ExpectPointID),
                                  ExpectPoint = (ep.ExpectPoint1 == null ? 0 : ep.ExpectPoint1),
                                  BetStartDate = p.BetStartDate,  //Phase2では不要
                                  GiveTargetMonth = p.GiveTargetMonth,
                                  BetSelectID = (ep.BetSelectID.Value == null ? 0 : ep.BetSelectID.Value),
                                  GameID = (et.UniqueID == null ? 0 : et.UniqueID),
                                  Odds = (from odds in DbContext.OddsInfo
                                          join bsm in DbContext.BetSelectMaster on odds.BetSelectMasterID equals bsm.BetSelectMasterID
                                          where
                                          ep.BetSelectID.ToString() == bsm.BetSelectID.ToString()
                                          && odds.ClassificationType == 2
                                          && odds.ExpectTargetID == et.ExpectTargetID
                                          select odds.Odds).FirstOrDefault(),

                                  StartScheduleDate = et.StartScheduleDate.Value == null ? DateTime.MinValue : et.StartScheduleDate.Value
                              };
                result.ExpectationInfoModels = expectations;
                
            }


            return result;
        }

        /// <summary>
        /// 会員IDでオンラインな所持ポイントを取得する。
        /// </summary>
        /// <returns>List Game predicted.</returns>
        public int GetOnlinePoint(int memberId)
        {
            SystemDatetimeService systemDatetimeService = new SystemDatetimeService();

            var point = (from p in this.DbContext.Point
                         where p.MemberID == memberId
                         && p.GiveTargetYear == (short)systemDatetimeService.TargetYear
                         && p.GiveTargetMonth == (short)systemDatetimeService.TargetMonth
                          orderby p.GiveTargetWeek descending, p.CreatedDate descending
                         select new MemberModel
                         {
                             MemberId = p.MemberID,
                             PossesionPoint = p.PossesionPoint,
                             FundsPoint = p.FundsPoint,
                             PayOffPoints = p.PayOffPoints
                         }
                        ).FirstOrDefault();

            if (point == null)
                return 0;

            return point.PossesionPoint;
        }

        /// <summary>
        /// ポイントテーブル存在判定
        /// </summary>
        public bool IsExistsPoint(long memberId, DateTime targetDateTime)
        {
            var point = (from p in this.DbContext.Point
                         where p.MemberID == memberId
                         && p.BetStartDate <= targetDateTime
                         && p.BetEndDate >= targetDateTime
                         //&& p.GiveTargetYear == (short)targetDateTime.Year
                         //&& p.GiveTargetMonth == (short)targetDateTime.Month
                         select p
                        ).FirstOrDefault();

            return (point != null);
        }

        /// <summary>
        /// 会員IDでオンラインな所持ポイントを取得する。
        /// </summary>
        /// <returns>List Game predicted.</returns>
        public string GetOnlinePointStr(int memberId)
        {
            SystemDatetimeService systemDatetimeService = new SystemDatetimeService();

            var point = (from p in this.DbContext.Point
                          where p.MemberID == memberId
                          && p.GiveTargetYear == (short)systemDatetimeService.TargetYear
                          && p.GiveTargetMonth == (short)systemDatetimeService.TargetMonth
                          orderby p.GiveTargetWeek descending, p.CreatedDate descending
                          select new MemberModel
                          {
                              MemberId = p.MemberID,
                              PossesionPoint = p.PossesionPoint,
                              FundsPoint = p.FundsPoint,
                              PayOffPoints = p.PayOffPoints
                          }
                        ).FirstOrDefault();

            if (point == null)
                return "0";

            return point.FormattedDisplayPoints;
        }

        /// <summary>
        /// 応募可能ポイント取得
        /// </summary>
        public int GetAvailablePointByMemberId(long memberId)
        {
            //Todo:GetOnlinePointのMemberIdの型が不適切。
            var Point = GetOnlinePoint((int)memberId);

            var availablePoint = Point - Constants.ExcludedPoint;

            return (availablePoint < 0) ? 0 : availablePoint;
        }
    }
}
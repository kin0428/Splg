using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Constant;
using Splg.Core.Models.Logging.Dto;
using Splg.Core.Services;
using Splg.Models;
using Splg.Models.Game.InfoModel;
using Splg.Services.Game;
using Splg.Services.System;
using Splg.Core.Extend;


namespace Splg.Services.Members
{
    public class NoticeService
    {
        // todo Splg.Utilsのメソッドを本クラスへ移動させる

        private ComEntities comEntities;

        private SystemDatetimeService systemDatetimeService;

        public NoticeService(ComEntities comEntities, SystemDatetimeService systemDatetimeService)
        {
            this.comEntities = comEntities ?? new ComEntities();
            this.systemDatetimeService = systemDatetimeService ?? new SystemDatetimeService();
        }

        /// <summary>
        /// Get Number of Unread Notice
        /// </summary>
        /// <param name="memberId">会員ID</param>
        /// <returns></returns>
        public int GetNumberOfUnreadNotice(long memberId)
        {
            var com = new ComEntities();
            DateTime oneMonthAgo = this.systemDatetimeService.Now.AddMonths(-1);
            //DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
            int result = 0;
            var query = from ni in com.NoticeInfo
                        join nds in com.NoticeDeliverySubject on ni.NoticeId equals nds.NoticeId
                        where (ni.NoticeClass == 1 || ni.NoticeClass == 3)
                        && nds.AlreadyReadFlg == false
                        && nds.MemberId == memberId
                        && nds.CreatedDate >= oneMonthAgo
                        //&& ni.NoticeId <= 4  //PointsPtが付与の仕様確定待ちのため暫定で絞る
                        select nds;
            if (query != null)
                result = query.Count();
            else
                result = 0;

            return result;
        }

        /// <summary>
        /// 試合情報の結果通知処理
        /// </summary>
        /// <param name="memberId">会員ID</param>
        /// <param name="fromDate">試合日付(FROM)</param>
        /// <param name="toDate">試合日付(TO)</param>
        /// <param name="createAccountId">ログイン中の会員ID</param>
        /// <returns>true:通知あり</returns>
        public bool NoticeExpectedGameInfo(long memberId, DateTime fromDate, DateTime toDate, long createAccountId)
        {
            // 予想情報を取得(試合終了,試合中止)
            var expectInfoService = new ExpectInfoService(this.comEntities);
            var expectationInfos = expectInfoService.GetExpectedGameInfo(memberId, fromDate, toDate).Where(e => new[] { 9, 10 }.Contains(e.GameStatus));
            
            // 未読の予想情報を抽出
            var unreadExpectationInfos = this.GetUnreadExpectionInfo(expectationInfos);

            var result = unreadExpectationInfos != null && unreadExpectationInfos.Any();

            // 履歴へ追加
            this.AddNoticePopupDisplayHistory(unreadExpectationInfos, createAccountId);

            return result;
        }

        /// <summary>
        /// 未読の予想情報を取得
        /// </summary>
        /// <param name="expectationInfos">試合予想情報</param>
        /// <returns>未読の試合予想情報</returns>
        public IEnumerable<ExpectationInfoModel> GetUnreadExpectionInfo(IEnumerable<ExpectationInfoModel> expectationInfos)
        {
            var read = from e in expectationInfos
                         join n in this.comEntities.NoticePopupDisplayHistory
                         on new { MembetId = e.MemberID, SportId = e.SportID, GameId = (int)e.GameID }
                         equals new { MembetId = n.MemberId, SportId = n.UniqueID, GameId = n.UniqueID2 }
                         select e;

            var unread = expectationInfos.Except(read);

            return unread;
        }

        /// <summary>
        /// お知らせポップアップ履歴を追加
        /// </summary>
        /// <param name="expectInfoModels"></param>
        /// <param name="createAccountId"></param>
        /// <returns></returns>
        public void AddNoticePopupDisplayHistory(IEnumerable<ExpectationInfoModel> expectInfoModels, long createAccountId)
        {
            if (expectInfoModels == null || !expectInfoModels.Any()) return;
            
            using (var transaction = this.comEntities.Database.BeginTransaction())
            {
                try
                {
                    foreach (var expectInfoModel in expectInfoModels)
                    {
                        this.comEntities.NoticePopupDisplayHistory.Add(
                            new NoticePopupDisplayHistory 
                            {
                                MemberId = expectInfoModel.MemberID,
                                ClassClass = 4, // 4:試合
                                UniqueID = expectInfoModel.SportID,
                                UniqueID2 = (int)expectInfoModel.GameID,
                                AlreadyReadFlg = true, // 1:既読
                                CreatedAccountID = createAccountId.ToString(),
                                ModifiedAccountID = createAccountId.ToString()
                            }
                        );
                    }

                    this.comEntities.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // TODO:とりあえずログ出力
                    // JS呼出時の集約例外処理の対応後に集約例外処理へthrow,ここでのRollbackは行わないように修正する
                    var writeLogFileService = new WriteLogFileService(WriteLogFileConst.ExceptionLoggerName);
                    //例外書き込み用
                    var exceptionLogModel = new ExceptionLogModel()
                    {
                        MemberId = HttpContext.Current.Session["CurrentUser"].GetNullableLong(),
                        Url = HttpContext.Current.Request.Url.UriString(),
                        UserAgent = HttpContext.Current.Request.UserAgent,
                        UrlReferrer = HttpContext.Current.Request.UrlReferrer.UriString(),
                        SessionId = HttpContext.Current.Session.SessionID,
                    };

                    int httpStatusCode = ex.GetHttpCode();

                    writeLogFileService.Fatal(ex, exceptionLogModel, httpStatusCode);
                    
                    transaction.Rollback();
                }
            }
        }

    }
}
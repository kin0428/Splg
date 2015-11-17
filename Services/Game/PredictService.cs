using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Splg.Models;
using Splg.Services.System;
using Splg.Core.Extend;

namespace Splg.Services.Game
{
    public class PredictService
    {
        private ComEntities comEntities;

        public PredictService(ComEntities comEntities)
        {
            this.comEntities = comEntities;
        }

        /// <summary>
        /// Cancel prediction : 3 step
        ///     1. Update record in table ExpectPoint.
        ///     2. Insert 2 records in table PointHistory.
        ///     3. Update possession point in table Point
        /// </summary>
        /// <param name="expectPointID">ExpectPointID</param>
        /// <param name="pointID">PointID</param>
        /// <param name="deletedPoint"></param>
        /// <param name="currentUser">ログイン中のユーザ</param>
        /// <returns>True : Cancel success.</returns>
        /// <returns>False : Cancel fail.</returns>
        public long CancelPrediction(string expectPointID, string pointID, int deletedPoint, long currentUser)
        {
            long newTotalPoint = 0;
            using (var dbContextTransaction = this.comEntities.Database.BeginTransaction())
            {
                try
                {
                    //if (Session["CurrentUser"] != null)
                    {
                        var memberID = "";
                        //var memberID = Session["CurrentUser"].ToString();
                        var expPointID = Convert.ToInt64(StringProtector.Unprotect(expectPointID));
                        var pID = Convert.ToInt64(StringProtector.Unprotect(pointID));

                        //0. Find expected point in table expectPoint.
                        //0.1 Not exist : rollback transaction.
                        //0.2 Exists : continue to step 1.
                        var expectRecord = this.comEntities.ExpectPoint.Where(m => m.ExpectPointID == expPointID).FirstOrDefault();
                        if (expectRecord != null)
                        {
                            //TODO SituationStatus 要確認（1,2以外のときを規定したほうがベター）
                            if (expectRecord.SituationStatus == 1)
                            {
                                //1. Update record in table ExpectPoint.
                                expectRecord.SituationStatus = 2;
                                expectRecord.ModifiedAccountID = memberID;
                                expectRecord.ModifiedDate = DateTime.Now;

                                //2. Insert 2 records in table PointHistory.
                                //First record.
                                PointHistory his = new PointHistory();
                                his.PointId = pID;
                                his.ExpectPointId = expPointID;
                                his.CampaignId = null;
                                his.PointClass = 3;
                                his.Points = deletedPoint;
                                his.HistoryClass = 6;
                                his.AdjustmentClass = 2;
                                his.OperationClass = 1;
                                his.Status = true;
                                his.CreatedAccountID = memberID;
                                his.CreatedDate = DateTime.Now;
                                this.comEntities.PointHistory.Add(his);

                                //Second record.
                                PointHistory his1 = new PointHistory();
                                his1.PointId = pID;
                                his1.ExpectPointId = expPointID;
                                his1.CampaignId = null;
                                his1.PointClass = 2;
                                his1.Points = deletedPoint;
                                his1.HistoryClass = 6;
                                his1.AdjustmentClass = 1;
                                his1.OperationClass = 1;
                                his1.Status = true;
                                his1.CreatedAccountID = memberID;
                                his1.CreatedDate = DateTime.Now;
                                this.comEntities.PointHistory.Add(his1);

                                //3. Update possession point in table Point : add deleted point to possession point.
                                var newTablePoint = this.comEntities.Point.Where(m => m.PointID == pID).FirstOrDefault();
                                var newPossPoint = newTablePoint.PossesionPoint + deletedPoint;
                                newTablePoint.PossesionPoint = newPossPoint;
                                newTablePoint.ModifiedAccountID = memberID;
                                newTablePoint.ModifiedDate = DateTime.Now;

                                this.comEntities.SaveChanges();
                                dbContextTransaction.Commit();
                                newTotalPoint = newPossPoint;
                            }
                            else if (expectRecord.SituationStatus == 2)
                            {
                                //Just return the possession point
                                var possPoint = this.comEntities.Point.Where(m => m.PointID == pID).Select(m => m.PossesionPoint).FirstOrDefault();
                                newTotalPoint = possPoint;
                                dbContextTransaction.Commit();
                            }
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                    //else
                    {
                        dbContextTransaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    //// TODO:とりあえずログ出力
                    //// JS呼出時の集約例外処理の対応後に集約例外処理へthrow,ここでのRollbackは行わないように修正する
                    //var writeLogFileService = new WriteLogFileService(WriteLogFileConst.ExceptionLoggerName);
                    ////例外書き込み用
                    //var exceptionLogModel = new ExceptionLogModel()
                    //{
                    //    MemberId = HttpContext.Current.Session["CurrentUser"].GetNullableLong(),
                    //    Url = HttpContext.Current.Request.Url.UriString(),
                    //    UserAgent = HttpContext.Current.Request.UserAgent,
                    //    UrlReferrer = HttpContext.Current.Request.UrlReferrer.UriString(),
                    //    SessionId = HttpContext.Current.Session.SessionID,
                    //};

                    //int httpStatusCode = ex.GetHttpCode();

                    //writeLogFileService.Fatal(ex, exceptionLogModel, httpStatusCode);

                    dbContextTransaction.Rollback();
                }
            }

            return newTotalPoint;
        }
    }
}
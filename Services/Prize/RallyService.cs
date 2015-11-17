using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Areas.Prize.Models;
using System.Data;
using Splg.Areas.Prize.Models.ViewModel;
using Splg.Models;
using Splg.Services.Point;
using System.Data.Entity.Validation;
using Splg.Core.Constant;

namespace Splg.Services.Prize
{
    public class RallyService
    {
        private PrizeEntities PrizeEntities { get; set; }
        private ComEntities ComEntities { get; set; }

        public RallyService(PrizeEntities prizeEntities, ComEntities comEntities = null)
        {
            PrizeEntities = prizeEntities;
            if (comEntities != null)
            {
                ComEntities = comEntities;
            }
        }

        /// <summary>
        /// 大会取得(Key:RallyId)
        /// </summary>
        public RallyViewModel GetRallyViewModelByRallyId(long rallyId)
        {
            var rallies = from rally in PrizeEntities.Rallies
                          where
                           rally.RallyId <= rallyId
                          select new RallyViewModel()
                          {
                              RallyId = rally.RallyId,
                              RallyName = rally.RallyName,
                              EntryStartDate = rally.EntryStartDate,
                              EntryEndDate = rally.EntryEndDate,
                              Description = rally.Description,
                              ImageUrl = rally.ImageUrl
                          };

            return rallies.FirstOrDefault();
        }

        /// <summary>
        /// 大会取得(Key:今日日付)
        /// </summary>
        public RallyViewModel GetRallyViewModelByToday(DateTime today)
        {
            var rallies = from rally in PrizeEntities.Rallies
                       where
                        rally.EntryStartDate <= today
                       && rally.EntryEndDate >= today
                       select new RallyViewModel()
                       {
                             RallyId = rally.RallyId,
                             RallyName = rally.RallyName,
                             EntryStartDate = rally.EntryStartDate,
                             EntryEndDate = rally.EntryEndDate,
                             Description = rally.Description,
                             ImageUrl = rally.ImageUrl
                       };

            return rallies.FirstOrDefault();
        }

        /// <summary>
        /// 大会取得(Key:過去分)
        /// </summary>
        public List<RallyViewModel> GetRallyViewModelAtPrevious(long rallyId)
        {
            var rallies = from rally in PrizeEntities.Rallies
                          where
                           rally.RallyId < rallyId
                          select new RallyViewModel()
                          {
                              RallyId = rally.RallyId,
                              RallyName = rally.RallyName,
                              EntryStartDate = rally.EntryStartDate,
                              EntryEndDate = rally.EntryEndDate,
                              Description = rally.Description,
                              ImageUrl = rally.ImageUrl
                          };

            return rallies.ToList();
        }

        /// <summary>
        /// 大会景品取得
        /// </summary>
        public List<RallyGoodViewModel> GetRallyGoodsViewModelsByRallyId(long rallyId)
        {
            var rallyGoods = (from rallyGood in PrizeEntities.RallyGoods
                          join goodSpec in PrizeEntities.GoodsSpec on rallyGood.GoodSpecId equals goodSpec.GoodSpecId
                          where
                           rallyGood.RallyId == rallyId
                          select new RallyGoodViewModel()
                          {
                              RallyGoodId = rallyGood.RallyGoodId,
                              GoodSpecId = rallyGood.GoodSpecId, 
                              RallyId = rallyGood.RallyId,
                              EntryLimit = rallyGood.EntryLimit,
                              EntryMethod = rallyGood.EntryMethod,
                              Points = rallyGood.Points,
                              WinVolume = rallyGood.WinVolume,
                              GoodName = goodSpec.GoodName,
                              GoodSubName = goodSpec.SubName,
                              Description = goodSpec.Description,
                              ThumbnailUrl = goodSpec.ThumbnailUrl,
                              FullImageUrl = goodSpec.FullImageUrl
                          }).ToList();


            return rallyGoods;
        }

        /// <summary>
        /// 大会景品取得
        /// </summary>
        public RallyGoodViewModel GetRallyGoodsViewModelByRallyGoodsId(long rallyGoodsId)
        {
            var rallyGoods = from rallyGood in PrizeEntities.RallyGoods
                             join goodSpec in PrizeEntities.GoodsSpec on rallyGood.GoodSpecId equals goodSpec.GoodSpecId
                             where
                              rallyGood.RallyGoodId == rallyGoodsId
                             select new RallyGoodViewModel()
                             {
                                 RallyGoodId = rallyGood.RallyGoodId,
                                 GoodSpecId = rallyGood.GoodSpecId,
                                 RallyId = rallyGood.RallyId,
                                 EntryLimit = rallyGood.EntryLimit,
                                 EntryMethod = rallyGood.EntryMethod,
                                 Points = rallyGood.Points,
                                 WinVolume = rallyGood.WinVolume,
                                 GoodName = goodSpec.GoodName,
                                 GoodSubName = goodSpec.SubName,
                                 Description = goodSpec.Description,
                                 ThumbnailUrl = goodSpec.ThumbnailUrl,
                                 FullImageUrl = goodSpec.FullImageUrl
                             };

            return rallyGoods.FirstOrDefault();
        }

        /// <summary>
        /// 大会景品補足取得
        /// </summary>
        public List<RallyGoodRemarksViewModel> GetRallyGoodsRemarksViewModel(long rallyGoodsId)
        {
            var rallyGoodsRemarks = (from rgr in PrizeEntities.RallyGoodsRemarks
                                    where rgr.RallyGoodId == rallyGoodsId
                                    select new RallyGoodRemarksViewModel
                                    {
                                        RallyGoodRemarksId = rgr.RallyGoodRemarksId,
                                        Title = rgr.Title,
                                        ImageUrl = rgr.ImageUrl,
                                        DisplayOrder = rgr.DisplayOrder
                                    }).ToList();

            rallyGoodsRemarks.Sort();

            return rallyGoodsRemarks;
        }

        /// <summary>
        /// 大会景品補足テキスト取得
        /// </summary>
        public List<RallyGoodRemarksTextViewModel> GetRallyGoodsRemarksTextViewModel(long rallyGoodsId)
        {
            var rallyGoodsRemarksText = (from rgrt in PrizeEntities.RallyGoodsRemarksText
                                         join rgr in PrizeEntities.RallyGoodsRemarks on rgrt.RallyGoodRemarksId equals rgr.RallyGoodRemarksId
                                         where rgr.RallyGoodId == rallyGoodsId
                                         select new RallyGoodRemarksTextViewModel
                                         {
                                             RallyGoodRemarksId = rgrt.RallyGoodRemarksId,
                                             Title = rgrt.Title,
                                             Contents = rgrt.Contents,
                                             DisplayOrder = rgrt.DisplayOrder
                                         }).ToList();

            rallyGoodsRemarksText.Sort();

            return rallyGoodsRemarksText;
        }

        /// <summary>
        /// 大会景品補足リンク取得
        /// </summary>
        public List<RallyGoodRemarksLinkViewModel> GetRallyGoodsRemarksLinkViewModel(long rallyGoodsId)
        {
            var rallyGoodsRemarksText = (from rgrl in PrizeEntities.RallyGoodsRemarksLink
                                         join rgr in PrizeEntities.RallyGoodsRemarks on rgrl.RallyGoodRemarksId equals rgr.RallyGoodRemarksId 
                                         where rgr.RallyGoodId == rallyGoodsId
                                         select new RallyGoodRemarksLinkViewModel
                                         {
                                             RallyGoodRemarksId = rgrl.RallyGoodRemarksId,
                                             Title = rgrl.Title,
                                             LinkUrl = rgrl.LinkUrl,
                                             DisplayOrder = rgrl.DisplayOrder
                                         }).ToList();

            rallyGoodsRemarksText.Sort();

            return rallyGoodsRemarksText;
        }
        
        /// <summary>
        /// 大会景品応募口数取得
        /// </summary>
        public int GetEntryCount(long memberId, long rallyGoodsId)
        {
            List<long> PointList = ComEntities.Point.Where(x => x.MemberID == memberId).Select(x => x.PointID).ToList();

            var entryList = PrizeEntities.EntryHistories.Where(x => x.RallyGoodId == rallyGoodsId).Where(x => PointList.Contains(x.PointId)).Select(x => x.EntryHistoryId);

            return entryList.Count();

        }
        /// <summary>
        /// 大会景品応募口数取得(現在時点の）
        /// </summary>
        public int GetTotalEntryCount(long rallyGoodsId)
        {
            var entryList = PrizeEntities.EntryHistories.Where(x => x.RallyGoodId == rallyGoodsId).Select(x => x.EntryHistoryId);
                
            return entryList.Count();
        }

        #region 懸賞に応募可能かチェックする(エラーの場合はエラーメッセージを返す)
        public string IsEntryPrizeCompetition(long memberId, int rallyGoodId, string entryCount)
        {
            // 数値入力判別用の変数セット
            int d = 0;
            // 初期値セット（空文字列）
            string result = string.Empty;

            // 数値が入力されていたか
            if (int.TryParse(entryCount,out d) == false)
            {
                result = "半角の数値を入力してください。";
            }
            else if (d == 0)
            {
                result = "口数には1以上の数値を入力してください。";
            }
            else
            {
                // 大会期間かのチェック
                if (IsDuringRally(rallyGoodId) == false)
                {
                    result = "大会期間外です。";
                }

                // 所持ポイントのチェック
                if (GetAfterAvailablePoint(memberId, rallyGoodId, d) < 0)
                {
                    result = "ポイントが不足しています。";
                }

                // 応募上限数(口数)のチェック
                if (IsValidEntryLimit(memberId, rallyGoodId, d) == false)
                {
                    result = "これ以上応募できません。";
                }

                // Todo:先着順パターン時の、当選数チェック
                //if (IsValidWinVolume(RallyGoodId, d) == false)
                //{
                //    result = "応募上限数に達したため、応募は締め切られました。";
                //}

            }


            return result;
        }

        public bool IsDuringRally(int RallyGoodId)
        {
            // 初期値セット
            bool result = false;

            var rally = (from rl in PrizeEntities.Rallies
                         join rg in PrizeEntities.RallyGoods on rl.RallyId equals rg.RallyId
                         where rg.RallyGoodId == RallyGoodId
                         && rl.EntryStartDate <= DateTime.Now
                         && rl.EntryEndDate >= DateTime.Now
                         select rl
                        );

            if (rally.Count() > 0 )
            {
                result = true;
            }

            return result;
        }

        public int GetAfterAvailablePoint(long memberId, int RallyGoodId, int EntryCount)
        {
            // 初期値セット
            int result;

            var pointInfoService = new PointInfoService(ComEntities);

            int AvailablePoint = pointInfoService.GetAvailablePointByMemberId(memberId);

            int rallyPoints = (from rg in PrizeEntities.RallyGoods 
                         where rg.RallyGoodId == RallyGoodId
                         select rg.Points
                        ).FirstOrDefault();

            int FinallyPoint = AvailablePoint - (rallyPoints * EntryCount);

            result = FinallyPoint;

            return result;
        }

        /// <summary>
        /// 大会景品取得(Key:大会懸賞Id)
        /// </summary>
        private RallyGoods GetRallyGoodsByRallyGoodId(int rallyGoodId)
        {
            return PrizeEntities.RallyGoods.Where(m => m.RallyGoodId == rallyGoodId).FirstOrDefault();        
        }

        /// <summary>
        /// 応募上限数判定
        /// </summary>
        public bool IsValidEntryLimit(long memberId, int rallyGoodId, int EntryCount)
        {
            var alreadyEntryCount = GetEntryCount(memberId, rallyGoodId);

            var rallyGoods = GetRallyGoodsByRallyGoodId(rallyGoodId);

            return (
                (rallyGoods.EntryLimit == 0)
                || (rallyGoods.EntryLimit >= (alreadyEntryCount + EntryCount))
                    );
        }

        /// <summary>
        /// 当選数判定
        /// </summary>
        public bool IsValidWinVolume(int rallyGoodId, int EntryCount)
        {
            var TotalEntryCount = GetTotalEntryCount(rallyGoodId);

            var rallyGoods = GetRallyGoodsByRallyGoodId(rallyGoodId);

            return (
                (rallyGoods.EntryMethod == (int)PrizeConst.EntryMethod.Draw)
                || (rallyGoods.WinVolume >= (TotalEntryCount + EntryCount))
                    );
        }
        
        #endregion

        #region 応募処理
        public bool EntryPrizeCompetition(long memberId,int RallyGoodId, int EntryCount, short EntryMethod)
        {
            // 初期値を設定
            bool result = false;
            short year = (short)DateTime.Now.Year;
            short month = (short)DateTime.Now.Month;

            // 今月のポイントID取得
            long PointId = ComEntities.Point.Where(x => x.MemberID == memberId)
                                            .Where(x => x.GiveTargetYear == year)
                                            .Where(x=> x.GiveTargetMonth == month)
                                            .Select(x => x.PointID).FirstOrDefault();

            using (var PrizeTransaction = PrizeEntities.Database.BeginTransaction())
            {
                try
                {

                    // 応募履歴テーブルのデータ作成
                    EntryHistories entryHistories = new EntryHistories();
                    for (int i = 1; i <= EntryCount; i++)
                    {
                        // データを入れる
                        entryHistories.PointId = PointId;
                        entryHistories.RallyGoodId = RallyGoodId;
                        entryHistories.EntryDateTime = DateTime.Now;
                        entryHistories.CreatedAccountID = memberId.ToString();
                        entryHistories.CreatedDate = DateTime.Now;

                        // 登録処理
                        PrizeEntities.EntryHistories.Add(entryHistories);
                        PrizeEntities.SaveChanges();
                    }

                    // ポイント消費
                    using (var ComTransaction = ComEntities.Database.BeginTransaction())
                    {
                        try
                        {
                            // 今回減るポイントを計算
                            int EntryCost = PrizeEntities.RallyGoods.Where(x => x.RallyGoodId == RallyGoodId).Select(x => x.Points).FirstOrDefault();
                            int TotalEntryCost = EntryCost * EntryCount;

                            // 応募履歴テーブルのデータ作成
                            var point = ComEntities.Point.Where(x => x.PointID == PointId).FirstOrDefault();
                            // データを入れる
                            int nowPoint = point.PossesionPoint;
                            point.PossesionPoint = nowPoint - TotalEntryCost;

                            // 更新処理
                            ComEntities.SaveChanges();

                            // コミット
                            ComTransaction.Commit();
                    
                        }
                        catch (DbEntityValidationException e)
                        {
                            ComTransaction.Rollback();
                            //Todo: エラーキャッチしたらraiseできないかしら
                            throw e;
                        }

                    }

                    // コミット
                    PrizeTransaction.Commit();
                    // 成功でtrueを返す
                    result = true;
                    return result;
                }
                catch (DbEntityValidationException e)
                {
                    // TODO:とりあえずログ出力
                    // JS呼出時の集約例外処理の対応後に集約例外処理へthrow,ここでのRollbackは行わないように修正する
                    PrizeTransaction.Rollback();
                    return result;
                }
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;

namespace Splg.Areas.Prize.Models.ViewModel
{
    public class RallyGoodViewModel
    {
        /// <summary>
        /// 大会景品Id
        /// </summary>
        public long RallyGoodId { get; set; }

        /// <summary>
        /// 大会Id
        /// </summary>
        public long RallyId { get; set; }
        
        /// <summary>
        /// 景品Id
        /// </summary>
        public long GoodSpecId { get; set; }

        /// <summary>
        /// 景品名
        /// </summary>
        public string GoodName { get; set; }

        /// <summary>
        /// 景品名補足
        /// </summary>
        public string GoodSubName { get; set; }

        /// <summary>
        /// サムネイル画像
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// フルイメージ画像
        /// </summary>
        public string FullImageUrl { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 応募方式
        /// </summary>
        public short EntryMethod { get; set; }

        /// <summary>
        /// 応募数制限
        /// </summary>
        public short EntryLimit { get; set; }

        /// <summary>
        /// 必要ポイント
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]          
        public int Points { get; set; }

        /// <summary>
        /// 当選本数
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]  
        public int WinVolume { get; set; }

        /// <summary>
        /// 当選本数表示
        /// </summary>
        public string WinVolumeView
        {
            get
            {
                var replaceString = WinVolume.ToString("#,##0");

                if (EntryMethod == (int)PrizeConst.EntryMethod.Buy)
                {
                    return "先着で{0}名様".Replace("{0}", replaceString);
                }
                else if (EntryMethod == (int)PrizeConst.EntryMethod.Draw)
                {
                    return "抽選で{0}名様".Replace("{0}", replaceString);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
       
    }
}
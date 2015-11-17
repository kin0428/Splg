using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Core.Constant;
using System.ComponentModel.DataAnnotations;

namespace Splg.Areas.Jleague.Models.Dto
{
    /// <summary>
    /// 関連記事（投稿記事、ニュース 混合表示用）
    /// </summary>
    public class RelatedArticle
    {
        /// <summary>
        /// 記事種類
        /// </summary>
        public　JlgConst.ArticleKind ArticleKind {get;set;}

        /// <summary>
        /// 発効日
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsDateAndDayOfWeekPlusTimeWithSlashSeparator)]
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// 記事ルート名
        /// </summary>
        public string ArticleRouteName { get; set; }

        /// <summary>
        /// アイコンリンクルート名
        /// </summary>
        public string IconsRouteName { get; set; }

        /// <summary>
        /// アイコンUrl
        /// </summary>
        public string IconsUrl { get; set; }

        /// <summary>
        /// キー名
        /// </summary>
        public long Key { get; set; }

        /// <summary>
        /// トピックId
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// 記事オーナーのメンバーId
        /// </summary>
        public long OwnersMemberId { get; set; }

        /// <summary>
        /// 記事オーナー
        /// </summary>
        public string ArticleOwner { get; set; }
        /// <summary>
        /// 記事タイトル
        /// </summary>
        public string Title { get; set; }
    }
}
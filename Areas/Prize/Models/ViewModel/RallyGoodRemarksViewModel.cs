using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;

namespace Splg.Areas.Prize.Models.ViewModel
{
    public class RallyGoodRemarksViewModel : System.IComparable
    {
        /// <summary>
        /// 補足Id
        /// </summary>
        public long RallyGoodRemarksId { get; set; }

        /// <summary>
        /// 補足タイトル
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 補足イメージURL
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 表示順
        /// </summary>
        public short DisplayOrder { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (this.GetType() != obj.GetType())
                throw new ArgumentException("別の型とは比較できません。", "obj");

            RallyGoodRemarksTextViewModel obj2 = (RallyGoodRemarksTextViewModel)obj;

            int comp = this.DisplayOrder - obj2.DisplayOrder;

            return comp;
        }

    }
}
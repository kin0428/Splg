using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.Dto
{
    public class PassIndex
    {
        /// <summary>
        /// パス本数
        /// </summary>
        public int Pass{get;set;}

        /// <summary>
        /// パス成功数
        /// </summary>
        public int PassSucceed { get; set; }

        /// <summary>
        /// ショートパス数
        /// </summary>
        public int ShortPassCount { get; set; }

        /// <summary>
        /// ミドルパス数
        /// </summary>
        public int MiddlePassCount { get; set; }

        /// <summary>
        /// ロングパス数
        /// </summary>
        public int LongPassCount { get; set; }

        /// <summary>
        /// パス失敗数
        /// </summary>
        public int PassFailed 
        {
            get 
            {
                return Pass - PassSucceed;
            }
        }

    }
}
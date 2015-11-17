using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Constant
{
    public class PrizeConst
    {
        public enum EntryMethod
        { 
            /// <summary>
            /// もれなく
            /// </summary>
            Buy = 1,

            /// <summary>
            /// 抽選
            /// </summary>
            Draw = 2        
        }
    }
}
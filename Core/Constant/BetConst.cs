using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Constant
{
    public class BetConst
    {
        public enum BetSelectID
        { 
            /// <summary>
            /// ホームの勝ち
            /// </summary>
            Home = 1,

            /// <summary>
            /// ビジターの勝ち
            /// </summary>
            Visitor = 2,        

            /// <summary>
            /// 引き分け
            /// </summary>
            Draw = 3        
        }
    }
}
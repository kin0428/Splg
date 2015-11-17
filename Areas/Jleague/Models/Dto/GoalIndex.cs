using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.Jleague.Models.Dto
{
    /// <summary>
    /// 得失点指標
    /// </summary>
    public class GoalIndex
    {
        /// <summary>
        /// 得失点指標（PK）
        /// </summary>
        public int AtPk{get;set;}
        
        /// <summary>
        /// 得失点指標（セットプレー直接）
        /// </summary>
	    public int AtSetPlayDirectly{get;set;}

        /// <summary>
        /// セットプレーから
        /// </summary>
	    public int AtSetPlay {get;set;}

        /// <summary>
        /// クロスから
        /// </summary>
        public int AtCross{get;set;}

        /// <summary>
        /// スルーパスから
        /// </summary>
        public int AtThroughPass{get;set;}

        /// <summary>
        /// ショートパスから
        /// </summary>
        public int AtShortPass{get;set;}

        /// <summary>
        /// ロングパスから
        /// </summary>
        public int AtLongPass{get;set;}

        /// <summary>
        /// ドリブルから
        /// </summary>
	    public int AtDribble{get;set;}

        /// <summary>
        /// こぼれ球から
        /// </summary>
	    public int AtLooseBall{get;set;}

        /// <summary>
        /// その他
        /// </summary>
        public int AtOther{get;set;}
    }
}
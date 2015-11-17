using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Extend
{
    /// <summary>
    /// Object型の拡張メソッド
    /// </summary>
    /// <remarks>
    /// 自由すぎてアレなので注意して利用してください。
    /// </remarks>
    public static class ObjectExtend
    {
        /// <summary>
        /// NullableLong型取得
        /// </summary>
        ///<remarks>
        /// NullableLong型を取得します。
        /// ※　型の整合性は担保しません。
        /// 変換可能かどうかの検査は必要に応じて行ってください。
        /// </remarks>
        public static long? GetNullableLong(this object target)
        {
            if (target == null)
            {
                return null;
            }
            else
            {
                return Convert.ToInt64(target);
            }
        }

        /// <summary>
        /// 文字列型取得
        /// </summary>
        ///<remarks>
        /// 文字列型を取得します。
        /// </remarks>
        public static string GetString(this object target)
        {
            if (target == null)
            {
                return null;
            }
            else
            {
                return target.ToString();
            }
        }
    }
}
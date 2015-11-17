using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Core.Models.Flotr2.ScriptParameter
{
    public class ContainerParameterKey
    {
        /// <summary>
        /// コンテナ
        /// </summary>
        public static readonly string Container = "@Container";

        /// <summary>
        /// データ
        /// </summary>
        public static readonly string Data = "@Data";

        /// <summary>
        /// ラベル
        /// </summary>
        public static readonly string Label = "@Label";

        public static readonly string ContainerId = "@ContainerId";

        /// <summary>
        /// trackerFormatter用
        /// 変数識別用のキー（変数1とか変数2の数字部分のようなもの）
        /// </summary>
        public static readonly string Suffix = "@Suffix";

        /// <summary>
        /// trackerFormatter用
        /// プレイヤーIDリスト
        /// </summary>
        public static readonly string PlayerIDList = "@PlayerIDList";

        /// <summary>
        /// trackerFormatter用
        /// バリュー（攻撃力とか守備力）のリスト
        /// </summary>
        public static readonly string ValueList = "@ValueList";

    }
}
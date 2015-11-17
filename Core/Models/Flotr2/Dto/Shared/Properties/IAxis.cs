using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splg.Core.Models.Flotr2.Dto.Shared.Properties
{
    public interface IAxis
    {
        /// <summary>
        /// ラベリング表示可否
        /// </summary>
        bool IsVisibleLabels{ get; set; } 
         
        /// <summary>
        /// 最小値（軸上）
        /// </summary>
        decimal MinValue{ get; set; }

        /// <summary>
        /// 最大値（軸上）
        /// </summary>
        decimal MaxValue { get; set; }

    }
}

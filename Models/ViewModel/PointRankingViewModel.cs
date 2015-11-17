#region  Author Information
/*  
 * 
 * Createdted      : Tran Sy Huynh
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;

namespace Splg.Models.ViewModel
{
    public class PointRankingViewModel
    {
        public long MemberId { get; set; }

        public string Nickname { get; set; }
        
        public string ProfileImage { get; set; }

        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]
        public int Point { get; set; }

        public int Ranking { get; set; }
    }
}
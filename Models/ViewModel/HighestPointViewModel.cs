#region  Author Information
/*  
 * 
 * Createdted      : Tran Sy Huynh
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models;
#endregion

namespace Splg.Models.ViewModel
{
    public class HighestPointViewModel
    {
        public long MemberID { get; set; }
        public string MemberName { get; set; }
        public string ProfileImg { get; set; }
        public int Point { get; set; }
    }
}
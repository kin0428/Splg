#region (c) 2015 Prime Labo - All rights reserved
/*                                      COPYRIGHT NOTICE
 * -------------------------------------------------------------------------------------
 * All materials (including but not limited to source code, compiled assemblies, images,
 * resources, etc.) are copyrighted to Prime Labo. No usage is allowed unless permitted 
 * by written consent. You may not use, reverse-engineer these materials under any
 * circumstances.
 * 
 *                                    PROJECT DESCRIPTION
 * -------------------------------------------------------------------------------------
 * Namespace	: Splg.Areas.MyPage.Models
 * Class		: MyPageTopViewModel
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Splg.Models.News.ViewModel;
using Splg.Models.Game.ViewModel;
using System.ComponentModel.DataAnnotations;
#endregion

namespace Splg.Areas.MyPage.Models.InfoModel
{
    public class MyExpectedListInfoModel
    {


        public class MyPointInfoModel
        {
            public long ExpectPointID { get; set; }
            public long ExpectTargetID { get; set; }
            public short GiveTargetYear { get; set; }
            public short GiveTargetMonth { get; set; }
            public int SportsID { get; set; }
            public short Status { get; set; }
            public Nullable<int> FixBetSelectID { get; set; }
            public long PointID { get; set; }
            public int ExpectPoint { get; set; }
            public Nullable<int> AcquisitionPoint { get; set; }
            public Nullable<short> ClassClass { get; set; }
            public int GameID { get; set; }
            public Nullable<int> BetSelectID { get; set; }
            public Nullable<short> VorD { get; set; }
            public short SituationStatus { get; set; }
            public DateTime BetStartDate { get; set; }
            public DateTime? StartScheduleDate { get; set; }

            private decimal odds;

            public decimal Odds
            {
                get
                {
                    decimal reault = Math.Round(odds, 1, MidpointRounding.AwayFromZero);
                    return reault;
                }
                set { odds = value; }
            }
        }

        public class ExpectChangeModel
        {
            public int SportsID { get; set; }
            public int GameID { get; set; }
            public int ExpectPoint { get; set; }
            public int OldExpectPoint { get; set; }

        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Models.ViewModel
{
    public class HomeGameInfoViewModel
    {
        public int SportID { get; set; }
        public int GameID { get; set; }
    }

    public class HomeGameInfoViewModel2 : HomeGameInfoViewModel
    {
        public long ExpectTargetID { get; set; }
        public int ExpectPoint { get; set; }

        public int SportID { get; set; }
        public int GameID { get; set; }
    }

}
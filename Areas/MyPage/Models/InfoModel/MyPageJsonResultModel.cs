using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.MyPage.Models.InfoModel
{
    public class MyPageJsonResultModel
    {
        public bool HasError { set; get; }
        public string Message { set; get; }
        public string FilePath { set; get; }
    }
}
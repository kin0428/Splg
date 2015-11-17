using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splg.Areas.MyPage.Models.ViewModel
{
    public class MyPageRightColumnViewModel
    {
       public IEnumerable<MyPageGroupListViewModel.GroupListInfo> groupList  { get; set; }

    }
}
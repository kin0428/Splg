#region Author Information
/*
 * Developer	: Tran Sy Huynh
 * 
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Models;
using Splg.Areas.Jleague.Models.ViewModel;
#endregion

namespace Splg.Areas.Jleague.Controllers
{
    public class JlgRightRecentNewsController : Controller
    {
        ComEntities com = new ComEntities();
        // GET: Jleague/JlgRightRecentNews
        public ActionResult ShowJlgRightRecentNews(int jType = 0)
        {
            ViewBag.JType = jType;
            return PartialView("_JleagueRightRecentNews", GetRecentNews());
        }

        private IEnumerable<BriefNews> GetRecentNews()
        {
            var query = (from brief in com.BriefNews
                     join itpc in com.ItpcSubject on brief.NewsItemID equals itpc.NewsItemID
                     join itpcsm in com.ItpcSubjectMaster on itpc.IptcSubjectCode equals itpcsm.IptcSubjectCode
                     where brief.Status == Constants.NEWS_VALID_STATUS && brief.CarryLimitDate >= DateTime.Now &&
                         itpcsm.IptcSubjectCode == Constants.JLEAGUE_ITPCSUBJECTCODE
                     orderby brief.DeliveryDate descending
                     select brief).Take(5);
            return query;
        }
    }
}
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
 * Namespace	: Splg.Areas.User.Controllers
 * Class		: UserArticleController
 * Developer	: Nojima
 * 
 */
#endregion

#region Using directives
using Splg.Areas.Jleague.Models;
using Splg.Areas.Mlb.Models;
using Splg.Areas.Npb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Splg.Models;
using Splg.Areas.User.Models.ViewModel;
using Splg.Areas.MyPage;
using Splg.Models.Game.InfoModel;

#endregion

namespace Splg.Areas.User.Controllers
{
    public class UserExpectedListController : Controller
    {
        private const int CLASSCLASS_TEAM = 2;
        private const int CLASSCLASS_GAME = 4;

        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();

        NpbEntities npb = new NpbEntities();
        JlgEntities jlg = new JlgEntities();
        MlbEntities mlb = new MlbEntities();
        /// <summary>
        /// Declare context JLeague to get db.
        /// </summary>
        //UserEntities user = new UserEntities();

        #endregion

        //
        // GET: /User/UserExpectedList/
        public ActionResult Index(long memberId)
        {
            UserExpectedListViewModel viewModel = new UserExpectedListViewModel();

            Int64 memberID = memberId;
            Member member = Utils.GetMember(memberId);

            #region 年月用変数の設定
            DateTime dtToday = DateTime.Today;
            viewModel.LastYear = dtToday.AddYears(-1).Year.ToString(); // 昨年
            viewModel.ThisYear = dtToday.Year.ToString(); // 今年
            viewModel.NextYear = dtToday.AddYears(1).Year.ToString(); // 来年
            int iYear = dtToday.Year;
            int iMonth = dtToday.Month;
            int i;
            for (i = 1; i <= 3; i++)
            {
                if (i == 2) // 必ず真中が対象年になる
                {
                    viewModel.YearListClass[i - 1] = "class=active"; //　アクティブ月のとき'class="active"'、そうでないとき''
                }
                else
                {
                    viewModel.YearListClass[i - 1] = ""; //　アクティブ月のとき'class="active"'、そうでないとき''
                }
            }
            for (i = 1; i <= 12; i++)
            {
                if (i == iMonth)
                {
                    viewModel.MonthListClass[i - 1] = "class=active"; //　アクティブ月のとき'class="active"'、そうでないとき''
                }
                else
                {
                    viewModel.MonthListClass[i - 1] = ""; //　アクティブ月のとき'class="active"'、そうでないとき''
                }
            }
            #endregion

            viewModel.MemberId = memberId;

            ViewBag.OtherMemberID = memberId;
            ViewBag.OtherMemberNickName = member.Nickname;

            return View(viewModel);
        }


        #region Get Point Of User
        /// <summary>
        /// Get point of user when user login.   <-SCpied from NpbRightContentController.GetPointOfUser
        /// </summary>
        /// <returns>List Game predicted.</returns>
        public List<GamePointInfoModel> GetPointOfUser(long memberID, int sportsId, DateTime startDate, DateTime endDate)
        {
            var query = default(List<GamePointInfoModel>);

            List<long> lstPointID = com.Point.Where(m => m.MemberID == memberID && DbFunctions.TruncateTime(m.BetStartDate) >= startDate.Date && DbFunctions.TruncateTime(m.BetEndDate) <= endDate.Date).Select(m => m.PointID).ToList();

            //Get these games that not end : not start or ongoing.
            var expectations = from et in com.ExpectTarget
                               join ep in com.ExpectPoint on et.ExpectTargetID equals ep.ExpectTargetID
                               where (et.FixBetSelectID == 0 || et.FixBetSelectID == null)    //確定BET選択肢ID
                             && lstPointID.Contains(ep.PointID)
                             && ep.SituationStatus != 2                 //状況ステータス １・・予想、２・・キャンセル、３・・中止、４・・結果確定
                             && et.SportsID == sportsId
                               select new
                               {
                                   et,
                                   ep
                               };

            //Get all prediction from start to end.
            if (expectations.Count() > 0)
            {
                query = (from p in com.Point
                         join q1 in expectations on p.PointID equals q1.ep.PointID
                         //from q1 in tmp1.DefaultIfEmpty()
                         where p.MemberID == memberID && q1.et != null && q1.ep != null
                         && DbFunctions.TruncateTime(p.BetStartDate) >= startDate.Date
                         && DbFunctions.TruncateTime(p.BetEndDate) <= endDate.Date
                         select new GamePointInfoModel
                         {
                             SportsID = sportsId,
                             PointID = p.PointID,
                             ExpectPoint = q1.ep.ExpectPoint1,
                             ExpectPointID = q1.ep == null ? 0 : q1.ep.ExpectPointID,
                             BetStartDate = p.BetStartDate,
                             GiveTargetMonth = p.GiveTargetMonth,
                             BetSelectID = (q1.ep == null ? 0 : (q1.ep.BetSelectID.Value == null ? 0 : q1.ep.BetSelectID.Value)),
                             GameID = q1.et.UniqueID,

                             Odds = (q1.ep == null || q1.et == null) ? (decimal)0 : (from od in com.OddsInfo
                                                                                     join bsm in com.BetSelectMaster on od.BetSelectMasterID equals bsm.BetSelectMasterID
                                                                                     where q1.ep.BetSelectID.ToString() == bsm.BetSelectMasterID.ToString() && od.ClassificationType == 2
                                                                                         && od.ExpectTargetID == q1.et.ExpectTargetID
                                                                                     select od.Odds).FirstOrDefault(),
                             StartScheduleDate = q1.et == null ? DateTime.MinValue : (q1.et.StartScheduleDate.Value == null ? DateTime.MinValue : q1.et.StartScheduleDate.Value),


                             AcquisitionPoint = q1.ep == null ? 0 : q1.ep.AcquisitionPoint,
                             ClassClass = q1.ep == null ? 0 : q1.ep.ClassClass,
                             ExpectTargetID = q1.et == null ? 0 : q1.et.ExpectTargetID,
                             FixBetSelectID = q1.et == null ? 0 : q1.et.FixBetSelectID,
                             SituationStatus = q1.ep == null ? (short)0 : q1.ep.SituationStatus,    //状況ステータス １・・予想、２・・キャンセル、３・・中止、４・・結果確定

                             Status = q1.et == null ? (short)0 : q1.et.Status,
                             VorD = q1.ep == null ? (short)0 : q1.ep.VorD
                         }).ToList();
            }

            return query;
        }
        #endregion

        #region Json Result

        #region Change YearMonth
        /// <summary>
        /// Change YearMonth
        /// </summary>
        /// <returns>   none
        [HttpPost]
        public JsonResult ChangeYearMonth(string yearmonth, long user_id)
        {
            UserExpectedListViewModel ViewModel = new UserExpectedListViewModel();
            UserExpectedListViewModel.AjaxResulModel result = new UserExpectedListViewModel.AjaxResulModel();
            result.Games = new List<GameInfoModel> { };
            Int64 memberID = user_id;
            int iYear = Convert.ToInt16(yearmonth.Split('-')[0]);
            int iMonth = Convert.ToInt16(yearmonth.Split('-')[1]);
            ViewModel.MemberId = user_id;

            DateTime dt = DateTime.Today;

            result.Games = MyPageCommon.GetGameInfo(memberID, iYear, iMonth);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #endregion


    }
}
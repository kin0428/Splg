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
 * Namespace	: Splg.Areas.Jleague.Controllers
 * Class	    : JlgTeamInfoPlayerController
 * Developer	: e-concier
 * Updated date : 2015-04-27
 */
#endregion

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Splg.Areas.Npb.Models;
using Splg.Areas.Npb.Models.ViewModel;
using Splg.Areas.Npb.Models.ViewModel.InfosModel;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
#endregion

namespace Splg.Areas.Jleague.Controllers
{
    public class JlgTeamInfoPlayerController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        JlgEntities jlg = new JlgEntities();
        #endregion

        #region "Action Result"
        // GET: Npb/NpbTeamInfoPlayer
        public ActionResult Index(string teamID, string typeID)
        {
            int inTeamID = int.MinValue;
            int inTypeID = int.MinValue;
            Int32.TryParse(teamID, out inTeamID);
            Int32.TryParse(typeID, out inTypeID);

            ViewBag.TeamID = teamID;
            ViewBag.TeamName = jlg.TeamInfoTE.Where(x => x.TeamID == inTeamID).Select(x => x.TeamName).FirstOrDefault();
            ViewBag.TeamInfoMenuTabActive = (int)JlgConstants.TeamInfoMenu.TabActive_4;

            int jType = JlgCommon.GetJlgType(Request.Url.AbsoluteUri);
            ViewBag.JleagueMenu = jType == 1 ? 2 : 3;
            ViewBag.JleagueSubMenu = 6;
            ViewBag.JleagueTeamMenu = 4;
            ViewBag.JType = jType;
            JlgTeamInfoPlayerViewModel jlgTeamInfoPlayerViewModel = new JlgTeamInfoPlayerViewModel();
            jlgTeamInfoPlayerViewModel.TeamInfoPlayerInfosGK = GetTeamInfoPlayerInfos(inTeamID,"GK");
            jlgTeamInfoPlayerViewModel.TeamInfoPlayerInfosDF = GetTeamInfoPlayerInfos(inTeamID, "DF");
            jlgTeamInfoPlayerViewModel.TeamInfoPlayerInfosMF = GetTeamInfoPlayerInfos(inTeamID, "MF");
            jlgTeamInfoPlayerViewModel.TeamInfoPlayerInfosFW = GetTeamInfoPlayerInfos(inTeamID, "FW");

            return View(jlgTeamInfoPlayerViewModel);
        }

        #endregion

        public IEnumerable<JlgTeamInfoPlayerInfos> GetTeamInfoPlayerInfos(int inTeamCD, string position)
        {
            var TeamInfoPlayerInfosQuery = (from playerHeader in jlg.DirectoryDI
                                                                            join playerInfo in jlg.PlayerInfoDI on playerHeader.DirectoryDIId equals playerInfo.DirectoryDIId
                                                                            where playerHeader.TeamID == inTeamCD && playerInfo.Position == position
                                                                            select new JlgTeamInfoPlayerInfos
                                                                            {
                                                                                TeamID = playerHeader.TeamID,
                                                                                PlayerNo = playerInfo.PlayerNo,
                                                                                PlayerID = playerInfo.PlayerID,
                                                                                RealName = playerInfo.RealName,
                                                                                RealNameK = playerInfo.RealNameK,
                                                                                PlayerName = playerInfo.PlayerName,
                                                                                PlayerNameS = playerInfo.PlayerNameS,
                                                                                PlayerNameKS = playerInfo.PlayerNameKS,
                                                                                PlayerNameE = playerInfo.PlayerNameE,
                                                                                PlayerNameES = playerInfo.PlayerNameES,
                                                                                UniformNO = playerInfo.UniformNO,
                                                                                Position = playerInfo.Position,
                                                                                Class = playerInfo.Class,
                                                                                NewcomerFlag = playerInfo.NewcomerFlag,
                                                                                HomeTown = playerInfo.HomeTown,
                                                                                Blood = playerInfo.Blood,
                                                                                Height = playerInfo.Height,
                                                                                Weight = playerInfo.Weight,
                                                                                Birthday = playerInfo.Birthday,
                                                                                Career = playerInfo.Career,
                                                                                IndividualTitle = playerInfo.IndividualTitle,
                                                                                PlayerTeamHistory = playerInfo.PlayerTeamHistory
                                                                            });

            List<JlgTeamInfoPlayerInfos> TeamInfoPlayerInfos = TeamInfoPlayerInfosQuery.ToList();
            TeamInfoPlayerInfos.Sort();

            return TeamInfoPlayerInfos;
        }
    }
}
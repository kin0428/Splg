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
 * Namespace	: Splg.Areas.Npb.Controllers
 * Class		: NpbTeamInfoPlayerController
 * Developer	: Nguyen Minh Kiet
 * Updated date : 2015-03-12
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

#endregion


namespace Splg.Areas.Npb.Controllers
{
    public class NpbTeamInfoPlayerController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        NpbEntities npb = new NpbEntities();
        #endregion

        #region "Action Result"
        // GET: Npb/NpbTeamInfoPlayer
        public ActionResult Index(string teamID, string typeID)
        {
            int inTeamCD = int.MinValue;
            int inTypeID = int.MinValue;
            Int32.TryParse(teamID, out inTeamCD);
            Int32.TryParse(typeID, out inTypeID);
            ViewBag.TeamID = teamID;
            NpbTeamInfoPlayerViewModel npbTeamInfoPlayerViewModel = new NpbTeamInfoPlayerViewModel();    
            switch (inTypeID)
            {
                case (int)NpbConstants.TypeID.TypeOne:
                    ///
                    ///Get PitchingInfos
                    ///
                    ViewBag.TeamInfoMenuTabActive = (int)NpbConstants.TeamInfoMenu.TabActive_4;
                    npbTeamInfoPlayerViewModel.TeamInfoPitchingInfos = GetTeamInfoPitchingInfos(inTeamCD);
                    break;
                case (int)NpbConstants.TypeID.TypeTwo:
                    ///
                    ///Get CatcherFielderInfos
                    ///
                    ViewBag.TeamInfoMenuTabActive = (int)NpbConstants.TeamInfoMenu.TabActive_5;
                    npbTeamInfoPlayerViewModel.TeamInfoCatcherFielderInfos = GetCatcherFielderInfos(inTeamCD);
                    break;
                case (int)NpbConstants.TypeID.TypeThree:
                    ///
                    ///Get DirectorStaffInfos
                    ///
                    ViewBag.TeamInfoMenuTabActive = (int)NpbConstants.TeamInfoMenu.TabActive_6;
                    npbTeamInfoPlayerViewModel.TeamInfoDirectorStaffInfos = GetTeamInfoDirectorStaffInfos(inTeamCD);
                    break;     
            }         
            return View(npbTeamInfoPlayerViewModel);
        }

        #endregion

        #region Get TeamInfoPitchingInfos
        /// <summary>
        /// 8-5-4
        /// Join 1 table PitchingStats  to get :
        /// 1. PitchingStats : TeamCD, Num, PlayerCD,Name,EarnedRunAverage, Game,  Win,  Lost,Save
        /// </summary>       
        /// <returns>下記、自チームID でアクセスして取得する。初期表示では、背番号で昇順にソートして出力する。</returns>
        public IEnumerable<NpbTeamInfoPlayerInfos> GetTeamInfoPitchingInfos(int inTeamCD)
        {
            IEnumerable<NpbTeamInfoPlayerInfos> varTeamInfoPitchingInfos = (from playerInfoMST in npb.PlayerInfoMST
                                                                            join playerHeader in npb.PlayerInfoMSTHeader on playerInfoMST.PlayerInfoMSTHeaderId equals playerHeader.PlayerInfoMSTHeaderId
                                                                            join pitchingStats in npb.PitchingStats on playerInfoMST.PlayerCD equals pitchingStats.PlayerCD into playerStats
                                                                            from player in playerStats.DefaultIfEmpty()
                                                                            join pitchingStatsHeader in npb.PitchingStatsHeader on player.PitchingStatsHeaderId equals pitchingStatsHeader.PitchingStatsHeaderId into playerStatsH
                                                                            from playerh in playerStatsH.DefaultIfEmpty()
                                                                            where (playerHeader.TeamCD == inTeamCD
                                                                                    && playerInfoMST.PitchField  ==1)
                                                                            where (player.TeamCD == inTeamCD)
                                                                            where (playerh.GameAssortment != Constants.OFFICIALSTATS_GAMEASSORTMENT_EXCHANGE_GAME)
                                                                            select new NpbTeamInfoPlayerInfos
                                                                                {
                                                                                    TeamCD = playerHeader.TeamCD,
                                                                                    Num = string.IsNullOrEmpty(player.Num) ? playerInfoMST.UniformNO : player.Num,
                                                                                    PlayerCD = player.PlayerCD,
                                                                                    playerMSTCD = playerInfoMST.PlayerCD,
                                                                                    Name =playerInfoMST.Player,
                                                                                    EarnedRunAverage = player.EarnedRunAverage,
                                                                                    Game = player.Game,
                                                                                    Win = player.Win,
                                                                                    Lost = player.Lost,
                                                                                    Save = player.Save,
                                                                                    Display = player.InningsPitched + " " + (player.InningsPitched3rd.Value == 0 ? "" : player.InningsPitched3rd.Value + "/3")
                                                                                });
            return varTeamInfoPitchingInfos;
        }

        #endregion

        #region Get CatcherFielderInfos
        /// <summary>
        /// 8-5-5
        /// Join 1 table HittingStats  to get :
        /// 1. HittingStats : TeamCD, Num,PlayerCD, Name,BattingAverage, Game,  PlateAppearance,  AtBat,Base,RunsBattingIn
        /// </summary>       
        /// <returns>下記、自チームID でアクセスして取得する。初期表示では、背番号で昇順にソートして出力する。</returns>
        public IEnumerable<NpbTeamInfoPlayerInfos> GetCatcherFielderInfos(int inTeamCD)
        {
            IEnumerable<NpbTeamInfoPlayerInfos> varCatcherFielderInfos = (from playerInfoMST in npb.PlayerInfoMST
                                                                          join playerHeader in npb.PlayerInfoMSTHeader on playerInfoMST.PlayerInfoMSTHeaderId equals playerHeader.PlayerInfoMSTHeaderId
                                                                          join hittingStats in npb.HittingStats on playerInfoMST.PlayerCD equals hittingStats.PlayerCD into playerHitting
                                                                          from player in playerHitting.DefaultIfEmpty()
                                                                          join hittingStatsHearder in npb.HittingStatsHeader on new { k1 = player.HittingStatsHeaderId, k2 = playerHeader.TeamCD } equals new { k1 = hittingStatsHearder.HittingStatsHeaderId, k2 = (int)hittingStatsHearder.TeamCD } 
                                                                          where (playerHeader.TeamCD == inTeamCD)
                                                                          && (playerInfoMST.PitchField == 2 || playerInfoMST.PitchField == 3 || playerInfoMST.PitchField == 4)
                                                                          where (hittingStatsHearder.GameAssortment != Constants.OFFICIALSTATS_GAMEASSORTMENT_EXCHANGE_GAME)
                                                                          select new NpbTeamInfoPlayerInfos
                                                                          {
                                                                              TeamCD = playerHeader.TeamCD,
                                                                              Num = string.IsNullOrEmpty(player.Num) ? playerInfoMST.UniformNO : player.Num,
                                                                              Name = playerInfoMST.Player,
                                                                              PlayerCD = player.PlayerCD,
                                                                              playerMSTCD = playerInfoMST.PlayerCD,
                                                                              BattingAverage = player.BattingAverage,
                                                                              Game = player.Game,
                                                                              PlateAppearance = player.PlateAppearance,
                                                                              AtBat = player.AtBat,
                                                                              Base = player.Base,
                                                                              Hit = player.Hit,
                                                                              RunsBattingIn = player.RunsBattingIn
                                                                          });

            return varCatcherFielderInfos;
        }

        #endregion

        #region Get TeamInfoDirectorStaffInfos
        /// <summary>
        /// Join 2 table Directories,PersonInfoes  to get :
        /// 1. Directories : TeamCD 
        /// 2. PersonInfoes : StaffTitle, BackNumber, PlayerName,Age, Career ,PlayerCD
        /// </summary>       
        /// <returns>下記、自チームID でアクセスして取得する。初期表示では、背番号で昇順にソートして出力する。</returns>
        public IEnumerable<NpbTeamInfoPlayerInfos> GetTeamInfoDirectorStaffInfos(int inTeamCD)
        {
            IEnumerable<NpbTeamInfoPlayerInfos> varTeamInfoDirectorStaffInfos = (from directories in npb.Directory
                                                                                 join personInfoes in npb.PersonInfo on directories.DirectoryId equals personInfoes.DirectoryId
                                                                                 where (directories.TeamID == inTeamCD && personInfoes.StaffKind != 4)
                                                                                 orderby personInfoes.StaffKind, personInfoes.BackNumber
                                                                                 select new NpbTeamInfoPlayerInfos
                                                                                 {
                                                                                     TeamCD = directories.TeamID,
                                                                                     StaffTitle = personInfoes.StaffTitle != string.Empty ? personInfoes.StaffTitle : personInfoes.StaffKind == 1 ? "監督" : personInfoes.StaffKind == 2 ? "2軍監督" : "",
                                                                                     BackNumber = personInfoes.BackNumber,
                                                                                     PlayerCD = personInfoes.ID,
                                                                                     PlayerName = personInfoes.PlayerName,
                                                                                     Age = personInfoes.Age,
                                                                                     Career = personInfoes.Career
                                                                                 });
            return varTeamInfoDirectorStaffInfos;
        }
        #endregion
    }
}
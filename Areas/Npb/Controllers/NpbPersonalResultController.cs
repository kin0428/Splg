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
 * Class		: NpbPersonalResultController
 * Developer	: Nguyen Minh Kiet
 * Updated date : 2015-03-04
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
    public class NpbPersonalResultController : Controller
    {
        #region Global Properties
        //Create context to get value from db.      
        NpbEntities npb = new NpbEntities();
        #endregion
       
        #region "Action Result"
        // GET: Npb/NpbPersonalResult
        public ActionResult Index()
        {
            NpbPersonalResultViewModel npbPersonalResultViewModel = new NpbPersonalResultViewModel();
            ///
            ///Get BattingAverageBest10
            ///
            npbPersonalResultViewModel.SeBattingAverageBest10 = GetBattingAverageBest10((int)NpbConstants.GameAssortment.Se);
            npbPersonalResultViewModel.PaBattingAverageBest10 = GetBattingAverageBest10((int)NpbConstants.GameAssortment.Pa);
            ///
            ///Get HomerunBest10
            ///
            npbPersonalResultViewModel.SeHomerunBest10 = GetHomerunBest10((int)NpbConstants.GameAssortment.Se);
            npbPersonalResultViewModel.PaHomerunBest10 = GetHomerunBest10((int)NpbConstants.GameAssortment.Pa);
            ///
            ///Get RunBattedInBest10
            ///
            npbPersonalResultViewModel.SeRunBattedInBest10 = GetRunBattedInBest10((int)NpbConstants.GameAssortment.Se);
            npbPersonalResultViewModel.PaRunBattedInBest10 = GetRunBattedInBest10((int)NpbConstants.GameAssortment.Pa);
            ///
            ///Get WinBest10
            ///
            npbPersonalResultViewModel.SeWinBest10 = GetWinBest10((int)NpbConstants.GameAssortment.Se);
            npbPersonalResultViewModel.PaWinBest10 = GetWinBest10((int)NpbConstants.GameAssortment.Pa);
            ///
            ///Get EarnedRunAverageBest10
            ///
            npbPersonalResultViewModel.SeEarnedRunAverageBest10 = GetEarnedRunAverageBest10((int)NpbConstants.GameAssortment.Se);
            npbPersonalResultViewModel.PaEarnedRunAverageBest10 = GetEarnedRunAverageBest10((int)NpbConstants.GameAssortment.Pa);
            ///
            ///Get SaveBest10
            ///
            npbPersonalResultViewModel.SeSaveBest10 = GetSaveBest10((int)NpbConstants.GameAssortment.Se);
            npbPersonalResultViewModel.PaSaveBest10 = GetSaveBest10((int)NpbConstants.GameAssortment.Pa);
            return View(npbPersonalResultViewModel);
        }

        #endregion

        #region Get BattingAverageBest10
        /// <summary>
        /// Join 2 table BattingAverageBest10Header, BattingAverageBest10 to get :
        /// 1. BattingAverageBest10 : Name, ShortNameTeam, BattingAverage, Homerun, RunsBattingIn.     
        /// </summary>       
        /// <returns>List top 10 rows </returns>
        public IEnumerable<NpbPersonalResultInfos> GetBattingAverageBest10(int inGameAssortment)
        {
            IEnumerable<NpbPersonalResultInfos> varBattingAverageBest10 = (from battingAverageBest10Header in npb.BattingAverageBest10Header
                                                                           join battingAverageBest10 in npb.BattingAverageBest10
                                                                                    on battingAverageBest10Header.BattingAverageBest10HeaderId equals battingAverageBest10.BattingAverageBest10HeaderId
                                                                           join teamMst in npb.TeamInfoMST on battingAverageBest10.TeamCD equals teamMst.TeamCD into teamInfo
                                                                           from team in teamInfo.DefaultIfEmpty()
                                                                           where battingAverageBest10Header.GameAssortment == inGameAssortment
                                                                           orderby battingAverageBest10.Ranking ascending
                                                                           select new NpbPersonalResultInfos
              {
                  Ranking = battingAverageBest10.Ranking,
                  TeamCD =battingAverageBest10.TeamCD,
                  PlayerCD = battingAverageBest10.PlayerCD,
                  Name = battingAverageBest10.Name,
                  ShortNameTeam = team.TeamCD == null ? battingAverageBest10.ShortNameTeam : team.OnceNameTeam,
                  BattingAverage = battingAverageBest10.BattingAverage,
                  Homerun = battingAverageBest10.Homerun,
                  RunsBattingIn = battingAverageBest10.RunsBattingIn
              }).Distinct();
            return varBattingAverageBest10;
        }

        #endregion

        #region Get HomerunBest10
        /// <summary>
        /// Join 3 table HomerunBest10Header, HomerunBest10 , HittingStats to get :
        /// 1. HomerunBest10 : Name, ShortNameTeam, BattingAverage, Homerun.   
        /// 2. HittingStats: RunsBattingIn
        /// </summary>       
        /// <returns>List top 10 rows </returns>
        public IEnumerable<NpbPersonalResultInfos> GetHomerunBest10(int inGameAssortment)
        {
            IEnumerable<NpbPersonalResultInfos> varHomerunBest10 = (from homerunBest10Header in npb.HomerunBest10Header
                                                                    join homerunBest10 in npb.HomerunBest10
                                                                                  on homerunBest10Header.HomerunBest10HeaderId equals homerunBest10.HomerunBest10HeaderId
                                                                    join hittingStats in npb.HittingStats on homerunBest10.PlayerCD equals hittingStats.PlayerCD
                                                                    join hittingStatsHeader in npb.HittingStatsHeader on hittingStats.HittingStatsHeaderId equals hittingStatsHeader.HittingStatsHeaderId
                                                                    join teamMst in npb.TeamInfoMST on homerunBest10.TeamCD equals teamMst.TeamCD into teamInfo
                                                                    from team in teamInfo.DefaultIfEmpty()
                                                                    where homerunBest10Header.GameAssortment == inGameAssortment
                                                                    && hittingStatsHeader.GameAssortment == inGameAssortment
                                                                    orderby homerunBest10.Ranking ascending
                                                                    select new NpbPersonalResultInfos
                                                                    {
                                                                        Ranking = homerunBest10.Ranking,
                                                                        TeamCD = homerunBest10.TeamCD,
                                                                        PlayerCD = homerunBest10.PlayerCD,
                                                                        Name = homerunBest10.Name,
                                                                        ShortNameTeam = team.TeamCD == null ? homerunBest10.ShortNameTeam : team.OnceNameTeam,
                                                                        BattingAverage = homerunBest10.BattingAverage,
                                                                        Homerun = homerunBest10.HomeRun,
                                                                        RunsBattingIn = hittingStats.RunsBattingIn
                                                                    }).Distinct();
            return varHomerunBest10;
        }

        #endregion

        #region Get RunBattedInBest10
        /// <summary>
        /// Join 3 table RunBattedInBest10Header, RunBattedInBest10, HomerunBest10 to get :
        /// 1. RunBattedInBest10 : Name, ShortNameTeam, BattingAverage, RunsBattingIn.     
        /// 2. HomerunBest10: Homerun
        /// </summary>       
        /// <returns>List top 10 rows </returns>
        public IEnumerable<NpbPersonalResultInfos> GetRunBattedInBest10(int inGameAssortment)
        {
            IEnumerable<NpbPersonalResultInfos> varRunBattedInBest10 = (from runBattedInBest10Header in npb.RunBattedInBest10Header
                                                                        join runBattedInBest10 in npb.RunBattedInBest10
                                                                                  on runBattedInBest10Header.RunBattedInBest10HeaderId equals runBattedInBest10.RunBattedInBest10HeaderId
                                                                        join homerunBest10 in npb.HomerunBest10 on runBattedInBest10.PlayerCD equals homerunBest10.PlayerCD
                                                                        join homeRunHeader in npb.HomerunBest10Header on homerunBest10.HomerunBest10HeaderId equals homeRunHeader.HomerunBest10HeaderId
                                                                        join teamMst in npb.TeamInfoMST on runBattedInBest10.TeamCD equals teamMst.TeamCD into teamInfo
                                                                        from team in teamInfo.DefaultIfEmpty()
                                                                        where runBattedInBest10Header.GameAssortment == inGameAssortment
                                                                        && homeRunHeader.GameAssortment == inGameAssortment
                                                                        orderby runBattedInBest10.Ranking ascending
                                                                        select new NpbPersonalResultInfos
                                                                        {
                                                                            Ranking = runBattedInBest10.Ranking,
                                                                            TeamCD = runBattedInBest10.TeamCD,
                                                                            PlayerCD = runBattedInBest10.PlayerCD,
                                                                            Name = runBattedInBest10.Name,
                                                                            ShortNameTeam = team.TeamCD== null? runBattedInBest10.ShortNameTeam: team.OnceNameTeam,
                                                                            BattingAverage = runBattedInBest10.BattingAverage,
                                                                            Homerun = homerunBest10.HomeRun,
                                                                            RunsBattingIn = runBattedInBest10.RunsBattingIn
                                                                        }).Distinct();
            return varRunBattedInBest10;
        }

        #endregion

        #region Get WinBest10
        /// <summary>
        /// Join 3 table WinBest10Header, WinBest10, PitchingStats to get :
        /// 1. WinBest10 : Name, ShortNameTeam, Win.     
        /// 2. PitchingStats: Lost,Strikeout
        /// </summary>       
        /// <returns>List top 10 rows </returns>
        public IEnumerable<NpbPersonalResultInfos> GetWinBest10(int inGameAssortment)
        {
            IEnumerable<NpbPersonalResultInfos> varWinBest10 = (from winBest10Header in npb.WinBest10Header
                                                                join winBest10 in npb.WinBest10
                                                                                  on winBest10Header.WinBest10HeaderId equals winBest10.WinBest10HeaderId
                                                                join pitchingStats in npb.PitchingStats on winBest10.PlayerCD equals pitchingStats.PlayerCD
                                                                join pitchingStatsHeader in npb.PitchingStatsHeader on pitchingStats.PitchingStatsHeaderId equals pitchingStatsHeader.PitchingStatsHeaderId
                                                                join teamMst in npb.TeamInfoMST on winBest10.TeamCD equals teamMst.TeamCD into teamInfo
                                                                from team in teamInfo.DefaultIfEmpty()
                                                                where winBest10Header.GameAssortment == inGameAssortment
                                                                && pitchingStatsHeader.GameAssortment == inGameAssortment
                                                                orderby winBest10.Ranking ascending
                                                                        select new NpbPersonalResultInfos
                                                                        {
                                                                            Ranking = winBest10.Ranking,
                                                                            TeamCD = winBest10.TeamCD,
                                                                            PlayerCD = winBest10.PlayerCD,
                                                                            Name = winBest10.Name,
                                                                            ShortNameTeam = team.TeamCD== null? winBest10.ShortNameTeam:team.OnceNameTeam,
                                                                            Win = winBest10.Win,
                                                                            Lost = pitchingStats.Lost,
                                                                            Strikeout = pitchingStats.Strikeout
                                                                        }).Distinct();
            return varWinBest10;
        }

        #endregion

        #region Get EarnedRunAverageBest10
        /// <summary>
        /// Join 2 table EarnedRunAverageBest10Header, EarnedRunAverageBest10 to get :
        /// 1. EarnedRunAverageBest10 : Name, ShortNameTeam, EarnedRunAverage, InningsPitched, InningsPitched3rd .       
        /// </summary>       
        /// <returns>List top 10 rows </returns>
        public IEnumerable<NpbPersonalResultInfos> GetEarnedRunAverageBest10(int inGameAssortment)
        {
            IEnumerable<NpbPersonalResultInfos> varEarnedRunAverageBest10 = (from earnedRunAverageBest10Header in npb.EarnedRunAverageBest10Header
                                                                             join earnedRunAverageBest10 in npb.EarnedRunAverageBest10
                                                                                  on earnedRunAverageBest10Header.EarnedRunAverageBest10HeaderId equals earnedRunAverageBest10.EarnedRunAverageBest10HeaderId
                                                                             join teamMst in npb.TeamInfoMST on earnedRunAverageBest10.TeamCD equals teamMst.TeamCD into teamInfo
                                                                             from team in teamInfo.DefaultIfEmpty()
                                                                             where earnedRunAverageBest10Header.GameAssortment == inGameAssortment
                                                                             orderby earnedRunAverageBest10.Ranking ascending
                                                                select new NpbPersonalResultInfos
                                                                {
                                                                    Ranking = earnedRunAverageBest10.Ranking,
                                                                    TeamCD = earnedRunAverageBest10.TeamCD,
                                                                    PlayerCD = earnedRunAverageBest10.PlayerCD,
                                                                    Name = earnedRunAverageBest10.Name,
                                                                    ShortNameTeam = team.TeamCD == null? earnedRunAverageBest10.ShortNameTeam: team.OnceNameTeam,
                                                                    EarnedRunAverage = earnedRunAverageBest10.EarnedRunAverage,
                                                                    InningsPitched = earnedRunAverageBest10.InningsPitched,
                                                                    InningsPitched3rd = earnedRunAverageBest10.InningsPitched3rd,
                                                                    Display = earnedRunAverageBest10.InningsPitched + " " + (earnedRunAverageBest10.InningsPitched3rd.Value == 0 ? "" : earnedRunAverageBest10.InningsPitched3rd.Value + "/3")
                                                                }).Distinct();
            return varEarnedRunAverageBest10;
        }

        #endregion

        #region Get SaveBest10
        /// <summary>
        /// Join 2 table SaveBest10Header, SaveBest10 to get :
        /// 1. SaveBest10 : Name, ShortNameTeam, Save, GamePitched .       
        /// </summary>       
        /// <returns>List top 10 rows </returns>
        public IEnumerable<NpbPersonalResultInfos> GetSaveBest10(int inGameAssortment)
        {
            IEnumerable<NpbPersonalResultInfos> varSaveBest10 = (from saveBest10Header in npb.SaveBest10Header
                                                                 join saveBest10 in npb.SaveBest10
                                                                                  on saveBest10Header.SaveBest10HeaderId equals saveBest10.SaveBest10HeaderId
                                                                 join teamMst in npb.TeamInfoMST on saveBest10.TeamCD equals teamMst.TeamCD into teamInfo
                                                                 from team in teamInfo.DefaultIfEmpty()
                                                                 where saveBest10Header.GameAssortment == inGameAssortment
                                                                 orderby saveBest10.Ranking ascending
                                                                             select new NpbPersonalResultInfos
                                                                             {
                                                                                 Ranking = saveBest10.Ranking,
                                                                                 TeamCD = saveBest10.TeamCD,
                                                                                 Name = saveBest10.Name,
                                                                                 PlayerCD = saveBest10.PlayerCD,
                                                                                 ShortNameTeam = team.TeamCD==null? saveBest10.ShortNameTeam:team.OnceNameTeam,
                                                                                 Save = saveBest10.Save,
                                                                                 GamePitched = saveBest10.GamePitched
                                                                             }).Distinct();
            return varSaveBest10;
        }

        #endregion
    }
}
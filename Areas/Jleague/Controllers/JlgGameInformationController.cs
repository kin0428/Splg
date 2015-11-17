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
 * Namespace	: Splg.Areas.Jlg.Controllers
 * Class		: JlgGameInformationController
 * Developer	: Nojima
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Splg.Areas.Jleague.Models;
using Splg.Areas.Jleague.Models.ViewModel;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.Jleague.Models.Dto;
using Splg.Areas.Npb.Models;
using Splg.Models;
using Splg.Models.Game.InfoModel;
using Splg.Models.Game.ViewModel;
using Splg.Services.Game;
using Splg.Core.Providers;

namespace Splg.Areas.Jleague.Controllers
{
    public class JlgGameInformationController : Controller
    {
        #region Global Properties
        /// <summary>
        /// Declare context Jlg to get db.
        /// </summary>
        JlgEntities jlg = new JlgEntities();
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        ComEntities com = new ComEntities();

        /// <summary>
        /// JLGサービス
        /// </summary>
        JlgService jlgService = new JlgService();

        #endregion

        #region Index
        /// <summary>
        /// Method to call when loading this page.
        /// </summary>
        /// <param name="gameID">GameID</param>
        /// <returns>Model in this view.</returns>
        public ActionResult Index(int? gameID)
        {
            JlgGameInfoViewModel viewModel = null;

            if (gameID != null)
            {
                viewModel = getViewModel(gameID);

                return View(viewModel);
            }

            return View();
        }

        /// <summary>
        /// ビューモデルのセット
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        private JlgGameInfoViewModel getViewModel(int? gameID)
        {   

            JlgGameInfoViewModel viewModel = new JlgGameInfoViewModel();

            viewModel.GameID = (int)gameID;
            viewModel.GameStatus = setGameStatus(gameID);
            long memberId = 0;
            object currentUser = Session["CurrentUser"];
            if (currentUser != null)
                memberId = Convert.ToInt64(currentUser.ToString());

            //一試合スケジュール_試合情報
            viewModel.ScheduleInfo = (from si in jlg.ScheduleInfo where si.GameID == gameID select si).FirstOrDefault();

            //一試合別通算対戦成績_試合情報
            viewModel.ScheduleInfoHE = (from sih in jlg.ScheduleInfoHE where sih.GameID == gameID select sih).FirstOrDefault();

            //一試合速報_ヘッダー情報
            viewModel.GameReportLG = (from gr in jlg.GameReportLG
                                      where gr.GameID == gameID
                                      select gr).FirstOrDefault();


            #region 試合前・試合後共通情報

            //////////////////////////////////////////////////////////////////////
            //試合情報 Htmlアクション

            //////////////////////////////////////////////////////////////////////
            //フォローユーザーの予想
            if (memberId > 0)
            {
                //ホームの勝ち
                viewModel.FollowMembersBetToWin = Utils.GetExpectingMembers(com, (int)gameID, memberId, 4, Constants.JLG_SPORT_ID, 1);
                viewModel.FollowMembersBetToDraw = Utils.GetExpectingMembers(com, (int)gameID, memberId, 4, Constants.JLG_SPORT_ID, 3);
                viewModel.FollowMembersBetToLose = Utils.GetExpectingMembers(com, (int)gameID, memberId, 4, Constants.JLG_SPORT_ID, 2);
            }

            //////////////////////////////////////////////////////////////////////
            //試合前 この試合の投稿記事
            //記事[新着順最大3件]
            //
            //TODO 試合で絞る共通メソッド待ち、5-2投稿記事 検索結果へのとびかた待ち
            //
            viewModel.PostedInfos = Splg.Controllers.PostedController.GetRecentPosts(2, Constants.JLG_SPORT_ID, null, 1);

            //////////////////////////////////////////////////////////////////////
            //カテゴリーニュース		Htmlアクション５件	(ピックアップニュース)
            //ホームチーム			　　Htmlアクション５件
            //ビジターチーム	        Htmlアクション５件

            /////////////////////////////////////////////////////////////////////////
            //次の節の試合情報　予想してみよう    
            //試合スケジュール_試合情報	

            //Htmlアクション　ShowGameInfo type=5  teamID occasionNo = viewMode.OccasionNo + 1

            // フォーメーション情報取得 
            viewModel.FormationChartDto = jlgService.GetFormationInfoWithAttackForMobile(viewModel.GameID);

            #endregion


            #region 試合前のみ

            if (viewModel.GameStatus < 1)
            {
                //////////////////////////////////////////////////////////////////////
                //対相手チームの成績 （05/07菅野さん指示で試合後のみに）
                //過去5試合_試合情報（ホーム）
                viewModel.List5GamesHomeInHistory = GetLast5HomeGames(viewModel.HomeTeamID);

                //対相手チームの成績
                //過去5試合_試合情報（アウェイ）
                viewModel.List5GamesAwayInHistory = GetLast5AwayGames(viewModel.AwayTeamID);

                //////////////////////////////////////////////////////////////////////
                //対相手チームの成績
                //試合別通算対戦成績_試合情報
                viewModel.List5GamesInHistory = GetPast5Games(viewModel);

                //////////////////////////////////////////////////////////////////////
                //直近5試合のチーム勝敗 HTMLには無い（05/07菅野さん指示で試合後のみに）
                
                // ホームチームとアウェーチームのスペックを取得
                viewModel.homeTeamSpec = jlgService.GetTeamSpecByTeamId(viewModel.HomeTeamID);
                viewModel.awayTeamSpec = jlgService.GetTeamSpecByTeamId(viewModel.AwayTeamID);

            }

            #endregion

            #region 試合中 試合後

            if (viewModel.GameStatus > 0)
            {
                // スターティングメンバー情報取得
                viewModel = GetStartingInfo(viewModel,(int)gameID);

                ////警告、退場情報
                //List<JlgPlayerGameInfoModel> homeStartMamberWarningInfos, homeSubMamberWarningInfos, awayStartMamberWarningInfos, awaySubMamberWarningInfos;

                //GetPlayerWarningInfos((int)gameID, 1, out homeStartMamberWarningInfos, out homeSubMamberWarningInfos);
                //GetPlayerWarningInfos((int)gameID, 2, out awayStartMamberWarningInfos, out awaySubMamberWarningInfos);
                //viewModel.HomeStartingMamberWarningInfos = homeStartMamberWarningInfos;
                //viewModel.HomeSubMamberWarningInfos = homeSubMamberWarningInfos;
                //viewModel.AwayStartingWarningInfos = awayStartMamberWarningInfos;
                //viewModel.AwaySubMamberWarningInfos = awaySubMamberWarningInfos;

                //////////////////////////////////////////////////////////////////////
                //テキスト速報 コメント配信_コメント情報
                var teamDic = new Dictionary<int, string>();
                teamDic.Add(viewModel.HomeTeamID, viewModel.HomeTeamNameS);
                teamDic.Add(viewModel.AwayTeamID, viewModel.AwayTeamNameS);
                viewModel.ListGameTexts = GetComment(gameID, teamDic);

                //戦評
                viewModel.Recaps = GetRecap(gameID);

                //////////////////////////////////////////////////////////////////////
                //PK情報の取得
                //if (viewModel.HasPK)
                //{
                viewModel.HomePKInfos = GetPKInfo((int)gameID, 1);
                viewModel.AwayPKInfos = GetPKInfo((int)gameID, 2);
                //}

                //////////////////////////////////////////////////////////////////////
                //ホーム・アウェイ別得点状況 HTMLなし

                //////////////////////////////////////////////////////////////////////
                //試合経過 HTMLなし

            }

            #endregion



            #region  試合後

            if (viewModel.GameStatus > 1)
            {
                //////////////////////////////////////////////////////////////////////
                //試合データ 一試合速報_試合終了情報
                viewModel.GameEndInfo = GetGameEndInfo(gameID);

                //////////////////////////////////////////////////////////////////////
                //試合結果　一試合速報_チーム情報
                viewModel.HomeGameScore = GetGameEndScoreInfo(gameID, 1);
                viewModel.AwayGameScore = GetGameEndScoreInfo(gameID, 2);

                //////////////////////////////////////////////////////////////////////
                //試合結果　スタッツ
                viewModel.HomeGameStats = GetGameStats((int)gameID, (int)viewModel.HomeTeamID);
                viewModel.AwayGameStats = GetGameStats((int)gameID, (int)viewModel.AwayTeamID);

            }

            #endregion

            return viewModel;
        }


        #region Get選手情報（得点、警告・退場、途中出場、途中交代)
        /// <summary>
        /// 一試合選手速報
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="hv"></param>
        /// <param name="startingMembers"></param>
        /// <param name="subMembers"></param>
        /// <returns></returns>
        private void GetPlayerGameDetailInfos(int gameID, int hv, int infoType,
            out List<JlgPlayerGameDetailInfoModel> startingMembers, out List<JlgPlayerGameDetailInfoModel> subMembers)
        {
            // Homeの場合
            if (hv == 1)
            {
                //スターティングメンバー情報
                switch (infoType)
                {
                    // 警告・退場
                    case 1:
                        var startingMembersQuery = (
                                           from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                           join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                           join pi in jlg.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                           join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                           join wilg in jlg.WarningInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.HomeTeamID, k3 = pi.ID } equals new { k1 = wilg.GameReportLGId, k2 = wilg.TeamID, k3 = (int)wilg.PlayerID }  // 一試合選手速報_選手情報（先発情報）
                                           where pr.GameID == gameID
                                           select new JlgPlayerGameDetailInfoModel
                                           {
                                               Itype = "card",
                                               TeamId = (int)pr.HomeTeamID,
                                               StateName = wilg.StateName,
                                               Half = (int)wilg.Half,
                                               Time = (int)wilg.Time,
                                               PlayerId = (int)wilg.PlayerID,
                                               PlayerName = wilg.PlayerName,
                                               PlayerNameS = wilg.PlayerNameS,
                                               Divide = (short)wilg.Divide,
                                               SecondF = (short)wilg.SecondF
                                           });
                        startingMembers = startingMembersQuery.ToList();
                        break;
                    // ゴール
                    case 2:
                        startingMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join gil in jlg.GoalInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.HomeTeamID, k3 = pi.ID } equals new { k1 = gil.GameReportLGId, k2 = gil.ScoreTeamID, k3 = (int)gil.GPlayerID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "goal",
                                                TeamId = (int)pr.HomeTeamID,
                                                StateName = gil.StateName,
                                                Half = (int)gil.Half,
                                                Time = (int)gil.Time,
                                                PlayerId = (int)gil.GPlayerID,
                                                PlayerName = gil.GPlayerName,
                                                PlayerNameS = gil.GPlayerNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        startingMembers = startingMembersQuery.ToList();
                        break;
                    // IN
                    case 3:
                        startingMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join cil in jlg.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                                            join pil in jlg.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.InID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            where cil.HV == hv
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "in",
                                                TeamId = (int)pr.HomeTeamID,
                                                StateName = pil.StateName,
                                                Half = (int)pil.Half,
                                                Time = (int)pil.Time,
                                                PlayerId = (int)pil.InID,
                                                PlayerName = pil.InName,
                                                PlayerNameS = pil.InNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        startingMembers = startingMembersQuery.ToList();
                        break;
                    // OUT
                    default:
                        startingMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join cil in jlg.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                                            join pil in jlg.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.OutID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            where cil.HV == hv
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "out",
                                                TeamId = (int)pr.HomeTeamID,
                                                StateName = pil.StateName,
                                                Half = (int)pil.Half,
                                                Time = (int)pil.Time,
                                                PlayerId = (int)pil.OutID,
                                                PlayerName = pil.OutName,
                                                PlayerNameS = pil.OutNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        startingMembers = startingMembersQuery.ToList();
                        break;
                }

                startingMembers.Sort();

                //ベンチ入り選手情報 サブメンバー情報
                switch (infoType)
                {
                    // 警告・退場
                    case 1:
                        var subMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join wilg in jlg.WarningInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.HomeTeamID, k3 = pi.ID } equals new { k1 = wilg.GameReportLGId, k2 = wilg.TeamID, k3 = (int)wilg.PlayerID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "card",
                                                TeamId = (int)pr.HomeTeamID,
                                                StateName = wilg.StateName,
                                                Half = (int)wilg.Half,
                                                Time = (int)wilg.Time,
                                                PlayerId = (int)wilg.PlayerID,
                                                PlayerName = wilg.PlayerName,
                                                PlayerNameS = wilg.PlayerNameS,
                                                Divide = (short)wilg.Divide,
                                                SecondF = (short)wilg.SecondF
                                            });
                        subMembers = subMembersQuery.ToList();
                        break;
                    // ゴール
                    case 2:
                        subMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join gil in jlg.GoalInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.HomeTeamID, k3 = pi.ID } equals new { k1 = gil.GameReportLGId, k2 = gil.ScoreTeamID, k3 = (int)gil.GPlayerID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "goal",
                                                TeamId = (int)pr.HomeTeamID,
                                                StateName = gil.StateName,
                                                Half = (int)gil.Half,
                                                Time = (int)gil.Time,
                                                PlayerId = (int)gil.GPlayerID,
                                                PlayerName = gil.GPlayerName,
                                                PlayerNameS = gil.GPlayerNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        subMembers = subMembersQuery.ToList();
                        break;
                    // IN
                    case 3:
                        subMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join cil in jlg.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                                            join pil in jlg.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.InID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            where cil.HV == hv
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "in",
                                                TeamId = (int)pr.HomeTeamID,
                                                StateName = pil.StateName,
                                                Half = (int)pil.Half,
                                                Time = (int)pil.Time,
                                                PlayerId = (int)pil.InID,
                                                PlayerName = pil.InName,
                                                PlayerNameS = pil.InNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        subMembers = subMembersQuery.ToList();
                        break;
                    // OUT
                    default:
                        subMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join cil in jlg.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                                            join pil in jlg.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.OutID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            where cil.HV == hv
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "out",
                                                TeamId = (int)pr.HomeTeamID,
                                                StateName = pil.StateName,
                                                Half = (int)pil.Half,
                                                Time = (int)pil.Time,
                                                PlayerId = (int)pil.OutID,
                                                PlayerName = pil.OutName,
                                                PlayerNameS = pil.OutNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        subMembers = subMembersQuery.ToList();
                        break;
                }

                subMembers.Sort();

            }
            else
            {
                //スターティングメンバー情報
                switch (infoType)
                {
                    // 警告・退場
                    case 1:
                        var startingMembersQuery = (
                                           from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                           join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                           join pi in jlg.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                           join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                           join wilg in jlg.WarningInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.AwayTeamID, k3 = pi.ID } equals new { k1 = wilg.GameReportLGId, k2 = wilg.TeamID, k3 = (int)wilg.PlayerID }  // 一試合選手速報_選手情報（先発情報）
                                           where pr.GameID == gameID
                                           select new JlgPlayerGameDetailInfoModel
                                           {
                                               Itype = "card",
                                               TeamId = (int)pr.AwayTeamID,
                                               StateName = wilg.StateName,
                                               Half = (int)wilg.Half,
                                               Time = (int)wilg.Time,
                                               PlayerId = (int)wilg.PlayerID,
                                               PlayerNameS = wilg.PlayerNameS,
                                               Divide = (short)wilg.Divide,
                                               SecondF = (short)wilg.SecondF
                                           });
                        startingMembers = startingMembersQuery.ToList();
                        break;
                    // ゴール
                    case 2:
                        startingMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join gil in jlg.GoalInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.AwayTeamID, k3 = pi.ID } equals new { k1 = gil.GameReportLGId, k2 = gil.ScoreTeamID, k3 = (int)gil.GPlayerID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "goal",
                                                TeamId = (int)pr.AwayTeamID,
                                                StateName = gil.StateName,
                                                Half = (int)gil.Half,
                                                Time = (int)gil.Time,
                                                PlayerId = (int)gil.GPlayerID,
                                                PlayerName = gil.GPlayerName,
                                                PlayerNameS = gil.GPlayerNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        startingMembers = startingMembersQuery.ToList();
                        break;
                    // IN
                    case 3:
                        startingMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join cil in jlg.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                                            join pil in jlg.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.InID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            where cil.HV == hv
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "in",
                                                TeamId = (int)pr.AwayTeamID,
                                                StateName = pil.StateName,
                                                Half = (int)pil.Half,
                                                Time = (int)pil.Time,
                                                PlayerId = (int)pil.InID,
                                                PlayerName = pil.InName,
                                                PlayerNameS = pil.InNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        startingMembers = startingMembersQuery.ToList();
                        break;
                    // OUT
                    default:
                        startingMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join cil in jlg.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                                            join pil in jlg.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.OutID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            where cil.HV == hv
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "out",
                                                TeamId = (int)pr.AwayTeamID,
                                                StateName = pil.StateName,
                                                Half = (int)pil.Half,
                                                Time = (int)pil.Time,
                                                PlayerId = (int)pil.OutID,
                                                PlayerName = pil.OutName,
                                                PlayerNameS = pil.OutNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        startingMembers = startingMembersQuery.ToList();
                        break;
                }

                startingMembers.Sort();

                //ベンチ入り選手情報 サブメンバー情報
                switch (infoType)
                {
                    // 警告・退場
                    case 1:
                        var subMembersQuery = (
                                       from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                       join st in jlg.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                       join pi in jlg.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                       join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                       join wilg in jlg.WarningInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.AwayTeamID, k3 = pi.ID } equals new { k1 = wilg.GameReportLGId, k2 = wilg.TeamID, k3 = (int)wilg.PlayerID }  // 一試合選手速報_選手情報（先発情報）
                                       where pr.GameID == gameID
                                       select new JlgPlayerGameDetailInfoModel
                                       {
                                           Itype = "card",
                                           TeamId = (int)pr.AwayTeamID,
                                           StateName = wilg.StateName,
                                           Half = (int)wilg.Half,
                                           Time = (int)wilg.Time,
                                           PlayerId = (int)wilg.PlayerID,
                                           PlayerName = wilg.PlayerName,
                                           PlayerNameS = wilg.PlayerNameS,
                                           Divide = (short)wilg.Divide,
                                           SecondF = (short)wilg.SecondF
                                       });
                        subMembers = subMembersQuery.ToList();
                        break;
                    // ゴール
                    case 2:
                        subMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join gil in jlg.GoalInfoLG on new { k1 = grl.GameReportLGId, k2 = pr.AwayTeamID, k3 = pi.ID } equals new { k1 = gil.GameReportLGId, k2 = gil.ScoreTeamID, k3 = (int)gil.GPlayerID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "goal",
                                                TeamId = (int)pr.AwayTeamID,
                                                StateName = gil.StateName,
                                                Half = (int)gil.Half,
                                                Time = (int)gil.Time,
                                                PlayerId = (int)gil.GPlayerID,
                                                PlayerName = gil.GPlayerName,
                                                PlayerNameS = gil.GPlayerNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        subMembers = subMembersQuery.ToList();
                        break;
                    // IN
                    case 3:
                        subMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join cil in jlg.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                                            join pil in jlg.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.InID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            where cil.HV == hv
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "in",
                                                TeamId = (int)pr.AwayTeamID,
                                                StateName = pil.StateName,
                                                Half = (int)pil.Half,
                                                Time = (int)pil.Time,
                                                PlayerId = (int)pil.InID,
                                                PlayerName = pil.InName,
                                                PlayerNameS = pil.InNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        subMembers = subMembersQuery.ToList();
                        break;
                    // OUT
                    default:
                        subMembersQuery = (
                                            from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                            join st in jlg.BenchInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                            join pi in jlg.PlayerInfoLPBE on st.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                            join grl in jlg.GameReportLG on pr.GameID equals grl.GameID  // 一試合選手速報_選手情報（先発情報）
                                            join cil in jlg.ChangeInfoLP on pr.PlayerReportLPId equals cil.PlayerReportLPId
                                            join pil in jlg.PlayerInfoLPCH on new { k1 = cil.ChangeInfoLPId, k2 = pi.ID } equals new { k1 = pil.ChangeInfoLPId, k2 = (int)pil.OutID }  // 一試合選手速報_選手情報（先発情報）
                                            where pr.GameID == gameID
                                            where cil.HV == hv
                                            select new JlgPlayerGameDetailInfoModel
                                            {
                                                Itype = "out",
                                                TeamId = (int)pr.AwayTeamID,
                                                StateName = pil.StateName,
                                                Half = (int)pil.Half,
                                                Time = (int)pil.Time,
                                                PlayerId = (int)pil.OutID,
                                                PlayerName = pil.OutName,
                                                PlayerNameS = pil.OutNameS,
                                                Divide = 0,
                                                SecondF = 0
                                            });
                        subMembers = subMembersQuery.ToList();
                        break;
                }

                subMembers.Sort();


            }
        }
        #endregion

        /// <summary>
        /// 一試合選手速報
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="hv"></param>
        /// <param name="startingMembers"></param>
        /// <param name="subMembers"></param>
        /// <returns></returns>
        private void GetPlayerGameInfos(int gameID, int hv,
            out List<JlgPlayerGameInfoModel> startingMembers, out List<JlgPlayerGameInfoModel> subMembers)
        {
            //スターティングメンバー情報
            var startingMembersQuery = (from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                        join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報
                                        join sti in jlg.StartingInfoLP on st.StartingInfoLPId equals sti.StartingInfoLPId //一試合選手速報_先発情報
                                        join pi in jlg.PlayerInfoLPST on sti.StartingInfoLPId equals pi.StartingInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                        where pr.GameID == gameID
                                        && st.HV == hv
                                        select new JlgPlayerGameInfoModel
                                        {
                                            HV = hv,
                                            ID = pi.ID,
                                            NameS = pi.NameS,
                                            Name = pi.Name,
                                            Goal = pi.Goal,
                                            Card = pi.Card,
                                            IsStartingMember = true,
                                            Pos = pi.Pos,
                                            Uni = pi.Uni,
                                            Yellow = pi.Yellow

                                        });


            startingMembers = startingMembersQuery.ToList();
            startingMembers.Sort();

            //ベンチ入り選手情報 サブメンバー情報
            var subMembersQuery = (from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                   join be in jlg.BenchInfoLP on pr.PlayerReportLPId equals be.PlayerReportLPId  //一試合選手速報_控え情報
                                   join sti in jlg.BenchInfoLP on be.BenchInfoLPId equals sti.BenchInfoLPId //一試合選手速報_先発情報
                                   join pi in jlg.PlayerInfoLPBE on sti.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                   where pr.GameID == gameID
                                   && be.HV == hv
                                   select new JlgPlayerGameInfoModel
                                   {
                                       HV = hv,
                                       ID = pi.ID,
                                       NameS = pi.NameS,
                                       Name = pi.Name,
                                       Goal = pi.Goal,
                                       Card = pi.Card,
                                       IsStartingMember = false,
                                       Pos = pi.Pos,
                                       Uni = pi.Uni,
                                       Yellow = pi.Yellow
                                   });

            subMembers = subMembersQuery.ToList();
            subMembers.Sort();

        }
        /// <summary>
        /// 交代
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="hv"></param>
        /// <param name="startingChangeInfos"></param>
        /// <param name="subMembersChangeInfos"></param>
        private void GetPlayerInfos(int gameID, int hv,
            out List<JlgPlayerGameInfoModel> startingChangeInfos, out List<JlgPlayerGameInfoModel> subMembersChangeInfos)
        {
            startingChangeInfos = new List<JlgPlayerGameInfoModel>();

            //スターティングメンバー交代情報
            var a = (from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                     join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報 
                     join pi in jlg.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId //一試合選手速報_先発情報
                     where pr.GameID == gameID
                     && st.HV == hv
                     select pi);

            var b = (from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                     join ch in jlg.ChangeInfoLP on pr.PlayerReportLPId equals ch.PlayerReportLPId        //一試合選手速報_交代情報
                     join chh in jlg.PlayerInfoLPCH on ch.ChangeInfoLPId equals chh.ChangeInfoLPId       //一試合選手速報_選手情報
                     where pr.GameID == gameID
                     && ch.HV == hv
                     select chh);


            //サブメンバー交代情報
            subMembersChangeInfos = (from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                     join be in jlg.BenchInfoLP on pr.PlayerReportLPId equals be.PlayerReportLPId  //一試合選手速報_控え情報
                                     join sti in jlg.BenchInfoLP on be.BenchInfoLPId equals sti.BenchInfoLPId //一試合選手速報_先発情報
                                     join pi in jlg.PlayerInfoLPBE on sti.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                     join ch in jlg.ChangeInfoLP on pr.PlayerReportLPId equals ch.PlayerReportLPId        //一試合選手速報_交代情報
                                     join chh in jlg.PlayerInfoLPCH on ch.ChangeInfoLPId equals chh.ChangeInfoLPId       //一試合選手速報_選手情報
                                     where pr.GameID == gameID
                                     && be.HV == hv
                                     orderby pi.ID, chh.Time
                                     select new JlgPlayerGameInfoModel
                                     {
                                         NameS = pi.NameS,
                                         Name = pi.Name,
                                         Goal = pi.Goal,
                                         Card = pi.Card,
                                         IsStartingMember = true,
                                         Yellow = pi.Yellow,
                                         Change_ExitF = chh.ExitF,
                                         Change_InID = chh.InID,
                                         Change_InNameS = chh.InNameS,
                                         Change_InName = chh.InName,
                                         Change_Half = chh.Half,
                                         Change_InPos = chh.InPos,
                                         Change_InUni = chh.InUni,
                                         Change_OutID = chh.OutID,
                                         Change_OutName = chh.OutName,
                                         Change_OutNameS = chh.OutNameS,
                                         Change_OutPos = chh.OutPos,
                                         Change_OutUni = chh.OutUni,
                                         Change_StateID = chh.StateID,
                                         Change_StateName = chh.StateName,
                                         Change_Time = chh.Time

                                     }).ToList();



        }

        /// <summary>
        /// 試合スタッツ
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        private JlgGameStats GetGameStats(int gameId, int teamID)
        {
            var result = (from tgs in jlg.TeamGameStats                                             //チーム試合スタッツ
                          join tet in jlg.TeamInfoTE on tgs.TeamID equals tet.TeamID
                          join tij in jlg.TeamIconJlg on tgs.TeamID equals tij.TeamCD into tmp_icon    //チームアイコン
                          from icon in tmp_icon.DefaultIfEmpty()
                          where tgs.GameID == gameId && tgs.TeamID == teamID
                          select new JlgGameStats
                          {
                             GameID	 = tgs.GameID,
                            GameDate	 = tgs.GameDate,
                            GameKindID	 = tgs.GameKindID,
                            OccasionNo	 = tgs.OccasionNo,
                            HomeAwayClass	 = tgs.HomeAwayClass,
                            TeamID	 = tgs.TeamID,
                            TeamName	 = tgs.TeamName,
                            TeamShortName = tet.TeamNameS,
                            OpponentTeamID	 = tgs.OpponentTeamID,
                            OpponentTeamName	 = tgs.OpponentTeamName,
                            GameResultID	 = tgs.GameResultID,
                            PlayCount	 = tgs.PlayCount,
                            GoalCount	 = tgs.GoalCount,
                            OGCount	 = tgs.OGCount,
                            ShootCount	 = tgs.ShootCount,
                            WithinShootCount	 = tgs.WithinShootCount,
                            PossessionRatio	 = tgs.PossessionRatio,
                            PassCount	 = tgs.PassCount,
                            CrossCount	 = tgs.CrossCount,
                            LeftSideCrossCount	 = tgs.LeftSideCrossCount,
                            RightSideCrossCount	 = tgs.RightSideCrossCount,
                            DribbleCount	 = tgs.DribbleCount,
                            TackleCount	 = tgs.TackleCount,
                            ClearCount	 = tgs.ClearCount,
                            InterceptCount	 = tgs.InterceptCount,
                            FoulCount	 = tgs.FoulCount,
                            a30mLineCount	 = tgs.C30mLineCount,
                            TeamIcon = icon.TeamIcon
                         }
                        ).FirstOrDefault();


            return result;

        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="hv"></param>
        /// <param name="startingChangeInfos"></param>
        /// <param name="subMembersChangeInfos"></param>
        private void GetPlayerWarningInfos(int gameID, int hv,
            out List<JlgPlayerGameInfoModel> startingChangeInfos, out List<JlgPlayerGameInfoModel> subMembersChangeInfos)
        {

            //スターティングメンバー交代情報
            startingChangeInfos = (from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                   join st in jlg.StartingInfoLP on pr.PlayerReportLPId equals st.PlayerReportLPId  //一試合選手速報_先発情報 
                                   join pi in jlg.PlayerInfoLPST on st.StartingInfoLPId equals pi.StartingInfoLPId //一試合選手速報_先発情報
                                   join ch in jlg.ChangeInfoLP on pr.PlayerReportLPId equals ch.PlayerReportLPId        //一試合選手速報_交代情報
                                   join chh in jlg.PlayerInfoLPCH on ch.ChangeInfoLPId equals chh.ChangeInfoLPId       //一試合選手速報_選手情報
                                   where pr.GameID == gameID
                                   && st.HV == hv
                                   orderby pi.ID, chh.Time
                                   select new JlgPlayerGameInfoModel
                                   {
                                       NameS = pi.NameS,
                                       Name = pi.Name,
                                       Goal = pi.Goal,
                                       Card = pi.Card,
                                       IsStartingMember = true,
                                       Yellow = pi.Yellow,
                                       Change_ExitF = chh.ExitF,
                                       Change_InID = chh.InID,
                                       Change_InNameS = chh.InNameS,
                                       Change_InName = chh.InName,
                                       Change_Half = chh.Half,
                                       Change_InPos = chh.InPos,
                                       Change_InUni = chh.InUni,
                                       Change_OutID = chh.OutID,
                                       Change_OutName = chh.OutName,
                                       Change_OutNameS = chh.OutNameS,
                                       Change_OutPos = chh.OutPos,
                                       Change_OutUni = chh.OutUni,
                                       Change_StateID = chh.StateID,
                                       Change_StateName = chh.StateName,
                                       Change_Time = chh.Time

                                   }).ToList();


            //サブメンバー交代情報
            subMembersChangeInfos = (from pr in jlg.PlayerReportLP                                      //一試合選手速報_ヘッダー情報
                                     join be in jlg.BenchInfoLP on pr.PlayerReportLPId equals be.PlayerReportLPId  //一試合選手速報_控え情報
                                     join sti in jlg.BenchInfoLP on be.BenchInfoLPId equals sti.BenchInfoLPId //一試合選手速報_先発情報
                                     join pi in jlg.PlayerInfoLPBE on sti.BenchInfoLPId equals pi.BenchInfoLPId  // 一試合選手速報_選手情報（先発情報）
                                     join ch in jlg.ChangeInfoLP on pr.PlayerReportLPId equals ch.PlayerReportLPId        //一試合選手速報_交代情報
                                     join chh in jlg.PlayerInfoLPCH on ch.ChangeInfoLPId equals chh.ChangeInfoLPId       //一試合選手速報_選手情報
                                     where pr.GameID == gameID
                                     && be.HV == hv
                                     orderby pi.ID, chh.Time
                                     select new JlgPlayerGameInfoModel
                                     {
                                         NameS = pi.NameS,
                                         Name = pi.Name,
                                         Goal = pi.Goal,
                                         Card = pi.Card,
                                         IsStartingMember = true,
                                         Yellow = pi.Yellow,
                                         Change_ExitF = chh.ExitF,
                                         Change_InID = chh.InID,
                                         Change_InNameS = chh.InNameS,
                                         Change_InName = chh.InName,
                                         Change_Half = chh.Half,
                                         Change_InPos = chh.InPos,
                                         Change_InUni = chh.InUni,
                                         Change_OutID = chh.OutID,
                                         Change_OutName = chh.OutName,
                                         Change_OutNameS = chh.OutNameS,
                                         Change_OutPos = chh.OutPos,
                                         Change_OutUni = chh.OutUni,
                                         Change_StateID = chh.StateID,
                                         Change_StateName = chh.StateName,
                                         Change_Time = chh.Time

                                     }).ToList();
        }

        /// <summary>
        /// PK情報の取得
        /// 一試合速報_キッカー情報
        /// </summary>
        private List<JlgPKInfoModel> GetPKInfo(int gameID, int hv)
        {
            List<JlgPKInfoModel> pks = (from gr in jlg.GameReportLG      //一試合速報_ヘッダー情報
                                        join pk in jlg.PKInfoLG          //一試合速報_PK戦情報
                                          on gr.GameReportLGId equals pk.GameReportLGId
                                        join pr in jlg.PlayerLG          //一試合速報_キッカー情報							
                                        on pk.PKInfoLGId equals pr.PKInfoLGId
                                        where gr.GameID == gameID
                                        && pk.HV == hv
                                        orderby pr.No
                                        select new JlgPKInfoModel
                                        {
                                            HV = hv,
                                            No = pr.No,
                                            PlayerID = pr.PlayerID,
                                            PlayerName = pr.PlayerName,
                                            PlayerNameS = pr.PlayerNameS,
                                            PlayerUni = pr.PlayerUni,
                                            Sign = pr.Sign,
                                            SuccessF = pr.SuccessF
                                        }).ToList();
            return pks;

        }


        /// <summary>
        /// 過去の直接対決の勝敗 試合別通算対戦成績_試合情報
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private IEnumerable<JlgPast5GamesModel> GetPast5Games(JlgGameInfoViewModel viewModel)
        {
            IEnumerable<JlgPast5GamesModel> result = new List<JlgPast5GamesModel>();

            //ScheduleInfo 試合スケジュール_試合情報はHomeTeamIDがすでにわかっているので不要？
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("SELECT ");
            sb.Append("GR.Gameid,GR.GameDate,HomeTeamid,HomeTeamnames,AwayTeamid,AwayTeamnames,HomeScore, AwayScore,O.Win,O.Lose, O.Draw ");
            sb.Append("FROM ");
            sb.Append("[SPLG].[JLG].[SCHEDULEINFOHE] SH ");
            sb.Append(" JOIN [SPLG].[JLG].[GAMEREPORTLG] GR ");
            sb.Append("ON SH.GAMEID = GR.GAMEID ");
            sb.Append("LEFT JOIN ");
            sb.Append(" ");
            sb.Append("( ");
            sb.Append(" ");
            sb.Append("SELECT ");
            sb.Append("OD.UNIQUEID,WIN,LOSE,DRAW ");
            sb.Append("FROM ");
            sb.Append("[SPLG].[COM].[EXPECTTARGET] ET ");
            sb.Append("LEFT JOIN ");
            sb.Append("( ");
            sb.Append("SELECT ODD.EXPECTTARGETID, ");
            sb.Append("       ODD.UNIQUEID, ");
            sb.Append("       Sum(CASE BE.BETSELECTID ");
            sb.Append("             WHEN 1 THEN ODD.ODDS ");
            sb.Append("             ELSE 0 ");
            sb.Append("           END) AS WIN, ");
            sb.Append("       Sum(CASE BE.BETSELECTID ");
            sb.Append("             WHEN 2 THEN ODD.ODDS ");
            sb.Append("             ELSE 0 ");
            sb.Append("           END) AS LOSE, ");
            sb.Append("       Sum(CASE BE.BETSELECTID ");
            sb.Append("             WHEN 3 THEN ODD.ODDS ");
            sb.Append("             ELSE 0 ");
            sb.Append("           END) AS DRAW ");
            sb.Append("FROM   [SPLG].[COM].[ODDSINFO] ODD ");
            sb.Append(" JOIN [SPLG].[COM].[BETSELECTMASTER] BE ON ODD.BETSELECTMASTERID = BE.BETSELECTMASTERID ");
            sb.Append(" WHERE  ODD.CLASSIFICATIONTYPE = 2 ");
            sb.Append("       AND ODD.EXPECTTARGETID = 4 ");
            sb.Append("       AND ODD.UNIQUEID = " + viewModel.GameID + " ");
            sb.Append("GROUP  BY ODD.EXPECTTARGETID, ");
            sb.Append("          ODD.UNIQUEID ");
            sb.Append(" ");
            sb.Append(") OD ");
            sb.Append("ON ET.EXPECTTARGETID = OD.EXPECTTARGETID AND ");
            sb.Append("ET.UNIQUEID = OD.UNIQUEID ");
            sb.Append("WHERE ");
            sb.Append("ET.SPORTSID = 2 ");
            sb.Append(" ");
            sb.Append(" ");
            sb.Append(") O ");
            sb.Append("ON O.UNIQUEID = SH.GAMEID ");
            sb.Append("WHERE ((SH.HOMETEAMID = " + viewModel.HomeTeamID + " ");
            sb.Append("AND SH.AWAYTEAMID = " + viewModel.AwayTeamID + ") ");
            sb.Append("OR (SH.HOMETEAMID = " + viewModel.AwayTeamID + " ");
            sb.Append("AND SH.AWAYTEAMID = " + viewModel.HomeTeamID + ")) ");
            sb.Append("AND SH.GAMEID != " + viewModel.GameID + " ");
            sb.Append("AND GR.GAMEDATE < " + viewModel.GameDateManaged + " ");

            string query = sb.ToString();

            var lines = com.Database.SqlQuery<JlgPast5GamesModel>(@query).ToList<JlgPast5GamesModel>();

            return result;
        }

        #region Get Last 5 GameInfo From History By TeamID for HomeTeam
        /// <summary>
        /// Get 5 game info that 2 team took part in.
        /// Need HomeTeamID, VisitorTeamID, DateJPN to get data.
        /// </summary>
        /// <param name="gameID">Game id need get 2 teams info.</param>
        /// <returns>List game store in history.</returns>
        public IEnumerable<JlgLast5GamesModel> GetLast5HomeGames(int TeamID)
        {
            var query = default(IEnumerable<JlgLast5GamesModel>);
            if (TeamID != 0)
            {
                //Get old games in history.
                query = ((from rit5 in jlg.ResultInfoT5
                          join t5gr in jlg.Team5thGameReportT5 on rit5.Team5thGameReportT5Id equals t5gr.Team5thGameReportT5Id
                          where (t5gr.TeamID == TeamID)
                          where (rit5.HV == 1)
                          select new JlgLast5GamesModel
                          {
                              GameDate = rit5.GameDate,
                              HomeTeamid = (int)t5gr.TeamID,
                              HomeTeamnames = t5gr.TeamNameS,
                              AwayTeamid = (int)rit5.ID,
                              AwayTeamnames = rit5.NameS,
                              Score = rit5.Score,
                              Lose = rit5.Lose,
                              GameResult = rit5.GameResult == "W" ? "○" : rit5.GameResult == "L" ? "●" : "△",
                              StadiumNameS = rit5.StadiumNameS
                          }).Union(
                        from rit5 in jlg.ResultInfoT5
                        join t5gr in jlg.Team5thGameReportT5 on rit5.Team5thGameReportT5Id equals t5gr.Team5thGameReportT5Id
                        where (t5gr.TeamID == TeamID)
                        where (rit5.HV == 2)
                        select new JlgLast5GamesModel
                        {
                            GameDate = rit5.GameDate,
                            HomeTeamid = (int)rit5.ID,
                            HomeTeamnames = rit5.NameS,
                            AwayTeamid = (int)t5gr.TeamID,
                            AwayTeamnames = t5gr.TeamNameS,
                            Score = rit5.Score,
                            Lose = rit5.Lose,
                            GameResult = rit5.GameResult == "W" ? "○" : rit5.GameResult == "L" ? "●" : "△",
                            StadiumNameS = rit5.StadiumNameS
                        })).OrderByDescending(rit5 => rit5.GameDate).Take(5).ToList();
            }

            return query;
        }
        #endregion
        #region Get Last 5 GameInfo From History By TeamID for AwayTeam
        /// <summary>
        /// Get 5 game info that 2 team took part in.
        /// Need HomeTeamID, VisitorTeamID, DateJPN to get data.
        /// </summary>
        /// <param name="gameID">Game id need get 2 teams info.</param>
        /// <returns>List game store in history.</returns>
        public IEnumerable<JlgLast5GamesModel> GetLast5AwayGames(int TeamID)
        {
            var query = default(IEnumerable<JlgLast5GamesModel>);
            if (TeamID != 0)
            {
                //Get old games in history.
                query = ((from rit5 in jlg.ResultInfoT5
                          join t5gr in jlg.Team5thGameReportT5 on rit5.Team5thGameReportT5Id equals t5gr.Team5thGameReportT5Id
                          where (t5gr.TeamID == TeamID)
                          where (rit5.HV == 1)
                          select new JlgLast5GamesModel
                          {
                              GameDate = rit5.GameDate,
                              HomeTeamid = (int)t5gr.TeamID,
                              HomeTeamnames = t5gr.TeamNameS,
                              AwayTeamid = (int)rit5.ID,
                              AwayTeamnames = rit5.NameS,
                              Score = rit5.Score,
                              Lose = rit5.Lose,
                              GameResult = rit5.GameResult == "W" ? "○" : rit5.GameResult == "L" ? "●" : "△",
                              StadiumNameS = rit5.StadiumNameS
                          }).Union(
                        from rit5 in jlg.ResultInfoT5
                        join t5gr in jlg.Team5thGameReportT5 on rit5.Team5thGameReportT5Id equals t5gr.Team5thGameReportT5Id
                        where (t5gr.TeamID == TeamID)
                        where (rit5.HV == 2)
                        select new JlgLast5GamesModel
                        {
                            GameDate = rit5.GameDate,
                            HomeTeamid = (int)rit5.ID,
                            HomeTeamnames = rit5.NameS,
                            AwayTeamid = (int)t5gr.TeamID,
                            AwayTeamnames = t5gr.TeamNameS,
                            Score = rit5.Score,
                            Lose = rit5.Lose,
                            GameResult = rit5.GameResult == "W" ? "○" : rit5.GameResult == "L" ? "●" : "△",
                            StadiumNameS = rit5.StadiumNameS
                        })).OrderByDescending(rit5 => rit5.GameDate).Take(5).ToList();
            }
            return query;
        }
        #endregion

        /// <summary>
        /// 試合後 
        /// 一試合速報_試合終了情報
        /// 天気等
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        private JlgGameEndInfoModel GetGameEndInfo(int? gameID)
        {
            return (from ge in jlg.GameEndInfoLG   //試合速報_試合終了情報
                    join gr in jlg.GameReportLG    //一試合速報_ヘッダー情報
                    on ge.GameReportLGId equals gr.GameReportLGId
                    where gr.GameID == gameID
                    select new JlgGameEndInfoModel
                    {
                        Linesman1 = gr.Linesman1,
                        Linesman2 = gr.Linesman2,
                        Referee = gr.Referee,
                        Spectators = ge.Spectators,
                        Condition = ge.Condition,
                        Humidity = ge.Humidity,
                        Surface = ge.Surface,
                        Temperature = ge.Temperature,
                        Weather = ge.Weather,
                        Wind = ge.Wind
                    }).FirstOrDefault();
        }

        /// <summary>
        /// 試合終了
        /// 画面仕様書には
        /// ３・ホーム・アウェイ別得点状況 のアクセス方法	
        /// ４・試合結果 のアクセス方法	
        /// があるが、HTMLには経過ではなく静的なテーブルがある（PKの下の試合終了）
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        public JlgGameEndScroreInfoModel GetGameEndScoreInfo(int? gameID, int hv)
        {
            var query = (from gr in jlg.GameReportLG      //一試合速報_ヘッダー情報
                         join ti in jlg.TeamInfoLG                            //一試合速報_チーム情報
                         on gr.GameReportLGId equals ti.GameReportLGId
                         //join go in jlg.GoalInfoLG                        //一試合速報_得点情報
                         //on gr.GameReportLGId equals go.GameReportLGId
                         //join wa in jlg.WarningInfoLG                     //一試合速報_警告、退場情報
                         //on gr.GameReportLGId equals wa.GameReportLGId
                         where gr.GameID == gameID
                         && ti.HV == hv                          //Home
                         //orderby wa.Time
                         select new JlgGameEndScroreInfoModel
                         {
                             CK = ti.CK,
                             FKD = ti.FKD,
                             FKI = ti.FKI,
                             Gain = ti.Gain,
                             GK = ti.GK,
                             Offside = ti.Offside,
                             PK = ti.PK,
                             Red = ti.Red,
                             Score = ti.Score,
                             Shoot = ti.Shoot,
                             ShootGoal = ti.ShootGoal,
                             Yellow = ti.Yellow

                         });

            JlgGameEndScroreInfoModel gameScore = query.FirstOrDefault();

            return gameScore;
        }

        /// <summary>
        /// 試合中
        /// コメント配信_コメント情報
        /// </summary>
        /// <param name="gameId">試合ID</param>
        /// <returns></returns>
        private List<JlgGameText> GetComment(int? gameId)
        {
            var scheduleInfo = this.jlg.ScheduleInfo.Where(si => si.GameID == gameId).FirstOrDefault();
            //var scheduleInfo = (from si in jlg.ScheduleInfo where si.GameID == gameId select si).FirstOrDefault();
            var teamDic = new Dictionary<int, string>();
            if (scheduleInfo != null)
            {
                teamDic.Add(scheduleInfo.HomeTeamID.Value, scheduleInfo.HomeTeamNameS);
                teamDic.Add(scheduleInfo.AwayTeamID.Value, scheduleInfo.AwayTeamNameS);
            }

            return this.GetComment(gameId, teamDic);
        }

        /// <summary>
        /// 試合中
        /// コメント配信_コメント情報
        /// </summary>
        /// <param name="gameID">試合ID</param>
        /// <param name="teamDic">Key:チームID Value：チーム名略 のDictionary</param>
        private List<JlgGameText> GetComment(int? gameID, Dictionary<int, string> teamDic)
        {
            var entities = (from c in jlg.CommentInfo     //コメント配信_コメント情報							
                            join ch in jlg.CommentReport  //コメント配信_ヘッダー情報
                            on c.CommentReportId equals ch.CommentReportId
                            where ch.GameID == gameID
                            select c).ToList();

            return entities.Select(x => new JlgGameText(x, entities, teamDic))
                           .OrderByDescending(a => a.StateID)
                           .ThenByDescending(b => b.Half)
                           .ThenByDescending(c => c.CommentInfoId)
                           .ToList();
        }

        /// <summary>
        /// 試合後
        /// 選評
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        private List<JlgRecapModel> GetRecap(int? gameID)
        {
            var query = (from rrr in jlg.RecapReportRE     //戦評価_ヘッダー情報 							
                         where rrr.GameID == gameID
                         orderby rrr.CreatedDate
                         select new JlgRecapModel
                         {

                             EndTime = rrr.EndTime,
                             GameRecap = rrr.GameRecap,
                             StartTime = rrr.StartTime
                         }).ToList();

            return query;
        }


        /// <summary>
        //試合後
        //選評
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="viewModel"></param>
        private List<JlgRecapModel> GetRecap2(int? gameID)
        {
            var query = (from rrr in jlg.RecapReportRE     //戦評価_ヘッダー情報 							
                         join tir in jlg.TeamInfoRE on rrr.RecapReportREId equals tir.RecapReportREId  //戦評_チーム情報
                         join pir in jlg.PlayerInfoRE on tir.TeamInfoREId equals pir.TeamInfoREId   //戦評_得点者
                         where rrr.GameID == gameID
                         orderby rrr.CreatedDate
                         select new JlgRecapModel
                         {

                             EndTime = rrr.EndTime,
                             GameRecap = rrr.GameRecap,
                             Half = pir.Half,
                             PlayerName = pir.PlayerName,
                             PlayerNameS = pir.PlayerNameS,
                             StartTime = rrr.StartTime,
                             StateID = pir.StateID,
                             StateName = pir.StateName,
                             Time = pir.Time

                         });
            var recaps = query.ToList();

            return recaps;
        }

        #endregion

        #region Partial View

        /// <summary>
        /// ゲームのステータスの確認
        /// </summary>
        /// <param name="gameID"></param>
        private int setGameStatus(int? gameID)
        {
            //Get game status the first time.
            int gameStatus = JlgCommon.GetStatusMatch(gameID.ToString());

            //Using TempData to store gameID from request to another request.
            TempData["gameID"] = gameID;
            TempData.Keep("gameID");

            //int gameStatus = 0;

            //Use Session to store Status : need store long time and many request.
            Session["Status"] = gameStatus;
            @ViewBag.Status = gameStatus;

            return gameStatus;
        }

        /// <summary>
        /// Show PlayerInfo of selected game in partial view.
        /// </summary>
        /// <returns>PartialView : _JlgGameInfoPlayerInfo</returns>        
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 15)]
        public ActionResult ShowGameInfoPlayerInfo(int? gameID, JlgGameInfoViewModel viewModel = null)
        {
            //Htmlアクションから呼ばれた場合、別プロセスとなるのでviewModelを取得しなおす
            if (viewModel == null)
                viewModel = getViewModel(gameID);

            return PartialView("_JlgGameInfoPlayerInfo", viewModel);

        }

        /// <summary>
        /// テキスト速報Viewを表示
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 15)]
        public ActionResult ShowGameTexts(int gameId)
        {
            var model = this.GetComment(gameId);
            return PartialView("_JlgGameTexts", model);
        }

        /// <summary>
        /// Edit Point in popup (mobile).
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>Game info.</returns>
        [NoCache]
        public ActionResult EditPointInMobile(string sportID, int? gameID, string expectTargetID, int betSelectID, string memberID, int teamID, string reloadurl, string reloadarea)
        {
            if (gameID != null)
            {
                var spID = Convert.ToInt32(sportID);
                ViewBag.SportID = spID;
                int mID = Convert.ToInt32(memberID);
                var query = default(ExpectationInfoModel);
                //Decrypt text
                long lgExtTarget = Convert.ToInt64(expectTargetID);
                //Get expect point 
                query = (from et in com.ExpectTarget
                         join ep in com.ExpectPoint on et.ExpectTargetID equals ep.ExpectTargetID
                         where et.ExpectTargetID == lgExtTarget
                            && et.UniqueID == gameID
                            && ep.CreatedAccountID == memberID
                            && ep.BetSelectID == betSelectID
                            && ep.UniqueID == teamID
                         select new ExpectationInfoModel
                         {
                             PointID = ep.PointID,
                             ExpectPointID = (ep.ExpectPointID == null ? 0 : ep.ExpectPointID),
                             ExpectPoint = (ep.ExpectPoint1 == null ? 0 : ep.ExpectPoint1),
                             BetSelectID = (ep.BetSelectID.Value == null ? 0 : ep.BetSelectID.Value),
                             GameID = (et.UniqueID == null ? 0 : et.UniqueID),
                             Odds = (from od in com.OddsInfo
                                     join bsm in com.BetSelectMaster on od.BetSelectMasterID equals bsm.BetSelectMasterID
                                     where ep.BetSelectID == bsm.BetSelectID
                                         && et.SportsID == spID
                                         && et.SportsID == od.SportID
                                         && od.ExpectTargetID == et.ExpectTargetID
                                     select od.Odds).FirstOrDefault(),
                         }).FirstOrDefault();

                query.ReloadUrl = reloadurl;
                query.ReloadArea = reloadarea;

                var gameInfo = new GameInfoViewModel();
                gameInfo = NpbCommon.GetGameInfoByGameID(Constants.JLG_SPORT_ID, (int)gameID);

                if (gameID != null)
                {
                    gameInfo.ExpectationInfo = query;
                }

                return PartialView("_JlgPointEdit", gameInfo);
            }
            else
            {
                return PartialView("_JlgPointEdit");
            }

        }

        #endregion



        #region Show Game In RightContent
        /// <summary>
        /// Show game info in right content.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>List game in date.</returns>
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 15)]
        public ActionResult ShowGameInRightContent(int? gameID)
        {
            if (gameID != null)
            {
                //Get status from this session, this session must be not null. 
                int status = (int)Session["Status"];

                List<JlgGameInfoViewModel> lstGame = new List<JlgGameInfoViewModel>();

                JlgGameInfoViewModel gameInfo = getViewModel(gameID);

                lstGame.Add(gameInfo);

                return PartialView("_NpbRightGames", lstGame);
            }
            else
            {
                return PartialView("_NpbRightGames", null);
            }
        }
        #endregion


        #region Json Result

        #region Get Game Status By GameID
        /// <summary>
        /// Get status of game: load or not load partialview _NpbGameInfoPlayerInfo.cshtml.
        /// </summary>
        /// <param name="gameID">GameID.</param>
        /// <returns>0: Not load _NpbGameInfoPlayerInfo.cshtml, status not changed.</returns>
        /// <returns>1: Must load _NpbGameInfoPlayerInfo.cshtml, status changed.</returns>
        [HttpPost]
        public JsonResult GetGameStatusByGameID(int gameID)
        {
            List<string> lstResult = new List<string>();
            var result = false;
            var statusNew = -1;
            if (gameID != null)
            {
                //Random rnd = new Random();
                //int gameStatus = rnd.Next(0, 3);
                int gameStatus = JlgCommon.GetStatusMatch(gameID.ToString());
                statusNew = gameStatus;
                if (Session["Status"] == null || statusNew != (int)Session["Status"])
                {
                    //Update new status to session.
                    Session["Status"] = statusNew;
                    result = true;
                }
            }
            lstResult.Add(result.ToString());
            lstResult.Add(Session["Status"].ToString());
            return Json(lstResult, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #endregion
        /// <summary>
        /// ゴール情報の取得
        /// 一試合速報_ゴール情報
        /// </summary>
        private List<JlgGoalInfo> GetGameInfos(int gameID, int hv)
        {
            if (hv == 1)
            {
                List<JlgGoalInfo> gif = (from si in jlg.ScheduleInfo       //試合スケジュール
                                        join grl in jlg.GameReportLG on si.GameID equals grl.GameID     //一試合速報_ゴール情報ヘッダー
                                        join gil in jlg.GoalInfoLG on new { k1 = grl.GameReportLGId, k2 = si.HomeTeamID } equals new { k1 = gil.GameReportLGId, k2 = gil.ScoreTeamID } // 一試合速報_ゴール情報  
                                        where si.GameID == gameID
                                        orderby gil.Time
                                        select new JlgGoalInfo {
                                            GoalInfoLGId = gil.GoalInfoLGId,
                                            GameReportLGId = gil.GameReportLGId,
                                            No = gil.No,
                                            StateID = gil.StateID,
                                            StateName = gil.StateName,
                                            Time = gil.Time,
                                            Half = gil.Half,
                                            TeamID = gil.TeamID,
                                            TeamNameS = gil.TeamNameS,
                                            GPlayerID = gil.GPlayerID,
                                            GPlayerName = gil.GPlayerName,
                                            GPlayerNameS = gil.GPlayerNameS,
                                            GPlayerUni = gil.GPlayerUni,
                                            Body = gil.Body,
                                            Operation = gil.Operation,
                                            APlayerID = gil.APlayerID,
                                            APlayerNameS = gil.APlayerNameS,
                                            APlayerUni = gil.APlayerUni,
                                            AssistKind = gil.AssistKind,
                                            OwnGoalF = gil.OwnGoalF,
                                            ScoreTeamID = gil.ScoreTeamID,
                                            ScoreTeamNameS = gil.ScoreTeamNameS,
                                            CreatedDate = gil.CreatedDate
                                        } 
                                        ).ToList();
                return gif;
            }
            else
            {
                List<JlgGoalInfo> gif = (from si in jlg.ScheduleInfo       //試合スケジュール
                                        join grl in jlg.GameReportLG on si.GameID equals grl.GameID     //一試合速報_ゴール情報ヘッダー
                                        join gil in jlg.GoalInfoLG on new { k1 = grl.GameReportLGId, k2 = si.AwayTeamID } equals new { k1 = gil.GameReportLGId, k2 = gil.ScoreTeamID } // 一試合速報_ゴール情報  
                                        where si.GameID == gameID
                                        orderby gil.Time
                                        select new JlgGoalInfo
                                        {
                                            GoalInfoLGId = gil.GoalInfoLGId,
                                            GameReportLGId = gil.GameReportLGId,
                                            No = gil.No,
                                            StateID = gil.StateID,
                                            StateName = gil.StateName,
                                            Time = gil.Time,
                                            Half = gil.Half,
                                            TeamID = gil.TeamID,
                                            TeamNameS = gil.TeamNameS,
                                            GPlayerID = gil.GPlayerID,
                                            GPlayerName = gil.GPlayerName,
                                            GPlayerNameS = gil.GPlayerNameS,
                                            GPlayerUni = gil.GPlayerUni,
                                            Body = gil.Body,
                                            Operation = gil.Operation,
                                            APlayerID = gil.APlayerID,
                                            APlayerNameS = gil.APlayerNameS,
                                            APlayerUni = gil.APlayerUni,
                                            AssistKind = gil.AssistKind,
                                            OwnGoalF = gil.OwnGoalF,
                                            ScoreTeamID = gil.ScoreTeamID,
                                            ScoreTeamNameS = gil.ScoreTeamNameS,
                                            CreatedDate = gil.CreatedDate
                                        }
                                        ).ToList(); return gif;
            }

        }
        /// <summary>
        /// 警告・退場の取得
        /// 一試合速報_ゴール情報
        /// </summary>
        private List<WarningInfoLG> GetWarningInfos(int gameID, int hv)
        {
            if (hv == 1)
            {
                List<WarningInfoLG> wil = (from si in jlg.ScheduleInfo       //試合スケジュール
                                           join grl in jlg.GameReportLG on si.GameID equals grl.GameID     //一試合速報_ゴール情報ヘッダー
                                           join wilg in jlg.WarningInfoLG on new { k1 = grl.GameReportLGId, k2 = si.HomeTeamID } equals new { k1 = wilg.GameReportLGId, k2 = wilg.TeamID } // 一試合速報_ゴール情報  
                                           where si.GameID == gameID
                                           orderby wilg.Time
                                           select wilg).ToList();
                return wil;
            }
            else
            {
                List<WarningInfoLG> wil = (from si in jlg.ScheduleInfo       //試合スケジュール
                                           join grl in jlg.GameReportLG on si.GameID equals grl.GameID     //一試合速報_ゴール情報ヘッダー
                                           join wilg in jlg.WarningInfoLG on new { k1 = grl.GameReportLGId, k2 = si.AwayTeamID } equals new { k1 = wilg.GameReportLGId, k2 = wilg.TeamID } // 一試合速報_ゴール情報  
                                           where si.GameID == gameID
                                           orderby wilg.Time
                                           select wilg).ToList();
                return wil;
            }

        }
        // Todo:JlgServiceにうつす
        /// <summary>
        /// 各チームのスターティングメンバーの情報を取得
        /// </summary>
        public JlgGameInfoViewModel GetStartingInfo(JlgGameInfoViewModel viewModel,int gameID)
        {
            // チーム関連情報取得
            int homeTeamID = jlgService.GetHomeTeamID(gameID);
            int awayTeamID = jlgService.GetAwayTeamID(gameID);
            // チーム情報取得
            viewModel.homeTeamSpec = jlgService.GetTeamSpecByTeamId(homeTeamID,gameID);
            viewModel.awayTeamSpec = jlgService.GetTeamSpecByTeamId(awayTeamID, gameID);

            //////////////////////////////////////////////////////////////////////
            //スターティングメンバー情報　ベンチ入り選手情報
            //得点・警告、退場・交代情報                
            //ホーム
            viewModel.HomeStartingMembers = jlgService.GetStartingPlayerGameInfos((int)gameID, 1);
            viewModel.HomeSubMembers = jlgService.GetSubPlayerGameInfos((int)gameID, 1);
            viewModel.HomeGoalInfos = GetGameInfos((int)gameID, 1);
            viewModel.HomeWarningInfos = GetWarningInfos((int)gameID, 1);
            //アウエイ
            viewModel.AwayStartingMembers = jlgService.GetStartingPlayerGameInfos((int)gameID, 2);
            viewModel.AwaySubMembers = jlgService.GetSubPlayerGameInfos((int)gameID, 2);
            viewModel.AwayGoalInfos = GetGameInfos((int)gameID, 2);
            viewModel.AwayWarningInfos = GetWarningInfos((int)gameID, 2);

            //----------------------------------------------------------------------
            //欠場選手情報
            viewModel.HomeInjuredMembers = jlgService.GetInjuredPlayers((int)viewModel.HomeTeamID);
            viewModel.AwayInjuredMembers = jlgService.GetInjuredPlayers((int)viewModel.AwayTeamID);

            //----------------------------------------------------------------------
            //出場停止選手情報
            viewModel.HomeSuspendedMembers = jlgService.GetSuspendedPlayers(gameID, (int)viewModel.HomeTeamID);
            viewModel.AwaySuspendedMembers = jlgService.GetSuspendedPlayers(gameID, (int)viewModel.AwayTeamID);

            //----------------------------------------------------------------------
            //得点・警告、退場・交代情報                
            List<JlgPlayerGameDetailInfoModel> homeStartMamberGameDetailCardInfos, homeSubMamberGameDetailCardInfos, awayStartMamberGameDetailCardInfos, awaySubMamberGameDetailCardInfos;
            List<JlgPlayerGameDetailInfoModel> homeStartMamberGameDetailGoalInfos, homeSubMamberGameDetailGoalInfos, awayStartMamberGameDetailGoalInfos, awaySubMamberGameDetailGoalInfos;
            List<JlgPlayerGameDetailInfoModel> homeStartMamberGameDetailInInfos, homeSubMamberGameDetailInInfos, awayStartMamberGameDetailInInfos, awaySubMamberGameDetailInInfos;
            List<JlgPlayerGameDetailInfoModel> homeStartMamberGameDetailOutInfos, homeSubMamberGameDetailOutInfos, awayStartMamberGameDetailOutInfos, awaySubMamberGameDetailOutInfos;

            GetPlayerGameDetailInfos(viewModel.GameID, 1, 1, out homeStartMamberGameDetailCardInfos, out homeSubMamberGameDetailCardInfos);
            GetPlayerGameDetailInfos(viewModel.GameID, 2, 1, out awayStartMamberGameDetailCardInfos, out awaySubMamberGameDetailCardInfos);
            GetPlayerGameDetailInfos(gameID, 1, 2, out homeStartMamberGameDetailGoalInfos, out homeSubMamberGameDetailGoalInfos);
            GetPlayerGameDetailInfos(gameID, 2, 2, out awayStartMamberGameDetailGoalInfos, out awaySubMamberGameDetailGoalInfos);
            GetPlayerGameDetailInfos(gameID, 1, 3, out homeStartMamberGameDetailInInfos, out homeSubMamberGameDetailInInfos);
            GetPlayerGameDetailInfos(gameID, 2, 3, out awayStartMamberGameDetailInInfos, out awaySubMamberGameDetailInInfos);
            GetPlayerGameDetailInfos(gameID, 1, 4, out homeStartMamberGameDetailOutInfos, out homeSubMamberGameDetailOutInfos);
            GetPlayerGameDetailInfos(gameID, 2, 4, out awayStartMamberGameDetailOutInfos, out awaySubMamberGameDetailOutInfos);
            viewModel.HomeStartingMamberGameDetailCardInfos = homeStartMamberGameDetailCardInfos;
            viewModel.HomeSubMamberGameDetailCardInfos = homeSubMamberGameDetailCardInfos;
            viewModel.AwayStartingMamberGameDetailCardInfos = awayStartMamberGameDetailCardInfos;
            viewModel.AwaySubMamberGameDetailCardInfos = awaySubMamberGameDetailCardInfos;
            viewModel.HomeStartingMamberGameDetailGoalInfos = homeStartMamberGameDetailGoalInfos;
            viewModel.HomeSubMamberGameDetailGoalInfos = homeSubMamberGameDetailGoalInfos;
            viewModel.AwayStartingMamberGameDetailGoalInfos = awayStartMamberGameDetailGoalInfos;
            viewModel.AwaySubMamberGameDetailGoalInfos = awaySubMamberGameDetailGoalInfos;
            viewModel.HomeStartingMamberGameDetailInInfos = homeStartMamberGameDetailInInfos;
            viewModel.HomeSubMamberGameDetailInInfos = homeSubMamberGameDetailInInfos;
            viewModel.AwayStartingMamberGameDetailInInfos = awayStartMamberGameDetailInInfos;
            viewModel.AwaySubMamberGameDetailInInfos = awaySubMamberGameDetailInInfos;
            viewModel.HomeStartingMamberGameDetailOutInfos = homeStartMamberGameDetailOutInfos;
            viewModel.HomeSubMamberGameDetailOutInfos = homeSubMamberGameDetailOutInfos;
            viewModel.AwayStartingMamberGameDetailOutInfos = awayStartMamberGameDetailOutInfos;
            viewModel.AwaySubMamberGameDetailOutInfos = awaySubMamberGameDetailOutInfos;
            return viewModel;
        }
        /// <summary>
        /// スターティングメンバー情報取得（フォーメーション含）
        /// </summary>
        [ChildActionOnly]
        public ActionResult GetStartingMembers(int GameID)
        {
            // Gameステータス取得
            int GameStatus = setGameStatus(GameID);
            // ビューに渡す入れ物
            JlgGameInfoViewModel ViewModel = new JlgGameInfoViewModel(); 

            if (GameStatus == 0)
            {
                ViewModel = jlgService.GetStartingInformation(GameID);
                return PartialView("_JlgStartingMembersPre", ViewModel);
            }
            else
            {
                ViewModel = GetStartingInfo(ViewModel, GameID);
                return PartialView("_JlgStartingMembers", ViewModel);
            }

        }
    }
}
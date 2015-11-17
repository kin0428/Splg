using System;
using System.Collections.Generic;
using System.Linq;
using Splg.Areas.Npb.Models;
using Splg.Areas.Npb.Models.ViewModel;
using Splg.Models.Game.ViewModel;

namespace Splg.Areas.Npb.Service
{
    public class NpbGameInfoService
    {
        // todo インスタンス管理
        private NpbEntities dbContext;

        public NpbGameInfoService(NpbEntities dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 試合情報ViewModelを取得
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="gameStatus"></param>
        /// <returns></returns>
        public NpbGameInfoViewModel GetViewModel(int? gameId, int gameStatus)
        {
            if (gameId != null)
            {
                //Get status from this session, this session must be not null. 
                //int status = (int)Session["Status"];

                //Set status for viewbag to use 1 request.
                //ViewBag.Status = status;

                var viewModel = new NpbGameInfoViewModel
                {
                    GameInSeasonSchedule = GetGameInfoCommonByGameId(gameId.Value)
                };

                if (viewModel.GameInSeasonSchedule != null)
                {
                    viewModel.ListPlayerInfo = GetPlayerInfoStByGameIdAndStatus(gameStatus, gameId.Value);

                    if (gameStatus == Constants.GAME_STATUS_PRE_GAME)
                    //if (status == 0)
                    {
                        viewModel.ListPreStartingPitcher = GetTeamInfoPspByGameId(gameId.Value);
                        viewModel.List5GamesInHistory = GetLast5GameInfoFromHistoryByGameId(gameId.Value,
                                                                                            viewModel.GameInSeasonSchedule.HomeTeamID,
                                                                                            viewModel.GameInSeasonSchedule.VisitorTeamID);
                    }
                    else if (gameStatus == Constants.GAME_STATUS_DURING_GAME)
                    //else if (status == 1)
                    {
                        viewModel.ListGameScoreOngoing = GetGameScoreOnGoingByGameId(gameId.Value);
                        viewModel.ListGameText = GetAllGameTextsByGameId(gameId.Value);
                    }
                    else
                    {
                        viewModel.ListGameText = GetAllGameTextsByGameId(gameId.Value);
                        viewModel.ListGameScoreEnd = GetGameScoreEndByGameId(gameId.Value);
                        viewModel.GameRoundComment = GetGameCommentByGameId(gameId.Value);
                        viewModel.ListPitcher = GetPitcherInfoInGameLiveByGameId(gameId.Value);
                        viewModel.ListHomerun = GetHomerunInGameLiveByGameId(gameId.Value);
                        viewModel.ListReliefInfoes = GetReliefInfoInGameLiveByGameId(gameId.Value);
                        viewModel.ListGameTextScore = GetGameTextScoreByGameId(gameId.Value);
                        //ViewBag.GameResult = GetGameResultByGameID(gameID.Value);
                    }
                }
                else
                {
                    viewModel = null;
                    //ViewBag.Status = -1;
                }

                return viewModel;
                //return PartialView("_NpbGameInfoPlayerInfo", gameInfo);
            }
            else
            {
                return null;
                //return PartialView("_NpbGameInfoPlayerInfo", null);
            }
        }

        #region Utilities

        #region Get TeamInfoPSP By GameID
        /// <summary>
        /// Get pre starting picher of home team and visitor team by GameID. 
        /// </summary>
        /// <param name="gameId">GameID to define another games.</param>
        /// <returns>List of pre starting pitcher.</returns>
        public IEnumerable<TeamInfoPSP> GetTeamInfoPspByGameId(int gameId)
        {
            var query = from gi in this.dbContext.GameInfoPSP
                        join ti in this.dbContext.TeamInfoPSP on gi.GameInfoPSPId equals ti.GameInfoPSPId
                        where gi.GameID == gameId
                        select ti;
            return query;
        }
        #endregion

        #region Get PlayerInfoST By GameID

        /// <summary>
        /// Get player take part in game By GameID and status..
        /// </summary>
        /// <param name="gameStatus"></param>
        /// <param name="gameId">GameID to define another games.</param>
        /// <returns>List of player in game.</returns>
        public IEnumerable<PlayerInfoInGame> GetPlayerInfoStByGameIdAndStatus(int gameStatus, int gameId)
        {
            var query = default(IEnumerable<PlayerInfoInGame>);

            if (gameStatus == Constants.GAME_STATUS_PRE_GAME ||
                gameStatus == Constants.GAME_STATUS_POST_GAME)
            //if (gameStatus == 0 || gameStatus == 2)
            {
                query = (from st in this.dbContext.StartingGameST
                         join ti in this.dbContext.TeamInfoST on st.StartingGameSTId equals ti.StartingGameSTId
                         join pi in this.dbContext.PlayerInfoST on ti.TeamInfoSTId equals pi.TeamInfoSTId
                         join df in this.dbContext.DefenceLocationMST on pi.StartPosition equals df.DefenceLocationID
                         where st.GameID == gameId
                         select new PlayerInfoInGame
                         {
                             PlayerInfoStarting = pi,
                             TeamID = ti.TeamID,
                             HV = ti.HV,
                             PositionName = df.NameS,
                             PlayerID = pi.PlayerID,
                             PositionID = df.DefenceLocationID
                         })
                         .OrderBy(x => x.PlayerInfoStarting.CreatedDate)
                         .GroupBy(x => x.PlayerID)
                         .Select(x => x.FirstOrDefault());
            }
            else
            {
                query = (from go in this.dbContext.GameOrderGO
                         join to in this.dbContext.TeamInfoGO on go.GameOrderGOId equals to.GameOrderGOId
                         join pgo in this.dbContext.PlayerInfoGO on to.TeamInfoGOId equals pgo.TeamInfoGOId
                         join df in this.dbContext.DefenceLocationMST on pgo.Position equals df.DefenceLocationID
                         where go.GameID == gameId
                         select new PlayerInfoInGame
                         {
                             PlayerInfoGameOrder = pgo,
                             TeamID = to.TeamID,
                             HV = to.HV,
                             PositionName = df.NameS,
                             PlayerID = pgo.PlayerID,
                             PositionID = df.DefenceLocationID
                         })
                         .OrderBy(x => x.PlayerInfoGameOrder.CreatedDate)
                         .GroupBy(x => x.PlayerID)
                         .Select(x => x.FirstOrDefault()); ;
            }

            return query;
        }
        #endregion

        #region Get Last 5 GameInfo From History By GameID

        /// <summary>
        /// Get 5 game info that 2 team took part in.
        /// Need HomeTeamID, VisitorTeamID, DateJPN to get data.
        /// </summary>
        /// <param name="gameId">Game id need get 2 teams info.</param>
        /// <param name="homeTeamId"></param>
        /// <param name="visitorTeamId"></param>
        /// <returns>List game store in history.</returns>
        public IEnumerable<GameInHistory> GetLast5GameInfoFromHistoryByGameId(int gameId, int homeTeamId, int visitorTeamId)
        {
            var query = default(IEnumerable<GameInHistory>);

            if (homeTeamId != 0 && visitorTeamId != 0)
            {
                var strGameId = gameId.ToString().Substring(0, 8);
                var date = Convert.ToInt32(strGameId);

                //Get old games in history.
                query = (from giss in this.dbContext.GameInfoSSN
                         where ((giss.HTeamID == homeTeamId || giss.HTeamID == visitorTeamId) && (giss.VTeamID.Value == visitorTeamId || giss.VTeamID == homeTeamId) && giss.DateJPN < date)
                         orderby giss.DateJPN descending
                         select new GameInHistory
                         {
                             GameInfoSSN = giss,
                             OddsHomeTeam = 0,
                             OddsVisitor = 0,
                             OddsDraw = 0
                         }).Take(5).ToList();
            }

            return query;
        }
        #endregion

        #region Get GameInfo Common By GameID
        /// <summary>
        /// Get game info common : home team name, visitor team name,... by gameID. 
        /// </summary>
        /// <param name="gameId">Game ID</param>
        /// <returns>Game info.</returns>
        public GameInfoViewModel GetGameInfoCommonByGameId(int gameId)
        {
            var query = (from gi in this.dbContext.GameInfoSS
                         join ti in this.dbContext.TeamIconNpb on gi.HomeTeamID equals ti.TeamCD
                         join tiv in this.dbContext.TeamIconNpb on gi.VisitorTeamID equals tiv.TeamCD
                         where gi.ID == gameId
                         select new GameInfoViewModel
                         {
                             GameID = gi.ID,
                             GameDate = gi.GameDate,
                             Time = gi.Time,
                             HomeTeamID = gi.HomeTeamID.Value,
                             HomeTeamName = gi.HomeTeamName,
                             HomeTeamNameS = gi.HomeTeamNameS,
                             HomeTeamIcon = ti.TeamIcon,
                             VisitorTeamID = gi.VisitorTeamID.Value,
                             VisitorTeamName = gi.VisitorTeamName,
                             VisitorTeamNameS = gi.VisitorTeamNameS,
                             VisitorTeamIcon = tiv.TeamIcon
                         }).FirstOrDefault();
            return query;
        }
        #endregion

        #region Get Game Score OnGoing By GameID
        /// <summary>
        /// Get score in game ongoing by gameID.
        /// </summary>
        /// <param name="gameId">GameID</param>
        /// <returns>List score of 2 team : hometeam and visitorteam.</returns>
        public IEnumerable<ScoreInGame> GetGameScoreOnGoingByGameId(int gameId)
        {
            var query = from gi in this.dbContext.GameInfoGTE
                        join ti in this.dbContext.TeamInfoGTE on gi.GameInfoGTEId equals ti.GameInfoGTEId into tmp
                        from tit in tmp.DefaultIfEmpty()
                        where gi.GameID == gameId
                        select new ScoreInGame
                        {
                            HV = tit.HV,
                            TeamID = tit.ID,
                            NameS = tit.NameS,
                            Runs = tit.R.Value,
                            Hits = tit.H.Value,
                            Err = tit.E.Value,
                            TeamInfoGTEID = tit.TeamInfoGTEId,
                            Inning = (gi.Inning.Value == null ? 0 : gi.Inning.Value)
                        };
            return query;
        }
        #endregion

        #region Get GameTextScore By GameID
        /// <summary>
        /// Get text score when game end by GameID.
        /// </summary>
        /// <param name="gameId">GameID.</param>
        /// <returns>All text that have scores.</returns>
        public IEnumerable<GameText> GetGameTextScoreByGameId(int gameId)
        {
            var flag = (short)1;
            var query = from gi in this.dbContext.GameInfoGTE
                        join txti in this.dbContext.TextInfoGTE on gi.GameInfoGTEId equals txti.GameInfoGTEId
                        join bat in this.dbContext.BatGTE on txti.TextInfoGTEId equals bat.TextInfoGTEId
                        join tigte in this.dbContext.TextGTE on bat.BatGTEId equals tigte.BatGTEId
                        where gi.GameID == gameId && tigte.F == flag
                        select new GameText
                        {
                            Round = txti.ID.ToString().Substring(0, 1),
                            RoundName = (txti.Name ?? string.Empty),
                            //RoundName = (txti.Name == null ? string.Empty : txti.Name),
                            TextOfTeam = (tigte.Text ?? string.Empty),
                            //TextOfTeam = (tigte.Text == null ? string.Empty : tigte.Text),
                            ID = txti.ID
                        };
            return query;
        }
        #endregion

        #region Get All GameTexts By GameID
        /// <summary>
        /// Get text in game that ongoing by gameID.
        /// </summary>
        /// <param name="gameId">GameID.</param>
        /// <returns>All text in game.</returns>
        public IEnumerable<GameText> GetAllGameTextsByGameId(int gameId)
        {
            var query = default(IEnumerable<GameText>);

            var queryFirst = from gi in this.dbContext.GameInfoGTE
                             join txti in this.dbContext.TextInfoGTE on gi.GameInfoGTEId equals txti.GameInfoGTEId
                             join bat in this.dbContext.BatGTE on txti.TextInfoGTEId equals bat.TextInfoGTEId
                             join tigte in this.dbContext.TextGTE on bat.BatGTEId equals tigte.BatGTEId
                             where gi.GameID == gameId && txti.ID != 0
                             select new GameText
                             {
                                 GameInfoGTEID = gi.GameInfoGTEId,
                                 Round = txti.ID.ToString().Substring(0, 1),
                                 RoundName = (txti.Name ?? string.Empty),
                                 TextOfTeam = (tigte.Text ?? string.Empty),
                                 //RoundName = (txti.Name == null ? string.Empty : txti.Name),
                                 //TextOfTeam = (tigte.Text == null ? string.Empty : tigte.Text),
                                 ID = txti.ID,

                             };

            //Get last digit of ID.
            List<GameText> newList = new List<GameText>();
            foreach (var item in queryFirst.ToList())
            {
                var gText = item;
                gText.ID = Convert.ToInt32(item.ID.ToString().Substring(item.ID.ToString().Length - 1, 1));
                newList.Add(gText);
            }

            query = from q1 in newList
                    join ti in this.dbContext.TeamInfoGTE on q1.GameInfoGTEID equals ti.GameInfoGTEId
                    where ti.HV != q1.ID
                    select new GameText
                    {
                        GameInfoGTEID = q1.GameInfoGTEID,
                        Round = q1.Round,
                        RoundName = q1.RoundName,
                        TeamID = ti.ID,
                        TeamNameS = ti.NameS,
                        TextOfTeam = q1.TextOfTeam,
                        ID = q1.ID,
                    };

            return query;
        }
        #endregion

        #region Get Game Score End By GameID
        /// <summary>
        /// Get score when game end by gameID.
        /// </summary>
        /// <param name="gameId">GameID.</param>
        /// <returns>List game score.</returns>
        public IEnumerable<ScoreInGame> GetGameScoreEndByGameId(int gameId)
        {
            var query = default(IEnumerable<ScoreInGame>);
            var queryFirst = from gigte in this.dbContext.GameInfoGTE
                             join tigte in this.dbContext.TeamInfoGTE on gigte.GameInfoGTEId equals tigte.GameInfoGTEId
                             where gigte.GameID == gameId
                             select new
                             {
                                 tigte.ID,
                                 tigte.E,
                                 tigte.H
                             };
            if (!queryFirst.Any())
            {
                query = from gi in this.dbContext.GameInfoCO
                        join ti in this.dbContext.TeamInfoCO on gi.GameInfoCOId equals ti.GameInfoCOId
                        where gi.GameID == gameId
                        select new ScoreInGame
                        {
                            TeamID = ti.TeamID,
                            NameS = ti.TeamNameS,
                            HV = ti.HomeVisitor,
                            Runs = ti.TotalScore.Value,
                            Hits = 0,
                            Err = 0,
                            Inning = (gi.Inning.Value == null ? 0 : gi.Inning.Value),
                            TeamInfoCOID = ti.TeamInfoCOId
                        };
            }
            else
            {
                query = from gi in this.dbContext.GameInfoCO
                        join ti in this.dbContext.TeamInfoCO on gi.GameInfoCOId equals ti.GameInfoCOId
                        join q1 in queryFirst on ti.TeamID equals q1.ID
                        where gi.GameID == gameId
                        select new ScoreInGame
                        {
                            TeamID = ti.TeamID,
                            NameS = ti.TeamNameS,
                            HV = ti.HomeVisitor,
                            Runs = ti.TotalScore.Value,
                            Hits = q1.H.Value,
                            Err = q1.E.Value,
                            Inning = (gi.Inning.Value == null ? 0 : gi.Inning.Value),
                            TeamInfoCOID = ti.TeamInfoCOId
                        };
            }

            return query;
        }
        #endregion

        #region Get Game Comment By GameID
        /// <summary>
        /// Get round and game comment by gameID.
        /// </summary>
        /// <param name="gameId">GameID</param>
        /// <returns>Round and Game Comment.</returns>
        public GameRoundComment GetGameCommentByGameId(int gameId)
        {
            var tmp1 = (short)1;
            var tmp2 = (short)2;

            var query = (from gi in this.dbContext.GameInfoCO
                         join ti in this.dbContext.TeamInfoCO on gi.GameInfoCOId equals ti.GameInfoCOId
                         join ti1 in this.dbContext.TeamInfoCO on gi.GameInfoCOId equals ti1.GameInfoCOId
                         where gi.GameID == gameId && ti.HomeVisitor == tmp1 && ti1.HomeVisitor == tmp2
                         select new GameRoundComment
                         {
                             HomeTeamNameS = ti.TeamNameS,
                             VisitorTeamNameS = ti1.TeamNameS,
                             Round = gi.Round.Value,
                             GameComment = (gi.GameComment ?? string.Empty),
                             //GameComment = (gi.GameComment == null ? string.Empty : gi.GameComment),
                             Atendance = gi.Atendance.Value,
                             StartTime = gi.StartTime.Value,
                             EndTime = gi.EndTime.Value,
                             Inning = gi.Inning.Value,
                             TB = gi.TB.Value,
                             TotalScoreHTeam = ti.TotalScore.Value,
                             TotalScoreVTeam = ti1.TotalScore.Value
                         }).FirstOrDefault();
            return query;
        }
        #endregion

        #region Get PitcherInfo In GameLive By GameID
        /// <summary>
        /// Get win pitcher, lose pitcher, and save pitcher info when end game.
        /// </summary>
        /// <param name="gameId">GameID</param>
        /// <returns>Player Info.</returns>
        public IEnumerable<PitcherGameEndInfo> GetPitcherInfoInGameLiveByGameId(int gameId)
        {
            var queryWin = from gl in this.dbContext.GameLiveGL
                           join ge in this.dbContext.GameEndInfoGL on gl.GameLiveGLId equals ge.GameLiveGLId
                           join wp in this.dbContext.WinPitcherGL on ge.GameEndInfoGLId equals wp.GameEndInfoGLId
                           join pi in this.dbContext.PersonInfo on wp.PlayerID equals pi.ID into tmp1
                           from pi1 in tmp1.DefaultIfEmpty()
                           join di in this.dbContext.Directory on pi1.DirectoryId equals di.DirectoryId into tmp2
                           from di1 in tmp2.DefaultIfEmpty()
                           where gl.GameID == gameId
                           select new PitcherGameEndInfo
                           {
                               TeamID = (di1.TeamID == null ? -1 : di1.TeamID),
                               TeamNameS = (di1.TeamNameS ?? string.Empty),
                               //TeamNameS = (di1.TeamNameS == null ? string.Empty : di1.TeamNameS),
                               PlayerID = wp.PlayerID,
                               PlayerNameS = wp.PlayerNameS,
                               Win = wp.Win.Value,
                               Lose = wp.Lose.Value,
                               Save = wp.Save.Value,
                               Type = 1
                           };

            var queryLose = from gl in this.dbContext.GameLiveGL
                            join ge in this.dbContext.GameEndInfoGL on gl.GameLiveGLId equals ge.GameLiveGLId
                            join wp in this.dbContext.LosePitcherGL on ge.GameEndInfoGLId equals wp.GameEndInfoGLId
                            join pi in this.dbContext.PersonInfo on wp.PlayerID equals pi.ID into tmp1
                            from pi1 in tmp1.DefaultIfEmpty()
                            join di in this.dbContext.Directory on pi1.DirectoryId equals di.DirectoryId into tmp2
                            from di1 in tmp2.DefaultIfEmpty()
                            where gl.GameID == gameId
                            select new PitcherGameEndInfo
                            {
                                TeamID = (di1.TeamID == null ? -1 : di1.TeamID),
                                TeamNameS = (di1.TeamNameS ?? string.Empty),
                                //TeamNameS = (di1.TeamNameS == null ? string.Empty : di1.TeamNameS),
                                PlayerID = wp.PlayerID,
                                PlayerNameS = wp.PlayerNameS,
                                Win = wp.Win.Value,
                                Lose = wp.Lose.Value,
                                Save = wp.Save.Value,
                                Type = 2
                            };

            var querySave = from gl in this.dbContext.GameLiveGL
                            join ge in this.dbContext.GameEndInfoGL on gl.GameLiveGLId equals ge.GameLiveGLId
                            join wp in this.dbContext.SavePitcherGL on ge.GameEndInfoGLId equals wp.GameEndInfoGLId
                            join pi in this.dbContext.PersonInfo on wp.PlayerID equals pi.ID into tmp1
                            from pi1 in tmp1.DefaultIfEmpty()
                            join di in this.dbContext.Directory on pi1.DirectoryId equals di.DirectoryId into tmp2
                            from di1 in tmp2.DefaultIfEmpty()
                            where gl.GameID == gameId
                            select new PitcherGameEndInfo
                            {
                                TeamID = (di1.TeamID == null ? -1 : di1.TeamID),
                                TeamNameS = (di1.TeamNameS ?? string.Empty),
                                //TeamNameS = (di1.TeamNameS == null ? string.Empty : di1.TeamNameS),
                                PlayerID = wp.PlayerID,
                                PlayerNameS = wp.PlayerNameS,
                                Win = wp.Win.Value,
                                Lose = wp.Lose.Value,
                                Save = wp.Save.Value,
                                Type = 3
                            };

            var query = queryWin.Union(queryLose);
            var result = query.Union(querySave);

            return result;
        }
        #endregion

        #region Get Homerun In GameLive By GameID
        /// <summary>
        /// Get homerun info when game end by GameID.
        /// </summary>
        /// <param name="gameId">GameID</param>
        /// <returns>List homerun info.</returns>
        public IEnumerable<HomerunGameEndInfo> GetHomerunInGameLiveByGameId(int gameId)
        {
            var query = from gl in this.dbContext.GameLiveGL
                        join hr in this.dbContext.HomerunInfoGL on gl.GameLiveGLId equals hr.GameLiveGLId
                        join pi in this.dbContext.PlayerGL on hr.HomerunInfoGLId equals pi.HomerunInfoGLId
                        join de in this.dbContext.DetailsGL on pi.PlayerGLId equals de.DetailsGLId
                        join si in this.dbContext.ScoreInfoGL on gl.GameLiveGLId equals si.GameLiveGLId
                        join pgl in this.dbContext.TeamGL on si.ScoreInfoGLId equals pgl.ScoreInfoGLId
                        where gl.GameID == gameId && pgl.HV == hr.HV
                        select new HomerunGameEndInfo
                        {
                            TeamID = (pgl.ID == null ? -1 : pgl.ID),
                            TeamNameS = (pgl.NameS ?? string.Empty),
                            //TeamNameS = (pgl.NameS == null ? string.Empty : pgl.NameS),
                            HV = hr.HV,
                            PlayerID = pi.ID,
                            PlayerNameS = pi.NameS,
                            TotalC = de.TotalC
                        };
            return query;
        }
        #endregion

        #region Get ReliefInfo In GameLive By GameID
        /// <summary>
        /// Get relief info : pitcher and catcher when game end by GameID.
        /// </summary>
        /// <param name="gameId">GameID.</param>
        /// <returns>List player : pitchers and catchers.</returns>
        public IEnumerable<ReliefInfoGameEndInfo> GetReliefInfoInGameLiveByGameId(int gameId)
        {
            var queryFirst = from gl in this.dbContext.GameLiveGL
                             join ri in this.dbContext.ReliefInfoGL on gl.GameLiveGLId equals ri.GameLiveGLId
                             join pi in this.dbContext.PitcherGL on ri.ReliefInfoGLId equals pi.ReliefInfoGLId
                             join si in this.dbContext.ScoreInfoGL on gl.GameLiveGLId equals si.GameLiveGLId
                             join pgl in this.dbContext.TeamGL on si.ScoreInfoGLId equals pgl.ScoreInfoGLId
                             where gl.GameID == gameId && ri.HV == pgl.HV
                             select new ReliefInfoGameEndInfo
                             {
                                 PlayerID = pi.ID,
                                 TeamID = pgl.ID,
                                 TeamNameS = pgl.NameS,
                                 NameS = pi.NameS,
                                 Type = 1,
                                 HV = ri.HV
                             };
            var querySecond = from gl in this.dbContext.GameLiveGL
                              join ri in this.dbContext.ReliefInfoGL on gl.GameLiveGLId equals ri.GameLiveGLId
                              join ct in this.dbContext.CatcherGL on ri.ReliefInfoGLId equals ct.ReliefInfoGLId
                              join si in this.dbContext.ScoreInfoGL on gl.GameLiveGLId equals si.GameLiveGLId
                              join pgl in this.dbContext.TeamGL on si.ScoreInfoGLId equals pgl.ScoreInfoGLId
                              where gl.GameID == gameId && ri.HV == pgl.HV
                              select new ReliefInfoGameEndInfo
                              {
                                  PlayerID = ct.ID,
                                  TeamID = pgl.ID,
                                  TeamNameS = pgl.NameS,
                                  NameS = ct.NameS,
                                  Type = 2,
                                  HV = ri.HV
                              };
            var query = queryFirst.Union(querySecond);
            return query;
        }
        #endregion

        #region Get Game Result By GameID
        /// <summary>
        /// Get game result when game end :
        ///     1 : Hometeam.
        ///     2 : Visitorteam.
        ///     3 : Draw.
        ///     4 : Game pending.
        /// </summary>
        /// <param name="gameId">GameID</param>
        /// <returns>Game result : win - lose - draw - pending.</returns>
        public int GetGameResultByGameId(int gameId)
        {
            int intResult = 0;
            var query = (from gi in this.dbContext.GameInfoGTE
                         join ge in this.dbContext.GameEndInfoGTE on gi.GameInfoGTEId equals ge.GameInfoGTEId
                         where gi.GameID == gameId
                         select ge.ResultID).FirstOrDefault();
            intResult = query;
            return intResult;
        }
        #endregion

        #region Get GameInfo In RightContent By GameID
        /// <summary>
        /// Get all games info in right content by date of GameID.
        /// </summary>
        /// <param name="gameId">GameID</param>
        /// <returns>Game Info.</returns>
        public IEnumerable<GameInfoViewModel> GetGameInfoRightContentByDate(int gameId)
        {
            int gameIdSub = Convert.ToInt32(gameId.ToString().Substring(0, 8));
            var query = default(IEnumerable<GameInfoViewModel>);

            query = from gi in this.dbContext.GameInfoSS
                    where gi.GameDate == gameIdSub && gi.ID != gameId
                    select new GameInfoViewModel
                    {
                        GameID = gi.ID,
                        HomeTeamID = gi.HomeTeamID.Value,
                        VisitorTeamID = gi.VisitorTeamID.Value,
                        HomeTeamName = gi.HomeTeamNameS,
                        VisitorTeamName = gi.VisitorTeamNameS,
                        Round = gi.Round.Value,
                        GameDate = gi.GameDate,
                        Time = gi.Time
                    };
            return query;
        }
        #endregion

        #endregion
    }
}
#region Using directives
using System.Linq;
using Splg.Models;
using Splg.Models.Game.InfoModel;
#endregion

namespace Splg.Services.Game
{
    public class OddsService
    {
        #region Global Properties
        /// <summary>
        /// Declare context Member to get db.
        /// </summary>
        private ComEntities com = new ComEntities();
        #endregion

        public GameOddsInfoModel GetOddsInfoByGameID(int sportsId, int gameId, long memberId = 0)
        {
            var oddInfo = default(GameOddsInfoModel);

            var expectInfoService = new ExpectInfoService(this.com);

            int? betSelectedId = null; // 選択済のBET選択肢ID
            if (memberId > 0)
            {
                var expectPointInfo = expectInfoService.GetExpectPointInfo(gameId, memberId, sportsId);
                if (expectPointInfo != null) betSelectedId = expectPointInfo.BetSelectID;
            }

            oddInfo = (from et in com.ExpectTarget

                       // HomeTeam
                       join od in com.OddsInfo on et.ExpectTargetID equals od.ExpectTargetID into tmp1
                       from odh in tmp1.DefaultIfEmpty()
                       join bsm in com.BetSelectMaster on odh.BetSelectMasterID equals bsm.BetSelectMasterID into bsmtp1
                       from bsmh in bsmtp1.DefaultIfEmpty()

                       // VisitorTeam
                       join od in com.OddsInfo on et.ExpectTargetID equals od.ExpectTargetID into tmp2
                       from odv in tmp2.DefaultIfEmpty()
                       join bsm in com.BetSelectMaster on odv.BetSelectMasterID equals bsm.BetSelectMasterID into bsmtp2
                       from bsmv in bsmtp2.DefaultIfEmpty()

                       // Draw
                       join od in com.OddsInfo on et.ExpectTargetID equals od.ExpectTargetID into tmp3
                       from odd in tmp3.DefaultIfEmpty()
                       join bsm in com.BetSelectMaster on odd.BetSelectMasterID equals bsm.BetSelectMasterID into bsmtp3
                       from bsmd in bsmtp3.DefaultIfEmpty()

                       where (et.SportsID == sportsId && et.ClassClass == 4 && et.UniqueID == gameId)
                               && (odh == null || bsmh == null || bsmh.BetSelectID == 1 && odh.ClassificationType == 2)
                               && (odv == null || bsmv == null || bsmv.BetSelectID == 2 && odv.ClassificationType == 2)
                               && (odd == null || bsmd == null || bsmd.BetSelectID == 3 && odd.ClassificationType == 2)

                       select new GameOddsInfoModel
                       {
                           ExpectTargetID = et.ExpectTargetID,
                           HomeTeamOdds = (odh.Odds == null ? 0 : odh.Odds),
                           VisitorTeamOdds = (odv == null ? 0 : odv.Odds),
                           DrawOdds = (odd == null ? 0 : odd.Odds),
                           BetSelectedID = 0,
                       }).FirstOrDefault();

            if (oddInfo != null)
            {
                oddInfo.BetSelectedID = memberId > 0 ? betSelectedId : null;
            }

            if (oddInfo == null)
            {
                //Create new class to store 3 odds with value 0.
                oddInfo = new GameOddsInfoModel
                {
                    ExpectTargetID = 0,
                    HomeTeamOdds = 0,
                    VisitorTeamOdds = 0,
                    DrawOdds = 0,
                    BetSelectedID = null
                };
            }

            return oddInfo;
        }
    }
}
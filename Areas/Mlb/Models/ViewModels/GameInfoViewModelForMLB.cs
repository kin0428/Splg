using Splg.Models.Game.InfoModel;
using Splg.Models.Game.ViewModel;

namespace Splg.Areas.Mlb.Models.ViewModel
{
    public class GameInfoViewModelForMLB : GameInfoViewModel
    {
        public PlayerInfoInGame PreStartingPitcher { get; set; }

        public PlayerInfoInGame StartingPitcher { get; set; }
        #region 先発投手などの情報
        public string PreForeRunnerNameSH
        {
            get
            {
                var result = PreStartingPitcher != null ? PreStartingPitcher.HomeStartingName : "";
                return result;
            }
        }
        public string PreForeRunnerNameSV
        {
            get
            {
                var result = PreStartingPitcher != null ? PreStartingPitcher.VisitorStartingName : "";
                return result;
            }
        }
        public string ForeRunnerNameSH
        {
            get
            {
                var result = StartingPitcher != null ? StartingPitcher.HomeStartingName : "";
                return result;
            }
        }
        public string ForeRunnerNameSV
        {
            get
            {
                var result = StartingPitcher != null ? StartingPitcher.VisitorStartingName : "";
                return result;
            }
        }
        #endregion

        // todo 整理
        public GameInfoModel GameInfoModel { get; set; }

        public int GameStatus { get; set; }

        public GameOddsInfoModel GameOddsInfoModel { get; set; }

        public ParameterInfoModel ParameterInfo { get; set; }

        public class ParameterInfoModel
        {
            public int? ParameterType { get; set; }
            public int? Link { get; set; }
            public int? GameDate { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public int? TeamId { get; set; }
            public int? GameId { get; set; }
            public string LstGameId { get; set; }
        }

    }
}
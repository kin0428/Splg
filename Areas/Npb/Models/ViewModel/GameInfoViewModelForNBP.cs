using Splg.Models.Game.InfoModel;
using Splg.Models.Game.ViewModel;

namespace Splg.Areas.Npb.Models.ViewModel
{
    public class GameInfoViewModelForNBP : GameInfoViewModel
    {
        public PlayerInfoInGame PreStartingPitcherH { get; set; }
        public PlayerInfoInGame PreStartingPitcherV { get; set; }
        public PlayerInfoInGame WinLosePitcherH { get; set; }
        public PlayerInfoInGame WinLosePitcherV { get; set; }

        public PlayerInfoInGame HomePlayerInfoStarting { get; set; }
        public PlayerInfoInGame VisitorPlayerInfoStarting { get; set; }

        #region 先発投手などの情報

        public string PreForeRunnerNameSH
        {
            get
            {
                var result = PreStartingPitcherH != null ? PreStartingPitcherH.PlayerNameS : "";
                return result;
            }
        }

        public string PreForeRunnerEraH
        {
            get
            {
                var result = PreStartingPitcherH != null ? PreStartingPitcherH.PlayerEra : "";
                return result;
            }
        }

        public string PreForeRunnerNameSV
        {
            get
            {
                var result = PreStartingPitcherV != null ? PreStartingPitcherV.PlayerNameS : "";
                return result;
            }
        }

        public string PreForeRunnerEraV
        {
            get
            {
                var result = PreStartingPitcherV != null ? PreStartingPitcherV.PlayerEra : "";
                return result;
            }
        }

        public string ForeRunnerNameSH
        {
            get
            {
                var result = HomePlayerInfoStarting != null ? HomePlayerInfoStarting.PlayerNameS : "";
                return result;
            }
        }

        public string ForeRunnerEraH
        {
            get
            {
                var result = HomePlayerInfoStarting != null ? HomePlayerInfoStarting.PlayerEra : "";
                return result;
            }
        }

        public string ForeRunnerNameSV
        {
            get
            {
                var result = VisitorPlayerInfoStarting != null ? VisitorPlayerInfoStarting.PlayerNameS : "";
                return result;
            }
        }

        public string ForeRunnerEraV
        {
            get
            {
                var result = VisitorPlayerInfoStarting != null ? VisitorPlayerInfoStarting.PlayerEra : "";
                return result;
            }
        }

        public string WinLoseNameSH
        {
            get
            {
                var result = WinLosePitcherH != null ? WinLosePitcherH.PlayerNameS : "";
                return result;
            }
        }

        public string WinLoseEraH
        {
            get
            {
                var result = WinLosePitcherH != null ? WinLosePitcherH.PlayerEra : "";
                return result;
            }
        }

        public string WinLoseNameSV
        {
            get
            {
                var result = WinLosePitcherV != null ? WinLosePitcherV.PlayerNameS : "";
                return result;
            }
        }

        public string WinLoseEraV
        {
            get
            {
                var result = WinLosePitcherV != null ? WinLosePitcherV.PlayerEra : "";
                return result;
            }
        }

        public bool IsWinH
        {
            get
            {
                var result = WinLosePitcherH != null ? WinLosePitcherH.IsWIN : true;
                return result;
            }
        }

        public bool IsWinV
        {
            get
            {
                var result = WinLosePitcherV != null ? WinLosePitcherV.IsWIN : true;
                return result;
            }
        }

        public string WinLoseTextH
        {
            get
            {
                string result = "";
                if (IsWinH)
                {
                    if (IsWinV)
                    {
                        result = "投手";
                    }
                    else
                    {
                        result = "勝ち投手";
                    }
                }
                else
                {
                    result = "負け投手";
                }

                return result;
            }
        }

        public string WinLoseTextV
        {
            get
            {
                string result = "";
                if (IsWinV)
                {
                    if (IsWinH)
                    {
                        result = "投手";
                    }
                    else
                    {
                        result = "勝ち投手";
                    }
                }
                else
                {
                    result = "負け投手";
                }
                return result;
            }
        }

        #endregion

        // todo 整理
        public GameInfoModel GameInfoModel { get; set; }

        public GameOddsInfoModel GameOddsInfoModel { get; set; }

        public ParameterInfoModel ParameterInfo { get; set; }

        public int GameStatus { get; set; }

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

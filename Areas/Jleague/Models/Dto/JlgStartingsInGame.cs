using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Splg.Areas.Jleague.Models.ViewModel.InfosModel;
using Splg.Areas.Jleague.Models.Dto;
using Splg.Core.Constant;

namespace Splg.Areas.Jleague.Models.Dto
{
    /// <summary>
    /// テキスト速報 Dto
    /// </summary>
    public class JlgMembersInGame : JlgPlayerGameInfoModel
    {
        /// <summary>
        /// 交代情報
        /// </summary>
        public List<JlgPlayerGameDetailInfoModel> CardInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> GoalInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> InInfos { get; set; }
        public List<JlgPlayerGameDetailInfoModel> OutInfos { get; set; }

        /// <summary>
        /// 警告・退場の時間
        /// </summary>
        public string CardTime(int PlayerId)
        {
            string result = "";
            if (CardInfos != null)
            {
                int counter = 1;
                foreach (var w in CardInfos.Where(x => x.PlayerId == PlayerId))
                {
                    if (counter == 1 && w.Divide == 1)
                    {
                        result += "警告 " + ((int)w.Time/45 == 0 ? "前半" + w.Time: "後半" + (w.Time - 45) ) + "分";
                        counter++;
                    }
                    else if (counter == 1 && w.Divide == 2)
                    {
                        result += "退場 " + ((int)w.Time / 45 == 0 ? "前半" + w.Time : "後半" + (w.Time - 45)) + "分";
                        counter++;
                    }
                    else
                    {
                        result += ",退場 " + ((int)w.Time / 45 == 0 ? "前半" + w.Time : "後半" + (w.Time - 45)) + "分";
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 警告・退場のカードのタイプ（イエロー？レッド？イエロー*2?）
        /// </summary>
        public string CssClassCardType(int PlayerId)
        {
            int cardType = 0;
            string result = string.Empty;
            if (CardInfos != null)
            {
                foreach (var w in CardInfos.Where(x => x.PlayerId == PlayerId))
                {
                    cardType += w.Divide + w.SecondF;
                }

                if (cardType == 1)
                {
                    result = JlgConst.CssClassYellowCard;
                }
                else if(cardType == 2)
                {
                    result = JlgConst.CssClassRedCard;
                }
                else if (cardType == 3)
                {
                    result = JlgConst.CssClassStackedRedCard;
                }
            }
            return result;
        }
        /// <summary>
        /// 得点の時間
        /// </summary>
        /// <param name="PlayerId"></param>
        /// <returns></returns>
        public string GoalTime(int PlayerId)
        {
            string result = "";
            if (GoalInfos != null)
            {
                int counter = 1;
                foreach (var w in GoalInfos.Where(x => x.PlayerId == PlayerId))
                {
                    if (counter == 1)
                    {
                        result += ((int)w.Time / 45 == 0 ? "前半" + w.Time : "後半" + (w.Time - 45)) + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + ((int)w.Time / 45 == 0 ? "前半" + w.Time : "後半" + (w.Time - 45)) + "分";
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 途中出場の時間
        /// </summary>
        /// <param name="PlayerId"></param>
        /// <returns></returns>
        public string InTime(int PlayerId)
        {
            string result = "";
            if (InInfos != null)
            {
                int counter = 1;
                foreach (var w in InInfos.Where(x => x.PlayerId == PlayerId))
                {
                    if (counter == 1)
                    {
                        result += ((int)w.Time / 45 == 0 ? "前半" + w.Time : "後半" + (w.Time - 45)) + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + ((int)w.Time / 45 == 0 ? "前半" + w.Time : "後半" + (w.Time - 45)) + "分";
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 途中交代の時間
        /// </summary>
        /// <param name="PlayerId"></param>
        /// <returns></returns>
        public string OutTime(int PlayerId)
        {
            string result = "";
            if (OutInfos != null)
            {
                int counter = 1;
                foreach (var w in OutInfos.Where(x => x.PlayerId == PlayerId))
                {
                    if (counter == 1)
                    {
                        result += ((int)w.Time / 45 == 0 ? "前半" + w.Time : "後半" + (w.Time - 45)) + "分";
                        counter++;
                    }
                    else
                    {
                        result += "," + ((int)w.Time / 45 == 0 ? "前半" + w.Time : "後半" + (w.Time - 45)) + "分";
                    }
                }
            }
            return result;
        }
    }
}
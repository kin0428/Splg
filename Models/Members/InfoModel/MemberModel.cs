using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Splg.Core.Constant;

namespace Splg.Models.Members.InfoModel
{
   
    /// <summary>
    /// 会員規定モデル
    /// </summary>
    public class MemberModel
    {
        /// <summary>
        /// 会員ID
        /// </summary>
        public long MemberId { get; set; }

        private string profileImg;

        /// <summary>
        /// イメージ画像
        /// </summary>
        public string ProfileImg
        {
            get
            {
                string result = "/Content/img/upload/member/DefaultProfilePicture.png";
                if (profileImg != null && !String.IsNullOrEmpty(profileImg.Trim()))
                {
                    result = profileImg;
                }

                return result;
            }
            set { profileImg = value; }
        }

        private string nickName;

        /// <summary>
        /// ニックネーム
        /// </summary>
        public string Nickname
        {
            get { return nickName ?? ""; }
            set { nickName = value; }
        }

        #region ログイン

        /// <summary>
        /// ログイン会員自身の場合true
        /// </summary>
        public bool IsLoginUser { get; set; }

        #endregion

        #region フォロー

        /// <summary>
        /// ログイン会員がフォローしている場合true
        /// </summary>
        public bool IsFollowing { get; set; }

        #endregion

        #region ポイント

        //TODO Models.PointInfo.InfoModel.PointInfoModelを持つようにする

        long pointId;
        /// <summary>
        /// 現在使用しているポイントレコードの
        /// </summary>
        public long PointId
        {
            get { return pointId; }
            set { pointId = value; }
        }


        int pointRank;

        /// <summary>
        /// ポイントランク
        /// </summary>
        public int PointRank
        {
            get { return pointRank; }
            set { pointRank = value; }
        }

        /// <summary>
        /// 原資ポイント
        /// </summary>
        public int FundsPoint { get; set; }

        /// <summary>
        /// 所持ポイント
        /// </summary>
        [DisplayFormat(DataFormatString = AnnotationFormatConst.IsCommaSeparated)]
        public int PossesionPoint { get; set; }

        /// <summary>
        /// 精算済みポイント
        /// </summary>
        public int PayOffPoints { get; set; }

        /// <summary>
        /// 景品に換えられるポイント
        /// TODO nojima 　名称整理したい
        /// </summary>
        public int CurrentPoints
        {
            get
            {
                if (PayOffPoints > 0)
                    return PayOffPoints;

                int result = (PossesionPoint - FundsPoint) < 0 ? 0 : PossesionPoint - FundsPoint;
                return result;
            }
            //get { return PayOffPoints; } //2015/05/19
        }

        /// <summary>
        /// フォーマットされた精算済みポイント
        /// </summary>
        public string FormattedPayOffPoints
        {
            get
            {
                return PayOffPoints.ToString("#,0");
            }
        }

        /// <summary>
        /// フォーマットされた所持ポイント。
        /// TODO nojima 　名称整理したい
        /// </summary>
        public string FormattedDisplayPoints
        {
            get
            {
                return PossesionPoint.ToString("#,0");
            }
        }

        /// <summary>
        /// フォーマットされた景品に換えられるポイント
        /// TODO nojima 　名称整理したい
        /// </summary>
        public string FormattedRankingPoints
        {
            get
            {
                return CurrentPoints.ToString("#,0");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime BetStartDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BetEndDate { get; set; }


        #endregion

        #region 予想

        int? betSelectID;

        public int? BetSelectID
        {
            get { return betSelectID; }
            set { betSelectID = value; }
        }

        public Nullable<System.DateTime> LastExpectedPointDate { get; set; }

        /// <summary>
        /// 最終予想日時
        /// </summary>
        public string FormattedLastExpectedPointDate
        {
            get
            {
                string result = null;
                if (LastExpectedPointDate != null)
                    result = Utils.FormatDateTime(LastExpectedPointDate.Value);
                return result;
            }
        }

        #endregion
    }
}
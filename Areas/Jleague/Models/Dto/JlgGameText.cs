using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Splg.Areas.Jleague.Models.Dto
{
    /// <summary>
    /// テキスト速報 Dto
    /// </summary>
    public class JlgGameText : CommentInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentInfo"></param>
        /// <param name="commentInfos"></param>
        /// <param name="teamDic">Key:チームID Value：チーム名略 のDictionary</param>
        public JlgGameText(CommentInfo commentInfo = null, IEnumerable<CommentInfo> commentInfos = null, Dictionary<int, string> teamDic = null)
        {
            if (commentInfo == null)
            {
                this.DislayHalf = GetDisplayHalf(0, null);
                return;
            }

            this.CommentInfoId = commentInfo.CommentInfoId;
            this.CommentReportId = commentInfo.CommentReportId;
            this.StateID = commentInfo.StateID;
            this.StateName = commentInfo.StateName;
            this.Half = commentInfo.Half;
            this.Comment = commentInfo.Comment;

            this.DislayHalf = this.GetDisplayHalf(this.CommentInfoId, commentInfos);
            
            int? teamId = null;
            string teamNameS = null;
            string displayComment = null;
            this.SplitComment(this.Comment, teamDic, ref teamId, ref teamNameS, ref displayComment);
            this.TeamID = teamId;
            this.TeamNameS = teamNameS;
            this.DisplayComment = displayComment;
        }

        /// <summary>
        /// 試合経過ハーフ時間(表示用)
        /// </summary>
        /// <remarks>
        /// 0～90表記
        /// 例）後半15分⇒'60
        /// </remarks>
        public string DislayHalf { get; private set; }

        /// <summary>
        /// チームID
        /// </summary>
        public int? TeamID { get; set; }

        /// <summary>
        /// チーム名略
        /// </summary>
        public string TeamNameS { get; set; }

        /// <summary>
        /// コメント(表示用)
        /// </summary>
        public string DisplayComment { get; set; }



        /// <summary>
        /// 試合経過ハーフ時間(表示用)の設定
        /// </summary>
        /// <param name="commnetInfoId">コメント情報主キー</param>
        /// <param name="commnetInfos"></param>
        /// <returns></returns>
        public string GetDisplayHalf(int? commnetInfoId, IEnumerable<CommentInfo> commnetInfos)
        {
            if (commnetInfos == null || !commnetInfoId.HasValue) { return string.Empty; }

            var item = commnetInfos.Where(c => c.CommentInfoId == commnetInfoId).FirstOrDefault();
            if (item == null) return string.Empty;

            // 前のStateIDのMAX + Half
            var halfMaxByStateId = commnetInfos.Where(a => a.CommentInfoId != item.CommentInfoId &&
                                               a.CommentReportId == item.CommentReportId &&
                                               a.StateID < item.StateID)   // 1:前半 2:後半 3:延長前半 4:延長後半 7:PK戦
                                    .GroupBy(b => b.StateID)
                                    .Select(c => new { StateId = c.Key, MaxHalf = c.Max(d => d.Half) })
                                    .Sum(e => e.MaxHalf);

            return string.Format("'{0}", item.Half.Value + halfMaxByStateId);
        }

        /// <summary>
        /// コメントとチーム名を分割
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="teamDic">Key:チームID, Value：チーム名略 のDictionary</param>
        /// <param name="teamId"></param>
        /// <param name="teamName"></param>
        /// <param name="displayComment"></param>
        public void SplitComment(string comment, Dictionary<int, string> teamDic, ref int? teamId, ref string teamName, ref string displayComment)
        {
            if (string.IsNullOrEmpty(comment) || teamDic == null) return;

            foreach (var t in teamDic)
            {
                // (チーム名)(スペース)(コメント内容)
                var pattern = string.Format(@"^(?<teamname>{0})\s+(?<comment>.*)$", t.Value);
                var regex = new Regex(pattern);
                var match = regex.Match(comment);
                if (match.Success)
                {
                    teamId = t.Key;
                    teamName = t.Value;
                    displayComment = match.Groups["comment"].Value;
                    break;
                }
            }

            if (displayComment == null)
            {
                displayComment = comment;
            }
        }

    }
}
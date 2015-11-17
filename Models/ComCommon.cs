using Splg.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace Splg.Models
{
    public static class ComCommon
    {
        /// <summary>
        /// Get AccessKey in table SnsCorpInformation
        /// </summary>
        /// <param name="socialType"></param>
        /// <returns>accessKey</returns>
        public static string GetAccessKey(string socialType)
        {
            string accessKey = string.Empty;
            ComEntities com = new ComEntities();
            accessKey = com.SnsCorpInformation.SingleOrDefault(sns => sns.SnsName.Equals(socialType)).AccessKey;
            return accessKey;
        }

        /// <summary>
        ///  Get SecretKey in table SnsCorpInformation
        /// </summary>
        /// <param name="socialType"></param>
        /// <returns></returns>
        public static string GetSecretKey(string socialType)
        {
            string secretKey = string.Empty;
            ComEntities com = new ComEntities();
            secretKey = com.SnsCorpInformation.SingleOrDefault(sns => sns.SnsName.Equals(socialType)).SecretKey;
            return secretKey;
        }

        /// <summary>
        /// Check email member logion exits
        /// </summary>
        /// <param name="email"></param>
        /// <returns>bool true or false</returns>
        public static bool CheckEmailAvailability(string email)
        {
            bool result = false;
            ComEntities com = new ComEntities();
            var data = com.Member.Where(p => p.Mail == email && (p.Status == 0 || p.Status == 1)).ToList();
            if (data.Count > 0)
            {
                result = true;
            }
            return result;
        }

        public static Member GetMemberLogin(string email)
        {
            ComEntities com = new ComEntities();
            var member = com.Member.SingleOrDefault(u => u.Mail.Equals(email));
            return member;
        }

        /// <summary>
        /// get object member when user login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static Member GetMemberLogin(string email, string pass)
        {
            ComEntities com = new ComEntities();
            string passHash = Utils.MD5Hash(pass);
            var member = com.Member.SingleOrDefault(u => u.Mail.Equals(email) && u.Password.Equals(passHash) && u.Status == 1);
            return member;
        }


        /// <summary>
        /// get object member when user login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static Member GetMemberByMemberId(Int64 memberId)
        {
            ComEntities com = new ComEntities();
            return com.Member.SingleOrDefault(u => u.MemberId.Equals(memberId));
        }

        /// <summary>
        /// get object member when user login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static Int64 GetMemberIDInSnsAuthInfo(string snsMemberID)
        {

            Int64 result = 0;
            ComEntities com = new ComEntities();
            var snsAuthInformation= com.SnsAuthInformation.SingleOrDefault(u => u.SnsMemberID.Equals(snsMemberID));
            if (snsAuthInformation !=null)
            {
                result = snsAuthInformation.MemberID;
            }
            return result;
        }


        /// <summary>
        /// Check email member logion exits
        /// </summary>
        /// <param name="email"></param>
        /// <returns>bool true or false</returns>
        public static bool CheckEmailForgotPass(string email)
        {
            bool result = false;
            ComEntities com = new ComEntities();
            var data = com.Member.Where(p => p.Mail == email && p.Status == 1).ToList();
            if (data.Count > 0)
            {
                result = true;
            }
            return result;
        }
    }
}



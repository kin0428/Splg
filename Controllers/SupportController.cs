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
 * Namespace	: Splg.Controllers
 * Class		: SupportController
 * Developer	: Nguyen Ho Long Hai
 * 
 */
#endregion

#region Using directives
using Splg.Models;
using Splg.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
#endregion

namespace Splg.Controllers
{
    public class SupportController : Controller
    {
        #region Global Properties
        //Create context to get value from db.         
        ComEntities com = new ComEntities();
        #endregion

        /// <summary>
        /// show view contact 
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ContactViewModel contactViewModel = new ContactViewModel();
            if(Session["CurrentUser"]!=null)
            {
                Int64 memberId = Convert.ToInt64(Session["CurrentUser"]);
                var objMember = (from member in com.Member
                                 where member.MemberId == memberId
                                 select member).FirstOrDefault();
                contactViewModel.EmailTo = (Session["CurrentUser"] != null ? objMember.Mail : contactViewModel.EmailTo);
                contactViewModel.DisplayMemberId = objMember.DisplayMemberId;
                contactViewModel.Nickname = objMember.Nickname;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Request.Url.AbsoluteUri);
                contactViewModel.UserLoad = request.UserAgent;
            }
            return View(contactViewModel);
        }


        /// <summary>
        /// send email when form submit
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Contact(ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                string fileName = "ContactTemp.html";
                string emailTo = contactViewModel.EmailTo;
                string title = contactViewModel.Title;
                Dictionary<string, string> dicContent = new Dictionary<string, string>();
                dicContent.Add("[content]", contactViewModel.Content);
                dicContent.Add("[DisplayMemberId]", contactViewModel.DisplayMemberId);
                dicContent.Add("[Nickname]", contactViewModel.Nickname);
                dicContent.Add("[UserLoad]", contactViewModel.UserLoad);
                string splologEmail = ConfigurationManager.AppSettings["SplologEmail"];
                EmailSender emailHelper = new EmailSender();
                string result = emailHelper.SendEmail(emailTo, fileName, title, dicContent, emailBcc: splologEmail);
                if (result.Equals(Constants.EMAIL_SEND))
                {
                    ViewBag.Contact = Constants.EMAIL_SEND;
                }
            }
            return View();

        }

        public ActionResult Rules()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult Howto()
        {
            return View();
        }

        public ActionResult Notice()
        {
            return View();
        }

    }
}
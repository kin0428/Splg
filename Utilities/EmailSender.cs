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
 * Namespace	: Splg
 * Class		: EmailSender
 * Developer	: Nguyen Ho Long Hai
 * 
 */
#endregion
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Splg
{
    public class EmailSender
    {
        private string NotSentRecipients = string.Empty;

        /// <summary>
        /// send email common
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="fileName"></param>
        /// <param name="tile"></param>
        /// <param name="dicContent"></param>
        public string SendEmail(string emailTo, string fileName, string tile, Dictionary<string, string> dicContent, string emailBcc = default(string))
        {
            try
            {
                string emailFrom = ConfigurationManager.AppSettings["Email"];
                string emailPass = ConfigurationManager.AppSettings["EmailPass"];
                Int32 emailPort = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"]);
                string smtpClient = ConfigurationManager.AppSettings["SmtpClient"];

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailFrom);
                mailMessage.To.Add(emailTo);
                if (!string.IsNullOrEmpty(emailBcc))
                {
                    mailMessage.Bcc.Add(emailBcc);
                }
                var tmpEmail = System.Web.Hosting.HostingEnvironment.MapPath("~/MailTemplate/" + fileName);

                StreamReader reader = new StreamReader(tmpEmail);
                string readFile = reader.ReadToEnd();
                string content = string.Empty;
                content = readFile;
                //content = readFile.Replace("\r\n", "<br>");
                //content = readFile.Replace("\r\n", "");
                foreach (var item in dicContent)
                {
                    content = content.Replace(item.Key, item.Value);
                }
                mailMessage.Subject = tile;
                mailMessage.Body = content;
                //mailMessage.IsBodyHtml = true;
                //mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.SubjectEncoding = Encoding.UTF8;

                mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("iso-2022-jp");
                //mailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding("iso-2022-jp");

                mailMessage.BodyTransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                // Configure the client:
                SmtpClient client = new SmtpClient(smtpClient);
                client.Host = smtpClient;
                client.Port = emailPort;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                if (client.Host == "smtp.gmail.com")
                {
                    client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    // Create the credentials:
                    NetworkCredential credentials = new NetworkCredential(emailFrom, emailPass);
                    client.EnableSsl = true;
                    client.Credentials = credentials;
                }
                client.Send(mailMessage);
                mailMessage.Dispose();
                client.Dispose();
                return Constants.EMAIL_SEND;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (Exception innerEx in ex.InnerExceptions)
                {
                    SmtpFailedRecipientException failedRecipEx = innerEx as SmtpFailedRecipientException;
                    if (failedRecipEx != null)
                    {
                        NotSentRecipients += (failedRecipEx.FailedRecipient.Replace("<", "").Replace(">", ""));
                    }
                }
                return "SmtpFailedRecipientsException";
            }
            catch (SmtpFailedRecipientException ex)
            {
                NotSentRecipients += ex.FailedRecipient.Replace("<", "").Replace(">", "");
                return "SmtpFailedRecipientException";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }



        public string SendEmail2(string emailTo, string body, string tile, Dictionary<string, string> dicContent)
        {
            try
            {
                string emailFrom = ConfigurationManager.AppSettings["Email"];
                string emailPass = ConfigurationManager.AppSettings["EmailPass"];
                Int32 emailPort = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"]);
                string smtpClient = ConfigurationManager.AppSettings["SmtpClient"];

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailFrom);
                mailMessage.To.Add(emailTo);

                string content = string.Empty;
                content = body;
                //content = readFile.Replace("\r\n", "<br>");
                //content = readFile.Replace("\r\n", "");

                if (dicContent != null)
                {
                    foreach (var item in dicContent)
                    {
                        content = content.Replace(item.Key, item.Value);
                    }
                }
                mailMessage.Subject = tile;
                mailMessage.Body = content;
                //mailMessage.IsBodyHtml = true;
                //mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.SubjectEncoding = Encoding.UTF8;

                mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("iso-2022-jp");
                //mailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding("iso-2022-jp");

                mailMessage.BodyTransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                // Configure the client:
                SmtpClient client = new SmtpClient(smtpClient);
                client.Host = smtpClient;
                client.Port = emailPort;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                if (client.Host == "smtp.gmail.com")
                {
                    client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    // Create the credentials:
                    NetworkCredential credentials = new NetworkCredential(emailFrom, emailPass);
                    client.EnableSsl = true;
                    client.Credentials = credentials;
                }
                client.Send(mailMessage);
                mailMessage.Dispose();
                client.Dispose();
                return Constants.EMAIL_SEND;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (Exception innerEx in ex.InnerExceptions)
                {
                    SmtpFailedRecipientException failedRecipEx = innerEx as SmtpFailedRecipientException;
                    if (failedRecipEx != null)
                    {
                        NotSentRecipients += (failedRecipEx.FailedRecipient.Replace("<", "").Replace(">", ""));
                    }
                }
                return "SmtpFailedRecipientsException";
            }
            catch (SmtpFailedRecipientException ex)
            {
                NotSentRecipients += ex.FailedRecipient.Replace("<", "").Replace(">", "");
                return "SmtpFailedRecipientException";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
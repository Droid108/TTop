using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace TTServices.Utilities
{
    public static class ExceptionHandler
    {

        private static string GetUser_IP()
        {
            string VisitorsIPAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            }
            return VisitorsIPAddr;
        }

        public static void SendExceptionEmail(Exception ex, string ErrorLocation, string UserName, string url)
        {
            bool isEmailRequired = false;
            isEmailRequired = Convert.ToBoolean(ConfigurationManager.AppSettings["SendErrorEmail"]);
            if (isEmailRequired)
            {
                string errorMessage = string.Format("User: {0}\r\nURL: {1}\r\n=====================\r\n{2}", UserName, url, AddExceptionText(ex));
                const string SERVER = "relay-hosting.secureserver.net";
                MailMessage oMail = new MailMessage();
                oMail.From = new MailAddress(ConfigurationManager.AppSettings["ErrorFromEmailAddress"].ToString());
                oMail.To.Add(ConfigurationManager.AppSettings["ErrorToEmailAddress"].ToString());
                oMail.Subject = "Error in application at " + url + "- IP:" + GetUser_IP();
                oMail.IsBodyHtml = true;
                oMail.Priority = MailPriority.High;	// enumeration
                oMail.Body = errorMessage;
                //SmtpMail.SmtpServer = SERVER;
                //SmtpMail.Send(oMail);
                SmtpClient client = new SmtpClient(SERVER);
                client.Send(oMail);
                oMail = null;	// free up resources
            }
        }

        public static string AddExceptionText(Exception ex)
        {
            string innermessage = string.Empty;
            if (ex.InnerException != null)
            {
                innermessage = string.Format("=======InnerException====== \r\n{0}", ExceptionHandler.AddExceptionText(ex.InnerException));
            }
            string message = string.Format("Message: {0}\r\nSource: {1}\r\nStack:\r\n{2}\r\n\r\n{3}", ex.Message, ex.Source, ex.StackTrace, innermessage);
            return message;
        }

    }
}
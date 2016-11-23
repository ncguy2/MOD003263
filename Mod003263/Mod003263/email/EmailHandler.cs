
/**
 * Author: Nick Guy
 * Date: 23/11/2016
 * Contains: EmailHandler
 */

using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;

namespace Mod003263.email {

    /// <summary>
    /// Singleton to handle email sending
    /// </summary>
    public class EmailHandler {

        private static EmailHandler instance;
        public static EmailHandler GetInstance() {
            return instance ?? (instance = new EmailHandler());
        }

        private readonly SmtpClient smtp;

        private EmailHandler() {
            smtp = new SmtpClient("smtp.gmail.com") {
                Port = 465,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("ssmithtech60@gmail.com", "HappyTech")
            };
            smtp.SendCompleted += Post;
        }

        public void Send(string from, string recipients, string subject, string body) {
            smtp.Send(from, recipients, subject, body);
        }

        public void Post(object obj, AsyncCompletedEventArgs args) {
            if(obj == null) obj = "";
            Console.WriteLine(obj.ToString()+" Complete");
        }

    }
}
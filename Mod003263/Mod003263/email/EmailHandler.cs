﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows;
using Mod003263.events.email;
using Mod003263.wpf;

/**
 * Author: Nick Guy
 * Date: 23/11/2016
 * Contains: EmailHandler
 */
namespace Mod003263.email {

    /// <summary>
    /// Singleton to handle email sending
    /// <br/>
    /// TODO Fix this shit
    /// </summary>
    public class EmailHandler {

        private static EmailHandler instance;
        public static EmailHandler GetInstance() {
            return instance ?? (instance = new EmailHandler());
        }

        private readonly SmtpClient smtp;

        private EmailHandler() {

            string user = PropertiesManager.GetInstance().GetPropertyOrDefault("email.username", "");
            string pass = PropertiesManager.GetInstance().GetPropertyOrDefault("email.password", "");

            smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(user, pass);

            smtp.SendCompleted += (sender, args) => {
                new EmailEvent().Fire();
            };
        }

        public void Send(string from, string recipients, string subject, string body) {
            try {
                MailMessage msg = new MailMessage(from, recipients);
                msg.Subject = subject;
                msg.Body = body;
                smtp.Send(msg);
            }catch (Exception e) {
                Application.Current.Dispatcher.Invoke(() => WPFMessageBoxFactory.CreateErrorAndShow(e));
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TestProject.Global.Config;

namespace TestProject.Tools.Mail
{
    public static class MailSender
    {
        private static IConfig _config;

        public static IConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = (DependencyResolver.Current).GetService<IConfig>();

                }
                return _config;
            }
        }
        
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void SendMail(string email, string subject, string body, MailAddress mailAddress = null)
        {

            try
            {
                
                if (Config.EnableMail)
                {
                    if (mailAddress == null)
                    {
                        mailAddress = new MailAddress(Config.MailSetting.SmtpReply, Config.MailSetting.SmtpUser);
                    }
                    MailMessage message = new MailMessage(
                        mailAddress,
                        new MailAddress(email))
                    {
                        Subject = subject,
                        BodyEncoding = Encoding.UTF8,
                        Body = body,
                        IsBodyHtml = true,
                        SubjectEncoding = Encoding.UTF8
                    };
                    SmtpClient client = new SmtpClient
                    {
                        Host = Config.MailSetting.SmtpServer,
                        Port = Config.MailSetting.SmtpPort,
                        UseDefaultCredentials = false,
                        EnableSsl = Config.MailSetting.EnableSsl,
                        Credentials =
                            new NetworkCredential(Config.MailSetting.SmtpUserName,
                                                  Config.MailSetting.SmtpPassword),
                        DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                        PickupDirectoryLocation = "C:/test"
                    };
                    client.Send(message);
                }
                else
                {
                    logger.Debug("Email : {0} {1} \t Subject: {2} {3} Body: {4}", email, Environment.NewLine, subject, Environment.NewLine, body);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Mail send exception", ex.Message);
                
            }
        }
    }
}
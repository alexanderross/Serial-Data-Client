using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace CCTVClient.Alerts
{
    public class SMSAlert
    {
        private String SMTPLabel;
        private bool SMTPSSL;

        private SmtpClient MailClient; 

        public SMSAlert(String smtpServer, int smtpPort,String smtpLabel)
        {
            MailClient = new SmtpClient(smtpServer, smtpPort);
            MailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SMTPLabel = smtpLabel;
        }

        public SMSAlert(String smtpServer, int smtpPort, String smtpLogin, String smtpPassword, Boolean useSSL, String smtpLabel)
        {
            MailClient = new SmtpClient(smtpServer, smtpPort);
            MailClient.EnableSsl = useSSL;
            MailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailClient.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
            SMTPLabel = smtpLabel;
        }

        public void SendMessage(String message,String destination)
        {
            MailMessage Broadcast = new MailMessage();
            Broadcast.To.Add(destination);

            Broadcast.From = new MailAddress("mailer@oneseventyfour.com",SMTPLabel); //See the note afterwards...
            Broadcast.Body = message;
            try
            {
                MailClient.Send(Broadcast);

            }
            catch (Exception ex)
            {
                Console.WriteLine("SendMessageToSubscribed@SMSAlert:"+ex.Message);
            }
        }
    }
}

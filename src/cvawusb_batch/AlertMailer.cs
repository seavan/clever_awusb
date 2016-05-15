using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Security.AccessControl;

namespace cvawusb_batch
{
    public class AlertMailer
    {
        public static void Send(string[] text)
        {
            var host = ConfigurationSettings.AppSettings["mail_host"];
            var user = ConfigurationSettings.AppSettings["mail_user"];
            var pass = ConfigurationSettings.AppSettings["mail_pass"];
            var subject = ConfigurationSettings.AppSettings["mail_title"];
            var to = ConfigurationSettings.AppSettings["mail_receivers"];
            MailMessage mail = new MailMessage(user, to);
            
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(user, pass);
            client.Host = host;
            mail.Subject = subject;
            mail.Body = string.Join("", text);
            client.Send(mail);
        }
    }
}
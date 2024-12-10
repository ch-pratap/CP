using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSiaService
{
    public partial class Form1 : Form
    {

        AppLogger appLogger = null;
        public Form1()
        {
            appLogger = new AppLogger(this.GetType().FullName);
            InitializeComponent();
        }

        private void SiaTimer_Tick(object sender, EventArgs e)
        {
            CheckSiaService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.SiaTimer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["JOB_INTERVAL"]);
            this.SiaTimer.Enabled = true;
            CheckSiaService();
        }

        public void CheckSiaService()
        {

            string url = "";

            url = "http://localhost:9980/";
            string responseFromServer = "";
            try
            {

                WebRequest request = WebRequest.Create(url);


                request.Method = "GET";
                ((HttpWebRequest)request).UserAgent = "Sia-Agent";

                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                //StreamReader reader = new StreamReader(dataStream);
                //// Read the content.  
                //responseFromServer = reader.ReadToEnd();

                StreamReader streamReader = new StreamReader(dataStream, Encoding.Default);
                StringBuilder responseString = new StringBuilder();
                char[] buffer = new char[1024];
                int charRead = 0;
                while ((charRead = streamReader.ReadBlock(buffer, 0, 1024)) > 0)
                {
                    responseString.Append(buffer);
                    buffer = null;
                    buffer = new char[1024];
                }

                appLogger.Info("Response 1");
                // Read the content.  
                responseFromServer = responseString.ToString().Replace("\0", "").Trim();

                
               

            }
            catch (WebException ex)
            {
                appLogger.Error(ex,"Web Exception");
                SendEmail("cps.1974@gmail.com", "Sia Not Working", "Sia is down. Pls start Sia again");
            }

            catch (Exception ex)
            {
               
                appLogger.Error(ex, " Error Testing : ");
            }
            
        }
        public void SendEmail(string toMail, string subject, string Message)
        {
            try
            {
                string smtp_host = ConfigurationManager.AppSettings["SMTP_HOST"];
                string mail_user = ConfigurationManager.AppSettings["MAIL_USER"];
                string mail_pwd = ConfigurationManager.AppSettings["MAIL_PWD"];
                string smtp_port = ConfigurationManager.AppSettings["SMTP_PORT"];
                var client = new SmtpClient(smtp_host, Convert.ToInt32(smtp_port))
                {
                    Credentials = new System.Net.NetworkCredential(mail_user, mail_pwd),
                    EnableSsl = true
                };
                MailAddress fromAddress = new MailAddress(mail_user);
                MailAddress toAddress = new MailAddress(toMail);


                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage(fromAddress, toAddress);

                mailMessage.Subject = subject;
                mailMessage.Body = Message;
               
                mailMessage.IsBodyHtml = false;


                client.Send(mailMessage);



                mailMessage.Dispose();
                mailMessage = null;
                fromAddress = null;
                toAddress = null;

                client.Dispose();
                client = null;
            }
            catch(Exception ex)
            {

            }
            
           


        }
    }
}

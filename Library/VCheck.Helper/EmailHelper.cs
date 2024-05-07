using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;

namespace VCheck.Helper
{
    public class EmailHelper
    {
        public static bool SendEmail(EmailObject objEmail, out string sErrorMessage)
        {
            sErrorMessage = "";

            bool boolStatus = false;
            string recipeintEmail = string.Empty;
            string CcrecipeintEmail = string.Empty;
            string BccrecipeintEmail = string.Empty;

            try
            {
                MailAddress FromAddress = new MailAddress(objEmail.SenderEmail, objEmail.DisplayName);
                MailMessage myMessage = new MailMessage();

                myMessage.From = FromAddress;

                //Recipient
                if (objEmail.RecipientEmail != null)
                {
                    for (int i = 0; i <= objEmail.RecipientEmail.Count - 1; i++)
                    {
                        recipeintEmail += objEmail.RecipientEmail[i].ToString() + ",";
                    }

                    if (recipeintEmail.EndsWith(","))
                    {
                        recipeintEmail = recipeintEmail.Substring(0, recipeintEmail.Length - 1);
                    }
                }

                //CC 
                if (objEmail.CCRecipientEmail != null)
                {
                    for (int i = 0; i <= objEmail.CCRecipientEmail.Count - 1; i++)
                    {
                        CcrecipeintEmail += objEmail.CCRecipientEmail[i].ToString() + ",";
                    }

                    if (CcrecipeintEmail.EndsWith(","))
                    {
                        CcrecipeintEmail = CcrecipeintEmail.Substring(0, CcrecipeintEmail.Length - 1);
                    }
                }

                //BCC 
                if (objEmail.BCCRecipientEmail != null)
                {
                    for (int i = 0; i <= objEmail.BCCRecipientEmail.Count - 1; i++)
                    {
                        BccrecipeintEmail += objEmail.BCCRecipientEmail[i].ToString() + ",";
                    }

                    if (BccrecipeintEmail.EndsWith(","))
                    {
                        BccrecipeintEmail = BccrecipeintEmail.Substring(0, BccrecipeintEmail.Length - 1);
                    }
                }

                if (recipeintEmail.Length > 0)
                {
                    myMessage.To.Add(recipeintEmail);
                }
                if (CcrecipeintEmail.Length > 0)
                {
                    myMessage.CC.Add(CcrecipeintEmail);
                }
                if (BccrecipeintEmail.Length > 0)
                {
                    myMessage.Bcc.Add(BccrecipeintEmail);
                }

                if (objEmail.EmailAttachement != null && objEmail.EmailAttachement.Count > 0)
                {
                    foreach (Attachment e in objEmail.EmailAttachement)
                    {
                        myMessage.Attachments.Add(e);
                    }
                }

                myMessage.Subject = objEmail.Subject;
                myMessage.Body = objEmail.Body;
                myMessage.From = new MailAddress(objEmail.SenderEmail, objEmail.DisplayName);
                myMessage.IsBodyHtml = objEmail.IsHtml;

                SmtpClient mySmtpClient = new SmtpClient()
                {
                    EnableSsl = objEmail.EnableSsl,
                    UseDefaultCredentials = objEmail.UseDefaultCredentials,
                    Host = objEmail.SMTPHost,
                    Credentials = new NetworkCredential(objEmail.HostUsername, objEmail.HostPassword),
                    Port = objEmail.PortNo,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                mySmtpClient.Timeout = 30000;
                mySmtpClient.Send(myMessage);

                boolStatus = true;
            }
            catch (Exception ex)
            {
                sErrorMessage = ex.ToString();
                boolStatus = false;
            }

            return boolStatus;
        }

        public static string GetTextStringFromFile(string sPath)
        {
            FileInfo fi = new FileInfo(sPath);
            FileStream fs = fi.OpenRead();
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            return System.Text.ASCIIEncoding.ASCII.GetString(buffer);
        }
    }

    public class EmailObject
    {
        #region Properties
        public string SenderEmail
        {
            get;
            set;
        }
        public List<string> RecipientEmail
        {
            get;
            set;
        }
        public List<string> CCRecipientEmail
        {
            get;
            set;
        }
        public List<string> BCCRecipientEmail
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Body
        {
            get;
            set;
        }
        public string TemplatePath
        {
            get;
            set;
        }
        public bool IsHtml
        {
            get;
            set;
        }
        public string DisplayName
        {
            get;
            set;
        }
        public List<Attachment> EmailAttachement
        {
            get;
            set;
        }
        public string SMTPHost
        {
            get;
            set;
        }
        public int PortNo
        {
            get; 
            set;
        }
        public string HostUsername
        {
            get;
            set;
        }
        public string HostPassword
        {
            get;
            set;
        }
        public bool EnableSsl
        {
            get;
            set;
        }
        public bool UseDefaultCredentials
        {
            get;
            set;
        }
        #endregion
    }
}

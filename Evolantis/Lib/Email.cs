using System;
using System.Net.Mail;

namespace Evolantis.Lib
{
    public class Email
    {
        public static bool Send(string from, string subject, string body, string[] to, string[] cc = null, string[] bcc = null, bool IsBodyHtml = true) {
            bool res = false;
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            try
            {
                mail.From = new MailAddress(from);
                if (to != null && to.Length > 0)
                {
                    foreach (string t in to)
                    {
                        mail.To.Add(t.ToString());
                    }
                }

                if (cc != null && cc.Length > 0)
                {
                    foreach (string c in cc)
                    {
                        mail.CC.Add(c.ToString());
                    }
                }

                if (bcc != null && bcc.Length > 0)
                {
                    foreach (string bc in bcc)
                    {
                        mail.Bcc.Add(bc.ToString());
                    }
                }

                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = IsBodyHtml;
                smtp.Send(mail);
                res = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return res;
        }
    }
}

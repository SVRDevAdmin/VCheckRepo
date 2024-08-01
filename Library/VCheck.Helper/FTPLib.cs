using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Helper
{
    public class FTPLib
    {
        /// <summary>
        /// Upload File function for FTP
        /// Notes: FTP Server URL in the format 'ftp://xxx.xxx.xx.xx/'
        /// </summary>
        /// <param name="sReadFile"></param>
        /// <param name="sFTPServerUrl"></param>
        /// <param name="sFTPUsername"></param>
        /// <param name="sFTPPassword"></param>
        /// <returns></returns>
        public static FtpStatusCode UploadFile(String sReadFile, String sFTPServerUrl, String sFTPUsername, String sFTPPassword)
        {
            try
            {
                var ftpServerUrl = sFTPServerUrl;
                var username = @sFTPUsername;
                var password = sFTPPassword;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpServerUrl + System.IO.Path.GetFileName(sReadFile)));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.KeepAlive = false;
                request.EnableSsl = false;

                byte[] fileContents;
                fileContents = System.IO.File.ReadAllBytes(sReadFile); ;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //return FtpStatusCode.Undefined;
            }
        }
    }
}

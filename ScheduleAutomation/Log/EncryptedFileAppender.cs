using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleAutomation.Log
{
    public class EncryptedFileAppender : AppenderSkeleton
    {
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("xN3JufknTsU+ml2b");
        private const string EncryptionKey = "Retes@123";

        private string _file;
        public string file
        {
            get { return _file; }
            set { _file = value; }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            string filename = loggingEvent.Level.ToString() == "ERROR" ? "DailyErrorLog_" : "GeneralLog_";
            string formattedMessage = $"{loggingEvent.TimeStamp:yyyy-MM-dd HH:mm:ss} {loggingEvent.Level} [{loggingEvent.LoggerName}] {loggingEvent.LocationInformation.MethodName} [{loggingEvent.LocationInformation.LineNumber}] - MESSAGE: {loggingEvent.RenderedMessage}\n{loggingEvent.GetExceptionString()}\n";
            string encryptedMessage = Encrypt(formattedMessage);
            string filePath = file + filename + $"{loggingEvent.TimeStamp:yyyyMMdd}.log";

            if (!Directory.Exists(file)) { Directory.CreateDirectory(file); }

            if (File.Exists(filePath))
            {
                var fileTotal = Directory.EnumerateFiles(file, filename + $"{loggingEvent.TimeStamp:yyyyMMdd}" + "*.log", SearchOption.AllDirectories).Count() - 1;
                var fileNo = fileTotal == 0 ? "" : "_" + fileTotal.ToString();

                filePath = file + filename + $"{loggingEvent.TimeStamp:yyyyMMdd}" + fileNo + ".log";

                FileInfo fileInfo = new FileInfo(filePath);
                long fileSizeInMB = fileInfo.Length / 1000000;

                if (fileSizeInMB > 100)
                {
                    filePath = file + filename + $"{loggingEvent.TimeStamp:yyyyMMdd}_" + (fileTotal + 1) + ".log";
                }
            }


            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(encryptedMessage);
                writer.Close();
            }
        }

        protected override bool RequiresLayout
        {
            get { return false; }
        }

        private static string Encrypt(string message)
        {
            byte[] encrypted;
            byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
            byte[] key = new byte[16];
            Array.Copy(keyBytes, key, Math.Min(keyBytes.Length, key.Length));

            using (Aes aes = Aes.Create())
            {
                //aes.Key = key;
                aes.Key = GetKey(EncryptionKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.IV = IV;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    encrypted = encryptor.TransformFinalBlock(messageBytes, 0, messageBytes.Length);
                    return Convert.ToBase64String(encrypted.ToArray());
                }
            }
        }

        // converts password to 128 bit hash
        private static byte[] GetKey(string password)
        {
            var keyBytes = Encoding.UTF8.GetBytes(password);
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(keyBytes);
            }
        }
    }
}

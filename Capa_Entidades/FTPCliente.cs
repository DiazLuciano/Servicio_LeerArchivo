using Servicio_DescargarArchivo_Galileo.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Servicio_DescargarArchivo_Galileo.Capa_Entidades
{
    public class FTPCliente
    {
        private string _host = null;
        private string _user = null;
        private string _pass = null;
        private FtpWebRequest _ftpRequest = null;
        private FtpWebResponse _ftpResponse = null;
        private Stream _ftpStream = null;
        private int _bufferSize = 2048;

        public FTPCliente()
        {
            _host = Settings.Default.FTP;
            _user = Settings.Default.FTP_Usuario;
            _pass = Settings.Default.FTP_Contrasena;
        }

        public int Download(string remoteFile, string localFile)
        {
            try
            {
                bool error = false;
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(remoteFile);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);

                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                _ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                byte[] byteBuffer = new byte[_bufferSize];
                int bytesRead = _ftpStream.Read(byteBuffer, 0, _bufferSize);
                try
                {
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = _ftpStream.Read(byteBuffer, 0, _bufferSize);
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.EscribirEnLog("Error reading file when downloading it from FTP. " + remoteFile + ". Exepcion: " + ex.Message);
                    error = true;
                }
                finally { localFileStream.Close(); }



                if (error)
                    return -1;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                Log.Instance.EscribirEnLog("Error downloading file from FTP: " + remoteFile + ". Exepcion: " + ex.Message);
                return -2;
            }
            finally
            {
                _ftpStream.Close();
                _ftpResponse.Close();
                _ftpRequest = null;
            }
        }

        public int Upload(string remoteFile, string localFile)
        {
            try
            {
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(remoteFile);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                FileStream stream = File.OpenRead(localFile);
                byte[] buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);
                stream.Close();

                Stream reqStream = _ftpRequest.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();

                return 1;
            }
            catch (Exception ex)
            {
                Log.Instance.EscribirEnLog("An error ocurred when uploading a file to the FTP. Excepción: " + ex.Message);
                return -1;
            }
        }

        public bool CreateDirectory(string newDirectory)
        {
            try
            {
                _ftpRequest = (FtpWebRequest)WebRequest.Create(newDirectory);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                _ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpResponse.Close();
                _ftpRequest = null;

                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
        }

        public string GetFileSize(string fileName)
        {
            try
            {
                _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + fileName);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = true;
                _ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                StreamReader ftpReader = new StreamReader(_ftpStream);
                string fileInfo = null;
                try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
                catch (Exception ex) { Log.Instance.EscribirEnLog(ex.ToString()); }
                ftpReader.Close();
                _ftpStream.Close();
                _ftpResponse.Close();
                _ftpRequest = null;
                return fileInfo;
            }
            catch (Exception ex) { Log.Instance.EscribirEnLog(ex.ToString()); }
            return "";
        }

        public bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}

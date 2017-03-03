using System;
using System.IO;
using System.Net;
using System.Text;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Ftp.Services {
    public class FtpService : IFtpService {
        public readonly ICakeLog _log;

        public FtpService(ICakeLog log) {
            _log = log;
        }

        public void UploadFile(Uri serverUri, IFile uploadFile, string username, string password) {
            // Adding verbose logging for the URI being used.
            _log.Verbose("Uploading file to {0}", serverUri);
            // Creating the request
            var request = (FtpWebRequest)WebRequest.Create(serverUri);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Adding verbose logging for credentials used.
            _log.Verbose("Using the following credentials {0}, {1}", username, password);
            request.Credentials = new NetworkCredential(username, password);

            request.ContentLength = uploadFile.Length;

            using (var stream = new FileStream(uploadFile.Path.FullPath, FileMode.Open, FileAccess.Read))
            {
                var requestStream = request.GetRequestStream();
                stream.CopyTo(requestStream);
                requestStream.Close();

                // Getting the response from the FTP server.
                var response = (FtpWebResponse)request.GetResponse();

                // Logging if it completed and the description of the status returned.
                _log.Information("File upload complete, status {0}", response.StatusDescription);
                response.Close();
            }
        }

        public void DeleteFile(Uri serverUri, string username, string password) {
            var request = (FtpWebRequest)WebRequest.Create(serverUri);
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            // Adding verbose logging for credentials used.
            _log.Verbose("Using the following credentials {0}, {1}", username, password);
            request.Credentials = new NetworkCredential(username, password);

            var response = (FtpWebResponse)request.GetResponse();
            // Logging if it completed and the description of the status returned.
            _log.Information("File upload complete, status {0}", response.StatusDescription);
            response.Close();
        }
    }
}

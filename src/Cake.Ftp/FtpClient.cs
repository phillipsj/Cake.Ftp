using System;
using System.IO;
using System.Net;
using System.Text;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Ftp {
    public class FtpClient {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly StringComparison _comparison;

        public FtpClient(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log) {
            if (fileSystem == null) {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null) {
                throw new ArgumentNullException(nameof(environment));
            }
            if (log == null) {
                throw new ArgumentNullException(nameof(log));
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
            _comparison = environment.Platform.IsUnix() ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        }

        public void UploadFile(Uri serverUri, FilePath fileToUpload, string username, string password)
        {
            if (serverUri == null)
            {
                throw new ArgumentNullException(nameof(serverUri), "Server URI is null.");
            }
            if (fileToUpload == null)
            {
                throw new ArgumentNullException(nameof(fileToUpload), "File to upload is null.");
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username), "Username is null.");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password), "Password is null.");
            }
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                throw new ArgumentOutOfRangeException("Server URI scheme is not FTP.");
            }

            // Adding verbose logging for the URI being used.
            _log.Verbose("Uploading file to {0}", serverUri);

            // Creating the request
            var request = (FtpWebRequest)WebRequest.Create(serverUri);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Adding verbose logging for credentials used.
            _log.Verbose("Using the following credentials {0}, {1}", username, password);
            request.Credentials = new NetworkCredential(username, password);

            // Using the abstracted filesystem to get the file.
            var uploadFile = _fileSystem.GetFile(fileToUpload);

            using (var streamReader = new StreamReader(uploadFile.OpenRead()))
            {
                // Get the file contents.
                var fileContents = Encoding.UTF8.GetBytes(streamReader.ReadToEnd());
                request.ContentLength = fileContents.Length;

                // Writing the file to the request stream.
                var requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                // Getting the response from the FTP server.
                var response = (FtpWebResponse)request.GetResponse();

                // Logging if it completed and the description of the status returned.
                _log.Information("File upload complete, status {0}", response.StatusDescription);
                response.Close();
            }
        }
        
        public void DeleteFile(Uri serverUri, string username, string password) {
            if (serverUri == null) {
                throw new ArgumentNullException(nameof(serverUri), "Server URI is null.");
            }
            if (string.IsNullOrWhiteSpace(username)) {
                throw new ArgumentNullException(nameof(username), "Username is null.");
            }
            if (string.IsNullOrWhiteSpace(password)) {
                throw new ArgumentNullException(nameof(password), "Password is null.");
            }
            if (serverUri.Scheme != Uri.UriSchemeFtp) {
                throw new ArgumentOutOfRangeException("Server URI scheme is not FTP.");
            }
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

        public void GetFile() {}
    }
}

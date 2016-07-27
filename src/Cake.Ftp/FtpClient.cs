using System;
using System.Net;
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

        public void UploadFile(Uri serverUri, FilePath fileToUpload, string username, string password) {
            if (serverUri == null) {
                throw new ArgumentNullException(nameof(serverUri), "Server URI is null.");
            }
            if (fileToUpload == null) {
                throw new ArgumentNullException(nameof(fileToUpload), "File to upload is null.");
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

        }

        public void UploadFile(FtpSettings settings) {}

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
            var request = WebRequest.Create(serverUri) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            var response = request.GetResponse() as FtpWebResponse;
        }

        public void GetFile() {}
    }
}

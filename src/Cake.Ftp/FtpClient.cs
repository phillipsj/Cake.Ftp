using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Ftp.Services;

namespace Cake.Ftp {
    public class FtpClient {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IFtpService _ftpService;
        private readonly StringComparison _comparison;

        public FtpClient(IFileSystem fileSystem, ICakeEnvironment environment, IFtpService ftpService) {
            if (fileSystem == null) {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null) {
                throw new ArgumentNullException(nameof(environment));
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _ftpService = ftpService;
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

            var file = _fileSystem.GetFile(fileToUpload);
            _ftpService.UploadFile(serverUri, file, username, password);
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
            _ftpService.DeleteFile(serverUri, username, password);
        }

        public void GetFile() {}
    }
}

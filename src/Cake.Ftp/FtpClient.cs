using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Ftp.Services;

namespace Cake.Ftp {
    /// <summary>
    /// The FTP Client.
    /// </summary>
    public class FtpClient {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IFtpService _ftpService;
        private readonly StringComparison _comparison;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileSystem">The filesystem.</param>
        /// <param name="environment">The cake environment.</param>
        /// <param name="ftpService">The FTP Service.</param>
        public FtpClient(IFileSystem fileSystem, ICakeEnvironment environment, IFtpService ftpService) {
            fileSystem.NotNull(nameof(fileSystem));
            environment.NotNull(nameof(environment));
            ftpService.NotNull(nameof(ftpService));

            _fileSystem = fileSystem;
            _environment = environment;
            _ftpService = ftpService;
            _comparison = environment.Platform.IsUnix() ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        }

        /// <summary>
        /// Uploads file to specified location using the supplied credentials.
        /// </summary>
        /// <param name="host">host of the FTP Client</param>
        /// <param name="remotePath">path on the file on the server</param>
        /// <param name="fileToUpload">The file to upload.</param>
        /// <param name="settings">The settings.</param>
        public void UploadFile(string host, string remotePath, FilePath fileToUpload, FtpSettings settings) {
            CheckParams(host, remotePath, settings);
            fileToUpload.NotNull(nameof(fileToUpload));

            var file = _fileSystem.GetFile(fileToUpload);
            _ftpService.UploadFile(host, remotePath, file, settings);
        }

        /// <summary>
        /// Deletes a file at the specified URI.
        /// </summary>
        /// <param name="host">host of the FTP Client</param>
        /// <param name="remotePath">path on the file on the server</param>
        /// <param name="settings">The settings.</param>
        public void DeleteFile(string host, string remotePath, FtpSettings settings) {
            CheckParams(host, remotePath, settings);

            _ftpService.DeleteFile(host, remotePath, settings);
        }
        
        private void CheckParams(string host, string remotePath, FtpSettings settings)
        {
            host.NotNullOrWhiteSpace(nameof(host));
            remotePath.NotNullOrWhiteSpace(nameof(host));
            settings.Username.NotNullOrWhiteSpace(nameof(settings.Username));
            settings.Password.NotNullOrWhiteSpace(nameof(settings.Password));
        }

    }
}

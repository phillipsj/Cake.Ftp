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
        /// <param name="serverUri">FTP Service URI.</param>
        /// <param name="fileToUpload">The file to upload.</param>
        /// <param name="settings">The settings.</param>
        public void UploadFile(Uri serverUri, FilePath fileToUpload, FtpSettings settings) {
            CheckParams(serverUri, settings);
            fileToUpload.NotNull(nameof(fileToUpload));

            var file = _fileSystem.GetFile(fileToUpload);
            _ftpService.UploadFile(serverUri, file, settings.Username, settings.Password);
        }

        /// <summary>
        /// Deletes a file at the specified URI.
        /// </summary>
        /// <param name="serverUri">FTP File URI.</param>
        /// <param name="settings">The settings.</param>
        public void DeleteFile(Uri serverUri, FtpSettings settings) {
            CheckParams(serverUri, settings);

            _ftpService.DeleteFile(serverUri, settings.Username, settings.Password);
        }
        
        private void CheckParams(Uri serverUri, FtpSettings settings) {
            serverUri.NotNull(nameof(serverUri));
            settings.Username.NotNullOrWhiteSpace(nameof(settings.Username));
            settings.Password.NotNullOrWhiteSpace(nameof(settings.Password));
            serverUri.NotFtpUriScheme(nameof(serverUri));
        }

        private void GetFile() { throw new NotImplementedException();}
    }
}

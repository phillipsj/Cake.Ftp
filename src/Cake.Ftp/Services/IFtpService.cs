using System;
using Cake.Core.IO;

namespace Cake.Ftp.Services {
    /// <summary>
    /// Interface for the <see cref="FtpService"/> class. 
    /// </summary>
    public interface IFtpService {
        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="serverUri">The URI for the FTP server.</param>
        /// <param name="fileToUpload">The file to upload.</param>
        /// <param name="username">The FTP username.</param>
        /// <param name="password">The FTP password.</param>
        void UploadFile(Uri serverUri, IFile fileToUpload, string username, string password);
        
        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="serverUri">The URI for the FTP server.</param>
        /// <param name="username">The FTP username.</param>
        /// <param name="password">The FTP password.</param>
        void DeleteFile(Uri serverUri, string username, string password);
    }
}

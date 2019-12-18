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
        /// <param name="host">host of the FTP Client</param>
        /// <param name="remotePath">path on the file on the server</param>
        /// <param name="fileToUpload">The file to upload.</param>
        /// <param name="settings">Ftp Settings</param>
        void UploadFile(string host, string remotePath, IFile fileToUpload, FtpSettings settings);

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="host">host of the FTP Client</param>
        /// <param name="remotePath">path on the file on the server</param>
        /// <param name="settings">Ftp Settings</param>
        void DeleteFile(string host, string remotePath, FtpSettings settings);
    }
}

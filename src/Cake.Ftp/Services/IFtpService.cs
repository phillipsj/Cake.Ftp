using System;
using System.Collections.Generic;
using Cake.Core.IO;
using FluentFTP;
using FluentFTP.Rules;

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

        /// <summary>
        /// Upload and Overwite remote folder with local folder for default
        /// </summary>
        /// <param name="host"></param>
        /// <param name="remoteFolder"></param>
        /// <param name="localFolder"></param>
        /// <param name="settings"></param>
        /// <param name="ftpFolderSyncMode"></param>
        /// <param name="ftpRemoteExists"></param>
        /// <param name="ftpVerify"></param>
        /// <param name="rules"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        List<FtpResult> UpdateFolder(string host, string remoteFolder, string localFolder, FtpSettings settings,
            List<FtpRule> rules = null, Action<FtpProgress> process = null,
            FtpFolderSyncMode ftpFolderSyncMode = FtpFolderSyncMode.Mirror, FtpRemoteExists ftpRemoteExists = FtpRemoteExists.Overwrite, FtpVerify ftpVerify = FtpVerify.None
            );
    }
}

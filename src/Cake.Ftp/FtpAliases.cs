using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Ftp.Services;
using FluentFTP;
using FluentFTP.Rules;

namespace Cake.Ftp {
    /// <summary>
    /// Contains functionality for working with FTP
    /// </summary>
    [CakeAliasCategory("FTP")]
    public static class FtpAliases {
        /// <summary>
        /// Uploads the file to the FTP server using the supplied credentials.
        /// </summary>
        /// <example>
        /// <code>
        /// Task("UploadFile")
        ///   .Does(() => {
        ///     var fileToUpload = File("some.txt");
        ///     FtpUploadFile("ftp://myserver/random/test.htm", fileToUpload, "some-user", "some-password");
        /// });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="serverUri">FTP URI requiring FTP:// schema.</param>
        /// <param name="fileToUpload">The file to be uploaded.</param>
        /// <param name="username">Username of the FTP account.</param>
        /// <param name="password">Password of the FTP account.</param>
        [CakeMethodAlias]
        public static void FtpUploadFile(this ICakeContext context, Uri serverUri, FilePath fileToUpload,
            string username, string password) {
            var settings = new FtpSettings() { Username = username, Password = password };
            FtpUploadFile(context, serverUri, fileToUpload, settings);
        }

        /// <summary>
        /// Uploads the file to the FTP server using the supplied credentials.
        /// </summary>
        /// <example>
        /// <code>
        /// Task("UploadFile")
        ///   .Does(() => {
        ///     var fileToUpload = File("some.txt");
        ///     var settings = new FtpSettings() {
        ///       Username = "some-user",
        ///       Password = "some-password",
        ///       FileExistsBehavior = FtpExists.Overwrite,
        ///       CreateRemoteDirectory = true
        ///     };
        ///     FtpUploadFile("ftp://myserver/random/test.htm", fileToUpload, settings);
        /// });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="serverUri">FTP URI requiring FTP:// schema.</param>
        /// <param name="fileToUpload">The file to be uploaded.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void FtpUploadFile(this ICakeContext context, Uri serverUri, FilePath fileToUpload,
            FtpSettings settings) {
            FtpUploadFile(context, serverUri.Host, serverUri.AbsolutePath, fileToUpload, settings);
        }

        /// <summary>
        /// Uploads the file to the FTP server using the supplied credentials.
        /// </summary>
        /// <example>
        /// <code>
        /// Task("UploadFile")
        ///   .Does(() => {
        ///     var fileToUpload = File("some.txt");
        ///     var settings = new FtpSettings() {
        ///       Username = "some-user",
        ///       Password = "some-password",
        ///       FileExistsBehavior = FtpExists.Overwrite,
        ///       CreateRemoteDirectory = true
        ///     };
        ///     FtpUploadFile("myserver", "/random/test.htm", fileToUpload, settings);
        /// });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="host">host of the FTP Client</param>
        /// <param name="remotePath">path on the file on the server</param>
        /// <param name="fileToUpload">The file to be uploaded.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void FtpUploadFile(this ICakeContext context, string host, string remotePath, FilePath fileToUpload,
            FtpSettings settings)
        {
            context.NotNull(nameof(context));
            var ftpClient = new FtpClient(context.FileSystem, context.Environment, new FtpService(context.Log));
            ftpClient.UploadFile(host, remotePath, fileToUpload, settings);
        }

        /// <summary>
        /// Upload and Overwite remote folder with local folder for default
        /// </summary>
        /// <param name="context"></param>
        /// <param name="host"></param>
        /// <param name="remoteFolder"></param>
        /// <param name="localFolder"></param>
        /// <param name="settings"></param>
        /// <param name="rules"></param>
        /// <param name="process"></param>
        /// <param name="ftpFolderSyncMode"></param>
        /// <param name="ftpRemoteExists"></param>
        /// <param name="ftpVerify"></param>
        [CakeMethodAlias]
        public static void FtpUploadFolder(this ICakeContext context, string host, string remoteFolder, string localFolder, FtpSettings settings,
            List<FtpRule> rules = null, Action<FtpProgress> process = null,
            FtpFolderSyncMode ftpFolderSyncMode = FtpFolderSyncMode.Mirror, FtpRemoteExists ftpRemoteExists = FtpRemoteExists.Overwrite, FtpVerify ftpVerify = FtpVerify.None)
        {
            context.NotNull(nameof(context));

            var ftpClient = new FtpClient(context.FileSystem, context.Environment, new FtpService(context.Log));            
            ftpClient.UploadFolder(host, remoteFolder, localFolder, settings, rules, process, ftpFolderSyncMode, ftpRemoteExists, ftpVerify);
        }

        /// <summary>
        /// Upload folder parallel
        /// </summary>
        /// <param name="context"></param>
        /// <param name="host"></param>
        /// <param name="remoteFolder"></param>
        /// <param name="localFolder"></param>
        /// <param name="settings"></param>
        /// <param name="parallel"></param>
        /// <param name="ignoreRule"></param>
        [CakeMethodAlias]
        public static void FtpUploadFolderParallel(this ICakeContext context, string host, string remoteFolder, string localFolder, FtpSettings settings, int parallel = 5, Func<string, bool> ignoreRule = null)
        {
            context.NotNull(nameof(context));            

            var ftpClient = new FtpClient(context.FileSystem, context.Environment, new FtpService(context.Log));
            ftpClient.UploadFolderParallel(host, remoteFolder, localFolder, settings, parallel, ignoreRule);
        }

        /// <summary>
        /// Deletes the file on the FTP server using the supplied credentials.
        /// </summary>
        /// <example>
        /// <code>
        /// Task("DeleteFile")
        ///   .Does(() => {
        ///     FtpDeleteFile("ftp://myserver/random/test.htm", "some-user", "some-password");
        /// });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="serverUri">FTP URI requring FTP:// scehma.</param>
        /// <param name="username">Username of the FTP account.</param>
        /// <param name="password">Password of the FTP account.</param>
        [CakeMethodAlias]
        public static void FtpDeleteFile(this ICakeContext context, Uri serverUri, string username, string password) {
            var settings = new FtpSettings() { Username = username, Password = password };
            FtpDeleteFile(context, serverUri, settings);
        }

        /// <summary>
        /// Deletes the file on the FTP server using the supplied credentials.
        /// </summary>
        /// <example>
        /// <code>
        /// Task("DeleteFile")
        ///   .Does(() => {
        ///     var settings = new FtpSettings() {
        ///       Username = "some-user",
        ///       Password = "some-password"
        ///     };
        ///     FtpDeleteFile("ftp://myserver/random/test.htm", settings);
        /// });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="serverUri">FTP URI requring FTP:// scehma.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void FtpDeleteFile(this ICakeContext context, Uri serverUri, FtpSettings settings) {
            FtpDeleteFile(context, serverUri.Host, serverUri.AbsolutePath, settings);
        }

        /// <summary>
        /// Deletes the file on the FTP server using the supplied credentials.
        /// </summary>
        /// <example>
        /// <code>
        /// Task("DeleteFile")
        ///   .Does(() => {
        ///     var settings = new FtpSettings() {
        ///       Username = "some-user",
        ///       Password = "some-password"
        ///     };
        ///     FtpDeleteFile("myserver", "/random/test.htm", settings);
        /// });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="host">host of the FTP Client</param>
        /// <param name="remotePath">path on the file on the server</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void FtpDeleteFile(this ICakeContext context, string host, string remotePath, FtpSettings settings)
        {
            context.NotNull(nameof(context));
            var ftpClient = new FtpClient(context.FileSystem, context.Environment, new FtpService(context.Log));
            ftpClient.DeleteFile(host, remotePath, settings);
        }
    }
}

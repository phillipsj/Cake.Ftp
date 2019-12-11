using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Ftp.Services;

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

using System;
using System.ComponentModel;
using System.Net;
using System.Security.Authentication;
using System.Text;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using FluentFTP;

namespace Cake.Ftp.Services {
    /// <summary>
    /// The FTP Service.
    /// </summary>
    public class FtpService : IFtpService {
        private readonly ICakeLog _log;

        /// <summary>
        /// Intializes a new instance of the <see cref="FtpService"/> class. 
        /// </summary>
        /// <param name="log"></param>
        public FtpService(ICakeLog log) {
            _log = log;
        }

        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="host">host of the FTP Client</param>
        /// <param name="remotePath">path on the file on the server</param>
        /// <param name="uploadFile">The file to upload.</param>
        /// <param name="settings">Ftp Settings</param>
        public void UploadFile(string host, string remotePath, IFile uploadFile, FtpSettings settings) {

            using (var client = CreateClient(host, settings)) {
                Connect(client, settings.AutoDetectConnectionSettings);

                client.UploadFile(uploadFile.Path.FullPath, remotePath, Translate(settings.FileExistsBehavior), settings.CreateRemoteDirectory);
                client.Disconnect();
            }
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="host">host of the FTP Client</param>
        /// <param name="remotePath">path on the file on the server</param>
        /// <param name="settings">Ftp Settings</param>
        public void DeleteFile(string host, string remotePath, FtpSettings settings) {

            using (var client = CreateClient(host, settings)) {
                Connect(client, settings.AutoDetectConnectionSettings);

                client.DeleteFile(remotePath);
                client.Disconnect();
            }

        }

        private FluentFTP.FtpClient CreateClient(string host, FtpSettings settings) {
            var client = new FluentFTP.FtpClient(host, new NetworkCredential(settings.Username, settings.Password));
            client.OnLogEvent += OnLogEvent;

            client.ValidateAnyCertificate = settings.ValidateAnyCertificate;
            
            if (settings.AutoDetectConnectionSettings) 
                return client;

            client.EncryptionMode = Translate(settings.EncryptionMode);
            client.SslProtocols = settings.SslProtocols;
            client.DataConnectionType = Translate(settings.DataConnectionType);

            return client;
        }

        private void Connect(FluentFTP.FtpClient client, bool autoDetectConnectionSettings) {
            if (autoDetectConnectionSettings) {
                client.AutoConnect();
            }
            else {
                client.Connect();
            }
        }

        private void OnLogEvent(FtpTraceLevel level, string message) {
            switch (level) {
                case FtpTraceLevel.Error: {
                    _log.Error(message);
                    break;
                }
                case FtpTraceLevel.Warn: {
                    _log.Warning(message);
                    break;
                }
                case FtpTraceLevel.Info: {
                    _log.Information(message);
                    break;
                }
                case FtpTraceLevel.Verbose: {
                    _log.Verbose(message);
                    break;
                }
            }
        }

        private FluentFTP.FtpExists Translate(FtpExists ftpExists) {
            switch (ftpExists) {
                case FtpExists.Append:
                    return FluentFTP.FtpExists.Append;
                case FtpExists.AppendNoCheck:
                    return FluentFTP.FtpExists.AppendNoCheck;
                case FtpExists.NoCheck :
                    return FluentFTP.FtpExists.NoCheck;
                case FtpExists.Overwrite:
                    return FluentFTP.FtpExists.Overwrite;
                case FtpExists.Skip:
                    return FluentFTP.FtpExists.Skip;
            }

            throw new InvalidEnumArgumentException($"{nameof(FtpExists)} enum value {ftpExists} is invalid as it has not been mapped");
        }

        private FluentFTP.FtpEncryptionMode Translate(FtpEncryptionMode ftpEncryptionMode)
        {
            switch (ftpEncryptionMode)
            {
                case FtpEncryptionMode.Explicit:
                    return FluentFTP.FtpEncryptionMode.Explicit;
                case FtpEncryptionMode.Implicit:
                    return FluentFTP.FtpEncryptionMode.Implicit;
                case FtpEncryptionMode.None:
                    return FluentFTP.FtpEncryptionMode.None;
            }

            throw new InvalidEnumArgumentException($"{nameof(FtpEncryptionMode)} enum value {ftpEncryptionMode} is invalid as it has not been mapped");
        }

        private FluentFTP.FtpDataConnectionType Translate(FtpDataConnectionType ftpDataConnectionType) {
            switch (ftpDataConnectionType) {
                case FtpDataConnectionType.AutoActive:
                    return FluentFTP.FtpDataConnectionType.AutoActive;
                case FtpDataConnectionType.AutoPassive:
                    return FluentFTP.FtpDataConnectionType.AutoPassive;
                case FtpDataConnectionType.EPRT:
                    return FluentFTP.FtpDataConnectionType.EPRT;
                case FtpDataConnectionType.EPSV:
                    return FluentFTP.FtpDataConnectionType.EPSV;
                case FtpDataConnectionType.PASV:
                    return FluentFTP.FtpDataConnectionType.PASV;
                case FtpDataConnectionType.PASVEX:
                    return FluentFTP.FtpDataConnectionType.PASVEX;
                case FtpDataConnectionType.PORT:
                    return FluentFTP.FtpDataConnectionType.PORT;
            }
            throw new InvalidEnumArgumentException($"{nameof(FtpDataConnectionType)} enum value {ftpDataConnectionType} is invalid as it has not been mapped");
        }

    }
}

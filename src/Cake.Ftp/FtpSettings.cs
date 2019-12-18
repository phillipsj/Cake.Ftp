using System.Security.Authentication;

namespace Cake.Ftp {
    /// <summary>
    /// Contains settings used by <see cref="FtpClient"/>
    /// </summary>
    public sealed class FtpSettings {
        
        /// <summary>
        /// The constructor.
        /// </summary>
        public FtpSettings() {
            FileExistsBehavior = FtpExists.Overwrite;
            EncryptionMode = FtpEncryptionMode.None;
            AutoDetectConnectionSettings = true;
            DataConnectionType = FtpDataConnectionType.AutoPassive;
        }

        /// <summary>
        /// Gets or sets the FTP username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the FTP password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Defines the behavior for uploading/downloading files that already exist. Default is Overwrite
        /// </summary>
        public FtpExists FileExistsBehavior { get; set; }

        /// <summary>
        /// Create the remote directory if it does not exist. Slows down upload due to additional checks required.
        /// </summary>
        public bool CreateRemoteDirectory { get; set; }

        /// <summary>
        /// Automatic FTP and FTPS connection negotiation.
        /// This method tries every possible combination of the FTP connection properties, and connects to the first successful profile.
        /// Returns the FtpProfile if the connection succeeded, or null if it failed.
        /// Default is true
        /// </summary>
        public bool AutoDetectConnectionSettings { get; set; }

        /// <summary>
        /// Only used if AutoDetectConnectionSettings is false.
        /// The type of SSL to use, or none. Default is none. Explicit is TLS, Implicit is SSL.
        /// </summary>
        public FtpEncryptionMode EncryptionMode { get; set; }

        /// <summary>
        /// Only used if AutoDetectConnectionSettings is false.
        /// Encryption protocols to use. Only valid if EncryptionMode property is not equal to <see cref="F:Cake.Ftp.FtpEncryptionMode.None" />.
        /// Default value is .NET Framework defaults from the <see cref="T:System.Net.Security.SslStream" /> class.
        /// </summary>
        public SslProtocols SslProtocols { get; set; }

        /// <summary>
        /// Only used if AutoDetectConnectionSettings is false.
        /// Data connection type, default is AutoPassive which tries
        /// a connection with EPSV first and if it fails then tries
        /// PASV before giving up. If you know exactly which kind of
        /// connection you need you can slightly increase performance
        /// by defining a specific type of passive or active data
        /// connection here.
        /// </summary>
        public FtpDataConnectionType DataConnectionType { get; set; }

        /// <summary>
        /// If true all ssl certificates are accepted
        /// </summary>
        public bool ValidateAnyCertificate { get; set; }
    }
}

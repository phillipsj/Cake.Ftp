namespace Cake.Ftp {
    /// <summary>
    /// Contains settings used by <see cref="FtpClient"/>
    /// </summary>
    public sealed class FtpSettings {
        /// <summary>
        /// Gets or sets the FTP username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the FTP password.
        /// </summary>
        public string Password { get; set; }
    }
}

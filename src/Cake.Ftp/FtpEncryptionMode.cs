﻿namespace Cake.Ftp {
    /// <summary>Defines the type of encryption to use</summary>
    public enum FtpEncryptionMode
    {
        /// <summary>Plain text.</summary>
        None,
        /// <summary>
        /// FTPS encryption is used from the start of the connection, port 990.
        /// </summary>
        Implicit,
        /// <summary>
        /// Connection starts in plain text and FTPS encryption is enabled
        /// with the AUTH command immediately after the server greeting.
        /// </summary>
        Explicit,
    }
}
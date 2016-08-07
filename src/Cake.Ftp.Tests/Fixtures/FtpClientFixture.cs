using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Ftp.Services;
using Cake.Testing;
using NSubstitute;

namespace Cake.Ftp.Tests.Fixtures {
    public class FtpClientFixture {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeLog Log { get; set; }
        public IFtpService FtpService { get; set; }
        public Uri ServerUri { get; set; }
        public FilePath FileToUpload { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public FtpClientFixture(bool fileToUploadExists = true) {
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateDirectory("/Working");

            if (fileToUploadExists) {
                var fileToUpload = fileSystem.CreateFile("/Working/upload.txt").SetContent("This file is for uploading.");
                FileToUpload = fileToUpload.Path;
            }
            Log = Substitute.For<ICakeLog>();
            FtpService = Substitute.For<IFtpService>();
            FileSystem = fileSystem;
        }

        public void UploadFile() {
            var ftpClient = new FtpClient(FileSystem, Environment, FtpService);
            ftpClient.UploadFile(ServerUri, FileToUpload, Username, Password);
        }

        public void DeleteFile() {
            var ftpClient = new FtpClient(FileSystem, Environment, FtpService);
            ftpClient.DeleteFile(ServerUri, Username, Password);
        }
    }
}

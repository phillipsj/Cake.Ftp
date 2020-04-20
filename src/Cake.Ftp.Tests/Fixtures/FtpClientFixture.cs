using System;
using System.Collections.Generic;
using System.Linq;
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

        public string Host { get; set; }
        public string RemotePath { get; set; }
        public string RemoteDirectory { get; set; }
        public FilePath FileToUpload { get; set; }
        public DirectoryPath DirectoryToUpload { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public FtpClientFixture(bool fileToUploadExists = true, bool directoryToUploadExists = true) {
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateDirectory("/Working");
            DirectoryToUpload = new DirectoryPath("/Working");
            
            if (fileToUploadExists) {
                var fileToUpload = fileSystem.CreateFile("/Working/upload.txt").SetContent("This file is for uploading.");
                FileToUpload = fileToUpload.Path;
            }

            if (directoryToUploadExists)
            {
                fileSystem.CreateDirectory("/Working/Complex");
                DirectoryToUpload = new DirectoryPath("/Working/Complex");

                fileSystem.CreateFile("/Working/Complex/upload1.txt").SetContent("Upload 1");

                fileSystem.CreateDirectory("/Working/Complex/Sub");
                fileSystem.CreateFile("/Working/Complex/Sub/upload2.txt").SetContent("Upload 2");
                fileSystem.CreateFile("/Working/Complex/Sub/upload3.txt").SetContent("Upload 3");
                
                fileSystem.CreateDirectory("/Working/Complex/Sub/Sub");
                fileSystem.CreateFile("/Working/Complex/Sub/Sub/upload4.txt").SetContent("Upload 4");

                fileSystem.CreateDirectory("/Working/Complex/Empty");
            }


            Log = Substitute.For<ICakeLog>();
            FtpService = Substitute.For<IFtpService>();
            FileSystem = fileSystem;
            Environment = environment;
            Username = "username";
            Password = "password";
            Host = "my.server.com";
            RemotePath = "/test.html";
            RemoteDirectory = "/Complex";

        }

        public void UploadFile() {
            var ftpClient = new FtpClient(FileSystem, Environment, FtpService);
            var settings = new FtpSettings() {Username = Username, Password = Password};
            ftpClient.UploadFile(Host, RemotePath, FileToUpload, settings);
        }

        public void DeleteFile() {
            var ftpClient = new FtpClient(FileSystem, Environment, FtpService);
            var settings = new FtpSettings() { Username = Username, Password = Password };
            ftpClient.DeleteFile(Host, RemotePath, settings);
        }

        public void UploadDirectory()
        {
            var ftpClient = new FtpClient(FileSystem, Environment, FtpService);
            var settings = new FtpSettings() { Username = Username, Password = Password };
            var sourceDirectory = DirectoryToUpload == null ? null : FileSystem.GetDirectory(DirectoryToUpload);
            ftpClient.UploadDirectory(Host, RemoteDirectory, sourceDirectory, settings);
        }

        public void VerifyDirectoryUpload_AllFilesPassedCorrectly() {
            FtpService.Received().UploadDirectories(
                Arg.Any<string>(), 
                Arg.Is<Dictionary<string, IEnumerable<string>>>(d => CheckUploadDictionary(d)),
                Arg.Any<FtpSettings>());
        }

        private bool CheckUploadDictionary(Dictionary<string, IEnumerable<string>> dictionary) {
            return dictionary.Count == 3
                   && dictionary.Keys.Contains($"{RemoteDirectory}")
                   && dictionary.Keys.Contains($"{RemoteDirectory}/Sub")
                   && dictionary.Keys.Contains($"{RemoteDirectory}/Sub/Sub")
                   && dictionary[$"{RemoteDirectory}"].Count()==1
                   && dictionary[$"{RemoteDirectory}"].Contains("/Working/Complex/upload1.txt")
                   && dictionary[$"{RemoteDirectory}/Sub"].Count() == 2
                   && dictionary[$"{RemoteDirectory}/Sub"].Contains("/Working/Complex/Sub/upload2.txt")
                   && dictionary[$"{RemoteDirectory}/Sub"].Contains("/Working/Complex/Sub/upload3.txt")
                   && dictionary[$"{RemoteDirectory}/Sub/Sub"].Count() == 1
                   && dictionary[$"{RemoteDirectory}/Sub/Sub"].Contains("/Working/Complex/Sub/Sub/upload4.txt");

        }

    }
}

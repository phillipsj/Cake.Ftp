using System;
using Cake.Ftp.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Cake.Ftp.Tests {
    public sealed class FtpTests {
        public sealed class TheConstructor {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null() {
                // Given
                var fixture = new FtpClientFixture { FileSystem = null };

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                 .ParamName.Should().BeEquivalentTo("fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Cake_Environment_Is_Null() {
                // Given
                var fixture = new FtpClientFixture { Environment = null };

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                .ParamName.Should().BeEquivalentTo("environment");
            }

            [Fact]
            public void Should_Throw_If_FTP_Service_Is_Null() {
                // Given
                var fixture = new FtpClientFixture { FtpService = null };

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                .ParamName.Should().BeEquivalentTo("ftpService");
            }
        }

        public sealed class TheUploadFileMethod {
            [Fact]
            public void Should_Throw_If_Host_Is_Null() {
                // Given
                var fixture = new FtpClientFixture { Host = null };

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                   .ParamName.Should().BeEquivalentTo("host");
            }

            [Fact]
            public void Should_Throw_If_RemotePath_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { RemotePath = null };

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                    .ParamName.Should().BeEquivalentTo("remotePath");
            }

            [Fact]
            public void Should_Throw_If_File_To_Upload_Is_Null() {
                // Given
                var fixture = new FtpClientFixture { FileToUpload = null };

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                  .ParamName.Should().BeEquivalentTo("fileToUpload");
            }

            [Fact]
            public void Should_Throw_If_Username_Is_Null() {
                // Given
                var fixture = new FtpClientFixture { Username = null };

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                   .ParamName.Should().BeEquivalentTo("Username");
            }

            [Fact]
            public void Should_Throw_If_Password_Is_Null() {
                // Given
                var fixture = new FtpClientFixture { Password = null };

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                   .ParamName.Should().BeEquivalentTo("Password");
            }

            [Fact]
            public void Should_Upload_File_Without_Error() {
                // Given
                var fixture = new FtpClientFixture();

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                result.Should().BeNull();
            }
        }

        public sealed class TheDeleteFileMethod
        {
            [Fact]
            public void Should_Throw_If_Host_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { Host = null };

                // When 
                var result = Record.Exception(() => fixture.DeleteFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                    .ParamName.Should().BeEquivalentTo("host");
            }

            [Fact]
            public void Should_Throw_If_RemotePath_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { RemotePath = null };

                // When 
                var result = Record.Exception(() => fixture.DeleteFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                    .ParamName.Should().BeEquivalentTo("remotePath");
            }

            [Fact]
            public void Should_Throw_If_Username_Is_Null() {
                // Given
                var fixture = new FtpClientFixture { Username = null };

                // When 
                var result = Record.Exception(() => fixture.DeleteFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                  .ParamName.Should().BeEquivalentTo("Username");
            }

            [Fact]
            public void Should_Throw_If_Password_Is_Null() {
                // Given
                var fixture = new FtpClientFixture { Password = null };

                // When 
                var result = Record.Exception(() => fixture.DeleteFile());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                 .ParamName.Should().BeEquivalentTo("Password");
            }

            [Fact]
            public void Should_Delete_File_Without_Error() {
                // Given
                var fixture = new FtpClientFixture();

                // When 
                var result = Record.Exception(() => fixture.DeleteFile());

                // Then
                result.Should().BeNull();
            }
        }


        public sealed class TheUploadDirectoryMethod
        {
            [Fact]
            public void Should_Throw_If_Host_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { Host = null };

                // When 
                var result = Record.Exception(() => fixture.UploadDirectory());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                   .ParamName.Should().BeEquivalentTo("host");
            }

            [Fact]
            public void Should_Throw_If_RemotePath_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { RemoteDirectory = null };

                // When 
                var result = Record.Exception(() => fixture.UploadDirectory());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                    .ParamName.Should().BeEquivalentTo("remotePath");
            }

            [Fact]
            public void Should_Throw_If_Source_Directory_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { DirectoryToUpload = null };

                // When 
                var result = Record.Exception(() => fixture.UploadDirectory());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                  .ParamName.Should().BeEquivalentTo("sourceDirectory");
            }

            [Fact]
            public void Should_Throw_If_Username_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { Username = null };

                // When 
                var result = Record.Exception(() => fixture.UploadDirectory());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                   .ParamName.Should().BeEquivalentTo("Username");
            }

            [Fact]
            public void Should_Throw_If_Password_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { Password = null };

                // When 
                var result = Record.Exception(() => fixture.UploadDirectory());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject
                   .ParamName.Should().BeEquivalentTo("Password");
            }

            [Fact]
            public void Should_Upload_File_Without_Error()
            {
                // Given
                var fixture = new FtpClientFixture();

                // When 
                var result = Record.Exception(() => fixture.UploadDirectory());

                // Then
                result.Should().BeNull();
                fixture.VerifyDirectoryUpload_AllFilesPassedCorrectly();
            }
        }
    }
}

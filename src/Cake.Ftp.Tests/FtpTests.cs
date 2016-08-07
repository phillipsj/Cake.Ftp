using System;
using Cake.Ftp.Tests.Fixtures;
using Xunit;
using Cake.Testing;

namespace Cake.Ftp.Tests {
    public sealed class FtpTests {
        public sealed class TheConstructor {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null() {
                // Given
                var fixture = new FtpClientFixture {FileSystem = null};

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Cake_Environment_Is_Null() {
                // Given
                var fixture = new FtpClientFixture {Environment = null};

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_FTP_Service_Is_Null() {
                // Given
                var fixture = new FtpClientFixture {FtpService = null};

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("ftpService", ((ArgumentNullException) result).ParamName);
            }
        }

        public sealed class TheUploadFileMethod {
            [Fact]
            public void Should_Throw_If_Server_Uri_Is_Null() {
                // Given
                var fixture = new FtpClientFixture {ServerUri = null};

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("serverUri", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_File_To_Upload_Is_Null() {
                // Given
                var fixture = new FtpClientFixture {FileToUpload = null};

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileToUpload", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Username_Is_Null() {
                // Given
                var fixture = new FtpClientFixture {Username = null};

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("username", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Password_Is_Null() {
                // Given
                var fixture = new FtpClientFixture {Password = null};

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("password", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Url_Is_Not_FTP_Scheme()
            {
                // Given
                var fixture = new FtpClientFixture { ServerUri = new Uri("http://my.server.com/test.html") };

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                Assert.IsType<ArgumentOutOfRangeException>(result);
                Assert.Equal("serverUri", ((ArgumentOutOfRangeException)result).ParamName);
            }

            [Fact]
            public void Should_Upload_File_Without_Error()
            {
                // Given
                var fixture = new FtpClientFixture();

                // When 
                var result = Record.Exception(() => fixture.UploadFile());

                // Then
                Assert.Equal(null, result);
            }
        }

        public sealed class TheDeleteFileMethod {
            [Fact]
            public void Should_Throw_If_Server_Uri_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { ServerUri = null };

                // When 
                var result = Record.Exception(() => fixture.DeleteFile());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("serverUri", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Username_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { Username = null };

                // When 
                var result = Record.Exception(() => fixture.DeleteFile());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("username", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Password_Is_Null()
            {
                // Given
                var fixture = new FtpClientFixture { Password = null };

                // When 
                var result = Record.Exception(() => fixture.DeleteFile());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("password", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Url_Is_Not_FTP_Scheme()
            {
                // Given
                var fixture = new FtpClientFixture { ServerUri = new Uri("http://my.server.com/test.html") };

                // When 
                var result = Record.Exception(() => fixture.DeleteFile());

                // Then
                Assert.IsType<ArgumentOutOfRangeException>(result);
                Assert.Equal("serverUri", ((ArgumentOutOfRangeException)result).ParamName);
            }

            [Fact]
            public void Should_Delete_File_Without_Error()
            {
                // Given
                var fixture = new FtpClientFixture();

                // When 
                var result = Record.Exception(() => fixture.DeleteFile());

                // Then
                Assert.Equal(null, result);
            }
        }
    }
}

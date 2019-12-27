# Cake.Ftp

Cake-Build addin that extends Cake with FTP commands using [FluentFTP](https://github.com/robinrodricks/FluentFTP).

## Table of contents

1. [Implemented functionality](https://github.com/cake-contrib/Cake.Ftp#implemented-functionality)
2. [Referencing](https://github.com/cake-contrib/Cake.Ftp#referencing)
3. [Usage](https://github.com/cake-contrib/Cake.Ftp#usage)
4. [Connections](https://github.com/cake-contrib/Cake.Ftp#connections)
5. [TroubleShooting](https://github.com/SharpeRAD/Cake.WebDeploy#troubleshooting)

## Implemented functionality

* Upload Files, including overwrite and automatic directory creation
* Delete Files
* FTP and FTPS

## Referencing

```csharp
#addin "nuget:?package=FluentFTP&version=28.0.5"
#addin Cake.Ftp
```

## Usage

```csharp
#addin "nuget:?package=FluentFTP&version=28.0.5"
#addin Cake.Ftp

Task("Upload-Auto-Connection")
    .Does(() => {
 
         var settings = new FtpSettings() {
            Username = "************",
            Password = "************",
        };

        var fileToUpload = File("test.txt");
        FtpUploadFile("www.myweb.com", "test/test2.txt", fileToUpload, settings);

    });

Task("Upload-Manual-Connection")
    .Does(() => {

        var settings = new FtpSettings() {
            Username = "************",
            Password = "************",
            AutoDetectConnectionSettings = false,
            EncryptionMode = FtpEncryptionMode.Explicit,
            SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
            DataConnectionType = FtpDataConnectionType.PASV
        };

        var fileToUpload = File("test.txt");
        FtpUploadFile("www.myweb.com", "test/test2.txt", fileToUpload, settings);

    });

Task("Delete-File")
    .Does(() => {
        Information("Deleting ...");

        var settings = new FtpSettings() {
            Username = "************",
            Password = "************"
        };

        FtpDeleteFile("www.myweb.com", "test/test2.txt", settings);

    });

Task("Upload-Directory")
    .Does(() => {
        Information("Uploading Directory ...");

        var settings = new FtpSettings() {
            Username = "************",
            Password = "************"
        };

        var directoryToUpload = Directory("./artifacts/");
        FtpUploadDirectory("www.myweb.com", "/httpdocs/", directoryToUpload, settings);

    });

RunTarget("Upload-Auto-Connection");

```

## Connections

By default it tries every possible combination of the FTP connection properties, and connects to the first successful profile.

To manually specify a specific connection set `AutoDetectConnectionSettings` to `false` and specify `EncryptionMode`, `SslProtocols` and `DataConnectionType`

## Troubleshooting

This addin simply wraps [FluentFTP](https://github.com/robinrodricks/FluentFTP), please check their excellent documentation and FAQs
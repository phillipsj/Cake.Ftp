using System;
using Cake.Core.IO;

namespace Cake.Ftp.Services
{
    public interface IFtpService {
        void UploadFile(Uri serverUri, IFile fileToUpload, string username, string password);
        void DeleteFile(Uri serverUri, string username, string password);
    }
}

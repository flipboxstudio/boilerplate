using System.IO;
using System.Security.Cryptography;
using System.Text;
using App.Options;
using Microsoft.AspNetCore.Http;

namespace App.Services
{
    public class Uploader
    {
        /// <summary>
        /// File to move.
        /// </summary>
        private readonly IFormFile _file;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="file"></param>
        public Uploader(IFormFile file)
        {
            _file = file;
        }

        /// <summary>
        /// Upload to path and given file name.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string UploadTo(string path, string fileName)
        {
            var absolutePath = string.Format(
                "{0}{1}{2}/{3}",
                AppConfig.Host,
                AppConfig.RelativeUploadPath.Replace("wwwroot/", ""),
                path,
                fileName
            );
            var fileSystemUploadPath = string.Format(
                "{0}{1}",
                AppConfig.FileSystemUploadPath,
                path
            );
            var saveTo = string.Format("{0}/{1}", fileSystemUploadPath, fileName);

            Directory.CreateDirectory(fileSystemUploadPath);

            using (var uploadedStream = new FileStream(saveTo, FileMode.Create))
            {
                _file.CopyTo(uploadedStream);

                return absolutePath;
            }
        }

        /// <summary>
        /// Get extension from file.
        /// </summary>
        /// <returns></returns>
        public string GetExtension()
        {
            var fileInfo = new FileInfo(_file.FileName);

            return fileInfo.Extension;
        }

        /// <summary>
        /// Calculate MD5 Sum from file.
        /// </summary>
        /// <returns></returns>
        public string CalculateHashSum()
        {
            using (var md5 = MD5.Create()) {
                using (var stream = _file.OpenReadStream()) {
                    var hash = md5.ComputeHash(stream);
                    var stringBuilder = new StringBuilder();

                    foreach (var character in hash)
                        stringBuilder.Append(character.ToString("X2"));

                    return stringBuilder.ToString();
                }
            }
        }
    }
}
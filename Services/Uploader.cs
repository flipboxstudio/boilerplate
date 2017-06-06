using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using App.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace App.Services
{
    public class Uploader
    {
        /// <summary>
        /// File to move.
        /// </summary>
        private readonly IFormFile _file;

        /// <summary>
        /// Application configuration.
        /// </summary>
        private readonly AppConfig _appConfig;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="file"></param>
        public Uploader(IFormFile file)
        {
            _file = file;
        }

        /// <summary>
        /// Application initializer.
        /// </summary>
        /// <param name="appConfig"></param>
        protected Uploader(IOptions<AppConfig> appConfig)
        {
            _appConfig = appConfig.Value;
        }

        /// <summary>
        /// Upload to path and given file name.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string UploadTo(string path, string fileName = null)
        {
            fileName = string.IsNullOrEmpty(fileName) ? GenerateFileName() : fileName;
            var url = UrlCombine(
                UrlCombine(
                    UrlCombine(
                        _appConfig.Host,
                        _appConfig.RelativeUploadPath.Replace("wwwroot/", "")
                    ),
                    path
                ),
                fileName
            );
            var fileSystemUploadPath = Path.Combine(_appConfig.AbsoluteUploadPath, path);
            var saveTo = Path.Combine(fileSystemUploadPath, fileName);

            Directory.CreateDirectory(fileSystemUploadPath);

            using (var uploadedStream = new FileStream(saveTo, FileMode.Create))
            {
                _file.CopyTo(uploadedStream);

                return url;
            }
        }

        /// <summary>
        /// Upload to root with generated filename.
        /// </summary>
        /// <returns></returns>
        public string Upload()
        {
            return UploadTo("/", GenerateFileName());
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
        /// Generate filename from MD5 and it's extension.
        /// </summary>
        /// <returns></returns>
        public string GenerateFileName()
        {
            return "{0}{1}".Format(new [] {
                CalculateHashSum(),
                GetExtension()
            });
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

        /// <summary>
        /// Combine two URL.
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <returns></returns>
        private string UrlCombine(string url1, string url2)
        {
            url1 = url1.TrimEnd('/');
            url2 = url2.TrimStart('/');

            return string.Format("{0}/{1}", url1, url2);
        }
    }
}

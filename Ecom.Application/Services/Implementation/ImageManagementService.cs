using Ecom.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Ecom.Application.Services.Implementation
{
    public class ImageManagementService : IImageManagementService
    {
        
        private readonly IFileProvider fileProvider;
        public ImageManagementService(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }


        /// <summary>
        /// Add a collection of images 
        /// </summary>
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            List<string> SaveImageSrc = new List<string>();

            var ImageDirectory = Path.Combine("wwwroot", "Images", src);

            if(Directory.Exists(ImageDirectory) is not true)
            {
                Directory.CreateDirectory(ImageDirectory);
            }

            foreach(var item in files)
            {
                if(item.Length > 0)
                {
                    //get iamge name
                    var ImageName = item.FileName;

                    var ImageSrc = $"Images/{src}/{ImageName}";

                    var root = Path.Combine(ImageDirectory, ImageName);

                    using(FileStream stream = new FileStream(root,FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(ImageSrc);
                }
            }
            return SaveImageSrc;
        }

        public void DeleteImageAsync(string src)
        {
            var info = fileProvider.GetFileInfo(src);

            if (info is null || !info.Exists || string.IsNullOrEmpty(info.PhysicalPath))
            {
                throw new FileNotFoundException($"The file '{src}' does not exist or its path is invalid.");
            }

            var root = info.PhysicalPath;

            File.Delete(root);
        }
    }
}

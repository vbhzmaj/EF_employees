using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEfCrud
{
    static class PhotoPath
    {
        public static string GetPictureFolder()
        {
            string currentFolder = Directory.GetCurrentDirectory();
            string photoPath = Path.Combine(currentFolder, "Pictures");
            return photoPath;
        }
        public static string GetPhotoPath(string photo)
        {
            string pictureFolder = GetPictureFolder();
            return Path.Combine(pictureFolder, photo);
        }
        public static string CreateDestination(string photoPath)
        {
            string photo = Path.GetFileName(photoPath);

            string photoWithoutExstension = Path.GetFileNameWithoutExtension(photo);
            string ekstension = Path.GetExtension(photo);
            string targetPath = GetPhotoPath(photo);
            int counter = 0;
            while (File.Exists(targetPath))
            {
                counter++;
                targetPath = GetPhotoPath(photoWithoutExstension + counter +
               ekstension);
            }
            return targetPath;
        }
    }
}

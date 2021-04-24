using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace MVCBookstoreProject.Helpers
{
    public class ImageConverter
    {
        public static byte[] ByteArrayFromPostedFile(HttpPostedFileBase file)
        {
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }
            return fileData;
        }

        public static Image ConvertToThumbnail(byte[] photo, int thumbWidth, int thumbHeight)
        {
            MemoryStream ms = new MemoryStream(photo);
            Image image = Image.FromStream(ms, true);
            Image imageThumb = image.GetThumbnailImage(thumbWidth, thumbHeight, () => false, IntPtr.Zero);
            return imageThumb;
        }
    }
}
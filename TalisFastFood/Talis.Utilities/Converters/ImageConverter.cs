using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace Talis.Utilities.Converters
{
    public static class ImageConverter
    {
        public static string ConvertImage(byte[] arrayImage)
        {
            string imreBase64Data = Convert.ToBase64String(arrayImage);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);

            return imgDataURL;
        }
    }
}

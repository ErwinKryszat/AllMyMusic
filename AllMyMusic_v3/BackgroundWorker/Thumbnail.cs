using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;


namespace AllMyMusic
{
    public static class Thumbnail
    {
        public static bool AbortThumbnailGeneration()
        {
            return false;
        }

        public static Image GetFolderImageSonos(string strFilename)
        {
            Image img2Scale = Image.FromFile(strFilename);

            int nImageWidth = img2Scale.Width;
            int nImageHeight = img2Scale.Height;

            if ((img2Scale.Height > 1024) || (img2Scale.Width > 1024))
            {
                double nScalePercentage = 953.0d / Math.Max(nImageWidth, nImageHeight);
                nImageWidth = (int)(nImageWidth * nScalePercentage);
                nImageHeight = (int)(nImageHeight * nScalePercentage);

                return GetThumbnail(img2Scale, nImageWidth, nImageHeight);
            }
            else
            {
                return img2Scale;
            }
        }

        

        public static Image GetThumbnail(string strFilename, int nScalePercentage)
        {
            Image img2Scale = Image.FromFile(strFilename);
            Image imgThumb = GetThumbnail(img2Scale, nScalePercentage);
            img2Scale.Dispose();  // cleanup
            return imgThumb;
        }

        public static Image GetThumbnail(Image imgFullSize, int nScalePercentage)
        {
            if (nScalePercentage < 1 || nScalePercentage > 99)
                throw new ArgumentException("Scale percentage must be between 1 and 99");

            int nImageWidth = imgFullSize.Width;
            int nImageHeight = imgFullSize.Height;
            nImageWidth = (int)((double)nImageWidth * ((double)nScalePercentage / 100.0));
            nImageHeight = (int)((double)nImageHeight * ((double)nScalePercentage / 100.0));
            return GetThumbnail(imgFullSize, nImageWidth, nImageHeight);
        }

        public static Image GetThumbnail(Image imgFullSize, int nWidth, int nHeight)
        {
            Image.GetThumbnailImageAbort cb = new Image.GetThumbnailImageAbort(AbortThumbnailGeneration);
            return imgFullSize.GetThumbnailImage(nWidth, nHeight, cb, IntPtr.Zero);
        }

        public static Image GetThumbnail(string strFilename, int nWidth, int nHeight)
        {
            Image newImage = null;
            FileInfo fi = new FileInfo(strFilename);
            if (fi.Length > 2000)
            {
                Image imgFullSize = Image.FromFile(strFilename);
                Image.GetThumbnailImageAbort cb = new Image.GetThumbnailImageAbort(AbortThumbnailGeneration);
                Image thumbNailImage = imgFullSize.GetThumbnailImage(nWidth, nHeight, cb, IntPtr.Zero);

                MemoryStream ms = new MemoryStream();
                thumbNailImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                ms.Position = 0;

                newImage = Image.FromStream(ms);
            }
            
            return newImage;
            
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Metadata.ID3
{
    public class CoverImages
    {
        /// <summary>
        /// Path of the fontal cover sleeve
        /// </summary>
        private String frontImage = String.Empty;
        public String FrontImage
        {
            get { return frontImage; }
            set { frontImage = value; }
        }


        /// <summary>
        /// Path of the back cover sleeve
        /// </summary>
        private String backImage = String.Empty;
        public String BackImage
        {
            get { return backImage; }
            set { backImage = value; }
        }


        /// <summary>
        /// Path of the thumbnail cover sleeve
        /// </summary>
        private String stampImage = String.Empty;
        public String StampImage
        {
            get { return stampImage; }
            set { stampImage = value; }
        }

        public CoverImages(FileInfo[] MultimediaFiles)
        {
            if (MultimediaFiles.Length == 0)
            {
                return;
            }

            foreach (FileInfo CoverFile in MultimediaFiles)
            {
                String extension = Path.GetExtension(CoverFile.Name).ToLower();
                if ((extension == ".jpg") || (extension == ".png"))
                {
                    if (CoverFile.Name.ToLower().IndexOf("cover") >= 0)
                    {
                        frontImage = CoverFile.Name;
                        continue;
                    }

                    if (CoverFile.Name.ToLower().IndexOf("frontal") >= 0)
                    {
                        frontImage = CoverFile.Name;
                        continue;
                    }

                    if (CoverFile.Name.ToLower().IndexOf("folder") >= 0)
                    {
                        frontImage = CoverFile.Name;
                        continue;
                    }

                    if (CoverFile.Name.ToLower().IndexOf("large") >= 0)
                    {
                        frontImage = CoverFile.Name;
                        continue;
                    }

                    if (CoverFile.Name.ToLower().IndexOf("stamp") >= 0)
                    {
                        stampImage = CoverFile.Name;
                        continue;
                    }

                    if (CoverFile.Name.ToLower().IndexOf("small") >= 0)
                    {
                        stampImage = CoverFile.Name;
                        continue;
                    }

                    if (CoverFile.Name.ToLower().IndexOf("back") >= 0)
                    {
                        backImage = CoverFile.Name;
                        continue;
                    }


                    if (CoverFile.Name.Length >= 5)
                    {
                        String lastCharacter = CoverFile.Name.Substring(CoverFile.Name.Length - 5, 1).ToLower();
                        if (lastCharacter == "a")
                        {
                            frontImage = CoverFile.Name;
                            continue;
                        }
                        if (lastCharacter == "b")
                        {
                            backImage = CoverFile.Name;
                            continue;
                        }
                        if (lastCharacter == "f")
                        {
                            frontImage = CoverFile.Name;
                            continue;
                        }
                    }

                    //if (frontImage == String.Empty)
                    //{
                    //    frontImage = CoverFile.Name;
                    //}
                }   
            }
        }
    }
}

/*
This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

Copyright (C) 2005-2009  Cyber Sinh (http://www.luminescence-software.org/)
*/

using System;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;     // reference PresentationCore



namespace Metadata.ID3
{
   /// <summary>
   /// Classe de lecture / écriture des images dans les fichiers FLAC.
   /// </summary>
   public class ID3PictureFrame
   {
      private ID3PictureType pictureType = ID3PictureType.FrontCover;
      private string description;
      private BitmapFrame bitmap;

      /// <summary>
      /// Constructeur de la classe ID3Picture.
      /// </summary>
      /// <param name="data">Données METADATA_BLOCK_PICTURE sous la forme d'un tableau d'octets</param>
      public ID3PictureFrame(byte[] data)
      {
         MemoryStream ms = new MemoryStream(data, 0, data.Length, false);
         BinaryReader br = new BinaryReader(ms);
         byte[] buffer;

         pictureType = (ID3PictureType)br.ReadBigEndianInt32();

         int mimeSize = br.ReadBigEndianInt32();
         ms.Position += mimeSize;

         int descSize = br.ReadBigEndianInt32();
         if (descSize > 0)
         {
            buffer = br.ReadBytes(descSize);
            UTF8Encoding utf8 = new UTF8Encoding();
            description = utf8.GetString(buffer);
         }

         ms.Position += 16;
         int pictSize = br.ReadBigEndianInt32();
         buffer = br.ReadBytes(pictSize);
         BitmapDecoder bd = BitmapDecoder.Create(new MemoryStream(buffer, 0, buffer.Length), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnDemand);
         bitmap = bd.Frames[0];
      }

      /// <summary>
      /// Constructeur de la classe ID3Picture.
      /// </summary>
      /// <param name="data">Bitmap constituant l'image</param>
      public ID3PictureFrame(BitmapFrame data)
      {
         bitmap = data;
      }

      /// <summary>
      /// Constructeur de la classe ID3Picture.
      /// </summary>
      /// <param name="data">Bitmap constituant l'image</param>
      /// <param name="pictType">Type d'image selon la norme ID3v2 (frame APIC)</param>
      public ID3PictureFrame(BitmapFrame data, ID3PictureType pictType)
      {
         bitmap = data;
         pictureType = pictType;
      }

      /// <summary>
      /// Type d'image selon la norme ID3v2 (frame APIC).
      /// </summary>
      public ID3PictureType PictureType
      {
         get { return pictureType; }
         set { pictureType = value; }
      }

      /// <summary>
      /// Description de l'image.
      /// </summary>
      public string Description
      {
         get { return description; }
         set { description = value; }
      }

      /// <summary>
      /// Bitmap constituant l'image.
      /// </summary>
      public BitmapFrame Picture
      {
         get { return bitmap; }
         set { bitmap = value; }
      }

      /// <summary>
      /// Retourne les données de l'image à la norme ID3 (frame APIC).
      /// </summary>
      /// <returns>Données de la frame sous la forme d'un tableau d'octets</returns>
      public byte[] ToArray()
      {
         MemoryStream ms = new MemoryStream();

         byte[] buffer = ((int)pictureType).ToBigEndian();
         ms.Write(buffer, 0, buffer.Length);

         ASCIIEncoding ascii = new ASCIIEncoding();
         byte[] binMime = ascii.GetBytes(bitmap.Decoder.CodecInfo.MimeTypes);
         buffer = binMime.Length.ToBigEndian();
         ms.Write(buffer, 0, buffer.Length);
         ms.Write(binMime, 0, binMime.Length);

         if (String.IsNullOrEmpty(description))
         {
            ms.Write(new byte[] { 0, 0, 0, 0 }, 0, 4);
         }
         else
         {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] binDesc = utf8.GetBytes(description);
            buffer = binDesc.Length.ToBigEndian();
            ms.Write(buffer, 0, buffer.Length);
            ms.Write(binMime, 0, binMime.Length);
         }

         buffer = bitmap.PixelWidth.ToBigEndian();
         ms.Write(buffer, 0, buffer.Length);

         buffer = bitmap.PixelHeight.ToBigEndian();
         ms.Write(buffer, 0, buffer.Length);

         int bitsPerPixel = bitmap.Format.BitsPerPixel;
         int nbIndexedColors = 0;
         if (bitmap.Palette != null)
            nbIndexedColors = bitmap.Palette.Colors.Count;

         buffer = bitsPerPixel.ToBigEndian();
         ms.Write(buffer, 0, buffer.Length);

         buffer = nbIndexedColors.ToBigEndian();
         ms.Write(buffer, 0, buffer.Length);

         MemoryStream image = new MemoryStream();
         BitmapEncoder be = BitmapEncoder.Create(bitmap.Decoder.CodecInfo.ContainerFormat);
         be.Frames.Add(bitmap);
         be.Save(image);
         byte[] binPicture = image.ToArray();
         buffer = binPicture.Length.ToBigEndian();
         ms.Write(buffer, 0, buffer.Length);
         ms.Write(binPicture, 0, binPicture.Length);

         return ms.ToArray();
      }
   }
}
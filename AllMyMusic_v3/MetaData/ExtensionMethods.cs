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
using System.Collections.Generic;
using System.IO;

namespace Metadata.ID3
{
   /// <summary>
   /// Méthodes d'extension diverses.
   /// </summary>
   internal static class ExtensionMethods
   {

      /// <summary>
      /// Copie les valeurs d'un dictionnaire de tags dans un nouveau dictionnaire.
      /// </summary>
      /// <param name="tags">Collection dont les valeurs seront copiées</param>
      /// <returns>Nouvelle collection contenant les valeurs copiées</returns>
      public static SortedList<string, List<string>> GetNewTagsCollection(this SortedList<string, List<string>> tags)
      {
         SortedList<string, List<string>> newTags = new SortedList<string, List<string>>();

         foreach (KeyValuePair<string, List<string>> kvp in tags)
         {
            newTags.Add(kvp.Key, new List<string>(kvp.Value));
         }

         return newTags;
      }
    
      /// <summary>
      /// Vérifie si le contenu de deux tableaux d'octets est identique.
      /// </summary>
      /// <param name="array1">Premier tableau</param>
      /// <param name="array2">Second tableau</param>
      /// <returns>True si les deux tableaux sont identiques, sinon False</returns>
      public static bool ArrayEquals(this byte[] array1, byte[] array2)
      {
         if (array1.Length != array2.Length) return false;

         for (int i = 0; i < array1.Length; i++)
         {
            if (array1[i] != array2[i]) return false;
         }

         return true;
      }

      /// <summary>
      /// Vérifie si le contenu de deux tableaux d'octets est identique.
      /// </summary>
      /// <param name="array1">Premier tableau</param>
      /// <param name="array2">Second tableau</param>
      /// <param name="offset">Octet à partir duquel la comparaison doit commencer</param>
      /// <param name="count">Nombre d'octets à comparer</param>
      /// <returns>True si les deux tableaux sont identiques, sinon False</returns>
      public static bool ArrayEquals(this byte[] array1, byte[] array2, int offset, int count)
      {
         if (array1.Length < count + offset || array2.Length < count) return false;

         for (int i = 0; i < count; i++)
         {
            if (array1[i + offset] != array2[i]) return false;
         }

         return true;
      }

      /// <summary>
      /// Lit un entier (big endian) signé de 4 octets à partir du flux actuel et avance la position actuelle du flux de 4 octets.
      /// </summary>
      /// <param name="reader">Un binary reader</param>
      /// <returns>Un entier sur 32 bits</returns>
      public static int ReadBigEndianInt32(this BinaryReader reader)
      {
         byte[] bytes = new byte[4];
         for (int i = 3; i > -1; i--)
         {
            bytes[i] = reader.ReadByte();
         }

         return BitConverter.ToInt32(bytes, 0);
      }

      /// <summary>
      /// Retourne un tableau d'octets contenant l'entier sur 32 bits codé avec une primauté des octets de poids fort (big endian).
      /// </summary>
      /// <param name="int32">Un entier sur 32 bits</param>
      /// <returns>Un tableau d'octets</returns>
      public static byte[] ToBigEndian(this int int32)
      {
         byte[] buffer = new byte[4];
         buffer[0] = (byte)(int32 >> 24);
         buffer[1] = (byte)(int32 >> 16);
         buffer[2] = (byte)(int32 >> 8);
         buffer[3] = (byte)(int32);

         return buffer;
      }

      /// <summary>
      /// Découpe un tableau d'octets en tableau de tableau d'octets.
      /// </summary>
      /// <param name="array">Tableau à découper</param>
      /// <param name="segment">Taille des segments</param>
      /// <returns>Tableau de tableau d'octets</returns>
      public static byte[][] Split(this byte[] array, int segment)
      {
         int segments = (int)Math.Ceiling(array.Length / (double)segment);
         byte[][] tab = new byte[segments][];
         int offset = 0;

         for (int i = 0; i < segments; i++)
         {
            int sizeRemaining = array.Length - (i * segment);
            if (sizeRemaining < segment)
               segment = sizeRemaining;

            MemoryStream ms = new MemoryStream(segment);
            ms.Write(array, offset, segment);
            tab[i] = ms.ToArray();

            offset += segment;
         }

         return tab;
      }
   }
}
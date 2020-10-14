using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using AllMyMusic_v3;


namespace Metadata.ID3
{
    /// <summary>
    /// This class is used to:
    /// Collection for ID3Tags
    /// </summary>
    public class Id3TagCollection : List<Id3Tag>
    {
        public Id3TagCollection()
        {

        }

        /// <summary>
        /// Find a tag in the collection specified by the tagType
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns>ID3Tag if the tag was found</returns>
        public Id3Tag FindTagByType(TagType tagType)
        {
            foreach (Id3Tag tag in this)
            {
                if (tag.TagType == tagType)
                {
                    return tag;
                }
            }
            return null;
        }

        /// <summary>
        /// Delete a tag from the collection specified by the tagType
        /// </summary>
        /// <param name="tagType"></param>
        public void DeleteTagByType(TagType tagType)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].TagType == tagType)
                {
                    this.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Delete a tag from the collection specified by the tagType and a tagValue
        /// This is used when multiple instances of the same tagType exist. 
        /// </summary>
        /// <param name="tagType"></param>
        /// <param name="tagValue"></param>
        public void DeleteTagByValue(TagType tagType, String tagValue)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].TagType == tagType)
                {
                    if (this[i].StringValue == tagValue)
                    {
                        this.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Replace the image in an APIC tag that matches the picType
        /// </summary>
        /// <param name="coverImage"></param>
        /// <param name="picType"></param>
        public void UpdatePicture(Image coverImage, PictureType pictureType)
        {
            APIC pictureTag = null;
            Int32 foundPosition = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].TagType == TagType.APIC)
                {
                    if (((APIC)this[i]).PictureType == pictureType)
                    {
                        pictureTag = (APIC)this[i];
                        foundPosition = i;
                        break;
                    }
                }
            }

            if (pictureTag != null)
            {
                ((APIC)this[foundPosition]).CoverImage = coverImage;
            }
            else
            {
                pictureTag = new APIC(coverImage, pictureType);
                this.Add(pictureTag);
            }
        }

        /// <summary>
        /// Change, add or delete an multiple ID3Tags according to information provided by the song object
        /// </summary>
        /// <param name="song"></param>
        public void UpdateTags(ChangedPropertiesList changedProperties)
        {
            for (int i = 0; i < changedProperties.Count; i++)
            {
                KeyValuePair<string, object> item = changedProperties[i];
                switch (item.Key)
                {
                    case "AlbumName":
                        UpdateTag(TagType.TALB, (String)item.Value);
                        break;
                    case "AlbumSortName":
                        UpdateTag(TagType.TSOA, (String)item.Value);
                        break;
                    case "BandName":
                        UpdateTag(TagType.TPE2, (String)item.Value);
                        break;
                    case "BandSortName":
                        UpdateTag(TagType.TSOP, (String)item.Value);
                        break;
                    case "Comment":
                        UpdateTag(TagType.COMM, (String)item.Value);
                        break;
                    case "ComposerName":
                        UpdateTag(TagType.TCOM, (String)item.Value);
                        break;
                    case "ConductorName":
                        UpdateTag(TagType.TPE3, (String)item.Value);
                        break;
                    case "Country":
                        UpdateCountry((String)item.Value);
                        break;
                    case "Encoder":
                        UpdateTag(TagType.TENC, (String)item.Value);
                        break;
                    case "Genre":
                        UpdateTag(TagType.TCON, (String)item.Value);
                        break;
                    case "Language":
                        UpdateTag(TagType.TLAN, (String)item.Value);
                        break;
                    case "LeadPerformer":
                        UpdateTag(TagType.TPE1, (String)item.Value);
                        break;
                    case "MilliSeconds":
                        UpdateTag(TagType.TLEN, (String)item.Value);
                        break;
                    case "Rating":
                        UpdateRating(Convert.ToInt32(item.Value));
                        break;
                    case "SongTitle":
                        UpdateTag(TagType.TIT2, (String)item.Value);
                        break;
                    case "Track":
                        UpdateTag(TagType.TRCK, (String)item.Value);
                        break;
                    case "Year":
                        UpdateTag(TagType.TYER, (String)item.Value);
                        break;
                    case "WebsiteCommercial":
                        UpdateTag(TagType.WCOM, (String)item.Value);
                        break;
                    case "WebsiteCopyright":
                        UpdateTag(TagType.WCOP, (String)item.Value);
                        break;
                    case "WebsiteAudioFile":
                        UpdateTag(TagType.WOAF, (String)item.Value);
                        break;
                    case "WebsiteArtist":
                        UpdateTag(TagType.WOAR, (String)item.Value);
                        break;
                    case "WebsiteAudioSource":
                        UpdateTag(TagType.WOAS, (String)item.Value);
                        break;
                    case "WebsiteInternetRadio":
                        UpdateTag(TagType.WORS, (String)item.Value);
                        break;
                    case "WebsitePayment":
                        UpdateTag(TagType.WPAY, (String)item.Value);
                        break;
                    case "WebsitePublisher":
                        UpdateTag(TagType.WPUB, (String)item.Value);
                        break;
                    case "WebsiteURL":
                        UpdateTag(TagType.WXXX, (String)item.Value);
                        break;
                    default:
                        break;
                }
            }            
        }

        /// <summary>
        /// Change, add or delete an multiple ID3Tags according to information provided by the song object
        /// </summary>
        /// <param name="song"></param>
        public void UpdateTags(SongItem song)
        {
            UpdateTag(TagType.TALB, song.AlbumName);
            UpdateTag(TagType.TSOA, song.AlbumSortName);
            UpdateTag(TagType.TPE2, song.BandName);
            UpdateTag(TagType.TSOP, song.BandSortName);
            UpdateTag(TagType.COMM, song.Comment);
            UpdateTag(TagType.TCOM, song.ComposerName);
            UpdateTag(TagType.TPE3, song.ConductorName);
            UpdateTag(TagType.TCON, song.Genre);
            UpdateTag(TagType.TLAN, song.Language);
            UpdateTag(TagType.TPE1, song.LeadPerformer);
            UpdateTag(TagType.TIT2, song.SongTitle);
            UpdateTag(TagType.TRCK, song.Track);
            UpdateTag(TagType.TYER, song.Year);
            UpdateTag(TagType.TLEN, song.MilliSeconds);

            UpdateCountry(song.Country);
            UpdateRating(song.Rating);

            UpdateTag(TagType.WCOM, song.WebsiteCommercial);
            UpdateTag(TagType.WCOP, song.WebsiteCopyright);
            UpdateTag(TagType.WOAF, song.WebsiteAudioFile);
            UpdateTag(TagType.WOAR, song.WebsiteArtist);
            UpdateTag(TagType.WOAS, song.WebsiteAudioSource);
            UpdateTag(TagType.WORS, song.WebsiteInternetRadio);
            UpdateTag(TagType.WPAY, song.WebsitePayment);
            UpdateTag(TagType.WPUB, song.WebsitePublisher);
            UpdateTag(TagType.WXXX, song.WebsiteUser);
        }

        /// <summary>
        /// Change, add or delete one ID3Tag specified by the tagType and a tagValue
        /// </summary>
        /// <param name="tagTpye"></param>
        /// <param name="tagValue"></param>
        public void UpdateTag(TagType tagTpye, String tagValue)
        {
            Id3Tag tag = FindTagByType(tagTpye);
            if (tag != null)
            {
                if (tag.IsString)
                {
                    if (String.IsNullOrEmpty(tagValue) == false)
                    {
                        // Update existing Tag
                        tag.StringValue = tagValue;
                    }
                    else
                    {
                        // Remove existing Tag
                        DeleteTagByType(tagTpye);
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(tagValue) == false)
                {
                    // Add new tag
                    Byte[] tagData = UnicodeData.EncodeStringValue(tagValue, true, CodingType.ISO_8859_1);
                    tag = new Id3Tag(tagTpye.ToString(), tagData);
                    this.Add(tag);
                }
            }
        }

        /// <summary>
        /// get the image object for the requested picture if an APIC tag exists in the collection
        /// </summary>
        /// <param name="picType"></param>
        /// <returns>Image object for the requested picture if an APIC tag exists in the collection</returns>
        public Image GetAttachedPicture(PictureType pictureType)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].TagType == TagType.APIC)
                {
                    if (((APIC)this[i]).PictureType == pictureType)
                    {
                        APIC pictureTag = (APIC)this[i];
                        return pictureTag.CoverImage;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Change or add the POPM tag
        /// </summary>
        /// <param name="rating"></param>
        private void UpdateRating(Int32 rating)
        {
            Id3Tag tag = FindTagByType(TagType.POPM);
            if (tag != null)
            {
                if (rating > 0)
                {
                    // Update existing tag
                    ((POPM)tag).Rating = rating;
                }
                else
                {
                    // Remove existing Tag
                    DeleteTagByType(TagType.POPM);
                }
            }
            else
            {
                if (rating > 0)
                {
                    // Add new tag
                    tag = new POPM(rating);
                    this.Add(tag);
                }
            }
        }

        /// <summary>
        /// Change, add or delete the PRIV tag that belongs to AllMyMusic. Other PRIV tags are not affected by this function.
        /// </summary>
        /// <param name="country"></param>
        private void UpdateCountry(String country)
        {
            PRIV countryTag = null;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].TagType == TagType.PRIV)
                {
                    if ((this[i].StringValue.Length >= 10) && (this[i].StringValue.Substring(0, 10) == "AllMyMusic"))
                    {
                        countryTag = (PRIV)this[i];
                        break;
                    }
                    if ((this[i].StringValue.Length >= 8) && (this[i].StringValue.Substring(0, 8) == "AllMyMp3"))
                    {
                        countryTag = (PRIV)this[i];
                        break;
                    }
                }
            }

            if (countryTag != null)
            {
                if (String.IsNullOrEmpty(country) == false)
                {
                    // Update existing Tag
                    ((PRIV)countryTag).Country = country;
                }
                else
                {
                    // Remove existing Tag
                    DeleteTagByType(TagType.PRIV);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(country) == false)
                {
                    countryTag = new PRIV(country);
                    this.Add(countryTag);
                }
            }
        }


        /// <summary>
        /// Calculate the required size for all ID3V2 tags
        /// </summary>
        /// <returns></returns>
        public Int32 GetRequiredSize()
        {
            Int32 requiredSize = 0;    // 10 Byte for the ID3V2 header
            foreach (Id3Tag tag in this)
            {
                requiredSize += (tag.TotalSize);
            }
            return requiredSize;
        }
    }
}

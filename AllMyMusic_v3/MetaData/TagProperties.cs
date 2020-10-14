using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Metadata.ID3
{
    /// <summary>
    /// CBR = Constant Bitrate; VBR  = Variable Bitrate
    /// </summary>
    public class TagProperty 
    {
        public String Name;
        public TagType TagType;
        public Boolean IsStandardTag;
        public Boolean IsStringValue;
        public Boolean AllowEncoding;
        public String Description;

        public TagProperty(String name, TagType tagType, Boolean isStandand, Boolean isString, Boolean encoding, String desc)
        {
            this.Name = name;
            this.TagType = tagType;
            this.IsStandardTag = isStandand;
            this.IsStringValue = isString;
            this.AllowEncoding = encoding;
            this.Description = desc;
        }
    }


    public static class TagProperties
    {
        public static String GetTagDescription(String tagName)
        {
            for (int i = 0; i < Item.Length; i++)
            {
                if (Item[i].Name == tagName)
                {
                    return Item[i].Description;
                }
            }
            return "This frame type is not supported in ID3 v2.4: " + tagName;
        }

        public static TagProperty[] Item = new TagProperty[93] 
        {
            //                                      Stdrd, String, Enc
            new TagProperty ( "AENC", TagType.AENC, false, true , true , "Audio encryption" ), 
            new TagProperty ( "APIC", TagType.APIC, true , false, false, "Attached picture" ), 
            new TagProperty ( "ASPI", TagType.ASPI, false, false, false, "Audio seek point index" ), 
            new TagProperty ( "COMM", TagType.COMM, false, true , true , "Comments" ), 
            new TagProperty ( "COMR", TagType.COMR, false, false, false, "Commercial frame" ), 
            new TagProperty ( "ENCR", TagType.ENCR, false, false, false, "Encryption method registration" ), 
            new TagProperty ( "EQU2", TagType.EQU2, false, false, false, "Equalisation (2)" ), 
            new TagProperty ( "ETCO", TagType.ETCO, false, false, false, "Event timing codes" ), 
            new TagProperty ( "GEOB", TagType.GEOB, false, false, false, "General encapsulated object" ), 
            new TagProperty ( "GRID", TagType.GRID, false, false, false, "Group identification registration" ), 
            new TagProperty ( "LINK", TagType.LINK, false, false, false, "Linked information" ), 
            new TagProperty ( "MCDI", TagType.MCDI, false, false, false, "Music CD identifier" ), 
            new TagProperty ( "MLLT", TagType.MLLT, false, false, false, "MPEG location lookup table" ), 
            new TagProperty ( "NCON", TagType.NCON, false, false, false, "MusicMatch Stuff ),   Binary Data" ), 
            new TagProperty ( "OWNE", TagType.OWNE, false, false, false, "Ownership frame" ), 
            new TagProperty ( "PCNT", TagType.PCNT, false, false, false, "Play counter" ), 
            new TagProperty ( "POPM", TagType.POPM, true , false, false, "Popularimeter" ), 
            new TagProperty ( "POSS", TagType.POSS, false, false, false, "Position synchronisation frame" ), 
            new TagProperty ( "PRIV", TagType.PRIV, true , false, false, "Private frame ),  Country for AllMyMusic" ), 
            new TagProperty ( "RGAD", TagType.RGAD, false, false, false, "Replay Gain Adjustment" ), 
            new TagProperty ( "RBUF", TagType.RBUF, false, false, false, "Recommended buffer size" ), 
            new TagProperty ( "RVAD", TagType.RVAD, false, false, false, "Relative volume adjustment" ), 
            new TagProperty ( "RVA2", TagType.RVA2, false, false, false, "Relative volume adjustment (2)" ), 
            new TagProperty ( "RVRB", TagType.RVRB, false, false, false, "Reverb" ), 
            new TagProperty ( "SEEK", TagType.SEEK, false, false, false, "Seek frame" ), 
            new TagProperty ( "SIGN", TagType.SIGN, false, false, false, "Signature frame" ), 
            new TagProperty ( "SYLT", TagType.SYLT, false, false, false, "Synchronised lyric/text" ), 
            new TagProperty ( "SYTC", TagType.SYTC, false, false, false, "Synchronised tempo codes" ), 
            new TagProperty ( "TALB", TagType.TALB, true , true , true , "Album/Movie/Show title" ), 
            new TagProperty ( "TBPM", TagType.TBPM, true , true , false, "BPM (beats per minute)" ), 
            new TagProperty ( "TCMP", TagType.TCMP, false, true , false, "Part of a compilation" ), 
            new TagProperty ( "TCOM", TagType.TCOM, true , true , true , "Composer" ), 
            new TagProperty ( "TCON", TagType.TCON, true , true , true , "Content type" ), 
            new TagProperty ( "TCOP", TagType.TCOP, false, true , true , "Copyright message" ), 
            new TagProperty ( "TDAT", TagType.TDAT, false, false, false, "Date of recording" ), 
            new TagProperty ( "TDEN", TagType.TDEN, false, true , true , "Encoding time" ), 
            new TagProperty ( "TDLY", TagType.TDLY, false, true , true , "PlaylistItemCollection delay" ), 
            new TagProperty ( "TDOR", TagType.TDOR, false, true , true , "Original release time" ), 
            new TagProperty ( "TDRC", TagType.TDRC, false, true , true , "Recording time" ), 
            new TagProperty ( "TDRL", TagType.TDRL, false, true , true , "Release time" ), 
            new TagProperty ( "TDTG", TagType.TDTG, false, false, false, "Tagging time" ), 
            new TagProperty ( "TENC", TagType.TENC, false, true , true , "Encoded by" ), 
            new TagProperty ( "TEXT", TagType.TEXT, true , true , true , "Lyricist/Text writer" ), 
            new TagProperty ( "TFLT", TagType.TFLT, false, true , true , "File type" ), 
            new TagProperty ( "TIME", TagType.TIME, false, false, false, "Time of recording" ), 
            new TagProperty ( "TIPL", TagType.TIPL, true , true , true , "Involved people list" ), 
            new TagProperty ( "TIT1", TagType.TIT1, true , true , true , "Content group description" ), 
            new TagProperty ( "TIT2", TagType.TIT2, true , true , true , "Title/songname/content description" ), 
            new TagProperty ( "TIT3", TagType.TIT3, true , true , true , "Subtitle/Description refinement" ), 
            new TagProperty ( "TKEY", TagType.TKEY, false, true , true , "Initial key" ), 
            new TagProperty ( "TLAN", TagType.TLAN, true , true , true , "Language(s)" ), 
            new TagProperty ( "TLEN", TagType.TLEN, true , true , true , "Length" ), 
            new TagProperty ( "TMCL", TagType.TMCL, false, true , true , "Musician credits list" ), 
            new TagProperty ( "TMED", TagType.TMED, false, true , true , "Media type" ), 
            new TagProperty ( "TMOO", TagType.TMOO, true , true , true , "Mood" ), 
            new TagProperty ( "TOAL", TagType.TOAL, false, true , true , "Original album/movie/show title" ), 
            new TagProperty ( "TOFN", TagType.TOFN, false, true , true , "Original filename" ), 
            new TagProperty ( "TOLY", TagType.TOLY, false, true , true , "Original lyricist(s)/text writer(s)" ), 
            new TagProperty ( "TOPE", TagType.TOPE, false, true , true , "Original artist(s)/performer(s)" ), 
            new TagProperty ( "TORY", TagType.TORY, false, true , true , "Original release year" ), 
            new TagProperty ( "TOWN", TagType.TOWN, false, true , true , "File owner/licensee" ), 
            new TagProperty ( "TPE1", TagType.TPE1, true , true , true , "Lead performer(s)/Soloist(s)" ), 
            new TagProperty ( "TPE2", TagType.TPE2, true , true , true , "Band/orchestra/accompaniment" ), 
            new TagProperty ( "TPE3", TagType.TPE3, true , true , true , "Conductor/performer refinement" ), 
            new TagProperty ( "TPE4", TagType.TPE4, true , true , true , "Interpreted, remixed, or otherwise modified by" ), 
            new TagProperty ( "TPOS", TagType.TPOS, false, true , true , "Part of a set" ), 
            new TagProperty ( "TPRO", TagType.TPRO, false, true , true , "Produced notice" ), 
            new TagProperty ( "TPUB", TagType.TPUB, false, true , true , "Publisher" ), 
            new TagProperty ( "TRCK", TagType.TRCK, true , true , true , "Track number/Position in set" ), 
            new TagProperty ( "TRSN", TagType.TRSN, false, true , true , "Internet radio station name" ), 
            new TagProperty ( "TRSO", TagType.TRSO, false, true , true , "Internet radio station owner" ), 
            new TagProperty ( "TSIZ", TagType.TSIZ, false, false, false, "Size of audio file" ), 
            new TagProperty ( "TSOA", TagType.TSOA, true, true , true , "Album sort order" ), 
            new TagProperty ( "TSOP", TagType.TSOP, true, true , true , "Performer sort order" ), 
            new TagProperty ( "TSOT", TagType.TSOT, false, true , true , "Title sort order" ), 
            new TagProperty ( "TSRC", TagType.TSRC, false, true , true , "(international) standard recording code" ), 
            new TagProperty ( "TSSE", TagType.TSSE, false, true, false, "Software/Hardware and settings used for encoding" ), 
            new TagProperty ( "TSST", TagType.TSST, false, true , true , "Set subtitle" ), 
            new TagProperty ( "TXXX", TagType.TXXX, false, false, false, "User defined text information frame" ), 
            new TagProperty ( "TYER", TagType.TYER, true , true , false, "Year of recording" ), 
            new TagProperty ( "UFID", TagType.UFID, false, false, false, "Unique file identifier" ), 
            new TagProperty ( "USER", TagType.USER, false, false, false, "USER Terms of use" ), 
            new TagProperty ( "USLT", TagType.USLT, false, false, false, "Unsynchronised lyric/text transcription" ), 
            new TagProperty ( "WCOM", TagType.WCOM, false, true , false, "Commercial information" ), 
            new TagProperty ( "WCOP", TagType.WCOP, false, true , false, "Copyright/Legal information" ), 
            new TagProperty ( "WOAF", TagType.WOAF, false, true , false, "Official audio file webpage" ), 
            new TagProperty ( "WOAR", TagType.WOAR, true , true , false, "Official artist/performer webpage" ), 
            new TagProperty ( "WOAS", TagType.WOAS, false, true , false, "Official audio source webpage" ), 
            new TagProperty ( "WORS", TagType.WORS, false, true , false, "Official Internet radio station homepage" ), 
            new TagProperty ( "WPAY", TagType.WPAY, false, true , false, "Payment" ), 
            new TagProperty ( "WPUB", TagType.WPUB, false, true , false, "Publishers official webpage" ), 
            new TagProperty ( "WXXX", TagType.WXXX, false, true , false, "User defined URL link frame" ), 
            new TagProperty ( "XSOP", TagType.XSOP, false, true , false, "Experimental, Sort order, Performer" ) 
        };
    }
}

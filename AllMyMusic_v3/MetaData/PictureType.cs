using System;
using System.Collections.Generic;
using System.Text;

namespace Metadata.ID3
{
    public enum PictureType
    {
        None = 0,
        Icon32x32 = 1,
        con = 2,
        FrontalCover = 3,
        BackCover = 4,
        LeafletPage = 5,
        Media = 6
    }

    /// <summary>
    /// Type d'image selon la norme ID3v2 (frame APIC).
    /// </summary>
    public enum ID3PictureType
    {
        /// <summary>
        /// Other
        /// </summary>
        /// <remarks>
        /// 'Others' are reserved and should not be used.
        /// </remarks>
        Other = 0,
        /// <summary>
        /// 32x32 pixels 'file icon' (PNG only)
        /// </summary>
        /// <remarks>
        /// There may only be one of this picture type in a file.
        /// </remarks>
        PNG32PixelsFileIcon = 1,
        /// <summary>
        /// Other file icon
        /// </summary>
        /// <remarks>
        /// There may only be one of this picture type in a file.
        /// </remarks>
        OtherFileIcon = 2,
        /// <summary>
        /// Cover (front)
        /// </summary>
        FrontCover = 3,
        /// <summary>
        /// Cover (back)
        /// </summary>
        BackCover = 4,
        /// <summary>
        /// Leaflet page
        /// </summary>
        LeafletPage = 5,
        /// <summary>
        /// Media (e.g. label side of CD)
        /// </summary>
        Media = 6,
        /// <summary>
        /// Lead artist, lead performer or soloist
        /// </summary>
        LeadArtist = 7,
        /// <summary>
        /// Artist or performer
        /// </summary>
        Artist = 8,
        /// <summary>
        /// Conductor
        /// </summary>
        Conductor = 9,
        /// <summary>
        /// Band or orchestra
        /// </summary>
        Orchestra = 10,
        /// <summary>
        /// Composer
        /// </summary>
        Composer = 11,
        /// <summary>
        /// Lyricist or text writer
        /// </summary>
        Lyricist = 12,
        /// <summary>
        /// Recording location
        /// </summary>
        RecordingLocation = 13,
        /// <summary>
        /// During recording
        /// </summary>
        DuringRecording = 14,
        /// <summary>
        /// During performance
        /// </summary>
        DuringPerformance = 15,
        /// <summary>
        /// Movie or video screen capture
        /// </summary>
        VideoScreenCapture = 16,
        /// <summary>
        /// A bright coloured fish
        /// </summary>
        BrightColouredFish = 17,
        /// <summary>
        /// Illustration
        /// </summary>
        Illustration = 18,
        /// <summary>
        /// Band or artist logotype
        /// </summary>
        ArtistLogo = 19,
        /// <summary>
        /// Publisher or studio logotype
        /// </summary>
        StudioLogo = 20
    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic_v3
{
    /// <summary>
    /// Specifies from what kind of data source the song comes.
    /// Used to determine what code shall be used to stream and play the song
    /// </summary>
    public enum SourceMediaType
    {
        Database,
        DiskFile,
        AudioCD,
        WebStream
    }
}

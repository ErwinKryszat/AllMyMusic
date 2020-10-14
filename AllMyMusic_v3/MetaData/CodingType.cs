using System;
using System.Collections.Generic;
using System.Text;

namespace Metadata.ID3
{
    /// <summary>
    /// Character encoding. Used to determine how a series of bytes is intepreted as readable text
    /// </summary>
    public enum CodingType
    {
        ISO_8859_1 = 0,
        UTF_16 = 1,
        UTF_16BE = 2,
        UTF_8 = 3,
        ASCII = 4,
        KOI8 = 5
    }
}

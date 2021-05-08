using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic
{
    public enum FilesystemAction
    {
        AddSongs,
        ManageImages,
        TagScanner,
        DeleteSelectedTags,
        DeleteTagType,
        DeleteTagValue,
        ModifyTags,
        DeleteAlbums,
        CopyAlbums,
        MoveAlbums,
        UpdateID3Tags,
        UpdateNewSongs,
        InformationExchange,
        Undefined
    }
}

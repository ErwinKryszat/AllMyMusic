DROP PROCEDURE DeleteSong;
-- GO



-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2014, Decmber 23>
-- Description:    Delete one Song
-- =============================================
CREATE PROCEDURE DeleteSong(
    IN var_SongID int,
    IN var_AlbumId int,
    IN var_BandID int,
    IN var_LeadPerformerID  int,
    OUT var_Rowcount int)

BEGIN

    DELETE FROM Songs WHERE Songs.ID = var_SongID;
    SET var_Rowcount = found_rows();

    -- Then delete the album if all songs are deleted
    SELECT * From viewSongs  WHERE IDAlbum = var_AlbumId Limit 10; 
    IF  found_rows() > 0 THEN
        BEGIN
            DELETE FROM Albums WHERE ID = var_AlbumId;
            DELETE FROM Images WHERE IDAlbum = var_AlbumId;
        END;
    END IF;

    -- Then delete the band if all songs are deleted
    SELECT * From viewSongs  WHERE IDBand = var_BandID Limit 10; 
    IF  found_rows() = 0 THEN     
        BEGIN
            DELETE FROM Bands WHERE ID = var_BandID;
        END;
    END IF;

    -- Then delete the leadperformer if all songs are deleted
    SELECT * From viewSongs  WHERE IDLeadPerformer = var_LeadPerformerID Limit 10;
    IF  found_rows() = 0 THEN
        BEGIN
            DELETE FROM LeadPerformer WHERE ID = var_LeadPerformerID;
        END;
    END IF;
END;

-- GO
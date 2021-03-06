DROP PROCEDURE DeleteAlbumGenre;
-- GO

-- =============================================
-- Author:        Erwin Kryszat
-- Create date:     <2009, September 25>
-- Description:    Delete one Album
-- =============================================
CREATE PROCEDURE DeleteAlbumGenre(
    IN var_AlbumGenreID int,
    OUT var_Rowcount int )

BEGIN


    DELETE Songs FROM Songs 
        INNER JOIN  Albums ON Albums.ID = Songs.IDAlbum
    WHERE 
        Albums.IDAlbumGenre = _AlbumGenreID
        AND Albums.VariousArtists = 1;

    DELETE Images FROM Images
        INNER JOIN  Albums ON Albums.ID = Images.IDAlbum
    WHERE 
        Albums.IDAlbumGenre = var_AlbumGenreID
        AND Albums.VariousArtists = 1;

    DELETE FROM Albums
    WHERE 
        Albums.IDAlbumGenre = var_AlbumGenreID
        AND Albums.VariousArtists = 1;

	DELETE FROM AlbumGenres
	WHERE ID = var_AlbumGenreID;

    SET var_Rowcount = found_rows();
    
END;

-- GO
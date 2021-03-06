
-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 28 October 2006
-- Description:	Delete one Album
-- =============================================
ALTER PROCEDURE [dbo].[DeleteAlbumGenre]
	@AlbumGenreID int,
	@Rowcount int OUTPUT


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	DELETE FROM Songs
		FROM Songs AS TempSongs
		INNER JOIN  Albums ON Albums.ID = TempSongs.IDAlbum
	WHERE 
		Albums.IDAlbumGenre = @AlbumGenreID
		AND Albums.VariousArtists = 1

	DELETE FROM Images
		FROM Images AS TempImages
		INNER JOIN  Albums ON Albums.ID = TempImages.IDAlbum
	WHERE 
		Albums.IDAlbumGenre = @AlbumGenreID
		AND Albums.VariousArtists = 1

	DELETE FROM Albums
	WHERE 
		Albums.IDAlbumGenre = @AlbumGenreID
		AND Albums.VariousArtists = 1

	DELETE FROM AlbumGenres
	WHERE ID = @AlbumGenreID

	SELECT @Rowcount = @@Rowcount
END

GO

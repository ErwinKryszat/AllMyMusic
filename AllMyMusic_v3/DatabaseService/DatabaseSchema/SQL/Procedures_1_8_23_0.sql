

-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 23 December 2014
-- Description:	Delete one Song
-- =============================================
ALTER PROCEDURE [dbo].[DeleteSong]
	@SongId int,
	@AlbumId int,
	@BandID int,
	@LeadPerformerID int,
	@Rowcount int OUTPUT


AS
BEGIN

	SET NOCOUNT ON;

	-- Then delete the song
	DELETE FROM Songs WHERE Songs.ID = @SongId
	SELECT @Rowcount = @@Rowcount

	-- Then delete the album if all songs are deleted
	SELECT TOP 10 * From viewSongs WHERE IDAlbum = @AlbumId
	IF @@ROWCOUNT = 0
	BEGIN
		DELETE FROM Albums WHERE ID = @AlbumId
		DELETE FROM Images WHERE IDAlbum = @AlbumId
	END

	-- Then delete the band if all songs are deleted
	SELECT TOP 10 * From viewSongs WHERE IDBand = @BandID
	IF @@ROWCOUNT = 0
	BEGIN
		DELETE FROM Bands WHERE ID = @BandID
	END

	-- Then delete the leadperformer if all songs are deleted
	SELECT TOP 10 * From viewSongs WHERE IDLeadPerformer = @LeadPerformerID
	IF @@ROWCOUNT = 0
	BEGIN
		DELETE FROM LeadPerformer WHERE ID = @LeadPerformerID
	END

END

GO
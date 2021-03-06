-- =============================================
-- Author:		 <Erwin Kryszat>
-- Create date:  <2006, July 22>
-- Last Updated: <2013, August 03>
-- Description:	 <Add an Song>
-- =============================================
CREATE PROCEDURE [dbo].[AddSong](
	@IDSong int,
	@IDBand int,
	@IDAlbum int,
	@IDGenre int,
	@IDLanguage int,
	@IDCountry int,
	@IDComposer int,
	@IDConductor int,
	@IDLeadPerformer int,
	@SongTitle nvarchar(250),
    @Path nvarchar(250),
    @Filename nvarchar(250),
    @Language nvarchar(50),
    @Country nvarchar(50),
    @Track int,
    @Rating int,
    @Genre nvarchar(50),
	@LengthInteger int,
	@LengthString nvarchar(8),
    @BitRate int,
    @SampleRate int,
    @CBR_VBR int,
	@ComposerName nvarchar(100),
	@ConductorName nvarchar(100),
	@LeadPerformerName nvarchar(100),
    @VA_Flag int,
	@DateAdded datetime,   
    @DatePlayed datetime, 
	@Comment nvarchar(250),
	@WebsiteUser nvarchar(250),
    @WebsiteArtist nvarchar(250),
	@ID int OUTPUT
	)
AS

DECLARE @Bookmarked int
SELECT @Bookmarked = 0
SELECT * FROM Bookmarks 
WHERE Reference = @SongTitle AND BookmarkType = 2
IF @@ROWCOUNT > 0
	BEGIN
		SELECT @bookmarked = 1
	END


-- Get the Genre ID or insert a new Genre record
IF @IDGenre = 0 AND LTRIM(@Genre) <> '' 
BEGIN
	SELECT @IDGenre = ID FROM Genres WHERE Genre = @Genre
	IF @@Rowcount = 0 
	BEGIN
		INSERT INTO Genres
			(
				Genre
			)
		VALUES
			(
				RTRIM(@Genre)
			)
		SELECT @IDGenre = SCOPE_IDENTITY()
	END
END




-- Get the Language ID or insert a new Language record
IF @IDLanguage = 0 AND LTRIM(@Language) <> '' 
BEGIN
	SELECT @IDLanguage = ID FROM Languages WHERE Language = @Language
	IF @@Rowcount = 0 
	BEGIN
		INSERT INTO Languages
			(
				Language
			)
		VALUES
			(
				RTRIM(@Language)
			)
		SELECT @IDLanguage = SCOPE_IDENTITY()
	END
END

-- Get the Country ID or insert a new record
IF @IDCountry = 0 AND LTRIM(@Country) <> '' 
BEGIN
	SELECT @IDCountry = ID FROM Countries WHERE Country = @Country
	IF @@Rowcount = 0 
	BEGIN
		INSERT INTO Countries
			(
				Country
			)
		VALUES
			(
				RTRIM(@Country)
			)
		SELECT @IDCountry = SCOPE_IDENTITY()
	END
END

-- Get the Composer ID  or insert a new record
IF @IDComposer = 0 AND LTRIM(@ComposerName) <> '' 
BEGIN
	SELECT @IDComposer = ID FROM Composer WHERE @ComposerName = Name
	IF @@Rowcount = 0 
	BEGIN
		INSERT INTO Composer
			(
				Name
			)

		VALUES
			(
				@ComposerName
			)

		SELECT @IDComposer = SCOPE_IDENTITY()
	END
END

-- Get the Conductor ID  or insert a new record
IF @IDConductor = 0 AND LTRIM(@ConductorName) <> '' 
BEGIN
	SELECT @IDConductor = ID FROM Conductor WHERE @ConductorName = Name
	IF @@Rowcount = 0  
	BEGIN
		INSERT INTO Conductor
			(
				Name
			)

		VALUES
			(
				@ConductorName
			)

		SELECT @IDConductor = SCOPE_IDENTITY()
	END
END

-- Get the LeadPerformer ID  or insert a new record
IF @IDLeadPerformer = 0 AND LTRIM(@LeadPerformerName) <> '' 
BEGIN
	SELECT @IDLeadPerformer = ID FROM LeadPerformer WHERE @LeadPerformerName = Name
	IF @@Rowcount = 0 
	BEGIN
		INSERT INTO LeadPerformer
			(
				Name
			)

		VALUES
			(
				@LeadPerformerName
			)

		SELECT @IDLeadPerformer = SCOPE_IDENTITY()
	END
END


-- Try to get an songid
SELECT @ID = @IDSong
IF @ID = 0 
BEGIN
	SELECT @ID = ID FROM Songs
	WHERE (@Path = Path) AND (@Filename = Filename) 
END

DECLARE @DateAdded2 datetime
IF @ID = 0
BEGIN
	SELECT @DateAdded2 = CURRENT_TIMESTAMP
END
ELSE
BEGIN
	SELECT @DateAdded2 = DateAdded FROM Songs WHERE ID = @ID
END

IF @ID = 0 AND LTRIM(@Path) <> '' AND LTRIM(@Filename) <> ''
BEGIN
	INSERT INTO Songs
		(
			IDBand,
			IDAlbum,
			SongTitle,
			Path,
			Filename,
			IDLanguage,
			IDCountry,
            Track,
            Rating,
			IDGenre,
			LengthInteger,
			LengthString,
			BitRate,
			SampleRate,
			CBR_VBR,
			IDComposer,
			IDConductor,
			IDLeadPerformer,
			VA_Flag,
			Bookmarked,
			DateAdded,   
			DatePlayed,
			Comment,
			WebsiteUser,
			WebsiteArtist
		)

	VALUES
		(
			@IDBand,
			@IDAlbum,
			@SongTitle,
			@Path,
			@Filename,
			@IDLanguage,
			@IDCountry,
			@Track,
			@Rating,
			@IDGenre,
			@LengthInteger,
			@LengthString,
			@BitRate,
			@SampleRate,
			@CBR_VBR,
			@IDComposer,
			@IDConductor,
			@IDLeadPerformer,
			@VA_Flag,
			@Bookmarked,
			@DateAdded2,   
			@DatePlayed,
			@Comment,
			@WebsiteUser,
			@WebsiteArtist
		)

	SELECT @ID = SCOPE_IDENTITY()
END
ELSE
BEGIN
	UPDATE Songs
	SET IDBand = @IDBand,
	    IDAlbum = @IDAlbum,
	    SongTitle = @SongTitle,
	    Path = @Path,
		Filename = @Filename,
	    IDLanguage = @IDLanguage,
		IDCountry = @IDCountry,
	    Track = @Track,
	    Rating = @Rating,
	    IDGenre = @IDGenre,
		LengthInteger = @LengthInteger,
		LengthString = @LengthString,
		BitRate = @BitRate,
		SampleRate = @SampleRate,
		CBR_VBR = @CBR_VBR,
		IDComposer = @IDComposer,
		IDConductor = @IDConductor,
		IDLeadPerformer = @IDLeadPerformer,
		VA_Flag = @VA_Flag,
		Bookmarked = @Bookmarked,
		DateAdded = @DateAdded2,
		DatePlayed = @DatePlayed,
		Comment = @Comment,
		WebsiteUser = @WebsiteUser,
		WebsiteArtist =	@WebsiteArtist
	WHERE ID = @ID
END

GO

-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2006, July 22>
-- Description:	<Add an Album>
-- =============================================
CREATE PROCEDURE [dbo].[AddAlbum](
    @IDAlbum int,
	@IDBand int,
	@Name nvarchar(100),
	@SortName nvarchar(100),
    @Year char(4),
	@AlbumVA int,
	@AlbumGenre nvarchar(50),
	@AlbumPath nvarchar(250),
	@ID int OUTPUT
	)
AS

DECLARE @Bookmarked int
SELECT @Bookmarked = 0
SELECT * FROM Bookmarks 
WHERE Reference = @Name AND BookmarkType = 1
IF @@ROWCOUNT > 0
	BEGIN
		SELECT @bookmarked = 1
	END

-- Get the Album Genre ID or insert a new Album Genre record
DECLARE @IDAlbumGenre int
SELECT @IDAlbumGenre = ID FROM AlbumGenres
WHERE AlbumGenre = @AlbumGenre
IF @@Rowcount = 0 AND LTRIM(@AlbumGenre) <> ''
BEGIN
	INSERT INTO AlbumGenres
		(
			AlbumGenre
		)
	VALUES
		(
			@AlbumGenre
		)
	SELECT @IDAlbumGenre = SCOPE_IDENTITY()
END

-- Try to get an albumID
SELECT @ID = @IDAlbum
IF @ID = 0 AND LTRIM(@AlbumPath) <> ''
BEGIN
	SELECT @ID = ID FROM Albums
	WHERE (@AlbumPath = AlbumPath) AND (@Name = Name)
END

IF @ID = 0 AND LTRIM(@Name) <> ''
	BEGIN
		INSERT INTO Albums
			(
				IDBand,
				Name,
				SortName,
				Year,
				VariousArtists,
				IDAlbumGenre,
				AlbumPath,
				Bookmarked
			)

		VALUES
			(
				@IDBand,
				@Name,
				@SortName,
				@Year,
				@AlbumVA,
				@IDAlbumGenre,
				@AlbumPath,
				@Bookmarked
			)

		SELECT @ID = SCOPE_IDENTITY()
	END
ELSE
	BEGIN
		UPDATE Albums
		SET IDBand = @IDBand,
			Name =	@Name,
			SortName =	@SortName,
			Year =	@Year,
			VariousArtists = @AlbumVA,
			IDAlbumGenre = @IDAlbumGenre,
			AlbumPath = @AlbumPath,
			Bookmarked = @Bookmarked
		WHERE ID = @ID
	END
GO


-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2006, July 16>
-- Description:	<Add an Artist>
-- =============================================
CREATE PROCEDURE [dbo].[AddBand](
	@Name nvarchar(100),
	@SortName nvarchar(100),
	@ID int OUTPUT
	)
AS

DECLARE @Bookmarked int
SELECT @Bookmarked = 0
SELECT * FROM Bookmarks 
WHERE Reference = @Name AND BookmarkType = 0
IF @@ROWCOUNT > 0
	BEGIN
		SELECT @bookmarked = 1
	END

SELECT @ID = ID FROM Bands
WHERE @Name = Name

IF @@ROWCOUNT = 0 
	BEGIN
		INSERT INTO Bands
			(
				Name, 
				SortName, 
				Bookmarked
			)

		VALUES
			(
				@Name,
				@SortName,
				@Bookmarked
			)

		SELECT @ID = SCOPE_IDENTITY()
	END

ELSE
	BEGIN
		UPDATE Bands
		SET Name = @Name,
		SortName = @SortName,
		Bookmarked = @Bookmarked
		WHERE ID = @ID
	END
GO



-- =============================================
-- Author:     <Erwin Kryszat>
-- Create date: <2009, January 11>
-- Description:   <Add a Bookmark>
-- =============================================
CREATE PROCEDURE [dbo].[AddBookmark](
   @BookmarkType int,
   @Reference nvarchar(100),
   @ID int OUTPUT 
   )
AS

SELECT @ID = ID FROM Bookmarks
WHERE Reference = @Reference

IF @@ROWCOUNT = 0 
   BEGIN
      INSERT INTO Bookmarks
         (
            BookmarkType,
            Reference
         )
      VALUES
         (
            @BookmarkType,
            @Reference
         )
      SELECT @ID = SCOPE_IDENTITY()
   END

ELSE
   BEGIN
      UPDATE Bookmarks
        SET BookmarkType = @BookmarkType,
        Reference = @Reference
      WHERE ID = @ID
   END



IF @BookmarkType = 0
BEGIN
	UPDATE Bands
	SET Bookmarked = 1
	WHERE Name = @Reference
END

IF @BookmarkType = 1
BEGIN
	UPDATE Albums
	SET Bookmarked = 1
	WHERE Name = @Reference
END

IF @BookmarkType = 2
BEGIN
	UPDATE Songs
	SET Bookmarked = 1
	WHERE SongTitle = @Reference
END
GO



-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2007, July 16>
-- Description:	<Add an Composer>
-- =============================================
CREATE PROCEDURE [dbo].[AddComposer](
	@Name nvarchar(100),
	@ID int OUTPUT
	)
AS

SELECT @ID = ID FROM Composer
WHERE @Name = Name

IF @@Rowcount = 0 AND LTRIM(@Name) <> ''
	BEGIN
		INSERT INTO Composer
			(
				Name
			)

		VALUES
			(
				@Name
			)

		SELECT @ID = SCOPE_IDENTITY()
	END


IF @@Rowcount > 0 AND LTRIM(@Name) <> ''
	BEGIN
		UPDATE Composer
		SET Name = @Name
		WHERE ID = @ID
	END
GO



-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2007, July 16>
-- Description:	<Add an Conductor>
-- =============================================
CREATE PROCEDURE [dbo].[AddConductor](
	@Name nvarchar(100),
	@ID int OUTPUT
	)
AS

SELECT @ID = ID FROM Conductor
WHERE @Name = Name

IF @@Rowcount = 0 AND LTRIM(@Name) <> ''
	BEGIN
		INSERT INTO Conductor
			(
				Name
			)

		VALUES
			(
				@Name
			)

		SELECT @ID = SCOPE_IDENTITY()
	END


IF @@Rowcount > 0 AND LTRIM(@Name) <> ''
	BEGIN
		UPDATE Conductor
		SET Name = @Name
		WHERE ID = @ID
	END

GO



-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2008, August 1>
-- Description:	<Add an Country>
-- =============================================
CREATE PROCEDURE [dbo].[AddCountry](
	@Country nvarchar(50),
	@Abbreviation nvarchar(10),
	@FlagPath nvarchar(200),
	@ID int OUTPUT
	)
AS

SELECT @ID = ID FROM Countries
WHERE @Country = Country

IF @@Rowcount = 0 
	BEGIN
		INSERT INTO Countries
			(
				Country,
				Abbreviation,
				FlagPath
			)

		VALUES
			(
				@Country,
				@Abbreviation,
				@FlagPath
			)

		SELECT @ID = SCOPE_IDENTITY()
	END
ELSE
	BEGIN
		UPDATE Countries
		SET Country = @Country,
		    Abbreviation = @Abbreviation,
		    FlagPath = @FlagPath
		WHERE ID = @ID
	END
GO




-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2006, September 17>
-- Description:	<Add / modify Image filenames>
-- =============================================
CREATE PROCEDURE [dbo].[AddImage](
	@IDAlbum int,
	@IDBand int,
	@Front nvarchar(250),
	@Stamp nvarchar(250),
	@Back nvarchar(250)
	)
AS

SELECT IDAlbum FROM Images
WHERE @IDAlbum = IDAlbum

IF @@Rowcount = 0
	BEGIN
		INSERT INTO Images
			(
				IDAlbum,
				IDBand,
				Front,
				Back,
				Stamp
			)

		VALUES
			(
				@IDAlbum,
				@IDBand,
				@Front,
				@Back,
				@Stamp
			)
	END
ELSE
	BEGIN
		UPDATE Images
		SET IDBand = @IDBand,
			Front =	@Front,
			Back = @Back,
			Stamp =	@Stamp
		WHERE @IDAlbum = IDAlbum
	END
GO




-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2007, July 16>
-- Description:	<Add an Composer>
-- =============================================
CREATE PROCEDURE [dbo].[AddLeadPerformer](
	@Name nvarchar(100),
	@ID int OUTPUT
	)
AS

SELECT @ID = ID FROM LeadPerformer
WHERE @Name = Name

IF @@Rowcount = 0 AND LTRIM(@Name) <> ''
	BEGIN
		INSERT INTO LeadPerformer
			(
				Name
			)

		VALUES
			(
				@Name
			)

		SELECT @ID = SCOPE_IDENTITY()
	END

IF @@Rowcount > 0 AND LTRIM(@Name) <> ''
	BEGIN
		UPDATE LeadPerformer
		SET Name = @Name
		WHERE ID = @ID
	END
GO




-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2008, January 27>
-- Description:	<Add an Parameter>
-- =============================================
CREATE PROCEDURE [dbo].[AddParameter](
	@Name nvarchar(16),
	@Value nvarchar(16)
)
AS

SELECT * FROM Parameter
WHERE @Name = Name

IF @@ROWCOUNT = 0 
	BEGIN
		INSERT INTO Parameter
			(
				Name,
				Value
			)

		VALUES
			(
				@Name,
				@Value
			)
	END

ELSE
	BEGIN
		UPDATE Parameter
		SET Value = @Value
		WHERE @Name = Name
	END

GO




-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2009, January 11>
-- Description:	<Add a Playlist>
-- =============================================
CREATE PROCEDURE [dbo].[AddPlaylist](
	@Name nvarchar(100),
	@PlaylistType int,
	@Path nvarchar(100),
	@ID int OUTPUT	)
AS

SELECT @ID = ID FROM PlaylistNames
WHERE @Name = Name

IF @@ROWCOUNT = 0 
	BEGIN
		INSERT INTO PlaylistNames
			(
				Name,
				PlaylistType,
				Path
			)
		VALUES
			(
				@Name,
				@PlaylistType,
				@Path
			)
		SELECT @ID = SCOPE_IDENTITY()
	END

ELSE
	BEGIN
		UPDATE PlaylistNames
		SET Name = @Name,
		  PlaylistType = @PlaylistType,
		  Path = @Path
		WHERE ID = @ID
	END
GO





-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2009, January 21>
-- Description:	<Add a Song to a Playlist>
-- =============================================
CREATE PROCEDURE [dbo].[AddPlaylistSong](
	@PlaylistID int,
	@SequenceNumber int,
	@SongFullPath nvarchar(250)
	)
AS
		
SELECT * FROM PlayListSongs
WHERE PlaylistID = @PlaylistID AND SongFullPath = @SongFullPath

IF @@ROWCOUNT = 0 
	BEGIN
		INSERT INTO PlaylistSongs
			(
				PlaylistID,
				SequenceNumber,
				SongFullPath	
			)
		VALUES
			(
				@PlaylistID,
				@SequenceNumber,
				@SongFullPath
			)
	END
ELSE
	BEGIN
		UPDATE PlaylistSongs
		SET PlaylistID = @PlaylistID,
			SequenceNumber = @SequenceNumber,
			SongFullPath =	@SongFullPath
		WHERE PlaylistID = @PlaylistID AND SongFullPath = @SongFullPath
	END


GO


==========================================
-- Author:		<Erwin Kryszat>
-- Create date: <2009, January 11>
-- Description:	<Add a Website URL>
-- =============================================
CREATE PROCEDURE [dbo].[AddWebsite](
	@URLtype nvarchar(4),
	@Bandname nvarchar(100),
	@URL nvarchar(250),
	@ID int OUTPUT	)
AS

SELECT @ID = ID FROM Websites
WHERE @URL = URL AND @Bandname = Bandname

IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO Websites
			(
				URLtype,
				Bandname,
				URL
			)
		VALUES
			(
				@URLtype,
				@Bandname,
				@URL
			)
		SELECT @ID = SCOPE_IDENTITY()
	END

ELSE
	BEGIN
		UPDATE Websites
		SET URLtype = @URLtype,
		Bandname = @Bandname,
		URL = @URL
		WHERE ID = @ID
	END
GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 28 October 2006
-- Description:	Delete one Album
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAlbum]
	@AlbumId int,
	@Rowcount int OUTPUT


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM Songs
	WHERE Songs.IDAlbum = @AlbumId

	DELETE FROM Albums
	WHERE Albums.ID = @AlbumId

    SELECT @Rowcount = @@Rowcount

	DELETE FROM Images
	WHERE Images.IDAlbum = @AlbumId
	
	DECLARE @BandID Integer
	SELECT @BandID = IDBand FROM Albums WHERE ID = @AlbumId AND VariousArtists = 0
	IF @BandID > 0 
	BEGIN
		SELECT TOP 10 * From viewSongs WHERE IDBand = @BandID
		IF @@ROWCOUNT = 0
		BEGIN
			DELETE FROM Bands WHERE ID = @BandID
		END
	END

END

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 28 October 2006
-- Description:	Delete one Album
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAlbumGenre]
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




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 28 October 2006
-- Description:	Delete one Band 
-- =============================================
CREATE PROCEDURE [dbo].[DeleteBand]
	@BandID int,
	@Rowcount int OUTPUT


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Name nvarchar(50)
	SELECT @Name = Name From Bands

	DELETE FROM Songs
	WHERE Songs.IDBand = @BandID

	DELETE FROM Albums
	WHERE Albums.IDBand = @BandID

	DELETE FROM Images
	WHERE Images.IDBand = @BandID

	DELETE FROM Bands
	WHERE Bands.ID = @BandID

	DELETE From AlbumGenres
	WHERE AlbumGenre Like @Name


	SELECT @Rowcount = @@Rowcount
END

GO




-- =============================================
-- Author:     <Erwin Kryszat>
-- Create date: <2009, January 11>
-- Description:   <Delete a Bookmark>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteBookmark](
	@BookmarkType int,
	@Reference nvarchar(100) )
AS

IF @BookmarkType = 0
BEGIN
	UPDATE Bands
	SET Bookmarked = 0
	WHERE Name = @Reference
END

IF @BookmarkType = 1
BEGIN
	UPDATE Albums
	SET Bookmarked = 0
	WHERE Name = @Reference
END

IF @BookmarkType = 2
BEGIN
	UPDATE Songs
	SET Bookmarked = 0
	WHERE SongTitle = @Reference
END

DELETE FROM Bookmarks
WHERE @Reference = Reference
 AND @BookmarkType = BookmarkType

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 11 November 2007
-- Description:	Delete one Composer 
-- =============================================
CREATE PROCEDURE [dbo].[DeleteComposer]
	@ComposerID int,
	@Rowcount int OUTPUT


AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM Songs
	WHERE Songs.IDComposer = @ComposerID

	DELETE FROM Composer
	WHERE Composer.ID = @ComposerID


	SELECT @Rowcount = @@Rowcount
END

GO





-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 11 November 2007
-- Description:	Delete one Conductor 
-- =============================================
CREATE PROCEDURE [dbo].[DeleteConductor]
	@ConductorID int,
	@Rowcount int OUTPUT


AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM Songs
	WHERE Songs.IDConductor = @ConductorID

	DELETE FROM Conductor
	WHERE Conductor.ID = @ConductorID


	SELECT @Rowcount = @@Rowcount
END

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 16 Marche 2008
-- Description:	Delete one Album
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCountry]
	@CountryID int,
	@Rowcount int OUTPUT


AS
BEGIN

	SET NOCOUNT ON;


	DELETE FROM Countries
	WHERE 
		Countries.ID = @CountryID

	SELECT @Rowcount = @@Rowcount
END

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 21 March 2008
-- Description:	Delete one Decade
-- =============================================
CREATE PROCEDURE [dbo].[DeleteDecade]
	@Decade int,
	@Rowcount int OUTPUT


AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM Songs
		FROM Songs AS TempSongs
		INNER JOIN  Albums ON Albums.ID = TempSongs.IDAlbum
	WHERE 
		Albums.Year >= @Decade AND Albums.Year < (@Decade + 10)



	SELECT @Rowcount = @@Rowcount
END

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 22 July 2007
-- Description:	Delete one Genre
-- =============================================
CREATE PROCEDURE [dbo].[DeleteGenre]
	@GenreID int,
	@Rowcount int OUTPUT


AS
BEGIN

	SET NOCOUNT ON;


	DELETE FROM Songs
		FROM Songs AS TempSongs
		INNER JOIN  Genres ON Genres.ID = TempSongs.IDGenre
	WHERE 
		Genres.ID = @GenreID


	DELETE FROM Genres
	WHERE 
		Genres.ID = @GenreID


	SELECT @Rowcount = @@Rowcount
END

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 16 Marche 2008
-- Description:	Delete one Album
-- =============================================
CREATE PROCEDURE [dbo].[DeleteLanguage]
	@LanguageID int,
	@Rowcount int OUTPUT


AS
BEGIN

	SET NOCOUNT ON;


	DELETE FROM Languages
	WHERE 
		Languages.ID = @LanguageID

	SELECT @Rowcount = @@Rowcount
END

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 11 November 2007
-- Description:	Delete one LeadPerformer 
-- =============================================
CREATE PROCEDURE [dbo].[DeleteLeadPerformer]
	@LeadPerformerID int,
	@Rowcount int OUTPUT


AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM Songs
	WHERE Songs.IDLeadPerformer = @LeadPerformerID

	DELETE FROM LeadPerformer
	WHERE LeadPerformer.ID = @LeadPerformerID


	SELECT @Rowcount = @@Rowcount
END

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 25 January 2009
-- Description:	Delete one PlaylistItemCollection 
-- =============================================
CREATE PROCEDURE [dbo].[DeletePlaylist]
	@PlaylistID int,
	@Rowcount int OUTPUT


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	DELETE FROM PlaylistNames
	WHERE ID = @PlaylistID

    DELETE FROM PlaylistSongs
	WHERE PlaylistID = @PlaylistID
	
	SELECT @Rowcount = @@Rowcount
END

GO




-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2009, January 29>
-- Description:	<Delete a Song from Playlist>
-- =============================================
CREATE PROCEDURE [dbo].[DeletePlaylistSong](
	@SongFullPath nvarchar(250)
)
AS



DELETE FROM PlaylistSongs
WHERE @SongFullPath = SongFullPath 

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 23 December 2014
-- Description:	Delete one Song
-- =============================================
CREATE PROCEDURE [dbo].[DeleteSong]
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




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 28 October 2006
-- Description:	Delete one Album
-- =============================================
CREATE PROCEDURE [dbo].[DeleteVariousArtists]
	@Rowcount int OUTPUT


AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM Songs
		FROM Songs AS TempSongs
		INNER JOIN  Albums ON Albums.ID = TempSongs.IDAlbum
	WHERE 
		Albums.VariousArtists = 1

	DELETE FROM Images
		FROM Images AS TempImages
		INNER JOIN  Albums ON Albums.ID = TempImages.IDAlbum
	WHERE 
		Albums.VariousArtists = 1

	DELETE FROM Albums
	WHERE 
		Albums.VariousArtists = 1


	SELECT @Rowcount = @@Rowcount
END

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 21 March 2008
-- Description:	Delete one Year
-- =============================================
CREATE PROCEDURE [dbo].[DeleteYear]
	@Year int,
	@Rowcount int OUTPUT


AS
BEGIN
	
	SET NOCOUNT ON;

	DELETE FROM Songs
		FROM Songs AS TempSongs
		INNER JOIN  Albums ON Albums.ID = TempSongs.IDAlbum
	WHERE 
		Albums.Year = @Year


	SELECT @Rowcount = @@Rowcount
END

GO




-- =============================================
-- Author:		<Erwin Kryszat>
-- Create date: <2009, February 15>
-- Description:	<Move Rows in a PlaylistItemCollection up or down>
-- =============================================
CREATE PROCEDURE [dbo].[PlaylistMoveRows](
	@PlaylistID int,
    @sourceStartIndex int,
    @sourceEndIndex int,
    @targetIndex int
	)
AS


Declare @rowsToInsert int

IF @sourceStartIndex <=  @sourceEndIndex
	BEGIN
		SELECT @rowsToInsert = @sourceEndIndex - @sourceStartIndex + 1

		-- create the hole 
		UPDATE PlaylistSongs
		SET   SequenceNumber = SequenceNumber + @rowsToInsert
		WHERE @PlaylistID = PlaylistID 
			AND SequenceNumber >= @targetIndex

		-- Move the selected rows down
		IF @sourceStartIndex < @targetIndex
			BEGIN
				-- move the selected songs to the hole
				UPDATE PlaylistSongs
				SET   SequenceNumber = SequenceNumber + @targetIndex - @sourceStartIndex
				WHERE @PlaylistID = PlaylistID 
					AND SequenceNumber >= @sourceStartIndex
					AND SequenceNumber <= @sourceEndIndex
			END
		
		-- Move the selected rows up
		IF @sourceStartIndex > @targetIndex
			BEGIN
				-- move the selected songs to the hole
				UPDATE PlaylistSongs
				SET   SequenceNumber = SequenceNumber + @targetIndex - @sourceStartIndex - @rowsToInsert
				WHERE @PlaylistID = PlaylistID 
					AND SequenceNumber  >= @sourceStartIndex + @rowsToInsert
					AND SequenceNumber  <= @sourceEndIndex + @rowsToInsert
			END

		CREATE TABLE #TempPlaylist
		(
			PlaylistID int,
			SequenceNumber int,
			SongFullPath nchar(250) COLLATE  database_default
		) 

		INSERT INTO #TempPlaylist
		SELECT * 
		FROM PlaylistSongs 
		WHERE @PlaylistID = PlaylistID
		ORDER BY SequenceNumber

		-- Renumber the sequenceNumbers from 1 to n in the Temporay Table
		DECLARE @index int
		SELECT @index = 0
		UPDATE #TempPlaylist
		SET   SequenceNumber = @index ,
			  @index = @index + 1
		WHERE @PlaylistID = PlaylistID 

		-- Update the new number to the playlist
		UPDATE PlaylistSongs
		SET  PlaylistSongs.SequenceNumber = #TempPlaylist.SequenceNumber
		FROM PlaylistSongs JOIN #TempPlaylistItemCollection ON #TempPlaylist.SongFullPath = PlaylistSongs.SongFullPath
		WHERE @PlaylistID = PlaylistSongs.PlaylistID 
			AND @PlaylistID = #TempPlaylist.PlaylistID
		
		DROP TABLE #TempPlaylist
	END

GO




-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 31 July 2006
-- Updated 30 April 2009
-- Description:	Purge the database
-- =============================================
CREATE PROCEDURE [dbo].[PurgeDatabase]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM Songs
	DELETE FROM Albums
	DELETE FROM Bands
	DELETE FROM Genres
	DELETE FROM AlbumGenres
	DELETE FROM Languages
    DELETE FROM Countries
	DELETE FROM Images
	DELETE FROM Composer
	DELETE FROM Conductor
	DELETE FROM LeadPerformer
    DELETE FROM Websites
    DELETE FROM ID3Tags

	dbcc checkident (Songs, reseed, 0)
	dbcc checkident (Albums, reseed, 0)
	dbcc checkident (Bands, reseed, 0)
	dbcc checkident (Genres, reseed, 0)
	dbcc checkident (AlbumGenres, reseed, 0)
	dbcc checkident (Languages, reseed, 0)
	dbcc checkident (Countries, reseed, 0)
	dbcc checkident (Composer, reseed, 0)
	dbcc checkident (Conductor, reseed, 0)
	dbcc checkident (LeadPerformer, reseed, 0)
    dbcc checkident (Websites, reseed, 0)
    dbcc checkident (ID3Tags, reseed, 0)

END

GO

-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 21 November 2009
-- Description:	Add ID3 Tags
-- =============================================
CREATE PROCEDURE [dbo].[AddID3Tag](
	@Header nchar(10),
	@Version int,
	@Revision int,
	@Flags int,
	@Tagname nchar(10),
	@Tagvalue nvarchar(250),
	@Tagsize int,
	@StandardTag int,
    @Path nvarchar(250),
    @Filename nvarchar(250)
	)
AS
BEGIN
INSERT INTO ID3Tags
	(
		Header,
		Version,
		Revision,
		Flags,
		Tagname,
		Tagvalue,
		Tagsize,
		StandardTag,
		Path,
		Filename
	)

VALUES
	(
		@Header,
		@Version,
		@Revision,
		@Flags,
		@Tagname,
		@Tagvalue,
		@Tagsize,
		@StandardTag,
		@Path,
		@Filename
	)
END

GO

-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 02 May 2021
-- Description:	Initialize Countries
-- =============================================
CREATE PROCEDURE [dbo].[InitializeCountries]

AS
BEGIN
SET NOCOUNT ON
SET IDENTITY_INSERT Countries ON;
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Albania', 'AL', 1);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('America', 'US', 2);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Andorra', 'AD', 3);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Antigua & Barbuda', 'AG', 4);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Argentina', 'AR', 5);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Australia', 'AU', 6);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Austria', 'AT', 7);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Azerbaijan', 'AZ', 8);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Barbados', 'BB', 9);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Belarus', 'BY', 10);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Belgium', 'BE', 11);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Benin', 'BJ', 12);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Bermuda', 'BM', 13);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Bosnia & Herzegovina', 'BA', 14);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Brazil', 'BR', 15);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Bulgaria', 'BG', 16);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Cameroon', 'CM', 17);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Canada', 'CA', 18);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Cape Verde Islands', 'CV', 19);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Chile', 'CL', 20);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Colombia', 'CO', 21);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Croatia', 'HR', 22);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Cuba', 'CU', 23);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Cyprus', 'CY', 24);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Czech Republic', 'CZ', 25);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Denmark', 'DK', 26);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Dominican Republic', 'DO', 27);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('England', 'GB', 28);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Estonia', 'EE', 29);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Finland', 'FI', 30);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('France', 'FR', 31);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Georgia', 'GE', 32);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Germany', 'DE', 33);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Greece', 'GR', 34);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Guyana', 'GY', 35);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Hawaii', 'HI', 36);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Hungary', 'HU', 37);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Iceland', 'IS', 38);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Ireland', 'IE', 39);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Israel', 'IL', 40);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Italy', 'IT', 41);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Jamaica', 'JM', 42);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Japan', 'JP', 43);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Latvia', 'LV', 44);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Lithuania', 'LT', 45);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Macedonia', 'MK', 46);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Malta', 'Mt', 47);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Mexico', 'Mx', 48);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Moldova', 'MD', 49);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Monaco', 'MC', 50);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Montenegro', 'ME', 51);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Netherlands', 'NL', 52);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('New Zealand', 'NZ', 53);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Norway', 'NO', 54);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Peru', 'PE', 55);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Philippines', 'PH', 56);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Poland', 'PL', 57);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Portugal', 'PT', 58);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Puerto Rico', 'PR', 59);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Romania', 'RO', 60);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Russia', 'RU', 61);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Rwanda', 'RW', 62);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('San Marino', 'SM', 63);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Scotland', 'UK', 64);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Senegal', 'SN', 65);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Serbia', 'RS', 66);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Slovakia', 'SK', 67);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Slovenia', 'SI', 68);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('South Africa', 'ZA', 69);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('South Korea', 'KR', 70);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Spain', 'ES', 71);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Sweden', 'SE', 72);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Switzerland', 'CH', 73);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Timor Leste', 'TL', 74);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Trinidad & Tobago', 'TT', 75);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Turkey', 'TR', 76);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Ukraine', 'UA', 77);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('United Kingdom', 'UK', 78);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Venezuela', 'VE', 79);
INSERT INTO Countries	(  Country, Abbreviation, ID )	VALUES	('Mongolia', 'MN', 80);

SET IDENTITY_INSERT Countries OFF;
END
GO

-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 02 May 2021
-- Description:	Initialize Languages
-- =============================================
CREATE PROCEDURE [dbo].[InitializeLanguages]
AS
BEGIN
SET NOCOUNT ON
SET IDENTITY_INSERT Languages ON;
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('albanian', 'AL', 1);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('bulgarian', 'BG', 2);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('croatian', 'HR', 3);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('dutch', 'NL', 4);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('english', 'GB', 5);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('estonian', 'EE', 6);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('finish', 'FI', 7);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('french', 'FR', 8);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('georgian', 'GE', 9);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('german', 'DE', 10);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('greek', 'GR', 11);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('hawaiian', 'HI', 12);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('hebrew', 'IL', 13);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('hungarian', 'HU', 14);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('icelandic', 'IS', 15);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('instrumental', 'NA', 16);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('italian', 'IT', 17);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('japanese', 'JP', 18);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('korean', 'KP', 19);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('lithuanian', 'LT', 20);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('mecdonian', 'MK', 21);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('moldavian', 'MD', 22);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('Mundart', 'CH', 23);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('polish', 'PL', 24);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('portuguese', 'PT', 25);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('romanian', 'RO', 26);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('russian', 'RU', 27);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('serbian', 'RS', 28);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('slovakian', 'SK', 29);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('slovenian', 'SI', 30);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('spanish', 'ES', 31);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('swedish', 'SE', 32);
INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('turkish', 'TR', 33);

INSERT INTO Languages	( Language, Abbreviation, ID )	VALUES	('afrikaans', 'TR', 34);
 
SET IDENTITY_INSERT Languages OFF;
END
GO

-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 22 May 2021
-- Description:	GetStatistics
-- =============================================
CREATE PROCEDURE [dbo].[GetStatistics]
AS
BEGIN
	SET NOCOUNT ON;
	declare @countSongs as int
	declare @countAlbums as int
	declare @countBands as int

	select @countSongs = Count(Path) from Songs
	select @countAlbums = Count(Name) from Albums
	select @countBands = Count(Name) from Bands

	select @countSongs As Songs, @countAlbums as Albums, @countBands as Bands
END
GO


-- =============================================
-- Author:		 <Erwin Kryszat>
-- Create date:  <2006, July 22>
-- Last Updated: <2013, August 03>
-- Description:	 <Add an Song>
-- =============================================
ALTER PROCEDURE [dbo].[AddSong](
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

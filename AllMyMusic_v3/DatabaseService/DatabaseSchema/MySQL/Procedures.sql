-- =============================================
-- Author:          <Erwin Kryszat>
-- Create date:     <2009, September 25>
-- Last Updated:    <2013, August 03>
-- Description:     <Add an Song>
-- =============================================
CREATE PROCEDURE `AddSong`(
    IN var_IDSong int,
    IN var_IDBand int,
    IN var_IDAlbum int,
    IN var_IDGenre int,
    IN var_IDLanguage int,
    IN var_IDCountry int,
    IN var_IDComposer int,
    IN var_IDConductor int,
    IN var_IDLeadPerformer int,
    IN var_SongTitle nvarchar(250),
    IN var_Path nvarchar(250),
    IN var_Filename nvarchar(250),
    IN var_Language nvarchar(50),
    IN var_Country nvarchar(50),
    IN var_Track int,
    IN var_Rating int,
    IN var_Genre nvarchar(50),
    IN var_LengthInteger int,
    IN var_LengthString nvarchar(8),
    IN var_BitRate int,
    IN var_SampleRate int,
    IN var_CBR_VBR int,
    IN var_ComposerName nvarchar(100),
    IN var_ConductorName nvarchar(100),
    IN var_LeadPerformerName nvarchar(100),
    IN var_VA_Flag int,
	IN var_DateAdded datetime,   
    IN var_DatePlayed datetime, 
	IN var_Comment nvarchar(250),
	IN var_WebsiteUser nvarchar(250),
    IN var_WebsiteArtist nvarchar(250),
    OUT var_ID int
    )
BEGIN


SET @var_Bookmarked = 0;
SELECT * FROM Bookmarks 
WHERE Reference = var_SongTitle AND BookmarkType = 2;

IF  found_rows() > 0 THEN
    BEGIN
        SELECT @var_Bookmarked = 1;
    END;
END IF;


-- Get the Genre ID or insert a new Genre record
SET @IDGenre = 0;
IF var_IDGenre = 0 AND LTRIM(var_Genre) <> '' THEN
    SELECT ID FROM Genres WHERE Genre = var_Genre INTO @IDGenre;
    IF  found_rows() = 0  THEN
    BEGIN
        INSERT INTO Genres
            (Genre ) VALUES ( RTRIM(var_Genre));
        SET @IDGenre = last_insert_id();
    END;
    END IF;
ELSE
    SET @IDGenre = var_IDGenre;
END IF;



-- Get the Language ID or insert a new Language record
SET @IDLanguage = 0;
IF var_IDLanguage = 0 AND LTRIM(var_Language) <> '' THEN
    SELECT ID FROM Languages WHERE Language = var_Language INTO @IDLanguage;
    IF  found_rows() = 0  THEN
    BEGIN
        INSERT INTO Languages
            (Language ) VALUES ( RTRIM(var_Language));
        SET @IDLanguage = last_insert_id();
    END;
    END IF;
ELSE
    SET @IDLanguage = var_IDLanguage;
END IF;



-- Get the Country ID or insert a new record
SET @IDCountry = 0;
IF var_IDCountry = 0 AND LTRIM(var_Country) <> '' THEN
    SELECT ID FROM Countries WHERE Country = var_Country INTO @IDCountry;
    IF  found_rows() = 0  THEN
        BEGIN
            INSERT INTO Countries (Country) VALUES (RTRIM(var_Country));
            SET @IDCountry = last_insert_id();
        END;
    END IF;
ELSE
    SET @IDCountry = var_IDCountry;
END IF;

-- Get the Composer ID  or insert a new record
SET @IDComposer = 0;
IF var_IDComposer = 0 AND LTRIM(var_ComposerName) <> '' THEN
    SELECT ID FROM Composer    WHERE var_ComposerName = Name INTO @IDComposer;

    IF  found_rows() = 0  THEN
        BEGIN
            INSERT INTO Composer (Name)    VALUES (var_ComposerName);
            SET @IDComposer = last_insert_id();
        END;
    END IF;
ELSE
    SET @IDComposer = var_IDComposer;
END IF;

-- Get the Conductor ID  or insert a new record
SET @IDConductor = 0;
IF var_IDConductor = 0 AND LTRIM(var_ConductorName) <> '' THEN
    SELECT ID FROM Conductor WHERE var_ConductorName = Name    INTO @IDConductor;
    IF  found_rows() = 0   THEN
        BEGIN
            INSERT INTO Conductor (Name) VALUES(var_ConductorName);
            SET @IDConductor = last_insert_id();
        END;
    END IF;
ELSE
    SET @IDConductor = var_IDConductor;
END IF;

-- Get the LeadPerformer ID  or insert a new record
SET @IDLeadPerformer = 0;
IF var_IDLeadPerformer = 0 AND LTRIM(var_LeadPerformerName) <> '' THEN
    SELECT ID FROM LeadPerformer WHERE var_LeadPerformerName = Name INTO @IDLeadPerformer;
    IF  found_rows() = 0  THEN
        BEGIN
            INSERT INTO LeadPerformer (Name) VALUES(var_LeadPerformerName);
            SET @IDLeadPerformer = last_insert_id();
        END;
    END IF;
ELSE
    SET @IDLeadPerformer = var_IDLeadPerformer;
END IF;

-- Try to get an songid
SET @RETURN_VALUE = var_IDSong;
IF @RETURN_VALUE = 0  THEN
    BEGIN
        SELECT ID FROM Songs
        WHERE (var_Path = Path) AND (var_Filename = Filename)
        INTO @RETURN_VALUE; 
    END;
END IF;

SET @DateAdded = DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');
IF @RETURN_VALUE > 0 THEN
    SELECT DATE_FORMAT(DateAdded, '%Y-%m-%d %H:%i:%s') FROM Songs WHERE ID = @RETURN_VALUE
	INTO @DateAdded;
END IF;

IF @RETURN_VALUE = 0 AND LTRIM(var_Path) <> '' AND LTRIM(var_Filename) <> '' THEN
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
            var_IDBand,
            var_IDAlbum,
            var_SongTitle,
            var_Path,
            var_Filename,
            @IDLanguage,
            @IDCountry,
            var_Track,
            var_Rating,
            @IDGenre,
            var_LengthInteger,
            var_LengthString,
            var_BitRate,
            var_SampleRate,
            var_CBR_VBR,
            @IDComposer,
            @IDConductor,
            @IDLeadPerformer,
            @var_VA_Flag,
            @var_Bookmarked,
			@DateAdded,   
			var_DatePlayed,
			var_Comment,
		    var_WebsiteUser,
		    var_WebsiteArtist 
        );

    SET @RETURN_VALUE = last_insert_id();
    END;
ELSE
    BEGIN
    UPDATE Songs
    SET IDBand = var_IDBand,
        IDAlbum = var_IDAlbum,
        SongTitle = var_SongTitle,
        Path = var_Path,
        Filename = var_Filename,
        IDLanguage = @IDLanguage,
        IDCountry = @IDCountry,
        Track = var_Track,
        Rating = var_Rating,
        IDGenre = @IDGenre,
        LengthInteger = var_LengthInteger,
        LengthString = var_LengthString,
        BitRate = var_BitRate,
        SampleRate = var_SampleRate,
        CBR_VBR = var_CBR_VBR,
        IDComposer = @IDComposer,
        IDConductor = @IDConductor,
        IDLeadPerformer = @IDLeadPerformer,
        VA_Flag = var_VA_Flag,
        Bookmarked = @var_Bookmarked,
		DateAdded = @DateAdded,
		DatePlayed = var_DatePlayed,
		Comment = var_Comment,
		WebsiteUser = var_WebsiteUser,
		WebsiteArtist =	var_WebsiteArtist
    WHERE ID = @RETURN_VALUE;
    END;
END IF;
SET  var_ID = @RETURN_VALUE;
END
-- GO


-- =============================================
-- Author:    <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description:    <Add an Album>
-- =============================================

CREATE PROCEDURE AddAlbum (
    IN var_IDAlbum int,
    IN var_IDBand int,
    IN var_Name nvarchar(100),
	IN var_SortName nvarchar(100),
    IN var_Year char(4),
    IN var_AlbumVA int,
    IN var_AlbumGenre nvarchar(50),
    IN var_AlbumPath nvarchar(250),
    OUT var_ID int )
BEGIN

SET @var_Bookmarked = 0;
SELECT * FROM Bookmarks 
WHERE Reference = var_Name AND BookmarkType = 1;

IF  found_rows() > 0 THEN
    SET @var_Bookmarked = 1;
END IF;


SET @var_IDAlbumGenre = 0;
SELECT ID FROM AlbumGenres
WHERE AlbumGenre = var_AlbumGenre
INTO @var_IDAlbumGenre;
IF  found_rows() = 0 AND LTRIM(var_AlbumGenre) <> '' THEN
    INSERT INTO AlbumGenres    (AlbumGenre) VALUES    (var_AlbumGenre);  
    SET @var_IDAlbumGenre = last_insert_id();
END IF;

-- Try to get an albumid
SET @RETURN_VALUE = var_IDAlbum;
IF @RETURN_VALUE = 0 AND LTRIM(var_AlbumPath) <> '' THEN
    SELECT ID FROM Albums
    WHERE (var_AlbumPath = AlbumPath) AND (var_Name = Name)
    INTO @RETURN_VALUE;
END IF;

IF @RETURN_VALUE = 0 AND LTRIM(var_Name) <> '' THEN
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
                var_IDBand,
                var_Name,
				var_SortName,
                var_Year,
                var_AlbumVA,
                @var_IDAlbumGenre,
                var_AlbumPath,
                @var_Bookmarked
            );

        SET @RETURN_VALUE = last_insert_id();
    END;
ELSE
    BEGIN
        UPDATE Albums
        SET IDBand = var_IDBand,
            Name =  var_Name,
			SortName =  var_SortName,
            Year =  var_Year,
            VariousArtists = var_AlbumVA,
            IDAlbumGenre = @var_IDAlbumGenre,
            AlbumPath = var_AlbumPath,
            Bookmarked = @var_Bookmarked
        WHERE ID = @RETURN_VALUE;
    END;
END IF;
SET  var_ID = @RETURN_VALUE;
END;
-- GO



-- =============================================
-- Author:      <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description: <Add an Artist>
-- =============================================
CREATE PROCEDURE AddBand(
    IN var_Name nvarchar(100),
	IN var_SortName nvarchar(100),
    OUT var_ID int
    )
BEGIN


SET @var_Bookmarked = 0;
SELECT * FROM Bookmarks 
WHERE Reference = var_Name AND BookmarkType = 0 ;
IF  found_rows() > 0 THEN
    BEGIN
    SET @var_Bookmarked = 1;
END;
END IF;

SET @RETURN_VALUE = -1;
SELECT  ID FROM Bands
WHERE var_Name = Name
INTO @RETURN_VALUE;
IF  found_rows() = 0  THEN
    BEGIN
        INSERT INTO Bands
            (
                Name, 
				SortName, 
                Bookmarked
            )

        VALUES
            (
                var_Name,
				var_SortName,
                @var_Bookmarked
            );
SET @RETURN_VALUE = last_insert_id();
END;
ELSE
    BEGIN
        UPDATE Bands
        SET Name = var_Name,
		SortName = var_SortName,
        Bookmarked = @var_Bookmarked
        WHERE ID = @RETURN_VALUE;
END;
END IF;
SET  var_ID = @RETURN_VALUE;
END;
-- GO


-- =============================================
-- Author:     <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description:   <Add a Bookmark>
-- =============================================
CREATE PROCEDURE AddBookmark(
   IN var_BookmarkType int,
   IN var_Reference nvarchar(100),
   OUT var_ID int 
   )
BEGIN

SET @RETURN_VALUE = -1;
SELECT ID FROM Bookmarks
WHERE Reference = var_Reference
INTO @RETURN_VALUE;

IF  found_rows() = 0  THEN
   BEGIN
      INSERT INTO Bookmarks
         (
            BookmarkType,
            Reference
         )
      VALUES
         (
            var_BookmarkType,
            var_Reference
         );
         
      SET @RETURN_VALUE = last_insert_id();
    END;
ELSE
   BEGIN
      UPDATE Bookmarks
        SET BookmarkType = var_BookmarkType,
        Reference = var_Reference
      WHERE ID = @RETURN_VALUE;
    END;
END IF; 



IF var_BookmarkType = 0 THEN
    BEGIN
        UPDATE Bands
        SET Bookmarked = 1
        WHERE Name = var_Reference;
    END;
END IF; 

IF var_BookmarkType = 1 THEN
    BEGIN
        UPDATE Albums
        SET Bookmarked = 1
        WHERE Name = var_Reference;
    END;
END IF; 

IF var_BookmarkType = 2 THEN
    BEGIN
        UPDATE Songs
        SET Bookmarked = 1
        WHERE SongTitle = var_Reference;
    END;
END IF; 
SET  var_ID = @RETURN_VALUE;
END;
-- GO


-- =============================================
-- Author:    <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description:    <Add an Composer>
-- =============================================
CREATE PROCEDURE AddComposer(
    IN var_Name nvarchar(100),
    OUT var_ID int )
BEGIN

SET @RETURN_VALUE = -1;
SELECT ID FROM Composer
WHERE var_Name = Name
INTO @RETURN_VALUE;

IF  found_rows() = 0 AND LTRIM(var_Name) <> '' THEN
    BEGIN
        INSERT INTO Composer  (Name)   VALUES  (var_Name);
        SET @RETURN_VALUE = last_insert_id();
    END;
END IF;     

IF  found_rows() > 0 AND LTRIM(var_Name) <> '' THEN  
    BEGIN
        UPDATE Composer
        SET Name = var_Name
        WHERE ID = @RETURN_VALUE;
    END;
END IF; 
SET  var_ID = @RETURN_VALUE;
END;
-- GO


-- =============================================
-- Author:        <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description:    <Add an Conductor>
-- =============================================
CREATE PROCEDURE AddConductor(
    IN var_Name nvarchar(100),
    OUT var_ID int )
BEGIN

SELECT ID FROM Conductor
WHERE var_Name = Name
INTO @RETURN_VALUE;

IF  found_rows() = 0 AND LTRIM(var_Name) <> '' THEN
    BEGIN
        INSERT INTO Conductor (  Name ) VALUES   ( var_Name);
        SET @RETURN_VALUE = last_insert_id();
    END;
END IF;     


IF  found_rows() > 0 AND LTRIM(var_Name) <> '' THEN
    BEGIN
        UPDATE Conductor
        SET Name = var_Name
        WHERE ID = @RETURN_VALUE;
    END;
END IF; 
SET  var_ID = @RETURN_VALUE;
END;
-- GO



-- =============================================
-- Author:        <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description:    <Add an Country>
-- =============================================
CREATE PROCEDURE AddCountry(
    IN var_Country nvarchar(50),
    IN var_Abbreviation nvarchar(10),
    IN var_FlagPath nvarchar(200),
    OUT var_ID int  )
BEGIN

SELECT ID FROM Countries
WHERE var_Country = Country
INTO @RETURN_VALUE;

IF  found_rows() = 0 AND LTRIM(var_Country) <> '' THEN
    BEGIN
        INSERT INTO Countries
            (
                Country,
                Abbreviation,
                FlagPath
            )

        VALUES
            (
                var_Country,
                var_Abbreviation,
                var_FlagPath
            );

        SET @RETURN_VALUE = last_insert_id();
    END;
END IF; 

IF  found_rows() > 0 AND LTRIM(var_Country) <> '' THEN
    BEGIN
        UPDATE Countries
        SET Country = var_Country,
            Abbreviation = var_Abbreviation,
            FlagPath = var_FlagPath
        WHERE ID = @RETURN_VALUE;
    END;
END IF; 
SET  var_ID = @RETURN_VALUE;
END;
-- GO


-- =============================================
-- Author:    <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description:    <Add / modify Image filenames>
-- =============================================
CREATE PROCEDURE AddImage(
    IN var_IDAlbum int,
    IN var_IDBand int,
    IN var_Front nvarchar(250),
    IN var_Stamp nvarchar(250),
    IN var_Back nvarchar(250)
    )
BEGIN

SELECT IDAlbum FROM Images
WHERE var_IDAlbum = IDAlbum;

IF  found_rows() = 0 THEN
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
                var_IDAlbum,
                var_IDBand,
                var_Front,
                var_Back,
                var_Stamp
            );
    END;
ELSE
    BEGIN
        UPDATE Images
        SET IDBand = var_IDBand,
            Front =  var_Front,
            Back = var_Back,
            Stamp =    var_Stamp
        WHERE var_IDAlbum = IDAlbum;
    END;
END IF; 
END;
-- GO


-- =============================================
-- Author:    <Erwin Kryszat>
-- Create date: <2007, July 16>
-- Description:    <Add an Composer>
-- =============================================
CREATE PROCEDURE AddLeadPerformer(
    IN var_Name nvarchar(100),
    OUT var_ID int
    )
BEGIN

SELECT ID FROM LeadPerformer
WHERE var_Name = Name
INTO @RETURN_VALUE;

IF  found_rows() = 0 AND LTRIM(var_Name) <> '' THEN
    BEGIN
        INSERT INTO LeadPerformer
            (
                Name
            )

        VALUES
            (
                var_Name
            );

        SET @RETURN_VALUE = last_insert_id();
    END;
END IF; 

IF  found_rows() > 0 AND LTRIM(var_Name) <> '' THEN
    BEGIN
        UPDATE LeadPerformer
        SET Name = var_Name
        WHERE ID = @RETURN_VALUE;
    END;
END IF; 
SET  var_ID = @RETURN_VALUE;
END;
-- GO

-- =============================================
-- Author:        <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description:    <Add an Parameter>
-- =============================================
CREATE PROCEDURE AddParameter(
    IN var_Name nvarchar(16),
    IN var_Value nvarchar(16)
)
BEGIN

SELECT * FROM Parameter
WHERE var_Name = Name;

IF  found_rows() = 0 THEN
    BEGIN
        INSERT INTO Parameter
            (
                Name,
                Value
            )

        VALUES
            (
                var_Name,
                var_Value
            );
    END;
ELSE
    BEGIN
        UPDATE Parameter
        SET Value = var_Value
        WHERE var_Name = Name;
    END;
END IF; 
END;
-- GO

-- =============================================
-- Author:      <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description: <Add a Playlist>
-- =============================================

CREATE PROCEDURE AddPlaylist(
    IN var_Name nvarchar(100),
    IN var_PlaylistType int,
    IN var_Path nvarchar(100),
    OUT var_ID int    )
BEGIN

SET @RETURN_VALUE = -1;
SELECT ID FROM PlaylistNames
WHERE var_Name = Name
INTO @RETURN_VALUE;

IF  found_rows() = 0  THEN
    BEGIN
        INSERT INTO PlaylistNames
            (
                Name,
                PlaylistType,
                Path
            )
        VALUES
            (
                var_Name,
                var_PlaylistType,
                var_Path
            );
        SET @RETURN_VALUE = last_insert_id();
    END;
ELSE
    BEGIN
        UPDATE PlaylistNames
        SET Name = var_Name,
          PlaylistType = var_PlaylistType,
          Path = var_Path
        WHERE ID = @RETURN_VALUE;
    END;
END IF; 
SET  var_ID = @RETURN_VALUE;
END;
-- GO


-- =============================================
-- Author:        <Erwin Kryszat>
-- Create date: <2009, January 21>
-- Description:    <Add a Song to a Playlist>
-- =============================================
CREATE PROCEDURE AddPlaylistSong(
    IN var_PlaylistID int,
    IN var_SequenceNumber int,
    IN var_SongFullPath nvarchar(250)
    )
BEGIN

SELECT * FROM PlaylistSongs
WHERE var_SongFullPath = SongFullPath AND PlaylistID =  var_PlaylistID;

IF found_rows() = 0 THEN
    BEGIN

        INSERT INTO PlaylistSongs
            (
                PlaylistID,
                SequenceNumber,
                SongFullPath    
            )
        VALUES
            (
                var_PlaylistID,
                var_SequenceNumber,
                var_SongFullPath
            );
    END;
ELSE
    BEGIN
        UPDATE PlaylistSongs
        SET PlaylistID =  var_PlaylistID,
            SequenceNumber = var_SequenceNumber,
            SongFullPath =    var_SongFullPath
        WHERE var_SongFullPath = SongFullPath AND PlaylistID =  var_PlaylistID;
    END;
END IF;
END;
-- GO


-- =============================================
-- Author:        <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description:    <Add a Website URL>
-- =============================================
CREATE PROCEDURE AddWebsite(
    IN var_URLtype nvarchar(4),
    IN var_Bandname nvarchar(100),
    IN var_URL nvarchar(250),
    OUT var_ID int    )
BEGIN

SET @RETURN_VALUE = -1;
SELECT ID FROM Websites
WHERE var_URL = URL AND var_Bandname = Bandname
INTO @RETURN_VALUE;

IF  found_rows() = 0 THEN
    BEGIN
        INSERT INTO Websites
            (
                URLtype,
                Bandname,
                URL
            )
        VALUES
            (
                var_URLtype,
                var_Bandname,
                var_URL
            );
        SET @RETURN_VALUE = last_insert_id();
    END;
ELSE
    BEGIN
        UPDATE Websites
        SET URLtype = var_URLtype,
        Bandname = var_Bandname,
        URL = var_URL
        WHERE ID = @RETURN_VALUE;
    END;
END IF;
SET  var_ID = @RETURN_VALUE;
END;
-- GO

-- =============================================
-- Author:        Erwin Kryszat
-- Create date: <2009, September 25>
-- Description:    Delete one Album
-- =============================================
CREATE PROCEDURE DeleteAlbum (
    IN var_AlbumID int,
    OUT var_Rowcount int )
BEGIN

    DELETE FROM Songs
    WHERE Songs.IDAlbum = var_AlbumID;

    DELETE FROM Albums
    WHERE Albums.ID = var_AlbumID;

    SELECT var_Rowcount =  found_rows();
    
    DELETE FROM Images
    WHERE Images.IDAlbum = var_AlbumID;

    SET @var_IDBand = 0;
    SELECT @var_IDBand = IDBand FROM Albums WHERE ID = var_AlbumID AND VariousArtists = 0;
    IF @var_IDBand > 0 THEN
        BEGIN
            SELECT * FROM viewSongs WHERE IDBand = @var_IDBand;
            IF  found_rows() = 0  THEN
                BEGIN
                    DELETE FROM Bands WHERE ID = @var_IDBand;
                END;     
            END IF;
        END;
    END IF;
END;
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

    SET var_Rowcount =  found_rows();
    
END;
-- GO






-- =============================================
-- Author:        Erwin Kryszat
-- Create date: <2009, September 25>
-- Description:    Delete one Band 
-- =============================================
CREATE PROCEDURE DeleteBand(
    IN var_BandID int,
    OUT var_Rowcount int )


BEGIN

    SET @var_Name = '';
    SELECT @var_Name = Name From Bands;

    DELETE FROM Songs
    WHERE Songs.IDBand = var_BandID;

    DELETE FROM Albums
    WHERE Albums.IDBand = var_BandID;

    DELETE FROM Images
    WHERE Images.IDBand = var_BandID;

    DELETE FROM Bands
    WHERE Bands.ID = var_BandID;

    DELETE From AlbumGenres
    WHERE AlbumGenre Like @var_Name;


    SET var_Rowcount =  found_rows();
END;
-- GO

-- =============================================
-- Author:     <Erwin Kryszat>
-- Create date: <2009, September 25>
-- Description:   <Delete a Bookmark>
-- =============================================
CREATE PROCEDURE DeleteBookmark(
    IN var_BookmarkType int,
    IN var_Reference nvarchar(100) )
BEGIN

IF var_BookmarkType = 0 THEN
    BEGIN
    UPDATE Bands
    SET Bookmarked = 0
    WHERE Name = var_Reference;
    END;
END IF;

IF var_BookmarkType = 1 THEN
    BEGIN
    UPDATE Albums
    SET Bookmarked = 0
    WHERE Name = var_Reference;
    END;
END IF;

IF var_BookmarkType = 2 THEN
    BEGIN
    UPDATE Songs
    SET Bookmarked = 0
    WHERE SongTitle = var_Reference;
    END;
END IF;

DELETE FROM Bookmarks
    WHERE var_Reference = Reference
    AND var_BookmarkType = BookmarkType;

END;
-- GO




-- =============================================
-- Author:    Erwin Kryszat
-- Create date: <2009, September 25>
-- Description:    Delete one Composer 
-- =============================================
CREATE PROCEDURE DeleteComposer(
    IN var_ComposerID int,
    OUT var_Rowcount int )
    
BEGIN

    DELETE FROM Songs
    WHERE Songs.IDComposer = var_ComposerID;

    DELETE FROM Composer
    WHERE Composer.ID = var_ComposerID;

    SET var_Rowcount =  found_rows();
END;
-- GO


-- =============================================
-- Author:    Erwin Kryszat
-- Create date: <2009, September 25>
-- Description:    Delete one Conductor 
-- =============================================
CREATE PROCEDURE DeleteConductor(
    IN var_ConductorID int,
    OUT var_Rowcount int )
    
BEGIN

    DELETE FROM Songs
    WHERE Songs.IDConductor = var_ConductorID;

    DELETE FROM Conductor
    WHERE Conductor.ID = var_ConductorID;

    SET var_Rowcount =  found_rows();
END;
-- GO


-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2009, September 25>
-- Description:    Delete one Album
-- =============================================
CREATE PROCEDURE DeleteCountry(
    IN var_CountryID int,
    OUT var_Rowcount int )

BEGIN

    DELETE FROM Countries
    WHERE 
        Countries.ID = var_CountryID;

    SET var_Rowcount =  found_rows();
END;
-- GO




-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2009, September 25>
-- Description:    Delete one Decade
-- =============================================
CREATE PROCEDURE DeleteDecade(
    IN var_Decade int,
    OUT var_Rowcount int )

BEGIN


    DELETE Songs FROM Songs 
        INNER JOIN  Albums ON Albums.ID = Songs.IDAlbum
    WHERE 
        Albums.Year >= var_Decade AND Albums.Year < (var_Decade + 10);

    SET var_Rowcount =  found_rows();

END;
-- GO




-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2009, September 25>
-- Description:    Delete one Genre
-- =============================================

CREATE PROCEDURE DeleteGenre(
    IN var_GenreID int,
    OUT var_Rowcount int )

BEGIN

    DELETE Songs FROM Songs
        INNER JOIN  Genres ON Genres.ID = Songs.IDGenre
    WHERE 
        Genres.ID = var_GenreID;


    DELETE FROM Genres
    WHERE 
        Genres.ID = var_GenreID;

    SET var_Rowcount =  found_rows();

END;
-- GO



-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2009, September 25>
-- Description:    Delete one Album
-- =============================================
CREATE PROCEDURE DeleteLanguage(
    IN var_LanguageID int,
    OUT var_Rowcount int )
    
BEGIN

    DELETE FROM Languages
    WHERE 
        Languages.ID = var_LanguageID;

    SET var_Rowcount =  found_rows();
END;
-- GO


-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2009, September 25>
-- Description:    Delete one LeadPerformer 
-- =============================================
CREATE PROCEDURE DeleteLeadPerformer(
    IN var_LeadPerformerID int,
    OUT var_Rowcount int)

BEGIN

    DELETE FROM Songs
    WHERE Songs.IDLeadPerformer = var_LeadPerformerID;

    DELETE FROM LeadPerformer
    WHERE LeadPerformer.ID = var_LeadPerformerID;

    SET var_Rowcount =  found_rows();
END;
-- GO


-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2009, September 25>
-- Description:    Delete one Playlist 
-- =============================================
CREATE PROCEDURE DeletePlaylist(
    IN var_PlaylistID int,
    OUT var_Rowcount int)

BEGIN

    DELETE FROM PlaylistNames
    WHERE ID = var_PlaylistID;

    DELETE FROM PlaylistSongs
    WHERE PlaylistID = var_PlaylistID;
    
    SET var_Rowcount =  found_rows();
END;
-- GO



-- =============================================
-- Author:    <Erwin Kryszat>
-- Create date:     <2009, September 25>
-- Description:    <Delete a Song from Playlist>
-- =============================================
CREATE PROCEDURE DeletePlaylistSong(
    IN var_SongFullPath nvarchar(250) )
BEGIN
    DELETE FROM PlaylistSongs
    WHERE var_SongFullPath = SongFullPath; 
END;
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
    SET var_Rowcount =  found_rows();

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


-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2009, September 25>
-- Description:    Delete one Album
-- =============================================
CREATE PROCEDURE DeleteVariousArtists(
    OUT var_Rowcount int)

BEGIN


    DELETE Songs FROM Songs
        INNER JOIN  Albums ON Albums.ID = Songs.IDAlbum
    WHERE 
        Albums.VariousArtists = 1;

    DELETE Images FROM Images
        INNER JOIN  Albums ON Albums.ID = Images.IDAlbum
    WHERE 
        Albums.VariousArtists = 1;

    DELETE FROM Albums
    WHERE 
        Albums.VariousArtists = 1;


    SET var_Rowcount =  found_rows();  
    

END;
-- GO


-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2009, September 25>
-- Description:    Delete one Year
-- =============================================
CREATE PROCEDURE DeleteYear(
    IN var_Year int,
    OUT var_Rowcount int)

BEGIN

    DELETE Songs FROM Songs
        INNER JOIN  Albums ON Albums.ID = Songs.IDAlbum
    WHERE 
        Albums.Year = var_Year;

    SET var_Rowcount =  found_rows();
   
END;
-- GO


-- =============================================
-- Author:    <Erwin Kryszat>
-- Create date:     <2009, September 25>
-- Description:    <Move Rows in a Playlist up or down>
-- =============================================
CREATE PROCEDURE PlaylistMoveRows(
    IN var_PlaylistID int,
    IN var_sourceStartIndex int,
    IN var_sourceEndIndex int,
    IN var_targetIndex int
    )
BEGIN

    SET @var_rowsToInsert = 0;

    IF var_sourceStartIndex <=  var_sourceEndIndex THEN
    BEGIN
        SET @var_rowsToInsert = var_sourceEndIndex - var_sourceStartIndex + 1;

        -- create the hole 
        UPDATE PlaylistSongs
        SET   SequenceNumber = SequenceNumber + @var_rowsToInsert
        WHERE var_PlaylistID = PlaylistID 
            AND SequenceNumber >= var_targetIndex;

        -- Move the selected rows down
        IF var_sourceStartIndex < var_targetIndex THEN
            BEGIN
                -- move the selected songs to the hole
                UPDATE PlaylistSongs
                SET   SequenceNumber = SequenceNumber + var_targetIndex - var_sourceStartIndex
                WHERE var_PlaylistID = PlaylistID 
                    AND SequenceNumber >= var_sourceStartIndex
                    AND SequenceNumber <= var_sourceEndIndex;
            END;
        END IF;
        
        -- Move the selected rows up
        IF var_sourceStartIndex > var_targetIndex THEN
            BEGIN
                -- move the selected songs to the hole
                UPDATE PlaylistSongs
                SET   SequenceNumber = SequenceNumber + var_targetIndex - var_sourceStartIndex - @var_rowsToInsert
                WHERE var_PlaylistID = PlaylistID 
                    AND SequenceNumber  >= var_sourceStartIndex + @var_rowsToInsert
                    AND SequenceNumber  <= var_sourceEndIndex + @var_rowsToInsert;
            END;
        END IF;

        CREATE TEMPORARY TABLE TempPlaylist
        (
            PlaylistID int,
            SequenceNumber int,
            SongFullPath nchar(250) 
        );

        INSERT INTO TempPlaylist
        SELECT * 
        FROM PlaylistSongs 
        WHERE var_PlaylistID = PlaylistID
        ORDER BY SequenceNumber;

        -- Renumber the sequenceNumbers from 1 to n in the Temporay Table
        SET @var_index = 0;  
        UPDATE TempPlaylist
        SET   SequenceNumber = @var_index := @var_index + 1
        WHERE var_PlaylistID = PlaylistID ;   

        

        -- Update the new number to the playlist
        UPDATE PlaylistSongs
        JOIN TempPlaylist ON TempPlaylist.SongFullPath = PlaylistSongs.SongFullPath
        SET  PlaylistSongs.SequenceNumber = TempPlaylist.SequenceNumber
        WHERE var_PlaylistID = PlaylistSongs.PlaylistID 
            AND var_PlaylistID = TempPlaylist.PlaylistID;

             
        DROP TABLE TempPlaylist;
    END;
    END IF;
END;
-- GO


-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2009, September 25>
-- Updated 30 April 2009
-- Description:    Purge the databBEGINe
-- =============================================
CREATE PROCEDURE PurgeDatabase()

BEGIN
    DELETE FROM Songs;
    DELETE FROM Albums;
    DELETE FROM Bands;
    DELETE FROM Genres;
    DELETE FROM AlbumGenres;
    DELETE FROM Languages;
    DELETE FROM Countries;
    DELETE FROM Images;
    DELETE FROM Composer;
    DELETE FROM Conductor;
    DELETE FROM LeadPerformer;
    DELETE FROM Websites;
    DELETE FROM ID3Tags;
    
    ALTER TABLE Songs AUTO_INCREMENT = 0;
    ALTER TABLE Albums AUTO_INCREMENT = 0;
    ALTER TABLE Bands AUTO_INCREMENT = 0;
    ALTER TABLE Genres AUTO_INCREMENT = 0;
    ALTER TABLE AlbumGenres AUTO_INCREMENT = 0;
    ALTER TABLE Languages AUTO_INCREMENT = 0;
    ALTER TABLE Countries AUTO_INCREMENT = 0;
    ALTER TABLE Composer AUTO_INCREMENT = 0;    
    ALTER TABLE Conductor AUTO_INCREMENT = 0;
    ALTER TABLE LeadPerformer AUTO_INCREMENT = 0;
    ALTER TABLE Songs AUTO_INCREMENT = 0;
    ALTER TABLE Websites AUTO_INCREMENT = 0;  
    ALTER TABLE ID3Tags AUTO_INCREMENT = 0;   
        
END;
-- GO


-- =============================================
-- Author:        Erwin Kryszat
-- Create date: 03 Januar 2010
-- Description:    Add ID3 Tags
-- =============================================

CREATE PROCEDURE AddID3Tag(
    var_Header nchar(10),
    var_Version int,
    var_Revision int,
    var_Flags int,
    var_Tagname nchar(10),
    var_Tagvalue nvarchar(250),
    var_Tagsize int,
    var_StandardTag int,
    var_Path nvarchar(250),
    var_Filename nvarchar(250)
    )
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
        var_Header,
        var_Version,
        var_Revision,
        var_Flags,
        var_Tagname,
        var_Tagvalue,
        var_Tagsize,
        var_StandardTag,
        var_Path,
        var_Filename
    );
END;
-- GO


-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 02 May 2021
-- Description:	Initialize Languages
-- =============================================
CREATE PROCEDURE InitializeLanguages()
AS
BEGIN

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
END;

-- GO


-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 02 May 2021
-- Description:	Initialize Countries
-- =============================================
CREATE PROCEDURE InitializeCountries()

AS
BEGIN

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
END;
-- GO

-- =============================================
-- Author:		Erwin Kryszat
-- Create date: 22 May 2021
-- Description:	GetStatistics
-- =============================================
CREATE PROCEDURE GetStatistics
AS
BEGIN

	SET @countSongs = 0;
	SET @countAlbums = 0;
	SET @countBands = 0;

	select @countSongs = Count(Path) from Songs
	select @countAlbums = Count(Name) from Albums
	select @countBands = Count(Name) from Bands

	select @countSongs As Songs, @countAlbums as Albums, @countBands as Bands
END;
--GO
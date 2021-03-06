DROP PROCEDURE AddAlbum;
DROP PROCEDURE AddBand;
DROP PROCEDURE AddSong;
DROP PROCEDURE DeleteSong
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
			DatePlayed 
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
			var_DatePlayed 
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
		DatePlayed = var_DatePlayed
    WHERE ID = @RETURN_VALUE;
    END;
END IF;
SET  var_ID = @RETURN_VALUE;
END

--GO

-- =============================================
-- Author:    Erwin Kryszat
-- Create date:     <2009, September 25>
-- Description:    Delete one Song
-- =============================================
CREATE PROCEDURE DeleteSong(
    IN var_SongID int,
    OUT var_Rowcount int)

BEGIN

    DELETE FROM Songs
    WHERE Songs.ID = var_SongID;

    SET var_Rowcount =  found_rows();
END;

-- GO
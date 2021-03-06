
CREATE TABLE AlbumGenres(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	AlbumGenre NVARCHAR(50) 
) ENGINE=InnoDB;
 


CREATE TABLE Albums(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	IDBand INT ,
	Name NVARCHAR(100) ,
	SortName NVARCHAR(100) ,
	Year CHAR(4) ,
	IDAlbumGenre INT ,
	AlbumPath NVARCHAR(250) ,
	VariousArtists INT ,
	Bookmarked INT 
) ENGINE=InnoDB;



CREATE TABLE Bands(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	Name NVARCHAR(100),
	SortName NVARCHAR(100),
    Bookmarked INT  
) ENGINE=InnoDB;


CREATE TABLE Composer(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	Name NVARCHAR(100) 
) ENGINE=InnoDB;


CREATE TABLE Conductor(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	Name NVARCHAR(100) 
) ENGINE=InnoDB;


CREATE TABLE Countries(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	Country NVARCHAR(50),
	Abbreviation NVARCHAR(10),
	FlagPath NVARCHAR(200)
) ENGINE=InnoDB; 


CREATE TABLE Genres(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	Genre NVARCHAR(50) 
) ENGINE=InnoDB;


CREATE TABLE Images(
	IDAlbum INT ,
	IDBand INT ,
	Front NVARCHAR(250) ,
	Back NVARCHAR(250) ,
	Stamp NVARCHAR(250) 
) ENGINE=InnoDB;


CREATE TABLE Languages(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	Language NVARCHAR(50) ,
	Abbreviation NVARCHAR(10) 
) ENGINE=InnoDB;


CREATE TABLE LeadPerformer(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	Name NVARCHAR(100) 
) ENGINE=InnoDB; 


CREATE TABLE Songs(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	IDBand INT ,
	IDAlbum INT ,
	IDLanguage INT ,
	IDGenre INT ,
	Path NVARCHAR(250) ,
	Filename NVARCHAR(250) ,
	IDLeadPerformer INT ,
	IDComposer INT ,
	IDConductor INT ,
	IDCountry INT ,
	SongTitle NVARCHAR(250) ,
	Track INT ,
	Rating INT ,
	LengthINTeger INT ,
	LengthString NVARCHAR(8) ,
	BitRate INT ,
	SampleRate INT ,
	CBR_VBR INT ,
	VA_Flag INT ,
    Bookmarked INT ,
	DateAdded datetime ,
	DatePlayed datetime,
	Comment NVARCHAR(250) ,
    WebsiteUser NVARCHAR(250) ,
    WebsiteArtist NVARCHAR(250)
) ENGINE=InnoDB;

CREATE TABLE Parameter(
	Name NVARCHAR(16),
	Value NVARCHAR(16)
) ENGINE=InnoDB; 

CREATE TABLE Websites(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	URLtype NVARCHAR(4),
	Bandname NVARCHAR(100),
	URL NVARCHAR(250)
) ENGINE=InnoDB; 

CREATE TABLE Bookmarks(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	BookmarkType INT ,
	Reference NVARCHAR(100)
) ENGINE=InnoDB; 

CREATE TABLE PlaylistNames(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY ,
	Name NVARCHAR(100),
	PlaylistType INT ,
	Path NVARCHAR(250)
) ENGINE=InnoDB; 

CREATE TABLE PlaylistSongs(
	PlaylistID INT NOT NULL,
	SequenceNumber INT ,
	SongFullPath NVARCHAR(250)
) ENGINE=InnoDB;

CREATE TABLE ID3Tags(
	ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
	Header NVARCHAR(10),
	Version INT,
	Revision INT,
	Flags INT,
	Tagname NVARCHAR(10),
	Tagvalue NVARCHAR(250),
	Tagsize INT,	
	StandardTag INT,
	Path NVARCHAR(250),
	Filename NVARCHAR(250)
) ENGINE=InnoDB; 

CREATE INDEX idx_albums ON Albums (id);

CREATE  INDEX idx_bands ON Albums (IDBand);

CREATE INDEX idx_albumgenres ON AlbumGenres (id);

CREATE INDEX idx_bands ON Bands (id);

CREATE INDEX idx_bookmarks ON Bookmarks (id);

CREATE INDEX idx_composer ON Composer (id);

CREATE INDEX idx_conductor ON Conductor (id);

CREATE INDEX idx_countries ON Countries (id);

CREATE INDEX idx_genres ON Genres (id);

CREATE INDEX idx_languages ON Languages (id);

CREATE INDEX idx_leadperformer ON LeadPerformer (id);

CREATE INDEX idx_songs ON Songs (id);
CREATE INDEX idx_albums ON Songs (IDAlbum);
CREATE INDEX idx_bands ON Songs (IDBAnd);

CREATE INDEX idx_albums ON Images (IDAlbum);
CREATE INDEX idx_bands ON Images (IDBAnd);

-- GO



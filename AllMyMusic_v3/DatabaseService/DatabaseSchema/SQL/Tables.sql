GO
/****** Object:  Table [dbo].[AlbumGenres]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlbumGenres](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AlbumGenre] [nvarchar](50) 
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Albums]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Albums](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDBand] [int] NULL,
	[Name] [nvarchar](100) ,
	[SortName] [nvarchar](100) ,
	[Year] [char](4) ,
	[IDAlbumGenre] [int] NULL,
	[AlbumPath] [nvarchar](250) ,
	[VariousArtists] [int] NULL,
	[Bookmarked] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Bands]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bands](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100),
	[SortName] [nvarchar](100),
    [Bookmarked] [int] NULL 
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Composer]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Composer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) 
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Conductor]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conductor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) 
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Countries]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Country] [nvarchar](50),
	[Abbreviation] [nvarchar](10),
	[FlagPath] [nvarchar](200)
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Genres]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genres](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Genre] [nvarchar](50) 
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Images]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[IDAlbum] [int] NULL,
	[IDBand] [int] NULL,
	[Front] [nvarchar](250) ,
	[Back] [nvarchar](250) ,
	[Stamp] [nvarchar](250) 
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Languages]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Languages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Language] [nvarchar](50) ,
	[Abbreviation] [nvarchar](10) 
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LeadPerformer]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeadPerformer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) 
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Songs]    Script Date: 12/01/2007 16:48:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Songs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDBand] [int] NULL,
	[IDAlbum] [int] NULL,
	[IDLanguage] [int] NULL,
	[IDGenre] [int] NULL,
	[Path] [nvarchar](250) ,
	[Filename] [nvarchar](250) ,
	[IDLeadPerformer] [int] NULL,
	[IDComposer] [int] NULL,
	[IDConductor] [int] NULL,
	[IDCountry] [int] NULL,
	[SongTitle] [nvarchar](250) ,
	[Track] [int] NULL,
	[Rating] [int] NULL,
	[LengthInteger] [int] NULL,
	[LengthString] [nchar](8) ,
	[BitRate] [int] NULL,
	[SampleRate] [int] NULL,
	[CBR_VBR] [int] NULL,
	[VA_Flag] [int] NULL,
    [Bookmarked] [int] NULL,
	[DateAdded] [datetime] NULL,
	[DatePlayed] [datetime] NULL,
	[Comment] [nvarchar](250) ,
    [WebsiteUser] [nvarchar](250) ,
    [WebsiteArtist] [nvarchar](250)
) ON [PRIMARY]
/****** Object:  Table [dbo].[Parameter]    Script Date: 01/27/2008 22:16:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameter](
	[Name] [nvarchar](16),
	[Value] [nvarchar](16)
) ON [PRIMARY]
/****** Object:  Table [dbo].[Websites]    Script Date: 01/11/2009 14:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Websites](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[URLtype] [nchar](4),
	[Bandname] [nvarchar](100),
	[URL] [nvarchar](250)
) ON [PRIMARY]
/****** Object:  Table [dbo].[Bookmarks]    Script Date: 01/11/2009 14:37:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bookmarks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BookmarkType] [int] NULL,
	[Reference] [nvarchar](100)
) ON [PRIMARY]
/****** Object:  Table [dbo].[Playlists]    Script Date: 01/11/2009 14:38:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlaylistNames](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100),
	[PlaylistType] [int] NULL,
	[Path] [nvarchar](250)
) ON [PRIMARY]
/****** Object:  Table [dbo].[PlaylistSongs]    Script Date: 01/21/2009 18:17:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlaylistSongs](
	[PlaylistID] [int] NOT NULL,
	[SequenceNumber] [int] NULL,
	[SongFullPath] [nvarchar](250)
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ID3Tags]    Script Date: 11/20/2009 22:35:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ID3Tags](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Header] [nvarchar](10) NULL,
	[Version] [int] NULL,
	[Revision] [int] NULL,
	[Flags] [int] NULL,
	[Tagname] [nvarchar](10) NULL,
	[Tagvalue] [nvarchar](250) NULL,
	[Tagsize] [int] NULL,
	[StandardTag] [int] NULL,
	[Path] [nvarchar](250) NULL,
	[Filename] [nvarchar](250) NULL
) ON [PRIMARY]
GO
CREATE CLUSTERED INDEX idx_albums ON Albums (id);
GO
CREATE  INDEX idx_bands ON Albums (IDBand);
GO
CREATE CLUSTERED INDEX idx_albumgenres ON Albumgenres (id);
GO
CREATE CLUSTERED INDEX idx_bands ON Bands (id);
GO
CREATE CLUSTERED INDEX idx_bookmarks ON Bookmarks (id);
GO
CREATE CLUSTERED INDEX idx_composer ON Composer (id);
GO
CREATE CLUSTERED INDEX idx_conductor ON Conductor (id);
GO
CREATE CLUSTERED INDEX idx_countries ON Countries (id);
GO
CREATE CLUSTERED INDEX idx_genres ON Genres (id);
GO
CREATE CLUSTERED INDEX idx_languages ON Languages (id);
GO
CREATE CLUSTERED INDEX idx_leadperformer ON Leadperformer (id);
GO
CREATE CLUSTERED INDEX idx_songs ON Songs (id);
GO
CREATE INDEX idx_albums ON Songs (IDAlbum);
GO
CREATE INDEX idx_bands ON Songs (IDBAnd);
GO
CREATE INDEX idx_albums ON Images (IDAlbum);
GO
CREATE INDEX idx_bands ON Images (IDBAnd);
GO
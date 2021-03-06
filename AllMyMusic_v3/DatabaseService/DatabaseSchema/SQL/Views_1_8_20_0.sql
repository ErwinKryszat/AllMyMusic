ALTER VIEW [dbo].[viewSongs]
AS
SELECT     dbo.Bands.Name AS BandName, dbo.Albums.Name AS AlbumName, dbo.Songs.SongTitle AS SongName, dbo.LeadPerformer.Name AS LeadPerformerName, 
                      dbo.Composer.Name AS ComposerName, dbo.Conductor.Name AS ConductorName, dbo.Songs.Track, dbo.Albums.Year, dbo.Genres.Genre, 
                      dbo.AlbumGenres.AlbumGenre, dbo.Songs.Rating, dbo.Countries.Country, dbo.Languages.Language, dbo.Songs.Path, dbo.Songs.Filename, dbo.Songs.LengthInteger, 
                      dbo.Songs.LengthString, dbo.Songs.BitRate, dbo.Songs.SampleRate, dbo.Songs.CBR_VBR, dbo.Albums.VariousArtists, dbo.Songs.IDBand, dbo.Songs.IDAlbum, 
                      dbo.Songs.ID AS IDSong, dbo.Songs.IDLeadPerformer, dbo.Songs.IDComposer, dbo.Songs.IDConductor, dbo.Songs.IDCountry, dbo.Songs.IDLanguage, 
                      dbo.Albums.IDAlbumGenre, dbo.Genres.ID AS IDGenre, dbo.Songs.Bookmarked, - 1 AS IDPlaylist, - 1 AS SequenceNumber, dbo.Songs.DateAdded, 
                      dbo.Songs.DatePlayed, dbo.Bands.SortName AS BandSortName, dbo.Albums.SortName AS AlbumSortName, dbo.Images.Front, dbo.Images.Back, dbo.Images.Stamp, 
                      dbo.Songs.Comment, dbo.Songs.WebsiteUser, dbo.Songs.WebsiteArtist
FROM         dbo.Songs LEFT OUTER JOIN
                      dbo.Bands ON dbo.Songs.IDBand = dbo.Bands.ID LEFT OUTER JOIN
                      dbo.Albums ON dbo.Songs.IDAlbum = dbo.Albums.ID LEFT OUTER JOIN
                      dbo.Countries ON dbo.Songs.IDCountry = dbo.Countries.ID LEFT OUTER JOIN
                      dbo.AlbumGenres ON dbo.Albums.IDAlbumGenre = dbo.AlbumGenres.ID LEFT OUTER JOIN
                      dbo.Languages ON dbo.Songs.IDLanguage = dbo.Languages.ID LEFT OUTER JOIN
                      dbo.Genres ON dbo.Songs.IDGenre = dbo.Genres.ID LEFT OUTER JOIN
                      dbo.Composer ON dbo.Songs.IDComposer = dbo.Composer.ID LEFT OUTER JOIN
                      dbo.Conductor ON dbo.Songs.IDConductor = dbo.Conductor.ID LEFT OUTER JOIN
                      dbo.LeadPerformer ON dbo.Songs.IDLeadPerformer = dbo.LeadPerformer.ID LEFT OUTER JOIN
                      dbo.Images ON dbo.Songs.IDAlbum = dbo.Images.IDAlbum
GO

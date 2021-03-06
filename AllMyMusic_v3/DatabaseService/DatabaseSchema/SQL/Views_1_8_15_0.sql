ALTER VIEW [dbo].[viewAlbums]
AS
SELECT     dbo.Albums.Name AS AlbumName, dbo.Albums.ID AS IDAlbum, COUNT(dbo.Songs.ID) AS SongCount, 
                      CASE WHEN VariousArtists = 0 THEN dbo.Bands.Name ELSE 'VA' END AS BandName, dbo.Songs.IDBand, dbo.Albums.AlbumPath, dbo.Albums.Year, 
                      dbo.AlbumGenres.AlbumGenre, dbo.Albums.VariousArtists, dbo.Images.Front, dbo.Images.Back, dbo.Images.Stamp, dbo.Albums.IDAlbumGenre, 
                      dbo.Albums.Bookmarked AS BookmarkedAlbum, dbo.Bands.Bookmarked AS BookmarkedBand, SUM(dbo.Songs.LengthInteger) AS TotalLength
FROM         dbo.Songs RIGHT OUTER JOIN
                      dbo.Albums ON dbo.Songs.IDAlbum = dbo.Albums.ID LEFT OUTER JOIN
                      dbo.Bands ON dbo.Songs.IDBand = dbo.Bands.ID LEFT OUTER JOIN
                      dbo.AlbumGenres ON dbo.Albums.IDAlbumGenre = dbo.AlbumGenres.ID LEFT OUTER JOIN
                      dbo.Images ON dbo.Albums.ID = dbo.Images.IDAlbum
GROUP BY dbo.Albums.Name, dbo.Albums.ID, dbo.Albums.VariousArtists, dbo.Bands.Name, dbo.Songs.IDBand, dbo.Albums.AlbumPath, 
                      dbo.Albums.Year, dbo.AlbumGenres.AlbumGenre, dbo.Albums.VariousArtists, dbo.Images.Front, dbo.Images.Back, dbo.Images.Stamp, dbo.Albums.IDAlbumGenre, 
                      dbo.Albums.Bookmarked, dbo.Bands.Bookmarked
GO

ALTER VIEW [dbo].[viewSongs]
AS
SELECT     dbo.Bands.Name AS BandName, dbo.Albums.Name AS AlbumName, dbo.Songs.SongTitle AS SongName, dbo.LeadPerformer.Name AS LeadPerformerName, 
                      dbo.Composer.Name AS ComposerName, dbo.Conductor.Name AS ConductorName, dbo.Songs.Track, dbo.Albums.Year, dbo.Genres.Genre, 
                      dbo.AlbumGenres.AlbumGenre, dbo.Songs.Rating, dbo.Countries.Country, dbo.Languages.Language, dbo.Songs.Path, dbo.Songs.Filename, dbo.Songs.LengthInteger, 
                      dbo.Songs.LengthString, dbo.Songs.BitRate, dbo.Songs.SampleRate, dbo.Songs.CBR_VBR, dbo.Albums.VariousArtists, dbo.Songs.IDBand, dbo.Songs.IDAlbum, 
                      dbo.Songs.ID AS IDSong, dbo.Songs.IDLeadPerformer, dbo.Songs.IDComposer, dbo.Songs.IDConductor, dbo.Songs.IDCountry, dbo.Songs.IDLanguage, 
                      dbo.Albums.IDAlbumGenre, dbo.Genres.ID AS IDGenre, dbo.Songs.Bookmarked, - 1 AS IDPlaylist, - 1 AS SequenceNumber, dbo.Songs.DateAdded, 
                      dbo.Songs.DatePlayed, dbo.Bands.SortName AS BandSortName, dbo.Albums.SortName AS AlbumSortName, dbo.Images.Front, dbo.Images.Back, dbo.Images.Stamp, 
                      dbo.Albums.Bookmarked AS BookmarkedAlbum, dbo.Bands.Bookmarked AS BookmarkedBand
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

ALTER VIEW [dbo].[viewBands]
AS
SELECT     dbo.Bands.Name AS BandName, dbo.Bands.ID AS IDBand, dbo.Bands.SortName, COUNT(dbo.Bands.Name) AS AlbumCount, dbo.Bands.Bookmarked, 
           CASE WHEN dbo.Albums.VariousArtists = 0 THEN 0 ELSE 1 END AS VariousArtists
FROM       dbo.Bands LEFT OUTER JOIN dbo.Albums ON dbo.Albums.IDBand = dbo.Bands.ID
GROUP BY dbo.Bands.Name, dbo.Bands.ID, dbo.Bands.SortName, dbo.Bands.Bookmarked, dbo.Albums.VariousArtists
UNION
SELECT     dbo.Bands.SortName AS BandName, dbo.Bands.ID AS IDBand, dbo.Bands.SortName, COUNT(dbo.Bands.Name) AS AlbumCount, dbo.Bands.Bookmarked, 
           CASE WHEN dbo.Albums.VariousArtists = 0 THEN 0 ELSE 1 END AS VariousArtists
FROM       dbo.Bands LEFT OUTER JOIN dbo.Albums ON dbo.Albums.IDBand = dbo.Bands.ID
WHERE     dbo.Bands.SortName NOT LIKE ''
GROUP BY dbo.Bands.Name, dbo.Bands.ID, dbo.Bands.SortName, dbo.Bands.Bookmarked, dbo.Albums.VariousArtists

GO

ALTER VIEW [dbo].[viewPlaylist]
AS
SELECT     dbo.viewSongs.BandName, dbo.viewSongs.AlbumName, dbo.viewSongs.SongName, dbo.viewSongs.LeadPerformerName, 
                      dbo.viewSongs.ComposerName, dbo.viewSongs.ConductorName, dbo.viewSongs.Track, dbo.viewSongs.Year, dbo.viewSongs.Genre, 
                      dbo.viewSongs.AlbumGenre, dbo.viewSongs.Rating, dbo.viewSongs.Country, dbo.viewSongs.Language, dbo.viewSongs.Path, 
                      dbo.viewSongs.Filename, dbo.viewSongs.LengthInteger, dbo.viewSongs.LengthString, dbo.viewSongs.BitRate, dbo.viewSongs.SampleRate, 
                      dbo.viewSongs.CBR_VBR, dbo.viewSongs.VariousArtists, dbo.viewSongs.IDBand, dbo.viewSongs.IDAlbum, dbo.viewSongs.IDSong, 
                      dbo.viewSongs.IDLeadPerformer, dbo.viewSongs.IDComposer, dbo.viewSongs.IDConductor, dbo.viewSongs.IDCountry, dbo.viewSongs.IDLanguage, 
                      dbo.viewSongs.IDAlbumGenre, dbo.viewSongs.IDGenre, dbo.viewSongs.Bookmarked, dbo.PlaylistSongs.PlaylistID, 
                      dbo.PlaylistSongs.SequenceNumber, dbo.viewSongs.DateAdded, dbo.viewSongs.DatePlayed
FROM         dbo.viewSongs LEFT OUTER JOIN
                      dbo.PlaylistSongs ON dbo.viewSongs.Path + dbo.viewSongs.Filename = dbo.PlaylistSongs.SongFullPath
GO


ALTER VIEW [dbo].[viewAlbumQuality]
AS
SELECT     dbo.Bands.Name AS BandName, dbo.Albums.Name AS AlbumName, COUNT(dbo.Songs.ID) AS SongCount, dbo.Albums.AlbumPath, dbo.Albums.Year, 
                      dbo.Albums.VariousArtists, dbo.Albums.ID AS IDAlbum, AVG(dbo.Songs.BitRate) AS BitRate, dbo.Albums.IDBand
FROM         dbo.Albums RIGHT OUTER JOIN
                      dbo.Bands ON dbo.Albums.IDBand = dbo.Bands.ID RIGHT OUTER JOIN
                      dbo.Songs ON dbo.Songs.IDAlbum = dbo.Albums.ID
GROUP BY dbo.Bands.Name, dbo.Albums.Name, dbo.Albums.AlbumPath, dbo.Albums.Year, dbo.Albums.VariousArtists, dbo.Albums.ID, dbo.Albums.IDBand

GO

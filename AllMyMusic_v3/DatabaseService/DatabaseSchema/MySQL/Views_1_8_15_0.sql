ALTER VIEW viewAlbums AS
SELECT     Albums.Name AS AlbumName, Albums.ID AS IDAlbum, COUNT(Songs.ID) AS SongCount,
                      CASE WHEN VariousArtists = 0 THEN Bands.Name ELSE 'VA' END AS BandName, 
					  Songs.IDBand IDBand, 
                      Albums.AlbumPath, Albums.Year, AlbumGenres.AlbumGenre, Albums.VariousArtists, 
                      Images.Front, Images.Back, Images.Stamp, Albums.IDAlbumGenre, 
                      Albums.Bookmarked AS BookmarkedAlbum, Bands.Bookmarked AS BookmarkedBand, SUM(Songs.LengthInteger) AS TotalLength
FROM         Songs LEFT OUTER JOIN
                      Albums ON Songs.IDAlbum = Albums.ID LEFT OUTER JOIN
                      Bands ON Songs.IDBand = Bands.ID LEFT OUTER JOIN
                      AlbumGenres ON Albums.IDAlbumGenre = AlbumGenres.ID LEFT OUTER JOIN
                      Images ON Albums.ID = Images.IDAlbum
GROUP BY Albums.Name, Albums.ID, Albums.VariousArtists, Bands.Name, Songs.IDBand, Albums.AlbumPath, 
                      Albums.Year, AlbumGenres.AlbumGenre, Albums.VariousArtists, Images.Front, Images.Back, Images.Stamp, Albums.IDAlbumGenre, 
                      Albums.Bookmarked, Bands.Bookmarked;

-- GO  

ALTER VIEW viewSongs AS
SELECT Bands.Name As BandName, Albums.Name AS AlbumName, Songs.SongTitle AS SongName, 
        LeadPerformer.Name AS LeadPerformerName, Composer.Name AS ComposerName, Conductor.Name AS ConductorName, 
        Songs.Track, Albums.Year, Genres.Genre, AlbumGenres.AlbumGenre, Songs.Rating, Countries.Country, 
        Languages.Language, Songs.Path, Songs.Filename, Songs.LengthInteger, Songs.LengthString, Songs.BitRate, 
        Songs.SampleRate, Songs.CBR_VBR,  Albums.VariousArtists AS VariousArtists, Songs.IDBand, Songs.IDAlbum, Songs.ID AS IDSong, 
        Songs.IDLeadPerformer, Songs.IDComposer, Songs.IDConductor, Songs.IDCountry, Songs.IDLanguage, 
        Albums.IDAlbumGenre, IDGenre, Songs.Bookmarked, - 1 AS IDPlaylist, - 1 AS SequenceNumber,
		Songs.DateAdded, Songs.DatePlayed, Bands.SortName AS BandSortName, Albums.SortName AS AlbumSortName,
		Images.Front, Images.Back, Images.Stamp, Albums.Bookmarked AS BookmarkedAlbum , Bands.Bookmarked AS BookmarkedBand
FROM    Songs LEFT OUTER JOIN
        Bands ON Songs.IDBand = Bands.ID LEFT OUTER JOIN
        Albums ON Songs.IDAlbum = Albums.ID LEFT OUTER JOIN
        Countries ON Songs.IDCountry = Countries.ID LEFT OUTER JOIN
        AlbumGenres ON Albums.IDAlbumGenre = AlbumGenres.ID LEFT OUTER JOIN
        Languages ON Songs.IDLanguage = Languages.ID LEFT OUTER JOIN
        Genres ON Songs.IDGenre = Genres.ID LEFT OUTER JOIN
        Composer ON Songs.IDComposer = Composer.ID LEFT OUTER JOIN
        Conductor ON Songs.IDConductor = Conductor.ID LEFT OUTER JOIN
        LeadPerformer ON Songs.IDLeadPerformer = LeadPerformer.ID LEFT OUTER JOIN
		Images ON Songs.IDAlbum = Images.IDAlbum;

-- GO   

ALTER VIEW viewBands AS
SELECT Bands.Name AS BandName, Bands.ID AS IDBand, Bands.SortName, COUNT(Bands.Name) AS AlbumCount, Bands.Bookmarked, 
       CASE WHEN Albums.VariousArtists = 0 THEN 0 ELSE 1 END AS VariousArtists
FROM   Bands LEFT OUTER JOIN  Albums ON Bands.ID = Albums.IDBand
GROUP BY Bands.Name, Bands.ID, Bands.SortName, Bands.Bookmarked, Albums.VariousArtists
UNION
SELECT Bands.SortName AS BandName, Bands.ID AS IDBand, Bands.SortName, COUNT(Bands.Name) AS AlbumCount, Bands.Bookmarked, 
	   CASE WHEN Albums.VariousArtists = 0 THEN 0 ELSE 1 END AS VariousArtists
FROM   Bands LEFT OUTER JOIN  Albums ON Bands.ID = Albums.IDBand
WHERE  Bands.SortName not like ''
GROUP BY Bands.Name, Bands.ID, Bands.SortName, Bands.Bookmarked, Albums.VariousArtists;
 
 -- GO  

ALTER VIEW viewPlaylist AS
SELECT     viewSongs.BandName, viewSongs.AlbumName, viewSongs.SongName, viewSongs.LeadPerformerName, 
                      viewSongs.ComposerName, viewSongs.ConductorName, viewSongs.Track, viewSongs.Year, viewSongs.Genre, 
                      viewSongs.AlbumGenre, viewSongs.Rating, viewSongs.Country, viewSongs.Language, viewSongs.Path, 
                      viewSongs.Filename, viewSongs.LengthInteger, viewSongs.LengthString, viewSongs.BitRate, viewSongs.SampleRate, 
                      viewSongs.CBR_VBR, viewSongs.VariousArtists, viewSongs.IDBand, viewSongs.IDAlbum, viewSongs.IDSong, 
                      viewSongs.IDLeadPerformer, viewSongs.IDComposer, viewSongs.IDConductor, viewSongs.IDCountry, viewSongs.IDLanguage, 
                      viewSongs.IDAlbumGenre, viewSongs.IDGenre, viewSongs.Bookmarked, PlaylistSongs.PlaylistID, PlaylistSongs.SequenceNumber,
					  viewSongs.DateAdded, viewSongs.DatePlayed
FROM         viewSongs LEFT OUTER JOIN
                      PlaylistSongs ON CONCAT(viewSongs.Path,viewSongs.Filename) = PlaylistSongs.SongFullPath;

 -- GO  
 
ALTER VIEW viewAlbumQuality AS
SELECT     Bands.Name AS BandName, Albums.Name AS AlbumName, COUNT(Songs.ID) AS SongCount, Albums.AlbumPath, Albums.Year, 
                      Albums.VariousArtists, Albums.ID AS IDAlbum, AVG(Songs.BitRate) AS BitRate, Albums.IDBand
FROM         Albums RIGHT OUTER JOIN
                      Bands ON Albums.IDBand = Bands.ID RIGHT OUTER JOIN
                      Songs ON Songs.IDAlbum = Albums.ID                               
GROUP BY Bands.Name, Albums.Name, Albums.AlbumPath, Albums.Year, 
         Albums.VariousArtists, Albums.ID, Albums.IDBand
		 
 -- GO  
 
 		 		 
                
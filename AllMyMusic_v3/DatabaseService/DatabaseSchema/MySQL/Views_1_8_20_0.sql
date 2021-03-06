ALTER VIEW viewSongs AS
SELECT Bands.Name As BandName, Albums.Name AS AlbumName, Songs.SongTitle AS SongName, 
        LeadPerformer.Name AS LeadPerformerName, Composer.Name AS ComposerName, Conductor.Name AS ConductorName, 
        Songs.Track, Albums.Year, Genres.Genre, AlbumGenres.AlbumGenre, Songs.Rating, Countries.Country, 
        Languages.Language, Songs.Path, Songs.Filename, Songs.LengthInteger, Songs.LengthString, Songs.BitRate, 
        Songs.SampleRate, Songs.CBR_VBR,  Albums.VariousArtists AS VariousArtists, Songs.IDBand, Songs.IDAlbum, Songs.ID AS IDSong, 
        Songs.IDLeadPerformer, Songs.IDComposer, Songs.IDConductor, Songs.IDCountry, Songs.IDLanguage, 
        Albums.IDAlbumGenre, IDGenre, Songs.Bookmarked, - 1 AS IDPlaylist, - 1 AS SequenceNumber,
		Songs.DateAdded, Songs.DatePlayed, Bands.SortName AS BandSortName, Albums.SortName AS AlbumSortName,
		Images.Front, Images.Back, Images.Stamp, Songs.Comment, Songs.WebsiteUser, Songs.WebsiteArtist
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

 		 		 
                
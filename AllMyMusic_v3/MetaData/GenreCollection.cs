using System;
using System.Collections.Generic;
using System.Text;

namespace Metadata.ID3
{
    /// <summary>
    /// This class is used to:
    /// Collection of genres as defined in ID3V1 standard
    /// </summary>
    public static class GenreCollection
    {
        public static  Genre[] Item = new Genre[148] 
        { 
            new Genre("Blues",0),
            new Genre("Classic Rock",1),
            new Genre("Country",2),
            new Genre("Dance",3),
            new Genre("Disco",4),
            new Genre("Funk",5),
            new Genre("Grunge",6),
            new Genre("Hip-Hop",7),
            new Genre("Jazz",8),
            new Genre("Metal",9),
            new Genre("New Age",10),
            new Genre("Oldies",11),
            new Genre("Other",12),
            new Genre("Pop",13),
            new Genre("Rhythm and Blues",14),
            new Genre("Rap",15),
            new Genre("Reggae",16),
            new Genre("Rock",17),
            new Genre("Techno",18),
            new Genre("Industrial",19),
            new Genre("Alternative",20),
            new Genre("Ska",21),
            new Genre("Death Metal",22),
            new Genre("Pranks",23),
            new Genre("Soundtrack",24),
            new Genre("Euro-Techno",25),
            new Genre("Ambient",26),
            new Genre("Trip-Hop",27),
            new Genre("Vocal", 28),
            new Genre("Jazz&Funk",29),
            new Genre("Fusion",30),
            new Genre("Trance",31),
            new Genre("Classical",32),
            new Genre("Instrumental",33),
            new Genre("Acid",34),
            new Genre("House",35),
            new Genre("Game",36),
            new Genre("Sound Clip",37),
            new Genre("Gospel",38),
            new Genre("Noise",39),
            new Genre("Alternative Rock",40),
            new Genre("Bass",41),
            new Genre("Soul",42),
            new Genre("Punk",43),
            new Genre("Space",44),
            new Genre("Meditative",45),
            new Genre("Instrumental Pop",46),
            new Genre("Instrumental Rock",47),
            new Genre("Ethnic",48),
            new Genre("Gothic",49),
            new Genre("Darkwave",50),
            new Genre("Techno-Industrial",51),
            new Genre("Electronic",52),
            new Genre("Pop-Folk",53),
            new Genre("Eurodance",54),
            new Genre("Dream",55),
            new Genre("Southern Rock",56),
            new Genre("Comedy",57),
            new Genre("Cult",58),
            new Genre("Gangsta",59),
            new Genre("Top 40",60),
            new Genre("Christian Rap",61),
            new Genre("Pop/Funk",62),
            new Genre("Jungle",63),
            new Genre("Native US",64),
            new Genre("Cabaret",65),
            new Genre("New Wave",66),
            new Genre("Psychedelic",67),
            new Genre("Rave",68),
            new Genre("Showtunes",69),
            new Genre("Trailer",70),
            new Genre("Lo-Fi",71),
            new Genre("Tribal",72),
            new Genre("Acid Punk",73),
            new Genre("Acid Jazz",74),
            new Genre("Polka",75),
            new Genre("Retro",76),
            new Genre("Musical",77),
            new Genre("Rock & Roll",78),
            new Genre("Hard Rock",79),
            new Genre("Folk",80),
            new Genre("Folk-Rock",81),
            new Genre("National Folk",82),
            new Genre("Swing",83),
            new Genre("Fast Fusion",84),
            new Genre("Bebob",85),
            new Genre("Latin",86),
            new Genre("Revival",87),
            new Genre("Celtic",88),
            new Genre("Bluegrass",89),
            new Genre("Avantgarde",90),
            new Genre("Gothic Rock",91),
            new Genre("Progressive Rock",92),
            new Genre("Psychedelic Rock",93),
            new Genre("Symphonic Rock",94),
            new Genre("Slow Rock",95),
            new Genre("Big Band",96),
            new Genre("Chorus",97),
            new Genre("Easy Listening",98),
            new Genre("Acoustic",99),
            new Genre("Humour",100),
            new Genre("Speech",101),
            new Genre("Chanson",102),
            new Genre("Opera",103),
            new Genre("Chamber Music",104),
            new Genre("Sonata",105),
            new Genre("Symphony",106),
            new Genre("Booty Bass",107),
            new Genre("Primus",108),
            new Genre("Porn Groove",109),
            new Genre("Satire",110),
            new Genre("Slow Jam",111),
            new Genre("Club",112),
            new Genre("Tango",113),
            new Genre("Samba",114),
            new Genre("Folklore",115),
            new Genre("Ballad",116),
            new Genre("Power Ballad",117),
            new Genre("Rhythmic Soul",118),
            new Genre("Freestyle",119),
            new Genre("Duet",120),
            new Genre("Punk Rock",121),
            new Genre("Drum Solo",122),
            new Genre("A capella",123),
            new Genre("Euro-House",124),
            new Genre("Dance Hall",125),
            new Genre("Goa",126),
            new Genre("Drum & Bass",127),
            new Genre("Club-House",128),
            new Genre("Hardcore",129),
            new Genre("Terror",130),
            new Genre("Indie",131),
            new Genre("BritPop",132),
            new Genre("Negerpunk",133),
            new Genre("Polsk Punk",134),
            new Genre("Beat",135),
            new Genre("Christian Gangsta Rap",136),
            new Genre("Heavy Metal",137),
            new Genre("Black Metal",138),
            new Genre("Crossover",139),
            new Genre("Contemporary Christian",140),
            new Genre("Christian Rock",141),
            new Genre("Merengue",142),
            new Genre("Salsa",143),
            new Genre("Thrash Metal",144),
            new Genre("Anime",145),
            new Genre("Jpop",146),
            new Genre("Synthpop",147)
        };

        public static String GetGenre(Int32 genreID)
        {
            if (genreID < Item.Length)
            {
                return Item[genreID].Name;
            }
            return "";
        }

        public static Byte GetGenreId(String genre)
        {
            for (Byte i = 0; i < Item.Length; i++)
            {
                if (Item[i].Name.ToUpper() == genre.ToUpper())
                {
                    return i;
                }
            }
            return 0;
        }
    }
}

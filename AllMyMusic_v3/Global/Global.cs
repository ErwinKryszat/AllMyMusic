using System;

namespace AllMyMusic_v3
{
    static class Global
    {
        public static String Application = "AllMyMusic_v3";
        public static String Images = "/" + Application + ";component/Resources/Images/";
        public static String Instruments = "/" + Application + ";component/Resources/Instruments/";
        // "/AllMyMusic_v3;component/Resources/images/CD_32.png"


        public static String Resources = Application +  ".Properties.Resources";

        public static String ApplicationDataPath = String.Empty;
        public static String ApplicationSettingsPath;
        //public static String ApplicationLogfilePath;
        public static String RenamePatternFile;
        public static String AutoTagPatternFile;
        public static String DbSetupFile;
        public static String FlagDownloadURL = "http://www.crwflags.com/fotw/images/";
        public static String PartyButtonConfigFile;
        public static String WorldCountriesFile;
        public static String FlagsPath;
        //public static String CachePath;
        //public static String CachePathImages;
        public static String PartyButtonImages;
        public static Random rnd = new Random();
        public static Boolean ViewVaBands = false;
        
    }
}

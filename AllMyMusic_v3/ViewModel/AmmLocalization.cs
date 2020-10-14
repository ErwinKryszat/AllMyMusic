using System;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace AllMyMusic_v3.ViewModel
{
    /// <summary>
    /// This class is used to:
    /// Load text resource strings used in the userinterface according to the currently selected language
    /// </summary>
    public static class AmmLocalization
    {
        public static CultureInfo ci;
        public static ResourceManager rm;

        /// <summary>
        /// en-US or de-De or fr-FR and so on
        /// </summary>
        private static String cultureName = String.Empty;
        public static String CultureName
        {
            
            get { return cultureName; }
        }

        /// <summary>
        /// Activate a new language given by the cultureInfoName
        /// </summary>
        /// <param name="cultureInfoName"></param>
        /// <example en-US or de-De or fr-FR></example>
        public static void Initialize(String languageGUI)
        {
            switch (languageGUI)
	        {
                case "English":
                    cultureName = "en-US";
                    break;
                case "German":
                    cultureName = "de-DE";
                    break;
                case "Français":
                    cultureName = "fr-FR";
                    break;
                case "Nederlands":
                    cultureName = "nl-NL";
                    break;
                case "Polski":
                    cultureName = "pl-PL";
                    break;
		        default:
                    cultureName = "en-US";
                    break;
	        }
           
            ci = new CultureInfo(cultureName);
            rm = new ResourceManager(Global.Resources, Assembly.GetExecutingAssembly());  
        }

        /// <summary>
        /// Gets the localized string according to the specified string name
        /// </summary>
        /// <param name="localizedStringName"></param>
        /// <returns>The localized string</returns>
        public static String GetLocalizedString(String localizedStringName)
        {
            try
            {
                if (ci != null)
                {
                    return rm.GetString(localizedStringName, ci);
                }
                return String.Empty;
            }
            catch (Exception)
            {
                //AmmLogger.logError("Localized String not found: " + localizedStringName, Err.ToString());
                throw;
            }
        }
    }
}

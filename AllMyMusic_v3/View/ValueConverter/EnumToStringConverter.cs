using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;


namespace AllMyMusic.View
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string EnumString;
            try
            {
                EnumString = Enum.GetName((value.GetType()), value);

                switch (EnumString)
                {
                    case "Dutch":
                        return "Nederlands";
                    case "English":
                        return "English";
                    case "French":
                        return "Français";
                    case "Italian":
                        return "Italiano";
                    case "German":
                        return "Deutsch";
                    case "Polish":
                        return "Polski";
                    case "Russian":
                        return "Русский";
                    case "Spanish":
                        return "Español";
                    default:
                        return "English";
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType,  object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (targetType.Name == "LanguagesGUI")
                {
                    switch ((String)value)
                    {
                        case "Deutsch":
                            return LanguagesGUI.German;
                        case "English":
                            return LanguagesGUI.English;
                        case "Français":
                            return LanguagesGUI.French;
                        case "Nederlands":
                            return LanguagesGUI.Dutch;
                        case "Polski":
                            return LanguagesGUI.Polish;
                        default:
                            return LanguagesGUI.English;
                    }
                }
                else if (targetType.Name == "LanguagesWikipedia")
                {
                    switch ((String)value)
                    {
                        case "Deutsch":
                            return LanguagesWikipedia.German;
                        case "English":
                            return LanguagesWikipedia.English;
                        case "Español":
                            return LanguagesWikipedia.Spanish;
                        case "Français":
                            return LanguagesWikipedia.French;
                        case "Italiano":
                            return LanguagesWikipedia.Italian;
                        case "Nederlands":
                            return LanguagesWikipedia.Dutch;
                        case "Polski":
                            return LanguagesWikipedia.Polish;
                        case "Русский":
                            return LanguagesWikipedia.Russian;
                        default:
                            return LanguagesWikipedia.English;
                    }
                }
            }
            return 0;
        }
    }   
}

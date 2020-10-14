using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace Metadata.ID3
{
    /// <summary>
    /// Regular Expression to determine if a text sting matches a specific pattern
    /// </summary>
    static class FormatValidation
    {
        public static Regex IsID3V2xTag = new Regex(@"[A-Z,0-9]{4}", RegexOptions.Compiled);
        public static Regex IsID3V20Tag = new Regex(@"[A-Z,0-9]{3}", RegexOptions.Compiled);
        public static Regex IsNumber = new Regex(@"^\d+$", RegexOptions.Compiled);
        //public static Regex IsTrackNumber = new Regex(@"^[0-9]{1,3}", RegexOptions.Compiled);
        public static Regex IsYear = new Regex(@"^[0-9]+$", RegexOptions.Compiled);
    }
}

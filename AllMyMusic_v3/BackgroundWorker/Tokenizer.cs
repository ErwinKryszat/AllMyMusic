using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;



namespace AllMyMusic
{
    public  class Tokenizer
    {
        private Boolean _replaceUnderscores = true;
        private Boolean _undoUpperCase = false;
        private ObservableCollection<SongItem> _songs;
        private Int32 _directoryCount = 0;

        public Boolean ReplaceUnderscores
        {
            get { return _replaceUnderscores; }
            set
            {
                if (value == _replaceUnderscores)
                    return;

                _replaceUnderscores = value;
            }
        }
        public Boolean UndoUpperCase
        {
            get { return _undoUpperCase; }
            set
            {
                if (value == _undoUpperCase)
                    return;

                _undoUpperCase = value;
            }
        }
        public ObservableCollection<SongItem> Songs
        {
            get { return _songs; }
            set
            {
                if (value == _songs)
                    return;

                _songs = value;

            }
        }

        public Tokenizer(ObservableCollection<SongItem> songs)
        {
            _songs = songs;
        }
        public Tokenizer(ObservableCollection<SongItem> songs, Boolean replaceUnderscores, Boolean undoUpperCase = false)
        {
            _songs = songs;
            _replaceUnderscores = replaceUnderscores;
            _undoUpperCase = undoUpperCase;
        }

        public void ParseFilename(String pattern)
        {
            for (int i = 0; i < _songs.Count; i++)
            {
                ParseFilename(pattern, _songs[i]);
            }
        }
        public void AssembleFilename(String pattern)
        {
            for (int i = 0; i < _songs.Count; i++)
            {
                AssembleFilename(pattern, _songs[i]);
            }
        }
        private void ParseFilename(String pattern, SongItem song)
        {
            // Date: 13 May 2008
            // Name: Erwin Kryszat
            // Pupose: get the values for path and filename according to provided pattern

            // Date: 22 August 2015
            // updated

            Int32 TokenIndex = 0;  // The numer of token currently in processing
            Int32 SearchPos = 0;   // The position until that the current path\filename is searched

            String filename = Path.GetFileNameWithoutExtension(song.SongFilename);
            String extension = Path.GetExtension(song.SongFilename);
            String path = song.SongPath;

            // get the values for the token and put to the cells
            ArrayList splittedPattern = SplitPattern(pattern);

            // the user has selected at least one "\" delimiter
            String fullFileName = ProcessFilename(filename, path);
            

            foreach (String token in splittedPattern)
            {
                String newValue = GetTokenValue(fullFileName, splittedPattern, TokenIndex, SearchPos);
                switch (token.ToLower())
                {
                    case "<album>":
                        song.SetValueByFieldName("AlbumName", newValue);
                        break;
                    case "<band>":
                        song.SetValueByFieldName("BandName", newValue);
                        break;
                    case "<country>":
                        song.SetValueByFieldName("Country", newValue);
                        break;
                    case "<leadperformer>":
                        song.SetValueByFieldName("Leadperformer", newValue);
                        break;
                    case "<genre>":
                        song.SetValueByFieldName("Genre", newValue);
                        break;
                    case "<song>":
                        song.SetValueByFieldName("SongTitle", newValue);
                        break;
                    case "<track>":
                        song.SetValueByFieldName("Track", newValue);
                        break;
                    case "<year>":
                        song.SetValueByFieldName("Year", newValue);
                        break;
                    case "*":
                        SearchPos++;
                        break;
                    default:
                        // fixed string, i.e. delimerter like "-", "_", " ", "\"
                        SearchPos = fullFileName.IndexOf(token, SearchPos) + token.Length;
                        break;
                }
                TokenIndex++;
            }
        }
        private void AssembleFilename(String pattern, SongItem song)
        {
            // Date: 18 July 2008
            // Name: Erwin Kryszat
            // Pupose: Assemble new filename from ID3 Tags according to provided pattern

            Int32 TokenIndex = 0;  // The numer of token currently in processing

            String filename = Path.GetFileNameWithoutExtension(song.SongFilename);
            String extension = Path.GetExtension(song.SongFilename);
            String path = song.SongPath;

            // get the values for the token and put to the cells
            ArrayList splittedPattern = SplitPattern(pattern);

            // the user has selected at least one "\" delimiter
            String fullFileName = ProcessFilename(filename, path);

            StringBuilder NewFilename = new StringBuilder();


            // get the values for the token and put to the cells
            foreach (String token in splittedPattern)
            {
                switch (token.ToLower())
                {
                    case "<abc>":
                        NewFilename.Append(song.BandName.Substring(0, 1).ToUpper());
                        break;
                    case "<album>":
                        NewFilename.Append(FileName.ReplaceForbiddenChars(song.AlbumName));
                        break;
                    case "<band>":
                        NewFilename.Append(FileName.ReplaceForbiddenChars(song.BandName));
                        break;
                    case "<country>":
                        NewFilename.Append(FileName.ReplaceForbiddenChars(song.Country));
                        break;
                    case "<leadperformer>":
                        NewFilename.Append(FileName.ReplaceForbiddenChars(song.LeadPerformer));
                        break;
                    case "<genre>":
                        NewFilename.Append(FileName.ReplaceForbiddenChars(song.Genre));
                        break;
                    case "<song>":
                        NewFilename.Append(FileName.ReplaceForbiddenChars(song.SongTitle));
                        break;
                    case "<track>":
                        if (song.Track.Length == 1)
                        {
                            NewFilename.Append("0" + song.Track);
                        }
                        else
                        {
                            NewFilename.Append(song.Track);
                        }
                        break;
                    case "<year>":
                        NewFilename.Append(song.Year);
                        break;
                    default:
                        // fixed string, i.e. delimerter like "-", "_", " ", "\"
                        NewFilename.Append(token);
                        break;
                }
                TokenIndex++;
            }
            NewFilename.Append(extension);

            song.NewFullPath = NewFilename.ToString().Trim();
        }
        private String ProcessFilename(String filename, String path)
        {
            String fullFileName = filename;
            if (_directoryCount > 0)
            {
                // cut of the part of the path in the beginning. i.e. C:\Doc&Settings\My Music\Rock
                path = path.Substring(0, path.Length - 1);
                String PathStart = path;
                for (int i = 0; i < _directoryCount; i++)
                {
                    Int32 pos = PathStart.LastIndexOf('\\');
                    if (pos >= 0)
                    {
                        PathStart = PathStart.Substring(0, pos);
                    }

                }
                // get only the second part of the path, without i.e. C:\Doc&Settings\My Music\Rock
                if (path.Length > PathStart.Length + 1)
                {
                    path = path.Substring(PathStart.Length + 1);
                }
                fullFileName = path + "\\" + filename;
            }
            return fullFileName;
        }
        private ArrayList SplitPattern(String pattern)
        {
            // Date: 13 May 2008
            // Name: Erwin Kryszat
            // Pupose: parse the pattern for tagging the mp3 files

            Int32 pos = 0;
            Int32 lastDelimiterPos = 0; ;
            Char lastDelimiterType = ' ';
            ArrayList splittedPattern = new ArrayList();

            for (int i = 0; i < pattern.Length; i++)
            {
                switch (pattern[i])
                {
                    case '<':
                        if ((pos < i) && (lastDelimiterType == '>'))
                        {
                            splittedPattern.Add(pattern.Substring(pos + 1, i - pos - 1));
                            pos = i;
                        }
                        if ((pos < i) && (lastDelimiterType != '>'))
                        {
                            splittedPattern.Add(pattern.Substring(pos, i - pos));
                            pos = i;
                        }
                        lastDelimiterPos = i;
                        lastDelimiterType = '<';
                        break;
                    case '>':
                        if ((pos < i) && (lastDelimiterType == '<'))
                        {
                            splittedPattern.Add(pattern.Substring(pos, i - pos + 1));
                            pos = i;
                        }
                        lastDelimiterPos = i;
                        lastDelimiterType = '>';
                        break;
                    case '\\':

                        // \<band>\<album> (<year>)\<track>.  - <song>

                        if ((pos < i) && (lastDelimiterType == '>'))
                        {
                            if (i - pos == 1)
                            {
                                splittedPattern.Add(pattern.Substring(pos + 1, 1));
                            }
                            else
                            {
                                splittedPattern.Add(pattern.Substring(pos + 1, 1));
                                splittedPattern.Add("\\");
                            }

                            pos = i + 1;
                        }
                        if ((pos < i) && (lastDelimiterType != '>'))
                        {
                            splittedPattern.Add(pattern.Substring(pos, i - pos));
                            pos = i + 1;
                        }
                        if (pos == i)
                        {
                            splittedPattern.Add("\\");
                            pos = i + 1;
                        }

                        break;
                    case '*':
                        if (pos < i)
                        {
                            if (i - pos == 1)
                            {
                                splittedPattern.Add(pattern.Substring(pos + 1, 1));
                            }
                            else
                            {
                                splittedPattern.Add(pattern.Substring(pos + 1, 1));
                                splittedPattern.Add("*");
                            }

                            pos = i + 1;
                        }
                        break;
                    default:
                        break;
                }
            }
            if ((pos + 1 < pattern.Length) && (lastDelimiterType == '>'))
            {
                splittedPattern.Add(pattern.Substring(pos + 1, pattern.Length - pos - 1));
            }


            foreach (String var in splittedPattern)
            {
                if (var == "\\")
                {
                    _directoryCount++;
                }
            }

            return splittedPattern;
        }
        private String GetTokenValue(String Filename, ArrayList SplittedPattern, Int32 i, Int32 start)
        {
            // Date: 13 May 2008
            // Name: Erwin Kryszat
            // Pupose: retrieve the string between two delimiters or between one delimiter and the end

            Int32 pos = -1;

            // find the position of the next "fixed-string-delimeter"
            if ((i + 1) < SplittedPattern.Count)
            {
                pos = Filename.IndexOf(SplittedPattern[i + 1].ToString(), start);
            }
            else
            {
                pos = Filename.Length;
            }

            if (pos >= 0)
            {
                if (pos > start)
                {
                    String value = Filename.Substring(start, (pos - start));
                    if (_replaceUnderscores == true)
                    {
                        value = value.Replace('_', ' ');
                    }
                    if (_undoUpperCase == true)
                    {
                        // Convert "ERWIN" to "Erwin"
                        value = value.Substring(0,1) + value.Substring(1,value.Length -1).ToLower();
                    }
                    return value.Trim();
                }
                else
                {
                    return "";
                }

            }
            return "";
        }

    }
}

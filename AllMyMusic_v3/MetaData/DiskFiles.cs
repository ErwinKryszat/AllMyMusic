using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;


using Metadata.Mp3;

namespace AllMyMusic_v3.ViewModel
{
    public class DiskFiles : ViewModelBase
    {
        private Int32 minimalFileSize = 10240;
        public Int32 MinimalFileSize
        {
            get { return minimalFileSize; }
            set { minimalFileSize = value; }
        }

        private ObservableCollection<SongItem> _songs;
        public ObservableCollection<SongItem> Songs
        {
            get { return _songs; }
            set
            {
                if (value == _songs)
                    return;

                _songs = value;

                RaisePropertyChanged("Songs");
            }
        }


        private String folderName = String.Empty;
        public DiskFiles(String folderName, Int32 minimalFileSize)
        {
            this.folderName = folderName;
            this.minimalFileSize = Math.Max(10240, minimalFileSize);
            _songs = new ObservableCollection<SongItem>();
            ReadFolder();
        }

        private void ReadFolder()
        {
            DirectoryInfo di = new DirectoryInfo(folderName);
            FileInfo[] musicFiles = di.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < musicFiles.Length; i++)
            {
                SongItem song = new SongItem();

                // Copy data from the MusicFile to the Row
                FileInfo fi = musicFiles[i];
                if (fi.Length < minimalFileSize)
                {
                    // Ignore small files and files with size = 0
                    continue;
                }

                
                // Date Collection for the current file
                Mp3Metaedit fileInfoMP3 = new Mp3Metaedit(fi.FullName);
                song = fileInfoMP3.Song;
                _songs.Add(song);
            }
        }
    }
}

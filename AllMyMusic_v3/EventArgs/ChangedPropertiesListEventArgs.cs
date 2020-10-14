using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;



namespace AllMyMusic_v3
{
    public class ChangedPropertiesListEventArgs : EventArgs
    {
        private ToolType _toolType;
        public ToolType ToolType
        {
            get { return _toolType; }
        }

        private ChangedPropertiesList _changedProperties;
        public ChangedPropertiesList ChangedProperties
        {
            get { return _changedProperties; }
        }

        private ObservableCollection<SongItem> _songs;
        public ObservableCollection<SongItem> Songs
        {
            get { return _songs; }
        }

        public ChangedPropertiesListEventArgs(ToolType toolType, ChangedPropertiesList changedProperties, ObservableCollection<SongItem> songs)
        {
            _toolType = toolType;
            _changedProperties = changedProperties;
            _songs = songs;
        }
    }
}

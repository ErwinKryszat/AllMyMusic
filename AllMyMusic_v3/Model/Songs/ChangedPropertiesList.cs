using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace AllMyMusic
{
    public class ChangedPropertiesList : List<KeyValuePair<string, object>>
    {
        //public List<KeyValuePair<string, object>> ListChanges = null;



        public void Add(String propertyName, object value)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Key == propertyName)
                {
                    this[i] = new KeyValuePair<string, object>(propertyName, value);
                    return;
                }
            }
            this.Add(new KeyValuePair<string, object>(propertyName, value));
        }

        public void UpdateSong(SongItem song)
        {
            for (int i = 0; i < this.Count; i++)
            {
                KeyValuePair<string, object> item = this[i];
                song.SetValueByFieldName(item.Key, item.Value);
            }
        }
    }
}

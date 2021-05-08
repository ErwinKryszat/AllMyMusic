using System;
using System.Collections.Generic;
using System.Text;

namespace AllMyMusic.Settings
{
    public class ColumnConfigCollection : List<dgColumnConfig>
    {
        public ColumnConfigCollection(String configName)
        {
            this.configName = configName;
        }

        private String configName = String.Empty;
        public String ConfigName
        {
            get { return configName; }
            set { configName = value; }
        }
    }

    public class dgColumnConfig
    {
        private String columnName = String.Empty;
        public String ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        private Int32 width = 50;
        public Int32 Width
        {
            get { return width; }
            set { width = value; }
        }

        private Int32 displayIndex;
        public Int32 DisplayIndex
        {
            get { return displayIndex; }
            set { displayIndex = value; }
        }

        private Boolean visible;
        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }
    }

}

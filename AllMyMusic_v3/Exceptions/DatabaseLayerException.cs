using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllMyMusic
{
    [Serializable]
    public class DatabaseLayerException :Exception
    {
        public DatabaseLayerException() : base() { }
        public DatabaseLayerException(string message) : base(message) { }
        public DatabaseLayerException(string message, Exception inner) : base(message, inner) { }

        protected DatabaseLayerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    }
}

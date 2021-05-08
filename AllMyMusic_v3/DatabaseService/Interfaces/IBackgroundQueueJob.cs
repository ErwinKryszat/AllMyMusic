using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllMyMusic
{
    public interface IBackgroundQueueJob
    {
        Task DoWork(object taskQueueItem);
    }
}

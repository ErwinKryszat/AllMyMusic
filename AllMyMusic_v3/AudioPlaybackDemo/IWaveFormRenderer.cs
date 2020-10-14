using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllMyMusic_v3
{
    public interface IWaveFormRenderer
    {
        void AddValue(float maxValue, float minValue);
    }
}

using System;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Threading;

namespace AllMyMusic_v3.Controls
{
    [Export(typeof(IVisualizationPlugin))]
    class PolylineWaveFormVisualization : IVisualizationPlugin
    {
        private PolylineWaveFormControl polylineWaveFormControl = new PolylineWaveFormControl();

        public string Name
        {
            get { return "Polyline WaveForm Visualization"; }
        }

        public object Content
        {
            get { return polylineWaveFormControl; }
        }

        public void OnMaxCalculated(float min, float max)
        {
            //Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => polylineWaveFormControl.AddValue(max, min)));
            polylineWaveFormControl.AddValue(max, min);
        }

        public void OnFftCalculated(NAudio.Dsp.Complex[] result)
        {
            // nothing to do
        }
    }
}

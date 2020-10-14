using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AllMyMusic_v3.Settings
{
    public class AudioSettings
    {
        public AudioSettings()
        {

        }

        #region Properties
        private Boolean alwaysOnTop = false;
        public Boolean AlwaysOnTop
        {
            get { return alwaysOnTop; }
            set { alwaysOnTop = value; }
        }

        private Boolean randomPlayer = false;
        public Boolean RandomPlayer
        {
            get { return randomPlayer; }
            set { randomPlayer = value; }
        }

        private String outputDriver = "DirectSound";
        public String OutputDriver
        {
            get { return outputDriver; }
            set { outputDriver = value; }
        }

        private Int32 latency = 125;
        public Int32 Latency
        {
            get { return latency; }
            set { latency = value; }
        }

        private float volume = 1;
        public float Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        private float equalizerLoBand = 0;
        public float EqualizerLoBand
        {
            get { return equalizerLoBand; }
            set { equalizerLoBand = value; }
        }

        private float equalizerMedBand = 0;
        public float EqualizerMedBand
        {
            get { return equalizerMedBand; }
            set { equalizerMedBand = value; }
        }

        private float equalizerHighBand = 0;
        public float EqualizerHighBand
        {
            get { return equalizerHighBand; }
            set { equalizerHighBand = value; }
        }
        #endregion

        public void Save(XmlDocument doc, XmlNode node)
        {
            try
            {
                XmlNode nodeAudio = doc.CreateElement("audio");
                node.AppendChild(nodeAudio);

                XmlNode nodeOutputDriver = doc.CreateElement("outputDriver");
                nodeOutputDriver.InnerText = outputDriver;
                nodeAudio.AppendChild(nodeOutputDriver);

                XmlNode nodeLatencey = doc.CreateElement("latencey");
                nodeLatencey.InnerText = latency.ToString();
                nodeAudio.AppendChild(nodeLatencey);

                XmlNode nodeVolume = doc.CreateElement("volume");
                nodeVolume.InnerText = volume.ToString();
                nodeAudio.AppendChild(nodeVolume);

                XmlNode nodeAlwaysOnTop = doc.CreateElement("alwaysOnTop");
                nodeAlwaysOnTop.InnerText = alwaysOnTop.ToString();
                nodeAudio.AppendChild(nodeAlwaysOnTop);

                XmlNode nodeRandomPlayer = doc.CreateElement("randomPlayer");
                nodeRandomPlayer.InnerText = randomPlayer.ToString();
                nodeAudio.AppendChild(nodeRandomPlayer);

                XmlNode nodeEqualizerLoBand = doc.CreateElement("equalizerLoBand");
                nodeEqualizerLoBand.InnerText = equalizerLoBand.ToString();
                nodeAudio.AppendChild(nodeEqualizerLoBand);

                XmlNode nodeEqualizerMedBand = doc.CreateElement("equalizerMedBand");
                nodeEqualizerMedBand.InnerText = equalizerMedBand.ToString();
                nodeAudio.AppendChild(nodeEqualizerMedBand);

                XmlNode nodeEqualizerHighBand = doc.CreateElement("equalizerHighBand");
                nodeEqualizerHighBand.InnerText = equalizerHighBand.ToString();
                nodeAudio.AppendChild(nodeEqualizerHighBand);
            }
            catch (Exception Err)
            {
                String errorMessage = "Error saving Audio settings.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
        public void Load(XmlNode node)
        {
            try
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "outputDriver":
                            outputDriver = childNode.InnerText;
                            break;
                        case "latencey":
                            latency = Convert.ToInt32(childNode.InnerText);
                            break;
                        case "alwaysOnTop":
                            alwaysOnTop = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "randomPlayer":
                            randomPlayer = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "volume":
                            volume = (float)Math.Min(Convert.ToDouble(childNode.InnerText), 1.0);
                            break;
                        case "equalizerLoBand":
                            equalizerLoBand = (float)Math.Min(Convert.ToDouble(childNode.InnerText), 1.0);
                            break;
                        case "equalizerMedBand":
                            equalizerMedBand = (float)Math.Min(Convert.ToDouble(childNode.InnerText), 1.0);
                            break;
                        case "equalizerHighBand":
                            equalizerHighBand = (float)Math.Min(Convert.ToDouble(childNode.InnerText), 1.0);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception Err)
            {
                String errorMessage = "Error loading Audio settings.";
                ShowError.ShowAndLog(Err, errorMessage, 2001);
            }
        }
    }
}

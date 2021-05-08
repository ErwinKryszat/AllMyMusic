using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Xml;


namespace AllMyMusic.WebServices
{
    public static class WebDownload
    {
        public static Boolean DownloadFile(String remoteURL, String localPath)
        {
            try
            {
                WebClient client = new WebClient();
                client.UseDefaultCredentials = true;

                Uri myUrL = new Uri(remoteURL);
                client.DownloadFile(myUrL, localPath);
                
                return true;
            }
            catch 
            {
               return false;
            }
        }
    }
}

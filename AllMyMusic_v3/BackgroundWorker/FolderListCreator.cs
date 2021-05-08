using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using AllMyMusic.ViewModel;

namespace AllMyMusic
{
    public class FolderListCreator
    {
        private Int32 count;
        private List<String> folders;
        private Boolean isCancellationRequested;


        public Int32 Count
        {
            get { return count; }
        }

        public Boolean IsCancellationRequested
        {
            get { return isCancellationRequested; }
            set { isCancellationRequested = value; }
        }

        public List<String> Folders
        {
            get { return folders; }
        }


        private ProgressDataViewModel progressData;
        private ReportProgress_Callback progress_Callback;
        private WorkDone_Callback done_Callback;

        public FolderListCreator(List<String> folderList, ReportProgress_Callback progress_Callback, WorkDone_Callback done_Callback)
        {
            this.progress_Callback = progress_Callback;
            this.done_Callback = done_Callback;

            folders = new List<string>();
            progressData = new ProgressDataViewModel();
            for (int i = 0; i < folderList.Count; i++)
            {
                folders.Add(folderList[i]);
                AddSubfolder(folderList[i]);
                count++;

                progressData.ProgressValue = 0;
                progressData.ProgressMaximum = count;
                progressData.FolderCount = "0 / " + count.ToString();
                progressData.CurrentFolder = folderList[i];

                if (progress_Callback != null)
                {
                    progress_Callback(progressData);
                }

                if (isCancellationRequested == true)
                {
                    break;
                }
            }

            if (done_Callback != null)
            {
                done_Callback();
            }
        }

        private void AddSubfolder(String folder)
        {
            if (Directory.Exists(folder) == true)
            {
                String[] subFolders = Directory.GetDirectories(folder);
                for (int i = 0; i < subFolders.Length; i++)
                {
                    folders.Add(subFolders[i]);
                    AddSubfolder(subFolders[i]);
                    count++;

                    progressData.ProgressValue = 0;
                    progressData.ProgressMaximum = count;
                    progressData.FolderCount = "0 / " + count.ToString();
                    progressData.CurrentFolder = subFolders[i];

                    if (progress_Callback != null)
                    {
                        progress_Callback(progressData);
                    }

                    if (isCancellationRequested == true)
                    {
                        break;
                    }
                }
            }
        }
    }
}

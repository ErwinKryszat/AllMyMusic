
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

using AllMyMusic_v3.Settings;

namespace AllMyMusic_v3.ViewModel
{
    public class RenameToolViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ObservableCollection<String> _renamePatternList;
        private String _renamePattern;

        private Boolean _patternListChanged;        
        #endregion // Fields

        #region Commands
       
        #endregion // Commands

        #region Presentation Properties
        public ObservableCollection<String> RenamePatternList
        {
            get { return _renamePatternList; }
            set
            {
                if (value == _renamePatternList)
                    return;

                _renamePatternList = value;

                RaisePropertyChanged("RenamePatternList");
            }
        }
        public String RenamePattern
        {
            get { return _renamePattern; }
            set
            {
                if (value == _renamePattern)
                    return;

                _renamePattern = value;
                RaisePropertyChanged("RenamePattern");

                if (_renamePattern != null)
                {
                    EventArgs args = new EventArgs();
                    OnPreviewRequested(this, args);
                }
            }
        }
       
        public Boolean PatternListChanged
        {
            get { return _patternListChanged; }
            set
            {
                if (value == _patternListChanged)
                    return;

                _patternListChanged = value;

                RaisePropertyChanged("PatternListChanged");
            }
        }
        #endregion

        #region Constructor
        public RenameToolViewModel()
        {

        }
        public void Init()
        {
            LoadPatternList();
            Localize();
        }
        public void Close()
        {
            AppSettings.FormSettings.FrmTools_RenameSelectedPattern = RenamePattern;
            if (_patternListChanged == true)
            {
                // remove emapty rows before saving file
                ObservableCollection<String> pattern = new ObservableCollection<string>();

                pattern.Add("; Pattern for the Rename tool");
                pattern.Add("; Add patterns here");
                pattern.Add("; Put most frequent used patterns to the top");
                pattern.Add(";");
                pattern.Add(@"; Save at: C:\Users\<Your Name>\AppData\Roaming\AllMyMusic");
                pattern.Add(";");


                for (int i = 0; i < _renamePatternList.Count; i++)
                {
                    if (String.IsNullOrEmpty(_renamePatternList[i]) == false)
                    {
                        pattern.Add(_renamePatternList[i]);
                    }
                }
                ResourceHelper.WriteTextFile(Global.RenamePatternFile, pattern);
            }
        }
        #endregion

        #region public
       
        #endregion

        #region private
        private void LoadPatternList()
        {
            _renamePatternList = new ObservableCollection<string>();

            if (File.Exists(Global.RenamePatternFile) == false)
            {
                ResourceHelper.CopyResourceTextFileToFilesystem("AllMyMusic_v3.Resources.Text.renameFiles.txt", Global.RenamePatternFile);
            }

            if (File.Exists(Global.RenamePatternFile) == true)
            {
                StreamReader reader = new StreamReader(Global.RenamePatternFile);
                while (reader.EndOfStream != true)
                {
                    String line = reader.ReadLine();
                    if ((String.IsNullOrEmpty(line) == false) && (line.Trim().Substring(0, 1) != ";"))
                    {
                        _renamePatternList.Add(line);
                    }
                }
                reader.Close();
            }

            RaisePropertyChanged("RenamePatternList");

            RenamePattern = AppSettings.FormSettings.FrmTools_RenameSelectedPattern;
        }
        #endregion

        #region Events
        public delegate void PreviewRequestedEventHandler(object sender, EventArgs e);
        public event PreviewRequestedEventHandler PreviewRequested;
        protected virtual void OnPreviewRequested(object sender, EventArgs e)
        {
            if (this.PreviewRequested != null)
            {
                this.PreviewRequested(this, e);
            }
        }

        #endregion // Events

        #region Localization
        private String _renameToolLabel = "Rename MP3 files  based on ID3 Tags";
        public String RenameToolLabel
        {
            get { return _renameToolLabel; }
            set
            {
                if (value == _renameToolLabel)
                    return;

                _renameToolLabel = value;

                RaisePropertyChanged("RenameToolLabel");
            }
        }
        private void Localize()
        {
            RenameToolLabel = AmmLocalization.GetLocalizedString("frmRename_Label1");
        }
        #endregion // Localization

        #region IDisposable Members
        public void Dispose()
        {
            // If this function is being called the user wants to release the
            // resources. lets call the Dispose which will do this for us.
            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
                ReleaseManagedResources();
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            ReleaseUnmangedResources();
        }

        ~RenameToolViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
  
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}

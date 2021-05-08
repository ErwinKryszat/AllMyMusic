
using System;
using System.ComponentModel;
using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

using AllMyMusic.Settings;

namespace AllMyMusic.ViewModel
{
    public class AutoTagToolViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private ObservableCollection<String> _autoTagPatternList;
        private String _autoTagPattern;
        private Boolean _replaceUnderscores = true;
        private Boolean _undoUpperCase = false;
        private Boolean _patternListChanged;
        private ChangedPropertiesList _changedProperties;
        #endregion // Fields

        #region Commands

        #endregion // Commands

        #region Presentation Properties
        public ObservableCollection<String> AutoTagPatternList
        {
            get { return _autoTagPatternList; }
            set
            {
                if (value == _autoTagPatternList)
                    return;

                _autoTagPatternList = value;

                RaisePropertyChanged("AutoTagPatternList");
            }
        }
        public String AutoTagPattern
        {
            get { return _autoTagPattern; }
            set
            {
                if (value == _autoTagPattern)
                    return;

                _autoTagPattern = value;
                RaisePropertyChanged("AutoTagPattern");

                if (_autoTagPattern != null)
                {
                    EventArgs args = new EventArgs();
                    OnPreviewRequested(this, args);
                }
            }
        }
        public Boolean ReplaceUnderscores
        {
            get { return _replaceUnderscores; }
            set
            {
                if (value == _replaceUnderscores)
                    return;

                _replaceUnderscores = value;

                RaisePropertyChanged("ReplaceUnderscores");
            }
        }
        public Boolean UndoUpperCase
        {
            get { return _undoUpperCase; }
            set
            {
                if (value == _undoUpperCase)
                    return;

                _undoUpperCase = value;

                RaisePropertyChanged("UndoUpperCase");
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
        public ChangedPropertiesList ChangedProperties
        {
            get { return _changedProperties; }
            set
            {
                if (value == _changedProperties)
                    return;

                _changedProperties = value;

                RaisePropertyChanged("ChangedProperties");
            }
        }
        #endregion

        #region Constructor
        public AutoTagToolViewModel()
        {
            _changedProperties = new ChangedPropertiesList();
        }
        public void Init()
        { 
            LoadPatternList();
            Localize();
        }
        public void Close()
        {
            AppSettings.FormSettings.FrmTools_AutoTagSelectedPattern = AutoTagPattern;
            if (_patternListChanged == true)
            {
                // remove emapty rows before saving file
                ObservableCollection<String> pattern = new ObservableCollection<string>();

                pattern.Add("; Pattern for the Auto-Tag tool");
                pattern.Add("; Add patterns here");
                pattern.Add("; Put most frequent used patterns to the top");
                pattern.Add(";");
                pattern.Add(@"; Save at: C:\Users\<Your Name>\AppData\Roaming\AllMyMusic");
                pattern.Add(";");

                for (int i = 0; i < _autoTagPatternList.Count; i++)
                {
                    if (String.IsNullOrEmpty(_autoTagPatternList[i]) == false)
                    {
                        pattern.Add(_autoTagPatternList[i]);
                    }
                }
                ResourceHelper.WriteTextFile(Global.AutoTagPatternFile, pattern);
            }
        }
        #endregion

        #region public
       
        #endregion

        #region private
        private void LoadPatternList()
        {
            _autoTagPatternList = new ObservableCollection<string>();

            if (File.Exists(Global.AutoTagPatternFile) == false)
            {
                ResourceHelper.CopyResourceTextFileToFilesystem("AllMyMusic.Resources.Text.autotagFiles.txt", Global.AutoTagPatternFile);
            }

            if (File.Exists(Global.AutoTagPatternFile) == true)
            {
                StreamReader reader = new StreamReader(Global.AutoTagPatternFile);
                while (reader.EndOfStream != true)
                {
                    String line = reader.ReadLine();
                    if ((String.IsNullOrEmpty(line) == false) && (line.Trim().Substring(0, 1) != ";"))
                    {
                        _autoTagPatternList.Add(line);
                    }
                }
                reader.Close();
            }

            RaisePropertyChanged("AutoTagPatternList");

            AutoTagPattern = AppSettings.FormSettings.FrmTools_AutoTagSelectedPattern;
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
        private String _autoTagToolLabel = "Update the ID3 Tags based on the filename.";
        private String _replaceUnderscoresLabel = "Replace underscores '_' with spaces '  '";
        private String _undoUpperCaseLabel = "Undo uppercase. 'TEXT' -> 'Text'";

        public String AutoTagToolLabel
        {
            get { return _autoTagToolLabel; }
            set
            {
                if (value == _autoTagToolLabel)
                    return;

                _autoTagToolLabel = value;

                RaisePropertyChanged("AutoTagToolLabel");
            }
        }
        public String ReplaceUnderscoresLabel
        {
            get { return _replaceUnderscoresLabel; }
            set
            {
                if (value == _replaceUnderscoresLabel)
                    return;

                _replaceUnderscoresLabel = value;

                RaisePropertyChanged("ReplaceUnderscoresLabel");
            }
        }
        public String UndoUpperCaseLabel
        {
            get { return _undoUpperCaseLabel; }
            set
            {
                if (value == _undoUpperCaseLabel)
                    return;

                _undoUpperCaseLabel = value;

                RaisePropertyChanged("UndoUpperCaseLabel");
            }
        }

        private void Localize()
        {
            AutoTagToolLabel = AmmLocalization.GetLocalizedString("frmAutoTag_Label1");
            ReplaceUnderscoresLabel = AmmLocalization.GetLocalizedString("frmAutoTag_ReplaceUnderscores");
            UndoUpperCaseLabel = AmmLocalization.GetLocalizedString("frmAutoTag_ToLowerCase");
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

        ~AutoTagToolViewModel()
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

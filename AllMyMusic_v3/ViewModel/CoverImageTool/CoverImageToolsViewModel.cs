

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Drawing;

using AllMyMusic.Settings;

namespace AllMyMusic.ViewModel
{
    public class CoverImageToolsViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        //private static Logger logger = LogManager.GetCurrentClassLogger();

        private Boolean _removeImages;
        private Boolean _insertStampImage;
        private Boolean _insertFrontImage;
        private Boolean _renameImageFiles;
        private Boolean _createStampsFiles;
        private Boolean _createFolderImage;
        private Boolean _saveImageToDisk;
        private Int32 _stampSize;
        private MultiFolderSelectViewModel _multiFolderSelectVM;
        #endregion // Fields

        #region Commands    
       
        #endregion // Command

        #region Presentation Properties

        public Boolean RemoveImages
        {
            get { return _removeImages; }
            set
            {
                if (value == _removeImages)
                    return;

                _removeImages = value;

                RaisePropertyChanged("RemoveImages");
            }
        }
        public Boolean InsertStampImage
        {
            get { return _insertStampImage; }
            set
            {
                if (value == _insertStampImage)
                    return;

                _insertStampImage = value;

                RaisePropertyChanged("InsertStampImage");
            }
        }
        public Boolean InsertFrontImage
        {
            get { return _insertFrontImage; }
            set
            {
                if (value == _insertFrontImage)
                    return;

                _insertFrontImage = value;

                RaisePropertyChanged("InsertFrontImage");
            }
        }
        public Boolean RenameImageFiles
        {
            get { return _renameImageFiles; }
            set
            {
                if (value == _renameImageFiles)
                    return;

                _renameImageFiles = value;

                RaisePropertyChanged("RenameImageFiles");
            }
        }
        public Boolean CreateStampsFiles
        {
            get { return _createStampsFiles; }
            set
            {
                if (value == _createStampsFiles)
                    return;

                _createStampsFiles = value;

                RaisePropertyChanged("CreateStampsFiles");
            }
        }
        public Boolean CreateFolderImage
        {
            get { return _createFolderImage; }
            set
            {
                if (value == _createFolderImage)
                    return;

                _createFolderImage = value;

                RaisePropertyChanged("CreateFolderImage");
            }
        }
        public Boolean SaveImageToDisk
        {
            get { return _saveImageToDisk; }
            set
            {
                if (value == _saveImageToDisk)
                    return;

                _saveImageToDisk = value;

                RaisePropertyChanged("SaveImageToDisk");
            }
        }
        public Int32 StampSize
        {
            get { return _stampSize; }
            set
            {
                if (value == _stampSize)
                    return;

                _stampSize = value;

                RaisePropertyChanged("StampSize");
            }
        }
        public MultiFolderSelectViewModel MultiFolderSelectVM
        {
            get { return _multiFolderSelectVM; }
            set
            {
                if (value == _multiFolderSelectVM)
                    return;

                _multiFolderSelectVM = value;

                RaisePropertyChanged("MultiFolderSelectVM");
            }
        }
        #endregion // Presentation Properties

        #region Constructor
        public CoverImageToolsViewModel()
        {
            if (IsInDesignMode == true)
            {
                
            }
            else
            {
                _multiFolderSelectVM = new MultiFolderSelectViewModel();

                _createFolderImage = AppSettings.CoverImageSettings.CreateStamps;
                _createStampsFiles = AppSettings.CoverImageSettings.CreateStamps;
                _insertFrontImage = AppSettings.CoverImageSettings.InsertFrontImage;
                _insertStampImage = AppSettings.CoverImageSettings.InsertStampImage;
                _removeImages = AppSettings.CoverImageSettings.RemoveImages;
                _renameImageFiles = AppSettings.CoverImageSettings.RenameImages;
                _saveImageToDisk = AppSettings.CoverImageSettings.SaveToDisk;
                _stampSize = AppSettings.CoverImageSettings.StampResolution;
                Localize();
            }
            
        }

        public void Save()
        {
            AppSettings.CoverImageSettings.CreateStamps = _createFolderImage;
            AppSettings.CoverImageSettings.CreateStamps = _createStampsFiles;
            AppSettings.CoverImageSettings.InsertFrontImage = _insertFrontImage;
            AppSettings.CoverImageSettings.InsertStampImage =_insertStampImage;
            AppSettings.CoverImageSettings.RemoveImages = _removeImages;
            AppSettings.CoverImageSettings.RenameImages = _renameImageFiles;
            AppSettings.CoverImageSettings.SaveToDisk = _saveImageToDisk;
            AppSettings.CoverImageSettings.StampResolution = _stampSize;
        }
        #endregion  // Constructor


        #region public
       
        #endregion  // public

        #region private helper
        
        #endregion // private helper

        #region Events
             
        #endregion // Events

        #region Localization

        private String _gpEmbeddedImages = "Embedded Cover Images";
        private String _gpFileRename = "File Rename Options";
        private String _gpMiniaturSleeves = "Miniatur sleeves";
        private String _gpSonos = "Sonos";

        private String _rbInsertFrontImage = "Insert front image";
        private String _rbInsertStampImage = "Insert stamp image";
        private String _rbNoActionAPIC = "No Action";
        private String _rbRemoveImages = "Remove all images";
        private String _rbSaveToDisk = "Save image to disk";

        private String _cbCreateStamps = "Create miniatur sleeve from front sleeve";
        private String _cbRenameImages = "Rename image files";
        private String _cbSonos = "Copy Frontcover as folder.jpg";

        private String _title = "CD Cover Sleeves";
        private String _titleTip = "This dialog helps you manage the images related to your music collection";


        public String GroupEmbeddedImages
        {
            get { return _gpEmbeddedImages; }
            set
            {
                if (value == _gpEmbeddedImages)
                    return;

                _gpEmbeddedImages = value;

                RaisePropertyChanged("GroupEmbeddedImages");
            }
        }
        public String GroupFileRename
        {
            get { return _gpFileRename; }
            set
            {
                if (value == _gpFileRename)
                    return;

                _gpFileRename = value;

                RaisePropertyChanged("GroupFileRename");
            }
        }
        public String GroupMiniaturSleeves
        {
            get { return _gpMiniaturSleeves; }
            set
            {
                if (value == _gpMiniaturSleeves)
                    return;

                _gpMiniaturSleeves = value;

                RaisePropertyChanged("GroupMiniaturSleeves");
            }
        }
        public String GroupSonos
        {
            get { return _gpSonos; }
            set
            {
                if (value == _gpSonos)
                    return;

                _gpSonos = value;

                RaisePropertyChanged("GroupSonos");
            }
        }

        public String RbInsertStampImage
        {
            get { return _rbInsertStampImage; }
            set
            {
                if (value == _rbInsertStampImage)
                    return;

                _rbInsertStampImage = value;

                RaisePropertyChanged("RbInsertStampImage");
            }
        }
        public String RbInsertFrontImage
        {
            get { return _rbInsertFrontImage; }
            set
            {
                if (value == _rbInsertFrontImage)
                    return;

                _rbInsertFrontImage = value;

                RaisePropertyChanged("RbInsertFrontImage");
            }
        }       
        public String RbNoActionAPIC
        {
            get { return _rbNoActionAPIC; }
            set
            {
                if (value == _rbNoActionAPIC)
                    return;

                _rbNoActionAPIC = value;

                RaisePropertyChanged("RbNoActionAPIC");
            }
        }
        public String RbSaveToDisk
        {
            get { return _rbSaveToDisk; }
            set
            {
                if (value == _rbSaveToDisk)
                    return;

                _rbSaveToDisk = value;

                RaisePropertyChanged("RbSaveToDisk");
            }
        }
        public String RbRemoveImages
        {
            get { return _rbRemoveImages; }
            set
            {
                if (value == _rbRemoveImages)
                    return;

                _rbRemoveImages = value;

                RaisePropertyChanged("RbRemoveImages");
            }
        }

        public String CbCreateStamps
        {
            get { return _cbCreateStamps; }
            set
            {
                if (value == _cbCreateStamps)
                    return;

                _cbCreateStamps = value;

                RaisePropertyChanged("CbCreateStamps");
            }
        }
        public String CbRenameImages
        {
            get { return _cbRenameImages; }
            set
            {
                if (value == _cbRenameImages)
                    return;

                _cbRenameImages = value;

                RaisePropertyChanged("CbRenameImages");
            }
        }
        public String CbSonos
        {
            get { return _cbSonos; }
            set
            {
                if (value == _cbSonos)
                    return;

                _cbSonos = value;

                RaisePropertyChanged("CbSonos");
            }
        }

        public String Title
        {
            get { return _title; }
            set
            {
                if (value == _title)
                    return;

                _title = value;

                RaisePropertyChanged("Title");
            }
        }

        public String TitleTip
        {
            get { return _titleTip; }
            set
            {
                if (value == _titleTip)
                    return;

                _titleTip = value;

                RaisePropertyChanged("TitleTip");
            }
        }
        

        public void Localize()
        {            
            Title = AmmLocalization.GetLocalizedString("frmCoverImage_Title");
            TitleTip = AmmLocalization.GetLocalizedString("frmCoverImage_TitleTip");

            GroupEmbeddedImages = AmmLocalization.GetLocalizedString("frmCoverImage_gpEmbeddedImages");
            GroupFileRename = AmmLocalization.GetLocalizedString("frmCoverImage_gpFileRename");
            GroupMiniaturSleeves = AmmLocalization.GetLocalizedString("frmCoverImage_gpMiniaturSleeves");
            GroupSonos = AmmLocalization.GetLocalizedString("cmd_PlayLast_ToolTip");

            RbInsertStampImage = AmmLocalization.GetLocalizedString("frmCoverImage_rbInsertStampImage");
            RbInsertFrontImage = AmmLocalization.GetLocalizedString("frmCoverImage_rbInsertFrontImage");
            RbNoActionAPIC = AmmLocalization.GetLocalizedString("frmCoverImage_rbNoActionAPIC");
            RbSaveToDisk = AmmLocalization.GetLocalizedString("frmCoverImage_rbSaveToDisk");
            RbRemoveImages = AmmLocalization.GetLocalizedString("frmCoverImage_rbRemoveImages");

            CbCreateStamps = AmmLocalization.GetLocalizedString("frmCoverImage_cbCreateStamps");
            CbRenameImages = AmmLocalization.GetLocalizedString("frmCoverImage_cbRenameImages");
            CbSonos = AmmLocalization.GetLocalizedString("frmCoverImage_cbSonos");
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

        ~CoverImageToolsViewModel()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }

        private void ReleaseManagedResources()
        {
            if (_multiFolderSelectVM != null)
            {
                _multiFolderSelectVM.Dispose();
            }
        }

        private void ReleaseUnmangedResources()
        {

        }
        #endregion
    }
}

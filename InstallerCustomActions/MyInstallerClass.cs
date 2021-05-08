using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace InstallerCustomActions
{
    // Things to do in your setup & deployment project
    // 1. Open the "Custom Action" editor
    // 2. Select "Commit" and then "Add Custom Action" 
    // 3. Double Click "Application Folder", select primary output of this InstallHelper
    // 4. At "custom actions" check in the property InstallerClass is TRUE
    // 5. Do the same for the "Install" custom action
    // 6. Add the CustomActionData to the primary output of InstallHelper:   /TARGETDIR="[TARGETDIR]\"

    // Debug:
    // msiexec.exe /i "SetupAllMyMusic.msi" /log installerLogfile.txt

    //Putting a trailing backslash on the Uninstall CustomActionData causes error 1001:
    //CustomActionData = /SOURCEDIR="[SOURCEDIR]\"
    //Error 1001. Error 1001. Exception occurred while initializing the installation:
    //System.IO.FileNotFoundException: Could not load file or assembly 'file:///C:\Windows\SysWOW64\Files' or one of its dependencies. The system cannot find the file specified..


    // Set Platform Taget to x86 for all projects in this solution

    [RunInstaller(true)]
    public class MyInstallerClass : Installer
    {

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            //MessageBox.Show(Context.Parameters["TARGETDIR"].ToString() + "AllMyMusic.exe");
        }


        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);


            // Register for writing to the Windows Application Eventlog File
            try
            {
                String sSource = "AllMyMusic_v3";
                String sLog = "Application";
                String sEvent = "Register for writing to the application event logfile";
                if (!EventLog.SourceExists(sSource))
                {
                    EventLog.CreateEventSource(sSource, sLog);
                }
                EventLog.WriteEntry(sSource, sEvent);
            }
            catch (Exception Err)
            {
                MessageBox.Show("Commit Error: " + Err.Message);
            }


            try
            {
                String _applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\winisoft\AllMyMusic"; // C:\ProgramData\winisoft\AllMyMusic
                if (Directory.Exists(_applicationDataPath) == false)
                {
                    CommonApplicationData commonAppDFata = new CommonApplicationData("winisoft", "AllMyMusic", true);
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show("Commit Error: " + Err.Message);
            }


            // let's launch the application
            //MessageBox.Show(Context.Parameters["TARGETDIR"].ToString() + "AllMyMusic.exe");
            //System.Diagnostics.Process.Start(Context.Parameters["TARGETDIR"].ToString() + "AllMyMusic.exe");

        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

using System.Security.Permissions;
using System.Security.Principal;
using Microsoft.Win32;

namespace AllMyMusic_v3
{
    public static class InstallerHelper
    {
        public static InstallationContexts GetInstalledContext(string p_appDisplayName)
        {
            //The S-1-5-18  = Local System user (Everyone) | HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products\

            //The S-XXXXXX  = Current user (Just me)       | HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-?!?!?!?!\Products\

            string key;
            string keyFormat = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\{0}\Products\";


            // Check if installed for -> Everyone
            key = string.Format(keyFormat, "S-1-5-18");
            bool res = GetInstalledContext_IsRegKeyExists(key, p_appDisplayName, StringComparison.OrdinalIgnoreCase);
            if (res == true)
            {
                return InstallationContexts.Everyone;
            }
 
            // Check if installed for -> Just me
            key = string.Format(keyFormat, WindowsIdentity.GetCurrent().User.Value);
            res = GetInstalledContext_IsRegKeyExists(key, p_appDisplayName, StringComparison.OrdinalIgnoreCase);
            if (res == true)
            {
                return InstallationContexts.JustMe;
            }
 
            return InstallationContexts.NotInstalled;
        }

        public static bool GetInstalledContext_IsRegKeyExists(string p_regKey, string p_appDisplayName, StringComparison p_scompare)
        {
            using (RegistryKey regkey = RegistryHelpers.GetRegistryKey(p_regKey))
            {
                if (regkey != null)
                {
                    RegistryKey rk;
                    string[] arrProducs = regkey.GetSubKeyNames();

                    if (arrProducs != null)
                    {
                        for (int i = 0; i < arrProducs.Length; i++)
                        {
                            using (rk = regkey.OpenSubKey(arrProducs[i] + @"\InstallProperties"))
                            {
                                if (rk != null)
                                {
                                    object name = rk.GetValue("DisplayName");
                                    if (name != null)
                                    {
                                        String displayName = name.ToString();
                                        if (p_appDisplayName.Equals(displayName, p_scompare) == true)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
 
            return false;
        }

    }
}

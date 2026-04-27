using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SmartPasswordManagerCsharpDesktop.Helpers
{
    public static class ShellShortcut
    {
        public static bool CreateDesktopShortcut()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string shortcutPath = Path.Combine(desktopPath, "Smart Password Manager.lnk");
                string targetPath = Application.ExecutablePath;

                if (File.Exists(shortcutPath))
                {
                    DialogResult result = MessageBox.Show(
                        "Shortcut already exists. Replace it?",
                        "Shortcut Exists",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                        return false;
                }

                dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell"));
                dynamic shortcut = shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = targetPath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
                shortcut.Description = "Smart Password Manager. Desktop manager for deterministic smart passwords. Generate, manage, and retrieve passwords without storing them. Your secret phrase never leaves your device.";
                shortcut.IconLocation = targetPath + ", 0";
                shortcut.Save();

                Marshal.ReleaseComObject(shortcut);
                Marshal.ReleaseComObject(shell);

                MessageBox.Show(
                    "Desktop shortcut created successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
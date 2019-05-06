using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SymlinkMaker
{
    class Regedit
    {
        private static bool isElevated()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static void Reg()
        {
            if (!isElevated())
            {
                MessageBox.Show("Please run as Administrator to modify registry!", "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            try
            {
                string exec = Path.GetFullPath(Assembly.GetEntryAssembly().Location);
                var mainkey = Registry.ClassesRoot.CreateSubKey(@"Directory\Background\shell\SymlinkMaker");
                mainkey.SetValue("", "Paste as Symbolic Link");
                mainkey.SetValue("Icon", $"{exec},0");
                var cmd = mainkey.CreateSubKey("command");
                cmd.SetValue("", $"\"{exec}\" \"%V\"");

                MessageBox.Show("Right click on any directory, the symlink option should appear.", "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registry Error: {ex.Message}", "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void UnReg()
        {
            if (!isElevated())
            {
                MessageBox.Show("Please run as Administrator to modify registry!", "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Registry.ClassesRoot.DeleteSubKeyTree(@"Directory\Background\shell\SymlinkMaker");
                MessageBox.Show("The symlink option should be deleted.", "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registry Error: {ex.Message}", "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

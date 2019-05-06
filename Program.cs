using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SymlinkMaker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var directory = Directory.GetCurrentDirectory();
            if (args.Count() > 0)
            {
                foreach (var arg in args)
                {
                    if (arg.ToLower().StartsWith("/reg"))
                    {
                        Regedit.Reg();
                        return;
                    }
                    else if (arg.ToLower().StartsWith("/unreg"))
                    {
                        Regedit.UnReg();
                        return;
                    }
                    else if (arg.isDirectory())
                    {
                        directory = arg;
                        break;
                    }
                }
            }
            else
            {
                string exec = Path.GetFileName(Assembly.GetEntryAssembly().Location);
                MessageBox.Show($"Please set the output directory!\nCommand: {exec} OutputDirectory",
                    "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var clipboard = Clipboard.GetFileDropList();
            var sources = new List<string>();
            foreach (var file in clipboard)
            {
                if (!file.isDirectory())
                {
                    if (!File.Exists(file))
                        continue;
                }

                sources.Add(file);
            }

            if (sources.Count == 0)
            {
                MessageBox.Show("You have to copy a file or folder first!", "Symlink Maker",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sources.Count > 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMultiple(sources, directory));
            }
            else
            {
                var filename = Path.GetFileName(sources[0]);
                var foldername = Path.GetFileName(directory);
                var dialog = MessageBox.Show($"This action will create symlink for '{filename}' into folder '{foldername}'.\n\nAre you sure?",
                                "Symlink Maker", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    try
                    {
                        var output = Path.Combine(directory, filename);
                        if (!MakeLink.Create(sources[0], output))
                        {
                            MessageBox.Show("Something went wrong, can't create symlink!", "Symlink Maker",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

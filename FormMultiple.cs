using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SymlinkMaker
{
    public partial class FormMultiple : Form
    {
        private List<string> sources;
        private string directory;
        private bool noError = true;

        public FormMultiple(List<string> sources, string directory)
        {
            InitializeComponent();
            this.sources = sources;
            this.directory = directory;

            textBox1.Text = directory;
            foreach (var file in sources)
            {
                var type = file.isDirectory() ? "Directory" : "File";
                var name = file.isDirectory() ? Path.GetDirectoryName(file) : Path.GetFileName(file);

                var item = new ListViewItem(type);
                item.SubItems.Add(name);
                listView1.Items.Add(item);
            }
        }

        private void BatchCreate()
        {
            foreach (var source in sources)
            {
                try
                {
                    var filename = Path.GetFileName(source);
                    var output = Path.Combine(directory, filename);
                    if (!MakeLink.Create(source, output))
                    {
                        noError = false;
                        MessageBox.Show($"Something went wrong, can't create symlink for {filename}!",
                            "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    noError = false;
                    MessageBox.Show(ex.Message, "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            await Task.Run(() => { BatchCreate(); });
            if (noError)
            {
                MessageBox.Show("Task finished!", "Symlink Maker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }

            button1.Enabled = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

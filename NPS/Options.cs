using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace NPS
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        void LoadSettings()
        {
            textDownload.Text = Settings.Instance.downloadDir;
            textPKGPath.Text = Settings.Instance.pkgPath;
            textParams.Text = Settings.Instance.pkgParams;
            textBox1.Text = Settings.Instance.GamesUri;
            textBox2.Text = Settings.Instance.DLCUri;
            checkBox1.Checked = Settings.Instance.deleteAfterUnpack;
            chkUnpackToOwnDirectory.Checked = Settings.Instance.unpackToOwnDirectory;
            numericUpDown1.Value = Settings.Instance.simultaneousDl;
            textBox3.Text = Settings.Instance.PSMUri;
            textBox4.Text = Settings.Instance.PSXUri;
            textBox5.Text = Settings.Instance.PSPUri;
            textBox6.Text = Settings.Instance.PSPDLCUri;
            tb_ps3uri.Text = Settings.Instance.PS3Uri;
            tb_ps3dlcuri.Text = Settings.Instance.PS3DLCUri;
            txtUnpackToOwnDirectory.Text = Settings.Instance.unpackToOwnDirectoryParams;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textDownload.Text = fbd.SelectedPath;
                    Settings.Instance.downloadDir = textDownload.Text;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var fbd = new OpenFileDialog())
            {
                if (Type.GetType("Mono.Runtime") != null)
                {
                    fbd.Filter = "|*";
                }
                else
                {
                    fbd.Filter = "|*.exe";
                }

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    textPKGPath.Text = fbd.FileName;
                    Settings.Instance.pkgPath = textPKGPath.Text;
                }
            }
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateSettings(true);
        }

        void UpdateSettings(bool withStoring)
        {
            Settings.Instance.pkgParams = textParams.Text;
            Settings.Instance.GamesUri = textBox1.Text;
            Settings.Instance.DLCUri = textBox2.Text;
            Settings.Instance.PSMUri = textBox3.Text;
            Settings.Instance.PSXUri = textBox4.Text;
            Settings.Instance.PSPUri = textBox5.Text;
            Settings.Instance.PSPDLCUri = textBox6.Text;
            Settings.Instance.PS3Uri = tb_ps3uri.Text;
            Settings.Instance.PS3DLCUri = tb_ps3dlcuri.Text;
            Settings.Instance.unpackToOwnDirectoryParams = txtUnpackToOwnDirectory.Text;
            if (withStoring)
                Settings.Instance.Store();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(textBox1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(textBox2);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Instance.deleteAfterUnpack = checkBox1.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Settings.Instance.simultaneousDl = (int)numericUpDown1.Value;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(textBox3);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(@"Here you can give parameters to pass to your pkg dec tool. Available variables are: 
- {zRifKey}
- {pkgFile}
- {gameTitle}
- {region}
- {titleID}");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(textBox4);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(textBox5);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(textBox6);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_ps3uri);
        }

        void ShowOpenFileWindow(TextBox tb)
        {

            using (var fbd = new OpenFileDialog())
            {
                fbd.Filter = "|*.tsv";

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    tb.Text = fbd.FileName;
                }
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            ShowOpenFileWindow(tb_ps3dlcuri);
        }

        private void button_export(object sender, EventArgs e)
        {
            try
            {
                using (var fsd = new SaveFileDialog())
                {
                    fsd.Filter = "|*.npsSettings";
                    DialogResult result = fsd.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        UpdateSettings(false);
                        FileStream stream = File.Create(fsd.FileName);
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(stream, Settings.Instance);
                        stream.Close();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        private void button_import(object sender, EventArgs e)
        {
            using (var fbd = new OpenFileDialog())
            {
                fbd.Filter = "|*.npsSettings";

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (System.IO.File.Exists(fbd.FileName))
                    {
                        var stream = File.OpenRead(fbd.FileName);
                        var formatter = new BinaryFormatter();
                        Settings.Instance = (Settings)formatter.Deserialize(stream);
                        stream.Close();
                        LoadSettings();
                    }
                }
            }
        }

        private void chkUnpackToOwnDirectory_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Instance.unpackToOwnDirectory = chkUnpackToOwnDirectory.Checked;
            txtUnpackToOwnDirectory.Enabled = chkUnpackToOwnDirectory.Checked;
        }
    }
}

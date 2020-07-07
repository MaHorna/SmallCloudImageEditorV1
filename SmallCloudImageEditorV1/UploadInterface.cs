using System;
using System.Windows.Forms;

namespace SmallCloudImageEditorV1
{
    public partial class UploadInterface : Form
    {
        public UploadInterface()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.TextLength < 3 || this.textBox2.TextLength < 3)
            {
                if (MessageBox.Show("Author's name or image name too short.(minimus is 3)", "Input too short", MessageBoxButtons.OK) != DialogResult.OK)
                    return;
                this.Close();
            }
            else if (this.textBox1.TextLength > 30 || this.textBox2.TextLength > 30)
            {
                if (MessageBox.Show("Author's name or image name too long.(maximum is 30)", "Input too long", MessageBoxButtons.OK) != DialogResult.OK)
                    return;
                this.Close();
            }
            else
            {
                MySQLHandle.FTPUploadImage(Form1.imageHistory[Form1.CurrentHistoryIndex]);
                MySQLHandle.SQLUploadInfo(Form1.lastBlurSize, this.textBox1.Text, this.textBox2.Text);
                if (MessageBox.Show("Successfully uploaded your image.", "Success", MessageBoxButtons.OK) != DialogResult.OK)
                    return;
                this.Close();
            }
        }
    }
}

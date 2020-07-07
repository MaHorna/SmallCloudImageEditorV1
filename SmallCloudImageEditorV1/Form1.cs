using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace SmallCloudImageEditorV1
{
    public partial class Form1 : Form
    {
        public static Bitmap[] imageHistory = new Bitmap[10]
        {
            new Bitmap(10, 10),
            new Bitmap(10, 10),
            new Bitmap(10, 10),
            new Bitmap(10, 10),
            new Bitmap(10, 10),
            new Bitmap(10, 10),
            new Bitmap(10, 10),
            new Bitmap(10, 10),
            new Bitmap(10, 10),
            new Bitmap(10, 10)
        };
        private bool ineditor = true;
        public int[] otherColorsCount = new int[9];
        public Color[] otherColors = new Color[9]
        {
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10)
        };
        public static int CurrentHistoryIndex;
        public int EditCount;
        public int sameColorCount;
        public Color tmp;
        public static Bitmap img;
        public Bitmap bluredSmallImg;
        public static int lastBlurSize;
        private int othercolorlastindex;
        private bool attheendofDB;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            PictureBox1.Location = new Point(170, 110);
            PictureBox1.Size = new Size(this.Width-170, this.Height-110);
            FlowLayoutPanel flowLayoutPanel = this.flowLayoutPanel1;
            flowLayoutPanel.Location = new Point(15, 25);
            flowLayoutPanel1.Size = new Size(this.Width - 25, this.Height - 53);
            flowLayoutPanel1.Visible = false;
            this.button2.Enabled = false;
            this.button4.Enabled = false;
            this.button5.Enabled = false;
            this.button6.Enabled = false;
            this.button10.Visible = false;
            string path = "C:\\SmallCloudImageEditor";
            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Image";
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Form1.img = new Bitmap(openFileDialog.FileName);
                this.PictureBox1.Image = (Image)Form1.img;
                this.button2.Enabled = true;
                this.button5.Enabled = true;
                this.button4.Enabled = true;
                Form1.imageHistory[0] = Form1.img;
                Form1.CurrentHistoryIndex = 0;
                this.EditCount = 0;
            }
            openFileDialog.Dispose();
        }

        private void button2_Click(object sender, EventArgs e) //scale
        {
            int num1 = this.trackBar1.Value * 2 + 3;
            int num2 = Form1.imageHistory[Form1.CurrentHistoryIndex].Width / num1 * num1;
            int num3 = Form1.imageHistory[Form1.CurrentHistoryIndex].Height / num1 * num1;
            if (num3 <= num1 || num2 <= num1)
                return;
            Bitmap bitmap = new Bitmap(num2 / num1, num3 / num1);
            int num4 = num1 / 2 / 3;
            int num5 = num1 / 2 % 3;
            Color.FromArgb(0, 0, 0);
            for (int x = 0; x < num2 / num1; ++x)
            {
                for (int y = 0; y < num3 / num1; ++y)
                {
                    double num6 = 0.0;
                    double num7 = 0.0;
                    double num8 = 0.0;
                    int num9 = 0;
                    Color color;
                    if (num1 <= 6)
                    {
                        color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num5, y * num1 + num5);
                        num8 += (double)color.R;
                        num7 += (double)color.G;
                        num6 += (double)color.B;
                        ++num9;
                    }
                    else
                    {
                        for (int index1 = 0; index1 < num4; ++index1)
                        {
                            color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num1 / 2, y * num1 + num1 / 2);
                            num8 += (double)color.R;
                            num7 += (double)color.G;
                            num6 += (double)color.B;
                            ++num9;
                            for (int index2 = 0; index2 < num4 + 1; ++index2)
                            {
                                color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num5 + index2 * 3, y * num1 + num5);
                                num8 += (double)color.R;
                                num7 += (double)color.G;
                                num6 += (double)color.B;
                                ++num9;
                            }
                            for (int index2 = 0; index2 < num4 + 1; ++index2)
                            {
                                color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num5 + index2 * 3, num5 + (index1 + 1) * 5 + index1 + y * num1);
                                num8 += (double)color.R;
                                num7 += (double)color.G;
                                num6 += (double)color.B;
                                ++num9;
                            }
                            for (int index2 = 0; index2 < num4; ++index2)
                            {
                                color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num5, num5 + y * num1 + 6 + index2 * 6);
                                double num10 = num8 + (double)color.R;
                                double num11 = num7 + (double)color.G;
                                double num12 = num6 + (double)color.B;
                                int num13 = num9 + 1;
                                color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num5 + (index1 + 1) * 5 + index1, num5 + y * num1 + 6 + index2 * 6);
                                num8 = num10 + (double)color.R;
                                num7 = num11 + (double)color.G;
                                num6 = num12 + (double)color.B;
                                num9 = num13 + 1;
                            }
                        }
                    }
                    color = Color.FromArgb((int)num8 / num9, (int)num7 / num9, (int)num6 / num9);
                    bitmap.SetPixel(x, y, color);
                }
            }
            CurrentHistoryIndex++;
            EditCount++;
            Form1.imageHistory[Form1.CurrentHistoryIndex] = bitmap;
            this.PictureBox1.Image = (Image)bitmap;
            if (Form1.CurrentHistoryIndex != 10)
                return;
            Form1.CurrentHistoryIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e) //undo
        {
            if (this.EditCount == 0)
                return;
            if (Form1.CurrentHistoryIndex == 0)
                Form1.CurrentHistoryIndex = 9;
            else
                --Form1.CurrentHistoryIndex;
            this.PictureBox1.Image = (Image)Form1.imageHistory[Form1.CurrentHistoryIndex];
            --this.EditCount;
        }

        private void button4_Click(object sender, EventArgs e) //blur
        {
            int num1 = this.trackBar1.Value * 2 + 3;
            int width = Form1.imageHistory[Form1.CurrentHistoryIndex].Width / num1 * num1;
            int height = Form1.imageHistory[Form1.CurrentHistoryIndex].Height / num1 * num1;
            if (height <= num1 || width <= num1)
                return;
            Bitmap bitmap = new Bitmap(width, height);
            this.bluredSmallImg = new Bitmap(width / num1, height / num1);
            int num2 = num1 / 2 / 3;
            int num3 = num1 / 2 % 3;
            Color.FromArgb(0, 0, 0);
            for (int x = 0; x < width / num1; ++x)
            {
                for (int y = 0; y < height / num1; ++y)
                {
                    double num4 = 0.0;
                    double num5 = 0.0;
                    double num6 = 0.0;
                    int num7 = 0;
                    Color color;
                    if (num1 <= 6)
                    {
                        color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num3, y * num1 + num3);
                        num6 += (double)color.R;
                        num5 += (double)color.G;
                        num4 += (double)color.B;
                        ++num7;
                    }
                    else
                    {
                        for (int index1 = 0; index1 < num2; ++index1)
                        {
                            color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num1 / 2, y * num1 + num1 / 2);
                            num6 += (double)color.R;
                            num5 += (double)color.G;
                            num4 += (double)color.B;
                            ++num7;
                            for (int index2 = 0; index2 < num2 + 1; ++index2)
                            {
                                color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num3 + index2 * 3, y * num1 + num3);
                                num6 += (double)color.R;
                                num5 += (double)color.G;
                                num4 += (double)color.B;
                                ++num7;
                            }
                            for (int index2 = 0; index2 < num2 + 1; ++index2)
                            {
                                color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num3 + index2 * 3, num3 + (index1 + 1) * 5 + index1 + y * num1);
                                num6 += (double)color.R;
                                num5 += (double)color.G;
                                num4 += (double)color.B;
                                ++num7;
                            }
                            for (int index2 = 0; index2 < num2; ++index2)
                            {
                                color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num3, num3 + y * num1 + 6 + index2 * 6);
                                double num8 = num6 + (double)color.R;
                                double num9 = num5 + (double)color.G;
                                double num10 = num4 + (double)color.B;
                                int num11 = num7 + 1;
                                color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x * num1 + num3 + (index1 + 1) * 5 + index1, num3 + y * num1 + 6 + index2 * 6);
                                num6 = num8 + (double)color.R;
                                num5 = num9 + (double)color.G;
                                num4 = num10 + (double)color.B;
                                num7 = num11 + 1;
                            }
                        }
                    }
                    color = Color.FromArgb((int)num6 / num7, (int)num5 / num7, (int)num4 / num7);
                    for (int index1 = 0; index1 < num1; ++index1)
                    {
                        for (int index2 = 0; index2 < num1; ++index2)
                        {
                            bitmap.SetPixel(x * num1 + index1, y * num1 + index2, color);
                            this.bluredSmallImg.SetPixel(x, y, color);
                            this.button6.Enabled = true;
                        }
                    }
                }
            }
            ++Form1.CurrentHistoryIndex;
            ++this.EditCount;
            if (Form1.CurrentHistoryIndex == 10)
                Form1.CurrentHistoryIndex = 0;
            Form1.imageHistory[Form1.CurrentHistoryIndex] = bitmap;
            this.PictureBox1.Image = (Image)bitmap;
            Form1.lastBlurSize = num1;
        }

        private void button5_Click(object sender, EventArgs e) //posterize
        {
            int num1 = 256 / (this.trackBar1.Value + 1);
            Bitmap bitmap = new Bitmap(Form1.imageHistory[Form1.CurrentHistoryIndex].Width, Form1.imageHistory[Form1.CurrentHistoryIndex].Height);
            Color.FromArgb(0, 0, 0);
            Color color;
            for (int x = 0; x < Form1.imageHistory[Form1.CurrentHistoryIndex].Width; ++x)
            {
                for (int y = 0; y < Form1.imageHistory[Form1.CurrentHistoryIndex].Height; ++y)
                {
                    color = Form1.imageHistory[Form1.CurrentHistoryIndex].GetPixel(x, y);
                    double num2 = Math.Round((double)color.R / Convert.ToDouble(num1)) * (double)num1;
                    double num3 = Math.Round((double)color.G / Convert.ToDouble(num1)) * (double)num1;
                    double num4 = Math.Round((double)color.B / Convert.ToDouble(num1)) * (double)num1;
                    if (num2 == 256.0)
                        num2 = (double)byte.MaxValue;
                    if (num3 == 256.0)
                        num3 = (double)byte.MaxValue;
                    if (num4 == 256.0)
                        num4 = (double)byte.MaxValue;
                    color = Color.FromArgb((int)num2, (int)num3, (int)num4);
                    bitmap.SetPixel(x, y, color);
                }
            }
            ++Form1.CurrentHistoryIndex;
            ++this.EditCount;
            if (Form1.CurrentHistoryIndex == 10)
                Form1.CurrentHistoryIndex = 0;
            Form1.imageHistory[Form1.CurrentHistoryIndex] = bitmap;
            this.PictureBox1.Image = (Image)bitmap;
            if (!this.button6.Enabled)
                return;
            int num5 = 256 / (this.trackBar1.Value + 1);
            color = Color.FromArgb(0, 0, 0);
            for (int x = 0; x < this.bluredSmallImg.Width; ++x)
            {
                for (int y = 0; y < this.bluredSmallImg.Height; ++y)
                {
                    color = this.bluredSmallImg.GetPixel(x, y);
                    double num2 = Math.Round((double)color.R / Convert.ToDouble(num5)) * (double)num5;
                    double num3 = Math.Round((double)color.G / Convert.ToDouble(num5)) * (double)num5;
                    double num4 = Math.Round((double)color.B / Convert.ToDouble(num5)) * (double)num5;
                    if (num2 == 256.0)
                        num2 = (double)byte.MaxValue;
                    if (num3 == 256.0)
                        num3 = (double)byte.MaxValue;
                    if (num4 == 256.0)
                        num4 = (double)byte.MaxValue;
                    color = Color.FromArgb((int)num2, (int)num3, (int)num4);
                    this.bluredSmallImg.SetPixel(x, y, color);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Color.FromArgb(0, 0, 0);
            for (int x = 1; x < this.bluredSmallImg.Width - 1; ++x)
            {
                for (int y = 1; y < this.bluredSmallImg.Height - 1; ++y)
                {
                    this.tmp = this.bluredSmallImg.GetPixel(x, y);
                    if (this.tmp == this.bluredSmallImg.GetPixel(x - 1, y - 1))
                        ++this.sameColorCount;
                    else
                        this.AddColorToArray(this.bluredSmallImg.GetPixel(x - 1, y - 1));
                    if (this.tmp == this.bluredSmallImg.GetPixel(x - 1, y + 1))
                        ++this.sameColorCount;
                    else
                        this.AddColorToArray(this.bluredSmallImg.GetPixel(x - 1, y + 1));
                    if (this.tmp == this.bluredSmallImg.GetPixel(x - 1, y))
                        ++this.sameColorCount;
                    else
                        this.AddColorToArray(this.bluredSmallImg.GetPixel(x - 1, y));
                    if (this.tmp == this.bluredSmallImg.GetPixel(x + 1, y - 1))
                        ++this.sameColorCount;
                    else
                        this.AddColorToArray(this.bluredSmallImg.GetPixel(x + 1, y - 1));
                    if (this.tmp == this.bluredSmallImg.GetPixel(x + 1, y + 1))
                        ++this.sameColorCount;
                    else
                        this.AddColorToArray(this.bluredSmallImg.GetPixel(x + 1, y + 1));
                    if (this.tmp == this.bluredSmallImg.GetPixel(x + 1, y))
                        ++this.sameColorCount;
                    else
                        this.AddColorToArray(this.bluredSmallImg.GetPixel(x + 1, y));
                    if (this.tmp == this.bluredSmallImg.GetPixel(x, y - 1))
                        ++this.sameColorCount;
                    else
                        this.AddColorToArray(this.bluredSmallImg.GetPixel(x, y - 1));
                    if (this.tmp == this.bluredSmallImg.GetPixel(x, y + 1))
                        ++this.sameColorCount;
                    else
                        this.AddColorToArray(this.bluredSmallImg.GetPixel(x, y + 1));
                    this.othercolorlastindex = 0;
                    if (this.sameColorCount <= 1)
                    {
                        int num = -1;
                        int index1 = 0;
                        for (int index2 = 0; index2 < this.otherColors.Length; ++index2)
                        {
                            if (this.otherColorsCount[index2] > num)
                            {
                                num = this.otherColorsCount[0];
                                index1 = index2;
                            }
                        }
                        this.bluredSmallImg.SetPixel(x, y, this.otherColors[index1]);
                    }
                    this.sameColorCount = 0;
                    this.otherColors = new Color[9]
                    {
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10),
            Color.FromArgb(10, 10, 10)
                    };
                    this.otherColorsCount = new int[9];
                }
            }
            Bitmap bitmap = new Bitmap(this.bluredSmallImg.Width * Form1.lastBlurSize, this.bluredSmallImg.Height * Form1.lastBlurSize);
            for (int x = 0; x < this.bluredSmallImg.Width; ++x)
            {
                for (int y = 0; y < this.bluredSmallImg.Height; ++y)
                {
                    for (int index1 = 0; index1 < Form1.lastBlurSize; ++index1)
                    {
                        for (int index2 = 0; index2 < Form1.lastBlurSize; ++index2)
                            bitmap.SetPixel(x * Form1.lastBlurSize + index1, y * Form1.lastBlurSize + index2, this.bluredSmallImg.GetPixel(x, y));
                    }
                }
            }
            ++Form1.CurrentHistoryIndex;
            ++this.EditCount;
            if (Form1.CurrentHistoryIndex == 10)
                Form1.CurrentHistoryIndex = 0;
            Form1.imageHistory[Form1.CurrentHistoryIndex] = bitmap;
            this.PictureBox1.Image = (Image)bitmap;
        }
        private void AddColorToArray(Color color)
        {
            bool flag = false;
            for (int index = 0; index < this.otherColors.Length; ++index)
            {
                if (this.otherColors[index] == color)
                {
                    ++this.otherColorsCount[index];
                    flag = true;
                }
            }
            if (flag)
                return;
            this.otherColors[this.othercolorlastindex] = color;
            this.otherColorsCount[this.othercolorlastindex] = 1;
            ++this.othercolorlastindex;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            Form1.imageHistory[Form1.CurrentHistoryIndex].Save(saveFileDialog.FileName, ImageFormat.Bmp);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new UploadInterface().Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Size = new Size(this.Width - 25, this.Height - 53);
            flowLayoutPanel1.Location = new Point(25,53);
            if (!this.ineditor)
            {
                this.button1.Visible = true;
                this.button6.Visible = true;
                this.button7.Visible = true;
                this.button8.Visible = true;
                this.button2.Visible = true;
                this.button3.Visible = true;
                this.button4.Visible = true;
                this.button5.Visible = true;
                this.button10.Visible = false;
                this.label1.Visible = true;
                this.label2.Visible = true;
                this.label3.Visible = true;
                this.label4.Visible = true;
                this.label5.Visible = true;
                this.trackBar1.Visible = true;
                this.PictureBox1.Visible = true;
                this.flowLayoutPanel1.Visible = false;
                this.button9.Text = "Show other's images";
                this.button9.Location = new Point(12, 99);
                this.ineditor = true;
            }
            else
            {
                this.button1.Visible = false;
                this.button6.Visible = false;
                this.button7.Visible = false;
                this.button8.Visible = false;
                this.button2.Visible = false;
                this.button3.Visible = false;
                this.button4.Visible = false;
                this.button5.Visible = false;
                this.button10.Visible = true;
                this.label1.Visible = false;
                this.label2.Visible = false;
                this.label3.Visible = false;
                this.label4.Visible = false;
                this.label5.Visible = false;
                this.trackBar1.Visible = false;
                this.PictureBox1.Visible = false;
                this.button9.Text = "Back to editor";
                this.button9.Location = new Point(12, 12);
                this.flowLayoutPanel1.Visible = true;
                this.flowLayoutPanel1.Location = new Point(13, 41);
                this.flowLayoutPanel1.AutoScroll = true;
                this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
                this.flowLayoutPanel1.WrapContents = true;
                this.ineditor = false;
                MySQLHandle.firsttimeGetShowedID = true;
                if (this.attheendofDB)
                {
                    int num = (int)MessageBox.Show("There are no more images to be seen", "End of database", MessageBoxButtons.OK);
                }
                else
                {
                    for (int index = 0; index < 10; ++index)
                    {
                        Button button = new Button();
                        button.Name = MySQLHandle.getshowedID().ToString();
                        Image lastUnseenImage = MySQLHandle.getLastUnseenImage();
                        button.Image = lastUnseenImage;
                        button.Text = MySQLHandle.getInfoString();
                        button.Size = new Size(lastUnseenImage.Width, lastUnseenImage.Height);
                        button.ForeColor = Color.White;
                        button.Click += new EventHandler(this.imageButtonDownload_Click);
                        this.flowLayoutPanel1.Controls.Add((Control)button);
                        if (MySQLHandle.lastseenID == 1)
                        {
                            this.attheendofDB = true;
                            break;
                        }
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (this.attheendofDB)
            {
                int num = (int)MessageBox.Show("There are no more images to be seen", "End of database", MessageBoxButtons.OK);
            }
            else
            {
                for (int index = 0; index < 10; ++index)
                {
                    Button button = new Button();
                    button.Name = MySQLHandle.getshowedID().ToString();
                    Image lastUnseenImage = MySQLHandle.getLastUnseenImage();
                    button.Image = lastUnseenImage;
                    button.Text = MySQLHandle.getInfoString();
                    button.Size = new Size(lastUnseenImage.Width, lastUnseenImage.Height);
                    this.flowLayoutPanel1.Controls.Add((Control)button);
                    button.Click += new EventHandler(this.imageButtonDownload_Click);
                    if (MySQLHandle.lastseenID == 1)
                    {
                        this.attheendofDB = true;
                        break;
                    }
                }
            }
        }
        public void imageButtonDownload_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                button.Image.Save(saveFileDialog.FileName, ImageFormat.Bmp);
            MySQLHandle.AddDownloadto(button.Name.Substring(0, button.Name.Length));
        }
    }
}

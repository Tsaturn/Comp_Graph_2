using System;
using System.Drawing;
using System.Windows.Forms;
using FastBitmapNamespace;

namespace Lab_2
{
    public partial class Task3 : Form
    {
        private Bitmap originalBitmap;
        public double hueAdjustment, saturationAdjustment, valueAdjustment;

        public Task3()
        {
            InitializeComponent();
            Load += Task3_Load;
        }

        private void Task3_Load(object sender, EventArgs e)
        {
            originalBitmap = (Bitmap)pictureBox1.Image;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            hueAdjustment = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            saturationAdjustment = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            valueAdjustment = trackBar3.Value;
        }

        private void ChangePicture()
        {
            if (originalBitmap == null) return;

            Bitmap bitmap = new Bitmap(originalBitmap);
            using (var fastBitmap = new FastBitmap(bitmap))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                {
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {
                        var color = fastBitmap[x, y];
                        double R = color.R / 255.0;
                        double G = color.G / 255.0;
                        double B = color.B / 255.0;
                        double min = Math.Min(R, Math.Min(G, B));
                        double max = Math.Max(R, Math.Max(G, B));
                        double delta = max - min;

                        double hue, saturation, value;

                        // Вычисление HUE
                        if (delta == 0)
                            hue = 0;
                        else if (max == R && G >= B)
                            hue = 60 * (G - B) / delta;
                        else if (max == R && G < B)
                            hue = 60 * (G - B) / delta + 360;
                        else if (max == G)
                            hue = 60 * (B - R) / delta + 120;
                        else
                            hue = 60 * (R - G) / delta + 240;

                        // Вычисление Saturation
                        if (max == 0)
                            saturation = 0;
                        else
                            saturation = delta / max;

                        // Вычисление Value (Яркость)
                        value = max;

                        hue += hueAdjustment;
                        if (hue < 0)
                            hue += 360;
                        else hue %= 360.0;

                        saturation += saturationAdjustment / 100f;
                        value += valueAdjustment / 100f;

                        saturation = Math.Max(0, Math.Min(1, saturation));
                        value = Math.Max(0, Math.Min(1, value));


                        Color newColor = ColorFromHSV(hue, saturation, value);
                        fastBitmap[x, y] = newColor;
                    }
                }
            }

            pictureBox1.Image = bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangePicture();
        }

        // Вспомогательная функция для конвертации HSV в RGB
        private Color ColorFromHSV(double h, double s, double v)
        {
            v = v * 255;
            int R = 0, G = 0, B = 0;

            int Hi = (int)(Math.Floor(h / 60.0)) % 6;
            double f = h / 60.0 - Math.Floor(h / 60.0);
            double p = v * (1 - s);
            double q = v * (1 - f * s);
            double t = v * (1 - (1 - f) * s);

            switch (Hi)
            {
                case 0:
                    R = (int)v; G = (int)t; B = (int)p;
                    break;
                case 1:
                    R = (int)q; G = (int)v; B = (int)p;
                    break;
                case 2:
                    R = (int)p; G = (int)v; B = (int)t;
                    break;
                case 3:
                    R = (int)p; G = (int)q; B = (int)v;
                    break;
                case 4:
                    R = (int)t; G = (int)p; B = (int)v;
                    break;
                case 5:
                    R = (int)v; G = (int)p; B = (int)q;
                    break;
            }

            return Color.FromArgb(R, G, B);
        }

        private void downloadFile(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog.FileName);
            }
        }

        private void Task3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
        }

        private void Task3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
        }



    }
}

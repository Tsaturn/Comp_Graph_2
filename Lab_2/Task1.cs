using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lab_2
{
    public partial class Task1 : Form
    {
        private Bitmap originalImage;
        private Bitmap grayImage1;
        private Bitmap grayImage2;
        private Bitmap diffImage;

        public Task1()
        {
            InitializeComponent();
            LoadImage();
        }

        private void LoadImage()
        {
            originalImage = Properties.Resources.img11;
            pictureBoxOriginal.Image = originalImage;
            ConvertImages();
        }

        private void ConvertImages()
        {
            if (originalImage != null)
            {
                grayImage1 = ConvertToGrayscale(originalImage, 1);
                grayImage2 = ConvertToGrayscale(originalImage, 2);

                diffImage = CalculateDifference(grayImage1, grayImage2);

                pictureBoxGray1.Image = grayImage1;
                pictureBoxGray2.Image = grayImage2;
                pictureBoxDiff.Image = diffImage;

                BuildHistogram(grayImage1, pictureBoxHist1);
                BuildHistogram(grayImage2, pictureBoxHist2);
            }
        }

        private Bitmap ConvertToGrayscale(Bitmap image, int formula)
        {
            Bitmap grayImage = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int grayValue;

                    if (formula == 1)
                    {
                        // Формула PAL/NTSC
                        grayValue = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
                    }
                    else
                    {
                        // Формула HDTV
                        grayValue = (int)(0.2126 * pixelColor.R + 0.7152 * pixelColor.G + 0.0722 * pixelColor.B);
                    }

                    grayValue = Math.Min(255, Math.Max(0, grayValue));
                    Color grayColor = Color.FromArgb(grayValue, grayValue, grayValue);
                    grayImage.SetPixel(x, y, grayColor);
                }
            }

            return grayImage;
        }

        private Bitmap CalculateDifference(Bitmap img1, Bitmap img2)
        {
            Bitmap diffImage = new Bitmap(img1.Width, img1.Height);

            for (int y = 0; y < img1.Height; y++)
            {
                for (int x = 0; x < img1.Width; x++)
                {
                    int diff = Math.Abs(img1.GetPixel(x, y).R - img2.GetPixel(x, y).R);
                    Color diffColor = Color.FromArgb(diff, diff, diff);
                    diffImage.SetPixel(x, y, diffColor);
                }
            }

            return diffImage;
        }

        private void BuildHistogram(Bitmap image, PictureBox pictureBox)
        {
            int[] histogram = new int[256];

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int intensity = pixelColor.R;
                    histogram[intensity]++;
                }
            }

            int histWidth = 256;
            int histHeight = 120;
            Bitmap histImage = new Bitmap(histWidth, histHeight);

            int max = histogram.Max();
            if (max == 0) max = 1; 

            using (Graphics g = Graphics.FromImage(histImage))
            {
                g.Clear(Color.White);

                for (int x = 0; x < histWidth; x++)
                {
                    int histValue = (int)((histogram[x] / (float)max) * histHeight);
                    using (Pen pen = new Pen(Color.Black))
                    {
                        g.DrawLine(pen, x, histHeight - 1, x, histHeight - histValue);
                    }
                }
            }

            pictureBox.Image = histImage;
        }
    }
}
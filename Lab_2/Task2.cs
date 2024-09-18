using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab_2
{
    public partial class Task2 : Form
    {
        private Bitmap originalImage, redChannel, greenChannel, blueChannel;

        public Task2()
        {
            InitializeComponent();
            SetupCharts();
        }

        private void Task2_Load(object sender, EventArgs e)
        {
            originalImage = (Bitmap)Properties.Resources.img11;
            // изображения для каждого канала 
            redChannel = new Bitmap(originalImage.Width, originalImage.Height);
            greenChannel = new Bitmap(originalImage.Width, originalImage.Height);
            blueChannel = new Bitmap(originalImage.Width, originalImage.Height);

            ProcessImage(); // обработка изображения 
        }

        // Настройка chart'ов
        private void SetupCharts()
        {
            chartRed.Series[0] = (new Series { Color = Color.Red, ChartType = SeriesChartType.Column, Name = "Red Channel" });
            chartGreen.Series[0] = new Series { Color = Color.Green, ChartType = SeriesChartType.Column, Name = "Green Channel" };
            chartBlue.Series[0] = (new Series { Color = Color.Blue, ChartType = SeriesChartType.Column, Name = "Blue Channel" });
        }

        // обработка изображения
        private void ProcessImage()
        {
            // Каналы
            int[] redHistogram = new int[256];
            int[] greenHistogram = new int[256];
            int[] blueHistogram = new int[256];

            // Обработка изображения и построение гистограмм
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color pixelColor = originalImage.GetPixel(x, y);

                    // установка цветного компонента
                    redChannel.SetPixel(x, y, Color.FromArgb(pixelColor.R, 0, 0));
                    greenChannel.SetPixel(x, y, Color.FromArgb(0, pixelColor.G, 0));
                    blueChannel.SetPixel(x, y, Color.FromArgb(0, 0, pixelColor.B));

                    redHistogram[pixelColor.R]++;
                    greenHistogram[pixelColor.G]++;
                    blueHistogram[pixelColor.B]++;
                }
            }

            // Заполнение гистограмм
            UpdateHistogramChart(chartRed, redHistogram);
            UpdateHistogramChart(chartGreen, greenHistogram);
            UpdateHistogramChart(chartBlue, blueHistogram);

            // Отображаем полученные каналы
            pictureBoxRed.Image = redChannel;
            pictureBoxGreen.Image = greenChannel;
            pictureBoxBlue.Image = blueChannel;
        }

        // Обновление графика
        private void UpdateHistogramChart(Chart chart, int[] histogram)
        {
            chart.Series[0].Points.Clear();

            for (int i = 0; i < histogram.Length; i++)
            {
                chart.Series[0].Points.AddXY(i, histogram[i]);
            }

            chart.Invalidate(); // Обновление графика
        }

        private void Task2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
        }

        private void Task2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();
        }
    }
}

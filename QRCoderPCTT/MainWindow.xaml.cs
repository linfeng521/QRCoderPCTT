using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QRCoder;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Microsoft.Win32;

namespace QRCoderPCTT
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap qrCodeBitmap; // 全局变量
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateQRCode_Click(object sender, RoutedEventArgs e)
        {
            string textToEncode = textBox.Text;

            if (!string.IsNullOrEmpty(textToEncode))
            {
                //创建一个新的QRCodeGenerator实例
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                //创建一个二维码 “textToEncode”为显示内容
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(textToEncode, QRCodeGenerator.ECCLevel.Q);
                //放入生成的二维码
                QRCode qrCode = new QRCode(qrCodeData);
                //获取到二维码图形
                qrCodeBitmap = qrCode.GetGraphic(20); // You can adjust the size
                //// Add Chinese characters to the bottom center of the QR Code
                //using (Graphics g = Graphics.FromImage(qrCodeBitmap))
                //{
                //    Font font = new Font("微软雅黑", 9); // You can adjust the font and size
                //    System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.Black);
                //    StringFormat stringFormat = new StringFormat
                //    {
                //        Alignment = StringAlignment.Center,
                //        LineAlignment = StringAlignment.Far // Bottom align
                //    };

                //    string chineseText = "你的汉字"; // 替换为你想要添加的汉字
                //    g.DrawString(chineseText, font, brush, new RectangleF(0, 0, qrCodeBitmap.Width, qrCodeBitmap.Height), stringFormat);
                //}
                // Convert Bitmap to BitmapImage for WPF
                var bitmapImage = BitmapToBitmapImage(qrCodeBitmap);
                qrCodeImageControl.Source = bitmapImage;
               // qrCodeImage.set
                //qrCodeImage.Dispose();
                //qrCodeImageControl.Source = bitmapImage;
            }
            else
            {
                MessageBox.Show("请输入文本以生成二维码。不能为空!Okay", "输入要求", MessageBoxButton.OK, MessageBoxImage.Information);
                textBox.Focus();
            }
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private void SaveQRCode_Click(object sender, RoutedEventArgs e)
        {
            if (qrCodeBitmap != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Files|*.png|All Files|*.*";
                saveFileDialog.Title = "Save QR Code Image";
                saveFileDialog.FileName = "QRCode"; // 初始文件名

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Save the Bitmap as PNG with the user-selected file name and path
                    SaveBitmapAsPng(qrCodeBitmap, saveFileDialog.FileName);
                    MessageBox.Show($"二维码已成功保存到：{saveFileDialog.FileName}", "保存成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("请先生成二维码再保存。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveBitmapAsPng(Bitmap qrCodeBitmap, string fileName)
        {
            qrCodeBitmap.Save(fileName, ImageFormat.Png);
        }
    }
}

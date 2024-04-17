using Porlam.Properties;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Porlam
{
    /// <summary>
    /// Логика взаимодействия для ProjectInfo.xaml
    /// </summary>
    public partial class ProjectInfo : Window
    {
        private byte[] zadanie= null;
        private DateTime datebefore;
        private DateTime datefrom;
        public ProjectInfo()
        {
            InitializeComponent();
            ResetUI();
        }

        private void ResetUI()
        {
            Server.Select("Active");
            DataTable data = Server.dataSet.Tables["Active"];
            var res = from row in data.AsEnumerable()
                      where row.Field<string>("Title") == Settings.Default.Post_id
                      select row;
            DataRow rowActive = res.First();
            _post_title.Text = rowActive[0].ToString();
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(rowActive[1].ToString());
            bitmapImage.EndInit();
            _post_img.Source = bitmapImage;
            _post_point.Text += rowActive[3].ToString();
            datebefore = (DateTime)rowActive[4];
            datefrom = (DateTime)rowActive[5];
            _post_datebefore.Text += datebefore.ToShortDateString();
            _post_datefrom.Text += datefrom.ToShortDateString();
            zadanie = (byte[])rowActive[2];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Перейти к файлу?", "Сохранение файла задания", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Yes)
            {
                File.WriteAllBytes($"E:\\zadanie.xlsx", zadanie);
                Process.Start("explorer.exe", @"E:\...");
            }
            if (Settings.Default.Client_ID != -1 && DateTime.Now < datebefore)
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add(Settings.Default.Client_ID);
                arrayList.Add(_post_title.Text);
                arrayList.Add(DateTime.Now);
                Server.Insert("Jurnal_Active", arrayList);
            }
        }
    }
}

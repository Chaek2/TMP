using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Porlam.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;

namespace Porlam
{
    /// <summary>
    /// Логика взаимодействия для ProjectInfo.xaml
    /// </summary>
    public partial class ProjectInfo : Window
    {
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
            _name.Text = rowActive[0].ToString();
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(rowActive[1].ToString());
            bitmapImage.EndInit();
            _img.Source = bitmapImage;
            _num.Text += rowActive[3].ToString();
            MessageBoxResult dialogResult = MessageBox.Show("Перейти к файлу?", "Сохранение файла задания", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogResult  == MessageBoxResult.Yes)
            {
                File.WriteAllBytes($"D:\\{rowActive[0].ToString()}.xlsx", (byte[])rowActive[2]);
                Process.Start("explorer.exe", @"D:\...");
            }
            if (Settings.Default.Client_ID!=-1)
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add(Settings.Default.Client_ID);
                arrayList.Add(rowActive[0].ToString());
                arrayList.Add(DateTime.Now);
                Server.Insert("Jurnal_Active", arrayList);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

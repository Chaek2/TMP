using Porlam.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Support.xaml
    /// </summary>
    public partial class Support : Window
    {
        public Support()
        {
            InitializeComponent();
            ResetUI();
        }

        private void ResetUI()
        {
            _id.Text += Settings.Default.Message_ID;
            Server.Select("Request");
            DataTable data = Server.dataSet.Tables["Request"];            
            if (data != null)
            {
                DataRow[] rows = data.Select($"Message_ID = '{Settings.Default.Message_ID}'");
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Vertical;
                foreach (DataRow item in rows)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = item[2].ToString();
                    textBlock.Padding = new Thickness(5);
                    stackPanel.Children.Add( textBlock );
                }
                _border.Child = stackPanel;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_txt.Text.Length > 1)
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add(Settings.Default.Message_ID);
                arrayList.Add(_txt.Text);
                bool insert = Server.Insert("Request", arrayList);
                if (!insert)
                {
                    MessageBox.Show("Запрос не отправлен");
                }
            }
            ResetUI();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

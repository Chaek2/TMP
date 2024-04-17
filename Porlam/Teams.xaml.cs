using Porlam.Properties;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Porlam
{
    /// <summary>
    /// Логика взаимодействия для Teams.xaml
    /// </summary>
    public partial class Teams : Window
    {
        private WordSert words = new WordSert();
        public Teams()
        {
            InitializeComponent();
            ResetUI();
        }
        private void ResetUI()
        {
            if (Settings.Default.Client_post == "Председатель")
            {
                btn_sert.Visibility = Visibility.Visible;
            }
            Server.Select("People");
            DataTable data = Server.dataSet.Tables["People"];
            if (data != null)
            {
                DataTable dataTable = data.AsEnumerable()
                       .OrderByDescending(r => r.Field<int>("Activities_Number"))
                       .ThenBy(r => r.Field<string>("Surname"))
                       .CopyToDataTable();
                StackPanel grid = new StackPanel();
                foreach (DataRow dr in dataTable.Rows)
                {
                    Border border = new Border();
                    border.Padding = new Thickness(10);
                    border.BorderBrush = new SolidColorBrush(Colors.Black);
                    border.BorderThickness = new Thickness(1);

                    TextBlock id_people = new TextBlock();
                    id_people.Padding = new Thickness(5);
                    id_people.Text = dr["ID_People"].ToString();

                    TextBlock surname = new TextBlock();
                    surname.Padding = new Thickness(5);
                    surname.Text = dr["Surname"].ToString();

                    TextBlock name = new TextBlock();
                    name.Padding = new Thickness(5);
                    name.Text = dr["Name"].ToString();

                    TextBlock activities_number = new TextBlock();
                    activities_number.Padding = new Thickness(5);
                    activities_number.Text = dr["Activities_Number"].ToString();

                    StackPanel stackPanel = new StackPanel();

                    stackPanel.Orientation = Orientation.Horizontal;
                    border.Child = stackPanel;
                    stackPanel.Children.Add(id_people);
                    stackPanel.Children.Add(surname);
                    stackPanel.Children.Add(name);
                    stackPanel.Children.Add(activities_number);

                    grid.Children.Add(border);

                }
                _border_grid.Child = grid;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn_sert_Click(object sender, RoutedEventArgs e)
        {

            Server.Select("People");
            DataTable data = Server.dataSet.Tables["People"];
            if (data != null)
            {
                DataTable dataTable = data.AsEnumerable()
                       .OrderByDescending(r => r.Field<int>("Activities_Number"))
                       .ThenBy(r => r.Field<string>("Surname"))
                       .CopyToDataTable();
                TelegramBots.init();
                foreach (DataRow dr in dataTable.Rows)
                {
                    words.Sert(dr);
                    TelegramBots.SetMessageDocument(@"E:\2\tmp\" + dr["ID_People"].ToString() + ".doc", dr["ID_People"].ToString());
                }
            }
        }
    }
}

using Porlam.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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

namespace Porlam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Server.init();
            Server.ResetActiveNumber();
            ResetUI();            
            Settings.Default.Message_ID = BitConverter.ToString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(System.Environment.MachineName))).Replace("-", String.Empty);
            Settings.Default.Save();
            Console.WriteLine(Settings.Default.Client_post);
        }

        private void ResetUI()
        {
            Server.Select("Active");
            _list_active.ItemsSource = Server.dataSet.Tables["Active"].DefaultView;
            if(Settings.Default.Client_ID != -1)
            {
                if(Settings.Default.Client_post == "Председатель")
                {
                    btn_admin.Visibility = Visibility.Visible;
                }
                btn_exit.Visibility = Visibility.Visible;
                btn_auth.Visibility = Visibility.Hidden;
            }
            else
            {
                btn_exit.Visibility = Visibility.Hidden;
                btn_admin.Visibility = Visibility.Hidden;
                btn_auth.Visibility = Visibility.Visible;
            }
        }

        private void mouse_click(object sender, MouseButtonEventArgs e)
        {
            int id = _list_active.SelectedIndex;
            DataRowView dataRow = _list_active.SelectedItem as DataRowView;
            if (dataRow != null)
            {
                Settings.Default.Post_id = dataRow.Row[0].ToString();
                ProjectInfo projectInfo = new ProjectInfo();
                projectInfo.Show();
                this.Close();
            }
        }

        private void btn_auth_Click(object sender, RoutedEventArgs e)
        {
            Authentication authentication = new Authentication();
            authentication.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Teams teams = new Teams();
            teams.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Support support = new Support();
            support.Show();
            this.Close();
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Client_ID = -1;
            Settings.Default.Client_post = "";
            Settings.Default.Clinet_login = "";
            Settings.Default.Clinet_password = "";
            Settings.Default.Save();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn_admin_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
            this.Close();
        }
    }
}

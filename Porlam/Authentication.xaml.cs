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
    /// Логика взаимодействия для Authentication.xaml
    /// </summary>
    public partial class Authentication : Window
    {
        bool auth_grid = true;
        public Authentication()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (auth_grid)
            {
                Authing(_login.Text,_pass.Password);
            }
            else
            {
                Register();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (auth_grid)
            {
                _Auth.Height = 450;
                _auth_grid.Visibility = Visibility.Hidden;
                _reg_grid.Visibility = Visibility.Visible;
                auth_grid = false;
            }
            else
            {
                _Auth.Height = 330;
                _auth_grid.Visibility = Visibility.Visible;
                _reg_grid.Visibility = Visibility.Hidden;
                auth_grid = true;
            }
        }

        private void Authing(string login,string password)
        {
            if (login.Length > 0 && password.Length > 0)
            {
                Server.Select("People");
                DataTable people = Server.dataSet.Tables["People"];
                var res = from row in people.AsEnumerable()
                          where row.Field<string>("Login") == login.ToString() && row.Field<string>("Password") == password.ToString()
                          select row;
                if (res.Count() > 0)
                {
                    DataRow rowActive = res.First();
                    Settings.Default.Client_ID = Int32.Parse(rowActive[0].ToString());
                    Settings.Default.Clinet_login = login;
                    Settings.Default.Clinet_password = password;
                    Settings.Default.Client_post = rowActive[6].ToString();
                    Settings.Default.Save();
                    Server.Authing();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    _pass.BorderBrush = new SolidColorBrush(Colors.Red);
                    _login.BorderBrush = new SolidColorBrush(Colors.Red);
                    _error.Content = "Неправильный логин или пароль.";
                }
            }
            else
            {
                if (_login.Text.Length < 1) _login.BorderBrush = new SolidColorBrush(Colors.Red);
                else _login.BorderBrush = new SolidColorBrush(Colors.Black);
                if (_pass.Password.Length < 1) _pass.BorderBrush = new SolidColorBrush(Colors.Red);
                else _pass.BorderBrush = new SolidColorBrush(Colors.Black);
                _error.Content = "Поля логин и пароль не должны быть пустыми";
            }
        }

        private void Register()
        {
            if (_login_reg.Text.Length > 0 && _pass_reg.Text.Length > 0 
                && _surname_reg.Text.Length > 0 && _name_reg.Text.Length > 0)
            {
                Server.Select("People");
                DataTable active = Server.dataSet.Tables[2];
                var res = from row in active.AsEnumerable()
                          where row.Field<string>("Login") == _login.Text.ToString()
                          select row;
                if (res.Count() < 1)
                {

                    ArrayList arrayList = new ArrayList();
                    arrayList.Add(_surname_reg.Text);
                    arrayList.Add(_name_reg.Text);
                    arrayList.Add(0);
                    arrayList.Add(_login_reg.Text);
                    arrayList.Add(_pass_reg.Text);
                    arrayList.Add("Активист");
                    bool insert = Server.Insert("People", arrayList);
                    if (insert)
                    {
                        Authing(_login_reg.Text, _pass_reg.Text);
                    }
                    else
                    {
                        _error_reg.Content = "Поля не соответствуют ограничениям.";
                    }
                }
                else
                {
                    _login_reg.BorderBrush = new SolidColorBrush(Colors.Red);
                    _pass_reg.BorderBrush = new SolidColorBrush(Colors.Red);
                    _error_reg.Content = "Такой логин уже существует.";
                }
            }
            else
            {
                if (_login_reg.Text.Length < 1) _login_reg.BorderBrush = new SolidColorBrush(Colors.Red);
                else _login_reg.BorderBrush = new SolidColorBrush(Colors.Black);
                if (_pass_reg.Text.Length < 1) _pass_reg.BorderBrush = new SolidColorBrush(Colors.Red);
                else _pass_reg.BorderBrush = new SolidColorBrush(Colors.Black);
                if (_surname_reg.Text.Length < 1) _surname_reg.BorderBrush = new SolidColorBrush(Colors.Red);
                else _surname_reg.BorderBrush = new SolidColorBrush(Colors.Black);
                if (_name_reg.Text.Length < 1) _name_reg.BorderBrush = new SolidColorBrush(Colors.Red);
                else _name_reg.BorderBrush = new SolidColorBrush(Colors.Black);
                _error_reg.Content = "Все поля должны быть заполнены";
            }
        }

    }
}

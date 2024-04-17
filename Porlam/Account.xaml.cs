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
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        public Account()
        {
            InitializeComponent();
            ResetUI();
        }

        private void ResetUI()
        {
            Server.Select("People");
            DataTable active = Server.dataSet.Tables[2];
            var res = from row in active.AsEnumerable()
                      where row.Field<int>("ID_People") == Settings.Default.Client_ID
                      select row;
            DataRow ac = res.ElementAt(0);
            Settings.Default.Client_ID = int.Parse(ac["ID_People"].ToString());
            Settings.Default.Clinet_login = ac["Login"].ToString();
            Settings.Default.Clinet_password = ac["Password"].ToString();
            _surname.Text = ac["Surname"].ToString();
            _name.Text = ac["Name"].ToString();
            _point.Text = ac["Activities_Number"].ToString();
            _login.Text = ac["Login"].ToString();
            _password.Text = ac["Password"].ToString();
            _phone.Text = ac["Phone"].ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_login.Text.Length > 5)
            {
                Server.Select("People");
                DataTable active = Server.dataSet.Tables[2];
                var res = from row in active.AsEnumerable()
                          where row.Field<string>("Login") == _login.Text.ToString()
                          select row;
                var acc = from row in active.AsEnumerable()
                          where row.Field<int>("ID_People") == Settings.Default.Client_ID
                          select row;
                if (res.Count() < 1)
                {
                    if (acc.Count() < 1)
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
                    DataRow ac = acc.ElementAt(0);
                    ArrayList arrayList = new ArrayList();
                    arrayList.Add(ac["Surname"]);
                    arrayList.Add(ac["Name"]);
                    arrayList.Add(ac["Activities_Number"]);
                    arrayList.Add(_login.Text);
                    arrayList.Add(ac["Password"]);
                    arrayList.Add(ac["Phone"]);
                    arrayList.Add(ac["Post_ID"]);
                    bool update = Server.Update("People", arrayList, Settings.Default.Client_ID.ToString());
                    if (update)
                    {
                        _error.Text = "Логин изменён";
                    }
                    else
                    {
                        _error.Text = "Ошибка изменения логина";
                    }
                }
            }
            ResetUI();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Server.Select("People");
            DataTable active = Server.dataSet.Tables[2];
            var acc = from row in active.AsEnumerable()
                      where row.Field<int>("ID_People") == Settings.Default.Client_ID
                      select row;
            if (acc.Count() < 1)
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
            DataRow ac = acc.ElementAt(0);
            ArrayList arrayList = new ArrayList();
            arrayList.Add(ac["Surname"]);
            arrayList.Add(ac["Name"]);
            arrayList.Add(ac["Activities_Number"]);
            arrayList.Add(ac["Login"]);
            arrayList.Add(_password.Text);
            arrayList.Add(ac["Phone"]);
            arrayList.Add(ac["Post_ID"]);
            bool update = Server.Update("People", arrayList, Settings.Default.Client_ID.ToString());
            if (update)
            {
                _error.Text = "Пароль изменён";
            }
            else
            {
                _error.Text = "Ошибка изменения пароля";
            }
            ResetUI();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (_phone.Text.Length == 11)
            {

                Server.Select("People");
                DataTable active = Server.dataSet.Tables[2];
                var acc = from row in active.AsEnumerable()
                          where row.Field<int>("ID_People") == Settings.Default.Client_ID
                          select row;
                if (acc.Count() < 1)
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
                DataRow ac = acc.ElementAt(0);
                ArrayList arrayList = new ArrayList();
                arrayList.Add(ac["Surname"]);
                arrayList.Add(ac["Name"]);
                arrayList.Add(ac["Activities_Number"]);
                arrayList.Add(ac["Login"]);
                arrayList.Add(ac["Password"]);
                arrayList.Add(_phone.Text);
                arrayList.Add(ac["Post_ID"]);
                bool update = Server.Update("People", arrayList,Settings.Default.Client_ID.ToString());
                if (update)
                {
                    _error.Text = "Телефон изменён";
                }
                else
                {
                    _error.Text = "Ошибка изменения телефона";
                }
            }
            else
            {
                _error.Text = "Телефон содержит 11 цифр. Пример: 89456012112";
            }
            ResetUI();
        }
    }
}

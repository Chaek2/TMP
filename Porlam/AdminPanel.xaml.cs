using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Porlam
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        string tablename = "";
        byte[] buffer;
        public AdminPanel()
        {
            InitializeComponent();
            TelegramBots.init();
            _dg.CanUserAddRows = false;
            _dg.CanUserDeleteRows = false;
            _dg.IsReadOnly = true;
            List<string> list = Server.TableNames();
            _list_table.Items.Clear();
            _list_table.ItemsSource = list;
        }

        private void _list_table_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(_list_table, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                Restart(item.ToString().Replace("System.Windows.Controls.ListBoxItem: ", ""));
            }
        }

        private void Restart(string table)
        {            
            tablename = table;
            if (tablename != "")
            {
                if (table == "Jurnal_People")
                {
                    Server.Select("Jurnal_People");
                    _dg.ItemsSource = Server.dataSet.Tables[table].DefaultView;
                }
                if (table == "People")
                {
                    Server.Select("People");
                    _dg.ItemsSource = Server.dataSet.Tables[table].DefaultView;
                }
                if (table == "Post")
                {
                    _grid_post.Visibility = Visibility.Visible;
                    Server.Select("Post");
                    _dg.ItemsSource = Server.dataSet.Tables[table].DefaultView;

                }
                else _grid_post.Visibility = Visibility.Hidden;
                if (table == "Active")
                {
                    _grid_active.Visibility = Visibility.Visible;
                    Server.Select("Active");
                    _dg.ItemsSource = Server.dataSet.Tables[table].DefaultView;
                }
                else _grid_active.Visibility = Visibility.Hidden;
                if (table == "Jurnal_Active")
                {
                    DataTable active = Server.dataSet.Tables["Active"];
                    DataTable people = Server.dataSet.Tables["People"];
                    _jurnal_active_ID.Items.Clear();
                    _jurnal_active_project.Items.Clear();
                    foreach (DataRow item in active.Rows)
                    {
                        _jurnal_active_project.Items.Add(item[0].ToString());
                    }
                    if(active.Rows.Count > 0) _jurnal_active_project.SelectedIndex = 0;
                    foreach (DataRow item in people.Rows)
                    {
                        _jurnal_active_ID.Items.Add(item[0].ToString());
                    }
                    if (people.Rows.Count > 0) _jurnal_active_ID.SelectedIndex = 0;
                    _grid_jurnal_active.Visibility = Visibility.Visible;
                    Server.Select("Jurnal_Active");
                    _dg.ItemsSource = Server.dataSet.Tables[table].DefaultView;
                }
                else _grid_jurnal_active.Visibility = Visibility.Hidden;
                if (table == "Request")
                {
                    _grid_request.Visibility = Visibility.Visible;
                    Server.Select("Request");
                    _dg.ItemsSource = Server.dataSet.Tables[table].DefaultView;
                }
                else _grid_request.Visibility = Visibility.Hidden;
            }
            else
            {
                _grid_active.Visibility = Visibility.Hidden;
                _grid_jurnal_active.Visibility = Visibility.Hidden;
                _grid_post.Visibility = Visibility.Hidden;
                _grid_request.Visibility = Visibility.Hidden;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if(this.WindowState == WindowState.Minimized)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void _insert_Click(object sender, RoutedEventArgs e)
        {
            //add
            bool error_tag = false;
            ArrayList array;
            switch (tablename)
            {
                case "Post":
                    if (_post_title.Text.Length > 1)
                    {
                        array = new ArrayList();
                        array.Add(_post_title.Text);
                        error_tag = Server.Insert("Post", array);
                    }
                    break;
                case "Active":
                    if (_active_title.Text.Length > 1 && _active_file.Content.ToString() == "Выбран"
                        && DateTime.Now < _active_datebefore.SelectedDate)
                    {
                        array = new ArrayList();
                        array.Add(_active_title.Text);
                        array.Add(_active_img.Text);
                        array.Add(buffer);
                        array.Add(Int32.Parse(_active_num.Text));
                        array.Add(_active_datebefore.SelectedDate);
                        array.Add(DateTime.Now);
                        error_tag = Server.Insert("Active", array);
                        if (error_tag)
                        {
                            EmailSet(_active_title.Text,_active_num.Text);
                        }
                    }
                    break;
                case "Request":
                    if (_request_id.Text.Length > 1 && _request_message.Text.Length > 1)
                    {
                        array = new ArrayList();
                        array.Add(_request_id.Text);
                        array.Add(_request_message.Text);
                        error_tag = Server.Insert("Request", array);
                    }
                    break;
                case "Jurnal_Active":
                    array = new ArrayList();
                    array.Add(_jurnal_active_ID.SelectedValue.ToString());
                    array.Add(_jurnal_active_project.SelectedValue.ToString());
                    array.Add(DateTime.Now);
                    error_tag = Server.Insert("Jurnal_Active", array);
                    break;
            }
            foreach (string item in Server.TableNames())
            {
                Server.Select(item);
            }
            Restart(tablename);
        }

        private void _update_Click(object sender, RoutedEventArgs e)
        {
            //update
            bool error_tag = false;
            DataRowView dataRowView = _dg.SelectedItem as DataRowView;
            ArrayList array;
            switch (tablename)
            {
                case "Post":
                    if (_post_title.Text.Length > 1)
                    {
                        if (dataRowView != null)
                        {
                            array = new ArrayList();
                            array.Add(_post_title.Text);
                            error_tag = Server.Update("Post", array, dataRowView[0].ToString());
                        }
                    }
                    break;
                case "Active":
                    if (_active_title.Text.Length > 1 && _active_file.Content.ToString() == "Выбран")
                    {
                        if (dataRowView != null)
                        {
                            array = new ArrayList();
                            array.Add(_active_img.Text);
                            array.Add(buffer);
                            array.Add(Int32.Parse(_active_num.Text));
                            array.Add(_active_datebefore.SelectedDate);
                            array.Add(DateTime.Now);
                            array.Add(_active_title.Text);
                            error_tag = Server.Update("Active", array, dataRowView[0].ToString());
                        }
                    }
                    break;
                case "Request":
                    if (_request_id.Text.Length > 1 && _request_message.Text.Length > 1)
                    {
                        if (dataRowView != null)
                        {
                            array = new ArrayList();
                            array.Add(_request_id.Text);
                            array.Add(_request_message.Text);
                            error_tag = Server.Update("Request", array, dataRowView[0].ToString());
                        }
                    }
                    break;
                case "Jurnal_Active":
                    if (dataRowView != null)
                    {
                        array = new ArrayList();
                        array.Add(_jurnal_active_ID.SelectedValue.ToString());
                        array.Add(_jurnal_active_project.SelectedValue.ToString());
                        array.Add(DateTime.Now);
                        error_tag = Server.Update("Jurnal_Active", array, dataRowView[0].ToString());
                    }
                    break;
            }
            foreach (string item in Server.TableNames())
            {
                Server.Select(item);
            }
            Restart(tablename);
        }

        private void _delete_Click(object sender, RoutedEventArgs e)
        {
            //delete
            bool error_tag = false;
            DataRowView dataRowView = _dg.SelectedItem as DataRowView;
            switch (tablename)
            {
                case "Post":
                    if (dataRowView != null) error_tag = Server.Delete("Post", dataRowView[0].ToString());
                    break;
                case "Active":
                    if (dataRowView != null) error_tag = Server.Delete("Active", dataRowView[0].ToString());
                    break;
                case "Request":
                    if (dataRowView != null) error_tag = Server.Delete("Request", dataRowView[0].ToString());
                    break;
                case "Jurnal_Active":
                    if (dataRowView != null) error_tag = Server.Delete("Jurnal_Active", dataRowView[0].ToString());
                    break;
            }
            foreach (string item in Server.TableNames())
            {
                Server.Select(item);
            }
            Restart(tablename);
        }

        private void _dg_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataRowView dataRowView = _dg.SelectedItem as DataRowView;
            if (dataRowView != null)
            {
                switch (tablename)
                {
                    case "Post":
                        _post_title.Text = dataRowView[0].ToString();
                        break;
                    case "Active":
                        _active_title.Text = dataRowView[0].ToString();
                        _active_img.Text = dataRowView[1].ToString();
                        buffer = (byte[])dataRowView[2];
                        _active_file.Content = "Выбран";
                        _active_num.Text = dataRowView[3].ToString();
                        _active_datebefore.SelectedDate = (DateTime)dataRowView[4];
                        break;
                    case "Request":
                        _request_id.Text = dataRowView[1].ToString();
                        _request_message.Text = dataRowView[2].ToString();
                        break;
                    case "Jurnal_Active":
                        _jurnal_active_ID.SelectedValue = dataRowView[2].ToString();
                        _jurnal_active_project.SelectedValue = dataRowView[3].ToString();
                        break;
                }
            }
        }

        private void _active_file_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                buffer = File.ReadAllBytes(dlg.FileName);
                _active_file.Content = "Выбран";
            }
            else
            {
                _active_file.Content = "файл не выбран";
            }
        }

        private void EmailSet( string title,string number)
        {
            TelegramBots.SetMessage(title, number);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Porlam.Properties;
using Microsoft.Extensions.Logging;
using NLog;

namespace Porlam
{
    public class Server
    {
        //public static
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public static List<String> tableName;  // таблицы

        public static DataSet dataSet = new DataSet();

        //private static
        private static string connText = "workstation id=molparlam.mssql.somee.com;packet size=4096;user id=Chaek2_SQLLogin_1;pwd=3n1y15qn22;data source=molparlam.mssql.somee.com;persist security info=False;initial catalog=molparlam;TrustServerCertificate=True"; // строка подключения
        private static string return_TableName = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES"; // вывод таблиц
        private static string return_ColumnName = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '"; // вывод столбцов таблицы
        private static string return_ColumnID = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE ORDINAL_POSITION = 1 and TABLE_NAME = '"; // вывод 1-ого столбца таблицы

        private static SqlConnection sqlConn = new SqlConnection(connText);

        //private

        // конструктор класса
        public static void init()
        {
            if (dataSet.Tables.Count == 0)
            {
                _logger.Fatal("Первый старт приложения");
                tableName = TableNames();
                SQL_State();
                for (int i = 0; i < tableName.Count(); i++)
                {
                    dataSet.Tables.Add(new DataTable(tableName[i]));
                    Select(tableName[i]);
                }
                SQL_State();
            }
        }

        // метод управления соединением
        private static void SQL_State()
        {
            try
            {
                if (ConnectionState.Open == sqlConn.State) sqlConn.Close();
                else sqlConn.Open();
            }
            catch { }
        }

        public static void Select(string table)
        {
            bool isValid = tableName.Any(str => str == table);
            if (isValid)
            {
                _logger.Fatal($"SELECT {table}");
                try
                {
                    SQL_State();
                    SqlDataAdapter adapter = new SqlDataAdapter("", sqlConn);
                    DataTable datatable = new DataTable();
                    dataSet.Tables[table].Columns.Clear();
                    dataSet.Tables[table].Rows.Clear();
                    adapter.SelectCommand = new SqlCommand($"SELECT * FROM {table}", sqlConn);
                    adapter.Fill(dataSet.Tables[table]);
                }
                catch
                {
                }
                finally
                {
                    SQL_State();
                }
            }
        }
        public static bool Insert(string table, ArrayList valueList = null)
        {
            bool isValid = tableName.Any(str => str == table);
            if (isValid && valueList.Count > 0)
            {
                _logger.Fatal($"INSERT {table}");
                try
                {
                    SQL_State();
                    string query = "";
                    SqlDataAdapter adapter = new SqlDataAdapter("", sqlConn);
                    DataTable datatable = new DataTable();
                    SqlCommand command = new SqlCommand("", sqlConn);
                    dataSet.Tables[table].Columns.Clear();
                    dataSet.Tables[table].Rows.Clear();
                    command.CommandText = return_ColumnName + $"{table}'";
                    datatable.Load(command.ExecuteReader());
                    query = $"INSERT {table} (";
                    if (datatable.Rows[0][0].ToString().IndexOf("ID_")!=-1) {

                        for (int i = 1; i < datatable.Rows.Count; i++)
                        {
                            query += $" {datatable.Rows[i][0]}";
                            if (i < datatable.Rows.Count - 1)
                                query += ",";
                        }
                        query += ") VALUES (";
                        for (int i = 1; i <= datatable.Rows.Count - 1; i++)
                        {
                            query += $" @{datatable.Rows[i][0]}";
                            if (i < datatable.Rows.Count - 1)
                                query += ",";
                        }
                        query += ")";
                        adapter.InsertCommand = new SqlCommand(query, sqlConn);
                        adapter.InsertCommand.Parameters.Clear();
                        for (int i = 1; i < datatable.Rows.Count; i++)
                        {
                            adapter.InsertCommand.Parameters.AddWithValue($"@{datatable.Rows[i][0]}", valueList[i - 1]);
                        }
                        adapter.InsertCommand.ExecuteNonQuery();
                    }
                    else
                    {

                        for (int i = 0; i < datatable.Rows.Count; i++)
                        {
                            query += $" {datatable.Rows[i][0]}";
                            if (i < datatable.Rows.Count - 1)
                                query += ",";
                        }
                        query += ") VALUES (";
                        for (int i = 0; i <= datatable.Rows.Count - 1; i++)
                        {
                            query += $" @{datatable.Rows[i][0]}";
                            if (i < datatable.Rows.Count - 1)
                                query += ",";
                        }
                        query += ")";
                        adapter.InsertCommand = new SqlCommand(query, sqlConn);
                        adapter.InsertCommand.Parameters.Clear();
                        for (int i = 0; i < datatable.Rows.Count; i++)
                        {
                            adapter.InsertCommand.Parameters.AddWithValue($"@{datatable.Rows[i][0]}", valueList[i]);
                        }
                        adapter.InsertCommand.ExecuteNonQuery();
                    }
                    adapter.SelectCommand = new SqlCommand($"SELECT * FROM {table}", sqlConn);
                    adapter.Fill(dataSet.Tables[table]);
                }
                catch
                {
                    return false;
                }
                finally
                {
                    SQL_State();
                }
                ResetActiveNumber();
                return true;
            }
            return false;
        }
        public static bool Update(string table, ArrayList valueList = null, string id = "")
        {
            ResetActiveNumber();
            bool isValid = tableName.Any(str => str == table);
            if (isValid && id != "" && valueList.Count > 0)
            {
                _logger.Fatal($"UPDATE {table}");
                try
                {
                    SQL_State();
                    string query = "";
                    SqlDataAdapter adapter = new SqlDataAdapter("", sqlConn);
                    DataTable datatable = new DataTable();
                    SqlCommand command = new SqlCommand("", sqlConn);
                    dataSet.Tables[table].Columns.Clear();
                    dataSet.Tables[table].Rows.Clear();
                    command.CommandText = return_ColumnName + $"{table}'";
                    datatable.Load(command.ExecuteReader());
                    if (valueList.Count == datatable.Rows.Count) { }
                    query = $"UPDATE {table} SET ";
                    for (int i = 1; i < datatable.Rows.Count; i++)
                    {
                        query += $" {datatable.Rows[i][0]} = @{datatable.Rows[i][0]}";
                        if (i < datatable.Rows.Count - 1)
                            query += ",";
                    }
                    query += $" WHERE {datatable.Rows[0][0]} = @id";
                    adapter.UpdateCommand = new SqlCommand(query, sqlConn);
                    adapter.UpdateCommand.Parameters.Clear();
                    adapter.UpdateCommand.Parameters.AddWithValue($"@id", id);
                    for (int i = 1; i <= datatable.Rows.Count - 1; i++)
                    {
                        adapter.UpdateCommand.Parameters.AddWithValue($"@{datatable.Rows[i][0]}", valueList[i - 1]);
                    }
                    adapter.UpdateCommand.ExecuteNonQuery();
                    adapter.SelectCommand = new SqlCommand($"SELECT * FROM {table}", sqlConn);
                    adapter.Fill(dataSet.Tables[table]);
                }
                catch
                {
                    return false;
                }
                finally
                {
                    SQL_State();
                }
                return true;
            }
            return false;
        }
        public static bool Delete(string table, string id = "")
        {
            ResetActiveNumber();
            bool isValid = tableName.Any(str => str == table);
            if (isValid && id != "")
            {
                _logger.Fatal($"DELETE {table}");
                try
                {
                    SQL_State();
                    SqlDataAdapter adapter = new SqlDataAdapter("", sqlConn);
                    DataTable datatable = new DataTable();
                    SqlCommand command = new SqlCommand("", sqlConn);
                    dataSet.Tables[table].Columns.Clear();
                    dataSet.Tables[table].Rows.Clear();
                    command.CommandText = return_ColumnID + $"{table}'";
                    datatable.Load(command.ExecuteReader());    
                    adapter.DeleteCommand = new SqlCommand($"DELETE FROM {table} WHERE {datatable.Rows[0][0]} = '{id}'", sqlConn);
                    adapter.DeleteCommand.ExecuteNonQuery();
                    adapter.SelectCommand = new SqlCommand($"SELECT * FROM {table}", sqlConn);
                    adapter.Fill(dataSet.Tables[table]);
                }
                catch
                {
                    return false;
                }
                finally
                {
                    SQL_State();
                }
                return true;
            }
            return false;
        }
        public static void ResetActiveNumber()
        {
            try
            {
                int a;
                Select("Active");
                Select("Jurnal_Active");
                Select("People");
                DataTable active = dataSet.Tables["Active"];
                DataTable people = dataSet.Tables["People"];
                DataTable jurnal_active = dataSet.Tables["Jurnal_Active"];
                Dictionary<string, int> jurnal = new Dictionary<string, int>();
                foreach (DataRow item in jurnal_active.Rows)
                {
                    var pe = from row in people.AsEnumerable()
                             where row.Field<int>("ID_People") == Int32.Parse(item[1].ToString())
                             select row;
                    DataRow perow = pe.First();
                    var ac = from row in active.AsEnumerable()
                             where row.Field<string>("Title") == item[2].ToString()
                             select row;
                    DataRow perac = ac.First();
                    if (jurnal.TryGetValue(perow[0].ToString(), out a))
                    {
                        int resurs = jurnal[perow[0].ToString()];
                        resurs += Int32.Parse(perac[3].ToString());
                    }
                    else
                    {
                        jurnal[perow[0].ToString()] = Int32.Parse(perac[3].ToString());
                    }
                }
                SQL_State();
                foreach (var item in jurnal)
                {
                    SqlCommand command = new SqlCommand($"exec Activities_Number_Update @ID_People={item.Key}, @Activities_Number = {item.Value}", sqlConn);
                    command.ExecuteNonQuery();
                }
            }
            catch { }
            finally
            {
                SQL_State();
            }
        }

        public static void Authing()
        {
            ResetActiveNumber();
            try
            {
                SQL_State();
                SqlCommand command = new SqlCommand($"exec Auth @ID_People={Settings.Default.Client_ID}", sqlConn);
                command.ExecuteNonQuery();
            }
            catch { }
            finally
            {
                SQL_State();
            }
        }

        public static List<String> TableNames()
        {
            List<String> list = new List<String>();
            try
            {
                SQL_State();
                SqlCommand command = new SqlCommand(return_TableName, sqlConn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader["TABLE_NAME"].ToString());
                }

            }
            catch
            {
            }
            finally
            {
                SQL_State();
            }
            return list;
        }
    }
}

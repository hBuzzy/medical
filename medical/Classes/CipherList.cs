using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace medical.Classes
{

    public class CipherList
    {

        private List<CipherItem> list;
        private MainWindow mainWindow;
        private int counter = 1;

        public CipherList(MainWindow mainWindow)
        {
            list = new List<CipherItem>();
            this.mainWindow = mainWindow;
            //fillList();
            //createTable();
            //worker.RunWorkerAsync();
            //insertInTable();
        }

        public List<CipherItem> List 
        {
            get
            {
                return this.list;
            }
        }

        private void createTable()
        {
            OleDbConnection connection = new OleDbConnection(mainWindow.WorkingPath.Connection);
            try
            {
                connection.Open();
                string strTemp = " [Id] Counter, [Code] Text ";
                OleDbCommand myCommand = new OleDbCommand();
                myCommand.Connection = connection;
                myCommand.CommandText = "CREATE TABLE [CipherList](" + strTemp + ")";
                myCommand.ExecuteNonQuery();
                myCommand.Connection.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        private void insertInTable(object sender, DoWorkEventArgs e)
        {
            OleDbConnection connection = new OleDbConnection(mainWindow.WorkingPath.Connection);
            OleDbDataAdapter adapter;
            
            for (int tableNumber = 1; tableNumber < 7; tableNumber++)
            {
                try
                {
                    DataSet dataSet = new DataSet();
                    connection.Open();

                    //adapter = new OleDbDataAdapter("SELECT id, code, info, parentId, superId, haveChild, " + tableNumber + " as tabId FROM Child" + tableNumber + " WHERE haveChild=false", connection);
                    adapter = new OleDbDataAdapter("SELECT code FROM Child" + tableNumber + " WHERE haveChild=false", connection);
                    OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                    adapter.Fill(dataSet, "Child" + tableNumber);

                    connection.Close();
                    if (dataSet.Tables[0].Rows.Count == 0)
                    {
                        //MessageBox.Show("empty");
                        //return;
                    }
                    else
                    {
                        addRowsToTable(dataSet, sender);

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            }
        }


        private void addRowsToTable(DataSet dataSet, object sender)
        {
            //this.tableItems = new List<TableItem>();
            OleDbConnection connection = new OleDbConnection(mainWindow.WorkingPath.Connection);
            OleDbDataAdapter adapter = new OleDbDataAdapter();

            try
            {
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DataRow dataRow = dataSet.Tables[0].Rows[i];

                    string query = "INSERT INTO CipherList (Id, Code) VALUES ('" + (this.counter++) + "', '" + dataRow.Field<String>(0) + "')";
                    Console.WriteLine("index = " + this.counter);
                    connection.Open();
                    adapter.InsertCommand = new OleDbCommand(query, connection);
                    adapter.InsertCommand.ExecuteNonQuery();
                    connection.Close();
                    (sender as BackgroundWorker).ReportProgress(i);
                    //mainWindow.textBox.Text = counter.ToString();
                    //this.list.Add(new CipherItem(
                    //      (dataRow.Field<Int32>(0)),
                    //    (dataRow.Field<String>(1))
                    //  ));

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public ObservableCollection<CipherItem> getDataFromTable(string text)
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mainWindow.WorkingPath.TemplateDataPath);
            OleDbDataAdapter adapter;
            DataSet dataSet = new DataSet();

            try
            {
                connection.Open();
                adapter = new OleDbDataAdapter("SELECT TOP 10 * FROM (SELECT * FROM CipherList WHERE code LIKE '%" + @text + "%')", connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(dataSet, "CipherList");
                connection.Close();

                if (dataSet.Tables[0].Rows.Count != 0)
                {
                    ObservableCollection<CipherItem> temp = new ObservableCollection<CipherItem>();
                    
                    for (int i =0; i <dataSet.Tables[0].Rows.Count; i++)
                    {
                        DataRow dataRow = dataSet.Tables[0].Rows[i];
                        temp.Add(new CipherItem(dataRow.Field<Int32>("id"), dataRow.Field<string>("code")));
                    }
                    return temp;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
            return null;

        }

        public List<CipherItem> getDataFromTable1(string text)
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mainWindow.WorkingPath.TemplateDataPath);
            OleDbDataAdapter adapter;
            DataSet dataSet = new DataSet();
            
            try
            {
                connection.Open();
                adapter = new OleDbDataAdapter("SELECT TOP 10 * FROM (SELECT * FROM CipherList WHERE code LIKE '%" + @text + "%')", connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(dataSet, "CipherList");
                connection.Close();

                if (dataSet.Tables[0].Rows.Count != 0)
                {
                    List<CipherItem> tempList = dataSet.Tables[0].AsEnumerable()
                            .Select(dataRow => new CipherItem(dataRow.Field<Int32>("id"), dataRow.Field<string>("code"))
                            ).ToList();
                    this.list.Clear();
                    this.list.AddRange(tempList);

                    string st = "";
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        st += tempList[i].Code + "\n";
                    }
                    MessageBox.Show("st = : " + st +"\n");

                    return this.list;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
            return null;

        }

        private void fillList1()
        {
            OleDbConnection connection = new OleDbConnection(mainWindow.WorkingPath.Connection);
            OleDbDataAdapter adapter;
            for (int tableNumber = 1; tableNumber < 7; tableNumber++)
            {
                try
                {
                    DataSet dataSet = new DataSet();
                    connection.Open();

                    //adapter = new OleDbDataAdapter("SELECT id, code, info, parentId, superId, haveChild, " + tableNumber + " as tabId FROM Child" + tableNumber + " WHERE haveChild=false", connection);
                    adapter = new OleDbDataAdapter("SELECT code FROM Child" + tableNumber + " WHERE haveChild=false", connection);
                    OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                    adapter.Fill(dataSet, "Child" + tableNumber);

                    connection.Close();
                    if (dataSet.Tables[0].Rows.Count == 0)
                    {
                        //MessageBox.Show("empty");
                        //return;
                    }
                    else
                    {
                        List<CipherItem> tempList = dataSet.Tables[0].AsEnumerable()
                            .Select(dataRow => new CipherItem(1, dataRow.Field<string>("code"))
                            ).ToList();
                        for (int i = 0; i < tempList.Count; i++)
                        {
                            tempList[i].Id = i + 1;
                        }
                        //MessageBox.Show(st);

                        this.list.AddRange(tempList);
   
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            }
        }
    }
}

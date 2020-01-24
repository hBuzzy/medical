using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Data.OleDb;
using medical.Classes;
using System.Collections.Generic;
using System.Data;
using System;

namespace medical
{
    /// <summary>
    /// Interaction logic for MKBWindow.xaml
    /// </summary>
    public partial class MKBWindow : Window
    {
        OleDbConnection connection;
        OleDbDataAdapter adapter;
        OleDbCommandBuilder cmdBuilder;
        int tableNumber;
        Stack<int> parentStack;
        MainWindow mainWindow;
        public MKBWindow(MainWindow mainWindow)
        {
            connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mainWindow.WorkingPath.TemplateDataPath);
            InitializeComponent();
            parentStack = new Stack<int>();
            tableNumber = 1;
            this.mainWindow = mainWindow;
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fillTable(selectChild(tableNumber, 0));
        }

        private DataSet selectChild(int tableNumber, int parent)
        {
            try
            {
                DataSet dataSet = new DataSet();
                connection.Open();

                adapter = new OleDbDataAdapter("SELECT * FROM Child" + tableNumber + " WHERE ParentId=" + parent, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(dataSet, "Child" + tableNumber);

                connection.Close();
                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    parentStack.Push(parent);
                    return dataSet;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private DataSet backToBarent()
        {
            return null;
        }

        private void fillTable(DataSet dataSet)
        {
            if (dataSet == null)
            {
                return;
            }
            else
            {
                List<Cipher> list = new List<Cipher>();

                
                if (this.tableNumber > 1)
                {
                    list.Add(new Cipher(-10, "...", "", -10, -10, false));
                }
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DataRow dataRow = dataSet.Tables[0].Rows[i];
                    list.Add(new Cipher(
                        (dataRow.Field<Int32>(0)),
                        (dataRow.Field<String>(1)),
                        (dataRow.Field<String>(2)),
                        (dataRow.Field<Int32>(3)),
                        (dataRow.Field<Int32>(4)),
                        (dataRow.Field<Boolean>(5))
                        ));
                }
                dataGrid_Main.ItemsSource = null;
                dataGrid_Main.ItemsSource = list;
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            int selfId = ((Cipher)dataGrid_Main.SelectedItems[0]).Id;


            if (this.tableNumber != 1)
            {
                if (dataGrid_Main.SelectedIndex != 0)
                {
                    DataSet tempTable = selectChild(this.tableNumber + 1, selfId);
                    if (tempTable != null)
                    {
                        this.tableNumber++;
                        fillTable(tempTable);
                    }
                }
                else
                {
                    //MessageBox.Show("table number = " + tableNumber + " parent = " + parentStack.Pop().ToString());
                    this.tableNumber--;
                    parentStack.Pop();
                    DataSet tempTable = selectChild(this.tableNumber, parentStack.Pop());
                    if (tempTable != null)
                    {
                        fillTable(tempTable);
                    }
                }
            }
            else
            {
                DataSet tempTable = selectChild(this.tableNumber + 1, selfId);
                if (tempTable != null)
                {
                    this.tableNumber++;
                    fillTable(tempTable);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.IsOpenMKB = false;
        }




        /// <summary>
        /// /////////////////////////////////////////////////
        /// </summary>

        private bool checkChilds(int tableNumber, int parentId)
        {
            try
            {
                DataSet dataSet = new DataSet();
                OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\database\New folder\TryTODonewMKB\New.mdb");
                connection.Open();

                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Child" + tableNumber + " WHERE ParentId=" + parentId, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(dataSet, "Child" + tableNumber);

                connection.Close();
                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("error");
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        private bool updateDateInChild(int tableNumber, int id, bool value)
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\database\New folder\TryTODonewMKB\New.mdb");
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
            OleDbCommand command = new OleDbCommand();
            OleDbParameter parameter;
            try
            {
                connection.Open();
                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText = "UPDATE Child" + tableNumber + " SET haveChild = " + value + " WHERE id = " + id;
                //MessageBox.Show("UPDATE Child" + tableNumber + " SET haveChild = '" + value + "' WHERE id = " + id);
                dataAdapter.UpdateCommand.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }

        private void btn_do_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\database\New folder\TryTODonewMKB\New.mdb");
                DataSet dataSet = new DataSet();
                connection.Open();
                int tableNumber = 4;
                adapter = new OleDbDataAdapter("SELECT * FROM Child" + tableNumber, connection);
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.Fill(dataSet, "Child" + tableNumber);

                connection.Close();
                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    string st = "";
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        DataRow dataRow = dataSet.Tables[0].Rows[i];
                        bool result = checkChilds(tableNumber + 1, dataRow.Field<Int32>(0));
                       // MessageBox.Show("result = " + result);
                        if (result == true)
                        {
                           // MessageBox.Show("try ypdate");
                            updateDateInChild(tableNumber, dataRow.Field<Int32>(0), result);
                        }

                        st += dataRow.Field<Int32>(0) + " " + dataRow.Field<String>(1) + " " + dataRow.Field<String>(2) + " " + dataRow.Field<Int32>(3) + " " + dataRow.Field<Int32>(4) + " " + dataRow.Field<Boolean>(5) + "\n";
                    }
                    MessageBox.Show(st);
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }*/

            
        }
    }
}

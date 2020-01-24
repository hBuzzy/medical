using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace medical.Classes
{
    public class MainTable : INotifyPropertyChanged
    {
        private int rowNumber;
        List<TableItem> tableItems { get; set; }
        private MainWindow mainWindow;
        OleDbConnection connection;
        public MainTable(MainWindow mainWindow)
        {
            this.rowNumber = 1;
            this.tableItems = new List<TableItem>();
            this.mainWindow = mainWindow;
            mainWindow.txtBox_number.Text = RowNumber.ToString();
        }

        public int RowNumber
        {
            get
            {
                return this.rowNumber;
            }
            set 
            {
                this.rowNumber = value;
                OnPropertyChanged("RowNumber");
            }
        }

        private DataSet selectData1()
        {
            connection = new OleDbConnection(mainWindow.WorkingPath.Connection);
            DataSet dataSet = new DataSet();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT dataId, gender, DateOfBirth, days, Cipher FROM InsertTable", connection);
            OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dataAdapter);
            try
            {
                connection.Open();
                dataAdapter.Fill(dataSet, "InsertTable");
                connection.Close();

                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return dataSet;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private int getPersonAge(DateTime birthDate)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - birthDate.Year;
            if (birthDate > now.AddYears(-age)) age--;
            return age;

        }

        internal void selectData()
        {
            DataSet dataSet = selectData1();

            if (dataSet != null)
            {
                this.tableItems = new List<TableItem>();
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DataRow dataRow = dataSet.Tables[0].Rows[i];
                    this.tableItems.Add(new TableItem(
                            (dataRow.Field<Int32>(0)),
                            (dataRow.Field<String>(1)),
                            (dataRow.Field<DateTime>(2)),
                            (dataRow.Field<Int32>(3)),
                            (dataRow.Field<String>(4))
                            ));
                    RowNumber++;
                }
                mainWindow.txtBox_number.Text = RowNumber.ToString();

            }
            else
            {
                MessageBox.Show("dataset is empty");
            }

        }

        public void updateRow()
        {

        }

        private void insertData()
        {
           // MessageBox.Show(mainWindow.WorkingPath.Connection);
            connection = new OleDbConnection(mainWindow.WorkingPath.Connection);
            OleDbDataAdapter adapter = new OleDbDataAdapter();

            Int32 dataId = Convert.ToInt32(this.rowNumber);
            String gender = (mainWindow.cmbBox_gender.Items[mainWindow.cmbBox_gender.SelectedIndex] as ComboBoxItem).Tag.ToString();
            DateTime dateOfBirth = Convert.ToDateTime(mainWindow.txtBox_birthday.Text);
            Int32 days = Convert.ToInt32(mainWindow.txtBox_numOfDays.Text);
            String cipher = (mainWindow.cmbBox_chipher.Items[mainWindow.cmbBox_chipher.SelectedIndex] as CipherItem).Code;
            //String cipher = (mainWindow.cmbBox_chipher.Items[])
            

            string query = "INSERT INTO InsertTable (dataId, gender, dateOfBirth, days, cipher) VALUES ('" + dataId + "', '" + gender + "', '" + 
                dateOfBirth + "', '" + days + "', '" + cipher + "')";
            try
            {
                connection.Open();
                adapter.InsertCommand = new OleDbCommand(query, connection);
                adapter.InsertCommand.ExecuteNonQuery();
               // MessageBox.Show("ok");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        internal List<TableItem> TableItems
        {
            get { return this.tableItems; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        internal bool AddRow(TableItem item)
        {
            tableItems.Add(item);
            //insertData();
            insertData();
            rowNumber++;
            mainWindow.txtBox_number.Text = RowNumber.ToString();
            mainWindow.cmbBox_gender.SelectedIndex = -1;
            mainWindow.txtBox_numOfDays.Text = "";
            mainWindow.txtBox_birthday.Text = ""; 
            mainWindow.cmbBox_chipher.Text = "";
            mainWindow.cmbBox_chipher.IsDropDownOpen = false;
            
            return true;
        }
    }
}

using medical.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using IO = System.IO;
using medical.Resources;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Reflection;
using System.Data;
using System.Data.OleDb;

namespace medical
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

        //Доделать проверки на все поля
        //Разобраться как создавать acsess файды
        //Разобраться как подключить библиотеку для сводных таблиц


    public partial class MainWindow : Window
    {

        SolidColorBrush toolsBGBrush = new SolidColorBrush(Colors.Red);
        DispatcherTimer timer;
        public OleDbConnection connection { get; set; }
        public MainTable table { get; set; }
        public Settings settings { get; set; }
        private WorkingPath workingPath;
        CipherList cipherList { get; set; }
        public List<CipherItem> cipherItemList { get; set; }
        
        private bool isOpenMKB = false;
        private bool isOpenResultTable = false;
        private bool isOpenSetting = false;
        private bool isChangeMode = false;
        private bool isCipherSelected = false;

        private List<string> testList { get; set; }
        public MainWindow()
        {
            settings = new Settings();
            settings.readSettings();
            testList = new List<string>();
            
            InitializeComponent();

            showInCenter();
            this.DataContext = this;
            timer = new DispatcherTimer();
            workingPath = new WorkingPath();
            cmbBox_chipher.DisplayMemberPath = "Code";

            checkWorkingFolder();
            

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
            
        }

        private void showInCenter()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        7#region Getters and Setters
        public bool IsOpenMKB
        {
            get { return this.isOpenMKB; }
            set { this.isOpenMKB = value; }
        }

        public WorkingPath WorkingPath
        {
            get { return this.workingPath; }
            set { this.workingPath = value; }
        }

        public bool IsOpenResultTable
        {
            get { return this.isOpenResultTable; }
            set { this.isOpenResultTable = value; }
        }

        public bool IsOpenSettings
        {
            get { return this.isOpenSetting; }
            set { this.isOpenSetting = value; }
        }
        #endregion

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            grid_MainTable.Width = this.ActualWidth - 15;
            grid_insertButtons.Width = wrap_insertGrid.ActualWidth;
            line_separator1.X2 = this.ActualWidth;
            line_separator2.X2 = this.ActualWidth;
            if (this.ActualWidth < 730)
            {
                grid_MainTable.Height = this.ActualHeight - 300;
                wrap_insertGrid.Height = 120;
            }
            else
            {
                grid_MainTable.Height = this.ActualHeight - 250;
                wrap_insertGrid.Height = 70;
            }
        }


        private void btn_insertData_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            if (!checkData(txtBox_birthday.Text, txtBox_numOfDays.Text))
            {
                toolsBGBrush.Opacity = 1;
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
                timer.Tick += new EventHandler(time_Tick);
                timer.Start();
                return;
            }


            int number = Convert.ToInt32(txtBox_number.Text);
            string gender = getGender();
            DateTime dateOfBirth = Convert.ToDateTime(txtBox_birthday.Text);
            int numberOfDays = Convert.ToInt32(txtBox_numOfDays.Text);
            string cipher = (cmbBox_chipher.Items[cmbBox_chipher.SelectedIndex] as CipherItem).Code;

            table.AddRow(new TableItem(number, gender, dateOfBirth, numberOfDays, cipher));
            grid_MainTable.ItemsSource = null;
            grid_MainTable.ItemsSource = table.TableItems;
        }




























        private string getGender()
        {
            return (cmbBox_gender.Items[cmbBox_gender.SelectedIndex] as ComboBoxItem).Tag.ToString();
        }

        private void cmbBox_gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show((cmbBox_gender.Items[cmbBox_gender.SelectedIndex] as ComboBoxItem).Tag.ToString());
        }

        private void StackPanel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("dddd");
        }

        private void txtBox_number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        #region checking data functions
        
        public bool checkData(string data, string numberOfDays)
        {
            bool flag = true;
            
            if (!checkDate(data))
            {
                if (flag) { flag = false; }
                txtBox_birthday.Background = toolsBGBrush;
            }

            if (!checkNumOfDays(numberOfDays))
            {
                if (flag) { flag = false; }
                txtBox_numOfDays.Background = toolsBGBrush;
            }

            if (!checkGender())
            {
                if (flag) { flag = false; }
                cmbBox_gender.Background = toolsBGBrush;
            }

            if (!checkChipher())
            {
                if (flag) { flag = false; }
                cmbBox_chipher.Background = toolsBGBrush;
            }

            return flag;

        }

        public bool checkDate(string date)
        {
            try
            {
                Convert.ToDateTime(date);
            }
            catch (Exception exception)
            {
                return false;
            }
            return true;
        }

        private bool checkNumOfDays(string number)
        {
            if (number == "" || number.Length == 0)
            {
                return false;
            }
            return true;
        }

        private bool checkGender()
        {
            if (cmbBox_gender.SelectedIndex == -1)
            {
                return false;
            }
            return true;
        }

        private bool checkChipher()
        {
            if (cmbBox_chipher.SelectedIndex == -1)
            {
                return false;
            }
            return true;
        }

        private void time_Tick(object sender, EventArgs e)
        {
            toolsBGBrush.Opacity -= 0.1;
            if (toolsBGBrush.Opacity < 0.1)
            {
                txtBox_birthday.Background = Brushes.Transparent;
                txtBox_numOfDays.Background = Brushes.Transparent;
                cmbBox_gender.Background = Brushes.Transparent;
                cmbBox_chipher.Background = Brushes.Transparent;
                toolsBGBrush.Opacity = 1;
                timer.Stop();
            }
            
        }

        
        #endregion



        public string getPath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                InitialDirectory = settings.WorkingFolder,
                Title = "Выбор файла проекта",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "mdb",
                Filter = "project file (*.mdb)|*.mdb",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true

            };

            if (fileDialog.ShowDialog() == true)
            {
                return fileDialog.FileName;
            }
            else
            {
                MessageBox.Show("not found");
                return null;
            }
        }

        private bool checkWorkingFolder()
        {
            if (!IO.Directory.Exists(settings.WorkingFolder))
            {
                MessageBox.Show("Программе не удалось найти папку по умолчанию. \nНеобходимо задать новую папку!");
                string path = getPath();

                if (path != null)
                {
                    settings.WorkingFolder = path;
                    settings.save();
                    return true;
                }
                return false;
            }
            return true;
        }    

        #region Menu buttons

        
        
        private bool moveFile()
        {
            try
            {
                IO.File.Copy(workingPath.TemplateDataPath, workingPath.DataFolder + @"\data.mdb", true);                
            }
            catch(Exception ex)
            {

            }
            return false;
        }

        private string createCurrentFolder()
        {
            try
            {
                table = new MainTable(this);
                grid_MainTable.ItemsSource = null;
                grid_MainTable.ItemsSource = table.TableItems;
                string newFolder = DateTime.Now.ToString("dd.MM.yyyy hh.mm.ss");
                string newPath = settings.WorkingFolder + @"\" + newFolder;
                IO.Directory.CreateDirectory(newPath);
                string newDataFolder = newPath + @"\Data";
                //MessageBox.Show("new folder = " + newPath);
                //MessageBox.Show(newDataFolder);
                IO.Directory.CreateDirectory(newDataFolder);

                //name of working folder
                workingPath.Folder = newFolder;
               // MessageBox.Show("folder = " + newFolder);
               //path to data folder
                workingPath.DataFolder = newDataFolder;
                //MessageBox.Show("dataFolder = " + newDataFolder);
                //full path till workingFolder
                workingPath.Path = newPath;
               // MessageBox.Show("path = " + newPath);
               // MessageBox.Show("path till wotking file = " + newDataFolder + "\\" + workingPath.File);
                workingPath.Connection = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + newDataFolder + @"\" + workingPath.File;
                //MessageBox.Show(workingPath.Connectiion);
                moveFile();
                cipherList = new CipherList(this);
                return newFolder;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        private void menuItem_NewProfect_Click(object sender, RoutedEventArgs e)
        {
            if (checkWorkingFolder())
            {
                createCurrentFolder();

                grid_body.Visibility = Visibility.Visible;
                btn_resultTableWindow.Visibility = Visibility.Visible;

            }
        
        }


        private void menuItem_OpenProfect_Click(object sender, RoutedEventArgs e)
        {
            string path = getPath();
            if (path != null)
            {
                table = new MainTable(this);
                //fill table from choosen file
                string[] array = path.Split(@"\");
                string st = "";
                for (int i = 0; i < array.Length; i++)
                {
                    st += array[i] + "\n";
                }
                workingPath.Folder = array[array.Length - 3];

                string temp = "";
                for (int i = 0; i < array.Length - 2; i++)
                {
                    temp += array[i] + @"\";
                }

                workingPath.DataFolder = temp + "Data";
                workingPath.Path = path;
                //MessageBox.Show(st);
                //MessageBox.Show("Folder = " + workingPath.Folder);
                //MessageBox.Show("Data folder = " + workingPath.DataFolder);
                //MessageBox.Show("path = " + workingPath.Path);
                //workingPath.Folder = ;
                workingPath.Connection = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path;
                //MessageBox.Show("connection = " + workingPath.Connection);

                ///

                //table.AddRow(new TableItem(number, gender, dateOfBirth, numberOfDays, chipher));
                
                grid_MainTable.ItemsSource = null;
                table.selectData();

                //MessageBox.Show("size = " + table.TableItems.Count);

                cipherList = new CipherList(this);
                grid_MainTable.ItemsSource = table.TableItems;

                grid_body.Visibility = Visibility.Visible;
                btn_resultTableWindow.Visibility = Visibility.Visible;
            }
        }


        private void menuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            moveFile();
        }

        private void menuItem_SaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
           SettingsWindow sw = new SettingsWindow(this);
           sw.ShowDialog();
        }

        private void MKB_Click(object sender, RoutedEventArgs e)
        {
            if (!this.isOpenMKB)
            {
                this.isOpenMKB = true;
                MKBWindow mkbWindow = new MKBWindow(this);
                mkbWindow.Show();
                
            }
        }

        private void btn_resultTableWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!this.isOpenResultTable)
            {
                this.isOpenResultTable = true;
                ResultTableWindow rsWindow = new ResultTableWindow(this);
                rsWindow.Show();
            }
        }

        private readonly List<string> priList = new List<string>();
        public void fillThatList()
        {
            priList.Clear();
            DataTable dataTable = new DataTable();
           // using (OleDbDataAdapter adapter = new OleDbDataAdapter()
        }

        
        private void btn_saveChanges_Click(object sender, RoutedEventArgs e)
        {
            table.updateRow();
        }

        private void btn_cancelChanges_Click(object sender, RoutedEventArgs e)
        {
            if (this.isChangeMode)
            {
                stack_changes.Visibility = Visibility.Hidden;
                btn_insertData.Visibility = Visibility.Visible;
                txtBox_number.Text = table.RowNumber.ToString();
                cmbBox_gender.SelectedIndex = -1;
                txtBox_numOfDays.Text = "";
                txtBox_birthday.Text = "";
                cmbBox_chipher.Text = "";
                cmbBox_chipher.IsDropDownOpen = false;
                this.isChangeMode = false;
            }
        }

        private void menuItem_change_Click(object sender, RoutedEventArgs e)
        {
            if (!this.isChangeMode)
            {
                btn_insertData.Visibility = Visibility.Hidden;
            }

            stack_changes.Visibility = Visibility.Visible;

            TableItem currentItem = (grid_MainTable.Items[grid_MainTable.SelectedIndex] as TableItem);
            txtBox_number.Text = currentItem.Number.ToString();
            if (currentItem.Gender == "М")
            {
                cmbBox_gender.SelectedIndex = 0;
            }
            else
            {
                cmbBox_gender.SelectedIndex = 1;
            }
            txtBox_birthday.Text = currentItem.DateOfBirth.ToString();
            txtBox_numOfDays.Text = currentItem.DaysNumber.ToString();                      

            this.isChangeMode = true;
        }

        private void menuItem_delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbBox_chipher_TextChanged(object sender, TextChangedEventArgs e)
        {

                        
            if (!this.isCipherSelected)
            {
                cmbBox_chipher.ItemsSource = null;

                string temp = ((ComboBox)sender).Text.ToUpper();
                var list = cipherList.getDataFromTable(cmbBox_chipher.Text);
                if (list != null)
                {
                    var newList = list.Where(x => x.Code.Contains(temp));
                    cipherItemList = newList.ToList();
                    cmbBox_chipher.ItemsSource = cipherItemList;

                    if (cmbBox_chipher.Items.Count == 1)
                    {
                        //cmbBox_chipher.SelectedIndex = 0;

                    }
                }

            }

            if (cmbBox_chipher.Text != "")
            {
                cmbBox_chipher.IsDropDownOpen = true;
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                table.RowNumber++;
                MessageBox.Show(table.RowNumber.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmbBox_chipher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.isCipherSelected = true;
        }

        private void cmbBox_chipher_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.isCipherSelected = false;
        }
    }


    #endregion



}

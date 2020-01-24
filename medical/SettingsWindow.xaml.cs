using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
using System.Globalization;

namespace medical
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
       // public MainTable table { get; set; }

        private bool tableCellTextIsBold = false;
        private bool tableCellTextIsItalic = false;
        private bool tableCellTextIsUnderline = false;

        private Style baseStyle { get; set; }

        private List<TableItem> table { get; set; }
        public PreviewSettings previewSettings { get; set; }
        public SettingsWindow(MainWindow mainWindow)
        {
            
            InitializeComponent();
            table = new List<TableItem>();
            table.Add(new TableItem(1, "М", Convert.ToDateTime("11.12.1995"), 7, "A09"));
            table.Add(new TableItem(1, "М", Convert.ToDateTime("11.12.1995"), 7, "A09"));
            table.Add(new TableItem(1, "М", Convert.ToDateTime("11.12.1995"), 7, "A09"));
            table.Add(new TableItem(1, "М", Convert.ToDateTime("11.12.1995"), 7, "A09"));
            previewSettings = new PreviewSettings();
            dataGrid_ExampleTable.ItemsSource = table;
            this.DataContext = this;
            baseStyle = new Style(typeof(DataGridCell), (FindResource("customCell") as Style));
            
        }

        public enum txtPreperty
        {
            Bold,
            Italic,
            Underline,
            TextAlign
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            showInCenter();
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

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btn_textTableColorPicker_Click(object sender, RoutedEventArgs e)
        {
            colorPopup.IsOpen = true;
        }

        private void btn_acceptColor_Click(object sender, RoutedEventArgs e)
        {
            colorPopup.IsOpen = false;
            btn_textTableColorPicker.Background = new SolidColorBrush(colorPicker.Color);
        }


        public Style newCellStyle(txtPreperty property , Setter addSetter)
        {
            //Style customCell = FindResource("customCell") as Style;
            // MessageBox.Show(customCell.ToString());
            Style style = new Style(typeof(DataGridCell), baseStyle);
            foreach(SetterBase setter in newSetters(property))
            {
                style.Setters.Add(setter);
            }
            style.Setters.Add(addSetter);

            return style;
        }

        
        public SetterBaseCollection newSetters(txtPreperty property)
        {
            SetterBaseCollection setterBases = new SetterBaseCollection();

            foreach (SetterBase setterBase in baseStyle.Setters)
            {
                setterBases.Add(setterBase);
            }

            return setterBases;

        }
        

        private void radio_cellAlign_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rButton = (RadioButton)sender;
            
            switch (rButton.Content.ToString())
            {
                case "Left":
                    previewSettings.TableCellAligment = TextAlignment.Left;                    
                    break;
                case "Right":
                    previewSettings.TableCellAligment = TextAlignment.Right;
                    break;
                case "Center":
                    previewSettings.TableCellAligment = TextAlignment.Center;
                    break;
                default:
                    return;
            }
            Setter setter = new Setter(TextBlock.TextAlignmentProperty, previewSettings.TableCellAligment);
            baseStyle = newCellStyle(txtPreperty.TextAlign, setter);
            dataGrid_ExampleTable.CellStyle = baseStyle;
        }

        private void radio_headerAlign_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void cBox_underline_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            switch (checkBox.Content.ToString())
            {
                case "cell":                    
                    break;
                case "header":
                    break;
                case "global":
                    break;
                default:
                    break;
            }
            MessageBox.Show(checkBox.Content.ToString());
        }

        private void cBox_italic_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            switch (checkBox.Content.ToString())
            {
                case "cell":
                    if (checkBox.IsChecked == true)
                    {
                        previewSettings.TableCellfontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        previewSettings.TableCellfontWeight = FontWeights.Normal;

                    }
                    baseStyle = newCellStyle(txtPreperty.Bold, new Setter(FontWeightProperty, previewSettings.TableCellfontWeight));
                    dataGrid_ExampleTable.CellStyle = baseStyle;
                    break;
                case "header":
                    break;
                case "global":
                    break;
                default:
                    break;
            }
            MessageBox.Show(checkBox.Content.ToString());
        }

        private void cBox_bold_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            switch (checkBox.Content.ToString())
            {
                case "cell":
                    if (checkBox.IsChecked == true)
                    {
                        previewSettings.TableCellfontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        previewSettings.TableCellfontWeight = FontWeights.Normal;
                        
                    }
                    baseStyle = newCellStyle(txtPreperty.Bold, new Setter(FontWeightProperty, previewSettings.TableCellfontWeight));
                    dataGrid_ExampleTable.CellStyle = baseStyle;
                    break;
                case "header":
                    break;
                case "global":
                    break;
                default:
                    break;
            }
            MessageBox.Show(checkBox.Content.ToString());
        }
    }

    
}

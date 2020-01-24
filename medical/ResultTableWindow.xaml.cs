using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace medical
{
    /// <summary>
    /// Interaction logic for ResultTableWindow.xaml
    /// </summary>
    public partial class ResultTableWindow : Window
    {
        MainWindow mainWindow;
        public ResultTableWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.IsOpenResultTable = false;
        }
    }
}

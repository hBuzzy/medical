using medical.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace medical.Classes
{
    public class Settings
    {
        
        public Settings()
        {

        }

        public bool FirtStart
        {
            get { return AppSettings.Default.firstStart; }
            set { AppSettings.Default.firstStart = value; }
        }

        public int FontSize
        {
            get { return AppSettings.Default.fontSize; }
            set { AppSettings.Default.fontSize = value; } 
        }

        public string FontFamily
        {
            get { return AppSettings.Default.fontFamily; }
            set { AppSettings.Default.fontFamily = value; }
        }

        public string WorkingFolder
        {
            get { return AppSettings.Default.workingFolder; }
            set { AppSettings.Default.workingFolder = value; }
        }

        public string ToolColor
        {
            get { return AppSettings.Default.toolsColor; }
            set { AppSettings.Default.toolsColor = value; }
        }

        public bool readSettings()
        {
            try
            {                                                       

            } catch(Exception ex)
            {
                return false;
            }

           // this.window.txtBox_number.FontSize = FontSize;
            return true;
        }

        public bool save()
        {
            AppSettings.Default.Save();
            return false;
        }

       

    }
}

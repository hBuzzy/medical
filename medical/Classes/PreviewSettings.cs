using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace medical.Classes
{
    public class PreviewSettings : INotifyPropertyChanged
    {
        private string tableFontFamily;
        private string tableCellAlign;
        private TextAlignment tableCellAlignment;
        private FontWeight tableCellFontWeight;
        private FontStyle tableCellFontStyle;
        public PreviewSettings()
        {
            loadSettings();
        }

        private void loadSettings()
        {
        }

        public TextAlignment TableCellAligment
        {
            get { return this.tableCellAlignment; }
            set
            {
                this.tableCellAlignment = value;
                OnPropertyChanged(nameof(TableCellAligment));
            }
        }

        public FontWeight TableCellfontWeight
        {
            get { return this.tableCellFontWeight; }
            set
            {
                this.tableCellFontWeight = value;
                OnPropertyChanged(nameof(TableCellfontWeight));
            }
        }

        public FontStyle TableCellFontStyle
        {
            get { return this.tableCellFontStyle; }
            set
            {
                this.tableCellFontStyle = value;
                OnPropertyChanged(nameof(TableCellFontStyle));
            }
        }


        public string TableFontFamily
        {
            get { return this.tableFontFamily; }
            set
            {
                this.tableFontFamily = value;
                OnPropertyChanged(nameof(TableFontFamily));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

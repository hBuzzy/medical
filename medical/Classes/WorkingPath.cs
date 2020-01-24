using System;
using System.Collections.Generic;
using System.Text;

namespace medical.Classes
{
    public class WorkingPath
    {
        private string path;
        private string folder;
        private string dataFolder;
        private string file;
        private string templateDataPath;
        private string connection;
        public WorkingPath()
        {
            templateDataPath = @"File template\template.mdb";
            file = "data.mdb";
        }

        public string File
        {
            get { return this.file; }
            set { this.file = value; }
        }

        public string Connection
        {
            get { return this.connection; }
            set { this.connection = value; }
        }

        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        public string Folder
        {
            get { return this.folder; }
            set { this.folder = value; }
        }

        public string DataFolder
        {
            get { return this.dataFolder; }
            set { this.dataFolder = value; }
        }

        public string TemplateDataPath
        {
            get { return this.templateDataPath; }
        }
    }
}

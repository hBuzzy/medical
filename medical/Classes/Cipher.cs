using System;
using System.Collections.Generic;
using System.Text;

namespace medical.Classes
{
    
    class Cipher
    {
        private int id;
        private string code;
        private string info;
        private int parentId;
        private int superId;
        private bool haveChild;
        public Cipher(int id, string code, string info, int parentId, int superId, bool haveChild)
        {
            this.id = id;
            this.code = code;
            this.info = info;
            this.parentId = parentId;
            this.superId = superId;
            this.haveChild = haveChild;
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        public string Info
        {
            get { return this.info; }
            set { this.info = value; }
        }

        public int ParentId
        {
            get { return this.parentId; }
            set { this.parentId = value; }
        }

        public int SuperId
        {
            get { return this.superId; }
            set { this.superId = value; }
        }

        public bool HaveChild
        {
            get { return this.haveChild; }
            set { this.haveChild = value; }
        }
    }
}

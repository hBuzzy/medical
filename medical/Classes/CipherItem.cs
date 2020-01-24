using System;
using System.Collections.Generic;
using System.Text;

namespace medical.Classes
{
    public class CipherItem
    {
        private string code;
        private int id;
        public CipherItem()
        {

        }
        public CipherItem(int id, string code)
        {
            this.id = id;
            this.code = code;
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

    }
}

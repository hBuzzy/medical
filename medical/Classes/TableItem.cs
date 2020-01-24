using System;
using System.Collections.Generic;
using System.Text;

namespace medical.Classes
{
    class TableItem
    {
        private int number;
        private string gender;
        private DateTime dateOfBirth;
        private int daysNumber;
        private string diagnosisСipher;
        public TableItem(int number, string gender, DateTime dateOfBirth, int daysNumber, string diagnosisСipher)
        {
            this.number = number;
            this.gender = gender;
            this.dateOfBirth = dateOfBirth;
            this.daysNumber = daysNumber;
            this.diagnosisСipher = diagnosisСipher;
        }

        public int Number
        {
            get { return this.number; }
            set { this.number = value; }
        }

        public string Gender
        {
            get { return this.gender; }
            set { this.gender = value; }
        }

        public DateTime DateOfBirth
        {
            get { return this.dateOfBirth; }
            set { this.dateOfBirth = value; }
        }

        public int DaysNumber
        {
            get { return this.daysNumber; }
            set { this.daysNumber = value; }
        }

        public string DiagnosisChipher
        {
            get { return this.diagnosisСipher; }
            set { this.diagnosisСipher = value; }
        }
    }
}

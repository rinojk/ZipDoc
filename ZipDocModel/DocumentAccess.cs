using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipDocModel
{
    public class DocumentAccess : Document
    {
        private string _username;
        private string _passWord;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string PassWord
        {
            get { return _passWord; }
            set { _passWord = value; }
        }
    }
}

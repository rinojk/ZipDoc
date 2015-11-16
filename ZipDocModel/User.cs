using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipDocModel
{
    public class User
    {
        private string _username;
        private string _passwordHash;
        private string _token;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string PasswordHash
        {
            get { return _passwordHash; }
            set { _passwordHash = value; }
        }

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.UI.WebControls;
using ZipDocDAL;
using ZipDocModel;

namespace ZipDocWeb.Controllers
{
    public class UserController : ApiController
    {
        private string _connectionString = "Database=ZipDocDB;Data Source=us-cdbr-azure-central-a.cloudapp.net;User Id=be3e0d0ebc3029;Password=7507ce92";
        Connection _conn;

        [HttpGet]
        public string Login([FromUri] string username, [FromUri] string password)
        {
            string result = string.Empty;
            _conn = new Connection(_connectionString);
            _conn.OpenConnection();
            result = _conn.Login(username, password);
            _conn.CloseConnection();
            return result;
        }

        [HttpGet]
        public bool Register([FromUri] string username, [FromUri] string password)
        {
            _conn = new Connection(_connectionString);
            _conn.OpenConnection();

            //Calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(username+password+DateTime.Now);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder tokenString = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                tokenString.Append(hash[i].ToString("X2"));
            }

            bool result = _conn.Register(username, password, tokenString.ToString());
            _conn.CloseConnection();
            return result;
        }

        [HttpGet]
        public IEnumerable<Document> GetAllDocuments(string token)
        {
            _conn = new Connection(_connectionString);
            try
            {
                _conn.OpenConnection();
                if (_conn.CheckUserByToken(token))
                    return _conn.GetAllDocuments();
                else
                {
                    return null;
                }
            }
            finally
            {
                _conn.CloseConnection();
            }
        }

        [HttpGet]
        public IEnumerable<Document> SearchDocuments(string token, string pattern)
        {
            _conn = new Connection(_connectionString);try
            {
                _conn.OpenConnection();
                if (_conn.CheckUserByToken(token))
                    return _conn.SearchDocuments(pattern);
                else{
                    return null;
                }
            }
            finally
            {
                _conn.CloseConnection();
            }
        }

        

    }



}
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using ZipDocModel;
using ZipDocDAL;

namespace ZipDoc.Web.Controllers.WebApi
{
    public class UserController : ApiController
    {
        private string _connectionString = "Database=ZipDocDB;Data Source=us-cdbr-azure-central-a.cloudapp.net;User Id=be3e0d0ebc3029;Password=7507ce92";
        Connection _conn;

        [HttpPost]
        public User Login([FromBody] User user)
        {
            _conn = new Connection(_connectionString);
            try
            {
                _conn.OpenConnection();
                string token = _conn.Login(user.Username, user.PasswordHash);
                if (!string.IsNullOrEmpty(token))
                {
                    user.Token = token;
                    return user;
                }
                else
                {
                    throw new Exception("Error login");
                }
            }
            finally
            {
                _conn.CloseConnection();
            }
        }

        [HttpPost]
        public User Register([FromBody] User user)
        {
            _conn = new Connection(_connectionString);
            _conn.OpenConnection();

            //Calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(user.Username + user.PasswordHash + DateTime.Now);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder tokenString = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                tokenString.Append(hash[i].ToString("X2"));
            }

            bool result = _conn.Register(user.Username, user.PasswordHash, tokenString.ToString());
            _conn.CloseConnection();
            if (result)
            {
                user.Token = tokenString.ToString();
                return user;
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Conflict));
            }
        }

        [HttpPost]
        public User PostTest([FromBody] User newUser)
        {
            newUser.Username = "OLOLOSHA";
            return newUser;
        }
    }
}

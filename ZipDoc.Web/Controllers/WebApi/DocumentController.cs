using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using ZipDocDAL;
using ZipDocModel;

namespace ZipDoc.Web.Controllers.WebApi
{
    public class DocumentController : ApiController
    {
        private string _connectionString =
           "Database=ZipDocDB;Data Source=us-cdbr-azure-central-a.cloudapp.net;User Id=be3e0d0ebc3029;Password=7507ce92";

        NetworkCredential _accessCredential = new NetworkCredential("123", "111");
        string _ftpUri = "ftp://192.168.1.38/Docs/";
        Connection _conn;

        [HttpGet]
        public IEnumerable<Document> GetAllDocuments(string token)
        {
            _conn = new Connection(_connectionString);
            try
            {
                _conn.OpenConnection();
                return !string.IsNullOrEmpty(_conn.GetUsernameByToken(token)) ? _conn.GetAllDocuments() : null;
            }
            finally
            {
                _conn.CloseConnection();
            }
        }

        [HttpGet]
        public IEnumerable<Document> SearchDocuments(string token, string pattern)
        {
            _conn = new Connection(_connectionString);
            try
            {
                _conn.OpenConnection();
                return !string.IsNullOrEmpty(_conn.GetUsernameByToken(token)) ? _conn.SearchDocuments(pattern) : null;
            }
            finally
            {
                _conn.CloseConnection();
            }
        }

        [HttpGet]
        public Document UploadDocument(string token, string name, long size, string description)
        {
            _conn = new Connection(_connectionString);
            _conn.OpenConnection();
            string username = _conn.GetUsernameByToken(token);
            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    Document doc = new Document()
                    {
                        Name = name,
                        Size = size,
                        Author = username,
                        Path = _ftpUri + name,
                        Description = description
                    };
                    return _conn.UploadDocument(doc) ? doc : null;
                }
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
        public NetworkCredential GetFtpCredential([FromUri] string token)
        {
            _conn = new Connection(_connectionString);
            try
            {
                _conn.OpenConnection();
                string username = _conn.GetUsernameByToken(token);
                return !string.IsNullOrEmpty(username) ? _accessCredential : null;
            }
            finally
            {
                _conn.CloseConnection();
            }

        }

        [HttpGet]
        public bool Delete([FromUri] string token, [FromUri] long id)
        {
            _conn = new Connection(_connectionString);
            try
            {
                _conn.OpenConnection();
                return !string.IsNullOrEmpty(_conn.GetUsernameByToken(token)) && _conn.DeleteDocument(id);
            }
            finally
            {
                _conn.CloseConnection();
            }
        }
    }
}

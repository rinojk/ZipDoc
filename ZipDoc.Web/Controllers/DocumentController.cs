using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ZipDocDAL;
using ZipDocModel;

namespace ZipDoc.Web.Controllers
{
    public class DocumentController : ApiController
    {
        private string _connectionString =
            "Database=ZipDocDB;Data Source=us-cdbr-azure-central-a.cloudapp.net;User Id=be3e0d0ebc3029;Password=7507ce92";

        NetworkCredential _accessCredential = new NetworkCredential("a8320691", "1111");
        string _ftpUri = "ftp://compresser.site40.net/public_html/";
        Connection _conn;

        [HttpGet]
        public IEnumerable<Document> GetAllDocuments(string token)
        {
            _conn = new Connection(_connectionString);
            try
            {
                _conn.OpenConnection();
                if (!string.IsNullOrEmpty(_conn.GetUsernameByToken(token)))
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
            _conn = new Connection(_connectionString);
            try
            {
                _conn.OpenConnection();
                if (!string.IsNullOrEmpty(_conn.GetUsernameByToken(token)))
                    return _conn.SearchDocuments(pattern);
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
                    if (_conn.UploadDocument(doc))
                    {
                        return doc;
                    }
                    else
                    {
                        return null;
                    }
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
                if (!string.IsNullOrEmpty(username))
                    return _accessCredential;
                else return null;
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
                if (!string.IsNullOrEmpty(_conn.GetUsernameByToken(token)) && _conn.DeleteDocument(id))
                    return true;
                else return false;
            }
            finally
            {
                _conn.CloseConnection();}
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ZipDocModel;

namespace ZipDocDAL
{
    public class Connection
    {
        public Connection(string connString)
        {
            if (!string.IsNullOrEmpty(connString))
                _connString = connString;
        }

        private string _connString;
        MySqlConnection _connection;
        

        public bool OpenConnection()
        {
            _connection = new MySqlConnection(_connString);
            try
            {
                _connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void CloseConnection()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public string Login(string username, string password)
        {
            List<string> result = new List<string>();

            var command = _connection.CreateCommand();
            /*command.CommandText = "SELECT token FROM users WHERE (username='@userName' and pass='@password')";
            command.Parameters.Add(new MySqlParameter()
            {
                DbType = DbType.AnsiString,
                ParameterName = "@userName"
            }).Value = username;
            command.Parameters.Add(new MySqlParameter()
            {
                DbType = DbType.AnsiString, ParameterName = "@password"
            }).Value=password;*/

            command.CommandText = "SELECT token FROM users WHERE (username=?userName and pass=?password)";
            command.Parameters.Add("?userName", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("?password", MySqlDbType.VarChar).Value = password;

            try
            {
                MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    result.Add(dataReader["token"].ToString());
                }
                if (result.Count > 1)
                {
                    throw new Exception("Not 1 token received!");
                }
            }
            finally
            {
                CloseConnection();
            }

            return result.FirstOrDefault();
        }

        public bool Register(string username, string password, string token)
        {
            //string query = $"SELECT * FROM users WHERE (username='{username}')";
            MySqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM users WHERE (username=?username)";
            command.Parameters.Add("?username", MySqlDbType.VarChar).Value = username;

            MySqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.HasRows) return false;
            dataReader.Close();
            dataReader.Dispose();
            command.Dispose();
            try
            {
                command = _connection.CreateCommand();
                command.CommandText =
                    "INSERT INTO users (username, pass, token) VALUES (?username, ?password, ?token)";
                command.Parameters.Add("?username", MySqlDbType.VarChar).Value = username;
                command.Parameters.Add("?password", MySqlDbType.VarChar).Value = password;
                command.Parameters.Add("?token", MySqlDbType.VarChar).Value = token;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Document> GetAllDocuments()
        {
            string query = "SELECT * FROM documents";
            MySqlCommand command = new MySqlCommand(query, _connection);
            
            List<Document> documents = new List<Document>();

            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                documents.Add(new Document
                {
                    Id = Int64.Parse(dataReader["id"].ToString()),
                    Name = dataReader["docname"].ToString(),
                    Size = Int64.Parse(dataReader["size"].ToString()),
                    Path = dataReader["path"].ToString(),
                    Author = dataReader["author"].ToString()
                });
            }
            dataReader.Close();
            dataReader.Dispose();
            return documents;
        }

        public IEnumerable<Document> SearchDocuments(string pattern)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT * FROM documents WHERE docname LIKE ?pattern";
            command.Parameters.Add("?pattern", MySqlDbType.VarChar).Value = pattern;
            List<Document> documents = new List<Document>();
            
            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                documents.Add(new Document
                {
                    Id = Int64.Parse(dataReader["id"].ToString()),
                    Name = dataReader["docname"].ToString(),
                    Size = Int64.Parse(dataReader["size"].ToString()),
                    Path = dataReader["path"].ToString(),
                    Author = dataReader["author"].ToString()
                });
            }
            return documents;
        }

        public bool UploadDocument(Document doc)
        {
            MySqlCommand command = _connection.CreateCommand();

            command.CommandText = "SELECT * FROM documents WHERE (docname = ?docname)";
            command.Parameters.Add("?docname", MySqlDbType.VarChar).Value = doc.Name;
            using (MySqlDataReader dataReader = command.ExecuteReader())
            {
                if (dataReader.HasRows)
                    return false;
                dataReader.Close();
            }
            command.Dispose();
            command = _connection.CreateCommand();

            command.CommandText = "INSERT INTO documents (docname, size, path, author, description) VALUES (?docname, ?size, ?path, ?author, ?description)";
            command.Parameters.Add("?docname", MySqlDbType.VarChar).Value = doc.Name;
            command.Parameters.Add("?size", MySqlDbType.Double).Value = doc.Size;
            command.Parameters.Add("?path", MySqlDbType.Text).Value = doc.Path;
            command.Parameters.Add("?author", MySqlDbType.VarChar).Value = doc.Author;
            command.Parameters.Add("?description", MySqlDbType.VarChar).Value = doc.Description;

            command.ExecuteNonQuery();
            return true;
        }

        public bool DeleteDocument(long id)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM documents WHERE id=?id";
            command.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
            command.ExecuteNonQuery();
            return true;
        }

        public string GetUsernameByToken(string token)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT username FROM users WHERE token=?token";
            command.Parameters.Add("?token", MySqlDbType.VarChar).Value = token;
            using (MySqlDataReader dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    return dataReader["username"].ToString();
                }
            }
            command.Dispose();
            return string.Empty;
        }
    }
}

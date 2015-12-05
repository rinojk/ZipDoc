using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class DALTest
    {

        [TestMethod]
        public void OpenConnTest()
        {
            string _connString = "Database=ZipDocDB;Data Source=us-cdbr-azure-central-a.cloudapp.net;User Id=be3e0d0ebc3029;Password=7507ce92";
            var conn = new ZipDocDAL.Connection(_connString);
            var result = conn.OpenConnection();
            conn.CloseConnection();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void LoginTest()
        {
            string username = "testeruser";
            string password = "f1c1592588411002af340cbaedd6fc33";

            string _connString = "Database=ZipDocDB;Data Source=us-cdbr-azure-central-a.cloudapp.net;User Id=be3e0d0ebc3029;Password=7507ce92";
            var conn = new ZipDocDAL.Connection(_connString);
            conn.OpenConnection();
            var result = conn.Login(username, password);
            conn.CloseConnection();
            Assert.AreEqual("token2token", result);
        }

        [TestMethod]
        public void GetUsernameByTokenTest()
        {
            string token = "token2token";
            string _connString = "Database=ZipDocDB;Data Source=us-cdbr-azure-central-a.cloudapp.net;User Id=be3e0d0ebc3029;Password=7507ce92";
            var conn = new ZipDocDAL.Connection(_connString);
            conn.OpenConnection();
            var result = conn.GetUsernameByToken(token);
            conn.CloseConnection();
            Assert.AreEqual("testeruser", result);
        }
    }
}

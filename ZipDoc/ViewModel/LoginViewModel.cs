using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ArxOne.Ftp;
using DevExpress.Xpf.Core.Native;
using Limilabs.FTP.Client;
using ZipDoc.View;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using ZipDoc.Annotations;
using ZipDocDAL;
using ZipDocModel;
using FtpTransferMode = ArxOne.Ftp.FtpTransferMode;

namespace ZipDoc.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public LoginViewModel()
        {
            LoginClickCommand = new Command(arg => LoginClick());
            RegisterClickCommand = new Command(arg => RegisterClick());

            TestClickCommand = new Command(arg => TestClick());
            User = new User();
        }

        #region Fields
        private User _user;
        private bool _rememberMe;
        private string _password;
        private string _apiUri = Properties.Settings.Default.ServerUri;
        #endregion

        #region Commands

        public ICommand LoginClickCommand { get; set; }
        public ICommand TestClickCommand { get; set; }

        public ICommand RegisterClickCommand { get; set; }

        #endregion

        #region Properties
        public User User
        {
            get { return _user; }
            set
            {
                _user = value; 
                OnPropertyChanged(nameof(User));
            }
        }

        public bool RememberMe
        {
            get { return _rememberMe; }
            set
            {
                _rememberMe = value; 
                OnPropertyChanged(nameof(RememberMe));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value; 
                OnPropertyChanged(nameof(Password));
            }
        }
        #endregion


        #region Methods

        private async void LoginClick()
        {
            //_user.PasswordHash = GetMD5(_password);
            using (HttpClient client = new HttpClient()){
                string uri = $"{_apiUri}api/User/login";
                if (string.IsNullOrEmpty(_password)||string.IsNullOrEmpty(_user.Username))
                {
                    MessageBox.Show("Empty Field", "Error");
                    return;
                }

                _user.PasswordHash = GetMD5(_password);
                HttpContent content = new StringContent(JsonConvert.SerializeObject(_user), Encoding.UTF8, "application/json");
                var response = client.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    //MessageBox.Show("All OK");
                    string result = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(result);
                    if (_rememberMe)
                    {
                        Properties.Settings.Default.Token = user.Token;
                    }
                    //Opening main window
                    var mainVM = new MainWindowView()
                    {
                        DataContext = new MainWindowViewModel(_user)};
                    mainVM.Show();
                    Application.Current.MainWindow.Close();
                }
                else
                {
                    MessageBox.Show("Error with authorisation!");
                }
            }
        }

        private async void RegisterClick()
        {
            using (HttpClient client = new HttpClient())
            {
                _user.PasswordHash = GetMD5(Password);
                string uri = $"{_apiUri}/api/User/register";
                if (string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_user.Username))
                {
                    MessageBox.Show("Empty Field", "Error");
                    return;
                }
                
                HttpContent content = new StringContent(JsonConvert.SerializeObject(_user), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(result);
                    if (_rememberMe)
                    {
                        Properties.Settings.Default.Token = user.Token;
                    }
                    //Opening main window
                    var mainVM = new MainWindowView()
                    {
                        DataContext = new MainWindowViewModel(_user)
                    };
                    mainVM.Show();

                    Application.Current.MainWindow.Close();
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    MessageBox.Show("User with this username already created or connection failed!");
                }
            }
        }

        private string GetMD5(string sourcePW)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(sourcePW);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private async void TestClick()
        {
            using (HttpClient client = new HttpClient())
            {
                var user = new User()
                {
                    Username = "testUsername",
                    PasswordHash = "hash2hash",
                    Token = "890"
                };
                var response =
                    client.PostAsync("http://localhost:13790/api/User/PostTest",
                        new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    User us = JsonConvert.DeserializeObject<User>(res);
                    MessageBox.Show(us.Username);

                    
                }
            }
        }

        private void TRUEFtpDownload()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://compresser.site40.net/public_html/timc.ppsx");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.KeepAlive = true;
            request.UsePassive = true;
            request.UseBinary = true;
            request.Credentials = new NetworkCredential("a8320691", "1111");
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            using (FileStream writer = new FileStream(@"F:\FileName.ppsx", FileMode.Create))
            {

                long length = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[2048];

                readCount = responseStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    writer.Write(buffer, 0, readCount);
                    readCount = responseStream.Read(buffer, 0, bufferSize);
                }
            }

            reader.Close();
            response.Close();
        }
        private void DownloadFileFTP()
        {
            string inputfilepath = @"F:\FileName.ppsx";
            string ftphost = "compresser.site40.net";
            string ftpfilepath = "/public_html/t.ppsx";

            string ftpfullpath = "ftp://" + ftphost + ftpfilepath;

            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("a8320691", "1111");
                byte[] fileData = request.DownloadData(ftpfullpath);

                using (FileStream file = File.Create(inputfilepath))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
                MessageBox.Show("Download Complete");
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

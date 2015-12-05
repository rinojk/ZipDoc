using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using ZipDocModel;

namespace ZipDoc.Register
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Random rand = new Random();
            for (int i = 0; i < 13; i++)
            {
                pwTextBox.Text += (char) rand.Next(65, 90);
            }
        }

        private string _apiUri = Properties.Settings.Default.ServerUri;

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginTextBox.Text) || string.IsNullOrEmpty(pwTextBox.Text))
            {
                MessageBox.Show("Fill all fields");
                return;
            }

            string login = loginTextBox.Text;
            string password = pwTextBox.Text;
            User user = new User
            {
                Username = login, PasswordHash = GetMD5(password)
            };

            using (HttpClient client = new HttpClient())
            {
                string uri = $"{_apiUri}/api/User/register";

                HttpContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"User {user.Username} Registered Successfully", "Registered");
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    MessageBox.Show("User with this username already created or connection failed!");
                }
                else
                {
                    MessageBox.Show("Some error occured!");
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
    }
}

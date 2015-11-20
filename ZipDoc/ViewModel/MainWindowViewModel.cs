using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using ZipDoc.Annotations;
using ZipDocModel;

namespace ZipDoc.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel(User user)
        {
            InitializeCommands();
            _user = user;
            /*User = user;
            //MessageBox.Show(_user.Username);
            */
            /*Docs.Add(new Document
            {
                Name = "azaza", Id = 100500
            });
            Docs.Add(new Document
            {
                Name = "Asdsa", Id = 65196849
            });*/
            ShowAllDocs();
        }

        public MainWindowViewModel()
        {
            InitializeCommands();
            _user= new User();
            _user.Token = ZipDoc.Properties.Settings.Default.Token;
            ShowAllDocs();
        }

        #region Fields

        private User _user;
        private ObservableCollection<Document> _docs = new ObservableCollection<Document>();
        private ObservableCollection<Document> _allDocuments = new ObservableCollection<Document>();
        private readonly string _serverUri = ZipDoc.Properties.Settings.Default.ServerUri;
        private string _searchPattern;
        private Document _selctd;
        private double _currentProgressValue;

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

        public ObservableCollection<Document> Docs
        {
            get { return _docs; }
            set
            {
                _docs = value;
                OnPropertyChanged(nameof(Docs));
            }
        }

        public string SearchPattern
        {
            get { return _searchPattern; }
            set
            {
                _searchPattern = value;
                OnPropertyChanged(nameof(SearchPattern));
                Search();
            }
        }

        public Document Selctd
        {
            get { return _selctd; }
            set
            {
                _selctd = value; 
                OnPropertyChanged(nameof(Selctd));
            }
        }

        public double CurrentProgressValue
        {
            get { return _currentProgressValue; }
            set
            {
                _currentProgressValue = value; 
                OnPropertyChanged(nameof(CurrentProgressValue));
            }
        }

        #endregion

        #region Commands

        public ICommand ClickCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand DownloadCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand UploadCommand { get; set; }

        
        #endregion

        #region Methods

        private void InitializeCommands()
        {
            ClickCommand = new Command(arg => ClickMethod());
            SearchCommand = new Command(arg => Search());
            ExitCommand = new Command(arg => ExitClick());
            DownloadCommand = new Command(arg => DownloadClick());
            RefreshCommand = new Command(arg => RefreshClick());
            UploadCommand = new Command(arg => UploadClick());
        }
        private void Search()
        {
            var result = from doc in _allDocuments where doc.Name.ToLower().IndexOf(_searchPattern.ToLower())!=-1 select doc;
            Docs = new ObservableCollection<Document>(result);
        }
        private void ClickMethod()
        {
            MessageBox.Show("All OK");
        }
        private async void ShowAllDocs()
        {
            using (WebClient client = new WebClient())
            {
                string uri = $"{_serverUri}/api/Document/GetAllDocuments?token={_user.Token}";
                string answer = await client.DownloadStringTaskAsync(uri);
                _allDocuments = JsonConvert.DeserializeObject<ObservableCollection<Document>>(answer);
                Docs = _allDocuments;
            }}
        private void DownloadClick()
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
        private void ExitClick()
        {
            Environment.Exit(1);
        }
        private void RefreshClick()
        {
            ShowAllDocs();
        }
        private async void UploadClick()
        {
            NetworkCredential credential;
            string ftpPath;
            string filePath;
            using (WebClient client = new WebClient())
            {
                string answer = await client.DownloadStringTaskAsync($"{_serverUri}/api/Document/GetFtpCredential?token={User.Token}");
                credential = JsonConvert.DeserializeObject<NetworkCredential>(answer);
                OpenFileDialog openFileDialog = new OpenFileDialog();
                bool? userClickedOK = openFileDialog.ShowDialog();
                if (userClickedOK == true && !string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    filePath = openFileDialog.FileName;
                    var docToSend = new Document();
                    //docToSend.Author = User.Username;
                    //docToSend.Description =
                    docToSend.Name = Path.GetFileName(openFileDialog.FileName);
                    docToSend.Size = new FileInfo(openFileDialog.FileName).Length;
                    answer =
                        await
                            client.DownloadStringTaskAsync(
                                $"{_serverUri}/api/Document/Uploaddocument?token={User.Token}&name={docToSend.Name}&description={docToSend.Description}&size={docToSend.Size}");
                }
                else
                {
                    MessageBox.Show("Choose file");
                    return;
                }
                try
                {
                    Document answerDocument = JsonConvert.DeserializeObject<Document>(answer);
                    ftpPath = answerDocument.Path;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending file to server! \nError message:\n"+ex.Message);
                    return;
                }
            }

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);

            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = credential;

            // Copy the contents of the file to the request stream.
            StreamReader sourceStream = new StreamReader(filePath);
            byte[] fileContents = File.ReadAllBytes(filePath);
            sourceStream.Close();
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();
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

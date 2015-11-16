using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZipDoc.Annotations;
using ZipDocModel;

namespace ZipDoc.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            ClickCommand = new Command(arg => ClickMethod());
            /*User = user;
            //MessageBox.Show(_user.Username);
            */
            Docs.Add(new Document
            {
                Name = "azaza", Id = 100500
            });
            Docs.Add(new Document
            {
                Name = "Asdsa", Id = 65196849
            });
        }

        
        #region Fields

        private User _user;
        private ObservableCollection<Document> _docs = new ObservableCollection<Document>();

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

        #endregion

        #region Commands

        public ICommand ClickCommand { get; set; }

        #endregion

        #region Methods

        private void ClickMethod()
        {
            MessageBox.Show("All OK");
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

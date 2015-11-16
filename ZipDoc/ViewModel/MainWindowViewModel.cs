using System;
using System.Collections.Generic;
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
        public MainWindowViewModel(User user)
        {
            ClickCommand = new Command(arg => ClickMethod());
            User = user;
            MessageBox.Show(_user.Username);
        }

        #region Fields

        private User _user;

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

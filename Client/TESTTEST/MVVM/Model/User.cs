using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TESTTEST.MVVM.Model
{
    public class User : BaseModel , INotifyPropertyChanged
    {
        private string name;
        private ObservableCollection<ToDoTask> tasks;
        private ObservableCollection<TaskGroup> groups;
        private Account account;
        private int accountId;
        private DateTime registeredAt;
     [Required]
        public string Name {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public ObservableCollection<ToDoTask> Tasks
        {
            get
            {
                return tasks;
            }
            set
            {
                tasks = value;
                OnPropertyChanged("Tasks");
            }
        }
        public ObservableCollection<TaskGroup> Groups 
        {
            get
            {
                return groups;
            }
            set
            {
                groups = value;
                OnPropertyChanged("Groups");
            }
        }
        public Account Account {
            get
            {
                return account;
            }
            set
            {
                account = value;
                OnPropertyChanged("Account");
            }
        }
        public int AccountId 
        {
            get { 
                return accountId; 
            }
            set
            {
                accountId = value;
                OnPropertyChanged("AccountId");
            }
        }
            
        [Required]
        public DateTime RegisteredAt 
        {
            get 
            {
              return registeredAt; 
            }
            set
            {
                registeredAt = value;
                OnPropertyChanged("RegisteredAt");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

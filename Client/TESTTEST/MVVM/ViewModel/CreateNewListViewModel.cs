using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TESTTEST.Core;
using TESTTEST.MVVM.Model;
using TESTTEST.MVVM.View;

namespace TESTTEST.MVVM.ViewModel
{
   public class CreateNewListViewModel : INotifyPropertyChanged
    {
        public TaskListEditWindow EditTaskListWindow;
        public User User { get; set; }
        public TaskGroup NewTaskGroup { get;  set; }
        private ToDoTask selectedtask;
        private ToDoTask selectedtaskincreatedlist;
        private RelayCommand addtoListCommand;
        private RelayCommand removefromListCommand;
        public RelayCommand RemoveFromListCommand
        {
            get
            {
                return removefromListCommand ?? (removefromListCommand = new RelayCommand((o) =>
                {
                    
                    NewTaskGroup.Tasks.Remove(SelectedTaskIncreatedlist);  
                    
                }, (o) => NewTaskGroup.Tasks.Count >= 0
                ));
            }
        }
        public RelayCommand AddToListCommand
        {
            get
            {
                return addtoListCommand ?? 
                    (addtoListCommand = new RelayCommand((o) =>
                {

                    if (NewTaskGroup.Tasks.FirstOrDefault(item => item.Id == SelectedTask.Id) != null)
                    {

                    }
                    else
                    {
                        NewTaskGroup.Tasks.Add(SelectedTask);
                    }
                    
                }
                 ));
            }
        }

        public CreateNewListViewModel(User user, TaskGroup NewTaskGroup)
        {
            User = user;
            this.NewTaskGroup = NewTaskGroup;
        }
        public CreateNewListViewModel(TaskListEditWindow editTaskListWindow,User user)
        {
            EditTaskListWindow = editTaskListWindow;
            User = user;
            NewTaskGroup = editTaskListWindow.TaskGroupForEdition;

        }
        public ToDoTask SelectedTaskIncreatedlist
        {
            get { return selectedtaskincreatedlist; }
            set
            {
                selectedtaskincreatedlist = value;
                OnPropertyChanged("SelectedTask1");
            }
        }
        public ToDoTask SelectedTask
        {
            get { return selectedtask; }
            set
            {
                selectedtask = value;
                OnPropertyChanged("SelectedTask");
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
